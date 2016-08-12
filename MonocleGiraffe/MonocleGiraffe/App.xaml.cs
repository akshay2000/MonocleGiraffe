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

namespace MonocleGiraffe
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Template10.Common.BootStrapper
    {
        private ExtendedSplash splash;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            HockeyClient.Current.Configure("00cd7c1e6d7c4bb6ad3adfb6f1ae7d1a");
            this.InitializeComponent();
            SplashFactory = (e) =>
            {
                splash = new ExtendedSplash(e);
                return splash;
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await InitLibrary();
            if (AuthenticationHelper.IsAuthIntended())
            {
                splash.IsLoading = true;
                JObject result = null;
                bool isSuccess = false;
                string errorMessage = "Could not connect to the internet";

                try
                {
                    result = await ShakeHands();
                }
                catch (AuthException e)
                {
                    if (e.Reason == AuthException.AuthExceptionReason.HttpError)
                        errorMessage = e.Message;
                    else
                        isSuccess = true;
                }

                if (result != null && result.HasValues)
                {
                    isSuccess = (bool)result["success"];
                    if (!isSuccess)
                    {
                        await SecretsHelper.RefreshAccessToken();
                        result = await ShakeHands();
                        isSuccess = (bool)result["success"];
                        errorMessage = JsonConvert.SerializeObject(result["data"], Formatting.Indented);
                    }
                }
                if (!isSuccess)
                {
                    ShowError(errorMessage, "Connection Error");
                    return;
                }
            }
            NavigationService.Navigate(typeof(FrontPage));
        }

        private async Task<JObject> ShakeHands()
        {
            string userName = await SecretsHelper.GetUserName();
            const string urlPattern = "account/{0}/images/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result;
        }

        private void ShowError(string message, string title)
        {
            splash.IsLoading = false;
            splash.ErrorMessage = message;
        }

        private async Task InitLibrary()
        {
            var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var libFolder = installationFolder;
            var file = await libFolder.GetFileAsync("Secrets.json");
            string configurationString = await Windows.Storage.FileIO.ReadTextAsync(file);
            XamarinImgur.Helpers.Initializer.Init(new AuthBroker(), new Vault(), new SettingsHelper(), configurationString, () => new HttpClient(), false);
            Portable.Helpers.Initializer.Init(new RoamingDataHelper(), new SharingHelper());
        }
    }
}
