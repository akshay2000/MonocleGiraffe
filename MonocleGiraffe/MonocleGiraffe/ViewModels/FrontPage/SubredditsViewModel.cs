using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class SubredditsViewModel : ViewModelBase
    {
        private ObservableCollection<Subreddit> subreddits;

        public ObservableCollection<Subreddit> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

    }
}
