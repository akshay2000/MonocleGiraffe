using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
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
        }

        //private async void RetryButton_Click(object sender, RoutedEventArgs e)
        //{
        //    await ShakeHandsAndNavigate();
        //}

        //private async void SignInButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var vm = DataContext as SplashViewModel;
        //    if (!(await vm.SignIn()))
        //        return;
        //    await Navigate();
        //}
    }
}
