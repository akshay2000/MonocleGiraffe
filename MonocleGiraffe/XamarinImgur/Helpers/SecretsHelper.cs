using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Helpers;
using XamarinImgur.Interfaces;

namespace XamarinImgur.Helpers
{
    public class SecretsHelper
    {        
        private readonly IVault vault;
        private readonly ISecretsProvider secretsProvider;

        private const string userNameKey = "UserName";
        private const string expiryKey = "Expiry";

        private const string accessResource = "AccessToken";
        private const string refreshResource = "RefreshToken";


        private ISettingsHelper SettingsHelper { get; set; }
        private AuthenticationHelper AuthenticationHelper { get; set; }
        

        public SecretsHelper(ISettingsHelper settingsHelper, AuthenticationHelper authHelper, IVault vault, ISecretsProvider secretsProvider)
        {
            SettingsHelper = settingsHelper;
            AuthenticationHelper = authHelper;
            this.vault = vault;
            this.secretsProvider = secretsProvider;
        }

        public async Task<JObject> GetConfiguration()
        {
            return await secretsProvider.GetSecrets();
        }

        public async Task FlushSecrets()
        {
            string userName = await GetUserName();
            SettingsHelper.RemoveLocalValue(userNameKey);
            SettingsHelper.RemoveLocalValue(expiryKey);
            vault.RemoveCredential(accessResource, userName);
            vault.RemoveCredential(refreshResource, userName);
        }

        public async Task<string> GetAccessToken()
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

        public async Task RefreshAccessToken()
        {
            //TODO: REMOVE THIS LINE NetworkHelper.FlushHttpClients();
            var accessToken = await AuthenticationHelper.RefreshAccessToken(await GetRefreshToken());
            UpdateCredentials(accessToken, await AuthenticationHelper.GetRefreshToken());
            SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
        }

        public async Task<string> GetRefreshToken()
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

        private IVault GetVault()
        {
            return vault;
        }

        private string userName;
        public async Task<string> GetUserName()
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

        private void UpdateCredentials(string accessToken = null, string refreshToken = null)
        {
            if (accessToken != null)
                GetVault().AddCredential(accessResource, userName, accessToken);
            if (refreshToken != null)
                GetVault().AddCredential(refreshResource, userName, refreshToken);
        }

        public async Task RefreshSecrets()
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
