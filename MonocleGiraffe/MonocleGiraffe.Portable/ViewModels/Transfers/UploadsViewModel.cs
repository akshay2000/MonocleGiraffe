using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.ViewModels.Transfers
{
    public class UploadsViewModel : BindableBase
    {
        private readonly INavigationService navigationService;

        public UploadsViewModel(INavigationService nav, bool isInDesignMode)
        {
            navigationService = nav;
            if (isInDesignMode)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        ObservableCollection<IUploadItem> uploads = default(ObservableCollection<IUploadItem>);
        public ObservableCollection<IUploadItem> Uploads { get { return uploads; } set { Set(ref uploads, value); } }

        SemaphoreSlim sem = new SemaphoreSlim(3);

        public async Task Enqueqe(IUploadItem item)
        {
            var name = item.Name;
            Debug.WriteLine($"Enqueqed upload {name} at {DateTime.Now}");
            Uploads.Add(item);
            await sem.WaitAsync();
            Debug.WriteLine($"Started upload {name} at {DateTime.Now}");
            await item.Upload();
            Debug.WriteLine($"Finished upload {name} at {DateTime.Now}");
            sem.Release();
        }

        protected void UploadTapped(IUploadItem item)
        {
            if (item.State != TransferStates.SUCCESSFUL)
                return;
            const string key = "ItemToEdit";
            StateHelper.SessionState[key] = item.Response;
            navigationService.NavigateTo(PageKeyHolder.EditItemPageKey, key);
        }

        RelayCommand cancelAllCommand;
        public RelayCommand CancelAllCommand
           => cancelAllCommand ?? (cancelAllCommand = new RelayCommand(() =>
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
            Uploads = new ObservableCollection<IUploadItem>();
            IsCancelAllEnabled = true;
        }

        protected virtual void InitDesignTime()
        {
            Uploads = new ObservableCollection<IUploadItem>();
            IsCancelAllEnabled = true;
        }
    }
}
