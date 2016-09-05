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
using Android.Util;
using MonocleGiraffe.Android.Helpers;

namespace MonocleGiraffe.Android.Controls
{
    public class FontTextView : TextView
    {

        public FontTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            //Typeface will not be applied in the layout editor.
            if (IsInEditMode)
            {
                return;
            }
            var styledAttrs = context.ObtainStyledAttributes(attrs, Resource.Styleable.FontTextView, 0, 0);

            //TypedArray styledAttrs = context.obtainStyledAttributes(attrs, R.styleable.CustomTextView);
            string font = styledAttrs.GetString(Resource.Styleable.FontTextView_typeface);
            styledAttrs.Recycle();

            if (font != null)
            {
                var typeface = FontManager.GetTypeface(context, font);
                SetTypeface(typeface, global::Android.Graphics.TypefaceStyle.Normal);
            }
        }
    }
}
