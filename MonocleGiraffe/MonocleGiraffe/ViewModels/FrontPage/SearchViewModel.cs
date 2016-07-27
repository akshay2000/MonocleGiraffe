using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using XamarinImgur.APIWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;
using System.Threading;
using Template10.Common;
using MonocleGiraffe.Pages;
using Windows.UI.Xaml.Controls;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class SearchViewModel : BindableBase
    {
        private SubredditsViewModel subredditsVM;

        public SearchViewModel()
        {
            InitDesignTime();
        }

        public SearchViewModel(SubredditsViewModel subredditsVM)
        {
            if (DesignMode.DesignModeEnabled)
                InitDesignTime();
            else
                Init(subredditsVM);
        }

        private void Init(SubredditsViewModel subredditsVM)
        {
            this.subredditsVM = subredditsVM;
            IsReddit = true;
        }


        bool isLoading = default(bool);
        public bool IsLoading { get { return isLoading; } set { Set(ref isLoading, value); } }
        
        bool isReddit = default(bool);
        public bool IsReddit { get { return isReddit; }
            set { Set(ref isReddit, value); } }

        bool isPosts = default(bool);
        public bool IsPosts { get { return isPosts; }
            set { Set(ref isPosts, value); } }

        bool isGifs = default(bool);
        public bool IsGifs { get { return isGifs; }
            set { Set(ref isGifs, value); } }

        string queryText = default(string);
        public string QueryText { get { return queryText; } set { Set(ref queryText, value); } }

        DelegateCommand<string> searchCommand;
        public DelegateCommand<string> SearchCommand
           => searchCommand ?? (searchCommand = new DelegateCommand<string>(async (string searchType) =>
           {
               IsLoading = true;
               await Refresh(searchType);
               IsLoading = false;
           }));

        private int imageSelectedIndex;
        public int ImageSelectedIndex
        {
            get { return imageSelectedIndex; }
            set { Set(ref imageSelectedIndex, value); }
        }

        private GalleryMetaInfo galleryMetaInfo;

        public void ImageTapped(object sender, object parameter)
        {
            var clickedItem = parameter as GalleryItem;
            ImageSelectedIndex = Posts.IndexOf(clickedItem);
            NavigateToBrowser(Posts);
        }

        public void GifTapped(object sender, object parameter)
        {
            var clickedItem = (parameter as ItemClickEventArgs).ClickedItem as GalleryItem;
            ImageSelectedIndex = Gifs.IndexOf(clickedItem);
            NavigateToBrowser(Gifs);
        }

        private void NavigateToBrowser(IncrementalPosts imageCollection)
        {
            const string navigationParamName = "GalleryInfo";
            galleryMetaInfo = new GalleryMetaInfo { Gallery = imageCollection, SelectedIndex = ImageSelectedIndex };
            BootStrapper.Current.SessionState[navigationParamName] = galleryMetaInfo;
            BootStrapper.Current.NavigationService.Navigate(typeof(BrowserPage), navigationParamName);
            return;
        }

        public async Task Refresh(string searchType)
        {
            switch (searchType)
            {
                case "Reddits":
                    IsPosts = IsGifs = IsReddit = false;
                    IsReddit = true;
                    await SearchSubreddits(QueryText);
                    break;
                case "Posts":
                    IsPosts = IsGifs = IsReddit = false;
                    IsPosts = true;
                    SearchPosts(QueryText);
                    break;
                case "Gifs":
                    IsPosts = IsGifs = IsReddit = false;
                    IsGifs = true;
                    SearchGifs(QueryText);
                    break;
                default:
                    if (IsReddit)
                        await SearchSubreddits(QueryText);
                    else if (IsPosts)
                        SearchPosts(QueryText);
                    else if (isGifs)
                        SearchGifs(QueryText);
                    break;
            }
        }

        #region Reddit

        private ObservableCollection<SubredditItem> subreddits;
        public ObservableCollection<SubredditItem> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        public async Task SearchSubreddits(string query)
        {
            if (string.IsNullOrWhiteSpace(QueryText))
                return;
            Subreddits = new ObservableCollection<SubredditItem>();
            var subs = await Reddits.SearchSubreddits(query);
            IEnumerable<string> subscribedSubs = subredditsVM.Subreddits.Select(s => s.Url);
            foreach (var sub in subs)
            {
                SubredditItem si = new SubredditItem(sub);
                si.IsFavorited = subscribedSubs.Contains(si.Url);
                Subreddits.Add(si);
            }
        }

        #endregion

        #region Posts

        IncrementalPosts posts = default(IncrementalPosts);
        public IncrementalPosts Posts { get { return posts; } set { Set(ref posts, value); } }

        public void SearchPosts(string query)
        {
            if (string.IsNullOrWhiteSpace(QueryText))
                return;
            Posts = new IncrementalPosts(query);
        }

        #endregion

        #region Gifs

        IncrementalPosts gifs = default(IncrementalPosts);
        public IncrementalPosts Gifs { get { return gifs; } set { Set(ref gifs, value); } }

        public void SearchGifs(string query)
        {
            if (string.IsNullOrWhiteSpace(QueryText))
                return;
            query += " ext: gif";
            Gifs = new IncrementalPosts(query);
        }

        #endregion

        private void InitDesignTime()
        {
            Subreddits = new ObservableCollection<SubredditItem>();
            Subreddits.Add(new SubredditItem { Title = "Funny", Url = "funny", IsFavorited = true });
            Subreddits.Add(new SubredditItem { Title = "Pictures", Url = "pics", IsFavorited = true });
            Subreddits.Add(new SubredditItem { Title = "WTF?!", Url = "funny", IsFavorited = false });
            Subreddits.Add(new SubredditItem { Title = "The cutest things on the internet", Url = "aww", IsFavorited = false });
            Subreddits.Add(new SubredditItem { Title = "Cats", Url = "cats", IsFavorited = true });
            Subreddits.Add(new SubredditItem { Title = "EarthPorn", Url = "earthporn", IsFavorited = false });
            Subreddits.Add(new SubredditItem { Title = "Dank Memes", Url = "adviceanimals", IsFavorited = true });
        }
    }

    public class IncrementalPosts : IncrementalCollection<GalleryItem>
    {
        public string Query { get; set; }
        public IncrementalPosts(string query)
        {
            Query = query;
        }

        public bool HasMore { get; set; } = true;

        protected override bool HasMoreItemsImpl()
        {
            return HasMore;
        }

        protected async override Task<List<GalleryItem>> LoadMoreItemsImplAsync(CancellationToken c, uint page)
        {
            var images = (await Gallery.SearchGallery(Query, Enums.Sort.Viral, (int)page)).Content;
            if (images.Count == 0)
            {
                HasMore = false;
                return new List<GalleryItem>();
            }
            return images.Select(i => new GalleryItem(i)).ToList();
        }
    }    
}
