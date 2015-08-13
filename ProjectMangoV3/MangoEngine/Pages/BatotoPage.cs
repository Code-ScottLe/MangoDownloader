using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Pages
{
    public class BatotoPage : MangaPage
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public BatotoPage() : base()
        {
            /*Default Constructors*/
            SourceName = "Batoto";
        }

        public BatotoPage(string url) : base(url)
        {
            /*Overloaded Constructors, accepts a string of URL*/
            SourceName = "Batoto";
        }


        #endregion

        #region Methods

        public override async Task<string> GetImageUrlAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
