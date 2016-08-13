using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using XamarinImgur.APIWrappers;
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
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using MonocleGiraffe.Pages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;
using MonocleGiraffe.Portable.Models;

namespace MonocleGiraffe.ViewModels
{
    public class SubGalleryPageViewModel : Portable.ViewModels.SubGalleryViewModel, INavigable
    {
        public INavigationService NavigationService { get; set; }
        public IDispatcherWrapper Dispatcher { get; set; }
        public IStateItems SessionState { get; set; }

        public SubGalleryPageViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav) { }
        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                Sub = JsonConvert.DeserializeObject<SubredditItem>((string)state["sub"]);
                Images = IncrementalSubredditGallery.fromJson((string)state["images"]);
                state.Clear();
            }
            else
            {
                if (mode == NavigationMode.Back)
                {
                    if (galleryMetaInfo == null)
                    {
                        galleryMetaInfo = BootStrapper.Current.SessionState["GalleryInfo"] as GalleryMetaInfo;
                        Images = galleryMetaInfo?.Gallery as IncrementalSubredditGallery;
                        var sub = (await Reddits.SearchSubreddits(Images.Subreddit)).First(s => s.Data.DisplayName == Images.Subreddit);
                        Sub = new SubredditItem(sub);
                    }
                    ImageSelectedIndex = galleryMetaInfo?.SelectedIndex ?? 0;
                }
                else
                {
                    //TODO: REMOVE NEXT TWO LINES AFTER REFACTOR IS DONE
                    var sub = BootStrapper.Current.SessionState[(string)parameter] as SubredditItem;
                    //Portable.Helpers.StateHelper.SessionState[(string)parameter] = sub;
                    //Activate(parameter);
                    Images = new IncrementalSubredditGallery(sub.Url, Enums.Sort.Time);
                    Sub = sub;
                }
            }
            await Task.CompletedTask;
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
    }

    public class IncrementalSubredditGallery : Portable.ViewModels.IncrementalSubredditGallery, ISupportIncrementalLoading
    {
        public IncrementalSubredditGallery(string subreddit, Enums.Sort sort)
            : base(subreddit, sort) { }

        public bool HasMoreItems
        {
            get
            {
                return HasMore;
            }
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

        public static IncrementalSubredditGallery fromJson(string s)
        {
            JObject o = JObject.Parse(s);
            Enums.Sort sort = JsonConvert.DeserializeObject<Enums.Sort>((string)o["sort"]);
            string subreddit = (string)o["subreddit"];
            return new IncrementalSubredditGallery(subreddit, sort);
        }
    }
}
