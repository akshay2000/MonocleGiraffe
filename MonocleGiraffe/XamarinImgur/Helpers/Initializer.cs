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
        public static IAuthBroker AuthBroker { get; set; }
        public static IVault Vault { get; set; }
        public static ISettingsHelper SettingsHelper { get; set; }
        public static string SecretsJson { get; set; }
    }
}
