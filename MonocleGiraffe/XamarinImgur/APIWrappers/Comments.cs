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
    public class Comments
    {
        private readonly NetworkHelper networkHelper;

        public Comments(NetworkHelper networkHelper)
        {
            this.networkHelper = networkHelper;
        }

        public async Task<Response<bool>> Vote(long commentId, string vote)
        {
            string url = $"comment/{commentId}/vote/{vote}";
            return await networkHelper.PostRequest<bool>(url, new JObject());
        }

        public async Task<Response<long?>> CreateComment(string comment, string imageId, long? parentId = null)
        {
            JObject payload = new JObject();
            payload["image_id"] = imageId;
            payload["comment"] = comment;
            if (parentId != null)
                payload["parent_id"] = parentId;
            string url = "comment";
            var response = await networkHelper.PostRequest<dynamic>(url, payload);
            Response<long?> ret = new Response<long?> { Content = (long?)((JObject)response.Content)["id"], Error = response.Error, IsError = response.IsError, Message = response.Message };
            return ret;
        }
    }
}
