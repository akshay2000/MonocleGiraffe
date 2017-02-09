using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonocleGiraffe.Portable.Helpers.Initializer;

namespace MonocleGiraffe.Portable.ViewModels.Front
{
    public class SubredditsViewModel : BindableBase
    {
        private const string BUSY = "Busy";
        private const string DONE = "Done";
        private const string EMPTY = "Empty";

        private readonly INavigationService navigationService;
        public SubredditsViewModel(INavigationService nav, bool isDesignMode)
        {
            navigationService = nav;
            if (isDesignMode)
                InitDesignTime();
            else
                Init();
        }

        private async void Init()
        {
            LoadUserSubreddits();
            LoadPopularSubreddits();
        }

        private void InitDesignTime()
        {
            LoadSubredditsDesignTime();
        }

        #region Popular

        string popularState = default(string);
        public string PopularState { get { return popularState; } set { Set(ref popularState, value); } }
        
        private ObservableCollection<SubredditItem> popularSubreddits;
        public ObservableCollection<SubredditItem> PopularSubreddits
        {
            get { return popularSubreddits; }
            set { Set(ref popularSubreddits, value); }
        }

        private async Task LoadPopularSubreddits()
        {
			if (PopularState == BUSY)
				return;
            PopularState = BUSY;            
            List<AzureSubredditItem> populars = (await AzureHelper.Instance.GetTopN(10)).Content;
            if (populars.Count == 0)
            {
                PopularState = EMPTY;
            }
            else
            {
                PopularSubreddits = new ObservableCollection<SubredditItem>(populars);
                PopularState = DONE;
            }
        }

        #endregion

        #region User

        string userState = default(string);
        public string UserState { get { return userState; } set { Set(ref userState, value); } }

        private ObservableCollection<SubredditItem> userSubreddits;
        public ObservableCollection<SubredditItem> UserSubreddits
        {
            get { return userSubreddits; }
            set { Set(ref userSubreddits, value); }
        }

        private const string subredditsFileName = "subreddits.json";
        public async Task LoadUserSubreddits()
        {
			if (UserState == BUSY)
				return;
            UserState = BUSY;
            string jsonString = await RoamingDataHelper.GetText(subredditsFileName);
            var subredditsList = JArray.Parse(jsonString).ToObject<List<SubredditItem>>();
            if (subredditsList.Count == 0)
            {
                UserState = EMPTY;
                UserSubreddits = new ObservableCollection<SubredditItem>();
            }
            else
            {
                UserSubreddits = new ObservableCollection<SubredditItem>(subredditsList);
                UserState = DONE;
            }
        }

        #endregion

        RelayCommand<SubredditItem> toggleFavorite;
        public RelayCommand<SubredditItem> ToggleFavorite
           => toggleFavorite ?? (toggleFavorite = new RelayCommand<SubredditItem>(async (SubredditItem parameter) =>
           {
               if (parameter.IsFavorited)
                   await RemoveSubreddit(parameter);
               else
                   await AddSubreddit(parameter);
           }));

        public async Task AddSubreddit(SubredditItem subreddit)
        {
            subreddit.IsFavorited = true;
            UserSubreddits = UserSubreddits ?? new ObservableCollection<SubredditItem>();
            UserSubreddits.Insert(0, subreddit);
            await SaveSubreddits();
        }

        public async Task RemoveSubreddit(SubredditItem subreddit)
        {
            subreddit.IsFavorited = false;
            UserSubreddits.Remove(UserSubreddits.Where(s => s.Url == subreddit.Url).First());
            await SaveSubreddits();            
        }

        private async Task SaveSubreddits()
        {
            string text = JsonConvert.SerializeObject(UserSubreddits);
            await RoamingDataHelper.StoreText(text, subredditsFileName);
        }

        RelayCommand<SubredditItem> goToSub;
        public RelayCommand<SubredditItem> GoToSub
           => goToSub ?? (goToSub = new RelayCommand<SubredditItem>((SubredditItem parameter) =>
           {
               NavigateToSub(parameter);
           }));

        protected void NavigateToSub(SubredditItem sub)
        {
            string navigationParamName = "Subreddit";
            StateHelper.SessionState[navigationParamName] = sub;
            navigationService.NavigateTo(PageKeyHolder.SubGalleryPageKey, navigationParamName);
            AzureHelper.Instance.UpsertItem(sub);
        }
        
        protected virtual void LoadSubredditsDesignTime()
        {
            UserSubreddits = new ObservableCollection<SubredditItem>();
            UserSubreddits.Add(new SubredditItem { Title = "Funny", Url = "funny" });
            UserSubreddits.Add(new SubredditItem { Title = "Pictures", Url = "pics" });
            UserSubreddits.Add(new SubredditItem { Title = "WTF?!", Url = "wtf" });
            UserSubreddits.Add(new SubredditItem { Title = "The cutest things on the internet", Url = "aww" });
            UserSubreddits.Add(new SubredditItem { Title = "Cats", Url = "cats" });
            UserSubreddits.Add(new SubredditItem { Title = "EarthPorn", Url = "earthporn" });
            UserSubreddits.Add(new SubredditItem { Title = "Dank Memes", Url = "adviceanimals" });
        }
    }
}
