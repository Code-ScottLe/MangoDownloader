using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Chapters;

namespace MangoEngine.Manga
{
    public interface IManga
    {
        /*Generally describe a manga from a manga source.*/

        #region Properties
        /*Properties*/

        //Source name
        string SourceName { get; }

        //name of the manga
        string Name { get; }

        //Author
        string Author { get; }

        //List of chaters
        List<IMangaChapter> Chapters { get; }

        #endregion

        #region Methods
        /*Methods*/
        #endregion

    }
}
