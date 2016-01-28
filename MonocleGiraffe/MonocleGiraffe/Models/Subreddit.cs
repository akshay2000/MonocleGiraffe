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
        public Subreddit(string name, string friendlyName)
        {
            Name = name;
            FriendlyName = friendlyName;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
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
