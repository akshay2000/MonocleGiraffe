using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XamarinImgur.Interfaces
{
    public interface IHttpClient
    {
        Task<string> GetAsync(Uri uri);
        Task<string> DeleteAsync(Uri uri);
        Task<string> PostAsync(Uri uri, string content, CancellationToken ct, IProgress<HttpProgress> progress);
        void SetDefaultRequestHeader(string key, string value);
     }
}
