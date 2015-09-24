using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using SharpImgur.APIWrappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonocleGiraffe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<string> subreddits = new ObservableCollection<string>();
        List<string> subredditsList = new List<string>() { "EarthPorn", "Aww", "Funny", "Pics", "GIFs" };
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            DataContext = StateHelper.ViewModel;
            SubredditsListView.ItemsSource = subreddits;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ImagesGridView.SelectedIndex = -1;

            if (e.NavigationMode == NavigationMode.New)
            {
                LoadGallery();
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ImagesGridView.ScrollIntoView(StateHelper.ViewModel.ImageItems[StateHelper.ViewModel.SelectedIndex]);
            }
        }

        private async void LoadGallery()
        {
            StateHelper.ViewModel.ImageItems.Clear();
            var gallery = await Gallery.GetGallery(Gallery.Section.Hot, Gallery.Sort.Viral, Gallery.Window.Day, true, 0);
            foreach (var image in gallery)
            {
                StateHelper.ViewModel.ImageItems.Add(await GalleryItem.New(image));
            }
        }

        private async void LoadSubreddit(string subreddit)
        {
            StateHelper.ViewModel.ImageItems.Clear();
            var subredditGallery = await Gallery.GetSubreddditGallery(subreddit);
            foreach (var image in subredditGallery)
            {
                StateHelper.ViewModel.ImageItems.Add(await GalleryItem.New(image));
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
            StateHelper.ViewModel.SelectedIndex = ImagesGridView.SelectedIndex;
            Frame.Navigate(typeof(FlipViewPage));
        }

        private async void SubredditsButton_Click(object sender, RoutedEventArgs e)
        {
            if (subreddits.Count > 0)
            {
                subreddits.Clear();
            }
            else
            {
                foreach(string subreddit in subredditsList)
                {
                    subreddits.Add(subreddit);
                    await Task.Delay(50);
                }
            }
        }

        private void SubredditWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string subredditName = (sender as Grid).DataContext as string;
            PageHeaderTextBox.Text = subredditName;
            LoadSubreddit(subredditName.ToLower());
            subreddits.Clear();
            MainSplitView.IsPaneOpen = false;
        }
    }
}
