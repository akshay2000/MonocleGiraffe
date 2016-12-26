using Newtonsoft.Json.Linq;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.APIWrappers
{
    public class Accounts
    {
        private readonly NetworkHelper networkHelper;

        public Accounts(NetworkHelper networkHelper)
        {
            this.networkHelper = networkHelper;
        }

        public async Task<GalleryProfile> GetGalleryProfile(string userName)
        {
            const string urlPattern = "account/{0}/gallery_profile";
            string url = string.Format(urlPattern, userName);
            JObject result = await networkHelper.ExecuteRequest(url);
            return result["data"].ToObject<GalleryProfile>();
        }

        public async Task<Account> GetAccount(string userName)
        {
            const string urlPattern = "account/{0}";
            string url = string.Format(urlPattern, userName);
            JObject result = await networkHelper.ExecuteRequest(url);
            return result["data"].ToObject<Account>();
        }

        public async Task<List<Image>> GetImages(string userName)
        {
            const string urlPattern = "account/{0}/images/";
            string url = string.Format(urlPattern, userName);
            JObject result = await networkHelper.ExecuteRequest(url);
            return result["data"].ToObject<List<Image>>();
            //return new List<Image>();
        }

        public async Task<int> GetImageCount(string userName)
        {
            const string urlPattern = "account/{0}/images/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await networkHelper.ExecuteRequest(url);
            return result["data"].ToObject<int>();
        }

        public async Task<List<Album>> GetAlbums(string userName)
        {
            const string urlPattern = "account/{0}/albums/";
            string url = string.Format(urlPattern, userName);
            JObject result = await networkHelper.ExecuteRequest(url);
            return result["data"].ToObject<List<Album>>();
        }        

        public async Task<int> GetAlbumCount(string userName)
        {
            const string urlPattern = "account/{0}/albums/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await networkHelper.ExecuteRequest(url);
            return result["data"].ToObject<int>();
        }

        public async Task<List<Image>> GetFavourites(string userName)
        {
            string uri = $"account/{userName}/favorites";
            JObject result = await networkHelper.ExecuteRequest(uri);
            if (result.HasValues && (bool)result["success"])
                return result["data"].ToObject<List<Image>>();
            else
                return new List<Image>();
        }

        public async Task<AccountSettings> GetAccountSettings(string userName)
        {
            string uri = $"account/{userName}/settings";
            JObject result = await networkHelper.ExecuteRequest(uri);
            if (result.HasValues && (bool)result["success"])
                return result["data"].ToObject<AccountSettings>();
            else
                return null;
        }

        public async Task<Response<bool>> SaveAccountSettings(string userName, JObject payload)
        {
            string uri = $"account/{userName}/settings";
            return await networkHelper.PostRequest<bool>(uri, payload);
        }
    }
}
