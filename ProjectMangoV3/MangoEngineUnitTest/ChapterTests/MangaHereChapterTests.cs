using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MangoEngine.Chapters;

namespace MangoEngineUnitTest.ChapterTests
{
    [TestClass]
    public class MangaHereChapterTests
    {

        [TestMethod]
        public async Task InitializationTest()
        {
            /*Test Object Intialization*/

            //Initlaize the object
            MangaHereChapter testChapter = new MangaHereChapter("http://www.mangahere.co/manga/d_frag/v04/c074/");
            await testChapter.InitAsync();

            //Verify the content.
            string sourceNameActual = "MangaHere";
            string mangaTitleActual = "D-Frag";
            string chapterTitleActual = "Vol 04 Ch 074: I Should've Opened It...";
            int pageCountActual = 16;

            //Test
            //Source Name
            Assert.AreEqual(testChapter.SourceName, sourceNameActual);

            //Manga Title
            Assert.AreEqual(testChapter.MangaTitle, mangaTitleActual);

            //Chapter Title
            Assert.AreEqual(testChapter.ChapterTitle, chapterTitleActual);

            //Page Count
            Assert.AreEqual(testChapter.PageCount, pageCountActual);
        }
        
    }
}
