using MonocleGiraffe.Controls;
using MonocleGiraffe.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpImgur.Helpers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Popups;

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
            this.InitializeComponent();
            SplashFactory = (e) =>
            {
                splash = new ExtendedSplash(e);
                return splash;
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (AuthenticationHelper.IsAuthIntended())
            {
                splash.IsLoading = true;
                var result = await ShakeHands();
                bool isSuccess = false;
                string errorMessage = "Could not connect to the internet";
                if (result.HasValues)
                {
                    isSuccess = (bool)result["success"];
                    if (!isSuccess)
                    {
                        await AuthenticationHelper.RefreshAccessToken(await SecretsHelper.GetRefreshToken());
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
    }
}
