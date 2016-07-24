using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Models
{
    public class AccountSettings
    {
        [JsonProperty("account_url")]
        public string AccountUrl { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("high_quality")]
        public bool HighQuality { get; set; }

        [JsonProperty("public_images")]
        public bool PublicImages { get; set; }

        [JsonProperty("album_privacy")]
        public string AlbumPrivacy { get; set; }

        [JsonProperty("pro_expiration")]
        public bool ProExpiration { get; set; }

        [JsonProperty("accepted_gallery_terms")]
        public bool AcceptedGalleryTerms { get; set; }

        [JsonProperty("active_emails")]
        public IList<string> ActiveEmails { get; set; }

        [JsonProperty("messaging_enabled")]
        public bool MessagingEnabled { get; set; }

        [JsonProperty("comment_replies")]
        public bool CommentReplies { get; set; }

        [JsonProperty("blocked_users")]
        public IList<object> BlockedUsers { get; set; }

        [JsonProperty("show_mature")]
        public bool ShowMature { get; set; }

        [JsonProperty("newsletter_subscribed")]
        public bool NewsletterSubscribed { get; set; }
    }
}
