using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using XamarinImgur.APIWrappers;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using MonocleGiraffe.Portable.Models;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class AccountViewModel : Portable.ViewModels.Front.AccountViewModel
    {
        public AccountViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav, DesignMode.DesignModeEnabled)
        { }

        public void ImageTapped(object sender, object args)
        {
            var info = args as ItemClickEventArgs;
            var clickedItem = (IGalleryItem)info.ClickedItem;
            var collection = (info.OriginalSource as ItemsControl).ItemsSource;
            base.ImageTapped(clickedItem, collection);
        }

        private void GoToBrowser(IEnumerable<IGalleryItem> gallery, int index, Type page)
        {
            const string navigationParamName = "GalleryInfo";
            var galleryMetaInfo = new GalleryMetaInfo { Gallery = gallery, SelectedIndex = index };
            BootStrapper.Current.SessionState[navigationParamName] = galleryMetaInfo;
            BootStrapper.Current.NavigationService.Navigate(page, navigationParamName);
            return;
        }
    }
}
