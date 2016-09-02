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
using MonocleGiraffe.Portable.ViewModels;
using Android.Support.V4.View;
using Android.Support.V4.App;
using MonocleGiraffe.Android.Fragments;
using MonocleGiraffe.Portable.Models;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "BrowserActivity")]
    public class BrowserActivity : FragmentActivity
    {
        private PagerAdapter adapter;

        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Browser);
            var param = Nav.GetAndRemoveParameter(Intent) ?? "GalleryInfo";
            Vm.Activate(param);
            adapter = new BrowserAdapter(Vm.Images, SupportFragmentManager);
            BrowserPager.Adapter = adapter;
            BrowserPager.SetCurrentItem(Vm.FlipViewIndex, false);
        }

        public BrowserViewModel Vm { get { return App.Locator.Browser; } }

        private ViewPager browserPager;
        public ViewPager BrowserPager
        {
            get
            {
                browserPager = browserPager ?? FindViewById<ViewPager>(Resource.Id.BrowserPager);
                return browserPager;
            }
        }

        private class BrowserAdapter : FragmentStatePagerAdapter
        {
            private IEnumerable<IGalleryItem> images;
            public BrowserAdapter(IEnumerable<IGalleryItem> images, global::Android.Support.V4.App.FragmentManager fm) : base(fm)
            {
                this.images = images;
            }
            
            public override int Count
            {
                get
                {
                    return images.Count();
                }
            }

            public override global::Android.Support.V4.App.Fragment GetItem(int position)
            {
                return BrowserItemFragment.NewInstance(position);
            }
        }
    }    
}