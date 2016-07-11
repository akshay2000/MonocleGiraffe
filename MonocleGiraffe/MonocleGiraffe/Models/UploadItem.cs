using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace MonocleGiraffe.Models
{
    public class UploadItem : BindableBase
    {
        public const string UPLOADING = "Uploading";
        public const string CANCELED = "Canceled";
        public const string SUCCESSFUL = "Successful";
        public const string ERROR = "Error";
        public const string PENDING = "Pending";
        
        public string Title { get; set; }
        public string Description { get; set; }

        private StorageFile file;
        public StorageFile File
        {
            get { return file; }
            set
            {
                if (file != value)
                {
                    file = value;
                    LoadThumbnail(value);
                    Name = value.Name;
                }
            }
        }

        private async void LoadThumbnail(StorageFile file)
        {
            var image = new BitmapImage();
            image.DecodePixelHeight = 72;
            using (var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read))
            {
                await image.SetSourceAsync(stream);
            }
            Image = image;
        }

        BitmapImage image = default(BitmapImage);
        public BitmapImage Image { get { return image; } set { Set(ref image, value); } }

        string name = default(string);
        public string Name { get { return name; } set { Set(ref name, value); } }   

        string state = PENDING;
        public string State { get { return state; } set { Set(ref state, value); } }

        Image response = default(Image);
        public Image Response { get { return response; } set { Set(ref response, value); } }

        ulong? totalSize = default(ulong?);
        public ulong? TotalSize { get { return totalSize; } set { Set(ref totalSize, value); } }

        ulong? currentSize = default(ulong?);
        public ulong? CurrentSize { get { return currentSize; } set { Set(ref currentSize, value); } }

        string message = default(string);
        public string Message { get { return message; } set { Set(ref message, value); } }

        private CancellationTokenSource CTS { get; set; }
        private Progress<HttpProgress> Progress { get; set; }

        public async Task Upload()
        {
            if (State == CANCELED)
                return;
            State = UPLOADING;
            CTS = new CancellationTokenSource();
            await Task.Delay(5000);
            Progress = new Progress<HttpProgress>(HandleProgress);
            var response = await Images.UploadImage(file, CTS.Token, Progress, Title, Description);
            if (response.IsError)
            {
                if (response.Error is TaskCanceledException)
                {
                    State = CANCELED;
                    Message = "Upload was canceled";
                }
                else
                {
                    State = ERROR;
                    Message = string.IsNullOrEmpty(response.Message) ? response?.Error?.Message : response.Message;
                }
            }
            else
            {
                State = SUCCESSFUL;
                Response = response.Content;
                Message = "Tap here to edit";
            }
        }

        DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand
           => cancelCommand ?? (cancelCommand = new DelegateCommand(() =>
           {
               Cancel();
           }));

        public void Cancel()
        {
            if (State == CANCELED)
                return;
            State = CANCELED;
            CTS?.Cancel();
            Message = "Upload was canceled";
        }

        DelegateCommand restartCommand;
        public DelegateCommand RestartCommand
           => restartCommand ?? (restartCommand = new DelegateCommand(async () =>
           {
               await Restart();
           }));

        private async Task Restart()
        {
            State = PENDING;
            await Upload();
        }

        private void HandleProgress(HttpProgress progress)
        {
            TotalSize = progress.TotalBytesToSend;
            CurrentSize = progress.BytesSent;
            if (TotalSize == CurrentSize)
                State = SUCCESSFUL;
        }
    }
}
