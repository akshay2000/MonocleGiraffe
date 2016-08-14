using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace MonocleGiraffe.Models
{
    public class DownloadItem : BindableBase, IDownloadItem
    {
        public static async Task<DownloadItem> Create(BackgroundDownloader b, string url)
        {
            DownloadItem item = new DownloadItem();
            item.Downloader = b;
            item.Url = url;
            await item.Construct();   
            return item;
        }
      
        public static async Task<DownloadItem> Create(DownloadOperation op)
        {
            DownloadItem item = new DownloadItem();
            item.Downloader = new BackgroundDownloader();
            item.Url = op.RequestedUri.OriginalString;
            await item.Construct(op);
            return item;
        }

        private async Task Construct(DownloadOperation op = null)
        {
            var folder = KnownFolders.SavedPictures;
            var fileName = Url.Split('/').Last();
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            if ((await file.GetBasicPropertiesAsync()).Size > 0)
                file = await folder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            File = file;
            Name = file.Name;
            Operation = op ?? Downloader.CreateDownload(new Uri(Url), file);
            Progress = new Progress<DownloadOperation>(HandleProgress);
            CancellationToken = new CancellationTokenSource();
        }

        public const string DOWNLOADING = "Downloading";
        public const string CANCELED = "Canceled";
        public const string SUCCESSFUL = "Successful";
        public const string ERROR = "Error";
        public const string PAUSED = "Paused";
        public const string PENDING = "Pending";

        string name = default(string);
        public string Name { get { return name; } set { Set(ref name, value); } }

        ulong totalSize = 100;
        public ulong TotalSize { get { return totalSize; } set { Set(ref totalSize, value); } }

        ulong currentSize = 0;
        public ulong CurrentSize { get { return currentSize; } set { Set(ref currentSize, value); } }

        string state = PENDING;
        public string State { get { return state; } set { Set(ref state, value); } }

        private DownloadOperation Operation { get; set; }
        private Progress<DownloadOperation> Progress { get; set; }
        private CancellationTokenSource CancellationToken { get; set; }
        private StorageFile File { get; set; }
        private BackgroundDownloader Downloader { get; set; }
        private string Url { get; set; }

        public void Pause()
        {
            Operation.Pause();
        }

        DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand
           => cancelCommand ?? (cancelCommand = new DelegateCommand(async () =>
           {
               await Cancel();
           }));

        public async Task Cancel()
        {
            State = CANCELED;
            CancellationToken.Cancel();
            await File.DeleteAsync();
        }

        DelegateCommand restartCommand;
        public DelegateCommand RestartCommand
           => restartCommand ?? (restartCommand = new DelegateCommand(async () =>
           {
               await Restart();
           }));

        public async Task Restart()
        {
            State = PENDING;
            await Construct();
            await Start();
        }

        public async Task Start()
        {
            try
            {
                await Operation.StartAsync().AsTask(CancellationToken.Token, Progress);
            }
            catch (TaskCanceledException)
            {
                State = CANCELED;
            }
        }
               
        private void HandleProgress(DownloadOperation op)
        {
            BackgroundDownloadProgress currentProgress = op.Progress;
            TotalSize = currentProgress.TotalBytesToReceive;
            CurrentSize = currentProgress.BytesReceived;
            switch (currentProgress.Status)
            {
                case BackgroundTransferStatus.Canceled:
                    State = CANCELED;
                    break;
                case BackgroundTransferStatus.Completed:
                    State = SUCCESSFUL;
                    break;
                case BackgroundTransferStatus.Running:
                    State = DOWNLOADING;
                    break;
                case BackgroundTransferStatus.Error:
                    State = ERROR;
                    break;
                default:
                    State = PAUSED;
                    break;                    
            }
            if (TotalSize == CurrentSize)
                State = SUCCESSFUL;
        }
    }
}
