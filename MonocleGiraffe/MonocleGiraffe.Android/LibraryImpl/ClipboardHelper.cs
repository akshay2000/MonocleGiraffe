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
        public void Clip(string text)
        {
            Console.WriteLine($"Clip called with text: {text}");
        }
    }
}