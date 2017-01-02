using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content.Res;
using Newtonsoft.Json.Linq;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.Android
{
	public class SecretsProvider : ISecretsProvider
	{
		AssetManager assets;
		public SecretsProvider(AssetManager assets)
		{
			this.assets = assets;
		}

		public async Task<JObject> GetSecrets()
		{
			return JObject.Parse(await GetSecretsString());
		}

		private string secretsJson;
		private async Task<string> GetSecretsString()
		{
			if (secretsJson == null)
			{
				using (StreamReader sr = new StreamReader(assets.Open("Secrets.json")))
				{
					secretsJson = sr.ReadToEnd();
				}
			}
			return secretsJson;
		}
	}
}
