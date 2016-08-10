using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Models
{
    public class GalleryMetaInfo
    {
        public IEnumerable<IGalleryItem> Gallery { get; set; }

        public int SelectedIndex { get; set; }
    }
}
