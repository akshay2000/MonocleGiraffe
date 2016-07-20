using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Models
{
    public class Topic
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("css")]
        public object Css { get; set; }

        [JsonProperty("ephemeral")]
        public bool Ephemeral { get; set; }

        [JsonProperty("readonly")]
        public bool Readonly { get; set; }
    }
}
