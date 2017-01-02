using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Models
{
    public class AzureSubredditItem : SubredditItem
    {
        public string Id { get; set; }
        public int Votes { get; set; }
    }
}
