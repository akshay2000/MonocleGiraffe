using MonocleGiraffe.Controls;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.ViewModels;
using MonocleGiraffe.ViewModels.FrontPage;
using XamarinImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GoogleAnalytics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FrontPage : Page
    {
        public FrontPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            ChageSystemTrayColor();
            MainPivot.SelectionChanged += MainPivot_SelectionChanged;
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = MainPivot.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex > 3)
                return;
            var tracker = EasyTracker.GetTracker();
            string[] screenNames = new string[] { "Gallery", "Reddits", "Search", "Account" };
            tracker.SendView(screenNames[selectedIndex]);
        }

        private void ChageSystemTrayColor()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Color.FromArgb(1, 37, 37, 37);
                }
                LayoutRoot.Margin = new Thickness(0,-12,0,0);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            (sender.DataContext as SearchViewModel).SearchCommand.Execute("default");
        }        
    }
}
