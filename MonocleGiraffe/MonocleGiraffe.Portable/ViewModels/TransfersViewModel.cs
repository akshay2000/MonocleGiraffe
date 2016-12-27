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
        public TransfersViewModel(DownloadsViewModel downloadsVM, UploadsViewModel uploadsVM)
        {
            DownloadsVM = downloadsVM;
            UploadsVM = uploadsVM;
        }

        private DownloadsViewModel downloadsVM;
        public DownloadsViewModel DownloadsVM
        {
            get { return downloadsVM; }
            set { Set(ref downloadsVM, value); }
        }

        public UploadsViewModel uploadsVM;
        public UploadsViewModel UploadsVM
        {
            get { return uploadsVM; }
            set { Set(ref uploadsVM, value); }
        }
    }
}
