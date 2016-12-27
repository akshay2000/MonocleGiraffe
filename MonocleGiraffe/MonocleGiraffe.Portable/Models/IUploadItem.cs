using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Models
{
    public interface IUploadItem
    {
        ulong? CurrentSize { get; set; }
        string Description { get; set; }
        //StorageFile File { get; set; }
        //BitmapImage Image { get; set; }
        string Message { get; set; }
        string Name { get; set; }
        GalleryItem Response { get; set; }
        //DelegateCommand RestartCommand { get; }
        string State { get; set; }
        string Title { get; set; }
        ulong? TotalSize { get; set; }

        void Cancel();
        Task Upload();
    }
}
