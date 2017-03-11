using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.Portable.ViewModels.Transfers;
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
    public class DownloadItem : Portable.Models.DownloadItem
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

        public override async Task Cancel()
        {
            State = TransferStates.CANCELED;
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
            State = TransferStates.PENDING;
            await Construct();
            await Start();
        }

        public override async Task Start()
        {
            try
            {
                await Operation.StartAsync().AsTask(CancellationToken.Token, Progress);
            }
            catch (TaskCanceledException)
            {
                State = TransferStates.CANCELED;
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
                    State = TransferStates.CANCELED;
                    break;
                case BackgroundTransferStatus.Completed:
                    State = TransferStates.SUCCESSFUL;
                    break;
                case BackgroundTransferStatus.Running:
                    State = TransferStates.DOWNLOADING;
                    break;
                case BackgroundTransferStatus.Error:
                    State = TransferStates.ERROR;
                    break;
                default:
                    State = TransferStates.PAUSED;
                    break;                    
            }
            if (TotalSize == CurrentSize)
                State = TransferStates.SUCCESSFUL;
        }
    }
}
