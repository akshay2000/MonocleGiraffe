using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using MonocleGiraffe.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.ApplicationModel;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class SubredditsViewModel : BindableBase
    {
        public SubredditsViewModel()
        {
            if (DesignMode.DesignModeEnabled)
                InitDesignTime();
            else
                Init();
        }

        private void Init()
        {
            LoadSubreddits();
        }

        private void InitDesignTime()
        {
            LoadSubredditsDesignTime();
        }

        private ObservableCollection<SubredditItem> subreddits;
        public ObservableCollection<SubredditItem> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        private const string subredditsFileName = "subreddits.json";
        public async void LoadSubreddits()
        {
            string jsonString = await RoamingDataHelper.GetText(subredditsFileName);
            var subredditsList = JArray.Parse(jsonString).ToObject<List<SubredditItem>>();
            if (subredditsList.Count == 0)
            {
                subredditsList = new List<SubredditItem>() {
                    new SubredditItem { Title = "Funny", Url = "funny" },
                    new SubredditItem { Title = "Pictures", Url = "pics" },
                    new SubredditItem { Title = "WTF?!", Url = "funny" },
                    new SubredditItem { Title = "The cutest things on the internet", Url = "aww" },
                    new SubredditItem { Title = "Cats", Url = "cats" },
                    new SubredditItem { Title = "EarthPorn", Url = "earthporn" },
                    new SubredditItem { Title = "Dank Memes", Url = "adviceanimals" }
                };            
            }
            Subreddits = new ObservableCollection<SubredditItem>(subredditsList);
        }

        DelegateCommand<SubredditItem> toggleFavorite;
        public DelegateCommand<SubredditItem> ToggleFavorite
           => toggleFavorite ?? (toggleFavorite = new DelegateCommand<SubredditItem>(async (SubredditItem parameter) =>
           {
               if (parameter.IsFavorited)
                   await RemoveSubreddit(parameter);
               else
                   await AddSubreddit(parameter);
           }));

        public async Task AddSubreddit(SubredditItem subreddit)
        {
            subreddit.IsFavorited = true;
            Subreddits.Insert(0, subreddit);
            await SaveSubreddits();
        }

        public async Task RemoveSubreddit(SubredditItem subreddit)
        {
            subreddit.IsFavorited = false;
            Subreddits.Remove(Subreddits.Where(s => s.Url == subreddit.Url).First());
            await SaveSubreddits();            
        }

        private async Task SaveSubreddits()
        {
            string text = JsonConvert.SerializeObject(Subreddits);
            await RoamingDataHelper.StoreText(text, subredditsFileName);
        }

        DelegateCommand<SubredditItem> goToSub;
        public DelegateCommand<SubredditItem> GoToSub
           => goToSub ?? (goToSub = new DelegateCommand<SubredditItem>((SubredditItem parameter) =>
           {
               NavigateToSub(parameter);
           }));

        private void NavigateToSub(SubredditItem sub)
        {
            string navigationParamName = "Subreddit";
            BootStrapper.Current.SessionState[navigationParamName] = sub;
            BootStrapper.Current.NavigationService.Navigate(typeof(SubGalleryPage), navigationParamName);            
        }

        public void SubredditTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as SubredditItem;
            NavigateToSub(clickedItem);
        }

        private void LoadSubredditsDesignTime()
        {
            Subreddits = new ObservableCollection<SubredditItem>();
            Subreddits.Add(new SubredditItem { Title = "Funny", Url = "funny" });
            Subreddits.Add(new SubredditItem { Title = "Pictures", Url = "pics" });
            Subreddits.Add(new SubredditItem { Title = "WTF?!", Url = "funny" });
            Subreddits.Add(new SubredditItem { Title = "The cutest things on the internet", Url = "aww" });
            Subreddits.Add(new SubredditItem { Title = "Cats", Url = "cats" });
            Subreddits.Add(new SubredditItem { Title = "EarthPorn", Url = "earthporn" });
            Subreddits.Add(new SubredditItem { Title = "Dank Memes", Url = "adviceanimals" });
        }
    }
}
