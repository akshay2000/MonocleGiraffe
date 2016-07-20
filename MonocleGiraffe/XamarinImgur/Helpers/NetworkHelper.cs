using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinImgur.Interfaces;
using XamarinImgur.Models;

namespace XamarinImgur.Helpers
{
    public static class NetworkHelper
    {        
        #region Imgur

        private static HttpClient httpClient;
        private static HttpClient authHttpClient;
        private static string baseURI = "https://imgur-apiv3.p.mashape.com/3/";
        private static string imgurBaseURI = "https://api.imgur.com/3/";

        //[Obsolete("Use GetRequest instead")]
        public static async Task<JObject> ExecuteRequest(string url, bool isNative = false)
        {
#if DEBUG
            isNative = true;
#endif
            Uri finalUrl = BuildUri(url, isNative);
            HttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            var r = await httpClient.GetAsync(finalUrl);
            string response = await r.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        public static async Task<Response<T>> GetRequest<T>(string url, bool isNative = false) where T : new()
        {
            Response<T> response = new Response<T>();
            try
            {
                JObject o = await ExecuteRequest(url, isNative);
                if ((bool)o["success"])
                    response.Content = o["data"].ToObject<T>();
                else
                {
                    response.IsError = true;
                    response.Message = o["data"]["error"].ToString();
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Error = ex;
            }
            return response;
        }

        public static async Task<Response<T>> DeleteRequest<T>(string relativeUri, bool isNative = false) where T : new()
        {
#if DEBUG
            isNative = true;
#endif
            Uri uri = BuildUri(relativeUri, isNative);
            HttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            Response<T> response = new Response<T>();
            try
            {
                var r = await httpClient.DeleteAsync(uri);
                string res = await r.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(res);
                if ((bool)o["success"])
                    response.Content = o["data"].ToObject<T>();
                else
                {
                    response.IsError = true;
                    response.Message = o["data"]["error"].ToString();
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Error = ex;
            }
            return response;
        }

        public static async Task<Response<T>> PostRequest<T>(string url, JObject payload, bool isNative = false) where T : new()
        {
            Response<T> response = new Response<T>();
            try
            {
                JObject o = await ExecutePostRequest(url, payload, isNative);
                if ((bool)o["success"])
                    response.Content = o["data"].ToObject<T>();
                else
                {
                    response.IsError = true;
                    response.Message = o["data"]["error"].ToString();
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Error = ex;
            }
            return response;
        }
        
        public static async Task<JObject> ExecutePostRequest(string url, JObject payload, bool isNative)
        {
            Uri uri = BuildUri(url, isNative);
            HttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            string response = "{}";
            HttpContent content = new StringContent(payload.ToString(), Encoding.Unicode, "application/json");
            var r = await httpClient.PostAsync(uri, content);//.AsTask(ct, progress);
            response = await r.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        private static Uri BuildUri(string url, bool isNative)
        {
            string finalUrl;
            if (url.StartsWith("http"))
                finalUrl = url;
            else
                finalUrl = isNative ? imgurBaseURI + url : baseURI + url;
            return new Uri(finalUrl);
        }

        internal static void FlushHttpClients()
        {
            httpClient = authHttpClient = null;
        }

        private static async Task<HttpClient> GetClient()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
                JObject config = await SecretsHelper.GetConfiguration();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {(string)config["Client_Id"]}");
                httpClient.DefaultRequestHeaders.Add("X-Mashape-Key", (string)config["Mashape_Key"]);
            }
            return httpClient;
        }

        private static async Task<HttpClient> GetAuthClient()
        {
            if (authHttpClient == null)
            {
                authHttpClient = new HttpClient();
                JObject config = await SecretsHelper.GetConfiguration();
                authHttpClient.DefaultRequestHeaders.Add("X-Mashape-Key", (string)config["Mashape_Key"]);
                try
                {
                    authHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {await SecretsHelper.GetAccessToken()}");
                }
                catch
                {
                    return await GetClient();
                }
            }
            return authHttpClient;
        }

        #endregion

        #region Reddit

        public static async Task<JObject> ExecuteRedditRequest(string url)
        {
            HttpClient httpClient = GetRedditClient();
            string response = "{}";
            try
            {
                response = await httpClient.GetStringAsync(new Uri(url));
            }
            catch
            {
                Debug.WriteLine("Netwrok Error!");
            }
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        private static HttpClient redditClient;
        private static HttpClient GetRedditClient()
        {
            if (redditClient == null)
            {
                redditClient = new HttpClient();
            }
            return redditClient;
        }

        #endregion
    }
}
