using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private ObservableCollection<Subreddit> subreddits;
        public ObservableCollection<Subreddit> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        private const string subredditsFileName = "subreddits.json";
        public async void LoadSubreddits()
        {
            string jsonString = await RoamingDataHelper.GetText(subredditsFileName);
            var subredditsList = JArray.Parse(jsonString).ToObject<List<Subreddit>>();
            if (subredditsList.Count == 0)
            {
                subredditsList = new List<Subreddit>() {
                    new Subreddit { FriendlyName = "Funny", ActualName = "funny" },
                    new Subreddit { FriendlyName = "Pictures", ActualName = "pics" },
                    new Subreddit { FriendlyName = "WTF?!", ActualName = "funny" },
                    new Subreddit { FriendlyName = "The cutest things on the internet", ActualName = "aww" },
                    new Subreddit { FriendlyName = "Cats", ActualName = "cats" },
                    new Subreddit { FriendlyName = "EarthPorn", ActualName = "earthporn" },
                    new Subreddit { FriendlyName = "Dank Memes", ActualName = "adviceanimals" }
                };            
            }
            Subreddits = new ObservableCollection<Subreddit>(subredditsList);
        }

        public void AddSubreddit(Subreddit subreddit)
        {
            Subreddits.Insert(0, subreddit);
            SaveSubreddits();
        }

        public void RemoveSubreddit(Subreddit subreddit)
        {
            Subreddits.Remove(subreddit);
            SaveSubreddits();            
        }

        internal async void SaveSubreddits()
        {
            string text = JsonConvert.SerializeObject(Subreddits);
            await RoamingDataHelper.StoreText(text, subredditsFileName);
        }

        private void LoadSubredditsDesignTime()
        {
            Subreddits = new ObservableCollection<Subreddit>();
            Subreddits.Add(new Subreddit { FriendlyName = "Funny", ActualName = "funny" });
            Subreddits.Add(new Subreddit { FriendlyName = "Pictures", ActualName = "pics" });
            Subreddits.Add(new Subreddit { FriendlyName = "WTF?!", ActualName = "funny" });
            Subreddits.Add(new Subreddit { FriendlyName = "The cutest things on the internet", ActualName = "aww" });
            Subreddits.Add(new Subreddit { FriendlyName = "Cats", ActualName = "cats" });
            Subreddits.Add(new Subreddit { FriendlyName = "EarthPorn", ActualName = "earthporn" });
            Subreddits.Add(new Subreddit { FriendlyName = "Dank Memes", ActualName = "adviceanimals" });
        }
    }
}
