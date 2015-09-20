using MonocleGiraffe.Models;
using MonocleGiraffe.ViewModels;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Helpers
{
    public static class StateHelper
    {
        //public static ObservableCollection<GalleryItem> CurrentGallery { get; set; } = new ObservableCollection<GalleryItem>();

        //public static int CurrentGalleryItemIndex { get; set; } = 0;

        public static MainViewModel ViewModel { get; set; } = new MainViewModel(new ObservableCollection<GalleryItem>());
    }
}
