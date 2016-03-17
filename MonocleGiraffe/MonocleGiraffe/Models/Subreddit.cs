using MonocleGiraffe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.Models
{
    public class Subreddit : BindableBase
    {
        private string actualName;
        public string ActualName
        {
            get { return actualName; }
            set { Set(ref actualName, value); }
        }

        private string friendlyName;
        public string FriendlyName
        {
            get { return friendlyName; }
            set { Set(ref friendlyName, value); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }

        private int subscribers;
        public int Subscribers
        {
            get { return subscribers; }
            set { Set(ref subscribers, value); }
        }

    }
}
