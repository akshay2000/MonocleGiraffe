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
using System.IO;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class RoamingDataHelper : IRoamingDataHelper
    {
        private Context context;
        public RoamingDataHelper(Context context = null)
        {
            this.context = context;
        }

        public async Task<string> GetText(string fileName)
        {
            string ret = "[]";
            try
            {
                string path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString();
                string fullFilePath = Path.Combine(path, fileName);
                using (var streamReader = new StreamReader(fullFilePath))
                {
                    ret = streamReader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine(ret);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
            return ret;
        }

        public async Task StoreText(string textToWrite, string fileName)
        {
            try
            {
                string path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString();
                string fullFilePath = Path.Combine(path, fileName);
                
                using (var streamWriter = new StreamWriter(fullFilePath, false))
                {
                    await streamWriter.WriteAsync(textToWrite);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
    }
}