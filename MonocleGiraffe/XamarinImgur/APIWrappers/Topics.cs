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
    public class Topics
    {
        private readonly NetworkHelper networkHelper;

        public Topics(NetworkHelper networkHelper)
        {
            this.networkHelper = networkHelper;
        }

        public async Task<Response<List<Image>>> GetTopicGallery(int topicId, Sort? sort = null, Window? window = null, int? page = null)
        {
            //{topicId}/{sort}/{window}/{page}
            string uri = "topics/" + topicId;
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

        public async Task<Response<List<Image>>> GetTopicGallery(int topicId, Sort? sort = null, int? page = null)
        {
            //{topicId}/{sort}/{page}
            string uri = "topics/" + topicId;
            if (sort != null)
            {
                if (page != null)
                {
                    uri += "/" + page;
                }
            }
            return await networkHelper.GetRequest<List<Image>>(uri);
        }

        public async Task<Response<List<Topic>>> GetDefaultTopics()
        {
            string uri = "topics/defaults";
            return await networkHelper.GetRequest<List<Topic>>(uri, true);
        }
    }
}
