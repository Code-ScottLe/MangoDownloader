using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WPFFolderBrowser;

namespace Project_Mango
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*Custom Fileds*/
        private MangoDownloader downloader;
        public MainWindow()
        {
            InitializeComponent();

            /*More Init code*/
            downloader = new MangoDownloader();
            downloader.PropertyChanged += downloader_PropertyChanged;

            SourceComboBox.Items.Add("Batoto");
        }

        void downloader_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            /*Event handler for this.*/
            if (e.PropertyName == "Log")
            {
                /*Log changed, update the log*/
                DetailedProgressTextBox.AppendText(downloader.Log);
                DetailedProgressTextBox.ScrollToEnd();
            }

            if (e.PropertyName == "CompletedPercentage")
            {
                /*Progress changed, update progress bar*/
                ProgressBar.Value = downloader.CompletedPercentage;
            }
        }


        private void SaveLocationChooseButton_OnClick(object sender, RoutedEventArgs e)
        {
            /*Get user a Folder Browser to choose.*/
            WPFFolderBrowserDialog dialog = new WPFFolderBrowserDialog();

            //Start from the previous choosen folder if needed to.
            if (!string.IsNullOrEmpty(SaveLocationTextBox.Text))
            {
                dialog.InitialDirectory = SaveLocationTextBox.Text;
            }

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                SaveLocationTextBox.Text = dialog.FileName + "\\";
            }
            
        }

        private async void DownloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            /*Kick off the download*/
            downloader.SourceUrl = UrlTextBox.Text;
            downloader.SaveLocation = SaveLocationTextBox.Text;

            //Start download
            await downloader.StartAsync();
        }
    }
}
