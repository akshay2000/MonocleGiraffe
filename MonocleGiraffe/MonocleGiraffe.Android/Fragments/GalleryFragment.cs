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
using MonocleGiraffe.Portable.ViewModels;
using Android.Support.V7.Widget;
using MonocleGiraffe.Portable.Models;
using GalaSoft.MvvmLight.Helpers;

namespace MonocleGiraffe.Android.Fragments
{
    public class GalleryFragment : global::Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.Front_Gallery, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            GalleryRecyclerView.SetLayoutManager(new StaggeredGridLayoutManager(3, StaggeredGridLayoutManager.Vertical));
            var adapter = Vm.GalleryVM.Images.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_SubredditThumbnail);
            GalleryRecyclerView.SetAdapter(adapter);
            Vm.GalleryVM.Images.LoadMoreAsync(60);
        }

        private void BindViewHolder(CachingViewHolder holder, GalleryItem item, int position)
        {
            var textView = holder.FindCachedViewById<TextView>(Resource.Id.textView1);
            textView.Text = item.Title;
        }

        public FrontViewModel Vm { get { return App.Locator.Front; } }

        private RecyclerView galleryRecyclerView;
        public RecyclerView GalleryRecyclerView
        {
            get
            {
                galleryRecyclerView = galleryRecyclerView ?? View.FindViewById<RecyclerView>(Resource.Id.GalleryRecyclerView);
                return galleryRecyclerView;
            }
        }
    }
}