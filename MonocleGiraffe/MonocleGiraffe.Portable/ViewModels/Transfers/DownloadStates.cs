using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.ViewModels.Transfers
{
    public class DownloadStates
    {
        public const string DOWNLOADING = "Downloading";
        public const string CANCELED = "Canceled";
        public const string SUCCESSFUL = "Successful";
        public const string ERROR = "Error";
        public const string PAUSED = "Paused";
        public const string PENDING = "Pending";
    }
}
