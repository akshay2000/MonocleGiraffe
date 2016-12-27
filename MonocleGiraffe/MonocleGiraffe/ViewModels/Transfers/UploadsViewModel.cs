using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.Portable.ViewModels.Transfers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels.Transfers
{
    public class UploadsViewModel : Portable.ViewModels.Transfers.UploadsViewModel
    {
        public UploadsViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav, DesignMode.DesignModeEnabled)
        { }
        

        public void UploadTapped(object sender, object args)
        {
            UploadItem clickedItem = (args as ItemClickEventArgs).ClickedItem as UploadItem;
            UploadTapped(clickedItem);
        }
        
        protected override void InitDesignTime()
        {
            Uploads = new ObservableCollection<IUploadItem>();
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 50, Name = "Unhand me woman", State = TransferStates.PENDING, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 70, Name = "Learn Python for Real", State = TransferStates.CANCELED, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 20, Name = "The full story", State = TransferStates.SUCCESSFUL, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 90, Name = "Goodbye EU", State = TransferStates.UPLOADING, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            IsCancelAllEnabled = true;
        }
    }
}
