using System;
using Android.Widget;

namespace MonocleGiraffe.Android.Fragments
{
	public partial class RedditFragment
	{
		private TextView noUserRedditsView;
		public TextView NoUserRedditsView
		{
			get
			{
				noUserRedditsView = noUserRedditsView ?? View.FindViewById<TextView>(Resource.Id.NoUserReddits);
				return noUserRedditsView;
			}
		}
		
		private ListView userSubreddisListView;
		public ListView UserSubreddisListView
		{
			get
			{
				userSubreddisListView = userSubreddisListView ?? View.FindViewById<ListView>(Resource.Id.UserSubs);
				return userSubreddisListView;
			}
		}

		private TextView noPopRedditsView;
		public TextView NoPopRedditsView
		{
			get
			{
				noPopRedditsView = noPopRedditsView ?? View.FindViewById<TextView>(Resource.Id.NoPopReddits);
				return noPopRedditsView;
			}
		}

		private ListView popularSubreddisListView;
		public ListView PopularSubreddisListView
		{
			get
			{
				popularSubreddisListView = popularSubreddisListView ?? View.FindViewById<ListView>(Resource.Id.pop_reddits_list);
				return popularSubreddisListView;
			}
		}
	}
}
