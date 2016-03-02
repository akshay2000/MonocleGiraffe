using MonocleGiraffe.Models;
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
    public class SubredditsViewModel : ViewModelBase
    {
        public SubredditsViewModel()
        {
            if (DesignMode.DesignModeEnabled)
                LoadSubredditsDesignTime();
            else
                LoadSubreddits();
        }

        private ObservableCollection<Subreddit> subreddits;
        public ObservableCollection<Subreddit> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        private void LoadSubreddits()
        {
            LoadSubredditsDesignTime();
        }

        private void LoadSubredditsDesignTime()
        {
            Subreddits = new ObservableCollection<Subreddit>();
            Subreddits.Add(new Subreddit { FriendlyName = "Funny", ActualName = "/r/funny" });
            Subreddits.Add(new Subreddit { FriendlyName = "Pictures", ActualName = "/r/pics" });
            Subreddits.Add(new Subreddit { FriendlyName = "WTF?!", ActualName = "/r/funny" });
            Subreddits.Add(new Subreddit { FriendlyName = "The cutest things on the internet", ActualName = "/r/aww" });
            Subreddits.Add(new Subreddit { FriendlyName = "Cats", ActualName = "/r/cats" });
            Subreddits.Add(new Subreddit { FriendlyName = "EarthPorn", ActualName = "/r/earthporn" });
            Subreddits.Add(new Subreddit { FriendlyName = "Dank Memes", ActualName = "/r/adviceanimals" });
        }
    }
}
