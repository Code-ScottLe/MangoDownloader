using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Helpers;
using MangoEngine.Exceptions;
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using MangoEngine.Pages;

namespace MangoEngine.Chapters
{
    public class MangaHereChapter : MangaChapter
    { 
        #region Fields
        /*Fields*/
        #endregion

        #region Properties
        /*Properties*/
        #endregion

        #region Constructors
        /*Constructors*/
        public MangaHereChapter() : base()
        {
            //default constructor, call base
            SourceName = "MangaHere";
        }

        public MangaHereChapter(string url) : base(url)
        {
            //Overloaded, call base and set name
            SourceName = "MangaHere";
        }

        #endregion

        #region Methods
        /*Methods*/
        public override async Task InitAsync()
        {
            /*Initialize the current instance of MangaChapter to the MangaHere standard.*/

            //Initialize the client for Web purpose
            HttpClient myClient = new HttpClient();

            //set the timeout of the client (30 secs)
            myClient.Timeout = new TimeSpan(0, 0, 30);

            //Try to get the stream to the website.
            Stream mangaHereStream = null;
            string compressionType = string.Empty;

            try
            {
                KeyValuePair<Stream, string> temp = await MangoWebHelpers.GetCompressedStreamAsync(myClient, BaseUrl, new TimeSpan(0, 0, 3));
                mangaHereStream = temp.Key;
                compressionType = temp.Value;
            }

            catch (Exception e)
            {
                throw new MangoException("Init failed! Can't get the stream from the website", e);
            }

            /*Parsing part*/          

            /*Initialize the parser*/
            HtmlParser parser = new HtmlParser();

            //Check if the stream is compressed, MangaHere uses random GZip at times.
            if (compressionType == "GZip")
            {
                //stream is compressed, decompress stream.
                mangaHereStream = new GZipStream(mangaHereStream,CompressionMode.Decompress);
            }

            //Parse the document
            var mangaHereDocument = await parser.ParseAsync(mangaHereStream);

            //Async Wrapper

            await Task.Run( () =>
            {
                /*MangaHere has the list of all the pages with links in a drop down*/
                //Get the select node wich contains all the pages with links
                var selectNode = mangaHereDocument.All.Where(n => n.ClassName == "wid60" && (n as IHtmlSelectElement) != null).Select(n => n).First();

                //Iterate through the entire collection and grab all the links.
                for (int i = 0; i < selectNode.Children.Count(); i++)
                {
                    //Get the url.
                    string url = selectNode.Children[i].Attributes["value"].Value;

                    //Create a MangaHere page from it.
                    MangaHerePage myPage = new MangaHerePage(url);
                    myPage.PageIndex = i + 1;

                    //Put that onto the list.
                    Pages.Add(myPage);
                }

                //Done adding, try to get the Chapter Title and Manga Title?

                //Get the Div node that containt the name.
                var divNode = mangaHereDocument.All.Where(n => n.ClassName == "title" && n.NodeName == "DIV").Select(n => n).First();

                //Grap the Title
                //MangaTitle = divNode.InnerHtml.Substring(0, divNode.InnerHtml.IndexOf("Manga"));

                //Get the script node that has all the define statments.
                var defineScriptNode = mangaHereDocument.Scripts.Where(n => n.InnerHtml.Contains("series_name")).Select(n => n).First();

                //Get the script node that contain the link to the js script that contain the array of chapters with links
                var rawArrayScriptNode = mangaHereDocument.Scripts.Where(n => n.InnerHtml.Contains("get_chapters")).Select(n => n).First();

                //Get the list of Chapter Title with links
                var ChapterTitle = GetCurrentChapterTitleAsync(myClient,defineScriptNode, rawArrayScriptNode).Result; //Force to block.

                //Get the current chapter.

            });


           

        }


        private async Task<string> GetCurrentChapterTitleAsync(HttpClient myClient, IHtmlScriptElement defineScript, IHtmlScriptElement ArrayQueueScript)
        {
            /*Get the list of available chapters + descriptions with its urls.
            Values return are List of KeyValuePair<ChapterDescription,Url>*/

            //Get the define scripts
            string jsDefineScript = defineScript.InnerHtml;

            //Parse the ArrayQueueScript for the link to the javascript array.
            //Get the first statement call of the script node, which is something like this:
            // $LAB.queueScript('http://www.mangahere.co/get_chapters8471.js?v=20151012145213');
            //                             We are interested in this link right here.  

            string rawArrayQueueScriptString = ArrayQueueScript.InnerHtml.Substring(0, ArrayQueueScript.InnerHtml.IndexOf(';'));

            //split the string based on the ' char.
            string[] tempStringArray = rawArrayQueueScriptString.Split('\'');

            //the url should be on the index 1.
            string jsArrayUrl = tempStringArray[1];

            //Get the Script file from the URL
            string jsArrayRawScript = await myClient.GetStringAsync(jsArrayUrl);

            //Get the first function from the jsArrayRawScript (where the array get defined)
            string jsArrayScript = jsArrayRawScript.Substring(0, jsArrayRawScript.IndexOf(";"));

            //Done getting array scripts

            //Async-Wrapper
            return await Task.Run<string>(() =>
           {
               //create Jint Engine
               Jint.Engine myEngine = new Jint.Engine();

               //Execute the define script
               myEngine.Execute(jsDefineScript);

               //Execute the array define script.
               myEngine.Execute(jsArrayScript);

               //Get the Array in the chapter_list value
               IEnumerable<object> rawChapterList = myEngine.GetValue("chapter_list").ToObject() as IEnumerable<object>;

               //Safety check
               if (rawChapterList == null)
               {
                   //something is wrong
                   throw new MangoException("Can't get rawChapterList! Can't cast the return value from Jint Engine to IENumerable<Object>");
               }

               //get the current chapter code
               string current_chapter = myEngine.GetValue("current_chapter").ToString();        //Please verify.

               var ChapterTitleWithLinks = rawChapterList.Where(n =>
               {
                   //cast the object as the Iterable objects.
                   var tempArray = n as IEnumerable<object>;

                   //the Iterable object consist of 2 object, 0 is the chapter title and 1 is the according links to the chapter.
                   //get the URL.
                   string url = tempArray.ElementAt(1) as string;
                   //make sure to get only the current chapter.
                   return (!string.IsNullOrEmpty(url)) && url.Contains(current_chapter);
               }).Select(n => n).First();

               string chapterTitle = (ChapterTitleWithLinks as IEnumerable<object>).ElementAt(0) as string;

               /*
               //Create the list to return.
               List<KeyValuePair<string, string>> ChaptersList = new List<KeyValuePair<string, string>>();

               //Iterate through the list of Chapters.
               foreach (object chapterUrlPair in rawChapterList)
               {
                   //cast the value as an Array to access.
                   var tempArray = chapterUrlPair as IEnumerable<object>;

                   //get the chapter title
                   string chapterTitlle = tempArray.ElementAt(0) as string;

                   //Get the url
                   string chapterUrl = tempArray.ElementAt(1) as string;

                   //Put onto the list
                   ChaptersList.Add(new KeyValuePair<string, string>(chapterTitlle, chapterUrl));
               }

               return ChaptersList;
               */

               return chapterTitle;

           });
        }
        #endregion



    }
}
