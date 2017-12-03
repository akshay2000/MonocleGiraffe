using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http.Headers;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace RedditWrapper
{
    public static class SubredditSearch
    {
        [FunctionName("SubredditSearch")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {

            string query = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => q.Key == "query")
                .Value;
            log.Info($"Processing request for {query}");
            JObject responseObject = new JObject();

            try
            {
                var client = await GetAuthClient(log);
                var response = await client.GetAsync($"https://oauth.reddit.com/subreddits/search?q={query}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    FlushAuthClient();
                    client = await GetAuthClient(log);
                    response = await client.GetAsync($"https://oauth.reddit.com/subreddits/search?q={query}");
                    if(response.IsSuccessStatusCode) log.Info("Successful after renewing token");
                }

                string contentStr = await response.Content.ReadAsStringAsync();
                JObject content = JObject.Parse(contentStr);
                JArray redditsArray = (JArray)content["data"]["children"];
                JArray responseArray = new JArray();
                foreach (var item in redditsArray)
                {
                    JObject inObject = (JObject)item["data"];
                    JObject subredditItem = new JObject();
                    string[] props = new string[] { "id", "display_name", "title", "over18", "subscribers", "name", "url" };
                    foreach (string propName in props)
                    {
                        subredditItem.Add(propName, inObject[propName]);
                    }
                    responseArray.Add(subredditItem);
                }
                responseObject["subreddits"] = responseArray;
                responseObject["count"] = redditsArray.Count;
            }
            catch (Exception e)
            {
                log.Error("Network exception", e);
            }

            return req.CreateResponse(HttpStatusCode.OK, responseObject, "application/json");
        }

        private static void FlushAuthClient()
        {
            authClient = null;
        }

        private static HttpClient authClient;
        private static async Task<HttpClient> GetAuthClient(TraceWriter log)
        {
            if (authClient == null)
            {
                log.Info("New token requested");
                authClient = new HttpClient();
                string accessToken = await GetAccessToken();
                authClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                authClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Azure:RedditWrapper:1.0.0 (by /u/akshay2000)");
            }
            return authClient;
        }

        private static async Task<string> GetAccessToken()
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            var userName = connectionStrings["ClientId"].ConnectionString;
            var password = connectionStrings["ClientSecret"].ConnectionString;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(
                            System.Text.Encoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", userName, password))));
                JObject request = new JObject(new JProperty("grant_type", "client_credentials"));
                var response = await client.PostAsync("https://www.reddit.com/api/v1/access_token",
                    new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("grant_type", "client_credentials")
                    }));
                dynamic data = await response.Content.ReadAsAsync<object>();
                string accessToken = data.access_token;
                return accessToken;
            }
        }
    }
}
