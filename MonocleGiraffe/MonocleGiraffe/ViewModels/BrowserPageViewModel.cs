﻿using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.ViewModels.FrontPage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.Services.Store;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{
    public class BrowserPageViewModel : Portable.ViewModels.BrowserViewModel, INavigable
    {
        public INavigationService NavigationService { get; set; }
        public IDispatcherWrapper Dispatcher { get; set; }
        public IStateItems SessionState { get; set; }

        public BrowserPageViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav)
        { }

        public IDictionary<string, object> State { get; set; }

        private async Task RestoreState(IDictionary<string, object> state)
        {
            string imagesJson = (string)state["images"];
            string type = (string)state["type"];
            int index = state["index"] is int ? (int)state["index"] : int.Parse((string)state["index"]);

            IEnumerable<IGalleryItem> collection = null;
            if (type == typeof(IncrementalGallery).Name) collection = IncrementalGallery.fromJson(imagesJson);
            if (type == typeof(IncrementalSubredditGallery).Name) collection = (IEnumerable<IGalleryItem>)IncrementalSubredditGallery.FromJson(imagesJson);

            if (collection != null && collection is ISupportIncrementalLoading)
            {
                var im = (ISupportIncrementalLoading)collection;
                while (collection.Count() < index + 1)
                    await im.LoadMoreItemsAsync(60);
            }
            Images = collection;
            FlipViewIndex = index;
            state.Clear();
        }

        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            IsBusy = true;
            if (State != null && State.Any())
                await RestoreState(State);
            else if (state.Any())
                await RestoreState(state);            
            else
                Activate(parameter);
            IsBusy = false;
        }       

        public Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
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
            return Task.CompletedTask;
        }

        public Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            if (galleryMetaInfo == null)
                BootStrapper.Current.SessionState["GalleryInfo"] = new GalleryMetaInfo { Gallery = Images, SelectedIndex = FlipViewIndex };
            else
                galleryMetaInfo.SelectedIndex = FlipViewIndex;
            return Task.CompletedTask;
        }

        private async Task Delete()
        {
            IsBusy = true;
            var currentItem = Images.ElementAt(FlipViewIndex);

            if (currentItem is GalleryItem)
            {
                bool isSuccess = (await Portable.Helpers.Initializer.Images.DeleteImage(currentItem.Id)).Content;
                if (isSuccess)
                    (Images as ObservableCollection<GalleryItem>)?.Remove((GalleryItem)currentItem);
            }
            if (currentItem is AlbumItem)
            {
                bool isSuccess = (await Portable.Helpers.Initializer.Albums.DeleteAlbum(currentItem.Id)).Content;
                if (isSuccess)
                    (Images as ObservableCollection<AlbumItem>)?.Remove((AlbumItem)currentItem);
            }

            IsBusy = false;
        }        
    }
    
}
