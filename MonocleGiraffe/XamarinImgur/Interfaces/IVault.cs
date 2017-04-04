using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Interfaces
{
    public interface IVault
    {
        string RetrievePassword(string resource, string userName);
        bool Contains(string resource, string userName);
        void AddCredential(string resource, string userName, string password);
        void RemoveCredential(string resource, string userName);
    }
}
