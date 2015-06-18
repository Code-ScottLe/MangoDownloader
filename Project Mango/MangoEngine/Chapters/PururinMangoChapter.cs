using System;
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

        internal async override Task InitAsync()
        {
            /*Initialize the current instance asynchronously*/

            //Create the Client to get stuffs from the website
            HttpClient myClient = new HttpClient();

            //Set the timeout of the client to 30 secs
            myClient.Timeout = new TimeSpan(0, 0, 30);

            //url handler here.
            try
            {
                //Get the response from the website.
                HttpResponseMessage responseMessage = await myClient.GetAsync(CurrentUrl);

                //Set Encoding
                EncodingType = GetEncoding(responseMessage);

                //Get a stream to Pururin
                Stream pururinStream = await myClient.GetStreamAsync(CurrentUrl);

                //Async-Wrapper for Parsing process
                await Task.Run(() =>
                {
                    /*Load the Stream up as HTML*/
                    HtmlDocument pururinDocument = new HtmlDocument();
                    pururinDocument.Load(pururinStream, EncodingType);

                    /*Pururin has the number of pages insid ehte select node with the id=image-pageSelect*/
                    HtmlNode pageSlectNode = pururinDocument.DocumentNode.SelectSingleNode("//select[@class=\"image-pageSelect\"]");

                    //The select node has the data-max attribute as the last index of the last page (count from 0);
                    HtmlAttribute datamaxAttribute = pageSlectNode.Attributes["data-max"];

                    //parse the value for the interger
                    int numPages = 0;

                    if (!Int32.TryParse(datamaxAttribute.Value, out numPages))
                    {
                    //if faulted.
                    throw new MangoException("can't convert number of total pages!");

                    }

                    //set the number of pages
                    PagesCount = numPages;

                });

            }

            catch (Exception e)
            {
                throw new MangoException("Intialize Failed!", e);
            }

            finally
            {
                //Done with everything, dispose the client
                myClient.Dispose();
            }

        }
        public override string GetImageUrl()
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetImageUrlAsync()
        {
            throw new NotImplementedException();
        }

        public override bool NextPage()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> NextPageAsync()
        {
            throw new NotImplementedException();
        }

        private void url_handler()
        {
            //Handle the case where the user might choose something elser rather than the chapter page.           
        }

        #endregion
    }
}
