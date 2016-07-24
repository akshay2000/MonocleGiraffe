using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Models
{
    public class Comment
    {
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        [JsonProperty("comment")]
        public string CommentText { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("author_id")]
        public Int64 AuthorId { get; set; }

        [JsonProperty("on_album")]
        public bool OnAlbum { get; set; }

        [JsonProperty("album_cover")]
        public object AlbumCover { get; set; }

        [JsonProperty("ups")]
        public Int64 Ups { get; set; }

        [JsonProperty("downs")]
        public Int64 Downs { get; set; }

        [JsonProperty("points")]
        public Int64 Points { get; set; }

        [JsonProperty("datetime")]
        public Int64 Datetime { get; set; }

        [JsonProperty("parent_id")]
        public Int64 ParentId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("vote")]
        public string Vote { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("children")]
        public IList<Comment> Children { get; set; }
    }
}
