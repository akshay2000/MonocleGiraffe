using SharpImgur.APIWrappers;
using SharpImgur.Helpers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonocleGiraffe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<string> thumbnails;
        public MainPage()
        {
            this.InitializeComponent();
            thumbnails = new ObservableCollection<string>();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            List<SharpImgur.Models.Image> gallery = await Gallery.GetGallery(Gallery.Section.Hot, Gallery.Sort.Viral, Gallery.Window.Day, true, 0);
            AddThumbnails(gallery, "b");
            ImagesGridView.ItemsSource = thumbnails;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HomeMenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddThumbnails(List<SharpImgur.Models.Image> images, string sizeSuffix = "s")
        {
            string baseUrl = "http://i.imgur.com/";
            foreach (var image in images)
            {
                if (image.IsAlbum)
                {
                    var albumImages = await Album.GetImages(image.Id);
                    string url = baseUrl + albumImages[0].Id + sizeSuffix + ".jpg";
                    thumbnails.Add(url);
                }
                else
                {
                    string url = baseUrl + image.Id + sizeSuffix + ".jpg";
                    thumbnails.Add(url);
                }
            }           
        }
    }
}
