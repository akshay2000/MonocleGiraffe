using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.Portable.Helpers
{
    public abstract class ReviewsHelper
    {
        private const string LaunchCountKey = "LaunchCount";
        private const string IsRatedKey = "IsRated";

        private ISettingsHelper settingsHelper;

        public ReviewsHelper(ISettingsHelper settingsHelper)
        {
            this.settingsHelper = settingsHelper;
        }
             
        protected abstract Task<bool> ShowRatingReviewDialog();
        protected abstract void PreDialogHook();
        protected abstract void PostDialogHook(bool isRated);

        public void IncrementLaunchCount()
        {
            int currentValue = settingsHelper.GetLocalValue<int>(LaunchCountKey, 0);
            currentValue++;
            settingsHelper.SetLocalValue(LaunchCountKey, currentValue);
        }

        public async Task ShowDialogIfNeeded()
        {
            if (ShouldShowDialog())
            {
                PreDialogHook();
                bool isRated = await ShowRatingReviewDialog();
                PostDialogHook(isRated);
                settingsHelper.SetLocalValue(IsRatedKey, isRated);                
            }
        }


        private bool ShouldShowDialog()
        {
            if (settingsHelper.GetLocalValue<bool>(IsRatedKey, false))
                return false;
            int launchCount = settingsHelper.GetLocalValue<int>(LaunchCountKey, 0);
            return launchCount > 0 && launchCount % 10 == 0;
        }
    }
}
