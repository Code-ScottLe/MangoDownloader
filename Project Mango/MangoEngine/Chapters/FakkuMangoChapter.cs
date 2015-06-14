using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using HtmlAgilityPack;
using MangoEngine.Exceptions;

namespace MangoEngine.Chapters
{
    public class FakkuMangoChapter : MangoChapter
    {
        #region Fields
        /*Fields*/
        private string _imgSource;
        private int _currentPageIndex;
        #endregion

        #region Properties
        /*Properties*/
        #endregion

        #region Constructors
        /*Constructors*/
        #endregion

        #region Methods
        /*Methods*/
        internal override void Init()
        {
            InitAsync().Wait();
        }

        internal override async Task InitAsync()
        {
            /*Initialzie the current instance of the FakkuMango_Source (async)*/
            
            //Create an instance of HttpClient for downloading.
            HttpClient myClient = new HttpClient();

            //Set the timeout of the client to be 30sec
            myClient.Timeout = new TimeSpan(0,0,30);

            //Fix the URl if needed to
            url_handler();

            try
            {
                //Get the response from the website
                HttpResponseMessage fakkuResponseMessage = await myClient.GetAsync(CurrentUrl);

                //Get the Encoding
                EncodingType = GetEncoding(fakkuResponseMessage);

                //Get a Stream to Fakku
                Stream fakkuStream = await myClient.GetStreamAsync(CurrentUrl);

                /*Load the Stream up as HTML*/
                HtmlDocument fakkuHtmlDocument = new HtmlDocument();
                fakkuHtmlDocument.Load(fakkuStream,EncodingType);

                /*Fakku contains the number of pages in a columns*/
                //Attempt to find the <div> node which contain 9 columns of info
                HtmlNode div9ColumnscontentNode =
                    fakkuHtmlDocument.DocumentNode.SelectSingleNode("//div[@class=\"nine columns content-right \"]");

                //Attemp to find  the div node that hold the page numbers
                HtmlNode divPagesInfoNode =
                    div9ColumnscontentNode.SelectSingleNode("//div[@class=\"left\" and text()= \"Pages\"]").ParentNode;

                //Get the node that contain the numbers of pages
                HtmlNode divPagesNode = divPagesInfoNode.SelectSingleNode("div[@class=\"right\"]");

                /*The div node will be in this format: <div class = "right"> 22 pages </div>
                 * We will have to clean the string before converting it to number*/

                //WARNING: This will break if the innter text is mistyped
                string pageNumberString = divPagesNode.InnerText.Substring(0, divPagesNode.InnerText.IndexOf(" "));
                int numPages;

                if (!Int32.TryParse(pageNumberString, out numPages))
                {
                    throw new MangoException("Can't get the number of pages");
                }

                //Set the number of pages
                PagesCount = numPages;

                //Done getting the number of pages, set the URl to the reading pages.
                CurrentUrl += "/read#page=1";

                //Search for the Meta Node that contain the property = "og:image"
                HtmlNode metaNode = fakkuHtmlDocument.DocumentNode.SelectSingleNode("//meta[@property = \"og:image\"]");

                //Get the image value out
                string imgLink = metaNode.Attributes["content"].Value;

                //set the imgLink as the current imgSource
                _imgSource = imgLink;

                //Set the page index
                _currentPageIndex = 1;


            }
            catch (Exception e)
            {
                throw new MangoException("Initialize Failed!", e);
            }
            finally
            {
                //Done with the client, dispose it.
                myClient.Dispose();
            }

        }

        public override bool NextPage()
        {
            return NextPageAsync().Result;
        }

        public override async Task<bool> NextPageAsync()
        {
            /*Modify the links to jump to next page*/
            return await Task.Run<bool>(() =>
            {
                _currentPageIndex ++;

                if (_currentPageIndex > PagesCount)
                {
                    return false;
                }

                string baseString = CurrentUrl.Substring(0, CurrentUrl.LastIndexOf('=') + 1);
                CurrentUrl = baseString += _currentPageIndex.ToString();

                return true;
            });
        }

        public override string GetImageUrl()
        {
            return GetImageUrlAsync().Result;
        }

        public override async Task<string> GetImageUrlAsync()
        {
            /*Get the imageURL from the page for downloading*/

            return await Task.Run<string>(() =>
            {
                try
                {
                    string pageNum = _currentPageIndex.ToString();

                    if (pageNum.Length == 2)
                    {
                        pageNum = "0" + pageNum;
                    }

                    else if (pageNum.Length == 1)
                    {
                        pageNum = "00" + pageNum;
                    }

                    //Get the image out
                    string imgLink = _imgSource + pageNum + ".jpg"; //Fakku only>?

                    return imgLink;
                }
                catch (Exception e)
                {
                    throw new MangoException("Failed to get to next page", e);
                }
            });
        }
        private void url_handler()
        {
            //Handle the case where the user might choose something elser rather than the chapter page.           

            if (CurrentUrl.Contains("/read#page") || CurrentUrl.Contains("/read"))
            {
                //Not the last in the URL string.
                int last_slash = CurrentUrl.LastIndexOf('/');
                string sub_url = CurrentUrl.Substring(0, last_slash);
                CurrentUrl = sub_url;
            }
        }
        #endregion
    }
}
