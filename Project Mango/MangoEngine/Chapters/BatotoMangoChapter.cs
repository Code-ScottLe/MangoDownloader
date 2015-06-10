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
        private List<string> _pagesLink;
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
            _pagesLink = new List<string>();
            _currentPageIndex = 0;
        }

        public BatotoMangoChapter(string url) : base(url)
        {
            /*Overloaded Constructors, accept a string of batoto url*/
            _pagesLink = new List<string>();
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

                //Load up the Stream as HTML file.
                HtmlDocument batotoHtmlDocument = new HtmlDocument();
                batotoHtmlDocument.Load(batotoStream);

                /*Batoto has the list of all the pages with links in a drop down*/
                //Get the select node which contains all the pages with links
                HtmlNode selectNode = batotoHtmlDocument.DocumentNode.SelectSingleNode("//select[@id = \"page_select\"]");

                //Add all the links onto the list of pages link.
                foreach (HtmlNode optionNode in selectNode.SelectNodes("option"))
                {
                    _pagesLink.Add(optionNode.Attributes["value"].Value);
                }

                //Set the number of pages
                PagesCount = _pagesLink.Count;
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
            throw new NotImplementedException();
        }

        public override async Task<bool> NextPageAsync()
        {
            throw new NotImplementedException();
        }

        public override string GetImageUrl()
        {
            throw new NotImplementedException();
        }

        public override async Task<string> GetImageUrlAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
