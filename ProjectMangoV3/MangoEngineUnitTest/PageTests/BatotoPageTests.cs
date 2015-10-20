using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MangoEngine.Pages;
using System.Threading.Tasks;

namespace MangoEngineUnitTest.PageTests
{
    [TestClass]
    public class BatotoPageTests
    {

        [TestMethod]
        public async Task GetImgLinkTest()
        {
            /*Test for the returned IMG Link from the page*/

            //Create a new instance of the Page.
            BatotoPage testPage = new BatotoPage("http://bato.to/read/_/316347/isuca_v7_ch40_by_sacred-blade-scans/2");

            //Try to get the image from it.
            string imgUrl = await testPage.GetImageUrlAsync();

            //expected result:
            string imgUrlSource = "http://img.bato.to/comics/2015/04/17/i/read5530a2d7d4d52/img000002.jpg";

            //Check for mismatch
            Assert.AreEqual(imgUrl, imgUrlSource);

        }

    }
}
