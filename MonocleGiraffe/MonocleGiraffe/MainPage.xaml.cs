using MonocleGiraffe.Controls;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using MonocleGiraffe.ViewModels;
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
        MainViewModel mainVM = StateHelper.ViewModel;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            DataContext = mainVM;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ImagesGridView.SelectedIndex = -1;

            if (e.NavigationMode == NavigationMode.New)
            {
                Init();
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ImagesGridView.ScrollIntoView(StateHelper.ViewModel.ImageItems[StateHelper.ViewModel.SelectedIndex]);
            }
        }

        private async void Init()
        {
            mainVM.LoadGallery();
            mainVM.LoadSubreddits();
            await Task.Delay(100);
            mainVM.LoadTopics();
        }        

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HomeMenuButton_Click(object sender, RoutedEventArgs e)
        {
            mainVM.LoadGallery();
            MainSplitView.IsPaneOpen = false;
        }

        private void ThumbnailWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StateHelper.ViewModel.SelectedIndex = ImagesGridView.SelectedIndex;
            Frame.Navigate(typeof(FlipViewPage));
        }

        private void SubredditsButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = true;
            SubredditsListView.Visibility = SubredditsListView.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SubredditWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Subreddit subreddit = (sender as Grid).DataContext as Subreddit;
            mainVM.LoadSubreddit(subreddit);
            MainSplitView.IsPaneOpen = false;
        }

        private void ImgurButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = true;
            TopicsListView.Visibility = TopicsListView.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void TopicWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SharpImgur.Models.Topic topic = (sender as FrameworkElement).DataContext as SharpImgur.Models.Topic;
            mainVM.LoadTopic(topic);
            MainSplitView.IsPaneOpen = false;
        }
      
        private void EditSubredditsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ManageSubredditsPage));
        }
    }
}
