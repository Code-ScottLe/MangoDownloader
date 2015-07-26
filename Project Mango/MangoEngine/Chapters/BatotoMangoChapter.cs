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
            SourceName = "Batoto";
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }

        public BatotoMangoChapter(string url) : base(url)
        {
            /*Overloaded Constructors, accept a string of batoto url*/
            SourceName = "Batoto";
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

            //Get the response from the website.   
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await myClient.GetAsync(CurrentUrl);
            }
                
            catch (Exception e)
            {
                throw new MangoException("Init failed! Can't get the response from the website!", e);
            }

            //response received. Get the Encoding.    
            EncodingType = GetEncoding(responseMessage);

            //Get the stream to the website.    
            Stream batotoStream = null;
            try
            {
                batotoStream = await myClient.GetStreamAsync(CurrentUrl);
            }
                 
            catch (Exception e)
            {
                throw new MangoException("Init failed! Can't get the stream from the website", e);
            }

            //Async-Wrapper for parsing    
            await Task.Run(() =>    
            {//Load up the Stream as HTML file.    
                HtmlDocument batotoHtmlDocument = new HtmlDocument();    
                batotoHtmlDocument.Load(batotoStream, EncodingType);    

                /*Batoto has the list of all the pages with links in a drop down*/    
                //Get the select node which contains all the pages with links    
                HtmlNode selectNode = batotoHtmlDocument.DocumentNode.SelectSingleNode("//select[@id = \"page_select\"]");

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

                //Get the Ul Code that contains everthing in the frame
                HtmlNode UlNode = selectNode.ParentNode.ParentNode;

                //Get the list of li nodes inside the Ul Node.
                HtmlNodeCollection liNodesCollection = UlNode.SelectNodes("li");

                //Get the Title of the Manga inside the inner html of the href node inside the 1st li node
                string mangaTitle = liNodesCollection[0].ChildNodes[0].InnerText;
                MangaTitle = mangaTitle;

                //Get the Title of the Chapter inside select box of the 2nd li node.
                HtmlNode chapterSelectNode = liNodesCollection[1].SelectSingleNode("select");
                HtmlNode selectedChapterNode = chapterSelectNode.SelectSingleNode("option[@selected=\"selected\"]");
                string chapterTitle = selectedChapterNode.NextSibling.InnerText;
                ChapterTitle = chapterTitle;
                    
            });    

            //Dispose the client
            myClient.Dispose();
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

            //Get the Stream to the website.    
            Stream batotoStream = null;
            try
            {
                batotoStream = await myClient.GetStreamAsync(CurrentUrl);
            }

            catch (Exception e)
            {
                throw new MangoException("GetImageUrl failed! Can't get Stream to the Website!", e);
            }

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
                    throw new MangoException("GetImageUrl failed! Can't find the img comic_page link!");    
                }    

                //Node was found, get the link out.    
                return imgNode.Attributes["src"].Value;    
            });

            //Dispose the client
            myClient.Dispose();

            //return the url    
            return imgUrl;    

        }

        #endregion
    }
}
