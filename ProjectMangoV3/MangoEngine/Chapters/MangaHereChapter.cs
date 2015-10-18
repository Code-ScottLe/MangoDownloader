﻿using System;
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

            await Task.Run(() =>
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
            });


           

        }
        #endregion



    }
}
