using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.LibraryImpl
{
    public class Vault : IVault
    {
        public void AddCredential(string resource, string userName, string password)
        {
            CurrentVault.Add(new PasswordCredential(resource, userName, password));
        }

        public bool Contains(string resource, string userName)
        {
            try
            {
                CurrentVault.Retrieve(resource, userName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string RetrievePassword(string resource, string userName)
        {
            return CurrentVault.Retrieve(resource, userName).Password;
        }

        private PasswordVault currentVault;
        public PasswordVault CurrentVault
        {
            get
            {
                currentVault = currentVault ?? new PasswordVault();
                return currentVault;
            }
        }
    }
}
