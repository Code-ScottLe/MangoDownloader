using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System.IO;
using System.Net.Http;
using MangoEngine.Helpers;
using MangoEngine.Exceptions;
using MangoEngine.Pages;

namespace MangoEngine.Chapters
{
    public class BatotoChapter : MangaChapter
    {
        #region Fields;
        /*Fields*/
        #endregion

        #region Property
        /*Property*/
        #endregion

        #region Constructors
        /*Constructors*/
        #endregion

        #region Methods
        /*Methods*/
        #endregion
        public override async Task InitAsync()
        {
            /*Initialize the current instance of MangaChapter to the Batoto standard.*/

            //Initialize the client for Web purpose
            HttpClient myClient = new HttpClient();

            //set the timeout of the client (30 secs)
            myClient.Timeout = new TimeSpan(0, 0, 30);

            //Try to get the stream to the website.
            Stream batotoStream = null;

            try
            {
                batotoStream = await MangoWebHelpers.GetStreamAsync(myClient, BaseUrl, new TimeSpan(0, 0, 3));
            }

            catch (Exception e)
            {
                throw new MangoException("Init failed! Can't get the stream from the website", e);
            }

            //Parsing part.

            //Initialize the Parser
            HtmlParser parser = new HtmlParser();

            //Parse the given stream of html
            var batotoDocument = await parser.ParseAsync(batotoStream);

            /*Batoto has the list of all the pages with links in a drop down*/
            //Get the select node which contains all the pages with links
            var selectNode = batotoDocument.All.Where(n => n.Id == "page_select").Select(n => n).First();   
            
            //Iterate through the option nodes and get the links for each pages.
            for(int i = 0; i < selectNode.Children.Count(); i++)
            {
                //Create an instance of a BatotoPage
                BatotoPage myPage = new BatotoPage();

                //set the link
                myPage.PageUrl = selectNode.Children[i].Attributes["value"].Value;

                //set the page number
                myPage.PageIndex = i + 1;

                //Add to the list of pages.
                Pages.Add(myPage);
            }
            
            //Get the Ul Node that contains everthin in the frame
            var UlNode = selectNode.ParentElement.ParentElement;

            //Get the tilte of the manga inside the inner html of the 1st li node
            string title = UlNode.Children[0].TextContent;
            MangaTitle = title;

            //Get the title of the chapter inside the select box of the 2nd li node.
            var chapterSelectNode = UlNode.Children[1].Children[0];           
            var selectedchapterNode = (from n in chapterSelectNode.Children
                               where (n as IHtmlOptionElement).IsSelected == true
                               select n).First();
            string chapterTitle = selectedchapterNode.TextContent;
            ChapterTitle = chapterTitle;
        }
    }
}
