using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Helpers;
using XamarinImgur.Interfaces;
using static XamarinImgur.Helpers.Initializer;

namespace XamarinImgur.Helpers
{
    public static class AuthenticationHelper
    {
        private const string authUrlPattern = "https://api.imgur.com/oauth2/authorize?response_type=token&client_id={0}&state=yo";
        private const string callback = "http://localhost:8080/MonocleGiraffe";

        private static Dictionary<string, string> authResult;
        private const string userNameKey = "account_username";
        private const string accessTokenKey = "access_token";
        private const string refreshTokenKey = "refresh_token";
        private const string expiresAtKey = "expires_at";
        private const string isAuthIntendedKey = "IsAuthIntended";

        public static async Task<Dictionary<string, string>> Auth()
        {
            SettingsHelper.SetLocalValue(isAuthIntendedKey, false);
            JObject config = await SecretsHelper.GetConfiguration();
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

        private static async Task<Dictionary<string, string>> GetAuthResult()
        {           
            authResult = authResult ?? await Auth();            
            return authResult;
        }

        public static async Task<string> GetAccessToken()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[accessTokenKey];
        }

        public static async Task<string> GetRefreshToken()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[refreshTokenKey];
        }

        public static async Task<string> GetUserName()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[userNameKey];
        }

        public static async Task<DateTime> GetExpiresAt()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return DateTime.Parse(result[expiresAtKey]);
        } 

        public static bool IsAuthIntended()
        {
            return SettingsHelper.GetLocalValue<bool>(isAuthIntendedKey, false);
        }

        public static async Task<string> RefreshAccessToken(string refreshToken)
        {
            SettingsHelper.SetLocalValue(isAuthIntendedKey, false);
            const string url = "https://api.imgur.com/oauth2/token";
            var config = await SecretsHelper.GetConfiguration();
            string clientId = (string)config["Client_Id"];
            string clientSecret = (string)config["Client_Secret"];
            JObject payload = new JObject();
            payload["refresh_token"] = refreshToken;
            payload["client_id"] = clientId;
            payload["client_secret"] = clientSecret;
            payload["grant_type"] = "refresh_token";

            JObject result = await NetworkHelper.ExecutePostRequest(url, payload, false);
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
