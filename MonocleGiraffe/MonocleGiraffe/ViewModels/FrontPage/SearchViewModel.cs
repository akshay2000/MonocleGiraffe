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
        }

        private ObservableCollection<SubredditItem> subreddits;
        public ObservableCollection<SubredditItem> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        DelegateCommand<string> searchRedditsCommand;
        public DelegateCommand<string> SearchRedditsCommand
           => searchRedditsCommand ?? (searchRedditsCommand = new DelegateCommand<string>(async (string parameter) =>
           {
               await SearchSubreddits(parameter);
           }));

        public async Task SearchSubreddits(string query)
        {
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
