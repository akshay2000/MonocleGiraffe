using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MonocleGiraffe.Controls;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Portable.ViewModels;
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
    public sealed partial class BrowserPage : Page
    {
        public BrowserPage()
        {
            this.InitializeComponent();
            AdControl ad = SimpleIoc.Default.GetInstance<AdHelper>().Banner;
            ad.CloseTapped += Ad_CloseTapped;
            LayoutRoot.Children.Add(ad);
        }

        private void Ad_CloseTapped(object sender, RoutedEventArgs e)
        {
            (DataContext as BrowserViewModel)?.CloseAdCommand.Execute(null);
        }

        private void MainFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("GalleryBrowserItem");
        }
    }
}
