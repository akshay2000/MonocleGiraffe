using XamarinImgur.APIWrappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.Portable.Helpers;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using System;

namespace MonocleGiraffe.Portable.ViewModels.Front
{
    public class SearchViewModel : BindableBase
    {
        private SubredditsViewModel subredditsVM;        
        private readonly INavigationService navigationService;

        public SearchViewModel(SubredditsViewModel subredditsVM, INavigationService nav, bool isInDesignMode)
        {
            navigationService = nav;
            if (isInDesignMode)
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

        RelayCommand<string> searchCommand;
        public RelayCommand<string> SearchCommand
           => searchCommand ?? (searchCommand = new RelayCommand<string>(async (string searchType) =>
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
        }

        public void ImageTapped(int index)
        {
            ImageTapped(Posts[index]);
        }

        public void ImageTapped(GalleryItem item)
        {
            ImageSelectedIndex = Posts.IndexOf(item);
            NavigateToBrowser(Posts);
        }

        public void GifTapped(int position)
        {
            GifTapped(Gifs[position]);
        }

        public void GifTapped(GalleryItem clickedItem)
        {
            ImageSelectedIndex = Gifs.IndexOf(clickedItem);
            NavigateToBrowser(Gifs);
        }

        public void SubredditTapped(int position)
        {
            SubredditTapped(Subreddits[position]);
        }

        public void SubredditTapped(SubredditItem item)
        {
            subredditsVM.GoToSub.Execute(item);
        }

        public RelayCommand<SubredditItem> ToggleFavorite => subredditsVM.ToggleFavorite;

        private void NavigateToBrowser(IncrementalPosts imageCollection)
        {
            const string navigationParamName = "GalleryInfo";
            galleryMetaInfo = new GalleryMetaInfo { Gallery = imageCollection, SelectedIndex = ImageSelectedIndex };
            Helpers.StateHelper.SessionState[navigationParamName] = galleryMetaInfo;
            navigationService.NavigateTo(PageKeyHolder.BrowserPageKey, navigationParamName);
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
            Posts = CreateIncrementalPosts(query);
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
            Gifs = CreateIncrementalPosts(query);
        }

        #endregion

        protected virtual IncrementalPosts CreateIncrementalPosts(string query)
        {
            return new IncrementalPosts(query);
        }

        protected virtual void InitDesignTime()
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
