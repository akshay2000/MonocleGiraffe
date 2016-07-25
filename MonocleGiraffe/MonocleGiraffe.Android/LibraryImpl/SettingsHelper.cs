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
using Android.Preferences;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class SettingsHelper : ISettingsHelper
    {
        private Context context;

        public SettingsHelper(Context context)
        {
            this.context = context;
        }

        public T GetLocalValue<T>(string key)
        {
            return GetLocalValue<T>(key, default(T));
        }

        public T GetLocalValue<T>(string key, object defaultValue)
        {
            object ret = null;
            Type type = typeof(T);
            if (type == typeof(int))
                ret = Preferences.GetInt(key, (int)defaultValue);
            else if (type == typeof(string))
                ret = Preferences.GetString(key, (string)defaultValue);
            else if (type == typeof(bool))
                ret = Preferences.GetBoolean(key, (bool)defaultValue);
            return (T)ret;
        }

        public T GetValue<T>(string key)
        {
            return GetLocalValue<T>(key, default(T));
        }

        public T GetValue<T>(string key, object defaultValue)
        {
            return GetLocalValue<T>(key, defaultValue);
        }

        public void RemoveLocalValue(string key)
        {
            var editor = Preferences.Edit();
            editor.Remove(key);
            editor.Commit();
        }

        public void SetLocalValue(string key, object value)
        {
            var editor = Preferences.Edit();
            Type type = value.GetType();
            if (type == typeof(int))
                editor.PutInt(key, (int)value);
            else if (type == typeof(string))
                editor.PutString(key, (string)value);
            else if (type == typeof(bool))
                editor.PutBoolean(key, (bool)value);
            editor.Commit();
        }

        public void SetValue(string key, object value)
        {
            SetLocalValue(key, value);
        }

        private ISharedPreferences preferences;
        public ISharedPreferences Preferences
        {
            get
            {
                preferences = preferences ?? PreferenceManager.GetDefaultSharedPreferences(context);
                return preferences;
            }
        }
    }
}