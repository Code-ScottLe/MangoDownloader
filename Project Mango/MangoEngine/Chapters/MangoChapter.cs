using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine
{
    public abstract class MangoChapter
    {
        /*Represent a chapter of a manga*/

        #region Fields
        /*Fields*/
        protected string _source_name;
        protected string _current_url;
        protected string _base_url;
        protected int _pages_count;
        protected Encoding _encoding;

        #endregion

        #region Properties

        /*Properties*/

        #endregion

        #region Constructors

        /*Constructors*/

        #endregion

        #region Methods

        /*Methods*/
        public abstract void Init();
        public abstract Task InitAsync();
        public abstract bool NextPage();
        public abstract Task<bool> NextPageAsync();
        public abstract string GetImageUrl();
        public abstract string GetImageUrlAsync();

        public virtual string GetFileName(string imgUrl)
        {
            /*Get the filename with the filetype from a image url*/
            //Strat: Scan from the bottom up for the last /.
            int lastSlashIndex = imgUrl.LastIndexOf('/');

            //create a substr without that last slash
            string filename = imgUrl.Substring(lastSlashIndex + 1);

            //return a copy of that.
            return filename;
        }



        #endregion
    }
}
