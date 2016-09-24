using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XamarinImgur.Interfaces;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Controls.WebAuthBroker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
        }

        internal static async Task<AuthResult> AuthenticateAsync(Uri requestUri, Uri callbackUri)
        {

            //var broker = new WebAuthBrokerOld();
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            var tcs = new TaskCompletionSource<AuthResult>();
            AuthPage broker = null;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(AuthPage));
                broker = frame.Content as AuthPage;
                broker.Loaded += (sender, e) => broker.MainWebView.Navigate(requestUri);                
                broker.MainWebView.NavigationStarting += (sender, e) =>
                {
                    var uriString = e.Uri.ToString();
                    if (uriString.StartsWith(callbackUri.OriginalString))
                    {
                        if (!CoreApplication.GetCurrentView().IsMain)
                        {
                            Window.Current.Close();
                        }
                        var parsedResult = ParseAuthResult(e.Uri.OriginalString);
                        if (parsedResult.ContainsKey("error"))
                            tcs.SetResult(new AuthResult(null, AuthResponseStatus.UserCancel));
                        else
                            tcs.SetResult(new AuthResult(ParseAuthResult(e.Uri.OriginalString), AuthResponseStatus.Success));
                    }
                };
                SystemNavigationManager.GetForCurrentView().BackRequested += (sender, e) =>
                {
                    var isSet = tcs.TrySetResult(new AuthResult(null, AuthResponseStatus.UserCancel));
                };
                ApplicationView.GetForCurrentView().Consolidated += (sender, e) =>
                {
                    var isSet = tcs.TrySetResult(new AuthResult(null, AuthResponseStatus.UserCancel));
                    if (!CoreApplication.GetCurrentView().IsMain)
                    {
                        Window.Current.Close();
                    }
                };
                Window.Current.Content = frame;
                Window.Current.Activate();
                newViewId = ApplicationView.GetApplicationViewIdForWindow(newView.CoreWindow);
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);            
            return await tcs.Task;
        }
        
        private static Dictionary<string, string> ParseAuthResult(string result)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Uri uri = new Uri(result.Replace('#', '&'));
            string query = uri.Query;
            string[] frags = query.Split('&');
            foreach (var frag in frags)
            {
                string[] splits = frag.Split('=');
                ret.Add(splits[0], splits[1]);
            }
            return ret;
        }
    }
}
