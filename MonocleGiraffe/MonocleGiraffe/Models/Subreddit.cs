using MonocleGiraffe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Models
{
    public class Subreddit : NotifyBase
    {
        public Subreddit()
        {

        }

        public Subreddit(string name, string friendlyName)
        {
            ActualName = name;
            FriendlyName = friendlyName;
        }

        private string actualName;
        public string ActualName
        {
            get { return actualName; }
            set
            {
                if (actualName != value)
                {
                    actualName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string friendlyName;
        public string FriendlyName
        {
            get
            {
                return friendlyName;
            }
            set
            {
                if (friendlyName != value)
                {
                    friendlyName = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
