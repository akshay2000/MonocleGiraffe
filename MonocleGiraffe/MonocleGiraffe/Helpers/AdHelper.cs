using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace MonocleGiraffe.Helpers
{
    public class AdHelper
    {
        private readonly string appKey;
        private readonly string bannerId;

        public AdHelper(JObject config)
        {
            appKey = (string)config["App_Key"];
            bannerId = (string)config["Banner_Id"];
            AdDuplex.AdDuplexClient.Initialize(appKey);
        }

        private AdControl banner;
        public AdControl Banner
        {
            get
            {
                if (banner == null)
                {
                    banner = new AdControl();
                    banner.Init(appKey, bannerId);
                }
                return banner;
            }
        }

        public async Task<bool> ShowAds()
        {
            AddOnsHelper addOnsHelper = SimpleIoc.Default.GetInstance<AddOnsHelper>();
            IReadOnlyDictionary<string, StoreLicense> licenses = await addOnsHelper.GetAddOnLicenses();
            foreach (var item in licenses)
            {
                if (item.Value.IsActive)
                    return false;
            }
            return true;
        }
    }
}
