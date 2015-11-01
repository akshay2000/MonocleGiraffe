using MonocleGiraffe.Helpers;
using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumPage : Page
    {
        AlbumViewModel dataContext;

        public AlbumPage()
        {
            this.InitializeComponent();
            dataContext = StateHelper.AlbumVM;
            DataContext = dataContext;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            var currentImage = dataContext.AlbumItem.AlbumImages[dataContext.SelectedIndex];
            request.Data.Properties.Title = currentImage.Title ?? dataContext.AlbumItem.Title;
            request.Data.SetWebLink(new Uri(currentImage.Link));
        }

        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            string clickedTag = (string)(sender as FrameworkElement).Tag;
            switch (clickedTag)
            {
                case "CopyLink":
                    CopyLink();
                    break;
                case "OpenBrowser":
                    OpenBrowser();
                    break;
                case "OtherApps":
                default:
                    DataTransferManager.ShowShareUI();
                    break;
            }
        }

        private void CopyLink()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(GetLinkToShare());
            Clipboard.SetContent(dataPackage);
        }

        private async void OpenBrowser()
        {
            Uri uriToLaunch = new Uri(GetLinkToShare());
            await Launcher.LaunchUriAsync(uriToLaunch);
        }

        private string GetLinkToShare()
        {
            return dataContext.AlbumItem.AlbumImages[dataContext.SelectedIndex].Link;
        }
    }
}
