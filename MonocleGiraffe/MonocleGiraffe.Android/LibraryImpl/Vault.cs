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
using Xamarin.Auth;
using MonocleGiraffe.Android.Activities;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class Vault : IVault
    {
        private const string PasswordKey = "Password";

        private Context context;

        public Vault(Context context)
        {
            this.context = context;
        }

        public void AddCredential(string resource, string userName, string password)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            props[PasswordKey] = password;
            Account account = new Account(userName, props);
            BackingStore.Save(account, resource);
        }

        public bool Contains(string resource, string userName)
        {
            return BackingStore.FindAccountsForService(resource).Where(a => a.Username == userName).Count() > 0;
        }

        public string RetrievePassword(string resource, string userName)
        {
            return BackingStore.FindAccountsForService(resource).Where(a => a.Username == userName).First()?.Properties[PasswordKey];
        }

        public void RemoveCredential(string resource, string userName)
        {
            var account = BackingStore.FindAccountsForService(resource).Where(a => a.Username == userName).First();
            if (account != null)
                BackingStore.Delete(account, resource);
        }

        private AccountStore backingStore;
        public AccountStore BackingStore
        {
            get
            {
                backingStore = backingStore ?? AccountStore.Create(context);
                return backingStore;
            }
        }
    }
}