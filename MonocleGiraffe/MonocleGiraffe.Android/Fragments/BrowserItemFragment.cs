using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MonocleGiraffe.Portable.Models;
using FFImageLoading.Views;
using FFImageLoading;
using Android.Media;
using MonocleGiraffe.Android.Controls;
using MonocleGiraffe.Android.Activities;

namespace MonocleGiraffe.Android.Fragments
{
    public class BrowserItemFragment : global::Android.Support.V4.App.Fragment
    {
        private IGalleryItem item;
        private bool isAlbum;
        public const string POSITION_ARG = "position";

        public static BrowserItemFragment NewInstance(int position)
        {
            BrowserItemFragment ret = new BrowserItemFragment();
            Bundle args = new Bundle(1);
            args.PutInt(POSITION_ARG, position);
            ret.Arguments = args;
            return ret;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var position = Arguments.GetInt(POSITION_ARG);
            item = (Activity as BrowserActivity)?.Vm.Images.ElementAt(position);
            isAlbum = item.ItemType == GalleryItemType.Album;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            int layout = isAlbum ? Resource.Layout.Tmpl_Album : Resource.Layout.Tmpl_Image;
            return inflater.Inflate(layout, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (!isAlbum)
                MainImage.RenderContent(item);
            Title.Text = item.Title;
            base.OnViewCreated(view, savedInstanceState);
        }

        private ImageControl mainImage;
        public ImageControl MainImage
        {
            get
            {
                if (isAlbum)
                    return null;
                mainImage = mainImage ?? View.FindViewById<ImageControl>(Resource.Id.MainImage);
                return mainImage;
            }
        }

        private TextView title;
        public TextView Title
        {
            get
            {                
                title = title ?? View.FindViewById<TextView>(Resource.Id.TitleTextView);
                return title;
            }
        }
    }
}