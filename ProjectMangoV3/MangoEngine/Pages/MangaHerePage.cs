using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Pages
{
    public class MangaHerePage : MangaPage
    {
        #region Fields
        /*Fields*/
        #endregion

        #region Properties
        /*Properties*/
        #endregion

        #region Constructors
        /*Constructors*/
        public MangaHerePage() : base()
        {
            //default constructor
            SourceName = "MangaHere";
        }

        public MangaHerePage(string url) : base(url)
        {
            //Overloaded constructor, accept a string of url
            SourceName = "MangaHere";
        }
        #endregion

        #region Methods
        /*Methods*/
        public override async Task<string> GetImageUrlAsync()
        {
            //Get the image URL from the current URL. 
        }
        #endregion
    }
}
