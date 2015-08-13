﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Exceptions;

namespace MangoEngine.Pages
{
    public abstract class MangaPage
    {
        #region Fields;
        /*Fields*/
        private string _sourceName;
        private string _pageUrl;
        private string _imgUrl;
        private int _pageIndex;
        #endregion

        #region Property
        /*Property*/
        public string PageUrl
        {
            get
            {
                return _pageUrl;
            }

            set
            {
                _pageUrl = value;

                //new page was manually modified. delete the current imgUrl if needed to
                ImgUrl = "";
            }
        }

        public string ImgUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_imgUrl))
                {
                    return _imgUrl;
                }

                throw new MangoException("ImgUrl is null! Please run GetImageUrlAsync()");
            }

            protected set
            {
                _imgUrl = value;
            }
        }

        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }

            set
            {
                _pageIndex = value;
            }
        }

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

        #endregion

        #region Constructors
        /*Constructors*/
        public MangaPage()
        {
            /*Default Constructor. Accepts no agurments*/
            _sourceName = string.Empty;
            _pageUrl = string.Empty;
            _imgUrl = string.Empty;
            _pageIndex = -1;
        }

        public MangaPage(string url) :this()
        {
            /*Accept a string of url for the page Url*/
            _pageUrl = url;
        }
        #endregion

        #region Methods
        /*Methods*/
        public abstract Task<string> GetImageUrlAsync();
        #endregion
    }
}
