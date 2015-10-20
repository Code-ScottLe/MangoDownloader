using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MangoEngine.Pages;
using System.Threading.Tasks;

namespace MangoEngineUnitTest.PageTests
{
    [TestClass]
    public class MangaHerePageTests
    {
        [TestMethod]
        public async Task GetImgUrlTest()
        {
            /*Test for the returned IMG Link from the page*/

            //Create an instance of the page.
            MangaHerePage testPage = new MangaHerePage("http://www.mangahere.co/manga/to_love_ru_darkness/c060/");

            //Try to get the link
            string imgUrl = await testPage.GetImageUrlAsync();

            string imgUrlActual = "http://a.mhcdn.net/store/manga/8471/060.0/compressed/v001.jpg?v=1444405923";

            //Compare
            Assert.AreEqual(imgUrl, imgUrlActual);
        }
    }
}
