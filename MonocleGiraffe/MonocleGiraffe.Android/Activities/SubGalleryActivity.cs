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
using MonocleGiraffe.Android.Helpers;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "SubGalleryActivity")]
    public class SubGalleryActivity : AppCompatActivity
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

            SetSupportActionBar(MainToolbar);

            layoutManager = new GridAutofitLayoutManager(this, 120);
            SubGalleryRecyclerView.SetLayoutManager(layoutManager);

            SwipeView.SetCommand("Refresh", Vm.RefreshCommand);

            bindings.Add(this.SetBinding(() => Vm.Images).WhenSourceChanges(BindCollection));
            bindings.Add(this.SetBinding(() => Vm.Sub.Title, () => SupportActionBar.Title));
            bindings.Add(this.SetBinding(() => Vm.Images.IsBusy, () => SwipeView.Refreshing));

            var param = Nav.GetAndRemoveParameter<string>(Intent);
            Vm.Activate(param);
            AnalyticsHelper.SendView("SubredditGallery");
        }

        private void BindCollection()
        {
            adapter = Vm.Images.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_SubredditThumbnail, ItemClicked);
            SubGalleryRecyclerView.SetAdapter(adapter);
            SubGalleryRecyclerView.ClearOnScrollListeners();
			var listener = new ScrollListener(Vm.Images);
            SubGalleryRecyclerView.AddOnScrollListener(listener);
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
			thumbnailView.SetImageResource(global::Android.Resource.Color.Transparent);
            holder.DeleteBinding(thumbnailView);
            var thumbnailBinding = new Binding<string, string>(item, () => item.Thumbnail)
                .WhenSourceChanges(() => ImageService.Instance.LoadUrl(item.Thumbnail).Into(thumbnailView));
            holder.SaveBinding(thumbnailView, thumbnailBinding);
        }

        private void ItemClicked(int oldPosition, View oldView, int position, View view)
        {
            Vm.ImageTapped(position);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach((b) => b.Detach());
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

        private global::Android.Support.V7.Widget.Toolbar mainToolbar;
        public global::Android.Support.V7.Widget.Toolbar MainToolbar
        {
            get
            {
                mainToolbar = mainToolbar ?? FindViewById<global::Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolbar);
                return mainToolbar;
            }
        }

        private SwipeRefreshLayout swipeView;
        public SwipeRefreshLayout SwipeView
        {
            get
            {
                swipeView = swipeView ?? FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeView);
                return swipeView;
            }
        }
    }
}