
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonocleGiraffe.Android
{
	[Activity(Label = "SettingsActivity")]
	public class SettingsActivity : AppCompatActivity
	{
		SettingsPagerAdapter pagerAdapter;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Settings);

			MainToolbar.Title = "Settings";

			pagerAdapter = new SettingsPagerAdapter(SupportFragmentManager);
			var pager = FindViewById<ViewPager>(Resource.Id.MainPager);
			var tabLayout = FindViewById<TabLayout>(Resource.Id.Tabs);
			tabLayout.SetupWithViewPager(pager);
			pager.Adapter = pagerAdapter;

			SetSupportActionBar(MainToolbar);
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
	}

	public class SettingsPagerAdapter : FragmentPagerAdapter
	{
		public SettingsPagerAdapter(global::Android.Support.V4.App.FragmentManager fm) : base(fm)
		{ }
		
		public override int Count { get { return 2; } }

		public override global::Android.Support.V4.App.Fragment GetItem(int position)
		{
			switch (position)
			{
				case 0:
					return AppSettingsFragment;
				case 1:
					return ImgurSettingsFragment;
				default:
					Log.Wtf("Pager", $"Requested fragment for index {position}");
					break;
			}
			return null;
		}

		private Java.Lang.String[] pageTitles = { new Java.Lang.String("App"), new Java.Lang.String("Imgur") };
		public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
		{
			return pageTitles[position];
		}

		private AppSettingsFragment appSettingsFragment;
		private AppSettingsFragment AppSettingsFragment
		{
			get
			{
				appSettingsFragment = appSettingsFragment ?? new AppSettingsFragment();
				return appSettingsFragment;
			}
		}

		private AppSettingsFragment imgurSettingsFragment;
		private AppSettingsFragment ImgurSettingsFragment
		{
			get
			{
				imgurSettingsFragment = imgurSettingsFragment ?? new AppSettingsFragment();
				return imgurSettingsFragment;
			}
		}
	}
}

