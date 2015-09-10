using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpDownloader
{
    public class HttpDownloadFile
    {
        #region Fields
        private Uri downloadUrl;
        private Uri saveLocation;
        #endregion

        #region Properties
        public Uri DownloadUrl
        {
            get { return downloadUrl; }
            set { downloadUrl = value; }
        }

        public Uri SaveLocation
        {
            get { return saveLocation; }
            set { saveLocation = value; }
        }
        #endregion

        #region Constructors
        internal HttpDownloadFile()
        {
            //default constructor
            
        }

        public HttpDownloadFile(Uri DownloadLink, Uri LocalSavePath)
        {
            //Overload constructor
            DownloadUrl = DownloadLink;
            SaveLocation = LocalSavePath;
        }

        public HttpDownloadFile(string DownloadLink, string LocalSavePath)
        {
            //Overload constructor
            DownloadUrl = new Uri(DownloadLink);
            SaveLocation = new Uri(LocalSavePath);
        }
        #endregion

        #region Methods
        #endregion

    }
}
