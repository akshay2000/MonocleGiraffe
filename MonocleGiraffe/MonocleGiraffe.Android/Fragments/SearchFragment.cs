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
using MonocleGiraffe.Portable.ViewModels.Front;
using GalaSoft.MvvmLight.Helpers;
using Android.Graphics.Drawables;
using MonocleGiraffe.Portable.Models;
using Android.Support.V7.Widget;
using FFImageLoading.Views;
using FFImageLoading;
using MonocleGiraffe.Android.Helpers;
using Android.Support.V4.Content;
using Android.Graphics;

namespace MonocleGiraffe.Android.Fragments
{
    public partial class SearchFragment : global::Android.Support.V4.App.Fragment
    {
        List<Binding> bindings = new List<Binding>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {            
            return inflater.Inflate(Resource.Layout.Front_Search, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            //Button bindings
            bindings.Add(this.SetBinding(() => Vm.IsReddit, () => RedditsButton.Background).ConvertSourceToTarget(ConvertBoolToDrawable));
            bindings.Add(this.SetBinding(() => Vm.IsPosts, () => PostsButton.Background).ConvertSourceToTarget(ConvertBoolToDrawable));
            bindings.Add(this.SetBinding(() => Vm.IsGifs, () => GifsButton.Background).ConvertSourceToTarget(ConvertBoolToDrawable));

            bindings.Add(this.SetBinding(() => Vm.QueryText, () => QueryEditText.Text, BindingMode.TwoWay));
            //Required for linker
            QueryEditText.TextChanged += delegate { };

            bindings.Add(this.SetBinding(() => Vm.IsLoading,
                () => ProgressBar.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget(flag => flag ? ViewStates.Visible : ViewStates.Gone));

            RedditsButton.Click += TypeButton_Click;
            PostsButton.Click += TypeButton_Click;
            GifsButton.Click += TypeButton_Click;

            QueryEditText.EditorAction += QueryEditText_EditorAction;
            QueryEditText.KeyPress += QueryEditText_KeyPress;
        }

        private void QueryEditText_KeyPress(object sender, View.KeyEventArgs e)
        {
            if(e.KeyCode == Keycode.Enter)
            {
                Vm.SearchCommand.Execute("default");
                RefreshUI();
            }
        }

        private void QueryEditText_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == global::Android.Views.InputMethods.ImeAction.Search)
            {
                Vm.SearchCommand.Execute("default");
                RefreshUI();
            }
        }

        private void TypeButton_Click(object sender, EventArgs e)
        {
            var type = (string)(sender as TextView)?.Tag;
            Vm.SearchCommand.Execute(type);
            RefreshUI();
        }

        private Drawable ConvertBoolToDrawable(bool flag)
        {            
            int ret = flag ? Utils.GetAccentColor(Context) : ContextCompat.GetColor(Context, Resource.Color.TabUnselected);
            return new ColorDrawable(new Color(ret));
        }

        private void RefreshUI()
        {
            ResultsView.ClearOnScrollListeners();
			QueryEditText.ClearFocus();
            if (Vm.IsReddit)
            {
                ResultsView.SetLayoutManager(new LinearLayoutManager(Context));
                var adapter = Vm.Subreddits.GetRecyclerAdapter(BindRedditView, Resource.Layout.Tmpl_SubredditResult, SubredditItemClick);
                ResultsView.SetAdapter(adapter);
				ResultsView.AddOnScrollListener(new ScrollListener(Vm.Subreddits));
            }
            else if (Vm.IsPosts)
            {
                ResultsView.SetLayoutManager(new StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.Vertical));
                var adapter = Vm.Posts.GetRecyclerAdapter(BindPostView, Resource.Layout.Tmpl_GalleryThumbnail, PostItemClick);
                ResultsView.SetAdapter(adapter);
                ResultsView.AddOnScrollListener(new ScrollListener(Vm.Posts));
            }
            else if (Vm.IsGifs)
            {
                ResultsView.SetLayoutManager(new GridLayoutManager(Context, 2));
                var adapter = Vm.Gifs.GetRecyclerAdapter(BindGifView, Resource.Layout.Tmpl_SubredditThumbnail, GifItemClick);
                ResultsView.SetAdapter(adapter);
                ResultsView.AddOnScrollListener(new ScrollListener(Vm.Gifs));
            }
        }

        private void SubredditItemClick(int oldPosition, View oldView, int position, View view)
        {
            Vm.SubredditTapped(position);
        }

        private void PostItemClick(int oldPosition, View oldView, int position, View view)
        {
            Vm.ImageTapped(position);
        }

        private void GifItemClick(int oldPosition, View oldView, int position, View view)
        {
            Vm.GifTapped(position);
        }

        private void BindRedditView(CachingViewHolder holder, SubredditItem item, int position)
        {
            holder.FindCachedViewById<TextView>(Resource.Id.TitleTextView).Text = item.Title.Replace("&amp;", "&");
            holder.FindCachedViewById<TextView>(Resource.Id.SubtitleTextView).Text = $"/r/{item.Url} • {item.Subscribers} Subscribers";

            var addButton = holder.FindCachedViewById<View>(Resource.Id.AddButton);
            var addButtonVisibilityBinding = new Binding<bool, ViewStates>(item, () => item.IsFavorited, addButton, () => addButton.Visibility).ConvertSourceToTarget((flag) => flag ? ViewStates.Gone : ViewStates.Visible);
            holder.DeleteBinding(nameof(addButtonVisibilityBinding)); 
            holder.SaveBinding(nameof(addButtonVisibilityBinding), addButtonVisibilityBinding);

            //addButton.SetCommand("Click", Vm.ToggleFavorite, item);

            var checkButton = holder.FindCachedViewById<View>(Resource.Id.CheckButton);
            var checkButtonVisibilityBinding = new Binding<bool, ViewStates>(item, () => item.IsFavorited, checkButton, () => checkButton.Visibility).ConvertSourceToTarget((flag) => !flag ? ViewStates.Gone : ViewStates.Visible);
            holder.DeleteBinding(nameof(checkButtonVisibilityBinding));
            holder.SaveBinding(nameof(checkButtonVisibilityBinding), checkButtonVisibilityBinding);

            //checkButton.SetCommand("Click", Vm.ToggleFavorite, item);

            var toggleContainer = holder.FindCachedViewById<View>(Resource.Id.ToggleContainer);
            toggleContainer.SetCommand("Click", Vm.ToggleFavorite, item);
        }

        private void BindPostView(CachingViewHolder holder, GalleryItem item, int position)
        {
            var thumbnail = holder.FindCachedViewById<ImageViewAsync>(Resource.Id.Thumbnail);
            thumbnail.Post(() =>
            {
                var height = item.BigThumbRatio * thumbnail.Width;
                var layoutParams = thumbnail.LayoutParameters;
                layoutParams.Height = (int)Math.Floor(height);
                thumbnail.LayoutParameters = layoutParams;
            });
            ImageService.Instance.LoadUrl(item.BigThumbnail).Into(thumbnail);

            var title = holder.FindCachedViewById<TextView>(Resource.Id.TitleTextView);
            title.Text = item.Title;
            var ups = holder.FindCachedViewById<TextView>(Resource.Id.UpsTextView);
            ups.Text = item.Ups.ToString();
            var comments = holder.FindCachedViewById<TextView>(Resource.Id.CommentsTextView);
            comments.Text = item.CommentCount.ToString();
        }

        private void BindGifView(CachingViewHolder holder, GalleryItem item, int position)
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach((b) => b.Detach());
        }

        public SearchViewModel Vm { get { return App.Locator.Front.SearchVM; } }
    }
}