using MonocleGiraffe.Models;
using MonocleGiraffe.ViewModels.FrontPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{
    public class FlipViewPageViewModel : ViewModelBase
    {
        private ObservableCollection<GalleryItem> images;
        public ObservableCollection<GalleryItem> Images
        {
            get { return images; }
            set { Set(ref images, value); }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { Set(ref selectedIndex, value); }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var param = (GalleryViewModel)BootStrapper.Current.SessionState[(string)parameter];
            Images = param.Images;
            SelectedIndex = param.ImageSelectedIndex;
            return base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
