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
using GalaSoft.MvvmLight.Helpers;
using MonocleGiraffe.Portable.ViewModels;
using MonocleGiraffe.Portable.Models;

namespace MonocleGiraffe.Android.Fragments
{
	public partial class RedditFragment : global::Android.Support.V4.App.Fragment
    {
        private readonly List<Binding> bindings = new List<Binding>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.Front_Reddits, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
			//User Subreddits
			bindings.Add(this.SetBinding(() => Vm.SubredditsVM.UserSubreddits)
			             .WhenSourceChanges(() => UserSubreddisListView.Adapter = Vm.SubredditsVM.UserSubreddits.GetAdapter(GetSubredditView)));
            UserSubreddisListView.ItemClick += SubreddisListView_ItemClick;
			bindings.Add(this.SetBinding(() => Vm.SubredditsVM.UserState, () => NoUserRedditsView.Visibility)
						 .ConvertSourceToTarget((s) => s == "Empty" ? ViewStates.Visible : ViewStates.Gone));
			bindings.Add(this.SetBinding(() => Vm.SubredditsVM.UserState, () => UserSubreddisListView.Visibility)
						 .ConvertSourceToTarget((s) => s == "Done" ? ViewStates.Visible : ViewStates.Gone));

			//Popular Subreddits
			bindings.Add(this.SetBinding(() => Vm.SubredditsVM.PopularSubreddits)
			             .WhenSourceChanges(() => PopularSubreddisListView.Adapter = Vm.SubredditsVM.PopularSubreddits.GetAdapter(GetSubredditView)));
			PopularSubreddisListView.ItemClick += SubreddisListView_ItemClick;
			bindings.Add(this.SetBinding(() => Vm.SubredditsVM.PopularState, () => NoPopRedditsView.Visibility)
						 .ConvertSourceToTarget((s) => s == "Empty" ? ViewStates.Visible : ViewStates.Gone));
			bindings.Add(this.SetBinding(() => Vm.SubredditsVM.PopularState, () => PopularSubreddisListView.Visibility)
						 .ConvertSourceToTarget((s) => s == "Done" ? ViewStates.Visible : ViewStates.Gone));
        }

        private void SetVisibility(View view, string state, string requiredState)
        {
            view.Visibility = state == requiredState ? ViewStates.Visible : ViewStates.Gone;
        }

        private void SubreddisListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var clickedItem = Vm.SubredditsVM.UserSubreddits[e.Position];
            Vm.SubredditsVM.GoToSub.Execute(clickedItem);
        }

        private View GetSubredditView(int position, SubredditItem item, View convertView)
        {
            View ret = convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.Tmpl_SubredditItem, null);
            ret.FindViewById<TextView>(Resource.Id.TitleTextView).Text = item.Title;
            ret.FindViewById<TextView>(Resource.Id.SubtitleTextView).Text = $"/r/{item.Url}";
            return ret;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach((b) => b.Detach());
			UserSubreddisListView.ItemClick -= SubreddisListView_ItemClick;
			PopularSubreddisListView.ItemClick -= SubreddisListView_ItemClick;
        }

        public FrontViewModel Vm { get { return App.Locator.Front; } }
    }
}