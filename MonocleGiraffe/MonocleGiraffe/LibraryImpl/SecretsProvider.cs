using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.LibraryImpl
{
    public class SecretsProvider : ISecretsProvider
    {
        public async Task<JObject> GetSecrets()
        {
            return JObject.Parse(await GetSecretsString());
        }

        private string secretsString;
        private async Task<string> GetSecretsString()
        {
            if (secretsString == default(string))
            {
                var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var libFolder = installationFolder;
                var file = await libFolder.GetFileAsync("Secrets.json");
                secretsString = await Windows.Storage.FileIO.ReadTextAsync(file);
            }
            return secretsString;
        }
    }
}
