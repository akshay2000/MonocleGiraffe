using MonocleGiraffe.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10.Controls;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MonocleGiraffe.Controls
{
    public sealed partial class WebAuthBrokerOld : UserControl
    {
        public WebAuthBrokerOld()
        {
            this.InitializeComponent();
        }        
        
        internal async static Task<string> AuthenticateAsync(Uri requestUri, Uri callbackUri)
        {
            var broker = new WebAuthBrokerOld();
            //CoreApplicationView newView = CoreApplication.CreateNewView();
            //int newViewId = 0;
            //await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    Frame frame = new Frame();
            //    frame.Navigate(typeof(TransfersPage));
            //    var c = frame.Content;
            //    Window.Current.Content = frame;
            //    newViewId = ApplicationView.GetApplicationViewIdForWindow(newView.CoreWindow);
            //});
            //bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
          
            var tcs = new TaskCompletionSource<string>();
            broker.MainWebView.NavigationStarting += (sender, e) =>
            {
                var uriString = e.Uri.ToString();
                if (uriString.StartsWith(callbackUri.OriginalString))
                    tcs.SetResult(e.Uri.OriginalString);
            };
            broker.MainWebView.Navigate(requestUri);
            return await tcs.Task;
        }
    }
}
