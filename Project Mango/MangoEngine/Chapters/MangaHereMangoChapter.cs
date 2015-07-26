using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using HtmlAgilityPack;
using MangoEngine.Exceptions;

namespace MangoEngine.Chapters
{
    public class MangaHereMangoChapter : MangoChapter
    {
        #region Fields
        /*Fields*/
        private List<string> _pagesLinks;
        private int _currentPageIndex;

        #endregion

        #region Properties

        /*Properties*/

        #endregion

        #region Constructors

        /*Constructors*/

        protected MangaHereMangoChapter() : base()
        {
            /*Default Constructor*/
            SourceName = "MangaHere";
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }

        public MangaHereMangoChapter(string url) : base(url)
        {
            /*Overloaded Constructor, accept a string of URL for the MangaHere source.*/
            SourceName = "MangaHere";
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }
        #endregion

        #region Methods

        /*Methods*/
        internal override void Init()
        {
            /*Initialize the current instance of the MangaHereMangoChapter synchronously*/
            InitAsync().Wait();
        }

        internal override async Task InitAsync()
        {
            /*Initialize the current instance of the MangaHereMangoChapter asynchronously*/

            //Create an instance of the HttpClient to get the data.
            HttpClient myClient = new HttpClient();

            //set the Timeout of the client to 30 secs
            myClient.Timeout = new TimeSpan(0,0,30);

            //Try to get the Response from the link   
            HttpResponseMessage sourceResponseMessage = null;
            try
            {
                sourceResponseMessage = await myClient.GetAsync(CurrentUrl);
            }
               
            catch(Exception e)
            {
                throw new MangoException("Init failed! Can't get the response from the Website", e);
            }

            /*Get the Data Encoding of the content*/    
            EncodingType = GetEncoding(sourceResponseMessage);

            //Get the Stream to the website    
            Stream sourceStream = null;
            try
            {
                sourceStream = await myClient.GetStreamAsync(CurrentUrl);
            }
                 
            catch(Exception e)
            {
                throw new MangoException("Init failed! Can't get the Stream to the Website!", e);
            }
            //Async-wrapper for parsing portion    
            await Task.Run(() =>    
            {    
                //Check if the stream is Compressed. MangaHere uses GZip    
                if (GetCompression(sourceResponseMessage) == "gzip")    
                {    
                    sourceStream = new GZipStream(sourceStream, CompressionMode.Decompress);    
                }    

                /*Load the stream up as HTML*/    
                HtmlDocument mangaHereDocument = new HtmlDocument();    
                mangaHereDocument.Load(sourceStream, EncodingType);    

                /*MangaHere has the list of all the pages with links in a drop down*/    
                //Get the select node which contains all the pages with links    
                HtmlNode selectNode = mangaHereDocument.DocumentNode.SelectSingleNode("//select[@class = \"wid60\"]");    

                if(selectNode == null)
                {
                    throw new MangoException("Init failed! Can't find selectNode!");
                }

                //Add all the links onto the list of pages link.    
                foreach (HtmlNode optionNode in selectNode.SelectNodes("option"))    
                {    
                    _pagesLinks.Add(optionNode.Attributes["value"].Value);    
                }    

                //Set the number of pages    
                PagesCount = _pagesLinks.Count;

                //Get the div title node
                HtmlNode titleDivNode = mangaHereDocument.DocumentNode.SelectSingleNode("//section[@class = \"readpage_top\"]/div[@class = \"title\"]");

                //Get the h2 node
                HtmlNode h2Node = titleDivNode.SelectSingleNode("h2");

                //Get the Manga title
                string title = MangaTitleHandler(h2Node.InnerText);
                MangaTitle = title;

            });    

            //Dispose the client
            myClient.Dispose();
               
        }

        public override bool NextPage()
        {
            /*Get to the next page of the current chapter synchrounously*/
            return NextPageAsync().Result;
        }

        public override async Task<bool> NextPageAsync()
        {
            /*Get to the next page of the current chapter asynchrounously*/

            //Increment the current page index    
            _currentPageIndex++;    

            //Verify that we haven't reached the end of the chapter    
            if (_currentPageIndex >= PagesCount)    
            {    
                return false;    
            }    

            CurrentUrl = _pagesLinks[_currentPageIndex];    
            return true;    
        }

        public override string GetImageUrl()
        {
            return GetImageUrlAsync().Result;
        }

        public override async Task<string> GetImageUrlAsync()
        {
            /*Get the image url of the page inside the current url asynchronously*/

            //Intialize the client.
            HttpClient myClient = new HttpClient();

            //Get the Response to the website.    
            HttpResponseMessage mangaHereResponseMessage = await myClient.GetAsync(CurrentUrl);    

            /*Check if the Stream is compressed or not*/    
            Stream mangaHereStream;    

            //MangaHere uses GZip, but switch off randomly    
            if (GetCompression(mangaHereResponseMessage) == "gzip")    
            {    
                mangaHereStream = new GZipStream(await myClient.GetStreamAsync(CurrentUrl),CompressionMode.Decompress);    
            }    

            else    
            {    
                mangaHereStream = await myClient.GetStreamAsync(CurrentUrl);    
            }    

            //Async-Wrapper for parsing    
            string imgUrl = await Task.Run<string>(() =>    
            {    
                /*Load up the Stream as HTML*/    
                HtmlDocument mangaHereHtmlDocument = new HtmlDocument();    
                mangaHereHtmlDocument.Load(mangaHereStream, EncodingType);    

                /* MangaHere holds the page's image url inside the img node with the id "image" */    
                //Get the img node.    
                HtmlNode imgNode = mangaHereHtmlDocument.DocumentNode.SelectSingleNode("//img[@id = \"image\"]");    

                //Check if the node is valid    
                if (imgNode == null)    
                {    
                    throw new MangoException("Can't find the img comic_page link!");    
                }    

                //Node was found, get the link out.    
                return imgNode.Attributes["src"].Value;    
            });

            //Dispose the client
            myClient.Dispose();
               
            //return the url
            return imgUrl;
         
        }

        private string MangaTitleHandler(string title)
        {
            int indexed = title.LastIndexOf("Manga");

            string cut = title.Substring(0, indexed - 1);

            return cut;
        }

        #endregion
    }
}
