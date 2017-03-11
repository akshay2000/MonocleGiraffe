using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using Android.Util;
using System.Diagnostics;
using FFImageLoading.Work;
using MonocleGiraffe.Portable.ViewModels.Transfers;
using Android.Content;
using static Android.Provider.MediaStore;
using Android.Provider;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using System.IO;
using MonocleGiraffe.Android.Helpers;

namespace MonocleGiraffe.Android.Models
{
    public class DownloadItem : Portable.Models.DownloadItem
    {
        private const string FolderName = "Monocle Giraffe/";

        private string url;
        private Context context;
        public DownloadItem(string url, Context context)
        {
            this.url = url;
            this.context = context;
        }

        public override Task Cancel()
        {
            State = TransferStates.CANCELED;
            return Task.CompletedTask;
        }

        public override Task Start()
        {            
            ImageService.Instance.LoadUrl(url)                
                .DownloadProgress(Progressed)
                .Success(Succeeded)
                .FileWriteFinished(WriteFinished)                
                .DownloadOnly();
            return Task.CompletedTask;
        }

        private async void Succeeded(ImageInformation info, LoadingResult result)
        {
            if (State != TransferStates.CANCELED)
                await InsertInGallery(info.FilePath);
        }

        private void Progressed(DownloadProgress progress)
        {
            TotalSize = Convert.ToUInt32(progress.Total);
            CurrentSize = Convert.ToUInt32(progress.Current);
        }

        private async void WriteFinished(FileWriteInfo info)
        {
            Log.Info("Downloader WriteFinished", $"File write finished to {info.FilePath}");
            Debug.WriteLine($"File write finished to {info.FilePath}");
            if (State != TransferStates.CANCELED)
                await InsertInGallery(info.FilePath);
        }

        private async Task InsertInGallery(string filePath)
        {
            var fileName = url.Split('/').Last();
            string extPath = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString();
            string outDirPath = Path.Combine(extPath, FolderName);
            Directory.CreateDirectory(outDirPath);
            string outFilePath = Path.Combine(outDirPath, fileName);
            await Utils.CopyFileAsync(filePath, outFilePath);

            ContentValues values = new ContentValues();

            values.Put(Images.ImageColumns.DateAdded, (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
            values.Put(Images.ImageColumns.MimeType, "image/jpeg");
            values.Put(MediaColumns.Data, outFilePath);

            var uri = Images.Media.ExternalContentUri;
            context.ContentResolver.Insert(uri, values);
        }
    }
}