using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.Res;

namespace MonocleGiraffe.Android.Helpers
{
    public static class Utils
    {
        public static int PxToDp(int px, Resources res)
        {
            int dp = (int)Math.Round(px / res.DisplayMetrics.Density);
            return dp;
        }

        public static int DpToPx(int dp, Resources res)
        {
            int px = (int)Math.Round(dp * res.DisplayMetrics.Density + 0.5f);
            return px;
        }
    }
}