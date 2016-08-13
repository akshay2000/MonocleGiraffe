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
using MonocleGiraffe.Portable.Models;

namespace MonocleGiraffe.Android.LibraryImpl
{
    public class SharingHelper : ISharingHelper
    {
        public void ShareComment(CommentViewModel comment)
        {
            throw new NotImplementedException();
        }

        public void ShareItem(IGalleryItem item)
        {
            throw new NotImplementedException();
        }
    }
}