using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MonocleGiraffe.Helpers
{
    [Obsolete]
    public static class RoamingDataHelper
    {
        private static StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
        public static async Task StoreText(string textToWrite, string fileName)
        {
            var file = await roamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, textToWrite);
        }

        public static async Task<string> GetText(string fileName)
        {
            string textToReturn;
            try
            {
                var file = await roamingFolder.GetFileAsync(fileName);
                textToReturn = await FileIO.ReadTextAsync(file);
            }
            catch (FileNotFoundException)
            {
                textToReturn = "[]";
            }
            return textToReturn;
        }
    }
}
