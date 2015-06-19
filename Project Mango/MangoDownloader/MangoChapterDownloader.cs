﻿using System;
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

        #endregion

        #region Events
        /*Events*/
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors
        /*Constructors*/
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
