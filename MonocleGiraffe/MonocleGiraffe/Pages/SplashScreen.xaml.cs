using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.ViewModels;
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
        private SplashViewModel vm;
        public SplashScreen()
        {
            this.InitializeComponent();
            vm = new SplashViewModel();
            DataContext = vm;
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await GoToFrontPage();
        }

        private async void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            await GoToFrontPage();
        }

        private async Task GoToFrontPage()
        {
            if (await vm.ShakeHands())
                Frame.Navigate(typeof(FrontPage));
            await Task.Delay(100);
            Frame.BackStack.Clear();
        }
    }
}
