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
    public class RedditFragment : global::Android.Support.V4.App.Fragment
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
            SubreddisListView.Adapter = Vm.SubredditsVM.UserSubreddits.GetAdapter(GetSubredditView);
            SubreddisListView.ItemClick += SubreddisListView_ItemClick;
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
        }

        public FrontViewModel Vm { get { return App.Locator.Front; } }        

        private ListView subreddisListView;
        public ListView SubreddisListView
        {
            get
            {
                subreddisListView = subreddisListView ?? View.FindViewById<ListView>(Resource.Id.reddits_list);
                return subreddisListView;
            }
        }
    }
}