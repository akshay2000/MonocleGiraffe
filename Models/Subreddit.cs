using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Models
{
    public class Subreddit
    {
        public Subreddit(string name, string friendlyName)
        {
            Name = name;
            FriendlyName = friendlyName;
        }

        public string Name { get; set; }

        public string FriendlyName { get; set; }       
    }
}
