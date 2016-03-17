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
    public class SearchViewModel : BindableBase
    {
        public SearchViewModel()
        {
            if (DesignMode.DesignModeEnabled)
                InitDesignTime();
            else
                Init();
        }

        private void Init()
        {
            Subreddits = new ObservableCollection<Subreddit>();
        }

        private ObservableCollection<Subreddit> subreddits;
        public ObservableCollection<Subreddit> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        private void InitDesignTime()
        {
            Subreddits = new ObservableCollection<Subreddit>();
        }
    }
}
