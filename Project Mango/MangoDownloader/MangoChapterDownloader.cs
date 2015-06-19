using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.ComponentModel;

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
            _completedPercentage = 0.0;
            _log = string.Empty;
            myClient = new HttpClient();
        }
        #endregion

        #region Methods
        /*Methods*/
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
