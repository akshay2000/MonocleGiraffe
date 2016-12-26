using Newtonsoft.Json.Linq;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinImgur.Interfaces;

namespace XamarinImgur.APIWrappers
{
    public class Images
    {
        private readonly NetworkHelper networkHelper;

        public Images(NetworkHelper networkHelper)
        {
            this.networkHelper = networkHelper;
        }

        public async Task<Response<Image>> UploadImage(string base64image, CancellationToken ct, IProgress<HttpProgress> progress, string title = null, string description = null, string albumId = null, string type = null)
        {
            JObject payload = new JObject();
            payload["image"] = base64image;
            if (albumId != null) payload["album"] = albumId;
            payload["type"] = type ?? "base64";
            if (title != null) payload["title"] = title;
            if (description != null) payload["description"] = description;
            string uri = "upload";
            return await networkHelper.PostRequest<Image>(uri, payload, ct, progress);
        }
        
        public async Task<Response<bool>> UpdateImage(string id, string title, string description)
        {
            string uri = $"image/{id}";
            JObject payload = new JObject();
            payload["title"] = title;
            payload["description"] = description;
            return await networkHelper.PostRequest<bool>(uri, payload);
        }

        public async Task<Response<bool>> DeleteImage(string id)
        {
            string uri = $"image/{id}";
            return await networkHelper.DeleteRequest<bool>(uri);
        }
    }
}
