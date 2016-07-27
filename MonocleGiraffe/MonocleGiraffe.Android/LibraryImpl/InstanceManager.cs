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

namespace MonocleGiraffe.Android.LibraryImpl
{
    public static class InstanceManager
    {
        private static HttpClient client;
        public static HttpClient Client
        {
            get
            {
                client = client ?? new HttpClient();
                return client;
            }
        }

        public static SettingsHelper SettingsHelper { get; set; }
    }
}