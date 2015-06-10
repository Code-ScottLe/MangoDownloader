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
        protected string _currentImgUrl;
        protected string _currentImgFilename;
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

        public string CurrentImgUrl
        {
            get { return _currentImgUrl; }
            protected set { _currentImgUrl = value; }
        }

        public string CurrentImgFilename
        {
            get { return _currentImgFilename; }
            protected set { _currentImgFilename = value; }
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

        #endregion

        #region Methods

        /*Methods*/
        public abstract void Init();

        public abstract Task InitAsync();

        public abstract bool NextPage();

        public abstract Task<bool> NextPageAsync();

        public abstract string GetImageUrl();

        public abstract string GetImageUrlAsync();

        public virtual string GetFileName(string imgUrl)
        {
            /*Get the filename with the filetype from a image url*/
            //Strat: Scan from the bottom up for the last /.
            int lastSlashIndex = imgUrl.LastIndexOf('/');

            //create a substr without that last slash
            string filename = imgUrl.Substring(lastSlashIndex + 1);

            //return a copy of that.
            return filename;
        }



        #endregion
    }
}
