using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels.Transfers
{
    public class UploadsViewModel : BindableBase
    {
        public UploadsViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        ObservableCollection<UploadItem> uploads = default(ObservableCollection<UploadItem>);
        public ObservableCollection<UploadItem> Uploads { get { return uploads; } set { Set(ref uploads, value); } }

        SemaphoreSlim sem = new SemaphoreSlim(1);

        public async Task Enqueqe(UploadItem item)
        {
            var name = item.File.DisplayName;
            Debug.WriteLine($"Enqueqed upload {name} at {DateTime.Now}");
            Uploads.Add(item);
            await sem.WaitAsync();
            Debug.WriteLine($"Started upload {name} at {DateTime.Now}");
            await item.Upload();
            Debug.WriteLine($"Finished upload {name} at {DateTime.Now}");
            sem.Release();
        }

        DelegateCommand cancelAllCommand;
        public DelegateCommand CancelAllCommand
           => cancelAllCommand ?? (cancelAllCommand = new DelegateCommand(() =>
           {
               CancelAll();
           }));

        bool isCancelAllEnabled = default(bool);
        public bool IsCancelAllEnabled { get { return isCancelAllEnabled; } set { Set(ref isCancelAllEnabled, value); } }

        private void CancelAll()
        {
            IsCancelAllEnabled = false;
            foreach (var u in Uploads)
            {
                u.Cancel();
            }
            IsCancelAllEnabled = true;
        }

        private void Init()
        {
            Uploads = new ObservableCollection<UploadItem>();
            IsCancelAllEnabled = true;
        }

        private void InitDesignTime()
        {
            Uploads = new ObservableCollection<UploadItem>();
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 50, Name = "Unhand me woman", State = UploadItem.PENDING, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 70, Name = "Learn Python for Real", State = UploadItem.CANCELED, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 20, Name = "The full story", State = UploadItem.SUCCESSFUL, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            Uploads.Add(new UploadItem { TotalSize = 100, CurrentSize = 90, Name = "Goodbye EU", State = UploadItem.UPLOADING, Image = new BitmapImage(new Uri("http://i.imgur.com/ngYg9yCb.jpg")) });
            IsCancelAllEnabled = true;
        }
    }
}
