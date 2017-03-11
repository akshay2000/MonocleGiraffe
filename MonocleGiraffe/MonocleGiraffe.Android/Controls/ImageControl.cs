using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using MonocleGiraffe.Android.Fragments;
using Android.Support.V4.App;
using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Android.LibraryImpl;
using MonocleGiraffe.Portable.Interfaces;
using MonocleGiraffe.Android.ViewModels;

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
            LayoutRoot.LongClick += LayoutRoot_LongClick;
        }

        private void LayoutRoot_LongClick(object sender, LongClickEventArgs e)
        {
            BrowserSheetFragment f = new BrowserSheetFragment();
            f.Show(fragmentManager, f.Tag);
            f.MenuTapped += async (s, a) =>
            {
                switch (a)
                {
                    case BrowserSheetFragment.MenuItem.Save:
                        switch (item.ItemType)
                        {
                            case GalleryItemType.Animation:
                                await App.Locator.Transfers.DownloadsVM.StartDownload(item.Mp4);
                                break;
                            case GalleryItemType.Image:
                                await App.Locator.Transfers.DownloadsVM.StartDownload(item.Link);
                                break;
                        }
                        break;
                    case BrowserSheetFragment.MenuItem.Share:
                        Portable.Helpers.Initializer.SharingHelper.ShareItem(item);
                        break;
                    case BrowserSheetFragment.MenuItem.CopyLink:
                        Portable.Helpers.Initializer.ClipboardHelper.Clip(item.Link);                       
                        break;
                }
                f.Dismiss();
            };

        }

        private IGalleryItem item;
        private FragmentManager fragmentManager;
		public void RenderContent(IGalleryItem itemToRender, FragmentManager fragmentManager)
        {
            this.fragmentManager = fragmentManager;
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
    }
}