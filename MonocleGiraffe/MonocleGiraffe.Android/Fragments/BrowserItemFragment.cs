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
using Android.Support.V7.Widget;
using GalaSoft.MvvmLight.Helpers;

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
            if (isAlbum)
                RenderAlbum(item);
            else
                RenderImage(item);
            base.OnViewCreated(view, savedInstanceState);
        }

        private void RenderImage(IGalleryItem item)
        {
            Title.Text = item.Title;
            MainImage.RenderContent(item);
            var hasDescription = !string.IsNullOrEmpty(item.Description);
            var descView = Description;
            if (hasDescription)
                descView.Text = item.Description;
            else
                descView.Visibility = ViewStates.Gone;
        }

        private void RenderAlbum(IGalleryItem item)
        {
            Title.Text = item.Title;
            AlbumRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            var adapter = item.AlbumImages.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_Item_Album);
            AlbumRecyclerView.SetAdapter(adapter);
        }

        private void BindViewHolder(CachingViewHolder holder, GalleryItem item, int position)
        {
            var hasTitle = !string.IsNullOrEmpty(item.Title);
            var titleView = holder.FindCachedViewById<TextView>(Resource.Id.TitleTextView);
            if (hasTitle)
                titleView.Text = item.Title;
            else
                titleView.Visibility = ViewStates.Gone;

            var image = holder.FindCachedViewById<ImageControl>(Resource.Id.MainImage);
            image.RenderContent(item);

            var hasDescription = !string.IsNullOrEmpty(item.Description);
            var descView = holder.FindCachedViewById<TextView>(Resource.Id.DescriptionTextView);
            if (hasDescription)
                descView.Text = item.Description;
            else
                descView.Visibility = ViewStates.Gone;
        }

        #region Views

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

        private RecyclerView albumRecyclerView;
        public RecyclerView AlbumRecyclerView
        {
            get
            {
                albumRecyclerView = albumRecyclerView ?? View.FindViewById<RecyclerView>(Resource.Id.AlbumRecyclerView);
                return albumRecyclerView;
            }
        }

        private TextView description;
        public TextView Description
        {
            get
            {
                description = description ?? View.FindViewById<TextView>(Resource.Id.DescriptionTextView);
                return description;
            }
        }

        #endregion
    }
}