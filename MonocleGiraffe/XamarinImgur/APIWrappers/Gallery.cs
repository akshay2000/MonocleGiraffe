using Newtonsoft.Json.Linq;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XamarinImgur.APIWrappers.Enums;

namespace XamarinImgur.APIWrappers
{
    public class Gallery
    {
        private readonly NetworkHelper networkHelper;

        public Gallery(NetworkHelper networkHelper)
        {
            this.networkHelper = networkHelper;
        }

        public async Task<Response<List<Image>>> GetGallery(Section? section = null, Sort? sort = null, Window? window = null, bool? showViral = null, int? page = null)
        {
            string uri = "gallery";
            if (section != null)
            {
                uri += "/" + section.ToString().ToLower();
                if(sort != null)
                {
                    uri += "/" + sort.ToString().ToLower();
                    if(window != null)
                    {
                        uri += "/" + window.ToString().ToLower();
                        if (showViral != null)
                        {
                            uri += "/" + showViral.ToString();
                            if (page != null)
                            {
                                uri += "/" + page;
                            }
                        }
                    }
                }
            }
            return await networkHelper.GetRequest<List<Image>>(uri);
        }

        /// <summary>
        /// Gets the gallery
        /// </summary>
        /// <param name="section"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="showViral">Used only when section is "user"</param>
        /// <returns></returns>
        public async Task<Response<List<Image>>> GetGallery(Section? section = null, Sort? sort = null, int? page = null, bool? showViral = null)
        {
            string uri = "gallery";
            if (section != null)
            {
                uri += "/" + section.ToString().ToLower();
                if (sort != null)
                {
                    if (page != null)
                    {
                        uri += "/" + page;
                    }
                }                
            }
            if (showViral != null)
                uri += ("?showViral=" + showViral.ToString().ToLower());
            var response = await networkHelper.GetRequest<List<Image>>(uri);
            return response;
        }

        public async Task<Response<List<Image>>> GetSubreddditGallery(string subreddit, Sort? sort = null, Window? window = null, int? page = null)
        {
            //{ subreddit}/{ sort}/{ window}/{ page}
            string uri = "gallery/r/" + subreddit;
            if (sort != null)
            {
                uri += "/" + sort.ToString().ToLower();
                if (window != null)
                {
                    uri += "/" + window.ToString().ToLower();

                    if (page != null)
                    {
                        uri += "/" + page;
                    }
                }
            }
            return await networkHelper.GetRequest<List<Image>>(uri);
        }

        public async Task<Response<List<Image>>> GetSubreddditGallery(string subreddit, Sort? sort = null, int? page = null)
        {
            //{ subreddit}/{ sort}/{ window}/{ page}
            string uri = "gallery/r/" + subreddit;
            if (sort != null)
            {
                uri += "/" + sort.ToString().ToLower();
                if (page != null)
                {
                    uri += "/" + page;
                }
            }
            return await networkHelper.GetRequest<List<Image>>(uri);
        }

        public async Task<Response<List<Comment>>> GetComments(string imageId, Sort sort = Sort.Best)
        {
            //gallery/{id}/comments/{sort}
            string uri = "gallery/" + imageId + "/comments/" + sort.ToString().ToLower();
            return await networkHelper.GetRequest<List<Comment>>(uri);
        }

        public async Task<Response<List<Image>>> SearchGallery(string query, Sort? sort = null, int? page = null )
        {
            //gallery/search/{sort}/{page}
            string uri = "gallery/search";
            if (sort != null)
            {
                uri += "/" + sort.ToString().ToLower();
                if (page != null)
                {
                    uri += "/" + page;
                }
            }
            uri = $"{uri}?q={query}";
            return await networkHelper.GetRequest<List<Image>>(uri);
        }
            
        public async Task<Response<bool>> Vote(string id, string vote)
        {
            string urlString = $"gallery/{id}/vote/{vote}";
            return await networkHelper.PostRequest<bool>(urlString, new JObject());
        }

        private async Task<Response<bool>> Favourite(string id, string type)
        {
            string urlString = $"{type}/{id}/favorite";
            return await networkHelper.PostRequest<bool>(urlString, new JObject());
        }

        public async Task<Response<bool>> FavouriteImage(string id)
        {
            return await Favourite(id, "image");
        }

        public async Task<Response<bool>> FavouriteAlbum(string id)
        {
            return await Favourite(id, "album");
        }
    }
}
