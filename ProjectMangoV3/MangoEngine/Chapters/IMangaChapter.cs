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
        /*Will be used later for testing out Fakes*/
        #region Properties
        /*Properties*/
        string SourceName { get;}
        string MangaTitle { get;}
        string ChapterTitle { get;}
        int PageCount { get; }
        string BaseUrl { get;}
        List<IMangaPage> Pages { get; }
        #endregion

        #region Method
        /*Methods*/
        #endregion

    }
}
