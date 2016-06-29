using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using SharpImgur.APIWrappers;
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
                // restore state
                state.Clear();
            }
            else
            {
                if (mode == NavigationMode.Back)
                {
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
                // save state
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
            galleryMetaInfo = new GalleryMetaInfo { Gallery = Images, SelectedIndex = Images.IndexOf(clickedItem) };
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

    public class IncrementalSubredditGallery : IncrementalCollection<GalleryItem>
    {
        public IncrementalSubredditGallery(string subreddit, Enums.Sort sort)
        {
            Subreddit = subreddit;
            Sort = sort;
        }

        public string Subreddit { get; set; }
        public Enums.Sort Sort { get; set; }
        public bool HasMore { get; set; } = true;

        protected override bool HasMoreItemsImpl()
        {
            return HasMore;
        }

        protected async override Task<List<GalleryItem>> LoadMoreItemsImplAsync(CancellationToken c, uint page)
        {
            var gallery = (await Gallery.GetSubreddditGallery(Subreddit, Sort, (int)page)).Content;
            if (gallery.Count == 0)
            {
                HasMore = false;
                return new List<GalleryItem>();
            }
            return gallery.Select(i => new GalleryItem(i)).ToList();
        }
    }
}
