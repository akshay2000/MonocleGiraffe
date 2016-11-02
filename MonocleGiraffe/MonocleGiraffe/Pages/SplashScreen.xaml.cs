using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using GoogleAnalytics;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Portable.ViewModels;
using MonocleGiraffe.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashScreen : Page
    {
        public SplashScreen()
        {
            this.InitializeComponent();
            var tracker = EasyTracker.GetTracker();
            tracker.SendView("SplashScreen");
            //TryAgainButton.Click += TryAgainButton_Click;
        }

        private long tryAgainCount = 0;
        private void TryAgainButton_Click(object sender, RoutedEventArgs e)
        {
            tryAgainCount++;
            var tracker = EasyTracker.GetTracker();
            tracker.SendEvent("ui", "click", "splash_try_again", tryAgainCount);
        }
    }
}
