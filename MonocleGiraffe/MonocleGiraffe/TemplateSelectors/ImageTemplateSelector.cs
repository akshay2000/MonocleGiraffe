using MonocleGiraffe.Models;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MonocleGiraffe.TemplateSelectors
{
    public class ImageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate AlbumTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if ((item as IGalleryItem).ItemType == GalleryItemType.Album)
                return AlbumTemplate;
            else //if ((item as GalleryItem).ItemType == GalleryItemType.Image)
                return ImageTemplate;
        }
    }
}
