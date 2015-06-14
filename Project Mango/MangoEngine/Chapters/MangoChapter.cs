using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using MangoEngine.Factories;

namespace MangoEngine.Chapters
{
    public abstract class MangoChapter
    {
        /*Represent a chapter of a manga*/

        #region Fields
        /*Fields*/
        private string _sourceName;
        private string _currentUrl;
        private string _baseUrl;
        private int _pagesCount;
        private Encoding _encodingType;

        #endregion

        #region Properties

        /*Properties*/

        public string SourceName
        {
            get { return _sourceName; }
            protected set { _sourceName = value; }
        }

        public string CurrentUrl
        {
            get { return _currentUrl; }
            protected set { _currentUrl = value; }
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            protected set { _baseUrl = value; }
        }

        public int PagesCount
        {
            get { return _pagesCount; }
            protected set { _pagesCount = value; }
        }

        public Encoding EncodingType
        {
            get { return _encodingType; }
            set { _encodingType = value; }
        }

        public static MangoChapterFactory Factory
        {
            get
            {
                return new MangoChapterFactory();
            }
        }

        #endregion

        #region Constructors

        /*Constructors*/

        protected MangoChapter()
        {
            /*Default Constructor. Accepts no parameters*/
            _sourceName = string.Empty;
            _currentUrl = string.Empty;
            _baseUrl = string.Empty;
            _pagesCount = 0;
            _encodingType = Encoding.UTF8;
        }
        #endregion

        protected MangoChapter(string url) : this()
        {
            /*Overloaded Constructor. Accept a string of URL for a Mango Chapter.*/
            _baseUrl = url;
            _currentUrl = url;
        }

        #region Methods

        /*Methods*/
        internal abstract void Init();

        internal abstract Task InitAsync();

        public abstract bool NextPage();

        public abstract Task<bool> NextPageAsync();

        public abstract string GetImageUrl();

        public abstract Task<string> GetImageUrlAsync();

        public virtual Encoding GetEncoding(HttpResponseMessage responseMessage)
        {
            /*Get the Encoding of the WebSite from the HttpResponseMessage*/

            //Get the Content Headers
            var responseContentHeader = responseMessage.Content.Headers;

            //Get the Content Type Header
            string contentTypeHeader = responseContentHeader.ContentType.ToString();

            //get the encoding type
            string contentEncodingStr = contentTypeHeader.Substring(contentTypeHeader.IndexOf("=") + 1);
            Encoding contentEncoding = Encoding.GetEncoding(contentEncodingStr);

            if (contentEncoding == null)
            {
                //Default to UTF-8 if hells break loose.
                contentEncoding = Encoding.UTF8;
            }

            //Return the encoding type
            return contentEncoding;

        }

        public virtual string GetCompression(HttpResponseMessage responseMessage)
        {
            /*GEt the Compression type of the Website fro the HttpResponseMessage*/
            if (responseMessage.Content.Headers.ContentEncoding.Count != 0)
            {
                return responseMessage.Content.Headers.ContentEncoding.ElementAt(0);
            }

            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
