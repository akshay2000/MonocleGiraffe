using MonocleGiraffe.Controls;
using MonocleGiraffe.Pages;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace MonocleGiraffe
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Template10.Common.BootStrapper
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            SplashFactory = e => new ExtendedSplash(e);
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await Task.Delay(5000);
            NavigationService.Navigate(typeof(FrontPage));
        }
    }
}
