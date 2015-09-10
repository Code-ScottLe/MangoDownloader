using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HttpDownloader
{
    public class HttpParallelDownloader
    {
        #region Fields
        private List<Task<Stream>> downloadThreads;
        private List<HttpDownloadFile> filesToDownload;
        private int maximumDownloadThreads;

        #endregion

        #region Properties
        public int MaximumDownloadThread
        {
            get
            {
                return maximumDownloadThreads;
            }
            set
            {
                maximumDownloadThreads = value;
            }
        }


        #endregion

        #region Constructors
        internal HttpParallelDownloader()
        {
            //default constructor, hidden.
            downloadThreads = new List<Task<Stream>>();
            filesToDownload = new List<HttpDownloadFile>();
            maximumDownloadThreads = 4;
        }

        public HttpParallelDownloader(int maxThreadsNumber) : this()
        {
            //Overloaded constructor. Take an interger as maximum number of threads to be used for downloading.
            maximumDownloadThreads = maxThreadsNumber;
        }
        #endregion

        #region Methods
        public bool AddFile(HttpDownloadFile file)
        {
            //add a file to the download queue.

        }
        #endregion

    }
}
