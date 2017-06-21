using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.StartScreen;
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
    public sealed partial class SubGalleryPage : Page
    {
        public SubGalleryPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await Task.Delay(100);
            var sub = (DataContext as SubGalleryPageViewModel).Sub;
            string tileId = sub.Url;
            ToggleAppBarButton(!SecondaryTile.Exists(tileId));
        }

        public void ScrollMe(object sender, object parameter)
        {
            int selectedIndex = (int)((parameter as DependencyPropertyChangedEventArgs).NewValue);
            MainGridView.ScrollIntoView(MainGridView.Items[selectedIndex]);
        }

        private async void TogglePin(SubredditItem subreddit, Rect rect)
        {
            string tileId = subreddit.Url;
            if (SecondaryTile.Exists(tileId))
            {
                // Unpin
                SecondaryTile secondaryTile = new SecondaryTile(tileId);
                Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above;
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(rect, placement);
                ToggleAppBarButton(isUnpinned);
            }
            else
            {
                string displayName = $"/r/{subreddit.Url}";
                string tileActivationArguments = tileId;
                Uri logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
                TileSize newTileDesiredSize = TileSize.Square150x150;

                SecondaryTile secondaryTile = new SecondaryTile(tileId, displayName, tileId, logo, newTileDesiredSize);

                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above;
                if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent(("Windows.Phone.UI.Input.HardwareButtons"))))
                {
                    bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(rect, placement);
                    ToggleAppBarButton(!isPinned);
                }

                //Phone
                if ((Windows.Foundation.Metadata.ApiInformation.IsTypePresent(("Windows.Phone.UI.Input.HardwareButtons"))))
                {
                    await secondaryTile.RequestCreateAsync();
                }
            }
        }

        private void ToggleAppBarButton(bool showPinButton)
        {
            if (showPinButton)
            {
                TogglePinButton.Label = "Pin to Start";
                TogglePinButton.Icon = new SymbolIcon(Symbol.Pin);
            }
            else
            {
                TogglePinButton.Label = "Unpin from Start";
                TogglePinButton.Icon = new SymbolIcon(Symbol.UnPin);
            }
            TogglePinButton.UpdateLayout();
        }

        private static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private void TogglePinButton_Click(object sender, RoutedEventArgs e)
        {
            Rect rect = GetElementRect((FrameworkElement)sender);
            TogglePin(((FrameworkElement)sender).DataContext as SubredditItem, rect);
        }
    }
}
