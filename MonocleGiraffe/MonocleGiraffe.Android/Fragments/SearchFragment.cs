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

namespace MonocleGiraffe.Android.Fragments
{
    public class SearchFragment : global::Android.Support.V4.App.Fragment
    {
        List<Binding> bindings = new List<Binding>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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

            RedditsButton.SetCommand("Click", Vm.SearchCommand, "Reddits");
            PostsButton.SetCommand("Click", Vm.SearchCommand, "Posts");
            GifsButton.SetCommand("Click", Vm.SearchCommand, "Gifs");
        }

        private Drawable ConvertBoolToDrawable(bool flag)
        {
            var ret = flag ? Resources.GetColor(Resource.Color.ImgurGreen) : Resources.GetColor(Resource.Color.Gray);
            return new ColorDrawable(ret);
        }

        public SearchViewModel Vm { get { return App.Locator.Front.SearchVM; } }

        #region Views

        private TextView redditsButton;
        private TextView RedditsButton
        {
            get
            {
                redditsButton = redditsButton ?? View.FindViewById<TextView>(Resource.Id.RedditsButton);
                return redditsButton;
            }
        }

        private TextView postsButton;
        private TextView PostsButton
        {
            get
            {
                postsButton = postsButton ?? View.FindViewById<TextView>(Resource.Id.PostsButton);
                return postsButton;
            }
        }

        private TextView gifsButton;
        private TextView GifsButton
        {
            get
            {
                gifsButton  = gifsButton ?? View.FindViewById<TextView>(Resource.Id.GifsButton);
                return gifsButton;
            }
        }

        #endregion
    }
}