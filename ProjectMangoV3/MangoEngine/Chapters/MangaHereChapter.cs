using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Helpers;
using MangoEngine.Exceptions;
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace MangoEngine.Chapters
{
    public class MangaHereChapter : MangaChapter
    {
        #region Fields
        /**/
        #endregion

        #region Property
        /**/
        #endregion

        #region Constructors
        /**/
        public MangaHereChapter() : base()
        {
            //default constructor, call base
            SourceName = "MangaHere";
        }

        public MangaHereChapter(string url) : base(url)
        {
            //Overloaded, call base and set name
            SourceName = "MangaHere";
        }

        #endregion

        #region Methods
        /**/
        public override async Task InitAsync()
        {

        }
        #endregion



    }
}
