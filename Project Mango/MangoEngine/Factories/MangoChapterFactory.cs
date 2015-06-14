using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Chapters;
using MangoEngine.Exceptions;

namespace MangoEngine.Factories
{
    public class MangoChapterFactory
    {
        #region Fields
        /*Fields*/
        #endregion

        #region Property
        /*Property*/
        #endregion

        #region Constructor
        /*Constructors*/
        #endregion

        #region Methods
        /*Methods*/

        public MangoChapter CreateNew(string sourceName, string sourceUrl)
        {
            /*Return back the correct instance of the corresponding source type (sync)*/
            return CreateNewAsync(sourceName, sourceUrl).Result;
        }

        public async Task<MangoChapter> CreateNewAsync(string sourceName, string sourceUrl)
        {
            /*Return back the correct instance of the corresponding source type (async)*/
            MangoChapter source = null;

            switch (sourceName)
            {
                case "Batoto":
                    source = new BatotoMangoChapter(sourceUrl);
                    break;

                case "MangaHere":
                    source = new MangaHereMangoChapter(sourceUrl);
                    break;

                /*case "Fakku":
                    source = new FakkuMangoSource(source_url);
                    break;*/
            }

            if (source == null)
            {
                throw new MangoException("Can't create an instance of  Mango_Source!");
            }

            //Initalize the source (synced)
            await source.InitAsync();

            //Done, return the source
            return source;
        }
        #endregion
    }
}
