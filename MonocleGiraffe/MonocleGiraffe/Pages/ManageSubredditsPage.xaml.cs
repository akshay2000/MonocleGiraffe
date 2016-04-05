using MonocleGiraffe.Controls;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageSubredditsPage : Page
    {
        private bool isNavigatingForward = false;
        public ManageSubredditsPage()
        {
            this.InitializeComponent();
            DataContext = StateHelper.ViewModel;
            SubredditsListView.SelectedIndex = 0;
        }

        private void SubredditWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!isNavigatingForward)
            {
                StateHelper.ViewModel.SaveSubreddits();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SubredditsListView.SelectedItem as SubredditItem;
            StateHelper.ViewModel.RemoveSubreddit(item);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SubredditsListView.SelectedItem as SubredditItem;
            item.Url = NameTextBox.Text;
            item.Title = FriendlyNameTextBox.Text;
            StateHelper.ViewModel.SaveSubreddits();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddSubredditContentDialog dialog = new AddSubredditContentDialog();
            dialog.ShowAsync();
        }
    }
}
