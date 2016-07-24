using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.LibraryImpl
{
    public class SettingsHelper : ISettingsHelper
    {
        private ApplicationDataContainer AppSettings = ApplicationData.Current.RoamingSettings;
        private ApplicationDataContainer AppLocalSettings = ApplicationData.Current.LocalSettings;

        public void SetValue(string key, object value)
        {
            AppSettings.Values[key] = value;
        }

        public T GetValue<T>(string key)
        {
            return (T)AppSettings.Values[key];
        }

        public T GetValue<T>(string key, object defaultValue)
        {
            if (AppSettings.Values[key] == null)
            {
                AppSettings.Values[key] = defaultValue;
            }
            return (T)AppSettings.Values[key];
        }

        public void SetLocalValue(string key, object value)
        {
            AppLocalSettings.Values[key] = value;
        }

        public T GetLocalValue<T>(string key)
        {
            return (T)AppLocalSettings.Values[key];
        }

        public T GetLocalValue<T>(string key, object defaultValue)
        {
            if (AppLocalSettings.Values[key] == null)
            {
                AppLocalSettings.Values[key] = defaultValue;
            }
            return (T)AppLocalSettings.Values[key];
        }

        public void RemoveLocalValue(string key)
        {
            AppLocalSettings.Values.Remove(key);
        }
    }
}
