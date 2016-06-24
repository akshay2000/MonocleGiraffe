using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.Models
{
    public class DownloadItem : BindableBase
    {
        public const string DOWNLOADING = "Downloading";
        public const string CANCELLED = "Cancelled";
        public const string SUCCESSFUL = "Successful";

        string name = default(string);
        public string Name { get { return name; } set { Set(ref name, value); } }
        
        long totalSize = default(long);
        public long TotalSize { get { return totalSize; } set { Set(ref totalSize, value); } }
        
        long currentSize = default(long);
        public long CurrentSize { get { return currentSize; } set { Set(ref currentSize, value); } }

        string state = DOWNLOADING;
        public string State { get { return state; } set { Set(ref state, value); } }        
    }
}
