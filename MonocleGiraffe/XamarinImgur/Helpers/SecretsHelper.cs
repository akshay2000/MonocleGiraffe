using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Helpers;
using XamarinImgur.Interfaces;
using static XamarinImgur.Helpers.Initializer;

namespace XamarinImgur.Helpers
{
    public class SecretsHelper
    {
        private static JObject configuration;

        private const string userNameKey = "UserName";
        private const string expiryKey = "Expiry";

        private const string accessResource = "AccessToken";
        private const string refreshResource = "RefreshToken";

        public static async Task<JObject> GetConfiguration()
        {
            if (configuration == null)            
                configuration = JObject.Parse(SecretsJson);            
            return configuration;
        }

        public static async Task<string> GetAccessToken()
        {
            string userName = await GetUserName();
            string token;
            DateTime expiry = DateTime.Parse(SettingsHelper.GetLocalValue<string>(expiryKey, DateTime.MinValue.ToString()));
            if (expiry == DateTime.MinValue)
            {
                token = await AuthenticationHelper.GetAccessToken();
                UpdateCredentials(token, await AuthenticationHelper.GetRefreshToken());
                SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
            }
            else if (expiry > DateTime.Now)
            {
                token = GetVault().RetrievePassword(accessResource, userName);
            }
            else
            {
                token = await AuthenticationHelper.RefreshAccessToken(await GetRefreshToken());
                UpdateCredentials(token, await AuthenticationHelper.GetRefreshToken());
                SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
            }
            return token;
        }

        public static async Task RefreshAccessToken()
        {
            NetworkHelper.FlushHttpClients();
            var accessToken = await AuthenticationHelper.RefreshAccessToken(await GetRefreshToken());
            UpdateCredentials(accessToken, await AuthenticationHelper.GetRefreshToken());
            SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
        }

        public static async Task<string> GetRefreshToken()
        {
            string userName = await GetUserName();
            string token;
            var currentVault = GetVault();
            if (currentVault.Contains(refreshResource, userName))
            {
                token = GetVault().RetrievePassword(refreshResource, userName);
            }
            else
            {
                token = await AuthenticationHelper.GetRefreshToken();
                UpdateCredentials(await AuthenticationHelper.GetAccessToken(), token);
            }
            return token;
        }

        private static IVault GetVault()
        {
            return Vault;
        }

        private static string userName;
        public static async Task<string> GetUserName()
        {
            if (userName == null)
                userName = SettingsHelper.GetLocalValue<string>(userNameKey);
            if (userName == null)
            {
                userName = await AuthenticationHelper.GetUserName();
                SettingsHelper.SetLocalValue(userNameKey, userName);
            }
            return userName;
        }

        private static void UpdateCredentials(string accessToken = null, string refreshToken = null)
        {
            if (accessToken != null)
                GetVault().AddCredential(accessResource, userName, accessToken);
            if (refreshToken != null)
                GetVault().AddCredential(refreshResource, userName, refreshToken);
        }

        public static async Task RefreshSecrets()
        {
            userName = null;
            SettingsHelper.RemoveLocalValue(userNameKey);
            string newUserName = await GetUserName();
            SettingsHelper.SetLocalValue(userNameKey, newUserName);
            string accessToken = await AuthenticationHelper.GetAccessToken();
            GetVault().AddCredential(accessResource, newUserName, accessToken);
            string refreshToken = await AuthenticationHelper.GetRefreshToken();
            GetVault().AddCredential(refreshResource, newUserName, refreshToken);
        }
    }
}
