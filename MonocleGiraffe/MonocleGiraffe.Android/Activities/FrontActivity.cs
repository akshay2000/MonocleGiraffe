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
using GalaSoft.MvvmLight.Views;
using Android.Support.V4.App;
using MonocleGiraffe.Android.Fragments;
using Android.Util;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Java.Lang;
using Android.Support.V4.Content;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "FrontActivity")]
    public class FrontActivity : FragmentActivity
    {
        FrontPagerAdapter pagerAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            
            SetContentView(Resource.Layout.Front);
            pagerAdapter = new FrontPagerAdapter(SupportFragmentManager);
            var pager = FindViewById<ViewPager>(Resource.Id.MainPager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.Tabs);
            tabLayout.SetupWithViewPager(pager);
            
            pager.OffscreenPageLimit = 3;
            pager.Adapter = pagerAdapter;

            SetTabContent(tabLayout);

            tabLayout.TabSelected += TabLayout_TabSelected;
            tabLayout.TabUnselected += TabLayout_TabUnselected;

            var selectedTab = tabLayout.GetTabAt(pager.CurrentItem);
            var imageView = selectedTab.CustomView as ImageView;
            imageView.SetColorFilter(new global::Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.TabSelected)));
        }

        private void TabLayout_TabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
        {
            var resolved = ContextCompat.GetColor(this, Resource.Color.TabUnselected);
            var tab = e.Tab;
            var imageView = tab.CustomView as ImageView;
            imageView.SetColorFilter(new global::Android.Graphics.Color(resolved));
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            var resolved = ContextCompat.GetColor(this, Resource.Color.TabSelected);
            var tab = e.Tab;
            var imageView = tab.CustomView as ImageView;
            imageView.SetColorFilter(new global::Android.Graphics.Color(resolved));
        }       

        private void SetTabContent(TabLayout layout)
        {
            for (int i = 0; i < layout.TabCount; i++)
            {
                layout.GetTabAt(i).SetCustomView(pagerAdapter.GetTabView(i, LayoutInflater));
            }
        }
                 
    }

    public class FrontPagerAdapter : FragmentPagerAdapter
    {
        public FrontPagerAdapter(global::Android.Support.V4.App.FragmentManager f) : base(f) { }

        public override int Count { get { return 3; } }

        public override global::Android.Support.V4.App.Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return Gallery;
                case 1:
                    return Reddits;
                case 2:
                    return Search;
                case 3:
                    return Account;
            }
            Log.Debug("FrontActivity", $"position was {position}");
            return null;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            switch (position)
            {
                case 0:
                    return new Java.Lang.String(nameof(Gallery));
                case 1:
                    return new Java.Lang.String(nameof(Reddits));
                case 2:
                    return new Java.Lang.String(nameof(Search));
                case 3:
                    return new Java.Lang.String(nameof(Account));
            }
            return new Java.Lang.String("Unsupported");
        }

        public View GetTabView(int index, LayoutInflater inflater)
        {
            View view = inflater.Inflate(Resource.Layout.Ctrl_Tab, null);
            var image = view.FindViewById<ImageView>(Resource.Id.TabIcon);
            image.SetImageResource(Resource.Drawable.Home);
            return view;
        }

        private GalleryFragment gallery;
        private GalleryFragment Gallery
        {
            get
            {
                gallery = gallery ?? new GalleryFragment();
                return gallery;
            }
        }

        private RedditFragment reddits;
        private RedditFragment Reddits
        {
            get
            {
                reddits = reddits ?? new RedditFragment();
                return reddits;
            }
        }

        private SearchFragment search;
        private SearchFragment Search
        {
            get
            {
                search = search ?? new SearchFragment();
                return search;
            }
        }

        private AccountFragment account;
        public AccountFragment Account
        {
            get
            {
                account = account ?? new AccountFragment();
                return account;
            }
        }

    }
}