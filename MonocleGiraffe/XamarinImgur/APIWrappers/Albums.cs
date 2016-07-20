using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Models;
using Newtonsoft.Json.Linq;
using XamarinImgur.Helpers;

namespace XamarinImgur.APIWrappers
{
    public class Albums
    {
        public static async Task<Album> GetAlbum(string albumId)
        {
            string uri = "album/" + albumId;
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            if (response.HasValues)
                return response["data"].ToObject<Album>();
            else
                return new Album();
        }

        public static async Task<List<Image>> GetImages(string albumId)
        {
            string uriString = $"album/{albumId}/images";
            JObject response = await NetworkHelper.ExecuteRequest(uriString);
            return response["data"].ToObject<List<Image>>();
        }

        public static async Task<Response<bool>> DeleteAlbum(string id)
        {
            string uri = $"album/{id}";
            return await NetworkHelper.DeleteRequest<bool>(uri);
        }

        public static async Task<Response<bool>> UpdateAlbum(string id, string[] ids = null, string title = null, string description = null, string privacy = null, string cover = null)
        {
            string uri = $"album/{id}";
            JObject payload = new JObject();
            if (ids != null) payload["ids"] = new JArray(ids);
            if (title != null) payload["title"] = title;
            if (description != null) payload["description"] = description;
            if (privacy != null) payload["privacy"] = privacy;
            if (cover != null) payload[cover] = cover;
            return await NetworkHelper.PostRequest<bool>(uri, payload);
        }
    }
}
