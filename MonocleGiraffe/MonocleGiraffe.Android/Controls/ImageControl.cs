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
using static MonocleGiraffe.Android.Helpers.Utils;
using MonocleGiraffe.Portable.Models;
using FFImageLoading.Views;
using FFImageLoading;
using Android.Media;
using Android.Util;

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
			//LayoutRoot.LongClick += (sender, args) => { 
			//	global::Android.Util.Log.Info("ImageControl", "Long pressed");
			//	var menu = new PopupMenu(Context, this);
			//	menu.Inflate(Resource.Menu.main_menu);
			//	menu.Show();
			//};
        }

		private IGalleryItem item;

		public void RenderContent(IGalleryItem itemToRender)
        {
			item = itemToRender;
            switch (itemToRender.ItemType)
            {
                case GalleryItemType.Animation:
                    RenderVideo(itemToRender);
                    break;
                case GalleryItemType.Image:
                    RenderImage(itemToRender);
                    break;
                default:
                    throw new NotSupportedException($"Itemtype {itemToRender.ItemType.ToString()} is not supported.");
            }
		}

		private void RenderVideo(IGalleryItem itemToRender)
        {
			MainImageView.Visibility = ViewStates.Gone;
			LayoutRoot.Post(() => { SetDimensions(MainVideoView, itemToRender); });
			VideoWrapper.Alpha = 0;
			VideoWrapper.Visibility = ViewStates.Visible;
			
            MainVideoView.SetVideoURI(global::Android.Net.Uri.Parse(itemToRender.Mp4));
   //         var controller = new MediaController(Context)
   //		  controller.SetAnchorView(this);
   //         controller.SetMediaPlayer(MainVideoView);
   //         MainVideoView.SetMediaController(controller);
			MainVideoView.Prepared += (sender, e) =>
			{
				var mp = sender as MediaPlayer;
				(sender as MediaPlayer).Looping = true;
				VideoWrapper.Alpha = 1;
				MainVideoView.Start();
			};
        }

        private void RenderImage(IGalleryItem item)
        {
			MainImageView.SetImageResource(global::Android.Resource.Color.Transparent);
            LayoutRoot.Post(() => SetDimensions(MainImageView, item));
            MainImageView.Visibility = ViewStates.Visible;
			VideoWrapper.Visibility = ViewStates.Gone;
			MainVideoView.Visibility = ViewStates.Gone;
            var width = DpToPx(Math.Min(PxToDp(LayoutRoot.Width, Resources), item.Width), Resources);
			ImageService.Instance
				.LoadUrl(item.Link)
				.DownSample(width)
				.Into(MainImageView);
        }

		private void SetDimensions(View view, IGalleryItem itemToRender)
        {
            var layParams = view.LayoutParameters;
            var width = DpToPx(Math.Min(PxToDp(LayoutRoot.Width, Resources), itemToRender.Width), Resources);
            var height = (int)Math.Ceiling((itemToRender.Height / (double)itemToRender.Width) * width);
            layParams.Width = width;
            layParams.Height = height;
            view.LayoutParameters = layParams;
        }

		#region UI

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

		private View videoWrapper;
		public View VideoWrapper
		{
			get
			{
				videoWrapper = videoWrapper ?? FindViewById<View>(Resource.Id.VideoWrapper);
				return videoWrapper;
			}
		}

		#endregion

        //private class GestureListener : GestureDetector.SimpleOnGestureListener
        //{
        //    VideoView view;
        //    public GestureListener(VideoView view)
        //    {
        //        this.view = view;
        //    }

        //    public override bool OnSingleTapConfirmed(MotionEvent e)
        //    {
        //        if (view.IsPlaying)
        //            view.Pause();
        //        else
        //            view.Start();
        //        return true;
        //    }
        //}
    }
}