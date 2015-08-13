using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Helpers;
using MangoEngine.Exceptions;
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace MangoEngine.Pages
{
    public class BatotoPage : MangaPage
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public BatotoPage() : base()
        {
            /*Default Constructors*/
            SourceName = "Batoto";
        }

        public BatotoPage(string url) : base(url)
        {
            /*Overloaded Constructors, accepts a string of URL*/
            SourceName = "Batoto";
        }


        #endregion

        #region Methods

        public override async Task<string> GetImageUrlAsync()
        {
            /*Get the image url of the page inside the current url asynchronously*/
            if (!string.IsNullOrEmpty(ImgUrl))
            {
                return ImgUrl;
            }

            //Initialize the client
            HttpClient myClient = new HttpClient();

            //Get the stream to the website. 
            Stream batotoStream = null;

            try
            {
                batotoStream = await MangoWebHelpers.GetStreamAsync(myClient, PageUrl, new TimeSpan(0, 0, 3));
            }

            catch (Exception e)
            {
                throw new MangoException("GetImageUrl failed! Can't get Stream to the Website!", e);
            }

            //Create the parser 
            HtmlParser myParser = new HtmlParser();

            //Parse the HTML
            var document = await myParser.ParseAsync(batotoStream);

            /* Batoto holds the page's image url inside the img node with the id "comic_page" */
            //Get the img node.   
            var imgNode = document.Images.Where(n => n.Id == "comic_page").Select(n => n).First();

            //Get the link
            string imgLink = imgNode.Source;

            //Set the imgUrl for the quick reference later.
            ImgUrl = imgLink;

            //return the result
            return ImgUrl;
        }
        #endregion

    }
}
