using Android.Content;
using Android.Graphics;
using Android.Widget;
using System.Collections.Generic;

namespace MonocleGiraffe.Android.Helpers
{
    public static class FontManager
    {
        private static Dictionary<string, Typeface> cache = new Dictionary<string, Typeface>();

        public const string AssetsRoot = "Fonts/";
        public const string Material = "MaterialIcons-Regular.ttf";

        public static Typeface GetTypeface(Context context, string font)
        {
            var key = AssetsRoot + font;
            if (!cache.ContainsKey(key))
                cache[key] = Typeface.CreateFromAsset(context.Assets, key);
            return cache[key];
        }

        public static void ApplyFont(this TextView text, Context context, string font)
        {
            text.SetTypeface(GetTypeface(context, font), TypefaceStyle.Normal);
        }
    }
}