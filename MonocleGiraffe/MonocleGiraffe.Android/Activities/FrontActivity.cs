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
using Android.Support.V7.App;
using Microsoft.Practices.ServiceLocation;
using MonocleGiraffe.Portable.ViewModels;
using MonocleGiraffe.Android.Helpers;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "FrontActivity")]
    public class FrontActivity : AppCompatActivity
    {
		public NavigationService Nav
		{
			get
			{
				return (NavigationService)ServiceLocator.Current
					.GetInstance<INavigationService>();
			}
		}

		public FrontViewModel Vm { get { return App.Locator.Front; } }

        FrontPagerAdapter pagerAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            
            SetContentView(Resource.Layout.Front);

            //Utils.SetPaddingForStatusBar(this, MainToolbar);

            pagerAdapter = new FrontPagerAdapter(SupportFragmentManager);
            var pager = FindViewById<ViewPager>(Resource.Id.MainPager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.Tabs);
            tabLayout.SetupWithViewPager(pager);
            
            pager.OffscreenPageLimit = 3;
            pager.Adapter = pagerAdapter;
            pager.PageSelected += Pager_PageSelected;

            SetTabContent(tabLayout);
            SetSupportActionBar(MainToolbar);
            
            tabLayout.TabSelected += TabLayout_TabSelected;
            tabLayout.TabUnselected += TabLayout_TabUnselected;

            //Launch setup
            int currentIndex = pager.CurrentItem;
            var selectedTab = tabLayout.GetTabAt(currentIndex);
            var imageView = selectedTab.CustomView as ImageView;
            imageView.SetColorFilter(new global::Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.TabSelected)));
            var title = pagerAdapter.GetTitle(currentIndex);
            SupportActionBar.Title = title;
            AnalyticsHelper.SendView(title);            
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main_menu, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var itemId = item.ItemId;
			switch (itemId)
			{
				case Resource.Id.SettingsMenuItem:
					StartActivity(new Intent(this, typeof(SettingsActivity)));
					break;
				case Resource.Id.DownloadsMenuItem:
					break;
				case Resource.Id.FeedbackMenuItem:
					LaunchFeedbackEmail();
					break;
			}
			return true;
		}

		private void LaunchFeedbackEmail()
		{
			var emailIntent = new Intent(Intent.ActionSend);
			emailIntent.SetType("plain/text");
			emailIntent.PutExtra(Intent.ExtraEmail, new string[] { "akshay2000+mg@hotmail.com" });
			emailIntent.PutExtra(Intent.ExtraSubject, "Monocle Giraffe for Android");
			emailIntent.PutExtra(Intent.ExtraText, "Write your feedback below\n_________________________");

			StartActivity(Intent.CreateChooser(emailIntent, "Send Feedback"));
		}

        private void Pager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            var title = pagerAdapter.GetTitle(e.Position);
            SupportActionBar.Title = title;
            AnalyticsHelper.SendView(title);
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
        
        private int[] tabIcons = new int[] { Resource.Drawable.home, Resource.Drawable.reddit, Resource.Drawable.search };
        public View GetTabView(int index, LayoutInflater inflater)
        {
            View view = inflater.Inflate(Resource.Layout.Ctrl_Tab, null);
            var image = view.FindViewById<ImageView>(Resource.Id.TabIcon);
            image.SetImageResource(tabIcons[index]);
            return view;
        }

        private string[] titles = new string[] { "Gallery", "Subreddits", "Search", "Account" };
        public string GetTitle(int index) => titles[index];

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