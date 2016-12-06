using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private AdDuplex.AdControl banner;
        public AdDuplex.AdControl Banner
        {
            get
            {
                if (banner == null)
                {
                    banner = new AdDuplex.AdControl();
                    banner.AdUnitId = bannerId;
                    banner.AppKey = appKey;
                }
                return banner;
            }
        }
    }
}
