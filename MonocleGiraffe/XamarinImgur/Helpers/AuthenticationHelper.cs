using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinImgur.Helpers;
using XamarinImgur.Interfaces;

namespace XamarinImgur.Helpers
{
    public class AuthenticationHelper
    {
        private const string authUrlPattern = "https://api.imgur.com/oauth2/authorize?response_type=token&client_id={0}&state=yo";
        private const string callback = "http://localhost:8080/MonocleGiraffe";

        private Dictionary<string, string> authResult;
        private const string userNameKey = "account_username";
        private const string accessTokenKey = "access_token";
        private const string refreshTokenKey = "refresh_token";
        private const string expiresAtKey = "expires_at";
        private const string isAuthIntendedKey = "IsAuthIntended";

        private ISettingsHelper SettingsHelper { get; set; }
        private IAuthBroker AuthBroker { get; set; }

        private readonly ISecretsProvider secretsProvider;
        private readonly IHttpClient httpClient;

        public AuthenticationHelper(ISettingsHelper settingsHelper, IAuthBroker authBroker, ISecretsProvider secretsProvider, IHttpClient httpClient)
        {
            SettingsHelper = settingsHelper;
            AuthBroker = authBroker;
            this.secretsProvider = secretsProvider;
            this.httpClient = httpClient;
        }

        public async Task<Dictionary<string, string>> Auth()
        {
            SettingsHelper.SetLocalValue(isAuthIntendedKey, false);
            JObject config = await secretsProvider.GetSecrets();
            Uri uri = new Uri(string.Format(authUrlPattern, config["Client_Id"]));
            var result = await AuthBroker.AuthenticateAsync(uri, new Uri(callback));
            if (result.ResponseStatus == AuthResponseStatus.ErrorHttp)
                throw new AuthException($"Error Code: {result.ErrorDetail}");
            else if (result.ResponseStatus == AuthResponseStatus.UserCancel)
                throw new AuthException($"User cancelled the auth process");
            SettingsHelper.SetLocalValue(isAuthIntendedKey, true);

            //result.ResponseData[expiresAtKey] = DateTime.Now.AddSeconds(int.Parse(ret["expires_in"])).ToString();
            //Imgur API is a liar. It says the token is valid for a month but expires it in an hour anyway.
            if (result.ResponseData != null)
                result.ResponseData[expiresAtKey] = DateTime.Now.AddSeconds(3600).ToString();
            return result.ResponseData;
        }

        public void SetAuthIntention(bool flag)
        {
            SettingsHelper.SetLocalValue(isAuthIntendedKey, flag);
        }

        private async Task<Dictionary<string, string>> GetAuthResult()
        {           
            authResult = authResult ?? await Auth();            
            return authResult;
        }

        public async Task<string> GetAccessToken()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[accessTokenKey];
        }

        public async Task<string> GetRefreshToken()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[refreshTokenKey];
        }

        public async Task<string> GetUserName()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[userNameKey];
        }

        public async Task<DateTime> GetExpiresAt()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return DateTime.Parse(result[expiresAtKey]);
        } 

        public bool IsAuthIntended()
        {
            return SettingsHelper.GetLocalValue<bool>(isAuthIntendedKey, false);
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            SettingsHelper.SetLocalValue(isAuthIntendedKey, false);
            const string url = "https://api.imgur.com/oauth2/token";
            var config = await secretsProvider.GetSecrets();
            string clientId = (string)config["Client_Id"];
            string clientSecret = (string)config["Client_Secret"];
            JObject payload = new JObject();
            payload["refresh_token"] = refreshToken;
            payload["client_id"] = clientId;
            payload["client_secret"] = clientSecret;
            payload["grant_type"] = "refresh_token";

            string resultString = await httpClient.PostAsync(new Uri(url), payload.ToString(), default(CancellationToken), null);
            //NetworkHelper.ExecutePostRequest(url, payload, false);
            JObject result = JObject.Parse(resultString);
            SettingsHelper.SetLocalValue(isAuthIntendedKey, true);
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret[userNameKey] = (string)result["account_username"];
            ret[accessTokenKey] = (string)result["access_token"];
            ret[refreshTokenKey] = (string)result["refresh_token"];
            //ret[expiresAtKey] = DateTime.Now.AddSeconds((int)result["expires_in"]).ToString();
            //We must do this because Imgur API lies about expiry time
            ret[expiresAtKey] = DateTime.Now.AddSeconds(3600).ToString();
            authResult = ret;
            return await GetAccessToken();
        }
    }

    public class AuthException : Exception
    {
        public AuthException() { }
        public AuthException(string message) : base(message) { }
        public AuthException(string message, AuthExceptionReason reason) : base(message) { Reason = reason; }
        public AuthException(string message, Exception inner) : base(message, inner) { }

        public AuthExceptionReason Reason{ get; set; }

        public enum AuthExceptionReason
        {
            UserCancelled,
            HttpError
        } 
    }
}
