using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Pages
{
    public class MangaPage
    {
        #region Fields;
        /*Fields*/
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
                return _imgUrl;
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


        #endregion

        #region Constructors
        /*Constructors*/
        #endregion

        #region Methods
        /*Methods*/
        #endregion
    }
}
