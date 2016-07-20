using Newtonsoft.Json;

namespace XamarinImgur.Models
{
    public class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }
                
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("over18")]
        public bool? Over18 { get; set; }

        [JsonProperty("subscribers")]
        public int? Subscribers { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }       
    }

    public class Subreddit
    {

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }
}
