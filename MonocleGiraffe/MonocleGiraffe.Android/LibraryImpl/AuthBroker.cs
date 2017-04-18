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
using Newtonsoft.Json.Linq;
using System.Threading;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class AuthBroker : IAuthBroker
    {
        private Context context;
        private ISecretsProvider secretsProvider;
        private IHttpClient httpClient;

        public AuthBroker(Context context, ISecretsProvider secretsProvider, IHttpClient client)
        {
            this.context = context;
            this.secretsProvider = secretsProvider;
            this.httpClient = client;
        }

        public async Task<AuthResult> AuthenticateAsync(Uri requestUri, Uri callbackUri)
        {
            var auth = await BuildAuthenticator(requestUri, callbackUri);
            var tcs = new TaskCompletionSource<AuthenticatorCompletedEventArgs>();
            auth.Completed += (s, e) => tcs.SetResult(e);
            context.StartActivity(auth.GetUI(context));
            var args = await tcs.Task;
            if (args.IsAuthenticated)
            {
                var props = args.Account.Properties;
                const string url = "https://api.imgur.com/oauth2/token";
                var config = await secretsProvider.GetSecrets();
                string clientId = (string)config["Client_Id"];
                string clientSecret = (string)config["Client_Secret"];
                JObject payload = new JObject();
                payload["refresh_token"] = props["refresh_token"];
                payload["client_id"] = clientId;
                payload["client_secret"] = clientSecret;
                payload["grant_type"] = "refresh_token";

                string resultString = await httpClient.PostAsync(new Uri(url), payload.ToString(), default(CancellationToken), null);
                //NetworkHelper.ExecutePostRequest(url, payload, false);
                JObject result = JObject.Parse(resultString);
                Dictionary<string, string> ret = new Dictionary<string, string>();
                ret["account_username"] = (string)result["account_username"];
                ret["access_token"] = (string)result["access_token"];
                ret["refresh_token"] = (string)result["refresh_token"];
                ret["expires_at"] = (string)result["expires_at"];
                return new AuthResult(ret, AuthResponseStatus.Success);
            }
            else
            {
                return new AuthResult(args.Account?.Properties, AuthResponseStatus.UserCancel);
            }
        }

        private async Task<WebRedirectAuthenticator> BuildAuthenticator(Uri requestUri, Uri callbackUri)
        {
            Uri accessTokenUri = new Uri("https://api.imgur.com/oauth2/token");
            var config = await secretsProvider.GetSecrets();
            string clientId = (string)config["Client_Id"];
            string clientSecret = (string)config["Client_Secret"];
            var auth = new OAuth2Authenticator(clientId, clientSecret, "", requestUri, callbackUri, accessTokenUri);
            return auth;
        }        
    }
}