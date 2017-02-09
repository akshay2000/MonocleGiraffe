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
using MonocleGiraffe.Android.Helpers;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using System.Collections.ObjectModel;

namespace MonocleGiraffe.Android.Fragments
{
    public class BrowserItemFragment : global::Android.Support.V4.App.Fragment
    {
        private IGalleryItem Item { get; set; }
        public GalleryItem GalleryItem { get { return Item as GalleryItem; } }

        private bool isAlbum;
        private List<Binding> bindings = new List<Binding>();

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
            Item = (Activity as BrowserActivity)?.Vm.Images.ElementAt(position);
            isAlbum = Item.ItemType == GalleryItemType.Album;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            int layout = isAlbum ? Resource.Layout.Tmpl_Album : Resource.Layout.Tmpl_Image;
            return inflater.Inflate(layout, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            RenderHeader(Item);
            if (isAlbum)
                RenderAlbum(Item);
            else
                RenderImage(Item);
            BindVotes();
            base.OnViewCreated(view, savedInstanceState);
            AnalyticsHelper.SendView("BrowserItem");
        }

        private void BindVotes()
        {
            if (GalleryItem == null)
                return;
            bindings.Add(this.SetBinding(() => GalleryItem.IsUpVoted)
                .WhenSourceChanges(() => SetColor(GalleryItem.IsUpVoted, UpvoteButton, new global::Android.Graphics.Color(ContextCompat.GetColor(Activity, Resource.Color.Upvote)))));
            UpvoteButton.SetCommand("Click", GalleryItem.VoteCommand, "up");
            bindings.Add(this.SetBinding(() => GalleryItem.IsDownVoted)
                .WhenSourceChanges(() => SetColor(GalleryItem.IsDownVoted, DownvoteButton, new global::Android.Graphics.Color(ContextCompat.GetColor(Activity, Resource.Color.Downvote)))));
            DownvoteButton.SetCommand("Click", GalleryItem.VoteCommand, "down");
            bindings.Add(this.SetBinding(() => GalleryItem.IsFavourited)
                .WhenSourceChanges(() => SetColor(GalleryItem.IsFavourited, FavoriteButton, new global::Android.Graphics.Color(ContextCompat.GetColor(Activity, Resource.Color.Favorite)))));
            FavoriteButton.SetCommand("Click", GalleryItem.Favourite);
        }

        private void SetColor(bool state, ImageView image, global::Android.Graphics.Color color)
        {
            var toSet = state ? color : new global::Android.Graphics.Color(ContextCompat.GetColor(Activity, Resource.Color.Veto));
            image.SetColorFilter(toSet);
        }

        private void RenderHeader(IGalleryItem item)
        {
            Title.Text = item.Title;
            string color = Utils.GetAccentColorHex(Activity);
            SubTitle.SetText(Utils.FromHtml($"by <b><font color='{color}'>{item.UploaderName}</font></b> • {item.Ups} points"), TextView.BufferType.Spannable);
        }

        private void RenderImage(IGalleryItem item)
        {
            var hasDescription = !string.IsNullOrEmpty(item.Description);
            //var descView = Description;
            if (hasDescription)
                Description.Text = item.Description;
            else
                Description.Visibility = ViewStates.Gone;
            MainImage.RenderContent(item);
            
        }

        private void RenderAlbum(IGalleryItem item)
        {
            Title.Text = item.Title;
            AlbumRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            bindings.Add(this.SetBinding(() => Item.AlbumImages).WhenSourceChanges(UpdateAlbumAdapter));
        }

        private void UpdateAlbumAdapter()
        {
            if (Item.AlbumImages == null)
                return;
            var bindableCollection = new ObservableCollection<GalleryItem>();
            var adapter = bindableCollection
                .GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_Item_Album);
            AlbumRecyclerView.ClearOnScrollListeners();
            var scrollListener = new AlbumScrollListener(Item.AlbumImages, bindableCollection);
            AlbumRecyclerView.AddOnScrollListener(scrollListener);
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
			//this.Activity.RegisterForContextMenu(image);
            image.RenderContent(item);

            var hasDescription = !string.IsNullOrEmpty(item.Description);
            var descView = holder.FindCachedViewById<TextView>(Resource.Id.DescriptionTextView);
            if (hasDescription)
                descView.Text = item.Description;
            else
                descView.Visibility = ViewStates.Gone;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach(b => b.Detach());
        }

        #region Views

        private AppBarLayout appBarLayout;
        public AppBarLayout AppBarLayout
        {
            get
            {
                if (!isAlbum)
                    return null;
                appBarLayout = appBarLayout ?? View.FindViewById<AppBarLayout>(Resource.Id.AppBar);
                return appBarLayout;
            }
        }

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

        private TextView subTitle;
        public TextView SubTitle
        {
            get
            {
                subTitle = subTitle ?? View.FindViewById<TextView>(Resource.Id.SubTitleTextView);
                return subTitle;
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

        private ImageView upvoteButton;
        public ImageView UpvoteButton
        {
            get
            {
                upvoteButton = upvoteButton ?? View.FindViewById<ImageView>(Resource.Id.UpvoteButton);
                return upvoteButton;
            }
        }

        private ImageView downButton;
        public ImageView DownvoteButton
        {
            get
            {
                downButton = downButton ?? View.FindViewById<ImageView>(Resource.Id.DownvoteButton);
                return downButton;
            }
        }

        private ImageView favoriteButton;
        public ImageView FavoriteButton
        {
            get
            {
                favoriteButton = favoriteButton ?? View.FindViewById<ImageView>(Resource.Id.FavoriteButton);
                return favoriteButton;
            }
        }

        #endregion
    }

	class PrefetchLinearLayoutManager : LinearLayoutManager
	{
		private Context context;
		private int? prefetchSize;

		public PrefetchLinearLayoutManager(Context context, int? prefetchSize = null) : base(context)
		{
			this.context = context;
			this.prefetchSize = prefetchSize;
		}

		public PrefetchLinearLayoutManager(IntPtr i, JniHandleOwnership o) : base(i, o)
		{ }

		public PrefetchLinearLayoutManager(Context context, IAttributeSet a, int i1, int i2) : base(context, a, i1, i2)
		{ 
			this.context = context;
		}

		public PrefetchLinearLayoutManager(Context context, int orientation, bool isReverse) : base(context, orientation, isReverse)
		{
			this.context = context;
		}

		protected override int GetExtraLayoutSpace(RecyclerView.State state)
		{
			return prefetchSize ?? 1920 * 2;
		}
	}
}