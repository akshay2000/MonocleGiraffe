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
using MonocleGiraffe.Portable.Interfaces;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class ClipboardHelper : IClipboardHelper
    {
        Context context;
        public ClipboardHelper(Context context)
        {
            this.context = context;
        }

        public void Clip(string text)
        {
            ClipboardManager clipboardManager = (ClipboardManager)context.GetSystemService(Context.ClipboardService);
            ClipData data = ClipData.NewPlainText("Text from Monocle Giraffe", text);
            clipboardManager.PrimaryClip = data;
            Toast toast = Toast.MakeText(context, "Copied to clipboard", ToastLength.Short);
            toast.Show();
        }
    }
}