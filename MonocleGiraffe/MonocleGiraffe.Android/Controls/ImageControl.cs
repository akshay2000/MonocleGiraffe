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
using MonocleGiraffe.Portable.Models;
using FFImageLoading.Views;
using FFImageLoading;
using Android.Media;

namespace MonocleGiraffe.Android.Controls
{
    public class ImageControl : FrameLayout
    {
        public ImageControl(Context context) : base(context)
        {
            Initialize();
        }

        public ImageControl(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public ImageControl(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }
        public ImageControl(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.Ctrl_Image, this);
        }

        public void RenderContent(IGalleryItem item)
        {
            switch (item.ItemType)
            {
                case GalleryItemType.Animation:
                    RenderVideo(item);
                    break;
                case GalleryItemType.Image:
                    RenderImage(item);
                    break;
                default:
                    throw new NotSupportedException($"Itemtype {item.ItemType.ToString()} is not supported.");
            }
        }

        private void MainVideoView_Prepared(object sender, EventArgs e)
        {
            (sender as MediaPlayer).Looping = true;
        }

        private void RenderVideo(IGalleryItem item)
        {
            LayoutRoot.Post(() => SetDimensions(MainVideoView, item));
            MainVideoView.Visibility = ViewStates.Visible;
            MainVideoView.SetVideoURI(global::Android.Net.Uri.Parse(item.Mp4));
            MainVideoView.Start();
            MainVideoView.Prepared += MainVideoView_Prepared;
        }

        private void RenderImage(IGalleryItem item)
        {
            LayoutRoot.Post(() => SetDimensions(MainImageView, item));
            MainImageView.Visibility = ViewStates.Visible;
            ImageService.Instance.LoadUrl(item.Link).Into(MainImageView);
        }

        private void SetDimensions(View view, IGalleryItem item)
        {
            var layParams = view.LayoutParameters;
            var width = Math.Min(LayoutRoot.Width, item.Width);
            var height = (int)Math.Ceiling((item.Height / (double)item.Width) * width);
            layParams.Width = width;
            layParams.Height = height;
            view.LayoutParameters = layParams;
        }

        private ImageViewAsync mainImageView;
        public ImageViewAsync MainImageView
        {
            get
            {
                mainImageView = mainImageView ?? FindViewById<ImageViewAsync>(Resource.Id.MainImageView);
                return mainImageView;
            }
        }

        private VideoView mainVideoView;
        public VideoView MainVideoView
        {
            get
            {
                mainVideoView = mainVideoView ?? FindViewById<VideoView>(Resource.Id.MainVideoView);
                return mainVideoView;
            }
        }

        private View layoutRoot;
        public View LayoutRoot
        {
            get
            {
                layoutRoot = layoutRoot ?? FindViewById<View>(Resource.Id.LayoutRoot);
                return layoutRoot;
            }
        }
    }
}