﻿using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.Portable.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using XamarinImgur.APIWrappers;

namespace MonocleGiraffe.ViewModels
{
    public class SubGalleryPageViewModel : SubGalleryViewModel, INavigable
    {
        public INavigationService NavigationService { get; set; }
        public IDispatcherWrapper Dispatcher { get; set; }
        public IStateItems SessionState { get; set; }

        public IDictionary<string, object> State { get; set; }

        private async Task RestoreState(IDictionary<string, object> state)
        {
            if (state["subredditUrl"] == null)
            {
                Sub = JsonConvert.DeserializeObject<SubredditItem>((string)state["sub"]);
                Images = IncrementalSubredditGallery.FromJson((string)state["images"]);
                state.Clear();
            }
            else
            {
                string subUrl = (string)state["subredditUrl"];
                var sub = await Initializer.Reddits.GetSubreddit(subUrl);
                Sub = new SubredditItem(sub);
                Images = new IncrementalSubredditGallery(subUrl, Enums.Sort.Time);
            }
        }

        public SubGalleryPageViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav) { }
        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            IsBusy = true;
            if (State != null && State.Any())
            {
                await RestoreState(State);
                State = null;
            }
            else if (state.Any())
            {
                await RestoreState(state);
            }
            else
            {
                if (mode == NavigationMode.Back)
                {
                    if (galleryMetaInfo == null)
                    {
                        galleryMetaInfo = BootStrapper.Current.SessionState["GalleryInfo"] as GalleryMetaInfo;
                        Images = galleryMetaInfo?.Gallery as IncrementalSubredditGallery;
                        var sub = await Initializer.Reddits.GetSubreddit(Images.Subreddit);
                        Sub = new SubredditItem(sub);
                    }
                    ImageSelectedIndex = galleryMetaInfo?.SelectedIndex ?? 0;
                }
                else
                {
                    Activate(parameter);
                }
            }
            await Task.CompletedTask;
            IsBusy = false;
        }

        public async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state["images"] = Images.toJson();
                state["sub"] = JsonConvert.SerializeObject(Sub);
            }
            await Task.CompletedTask;
        }

        public Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }

        public void ImageTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as GalleryItem;
            ImageTapped(clickedItem);
            //TODO Remove this after refactor is done
            BootStrapper.Current.SessionState["GalleryInfo"] = galleryMetaInfo;
        }

        protected override Portable.ViewModels.IncrementalSubredditGallery CreateSubredditGallery(string subreddit, Enums.Sort sort)
        {
            return new IncrementalSubredditGallery(subreddit, sort);
        }
    }

    public class IncrementalSubredditGallery : Portable.ViewModels.IncrementalSubredditGallery, ISupportIncrementalLoading
    {
        public IncrementalSubredditGallery(string subreddit, Enums.Sort sort)
            : base(subreddit, sort)
        {
            Init();
        }

        public bool HasMoreItems
        {
            get
            {
                return HasMore;
            }
        }

        private async void Init()
        {
            if (!IsBusy)
                await LoadMoreAsync(60);
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            var ret = await LoadMoreAsync(c, count);
            return new LoadMoreItemsResult { Count = count };
        }

        public static IncrementalSubredditGallery FromJson(string s)
        {
            JObject o = JObject.Parse(s);
            Enums.Sort sort = JsonConvert.DeserializeObject<Enums.Sort>((string)o["sort"]);
            string subreddit = (string)o["subreddit"];
            return new IncrementalSubredditGallery(subreddit, sort);
        }
    }
}
