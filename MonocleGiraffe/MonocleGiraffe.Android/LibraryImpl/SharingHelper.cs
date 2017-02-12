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
        private Context context;

        public SharingHelper(Context context)
        {
            this.context = context;
        }

        public void ShareComment(CommentViewModel comment)
        {
            throw new NotImplementedException();
        }

        public void ShareItem(IGalleryItem item)
        {
            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraTitle, item.Title ?? "Sharing from Monocle Giraffe");
            sendIntent.PutExtra(Intent.ExtraText, item.Link);
            sendIntent.SetType("text/plain");
            context.StartActivity(sendIntent);
        }
    }
}