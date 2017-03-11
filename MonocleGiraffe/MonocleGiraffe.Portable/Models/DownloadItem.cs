using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.ViewModels.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Models
{
    public abstract class DownloadItem : BindableBase, IDownloadItem
    {
        string name = default(string);
        public string Name { get { return name; } set { Set(ref name, value); } }

        ulong totalSize = 100;
        public ulong TotalSize { get { return totalSize; } set { Set(ref totalSize, value); } }

        ulong currentSize = 0;
        public ulong CurrentSize { get { return currentSize; } set { Set(ref currentSize, value); } }

        string state = TransferStates.PENDING;
        public string State { get { return state; } set { Set(ref state, value); } }

        public abstract Task Cancel();

        public abstract Task Start();
    }
}
