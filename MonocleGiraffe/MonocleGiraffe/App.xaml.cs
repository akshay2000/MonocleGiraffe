using Microsoft.HockeyApp;
using MonocleGiraffe.Controls;
using MonocleGiraffe.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using System;
using Windows.Foundation;
using Windows.UI.Popups;
using MonocleGiraffe.LibraryImpl;
using XamarinImgur.Helpers;
using MonocleGiraffe.Helpers;
using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Portable.ViewModels;

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
            HockeyClient.Current.Configure("00cd7c1e6d7c4bb6ad3adfb6f1ae7d1a");
            this.InitializeComponent();
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Lifecycle", startKind.ToString(), null, 0);
            bool isNewLaunch = args.PreviousExecutionState == ApplicationExecutionState.NotRunning;
            if (isNewLaunch)
            {
                await InitLibrary();

                var nav = new MergedNavigationService(NavigationService);
                nav.Configure(ViewModelLocator.SplashPageKey, typeof(Pages.SplashScreen));
                nav.Configure(ViewModelLocator.FrontPageKey, typeof(FrontPage));
                nav.Configure(ViewModelLocator.SubGalleryPageKey, typeof(SubGalleryPage));
                nav.Configure(ViewModelLocator.SubredditBrowserPageKey, typeof(SubredditBrowserPage));
                nav.Configure(ViewModelLocator.BrowserPageKey, typeof(BrowserPage));
                if (!SimpleIoc.Default.IsRegistered<GalaSoft.MvvmLight.Views.INavigationService>())
                    SimpleIoc.Default.Register<GalaSoft.MvvmLight.Views.INavigationService>(() => nav);
                SimpleIoc.Default.Register<IViewModelLocator>(() => ViewModelLocator.GetInstance());
                SimpleIoc.Default.Register<RemoteDeviceHelper>();
            }
            JObject navigationParam = new JObject();
            navigationParam["isNewLaunch"] = isNewLaunch;
            if (args is ProtocolActivatedEventArgs)
            {
                var protoArgs = args as ProtocolActivatedEventArgs;
                if (args.Kind == ActivationKind.Protocol)
                    navigationParam["url"] = protoArgs.Uri.AbsoluteUri;
            }
            Portable.Helpers.StateHelper.SessionState["LaunchData"] = navigationParam;
            SimpleIoc.Default.GetInstance< GalaSoft.MvvmLight.Views.INavigationService>().NavigateTo(ViewModelLocator.SplashPageKey);
        }

        private async Task InitLibrary()
        {
            var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var libFolder = installationFolder;
            var file = await libFolder.GetFileAsync("Secrets.json");
            string configurationString = await Windows.Storage.FileIO.ReadTextAsync(file);
            XamarinImgur.Helpers.Initializer.Init(new AuthBroker(), new Vault(), new SettingsHelper(), configurationString, () => new HttpClient(), false);
            Portable.Helpers.Initializer.Init(new RoamingDataHelper(), new SharingHelper(), new ClipboardHelper());
        }
    }
}
