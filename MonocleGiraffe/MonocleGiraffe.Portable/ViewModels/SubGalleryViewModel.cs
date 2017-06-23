using XamarinImgur.APIWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;
using MonocleGiraffe.Portable.Models;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using MonocleGiraffe.Portable.Interfaces;
using MonocleGiraffe.Portable.Helpers;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class SubGalleryViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService navigationService;
        public SubGalleryViewModel(INavigationService nav)
        {
            navigationService = nav;
            if (IsInDesignMode)
            {
                // design-time experience
            }
            else
            {
                // runtime experience
            }
        }

        bool isBusy;
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

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

        protected GalleryMetaInfo galleryMetaInfo;

        public void ImageTapped(int index)
        {
            var item = Images[index];
            ImageTapped(item);
        }

        public void ImageTapped(GalleryItem clickedItem)
        {
            const string navigationParamName = "GalleryInfo";
            galleryMetaInfo = new GalleryMetaInfo { Gallery = Images, SelectedIndex = Images.IndexOf(clickedItem) };
            Helpers.StateHelper.SessionState[navigationParamName] = galleryMetaInfo;
            navigationService.NavigateTo(PageKeyHolder.SubredditBrowserPageKey, navigationParamName);
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

        RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
           => refreshCommand ?? (refreshCommand = new RelayCommand(() =>
           {
               Images = CreateSubredditGallery(Sub.Url, ToSort(Sort));
           }, () => true));


        RelayCommand<string> sortCommand;
        public RelayCommand<string> SortCommand
           => sortCommand ?? (sortCommand = new RelayCommand<string>(SortCommandExecute, SortCommandCanExecute));
        bool SortCommandCanExecute(string param) => true;
        void SortCommandExecute(string param)
        {
            Sort = param;
            RefreshCommand.Execute(null);
        }

        public void Activate(object parameter)
        {
            var sub = StateHelper.SessionState[(string)parameter] as SubredditItem;
            Images = CreateSubredditGallery(sub.Url, Enums.Sort.Time);
            Sub = sub;
        }

        public void Deactivate()
        {
            Images = null;
            Sub = null;
        }
        
        protected virtual IncrementalSubredditGallery CreateSubredditGallery(string subreddit, Enums.Sort sort)
        {
            return new IncrementalSubredditGallery(subreddit, sort);
        }
    }

    public class IncrementalSubredditGallery : IncrementalCollection<GalleryItem>, IJsonizable
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
            var gallery = (await Helpers.Initializer.Gallery.GetSubreddditGallery(Subreddit, Sort, (int)page)).Content;
            if (gallery.Count == 0)
            {
                HasMore = false;
                return new List<GalleryItem>();
            }
            return gallery.Select(i => new GalleryItem(i)).ToList();
        }

        public string toJson()
        {
            JObject o = new JObject();
            o["subreddit"] = Subreddit;
            o["sort"] = JsonConvert.SerializeObject(Sort);
            return o.ToString(Formatting.None);
        }
    }
}
