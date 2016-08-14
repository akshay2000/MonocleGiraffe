using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Models
{
    public interface IDownloadItem
    {
        string State { get; set; }
        string Name { get; set; }
        ulong TotalSize { get; set; }
        ulong CurrentSize { get; set; }

        Task Cancel();
        Task Start();
    }
}
