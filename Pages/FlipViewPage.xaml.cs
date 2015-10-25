using MonocleGiraffe.Helpers;
using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class FlipViewPage : Page
    {
        MainViewModel dataContext;
        bool isViewRendered = false;
        public FlipViewPage()
        {
            this.InitializeComponent();
            dataContext = StateHelper.ViewModel;
            DataContext = dataContext;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            var currentImage = dataContext.ImageItems[dataContext.SelectedIndex];
            request.Data.Properties.Title = currentImage.Title;
            request.Data.SetWebLink(new Uri(currentImage.Link));
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
       
        private void AlbumGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StateHelper.AlbumVM.AlbumItem = dataContext.ImageItems[dataContext.SelectedIndex];
            StateHelper.AlbumVM.SelectedIndex = (sender as GridView).SelectedIndex;
            Frame.Navigate(typeof(AlbumPage));
        }
    }
}
