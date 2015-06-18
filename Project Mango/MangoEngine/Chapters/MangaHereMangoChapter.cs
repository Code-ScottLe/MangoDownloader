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
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }

        public MangaHereMangoChapter(string url) : base(url)
        {
            /*Overloaded Constructor, accept a string of URL for the MangaHere source.*/
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

            try
            {
                //Try to get the Response from the link
                HttpResponseMessage sourceResponseMessage = await myClient.GetAsync(CurrentUrl);

                /*Get the Data Encoding of the content*/
                EncodingType = GetEncoding(sourceResponseMessage);

                //Get the Stream to the website
                Stream sourceStream = await myClient.GetStreamAsync(CurrentUrl);

                //Async-wrapper for parsing portion
                await Task.Run(() =>
                {
                    //Check if the stream is Compressed. MangaHere uses GZip
                    if (GetCompression(sourceResponseMessage) == "gzip")
                    {
                        sourceStream = new GZipStream(sourceStream, CompressionMode.Decompress);
                    }

                    /*Load the stream up as HTML*/
                    HtmlDocument myDocument = new HtmlDocument();
                    myDocument.Load(sourceStream, EncodingType);

                    /*MangaHere has the list of all the pages with links in a drop down*/
                    //Get the select node which contains all the pages with links
                    HtmlNode selectNode = myDocument.DocumentNode.SelectSingleNode("//select[@class = \"wid60\"]");

                    //Add all the links onto the list of pages link.
                    foreach (HtmlNode optionNode in selectNode.SelectNodes("option"))
                    {
                        _pagesLinks.Add(optionNode.Attributes["value"].Value);
                    }

                    //Set the number of pages
                    PagesCount = _pagesLinks.Count;
                });


                
            }
            catch (Exception e)
            {
                //Something is wrong, re-throw the exception
                throw new MangoException("Initialize Failed!", e);
            }

            finally
            {
                //Done with everything, dispose the client to save memory.
                myClient.Dispose();
            }

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

            try
            {
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
               
                //return the url
                return imgUrl;

            }

            catch (Exception e)
            {
                throw new MangoException("Get Image URL Failed!", e);
            }

            finally
            {
                //Done with everything, clean up the client
                myClient.Dispose();
            }
        }

        #endregion
    }
}
