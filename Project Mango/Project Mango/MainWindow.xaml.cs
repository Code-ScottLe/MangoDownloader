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
using MangoDownloader;

namespace Project_Mango
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*Custom Fileds*/

        public MainWindow()
        {
            InitializeComponent();
            //More init
            var downloader = (MangoChapterDownloader)this.Resources["Dummer"];

            //Try to init the Combo Box
            for (int i = 0; i < downloader.SourceNameList.Count;i++)
            {
                SourceComboBox.Items.Add(downloader.SourceNameList[i]);
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
                ((MangoChapterDownloader)this.Resources["Dummer"]).SaveTo = dialog.FileName + "\\";
            }
            
        }

        private async void DownloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Check Combo Box for the Source name
            var selected = SourceComboBox.SelectedItem.ToString();

            if(!string.IsNullOrEmpty(selected))
            {
                //Get the reference for the downloader
                var downloader = (MangoChapterDownloader)this.Resources["Dummer"];

                //Set the source name
                downloader.SourceName = selected;

                //Temporary disable the button
                DownloadButton.IsEnabled = false;

                //Kick off the download
                await downloader.DownloadAsync();

                //done with download, re-enable the button
                DownloadButton.IsEnabled = true;
            }
        }

        private void DetailedProgressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //just scroll to the end
            DetailedProgressTextBox.ScrollToEnd();
        }
    }


}
