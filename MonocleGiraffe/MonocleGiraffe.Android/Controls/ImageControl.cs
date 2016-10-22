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
       // GestureDetector detector;

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

        private void RenderVideo(IGalleryItem item)
        {
            //detector = new GestureDetector(new GestureListener(MainVideoView));
            LayoutRoot.Post(() => SetDimensions(MainVideoView, item));
            MainVideoView.Visibility = ViewStates.Visible;
            MainVideoView.SetVideoURI(global::Android.Net.Uri.Parse(item.Mp4));
            MediaController controller = new MediaController(Context);
            controller.SetAnchorView(MainVideoView);
            controller.SetMediaPlayer(MainVideoView);
            MainVideoView.SetMediaController(controller);
            //MainVideoView.Start();
            //MainVideoView.SeekTo(100);
           // MainVideoView.Touch += MainVideoView_Touch;
            MainVideoView.Prepared += MainVideoView_Prepared;
        }

        private void MainVideoView_Touch(object sender, TouchEventArgs e)
        {
            //detector.OnTouchEvent(e.Event);
        }

        private void MainVideoView_Prepared(object sender, EventArgs e)
        {
            (sender as MediaPlayer).Looping = true;
        }

        private void RenderImage(IGalleryItem item)
        {
            var imageService = ImageService.Instance;
            imageService.LoadFileFromApplicationBundle("DummyImage.png").Into(MainImageView);
            LayoutRoot.Post(() => SetDimensions(MainImageView, item));
            MainImageView.Visibility = ViewStates.Visible;
            var width = DpToPx(Math.Min(PxToDp(LayoutRoot.Width), item.Width));
            imageService.LoadUrl(item.Link).DownSample(width).Into(MainImageView);
        }

        private void SetDimensions(View view, IGalleryItem item)
        {
            var layParams = view.LayoutParameters;
            var width = DpToPx(Math.Min(PxToDp(LayoutRoot.Width), item.Width));
            var height = (int)Math.Ceiling((item.Height / (double)item.Width) * width);
            layParams.Width = width;
            layParams.Height = height;
            view.LayoutParameters = layParams;
        }

        private int PxToDp(int px)
        {
            int dp = (int)Math.Round(px / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int DpToPx(int dp)
        {
            int px = (int)Math.Round(dp * Resources.DisplayMetrics.Density + 0.5f);
            return px;
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

        private class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            VideoView view;
            public GestureListener(VideoView view)
            {
                this.view = view;
            }

            public override bool OnSingleTapConfirmed(MotionEvent e)
            {
                if (view.IsPlaying)
                    view.Pause();
                else
                    view.Start();
                return true;
            }
        }
    }
}