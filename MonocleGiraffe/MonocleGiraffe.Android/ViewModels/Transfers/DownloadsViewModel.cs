using Android.Content;
using MonocleGiraffe.Android.Models;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Android.ViewModels.Transfers
{
    public class DownloadsViewModel : Portable.ViewModels.Transfers.DownloadsViewModel
    {
        //private Context context;
        public DownloadsViewModel(Context context) : base(async (u) => new Models.DownloadItem(u, context), false)
        {
          //  this.context = context;
        }

        //private static async Task<IDownloadItem> DownloadItemFactory(string url)
        //{
        //    return new Models.DownloadItem(url);
        //}
    }
}