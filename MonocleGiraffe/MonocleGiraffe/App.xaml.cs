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
using XamarinImgur.Interfaces;

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
            if (!SimpleIoc.Default.IsRegistered<GalaSoft.MvvmLight.Views.INavigationService>())
                await ConfigureIoc();
            if (isNewLaunch)
            {
                await InitLibrary();
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
            
            SimpleIoc.Default.GetInstance<GalaSoft.MvvmLight.Views.INavigationService>().NavigateTo(ViewModelLocator.SplashPageKey);
        }

        private async Task ConfigureIoc()
        {
            var nav = new MergedNavigationService(NavigationService);
            nav.Configure(ViewModelLocator.SplashPageKey, typeof(Pages.SplashScreen));
            nav.Configure(ViewModelLocator.FrontPageKey, typeof(FrontPage));
            nav.Configure(ViewModelLocator.SubGalleryPageKey, typeof(SubGalleryPage));
            nav.Configure(ViewModelLocator.SubredditBrowserPageKey, typeof(SubredditBrowserPage));
            nav.Configure(ViewModelLocator.BrowserPageKey, typeof(BrowserPage));
            nav.Configure(ViewModelLocator.SettingsPageKey, typeof(SettingsPage));
            nav.Configure(ViewModelLocator.EditItemPageKey, typeof(EditItemPage));
            nav.Configure(ViewModelLocator.SelfBrowserPageKey, typeof(SelfBrowserPage));
            if (!SimpleIoc.Default.IsRegistered<GalaSoft.MvvmLight.Views.INavigationService>())
                SimpleIoc.Default.Register<GalaSoft.MvvmLight.Views.INavigationService>(() => nav);
            SimpleIoc.Default.Register<IViewModelLocator>(() => ViewModelLocator.GetInstance());

            SimpleIoc.Default.Register<RemoteDeviceHelper>();
            SimpleIoc.Default.Register<AddOnsHelper>();

            SimpleIoc.Default.Register<IHttpClient, HttpClient>();
            SimpleIoc.Default.Register<ISecretsProvider, SecretsProvider>(true);
            SimpleIoc.Default.Register<IVault, Vault>();
            SimpleIoc.Default.Register<IAuthBroker, AuthBroker>();
            SimpleIoc.Default.Register<ISettingsHelper, SettingsHelper>();
            SimpleIoc.Default.Register<AuthenticationHelper>();
            SimpleIoc.Default.Register<SecretsHelper>();
            SimpleIoc.Default.Register<TileManager>();
            var authHelper = SimpleIoc.Default.GetInstance<AuthenticationHelper>();
            var secretsHelper = SimpleIoc.Default.GetInstance<SecretsHelper>();
            SimpleIoc.Default.Register<NetworkHelper>(() => new NetworkHelper(authHelper, () => new HttpClient(), secretsHelper));
            
            SimpleIoc.Default.Register<AdHelper>();
        }

        private async Task InitLibrary()
        {            
            Initializer.Init(false);
            Portable.Helpers.Initializer.Init(new RoamingDataHelper(), new SharingHelper(), new ClipboardHelper());
        }        
    }
}
