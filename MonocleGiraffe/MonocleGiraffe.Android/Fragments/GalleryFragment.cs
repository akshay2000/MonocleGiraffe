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
using FFImageLoading.Views;
using FFImageLoading;
using MonocleGiraffe.Android.Helpers;

namespace MonocleGiraffe.Android.Fragments
{
    public class GalleryFragment : global::Android.Support.V4.App.Fragment
    {
        private List<Binding> bindings = new List<Binding>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.Front_Gallery, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            bindings.Add(this.SetBinding(() => Vm.Images).WhenSourceChanges(BindCollection));
			bindings.Add(this.SetBinding(() => Vm.Topics).WhenSourceChanges(BindTopics));

			TopicsSpinner.ItemSelected += TopicsSpinner_ItemSelected;
        }
        
        private void BindCollection()
        {
			var layoutManager = new AutoFitStaggeredLayoutManager(180, StaggeredGridLayoutManager.Vertical, Context);
			GalleryRecyclerView.SetLayoutManager(layoutManager);
			var adapter = Vm.Images.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_GalleryThumbnail, ItemClicked);
            GalleryRecyclerView.SetAdapter(adapter);
            GalleryRecyclerView.ClearOnScrollListeners();
			var listener = new ScrollListener(Vm.Images);
            GalleryRecyclerView.AddOnScrollListener(listener);
        }

		private void ItemClicked(int oldPosition, View oldView, int position, View view)
		{
			Vm.ImageTapped(position);
		}

		private void BindTopics()
		{
			if (Vm.Topics == null || Vm.Topics.Count == 0)
				return;
			var adapter = new ArrayAdapter<string>(Context, global::Android.Resource.Layout.SimpleSpinnerItem, Vm.Topics.Select(t => t.Name).ToList());
			adapter.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
			TopicsSpinner.Adapter = adapter;
		}

		void TopicsSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Vm.TopicTapped(e.Position);
		}

        private void BindViewHolder(CachingViewHolder holder, GalleryItem item, int position)
        {
            var thumbnail = holder.FindCachedViewById<ImageViewAsync>(Resource.Id.Thumbnail);
			thumbnail.SetImageResource(global::Android.Resource.Color.Transparent);
            thumbnail.Post(() =>
            {
                var height = item.BigThumbRatio * thumbnail.Width;
                var layoutParams = thumbnail.LayoutParameters;
                layoutParams.Height = (int)Math.Floor(height);
                thumbnail.LayoutParameters = layoutParams;
            });
            holder.DeleteBinding(thumbnail);
            var imageBinding = new Binding<string, string>(item, () => item.BigThumbnail).WhenSourceChanges(() => { ImageService.Instance.LoadUrl(item.BigThumbnail).Into(thumbnail); });
            holder.SaveBinding(thumbnail, imageBinding);
            

            var title = holder.FindCachedViewById<TextView>(Resource.Id.TitleTextView);
            title.Text = item.Title;
            var ups = holder.FindCachedViewById<TextView>(Resource.Id.UpsTextView);
            ups.Text = item.Ups.ToString();
            var comments = holder.FindCachedViewById<TextView>(Resource.Id.CommentsTextView);
            comments.Text = item.CommentCount.ToString();
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

        private LinearLayout layoutRoot;
        public LinearLayout LayoutRoot
        {
            get
            {
                layoutRoot = layoutRoot ?? View.FindViewById<LinearLayout>(Resource.Id.LayoutRoot);
                return layoutRoot;
            }
        }

		private Spinner topicsSpinner;
		public Spinner TopicsSpinner
		{
			get
			{
				topicsSpinner = topicsSpinner ?? View.FindViewById<Spinner>(Resource.Id.TopicsSpinner);
				return topicsSpinner;
			}
		}

        public override void OnDestroyView()
        {
            galleryRecyclerView = null;
            base.OnDestroyView();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach((b) => b.Detach());
        }
    }
    
    public class AutoFitStaggeredLayoutManager : StaggeredGridLayoutManager
    {
        private int ColumnWidth { get; set; }
        private Context Context { get; set; }
        public AutoFitStaggeredLayoutManager(int columnWidth, int orientation, Context context) : base (1, orientation)
        {
            ColumnWidth = columnWidth;
            Context = context;
        }

        private int oldWidth = 0;
        public override void OnMeasure(RecyclerView.Recycler recycler, RecyclerView.State state, int widthSpec, int heightSpec)
        {
            int width = Width;
            bool isWidthChanged = oldWidth != width;
            if (isWidthChanged && ColumnWidth > 0 && width > 0)
            {
                oldWidth = width;
                int totalWidth = width - PaddingRight - PaddingLeft;
                int totalWidthInDp = Utils.PxToDp(totalWidth, Context.Resources);
                int spanCount = Math.Max(1, totalWidthInDp / ColumnWidth);
                SpanCount = spanCount;
            }
            base.OnMeasure(recycler, state, widthSpec, heightSpec);
        }        
    }
}