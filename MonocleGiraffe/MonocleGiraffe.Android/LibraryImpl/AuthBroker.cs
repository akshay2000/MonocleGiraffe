using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamarinImgur.Interfaces;
using Xamarin.Auth;
using XamarinImgur.Helpers;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class AuthBroker : IAuthBroker
    {
        private Context context;
        public AuthBroker(Context context)
        {
            this.context = context;
        }

        public async Task<AuthResult> AuthenticateAsync(Uri requestUri, Uri callbackUri)
        {
            var auth = await BuildAuthenticator(requestUri, callbackUri);
            var tcs = new TaskCompletionSource<AuthenticatorCompletedEventArgs>();
            auth.Completed += (s, e) => tcs.SetResult(e);
            context.StartActivity(auth.GetUI(context));
            var args = await tcs.Task;
            if (args.IsAuthenticated)
                return new AuthResult(args.Account.Properties, AuthResponseStatus.Success);
            else
                return new AuthResult(args.Account?.Properties, AuthResponseStatus.UserCancel);
        }

        private async Task<WebRedirectAuthenticator> BuildAuthenticator(Uri requestUri, Uri callbackUri)
        {
            Uri accessTokenUri = new Uri("https://api.imgur.com/oauth2/token");
            var config = await SecretsHelper.GetConfiguration();
            string clientId = (string)config["Client_Id"];
            string clientSecret = (string)config["Client_Secret"];
            var auth = new OAuth2Authenticator(clientId, clientSecret, "", requestUri, callbackUri, accessTokenUri);
            return auth;
        }
    }
}