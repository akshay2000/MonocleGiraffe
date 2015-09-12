using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
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
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ImagesGridView.SelectedIndex = -1;

            if (e.NavigationMode == NavigationMode.New)
            {
                ImagesGridView.ItemsSource = StateHelper.CurrentGallery;
                LoadGallery();
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ImagesGridView.ScrollIntoView(StateHelper.CurrentGallery[StateHelper.CurrentGalleryItemIndex]);
            }
        }

        private async void LoadGallery()
        {
            var gallery = await Gallery.GetGallery(Gallery.Section.Hot, Gallery.Sort.Viral, Gallery.Window.Day, true, 0);
            foreach (var image in gallery)
            {
                StateHelper.CurrentGallery.Add(await GalleryItem.New(image));
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HomeMenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThumbnailWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StateHelper.CurrentGalleryItemIndex = ImagesGridView.SelectedIndex;
            Frame.Navigate(typeof(FlipViewPage));
        }
    }
}
