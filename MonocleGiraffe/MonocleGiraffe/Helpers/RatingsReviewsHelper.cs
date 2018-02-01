using MonocleGiraffe.Portable.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.Helpers
{
    public class RatingsReviewsHelper : ReviewsHelper
    {
        public RatingsReviewsHelper(ISettingsHelper settingsHelper) : base(settingsHelper) { }

        protected override void PostDialogHook(bool isRated)
        {
            var yesNo = isRated ? "Yes" : "No";
            GoogleAnalytics.EasyTracker.GetTracker().SendView($"RatingsDialog{yesNo}");
        }

        protected override void PreDialogHook()
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("RatingsDialog");
        }

        protected override async Task<bool> ShowRatingReviewDialog()
        {
            StoreSendRequestResult result = await StoreRequestHelper.SendRequestAsync(StoreContext.GetDefault(), 16, String.Empty);
            if (result.ExtendedError == null)
            {
                JObject jsonObject = JObject.Parse(result.Response);
                if (jsonObject.SelectToken("status").ToString() == "success")
                {
                    return true;
                }
            }            
            return false;
        }
    }
}
