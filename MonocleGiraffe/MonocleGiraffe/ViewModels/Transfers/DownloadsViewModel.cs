using MonocleGiraffe.Models;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.Networking.BackgroundTransfer;

namespace MonocleGiraffe.ViewModels.Transfers
{
    public class DownloadsViewModel : Portable.ViewModels.Transfers.DownloadsViewModel
    {
        public DownloadsViewModel() : base(DownloadItemFactory, DesignMode.DesignModeEnabled)
        { }

        public static async Task<IDownloadItem> DownloadItemFactory(string url)
        {
            return await DownloadItem.Create(Downloader, url);
        }

        private static BackgroundDownloader downloader;
        private static BackgroundDownloader Downloader
        {
            get
            {
                downloader = downloader ?? new BackgroundDownloader();
                return downloader;
            }
        }
    }
}
    