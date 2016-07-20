using Newtonsoft.Json.Linq;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XamarinImgur.APIWrappers
{
    public static class Images
    {
        public static async Task<Response<Image>> UploadImage(string base64image, string title = null, string description = null, string albumId = null, string type = null)
        {
            JObject payload = new JObject();
            payload["image"] = base64image;
            if (albumId != null) payload["album"] = albumId;
            if (type != null) payload["type"] = type;
            if (title != null) payload["title"] = title;
            if (description != null) payload["description"] = description;
            string uri = "upload";
            return await NetworkHelper.PostRequest<Image>(uri, payload);
        }
        
        public static async Task<Response<bool>> UpdateImage(string id, string title, string description)
        {
            string uri = $"image/{id}";
            JObject payload = new JObject();
            payload["title"] = title;
            payload["description"] = description;
            return await NetworkHelper.PostRequest<bool>(uri, payload);
        }

        public static async Task<Response<bool>> DeleteImage(string id)
        {
            string uri = $"image/{id}";
            return await NetworkHelper.DeleteRequest<bool>(uri);
        }
    }
}
