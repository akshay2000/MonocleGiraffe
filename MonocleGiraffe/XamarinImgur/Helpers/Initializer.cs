using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Interfaces;

namespace XamarinImgur.Helpers
{
    public static class Initializer
    {
        public static void Init(bool isCommercial)
        {
            //AuthBroker = broker;
            //Vault = vault;
            //SettingsHelper = settingsHelper;
            //SecretsJson = secretsJson;
            //HttpClientFactory = clientFactory;
            IsCommercial = isCommercial;
        }

        //public static IAuthBroker AuthBroker { get; private set; }
        //public static IVault Vault { get; private set; }
        //public static ISettingsHelper SettingsHelper { get; private set; }
        //public static string SecretsJson { get; private set; }
        //public static Func<IHttpClient> HttpClientFactory { get; private set; }
        public static bool IsCommercial { get; private set; }        
    }
}
