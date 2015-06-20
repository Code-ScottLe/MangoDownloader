using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.ComponentModel;
using MangoEngine.Chapters;

namespace MangoDownloader
{
    public class MangoChapterDownloader : INotifyPropertyChanged
    {
        #region #Fields
        /*Fields*/
        protected HttpClient myClient;
        private string _url;
        private string _saveTo;
        private string _sourceName;
        private List<string> _sourceNameList;
        private double _completedPercentage;
        private string _log;
        #endregion

        #region Properties
        /*Properties*/
        public string URL
        {
            get { return _url; }
            set { _url = value; OnPropertyChanged("URL"); }
        }

        public string SaveTo
        {
            get { return _saveTo; }
            set { _saveTo = value; OnPropertyChanged("SaveTo"); }
        }

        public string SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; OnPropertyChanged("SourceName"); }
        }

        public List<string> SourceNameList
        {
            get { return _sourceNameList; }
            set { _sourceNameList = value; OnPropertyChanged("SourceNameList"); }
        }

        public double CompletedPercentage
        {
            get { return _completedPercentage; }
            set { _completedPercentage = value; OnPropertyChanged("CompletedPercentage"); }
        }

        public string Log
        {
            get { return _log; }
            set { _log = value; OnPropertyChanged("Log"); }
        }

        #endregion

        #region Events
        /*Events*/
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors
        /*Constructors*/
        public MangoChapterDownloader()
        {
            _url = string.Empty;
            _saveTo = string.Empty;
            _sourceName = string.Empty;
            _sourceNameList = new List<string>();
            _completedPercentage = 0.0;
            _log = string.Empty;
            myClient = new HttpClient();
            SupportedSourceList();
        }
        #endregion

        #region Methods
        /*Methods*/
        private void SupportedSourceList()
        {
            _sourceNameList.Add("Batoto");
            _sourceNameList.Add("MangaHere");
            _sourceNameList.Add("Fakku");
            _sourceNameList.Add("Pururin");
        }

        public async Task DownloadAsync()
        {
            /*Download the current chapter*/

            //Try to create an instance of the chapter
            MangoChapter myChapter = null;

            try
            {
                myChapter = await MangoChapter.Factory.CreateNewAsync(SourceName,URL);
            }

            catch (Exception e)
            {
                //failed!
                return;
            }

            //Able to get the initialization to the source kick off the download
            int DownloadCount = 0;
            do
            {
                await DownloadCurrentPage(myChapter);

                //Increase the download count
                DownloadCount++;

                //Update the completed Percentage
                CompletedPercentage = CalculateCompletedPercentage(DownloadCount, myChapter.PagesCount);

            } while (await myChapter.NextPageAsync() == true);
        }

        protected async Task DownloadCurrentPage(MangoChapter source)
        {
            /*Download the current page of the chapter*/

            //get the img link
            string imgLink = await source.GetImageUrlAsync();

            //get the local path to save
            string saveLocalPath = SaveTo + GetFileName(imgLink);

            //Create a stream to the website.
            Stream downloadStream = null;
            try
            {
                downloadStream = await GetDownloadStream(myClient, imgLink, new TimeSpan(0, 0, 1));
            }
             
            catch (Exception e)
            {
                return;
            }

            //Create a FileStream to the local file.
            Stream saveStream = new FileStream(saveLocalPath, FileMode.OpenOrCreate);

            //Save the file
            await downloadStream.CopyToAsync(saveStream);

            //Flush and close the stream
            saveStream.Flush();
            saveStream.Close();
        }

        protected async Task<Stream> GetDownloadStream(HttpClient myClient, string url, TimeSpan retryInterval, int retryCount = 3)
        {
            /*Get the instance of the MangoChapter with [default] 3 trials*/

            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    var stream = await myClient.GetStreamAsync(url);

                    return stream;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    await Task.Delay(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
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

        protected int CalculateCompletedPercentage(int DownloadCount, int PagesCount)
        {
            float percentaged = (float)DownloadCount / (float)PagesCount;
        
            int percentage = (int)(percentaged * 100);

            return percentage;
        }

        void OnPropertyChanged(string propertyName)
        {
            /*Fire up the PropertyChanged Event with the given property name*/
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
