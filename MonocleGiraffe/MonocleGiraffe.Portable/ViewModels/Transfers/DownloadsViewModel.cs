using GalaSoft.MvvmLight.Command;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.ViewModels.Transfers
{
    public class DownloadsViewModel : BindableBase
    {
        private Func<string, Task<IDownloadItem>> downloadItemFactory;
        public DownloadsViewModel(Func<string, Task<IDownloadItem>> downloadItemFactory, bool isInDesignMode)
        {
            this.downloadItemFactory = downloadItemFactory;
            if (isInDesignMode)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        ObservableCollection<IDownloadItem> downloads = default(ObservableCollection<IDownloadItem>);
        public ObservableCollection<IDownloadItem> Downloads { get { return downloads; } set { Set(ref downloads, value); } }

        bool isCancelAllEnabled = default(bool);
        public bool IsCancelAllEnabled { get { return isCancelAllEnabled; } set { Set(ref isCancelAllEnabled, value); } }

        private async Task Init()
        {
            IsCancelAllEnabled = false;
            Downloads = new ObservableCollection<IDownloadItem>();
            await LoadExistingDownloads();
            if (Downloads.Count > 0)
                IsCancelAllEnabled = true;
        }

        protected virtual Task LoadExistingDownloads()
        {
            return Task.FromResult(0);
        }

        RelayCommand cancelAllCommand;
        public RelayCommand CancelAllCommand
           => cancelAllCommand ?? (cancelAllCommand = new RelayCommand(async () =>
           {
               await CancelAll();
           }));

        private async Task CancelAll()
        {
            IsCancelAllEnabled = false;
            List<Task> all = new List<Task>();
            foreach (var d in Downloads)
            {
                if (d.State != DownloadStates.CANCELED)
                    all.Add(d.Cancel());
            }
            await Task.WhenAll(all);
            IsCancelAllEnabled = true;
        }

        public async Task StartDownload(string url)
        {
            IDownloadItem item = await downloadItemFactory(url);
            Downloads.Add(item);
            await item.Start();
            if (Downloads.Count > 0)
                IsCancelAllEnabled = true;
        }
        
        protected virtual void InitDesignTime()
        {
            Downloads = new ObservableCollection<IDownloadItem>();
        }
    }
}
    