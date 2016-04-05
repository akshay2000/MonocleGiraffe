using MonocleGiraffe.Models;
using SharpImgur.APIWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{
    public class SubGalleryPageViewModel : ViewModelBase
    {
        public SubGalleryPageViewModel()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                // design-time experience
            }
            else
            {
                // runtime experience
            }
        }


        ObservableCollection<GalleryItem> images = default(ObservableCollection<GalleryItem>);
        public ObservableCollection<GalleryItem> Images { get { return images; } set { Set(ref images, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // restore state
                state.Clear();
            }
            else
            {
                var sub = BootStrapper.Current.SessionState[(string)parameter] as SubredditItem;
                Images = new ObservableCollection<GalleryItem>();
                var gallery = await Gallery.GetSubreddditGallery(sub.Url);
                foreach(var i in gallery)
                {
                    Images.Add(new GalleryItem(i));
                }
            }
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // save state
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}
