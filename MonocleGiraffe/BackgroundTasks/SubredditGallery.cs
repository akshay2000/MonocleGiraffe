using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace BackgroundTasks
{
    class SubredditGallery
    {
        public IAsyncOperation<IEnumerable<string>> GetSubredditGallery(string subUrl)
        {
            try
            {
                return GetSubredditGalleryHelper(subUrl).AsAsyncOperation();
            }
            catch
            {
                return null;
            }
        }

        private string clientId;
        private async Task<string> GetClientId()
        {
            if (clientId == null)
            {
                var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var libFolder = installationFolder;
                var file = await libFolder.GetFileAsync("Secrets.json");
                string fileContent = await Windows.Storage.FileIO.ReadTextAsync(file);
                clientId = (string)JObject.Parse(fileContent)["Client_Id"];
            }
            return clientId;
        }

        private async Task<IEnumerable<string>> GetSubredditGalleryHelper(string subUrl)
        {
            string url = $"https://api.imgur.com/3/gallery/r/{subUrl}";
            string response = await (await GetHttpClient()).GetStringAsync(new Uri(url));
            JObject jObject = JObject.Parse(response);
            JArray array = (JArray)jObject["data"];
            return array.Select(ToThumbId);
        }

        private string ToThumbId(JToken o)
        {
            return (bool)o["is_album"] ? (string)o["cover"] : (string)o["id"];
        }

        private HttpClient httpClient;
        private async Task<HttpClient> GetHttpClient()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders["Authorization"] = $"Client-ID {await GetClientId()}";
            }
            return httpClient;
        }
    }
}
