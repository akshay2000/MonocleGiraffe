using MonocleGiraffe.Models;
using SharpImgur.APIWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;

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
            IsGifs = true;
        }
        
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
               switch (searchType) {
                   case "Reddits":
                       IsPosts = IsGifs = IsReddit = false;
                       IsReddit = true;
                       await SearchSubreddits(QueryText);
                       break;
                   case "Posts":
                       IsPosts = IsGifs = IsReddit = false;
                       IsPosts = true;
                       await SearchPosts(QueryText);
                       break;
                   case "Gifs":
                       IsPosts = IsGifs = IsReddit = false;
                       IsGifs = true;
                       await SearchGifs(QueryText);
                       break;
                   default:
                       if(IsReddit)
                           await SearchSubreddits(QueryText);
                       else if(IsPosts)
                           await SearchPosts(QueryText);
                       else if(isGifs)
                           await SearchGifs(QueryText);
                       break;
               }
           }));



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

        ObservableCollection<GalleryItem> posts = default(ObservableCollection<GalleryItem>);
        public ObservableCollection<GalleryItem> Posts { get { return posts; } set { Set(ref posts, value); } }

        public async Task SearchPosts(string query)
        {
            if (string.IsNullOrWhiteSpace(QueryText))
                return;
            Posts = new ObservableCollection<GalleryItem>();
            var images = await Gallery.SearchGallery(query);
            foreach (var i in images)
            {
                Posts.Add(new GalleryItem(i));
            }
        }

        #endregion

        #region Gifs

        ObservableCollection<GalleryItem> gifs = default(ObservableCollection<GalleryItem>);
        public ObservableCollection<GalleryItem> Gifs { get { return gifs; } set { Set(ref gifs, value); } }

        public async Task SearchGifs(string query)
        {
            if (string.IsNullOrWhiteSpace(QueryText))
                return;
            query += " ext: gif";
            Gifs = new ObservableCollection<GalleryItem>();
            var images = await Gallery.SearchGallery(query);
            foreach (var i in images)
            {
                Gifs.Add(new GalleryItem(i));
            }
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
}
