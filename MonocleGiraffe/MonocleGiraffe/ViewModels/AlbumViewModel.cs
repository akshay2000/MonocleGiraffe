using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.ViewModels
{
    public class AlbumViewModel : NotifyBase
    {
        private GalleryItem albumItem;

        public GalleryItem AlbumItem
        {
            get { return albumItem; }
            set
            {
                if (albumItem != value)
                {
                    albumItem = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                }
            }
        }

    }
}
