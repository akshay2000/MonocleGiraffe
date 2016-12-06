using XamarinImgur.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MonocleGiraffe.Portable.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Interfaces;
using System;
using GalaSoft.MvvmLight.Command;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class BrowserViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService navigationService;
        public BrowserViewModel(INavigationService nav)
        {
            navigationService = nav;
            if (IsInDesignMode)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        private void Init()
        { }

        private IEnumerable<IGalleryItem> images;
        public IEnumerable<IGalleryItem> Images
        {
            get { return images; }
            set { Set(ref images, value); }
        }

        int flipViewIndex;
        public int FlipViewIndex
        {
            get { return flipViewIndex; }
            set { Set(ref flipViewIndex, value); }
        }

        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        protected GalleryMetaInfo galleryMetaInfo;
                
        RelayCommand shareCommand;
        public RelayCommand ShareCommand
           => shareCommand ?? (shareCommand = new RelayCommand(() =>
           {
               Share();
           }, () => true));

        private void Share()
        {
            IGalleryItem toShare = Images.ElementAt(FlipViewIndex);
            Helpers.Initializer.SharingHelper.ShareItem(toShare);
        }
        
        RelayCommand editCommand;
        public RelayCommand EditCommand
           => editCommand ?? (editCommand = new RelayCommand(() =>
           {
               Edit();
           }, () => true));

        private void Edit()
        {
            var currentItem = Images.ElementAt(FlipViewIndex);
            const string navigationParamName = "ItemToEdit";
            Helpers.StateHelper.SessionState[navigationParamName] = currentItem;
            navigationService.NavigateTo(PageKeyHolder.EditItemPageKey, navigationParamName);
            return;
        }

        RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
           => deleteCommand ?? (deleteCommand = new RelayCommand(async () =>
           {
               await Delete();
           }));

        private async Task Delete()
        {
            IsBusy = true;
            var currentItem = Images.ElementAt(FlipViewIndex);

            if (currentItem is GalleryItem)
            {
                bool isSuccess = (await XamarinImgur.APIWrappers.Images.DeleteImage(currentItem.Id)).Content;
                if (isSuccess)
                    (Images as ObservableCollection<GalleryItem>)?.Remove((GalleryItem)currentItem);
            }
            if (currentItem is AlbumItem)
            {
                bool isSuccess = (await XamarinImgur.APIWrappers.Albums.DeleteAlbum(currentItem.Id)).Content;
                if (isSuccess)
                    (Images as ObservableCollection<AlbumItem>)?.Remove((AlbumItem)currentItem);
            }

            IsBusy = false;
        }

        RelayCommand closeAdCommand;
        public RelayCommand CloseAdCommand
           => closeAdCommand ?? (closeAdCommand = new RelayCommand(() =>
           {
               navigationService.NavigateTo(PageKeyHolder.SettingsPageKey);
           }));

        public void Activate(object parameter)
        {
            galleryMetaInfo = (GalleryMetaInfo)Helpers.StateHelper.SessionState[(string)parameter];
            Images = galleryMetaInfo.Gallery;
            FlipViewIndex = galleryMetaInfo.SelectedIndex;
        }

        public void Deactivate()
        {
            Images = null;
            FlipViewIndex = -1;
        }

        protected virtual void InitDesignTime()
        {
            var images = new ObservableCollection<GalleryItem>();
            images.Add(new GalleryItem(new Image { Title = "Paper Wizard", Animated = true, Link = "http://i.imgur.com/kJYBDHJh.gif", AccountUrl = "AvengeMeKreigerBots", Mp4 = "http://i.imgur.com/kJYBDHJ.mp4", Ups = 73474, CommentCount = 345, Description="Never made the front page before" }));
            images.Add(new GalleryItem(new Image { Title = "Upvote baby duck for good luck", Animated = false, Link = "http://i.imgur.com/j1jujAp.jpg", AccountUrl = "Snickletits", Mp4 = "", Ups = 879, CommentCount = 49 }));
            Images = images;
        }

    }
    
}
