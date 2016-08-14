using GalaSoft.MvvmLight;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.ViewModels.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class TransfersViewModel : ViewModelBase
    {
        public TransfersViewModel(DownloadsViewModel downloadsVM)
        {
            DownloadsVM = downloadsVM;
        }
        
        public DownloadsViewModel DownloadsVM { get; set; }
    }
}
