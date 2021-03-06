﻿using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Helpers;
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

        private SubGalleryPageViewModel Vm
        {
            get
            {
                return DataContext as SubGalleryPageViewModel;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ToggleAppBarButton(true);
            if (Vm == null)
                await Task.Delay(100);
            Vm.PropertyChanged += Vm_PropertyChanged;
        }
        
        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Sub") return;
            string tileId = Vm.Sub.Url;
            ToggleAppBarButton(!SecondaryTile.Exists(tileId));
            Vm.PropertyChanged -= Vm_PropertyChanged;
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
                SecondaryTile secondaryTile = new SecondaryTile(tileId);
                Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above;
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(rect, placement);
                ToggleAppBarButton(isUnpinned);
            }
            else
            {
                string displayName = $"/r/{subreddit.Url}";
                string tileActivationArguments = $"subreddit://{tileId}";
                Uri logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
                TileSize newTileDesiredSize = TileSize.Square150x150;

                SecondaryTile secondaryTile = new SecondaryTile(tileId, displayName, tileActivationArguments, logo, newTileDesiredSize);

                secondaryTile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Square71x71Logo.png");
                secondaryTile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
                secondaryTile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.png");
                secondaryTile.VisualElements.Square44x44Logo = new Uri("ms-appx:///Assets/Square44x44Logo.png");

                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;
                secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;

                Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above;
                var tileManager = SimpleIoc.Default.GetInstance<TileManager>();
                if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent(("Windows.Phone.UI.Input.HardwareButtons"))))
                {
                    bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(rect, placement);
                    ToggleAppBarButton(!isPinned);
                }

                //Phone
                if ((Windows.Foundation.Metadata.ApiInformation.IsTypePresent(("Windows.Phone.UI.Input.HardwareButtons"))))
                {
                    //tileManager.ScheduleRedditTileUpdate(tileId, Vm.Images);
                    bool isPinned = await secondaryTile.RequestCreateAsync();
                    ToggleAppBarButton(!isPinned);                    
                }
                tileManager.UpdateRedditTile(tileId, Vm.Images);
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
