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
using Android.Util;
using Android.Text;

namespace MonocleGiraffe.Android.Helpers
{
    public static class Utils
    {
        public static int CalculateColumnCount(int reqestedWidth, int availableWidth)
        {
            int minorCount = Math.Max(1, availableWidth / reqestedWidth);
            int majorWidth = availableWidth / minorCount;
            int minorWidth = availableWidth / (minorCount + 1);
            int ret = Math.Abs(minorWidth - reqestedWidth) < Math.Abs(majorWidth - reqestedWidth) ? minorCount + 1 : minorCount;
            return ret;
        }

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

        private static int? accentColor;
        public static int GetAccentColor(Context context)
        {
            if (accentColor == null)
            {
                var typedValue = new TypedValue();
                TypedArray a = context.ObtainStyledAttributes(typedValue.Data, new int[] { Resource.Attribute.colorAccent });
                accentColor = a.GetColor(0, 0);
                a.Recycle();
            }
            return accentColor ?? -44462;
        }

        public static string GetAccentColorHex(Context context)
        {
            return $"#{GetAccentColor(context).ToString("X").Substring(2)}";
        }

        public static ISpanned FromHtml(string source)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                return Html.FromHtml(source, FromHtmlOptions.ModeLegacy);
            else
#pragma warning disable 612, 618
                return Html.FromHtml(source);
#pragma warning restore 612, 618
        }

        public static void SetPaddingForStatusBar(Activity activity, View itemToPad)
        {
            int barHeight = 0;
            int resourceId = activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
                barHeight = activity.Resources.GetDimensionPixelSize(resourceId);
            itemToPad.SetPadding(itemToPad.Left, itemToPad.PaddingTop+ barHeight, itemToPad.PaddingRight, itemToPad.PaddingBottom);
        }
    }
}