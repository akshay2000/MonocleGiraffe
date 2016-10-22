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
            var actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.SetDisplayShowTitleEnabled(false);

            SetContentView(Resource.Layout.Front);
            actionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            pagerAdapter = new FrontPagerAdapter(SupportFragmentManager);
            var pager = FindViewById<ViewPager>(Resource.Id.MainPager);
            pager.PageSelected += (o, e) => actionBar.SetSelectedNavigationItem(e.Position);
            pager.OffscreenPageLimit = 3;
            pager.Adapter = pagerAdapter;

            actionBar.AddTab(pager.GetViewPageTab(ActionBar, "Gallery"));
            actionBar.AddTab(pager.GetViewPageTab(ActionBar, "Reddits"));
            actionBar.AddTab(pager.GetViewPageTab(ActionBar, "Search"));
            //actionBar.AddTab(pager.GetViewPageTab(ActionBar, "Account"));
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

    public static class ViewPagerExtensions
    {
        public static ActionBar.Tab GetViewPageTab(this ViewPager viewPager, ActionBar actionBar, string name)
        {
            var tab = actionBar.NewTab();
            tab.SetText(name);
            tab.TabSelected += (o, e) =>
            {
                viewPager.SetCurrentItem(actionBar.SelectedNavigationIndex, true);
            };
            return tab;
        }
    }
}