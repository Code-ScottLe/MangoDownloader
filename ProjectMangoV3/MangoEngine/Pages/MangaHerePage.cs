using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using MangoEngine.Helpers;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using MangoEngine.Exceptions;

namespace MangoEngine.Pages
{
    public class MangaHerePage : MangaPage
    {
        #region Fields
        /*Fields*/
        #endregion

        #region Properties
        /*Properties*/
        #endregion

        #region Constructors
        /*Constructors*/
        public MangaHerePage() : base()
        {
            //default constructor
            SourceName = "MangaHere";
        }

        public MangaHerePage(string url) : base(url)
        {
            //Overloaded constructor, accept a string of url
            SourceName = "MangaHere";
        }
        #endregion

        #region Methods
        /*Methods*/
        public override async Task<string> GetImageUrlAsync()
        {
            //Get the image URL from the current URL. 

            //Create the HttpClient
            HttpClient myClient = new HttpClient();

            //Try to get the Stream to the Website.
            //MangaHere is known for their compressed skills.
            KeyValuePair<Stream, string> temp = await MangoWebHelpers.GetCompressedStreamAsync(myClient, PageUrl, new TimeSpan(0, 0, 5));

            Stream mangaHereStream = temp.Key;

            //Check if the stream is compressed or not.
            if (temp.Value == "GZip")
            {
                //stream is compressed.
                mangaHereStream = new GZipStream(mangaHereStream, CompressionMode.Decompress);
            }

            //Create parser
            HtmlParser myParser = new HtmlParser();

            //Parse the document
            IHtmlDocument mangaHereDocument = await myParser.ParseAsync(mangaHereStream);

            //Get the image node which has the id == "image";
            var imgNode = mangaHereDocument.Images.Where(n => n.Id == "image").Select(n => n).First();


            //check if we actually get it.
            if(imgNode == null)
            {
                throw new MangoException("Can't get image node that containt img link");
            }

            //set the ImgUrl
            ImgUrl = imgNode.Source;

            //return.
            return ImgUrl;
        }
        #endregion
    }
}
