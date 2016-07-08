using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using MonocleGiraffe.ViewModels.FrontPage;
using Newtonsoft.Json;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{

    public class BrowserPageViewModel : ViewModelBase
    {
        public BrowserPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        private void Init()
        {
        }

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

        GalleryMetaInfo galleryMetaInfo;
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            IsBusy = true;
            if (state.Any())
            {
                string imagesJson = (string)state["images"];
                string type = (string)state["type"];
                int index = (int)state["index"];

                IEnumerable<IGalleryItem> collection = null;
                if (type == typeof(IncrementalGallery).Name) collection = IncrementalGallery.fromJson(imagesJson);
                if (type == typeof(IncrementalSubredditGallery).Name) collection = IncrementalSubredditGallery.fromJson(imagesJson);

                if(collection != null && collection is ISupportIncrementalLoading)
                {
                    var im = (ISupportIncrementalLoading)collection;
                    while (collection.Count() < index)
                        await im.LoadMoreItemsAsync(60);
                }
                Images = collection;
                FlipViewIndex = index;
                state.Clear();
            }
            else
            {
                galleryMetaInfo = (GalleryMetaInfo)BootStrapper.Current.SessionState[(string)parameter];
                Images = galleryMetaInfo.Gallery;
                FlipViewIndex = galleryMetaInfo.SelectedIndex;
                await base.OnNavigatedToAsync(parameter, mode, state);
            }
            IsBusy = false;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                if (Images is IJsonizable)
                {
                    state["images"] = ((IJsonizable)Images).toJson();
                    state["index"] = FlipViewIndex;
                    state["type"] = Images.GetType().Name;
                }
            }
            return base.OnNavigatedFromAsync(state, suspending);
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            if (galleryMetaInfo == null)
                BootStrapper.Current.SessionState["GalleryInfo"] = new GalleryMetaInfo { Gallery = Images, SelectedIndex = FlipViewIndex };
            else
                galleryMetaInfo.SelectedIndex = FlipViewIndex;
            return base.OnNavigatingFromAsync(args);
        }


        DelegateCommand shareCommand;
        public DelegateCommand ShareCommand
           => shareCommand ?? (shareCommand = new DelegateCommand(() =>
           {
               Share();
           }, () => true));

        private void Share()
        {
            IGalleryItem toShare = Images.ElementAt(FlipViewIndex);
            SharingHelper.ShareItem(toShare);
        }
        
        DelegateCommand editCommand;
        public DelegateCommand EditCommand
           => editCommand ?? (editCommand = new DelegateCommand(() =>
           {
               Edit();
           }, () => true));

        private void Edit()
        {
            var currentItem = Images.ElementAt(FlipViewIndex);
            const string navigationParamName = "ItemToEdit";
            BootStrapper.Current.SessionState[navigationParamName] = currentItem;
            BootStrapper.Current.NavigationService.Navigate(typeof(EditItemPage), navigationParamName);
            return;
        }

        DelegateCommand deleteCommand;
        public DelegateCommand DeleteCommand
           => deleteCommand ?? (deleteCommand = new DelegateCommand(async() =>
           {
               await Delete();
           }, () => true));

        private async Task Delete()
        {
            IsBusy = true;
            var currentItem = Images.ElementAt(FlipViewIndex);

            if (currentItem is GalleryItem)
            {
                bool isSuccess = (await SharpImgur.APIWrappers.Images.DeleteImage(currentItem.Id)).Content;
                if (isSuccess)
                    (Images as ObservableCollection<GalleryItem>)?.Remove((GalleryItem)currentItem);
            }
            if (currentItem is AlbumItem)
            {
                bool isSuccess = (await SharpImgur.APIWrappers.Albums.DeleteAlbum(currentItem.Id)).Content;
                if (isSuccess)
                    (Images as ObservableCollection<AlbumItem>)?.Remove((AlbumItem)currentItem);
            }

            IsBusy = false;
        }

        private void InitDesignTime()
        {
            var images = new ObservableCollection<GalleryItem>();
            images.Add(new GalleryItem(new Image { Title = "Paper Wizard", Animated = true, Link = "http://i.imgur.com/kJYBDHJh.gif", AccountUrl = "AvengeMeKreigerBots", Mp4 = "http://i.imgur.com/kJYBDHJ.mp4", Ups = 73474, CommentCount = 345, Description="Never made the front page before" }));
            images.Add(new GalleryItem(new Image { Title = "Upvote baby duck for good luck", Animated = false, Link = "http://i.imgur.com/j1jujAp.jpg", AccountUrl = "Snickletits", Mp4 = "", Ups = 879, CommentCount = 49 }));
            Images = images;
        }
    }
    
}
