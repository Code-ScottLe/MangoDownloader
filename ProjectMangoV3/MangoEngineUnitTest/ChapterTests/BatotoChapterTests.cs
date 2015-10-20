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
    public class BatotoChapterTests
    {
        [TestMethod]
        public async Task InitializationTest()
        {
            /*Test object intialization*/

            //Initialize the chapter.
            BatotoChapter testChapter = new BatotoChapter("http://bato.to/read/_/304588/isuca_v6_ch35_by_sacred-blade-scans");
            await testChapter.InitAsync();

            //Verify the content.
            string sourceNameActual = "Batoto";
            string mangaTitleActual = "Isuca";
            string chapterTitleActual = "Vol.6 Ch.35";
            int pageCountActual = 28;

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
