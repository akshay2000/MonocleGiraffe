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
    public class DownloadItem : BindableBase
    {
        public static async Task<DownloadItem> Create(BackgroundDownloader b, string url)
        {
            DownloadItem item = new DownloadItem();
            var folder = KnownFolders.SavedPictures;
            var fileName = url.Split('/').Last();
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            item.Name = file.Name;
            item.Operation = b.CreateDownload(new Uri(url), file);
            item.Progress = new Progress<DownloadOperation>(item.HandleProgress);
            item.CancellationToken = new CancellationTokenSource();
            return item;
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

        public void Pause()
        {
            Operation.Pause();
        }

        public void Cancel()
        {
            CancellationToken.Cancel();
        }

        public async Task Start()
        {            
            await Operation.StartAsync().AsTask(CancellationToken.Token, Progress);
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
        }
    }
}
