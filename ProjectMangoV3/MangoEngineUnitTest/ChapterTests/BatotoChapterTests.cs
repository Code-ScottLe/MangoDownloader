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
            SourceNameCheckTest(testChapter, sourceNameActual);

            //Manga Title
            MangaTitleCheckTest(testChapter, mangaTitleActual);

            //Chapter Title
            ChapterTitleCheckTest(testChapter, chapterTitleActual);

            //Page Count
            PageCountCheckTest(testChapter, pageCountActual);

        }

        [TestMethod]
        private void SourceNameCheckTest(BatotoChapter testChapter, string actualSourceName)
        {
            Assert.AreEqual(testChapter.SourceName, actualSourceName);
        }

        [TestMethod]
        private void MangaTitleCheckTest(BatotoChapter testChapter, string actualMangaTitle)
        {
            Assert.AreEqual(testChapter.MangaTitle, actualMangaTitle);
        }

        [TestMethod]
        private void ChapterTitleCheckTest(BatotoChapter testChapter, string actualChapterTitle)
        {
            Assert.AreEqual(testChapter.ChapterTitle, actualChapterTitle);
        }

        [TestMethod]
        private void PageCountCheckTest(BatotoChapter testChapter, int actualPageCount)
        {
            Assert.AreEqual(testChapter.PageCount, actualPageCount);
        }
    }
}
