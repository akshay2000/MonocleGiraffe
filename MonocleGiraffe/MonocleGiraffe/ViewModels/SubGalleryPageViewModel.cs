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
    public class SubGalleryPageViewModel : ViewModelBase
    {
        public SubGalleryPageViewModel()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                // design-time experience
            }
            else
            {
                // runtime experience
            }
        }


        IncrementalSubredditGallery images = default(IncrementalSubredditGallery);
        public IncrementalSubredditGallery Images { get { return images; } set { Set(ref images, value); } }

        SubredditItem sub = default(SubredditItem);
        public SubredditItem Sub { get { return sub; } set { Set(ref sub, value); } }

        private int imageSelectedIndex;
        public int ImageSelectedIndex
        {
            get { return imageSelectedIndex; }
            set { Set(ref imageSelectedIndex, value); }
        }

        string sort = "Time";
        public string Sort { get { return sort; } set { Set(ref sort, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
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
                    if(galleryMetaInfo == null)
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
                    var sub = BootStrapper.Current.SessionState[(string)parameter] as SubredditItem;
                    Images = new IncrementalSubredditGallery(sub.Url, Enums.Sort.Time);
                    Sub = sub;
                }
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state["images"] = Images.toJson();
                state["sub"] = JsonConvert.SerializeObject(Sub);
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        private GalleryMetaInfo galleryMetaInfo;

        public void ImageTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as GalleryItem;
            const string navigationParamName = "GalleryInfo";
            galleryMetaInfo = new GalleryMetaInfo { Gallery = (IEnumerable<IGalleryItem>)Images, SelectedIndex = Images.IndexOf(clickedItem) };
            BootStrapper.Current.SessionState[navigationParamName] = galleryMetaInfo;
            BootStrapper.Current.NavigationService.Navigate(typeof(SubredditBrowserPage), navigationParamName);
            return;
        }

        private Enums.Sort ToSort(string s)
        {
            switch (s)
            {                
                case "Time":
                    return Enums.Sort.Time;
                case "Top":
                    return Enums.Sort.Top;
                default:
                    throw new NotImplementedException($"Can't convert {s} to Sort");
            }
        }

        DelegateCommand refreshCommand;
        public DelegateCommand RefreshCommand
           => refreshCommand ?? (refreshCommand = new DelegateCommand(() =>
           {
               Images = new IncrementalSubredditGallery(Sub.Url, ToSort(Sort));
           }, () => true));


        DelegateCommand<string> sortCommand;
        public DelegateCommand<string> SortCommand
           => sortCommand ?? (sortCommand = new DelegateCommand<string>(SortCommandExecute, SortCommandCanExecute));
        bool SortCommandCanExecute(string param) => true;
        void SortCommandExecute(string param)
        {
            Sort = param;
            RefreshCommand.Execute();
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
