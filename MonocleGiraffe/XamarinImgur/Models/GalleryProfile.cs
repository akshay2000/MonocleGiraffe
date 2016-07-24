using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Models
{
    public class GalleryProfile
    {
        [JsonProperty("trophies")]
        public IList<Trophy> Trophies { get; set; }

        [JsonProperty("total_gallery_comments")]
        public int TotalGalleryComments { get; set; }

        [JsonProperty("total_gallery_favorites")]
        public int TotalGalleryFavorites { get; set; }

        [JsonProperty("total_gallery_submissions")]
        public int TotalGallerySubmissions { get; set; }
    }
}
