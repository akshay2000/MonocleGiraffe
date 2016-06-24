using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;

namespace MonocleGiraffe.ViewModels.Transfers
{
    public class DownloadsViewModel : BindableBase
    {
        public DownloadsViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        ObservableCollection<DownloadItem> downloads = default(ObservableCollection<DownloadItem>);
        public ObservableCollection<DownloadItem> Downloads { get { return downloads; } set { Set(ref downloads, value); } }

        private void Init()
        {
            
        }

        private void InitDesignTime()
        {
            Downloads = new ObservableCollection<DownloadItem>();
            Downloads.Add(new DownloadItem { TotalSize = 100, CurrentSize = 50, Name = "Unhand me woman", State = DownloadItem.DOWNLOADING });
            Downloads.Add(new DownloadItem { TotalSize = 100, CurrentSize = 70, Name = "Learn Python for Real", State = DownloadItem.CANCELLED });
            Downloads.Add(new DownloadItem { TotalSize = 100, CurrentSize = 20, Name = "The full story", State = DownloadItem.SUCCESSFUL });
            Downloads.Add(new DownloadItem { TotalSize = 100, CurrentSize = 90, Name = "Goodbye EU", State = DownloadItem.DOWNLOADING });
        }
    }
}
