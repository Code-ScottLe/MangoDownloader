using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine
{
    public abstract class MangoChapter
    {
        /*Represent a chapter of a manga*/

        #region Fields
        /*Fields*/
        protected string _sourceName;
        protected string _currentUrl;
        protected string _baseUrl;
        protected int _pagesCount;
        protected Encoding _encodingType;

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

        public abstract string GetImageUrlAsync();

        #endregion
    }
}
