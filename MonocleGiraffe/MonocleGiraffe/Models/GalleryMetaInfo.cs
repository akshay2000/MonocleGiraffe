using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Models
{
    public class GalleryMetaInfo
    {
        public ObservableCollection<GalleryItem> Gallery { get; set; }

        public int SelectedIndex { get; set; }
    }
}
