using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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

            }

            catch (Exception)
            {
                throw;
            }

            finally
            {

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
