using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MangoEngine.Exceptions;

namespace MangoEngine.Chapters
{
    public class BatotoMangoChapter : MangoChapter
    {
        /*Represent a manga chapter from Batoto*/

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

        protected BatotoMangoChapter() : base()
        {
            /*Default Constructor*/
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }

        public BatotoMangoChapter(string url) : base(url)
        {
            /*Overloaded Constructors, accept a string of batoto url*/
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }
        #endregion

        #region Methods
        /*Methods*/

        internal override void Init()
        {
            /*Intialize current instance of BatotoMangoChapter synchronously*/
            InitAsync().Wait();
        }

        internal override async Task InitAsync()
        {
            /*Intialize current instance of BatotoMangoChapter asynchronously*/

            //initialize the Httpclient
            HttpClient myClient = new HttpClient();

            //Set the timeout of the client (30 secs)
            myClient.Timeout = new TimeSpan(0,0,0,30);

            try
            {
                //Get the response from the website.
                HttpResponseMessage responseMessage = await myClient.GetAsync(CurrentUrl);

                //response received. Get the Encoding.
                EncodingType = GetEncoding(responseMessage);

                //Get the stream to the website.
                Stream batotoStream = await myClient.GetStreamAsync(CurrentUrl);

                await Task.Run(() =>
                {//Load up the Stream as HTML file.
                    HtmlDocument batotoHtmlDocument = new HtmlDocument();
                    batotoHtmlDocument.Load(batotoStream, EncodingType);

                    /*Batoto has the list of all the pages with links in a drop down*/
                    //Get the select node which contains all the pages with links
                    HtmlNode selectNode = batotoHtmlDocument.DocumentNode.SelectSingleNode("//select[@id = \"page_select\"]");

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
                throw new MangoException("Initialize Failed!", e);
            }

            finally
            {
                //Dispose the client when done.
                myClient.Dispose();
            }
        }

        public override bool NextPage()
        {
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
            /*Get the image url of the page inside the current url synchronously*/
            return GetImageUrlAsync().Result;
        }

        public override async Task<string> GetImageUrlAsync()
        {
            /*Get the image url of the page inside the current url asynchronously*/
            
            //Intialize the client.
            HttpClient myClient = new HttpClient();

            try
            {
                //Get the Stream to the website.
                Stream batotoStream = await myClient.GetStreamAsync(CurrentUrl);

                //get the img url.
                string imgUrl = await Task.Run<string>(() =>
                {
                    //Load up the Stream as the Html
                    HtmlDocument batotoHtmlDocument = new HtmlDocument();
                    batotoHtmlDocument.Load(batotoStream, EncodingType);

                    /* Batoto holds the page's image url inside the img node with the id "comic_page" */
                    //Get the img node.
                    HtmlNode imgNode = batotoHtmlDocument.DocumentNode.SelectSingleNode("//img[@id = \"comic_page\"]");

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
