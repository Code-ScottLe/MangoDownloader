using MangoEngine.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Chapters
{
    public abstract class MangaChapter
    {
        /*Represent a chapter of a manga*/

        #region Fields;
        /*Fields*/

        //metadata
        private string _sourceName;
        private string _mangaTitle;
        private string _chapterTitle;
        private int _pagesCount;
        private string _baseUrl;

        //custom data
        private List<MangaPage> _pages;

        #endregion

        #region Property
        /*Property*/
        public string SourceName
        {
            get
            {
                return _sourceName;
            }

            protected set
            {
                _sourceName = value;
            }
        }

        public string MangaTitle
        {
            get
            {
                return _mangaTitle;
            }

            protected set
            {
                _mangaTitle = value;
            }
        }

        public string ChapterTitle
        {
            get
            {
                return _chapterTitle;
            }

            protected set
            {
                _chapterTitle = value;
            }
        }

        public int PageCount
        {
            get
            {
                return _pagesCount;
            }

            protected set
            {
                _pagesCount = value;
            }
        }

        public string BaseUrl
        {
            get
            {
                return _baseUrl;
            }

            protected set
            {
                _baseUrl = value;
            }
        }

        public List<MangaPage> Pages
        {
            get { return _pages; }
            protected set { _pages = value; }
        }

        #endregion

        #region Constructors
        /*Constructors*/
        public MangaChapter()
        {
            /*Default Constructor, Accepts no parameter*/
            _sourceName = string.Empty;
            _mangaTitle = string.Empty;
            _chapterTitle = string.Empty;
            _pagesCount = -1;
            _baseUrl = string.Empty;
            _pages = new List<MangaPage>();
        }

        public MangaChapter(string url): this()
        {
            /*Overloaded Constructor. Accepts a string of URL for a manga chapter.*/
            _baseUrl = url;
        }
        #endregion

        #region Methods
        /*Methods*/
        public abstract Task InitAsync();

        
        #endregion
    }
}
