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

        #endregion

        #region Property
        /*Property*/
        public string PageUrl
        {
            get
            {
                return _pageUrl;
            }

            protected set
            {
                _pageUrl = value;
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
        #endregion

        #region Constructors
        /*Constructors*/
        #endregion

        #region Methods
        /*Methods*/
        #endregion
    }
}
