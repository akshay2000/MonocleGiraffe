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
using Windows.UI;
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

        SemaphoreSlim sem = new SemaphoreSlim(3);

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

        private void Init()
        {
            Uploads = new ObservableCollection<UploadItem>();
        }

        private void InitDesignTime()
        {
        }
    }
}
