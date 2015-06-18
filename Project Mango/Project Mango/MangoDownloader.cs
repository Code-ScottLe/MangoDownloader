using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangoEngine.Chapters;
using MangoEngine.Exceptions;
using MangoEngine.Factories;
using System.Net.Http;

namespace Project_Mango
{

    public class MangoDownloader : INotifyPropertyChanged
    {
        #region Fields
        /*Fields*/
        private HttpClient _downloadClient;
        private MangoChapter _source;
        private string _sourceUrl;
        private string _sourceName;
        private string _saveLocation;
        private int _downloadCount;
        private int _completedPercentage;
        private string _log;

        #endregion

        #region Properties

        /*Properties*/

        public string SourceUrl
        {
            get { return _sourceUrl; }
            set { _sourceUrl = value; }
        }

        public string SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; }
        }

        public string SaveLocation
        {
            get { return _saveLocation; }
            set { _saveLocation = value; }
        }

        public int DownloadCount
        {
            get { return _downloadCount; }
            protected set { _downloadCount = value; }
        }

        public int CompletedPercentage
        {
            get { return _completedPercentage; }
            protected set
            {
                _completedPercentage = value;

                NotifyPropertyChanged("CompletedPercentage");
            }
        }

        public string Log
        {
            get { return _log; }
            protected set
            {
                _log = value;
                NotifyPropertyChanged("Log");
            }
        }

        #endregion

        #region Events
        /*Events*/
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Constructors

        /*Constructors*/

        public MangoDownloader()
        {
            /*Default constructor*/
            _downloadClient = new HttpClient();
            _source = null;
            _sourceUrl = string.Empty;
            _sourceName = string.Empty;
            _saveLocation = string.Empty;
            _downloadCount = 0;
            _completedPercentage = 0;
            _log = string.Empty;
        }

        public MangoDownloader(string url, string saveTo) : this()
        {
            /*Overloaded Constructor, accept a string of URL and a location to save*/
            _sourceUrl = url;
            _saveLocation = saveTo;
        }
        #endregion

        #region Methods

        /*Methods*/

        public async Task StartAsync()
        {
            /*Download the entire manga.*/

            //Update the log
            Log = "Starting...";

            //Initalize the Chapter
            Log = "Initialize Mango Chapter...\n";
            _source = await MangoChapter.Factory.CreateNewAsync(SourceName, SourceUrl);

            //Start downloading.
            Log = "Downloading...\n.\n.\n.\n";

            do
            {
                //Download the current file.
                await DownloadCurrentPageAsync();

            } while (await _source.NextPageAsync() == true);

        }

        protected async Task DownloadCurrentPageAsync()
        {
            int retryCounter = 0;
            /*Download the current file Asynchronously*/
            try
            {
                //Get the img url
                string currentFileUrl = await _source.GetImageUrlAsync();

                //Get the local path of the file
                string save_to = SaveLocation + GetFileName(currentFileUrl);

                //Create a stream to the website.
                Stream downloadStream = await _downloadClient.GetStreamAsync(currentFileUrl);

                //Create a FileStream to the local file.
                Stream saveStream = new FileStream(save_to, FileMode.OpenOrCreate);
                
                //Save the file
                await downloadStream.CopyToAsync(saveStream);

                //Flush and close the stream
                saveStream.Flush();
                saveStream.Close();

                //increment the download counter
                DownloadCount++;

                //Update the log
                Log = GetFileName(currentFileUrl) + " downloaded\n";

                //Update the Completed Progres
                CompletedPercentage = CalculateCompletedPercentage();

            }
            catch (Exception e)
            {
                Log = "Something is Wrong! Download Failed! Detailed Message: " + e.ToString() + "\n";

            }

        }

        protected int CalculateCompletedPercentage()
        {
            float percentaged = (float)DownloadCount / (float)_source.PagesCount;

            int percentage = (int)(percentaged * 100);

            return percentage;
        }

        protected string GetFileName(string src_url)
        {
            //Parse the URl and give back the original file name.
            //Strat: Scan from the bottom up for the last /.
            int last_slash_index = src_url.LastIndexOf('/');

            string filename = string.Empty;
            if (SourceName == "MangaHere")
            {
                int last_question_mark = src_url.LastIndexOf('?');
                filename = src_url.Substring(last_slash_index + 1, last_question_mark - last_slash_index - 1);

            }

            else
            {
                filename = src_url.Substring(last_slash_index + 1);
            }

            //return a copy of that.
            return filename;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }
}
