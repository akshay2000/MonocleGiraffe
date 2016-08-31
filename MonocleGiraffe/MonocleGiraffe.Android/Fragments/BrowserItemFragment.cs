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

namespace MonocleGiraffe.Android.Fragments
{
    public class BrowserItemFragment : global::Android.Support.V4.App.Fragment
    {
        private IGalleryItem item;
        private bool isAlbum;
        public BrowserItemFragment(IGalleryItem item)
        {
            this.item = item;
            isAlbum = item.ItemType == GalleryItemType.Album;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var layout = isAlbum ? Resource.Layout.Tmpl_Album : Resource.Layout.Tmpl_Image;
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(layout, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (!isAlbum)
                ImageService.Instance.LoadUrl(item.Link).Into(MainImage);
            Title.Text = item.Title;
            base.OnViewCreated(view, savedInstanceState);
        }

        private ImageViewAsync mainImage;
        public ImageViewAsync MainImage
        {
            get
            {
                if (isAlbum)
                    return null;
                mainImage = mainImage ?? View.FindViewById<ImageViewAsync>(Resource.Id.MainImage);
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