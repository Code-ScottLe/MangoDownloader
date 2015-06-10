using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
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
