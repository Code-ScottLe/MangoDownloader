using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Pages
{
    public interface IMangaPage
    {
        #region Properties
        /*Properties*/
        string PageUrl { get; set; }
        string ImgUrl { get; set; }
        int PageIndex { get; set; }
        string SourceName { get; set; }
        #endregion

        #region Methods
        /*Methods*/
        Task<string> GetImageUrlAsync();
        #endregion
    }
}
