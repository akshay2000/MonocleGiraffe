using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class Vault : IVault
    {
        public void AddCredential(string resource, string userName, string password)
        {
            InstanceManager.SettingsHelper.SetLocalValue($"{resource};{userName}", password);
        }

        public bool Contains(string resource, string userName)
        {
            string password = InstanceManager.SettingsHelper.GetLocalValue<string>($"{resource};{userName}", string.Empty);
            return password != string.Empty;
        }

        public string RetrievePassword(string resource, string userName)
        {
            return InstanceManager.SettingsHelper.GetLocalValue<string>($"{resource};{userName}", string.Empty);
        }
    }
}