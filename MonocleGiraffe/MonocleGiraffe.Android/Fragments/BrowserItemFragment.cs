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
            int layout;// = isAlbum ? Resource.Layout.Tmpl_Album : Resource.Layout.Tmpl_Image;
            switch (item.ItemType)
            {
                case GalleryItemType.Album:
                    layout = Resource.Layout.Tmpl_Album;
                    break;
                case GalleryItemType.Animation:
                    layout = Resource.Layout.Ctrl_Image;
                    break;
                default:
                    layout = Resource.Layout.Tmpl_Image;
                    break;

            }
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(layout, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            switch (item.ItemType)
            {
                case GalleryItemType.Album:
                    Title.Text = item.Title;
                    break;
                case GalleryItemType.Animation:
                    MainVideoView.SetVideoURI(global::Android.Net.Uri.Parse(item.Mp4));
                    MainVideoView.Start();
                    MainVideoView.Prepared += MainVideoView_Prepared;
                    break;
                default:
                    Title.Text = item.Title;
                    ImageService.Instance.LoadUrl(item.Link).Into(MainImage);
                    break;

            }
            //if (!isAlbum)
            //    ImageService.Instance.LoadUrl(item.Link).Into(MainImage);
            //Title.Text = item.Title;
            base.OnViewCreated(view, savedInstanceState);
        }

        private void MainVideoView_Prepared(object sender, EventArgs e)
        {
            var mp = sender as MediaPlayer;
            mp.Looping = true;
        }

        private VideoView mainVideoView;
        public VideoView MainVideoView
        {
            get
            {
                mainVideoView = mainVideoView ?? View.FindViewById<VideoView>(Resource.Id.MainVideoView);
                return mainVideoView;
            }
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