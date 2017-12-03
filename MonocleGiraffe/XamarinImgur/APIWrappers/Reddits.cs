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
        private readonly NetworkHelper networkHelper;

        public Reddits(NetworkHelper networkHelper)
        {
            this.networkHelper = networkHelper;
        }

        public async Task<List<Subreddit>> SearchSubreddits(string query)
        {
            string url = $"http://redditwrapper.azurewebsites.net/api/SubredditSearch?query={query}";
            JObject result = await networkHelper.ExecuteRedditWrapperRequest(url);
            return result["subreddits"].ToObject<List<Subreddit>>();
        }

        public async Task<Subreddit> GetSubreddit(string idUrl)
        {
            string url = $"https://www.reddit.com/r/{idUrl}/about.json";
            JObject result = await networkHelper.ExecuteRedditRequest(url);
            return result.ToObject<Subreddit>();
        }
    }
}
