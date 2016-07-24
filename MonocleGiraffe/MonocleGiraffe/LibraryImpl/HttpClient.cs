using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.LibraryImpl
{
    public class HttpClient : IHttpClient
    {
        private Dictionary<IProgress<Windows.Web.Http.HttpProgress>, IProgress<HttpProgress>> progresses;
        public HttpClient()
        {
            progresses = new Dictionary<IProgress<Windows.Web.Http.HttpProgress>, IProgress<HttpProgress>>();
        }

        public async Task<string> DeleteAsync(Uri uri)
        {
            var r = await Client.DeleteAsync(uri);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(Uri uri)
        {
            return await Client.GetStringAsync(uri);
        }

        public async Task<string> PostAsync(Uri uri, string content, CancellationToken ct, IProgress<HttpProgress> progress)
        {
            var httpContent = new Windows.Web.Http.HttpStringContent(content, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            Windows.Web.Http.HttpResponseMessage r;
            if (progress != null)
            {
                Progress<Windows.Web.Http.HttpProgress> httpProgress = new Progress<Windows.Web.Http.HttpProgress>();
                httpProgress.ProgressChanged += HttpProgress_ProgressChanged;
                progresses[httpProgress] = progress;
                r = await Client.PostAsync(uri, httpContent).AsTask(ct, httpProgress);
            }
            else
            {
                r = await Client.PostAsync(uri, httpContent).AsTask(ct);
            }
            return await r.Content.ReadAsStringAsync();
        }

        private void HttpProgress_ProgressChanged(object sender, Windows.Web.Http.HttpProgress e)
        {
            var key = (IProgress<Windows.Web.Http.HttpProgress>)sender;
            HttpProgress newProgress = new HttpProgress()
            {
                BytesReceived = e.BytesReceived,
                BytesSent = e.BytesSent,
                Retries = e.Retries,
                Stage = (HttpProgressStage)Enum.Parse(typeof(HttpProgressStage), e.Stage.ToString()),
                TotalBytesToReceive = e.TotalBytesToReceive,
                TotalBytesToSend = e.TotalBytesToSend
            };
            var value = progresses[key];
            value.Report(newProgress);
        }
        
        public void SetDefaultRequestHeader(string key, string value)
        {
            Client.DefaultRequestHeaders[key] = value;
        }

        private Windows.Web.Http.HttpClient client;
        private Windows.Web.Http.HttpClient Client
        {
            get
            {
                client = client ?? new Windows.Web.Http.HttpClient();
                return client;
            }
        }
    }
}
