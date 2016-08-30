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
using GalaSoft.MvvmLight.Helpers;
using MonocleGiraffe.Portable.ViewModels;
using MonocleGiraffe.Portable.Models;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Android.Support.V7.Widget;
using FFImageLoading.Views;
using FFImageLoading;
using MonocleGiraffe.Android.Controls;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "SubGalleryActivity")]
    public class SubGalleryActivity : Activity
    {
        private readonly List<Binding> bindings = new List<Binding>();
        private ObservableRecyclerAdapter<GalleryItem, CachingViewHolder> adapter;
        private GridAutofitLayoutManager layoutManager;

        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SubGallery);
            layoutManager = new GridAutofitLayoutManager(this, 240);
            SubGalleryRecyclerView.SetLayoutManager(layoutManager);

            //Hacky way to bind
            Vm.PropertyChanged += Vm_PropertyChanged;

            var param = Nav.GetAndRemoveParameter<string>(Intent);
            Vm.Activate(param);

            Vm.Images.LoadMoreAsync(60);
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Images")
                BindCollection();
        }

        private void BindCollection()
        {
            adapter = Vm.Images.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_SubredditThumbnail);
            SubGalleryRecyclerView.SetAdapter(adapter);
        }

        private void BindViewHolder(CachingViewHolder holder, GalleryItem item, int position)
        {
            var layoutRoot = holder.ItemView;
            layoutRoot.Post(() =>
            {
                var width = layoutRoot.Width;
                var layoutParams = layoutRoot.LayoutParameters;
                layoutParams.Height = width;
                layoutRoot.LayoutParameters = layoutParams;
            });
            var thumbnailView = holder.FindCachedViewById<ImageViewAsync>(Resource.Id.Thumbnail);
            ImageService.Instance.LoadUrl(item.Thumbnail).Into(thumbnailView);
        }

        public SubGalleryViewModel Vm { get { return App.Locator.SubGallery; } }

        private RecyclerView subGalleryRecyclerView;
        public RecyclerView SubGalleryRecyclerView
        {
            get
            {
                subGalleryRecyclerView = subGalleryRecyclerView ?? FindViewById<RecyclerView>(Resource.Id.SubGalleryRecyclerView);
                return subGalleryRecyclerView;
            }
        }
    }
}