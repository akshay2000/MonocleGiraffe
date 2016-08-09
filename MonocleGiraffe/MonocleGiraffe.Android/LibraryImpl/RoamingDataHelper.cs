using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonocleGiraffe.Portable.Interfaces;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class RoamingDataHelper : IRoamingDataHelper
    {
        public async Task<string> GetText(string fileName)
        {
            return "[]";
        }

        public async Task StoreText(string textToWrite, string fileName)
        {
            return;
        }
    }
}