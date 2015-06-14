using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Chapters
{
    public class MangaHereMangoChapter : MangoChapter
    {
        #region Fields
        /*Fields*/
        private List<string> _pagesLinks;
        private int _currentPageIndex;

        #endregion

        #region Properties

        /*Properties*/

        #endregion

        #region Constructors

        /*Constructors*/

        protected MangaHereMangoChapter() : base()
        {
            /*Default Constructor*/
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }

        public MangaHereMangoChapter(string url) : base(url)
        {
            /*Overloaded Constructor, accept a string of URL for the MangaHere source.*/
            _pagesLinks = new List<string>();
            _currentPageIndex = 0;
        }
        #endregion

        #region Methods

        /*Methods*/
        internal override void Init()
        {
            throw new NotImplementedException();
        }

        internal override Task InitAsync()
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

        public override string GetImageUrl()
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetImageUrlAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
