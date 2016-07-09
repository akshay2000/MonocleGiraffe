using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MonocleGiraffe.Models
{
    public class UploadItem : BindableBase
    {
        public const string UPLOADING = "UPLOADING";
        public const string CANCELED = "Canceled";
        public const string SUCCESSFUL = "Successful";
        public const string ERROR = "Error";
        public const string PAUSED = "Paused";
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
                }
            }
        }

        private async void LoadThumbnail(StorageFile file)
        {
            var image = new BitmapImage();
            image.DecodePixelHeight = 72;
            image.DecodePixelWidth = 72;
            using (var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read))
            {
                await image.SetSourceAsync(stream);
            }
            Image = image;
        }

        BitmapImage image = default(BitmapImage);
        public BitmapImage Image { get { return image; } set { Set(ref image, value); } }

        string state = PENDING;
        public string State { get { return state; } set { Set(ref state, value); } }

        Image response = default(Image);
        public Image Response { get { return response; } set { Set(ref response, value); } }

        public async Task Upload()
        {
            State = UPLOADING;
            var response = await Images.UploadImage(file, null, null, Title, Description);
            if (response.IsError)
                State = ERROR;
            else
            {
                State = SUCCESSFUL;
                Response = response.Content;
            }
        }
    }
}
