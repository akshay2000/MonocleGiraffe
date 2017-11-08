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
using MonocleGiraffe.Portable.Helpers;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Media;
using Windows.Foundation.Metadata;

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
            SetAdaptiveAcrylicBrush();
            bool isNewLaunch = args.PreviousExecutionState == ApplicationExecutionState.NotRunning;
            if (!SimpleIoc.Default.IsRegistered<GalaSoft.MvvmLight.Views.INavigationService>())
                await ConfigureIoc();
            if (isNewLaunch)
            {
                await InitLibrary();
                RegisterBackgroundTask();
            }
            JObject navigationParam = new JObject();
            const string launchType = "launchType";
            navigationParam[launchType] = LaunchType.AppTile.ToString();
            navigationParam["isNewLaunch"] = isNewLaunch;
            if (args is ProtocolActivatedEventArgs)
            {
                var protoArgs = args as ProtocolActivatedEventArgs;
                if (args.Kind == ActivationKind.Protocol)
                {
                    navigationParam["url"] = protoArgs.Uri.AbsoluteUri;
                    navigationParam[launchType] = LaunchType.Url.ToString();
                }
            }

            if(args is LaunchActivatedEventArgs)
            {
                var launchArgs = args as LaunchActivatedEventArgs;
                if (!string.IsNullOrEmpty(launchArgs.Arguments))
                {
                    navigationParam["tileArgs"] = launchArgs.Arguments;
                    navigationParam[launchType] = LaunchType.SecondaryTile.ToString();
                }
            }
            Portable.Helpers.StateHelper.SessionState["LaunchData"] = navigationParam;
            
            SimpleIoc.Default.GetInstance<GalaSoft.MvvmLight.Views.INavigationService>().NavigateTo(ViewModelLocator.SplashPageKey);
        }

        private void SetAdaptiveAcrylicBrush()
        {
            const string adaptiveBrush = "AdaptiveAcrylicBrush";
            if (!Current.Resources.ContainsKey(adaptiveBrush))
            {
                Current.Resources[adaptiveBrush] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 37, 37, 37));
                if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
                {
                    Current.Resources[adaptiveBrush] = Current.Resources["SystemControlChromeHighAcrylicWindowMediumBrush"];
                }
            }
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
            XamarinImgur.Helpers.Initializer.Init(false);
            Portable.Helpers.Initializer.Init(new RoamingDataHelper(), new SharingHelper(), new ClipboardHelper());
        }

        private async void RegisterBackgroundTask()
        {
            const string taskName = "TileUpdateTask";
            const string taskEntryPoint = "BackgroundTasks.TileUpdateTask";
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)                
                    return;                
            }

            var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();
            bool shouldRegister = requestStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy || requestStatus == BackgroundAccessStatus.AlwaysAllowed;
            if (!shouldRegister)
                return;

            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = taskName;
            taskBuilder.TaskEntryPoint = taskEntryPoint;
            taskBuilder.SetTrigger(new TimeTrigger(90, false));
            taskBuilder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            var registration = taskBuilder.Register();
        }
    }
}
