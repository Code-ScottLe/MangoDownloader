using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Pages;

namespace MangoEngine.Chapters
{
    public interface IMangaChapter
    {
        #region Properties
        /*Properties*/
        string SourceName { get; set; }
        string MangaTitle { get; set; }
        string ChapterTitle { get; set; }
        int PageCount { get; set; }
        string BaseUrl { get; set; }
        List<IMangaPage> Pages { get; set; }
        #endregion

        #region Method
        /*Methods*/
        #endregion

    }
}
