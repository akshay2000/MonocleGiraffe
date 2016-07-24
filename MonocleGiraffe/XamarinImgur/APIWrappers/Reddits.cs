using Newtonsoft.Json.Linq;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.APIWrappers
{
    public class Reddits
    {
        public static async Task<List<Subreddit>> SearchSubreddits(string query)
        {
            string url = $"https://www.reddit.com/subreddits/search.json?q={query}";
            JObject result = await NetworkHelper.ExecuteRedditRequest(url);
            return result["data"]["children"].ToObject<List<Subreddit>>();
        }
    }
}
