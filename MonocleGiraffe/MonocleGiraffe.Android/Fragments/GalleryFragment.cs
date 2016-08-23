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
using MonocleGiraffe.Portable.ViewModels.Front;

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
            //Hacky way to bind
            BindCollection();
            Vm.PropertyChanged += Vm_PropertyChanged;

            Vm.Images.LoadMoreAsync(60);
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Images")
                BindCollection();
        }

        private void BindCollection()
        {
            var adapter = Vm.Images.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_SubredditThumbnail);
            GalleryRecyclerView.SetAdapter(adapter);
        }

        private void BindViewHolder(CachingViewHolder holder, GalleryItem item, int position)
        {
            var textView = holder.FindCachedViewById<TextView>(Resource.Id.textView1);
            textView.Text = item.Title;
        }

        public GalleryViewModel Vm { get { return App.Locator.Front.GalleryVM; } }

        private RecyclerView galleryRecyclerView;
        public RecyclerView GalleryRecyclerView
        {
            get
            {
                galleryRecyclerView = galleryRecyclerView ?? View.FindViewById<RecyclerView>(Resource.Id.GalleryRecyclerView);
                return galleryRecyclerView;
            }
        }

        public override void OnDestroyView()
        {
            galleryRecyclerView = null;
            base.OnDestroyView();
        }        
    }
}