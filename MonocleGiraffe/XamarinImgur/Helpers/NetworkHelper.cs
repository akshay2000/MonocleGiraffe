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
using static XamarinImgur.Helpers.Initializer;

namespace XamarinImgur.Helpers
{
    public static class NetworkHelper
    {        
        #region Imgur

        private static IHttpClient httpClient;
        private static IHttpClient authHttpClient;
        private static string baseURI = "https://imgur-apiv3.p.mashape.com/3/";
        private static string imgurBaseURI = "https://api.imgur.com/3/";

        //[Obsolete("Use GetRequest instead")]
        public static async Task<JObject> ExecuteRequest(string url, bool? isNative = null)
        {
            isNative = isNative ?? !IsCommercial;
            Uri finalUrl = BuildUri(url, (bool)isNative);
            IHttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();            
            string response = await httpClient.GetAsync(finalUrl);
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        public static async Task<Response<T>> GetRequest<T>(string url, bool? isNative = null) where T : new()
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

        public static async Task<Response<T>> DeleteRequest<T>(string relativeUri, bool? isNative = null) where T : new()
        {
            isNative = isNative ?? !IsCommercial;
            Uri uri = BuildUri(relativeUri, (bool)isNative);
            IHttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            Response<T> response = new Response<T>();
            try
            {
                string res = await httpClient.DeleteAsync(uri);
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

        public static async Task<Response<T>> PostRequest<T>(string url, JObject payload, CancellationToken ct, IProgress<HttpProgress> progress, bool? isNative = null) where T : new()
        {
            Response<T> response = new Response<T>();
            try
            {
                JObject o = await ExecutePostRequest(url, payload, isNative, ct, progress);
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

        public static async Task<Response<T>> PostRequest<T>(string url, JObject payload, bool? isNative = null) where T : new()
        {
            return await PostRequest<T>(url, payload, CancellationToken.None, null, isNative);
        }

        public static async Task<JObject> ExecutePostRequest(string url, JObject payload, bool? isNative, CancellationToken ct = default(CancellationToken), IProgress<HttpProgress> progress = null)
        {
            isNative = isNative ?? !IsCommercial;
            Uri uri = BuildUri(url, (bool)isNative);
            IHttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            string response = await httpClient.PostAsync(uri, payload.ToString(), ct, progress);
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

        private static async Task<IHttpClient> GetClient()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.Invoke();
                JObject config = await SecretsHelper.GetConfiguration();
                httpClient.SetDefaultRequestHeader("Authorization", $"Client-ID {(string)config["Client_Id"]}");
                if (IsCommercial)
                    httpClient.SetDefaultRequestHeader("X-Mashape-Key", (string)config["Mashape_Key"]);
            }
            return httpClient;
        }

        private static async Task<IHttpClient> GetAuthClient()
        {
            if (authHttpClient == null)
            {
                authHttpClient = HttpClientFactory.Invoke();
                JObject config = await SecretsHelper.GetConfiguration();
                if (IsCommercial)
                    authHttpClient.SetDefaultRequestHeader("X-Mashape-Key", (string)config["Mashape_Key"]);
                try
                {
                    authHttpClient.SetDefaultRequestHeader("Authorization", $"Bearer {await SecretsHelper.GetAccessToken()}");
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
            IHttpClient httpClient = GetRedditClient();
            string response = "{}";
            try
            {
                response = await httpClient.GetAsync(new Uri(url));
            }
            catch
            {
                Debug.WriteLine("Netwrok Error!");
            }
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        private static IHttpClient redditClient;
        private static IHttpClient GetRedditClient()
        {
            if (redditClient == null)
                redditClient = HttpClientFactory.Invoke();
            return redditClient;
        }

        #endregion
    }
}
