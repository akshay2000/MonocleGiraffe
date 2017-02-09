using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamarinImgur.Interfaces;
using System.Net.Http;
using Xamarin.Android.Net;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class HttpClient : IHttpClient
    {
        public async Task<string> DeleteAsync(Uri uri)
        {
            var r = await Client.DeleteAsync(uri);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(Uri uri)
        {
            var r = await Client.GetAsync(uri);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(Uri uri, string content, CancellationToken ct, IProgress<HttpProgress> progress)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            if (progress != null)
                progress.Report(new HttpProgress { BytesSent = 0, TotalBytesToSend = 100, Stage = HttpProgressStage.SendingContent });            
            var r = await Client.PostAsync(uri, httpContent, ct);
            string ret = await r.Content.ReadAsStringAsync();
            if(progress != null)
                progress.Report(new HttpProgress { BytesSent = 100, TotalBytesToSend = 100, Stage = HttpProgressStage.None });
            return ret;
        }

        public void SetDefaultRequestHeader(string key, string value)
        {
            Client.DefaultRequestHeaders.Add(key, value);
        }

        private System.Net.Http.HttpClient client;
        private System.Net.Http.HttpClient Client
        {
            get
            {
                client = client ?? new System.Net.Http.HttpClient(new AndroidClientHandler());
                return client;
            }
        }
    }
}