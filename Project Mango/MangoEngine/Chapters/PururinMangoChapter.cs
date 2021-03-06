﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using MangoEngine.Exceptions;
using HtmlAgilityPack;

namespace MangoEngine.Chapters
{
    public class PururinMangoChapter : MangoChapter
    {
        #region Fields
        /*Fields*/
        private string _nextPageUrl;
        private string _pururinPrefix;
        private int _currentPage;
        #endregion

        #region Properties
        /*Properties*/
        #endregion

        #region Constructors
        /*Constructors*/
        protected PururinMangoChapter() : base()
        {
            SourceName = "Pururin";
            _nextPageUrl = string.Empty;
            _pururinPrefix = "http://pururin.com";
            _currentPage = 1;
        }

        public PururinMangoChapter(string url) : base(url)
        {
            SourceName = "Pururin";
            _nextPageUrl = string.Empty;
            _pururinPrefix = "http://pururin.com";
            _currentPage = 1;
        }


        #endregion

        #region Methods
        /*Methods*/
        internal override void Init()
        {
            InitAsync().Wait();
        }

        /*NSFW content, use this link to test http://pururin.com/view/13818/00/aikawarazu_1.html */

        internal async override Task InitAsync()
        {
            /*Initialize the current instance asynchronously*/

            //Create the Client to get stuffs from the website
            HttpClient myClient = new HttpClient();

            //Set the timeout of the client to 30 secs
            myClient.Timeout = new TimeSpan(0, 0, 30);

            //url handler here.
            await urlHandlerAsync();

            //Get the response from the website.
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await myClient.GetAsync(CurrentUrl);
            }

            catch (Exception e)
            {
                throw new MangoException("Init failed! Can't Get Response from Website", e);
            }
                

            //Set Encoding    
            EncodingType = GetEncoding(responseMessage);

            //Get a stream to Pururin    
            Stream pururinStream = null;
            try
            {
                pururinStream = await base.GetStreamAsync(myClient, CurrentUrl, new TimeSpan(0, 0, 3));
            }

            catch (Exception e)
            {
                throw new MangoException("Init failed! Can't get Stream to the Website",e);
            }

            //Async-Wrapper for Parsing process    
            await Task.Run(() =>    
            {    
                /*Load the Stream up as HTML*/    
                HtmlDocument pururinDocument = new HtmlDocument();    
                pururinDocument.Load(pururinStream, EncodingType);    

                /*Pururin has the number of pages insid ehte select node with the id=image-pageSelect*/    
                HtmlNode pageSelectNode = pururinDocument.DocumentNode.SelectSingleNode("//select[@class=\"image-pageSelect\"]");

                if (pageSelectNode == null)
                {
                    throw new MangoException("Init Failed! Can't find the pageSelectNode");
                }    

                //The select node has the data-max attribute as the last index of the last page (count from 0);    
                HtmlAttribute datamaxAttribute = pageSelectNode.Attributes["data-max"];    

                //parse the value for the interger    
                int numPages = 0;    

                if (!Int32.TryParse(datamaxAttribute.Value, out numPages))    
                {    
                //if faulted.    
                throw new MangoException("Init failed! Can't convert number of total pages!");    
                }    

                //set the number of pages (plus one due to the index are 0-based)    
                PagesCount = numPages + 1;

                //Get the Manga Title
                HtmlNode headerBreadcrumbsNode = pururinDocument.DocumentNode.SelectSingleNode("//div[@class = \"header-breadcrumbs\"]");
                HtmlNode spanTitleNode = headerBreadcrumbsNode.SelectNodes("span")[3];
                string mangaTitle = spanTitleNode.SelectSingleNode("a").SelectSingleNode("span").InnerText;
                MangaTitle = mangaTitle;
            });

            //done with the client
            myClient.Dispose();

        }
        public override string GetImageUrl()
        {
            return GetImageUrlAsync().Result;
        }

        public override async Task<string> GetImageUrlAsync()
        {
            /*Parse the website and get the img link asynchronously*/

            //create the client to download stuffs from the website
            HttpClient myClient = new HttpClient();

            //Set the timeout of the client to 30 secs
            myClient.Timeout = new TimeSpan(0, 0, 30);

            //Get the Stream to the website.    
            Stream pururinStream = null;
            try
            {
                pururinStream = await base.GetStreamAsync(myClient,CurrentUrl, new TimeSpan (0,0,3));
            }
                 
            catch (Exception e)
            {
                throw new MangoException("GetImageUrl failed! Can't get Stream to the website!", e);
            }

            string PartialimgUrl = await Task.Run<string>(() =>    
            {    
                //Load it up as a HTML document    
                HtmlDocument pururinDocument = new HtmlDocument();    
                pururinDocument.Load(pururinStream);    

                /*Pururin has the img link embedded inside a img node with class ="b",    
                We will get the next link while grabbing the IMG url because they are next to each other.    
                So we don't have to do the GetStream twice.*/    

                HtmlNode imgNode = pururinDocument.DocumentNode.SelectSingleNode("//img[@class=\"b\"]");    
                //Check if the node is valid    
                if (imgNode == null)    
                {    
                    throw new MangoException("GetImageUrl failed! Can't find the img comic_page link!");    
                }    

                HtmlNode ImageNextNode = pururinDocument.DocumentNode.SelectSingleNode("//a[@class=\"image-next\"]");    

                //only update if the link is valid/there    
                if (ImageNextNode != null && !string.IsNullOrEmpty(ImageNextNode.Attributes["href"].Value))    
                {    
                    _nextPageUrl = _pururinPrefix + ImageNextNode.Attributes["href"].Value;    
                }    
                    
                return imgNode.Attributes["src"].Value;    
            });

            //Dispose the client
            myClient.Dispose();

            return _pururinPrefix + PartialimgUrl;    

        }

        public override bool NextPage()
        {
            return NextPageAsync().Result;
        }

        public override async Task<bool> NextPageAsync()
        {
            /*Get the next page link if possible*/

            _currentPage++;

            if (_currentPage > PagesCount)
            {
                return false;
            }

            CurrentUrl = _nextPageUrl;
            return true;
        }

        private async Task urlHandlerAsync()
        {
            //Handle the case where the user might choose the overview pages.

            //check if the current url has the gallery keyword inside.
            if (CurrentUrl.Contains("gallery"))
            {
                //Get the Stream to the website.
                HttpClient myClient = new HttpClient();
                Stream pururinStream = await base.GetStreamAsync(myClient, CurrentUrl, new TimeSpan(0, 0, 3));

                //Async-Wrapper for the parsing portion
                await Task.Run(() =>
                {
                    //Load it up as HTML
                    HtmlDocument pururinDocument = new HtmlDocument();
                    pururinDocument.Load(pururinStream, Encoding.UTF8);

                    //Get the btn link-next node 
                    HtmlNode readOnlineNode = pururinDocument.DocumentNode.SelectSingleNode("//a[@class=\"btn link-next\"]");

                    //Get the value from href attribute
                    string partialLink = readOnlineNode.Attributes["href"].Value;

                    //set it to the CurrentUrl
                    CurrentUrl = _pururinPrefix + partialLink;

                });


            }

        }

        #endregion
    }
}
