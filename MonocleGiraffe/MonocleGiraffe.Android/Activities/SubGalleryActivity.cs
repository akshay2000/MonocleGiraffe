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
using GalaSoft.MvvmLight.Helpers;
using MonocleGiraffe.Portable.ViewModels;
using MonocleGiraffe.Portable.Models;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Android.Support.V7.Widget;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "SubGalleryActivity")]
    public class SubGalleryActivity : Activity
    {
        private readonly List<Binding> bindings = new List<Binding>();
        private ObservableRecyclerAdapter<GalleryItem, CachingViewHolder> adapter;

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
            SetContentView(Resource.Layout.SubGallery);
            SubGalleryRecyclerView.SetLayoutManager(new GridLayoutManager(this, 3));

            //Hacky way to bind
            Vm.PropertyChanged += Vm_PropertyChanged;

            var param = Nav.GetAndRemoveParameter<string>(Intent);
            Vm.Activate(param);

            Vm.Images.LoadMoreAsync(60);
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Images")
                BindCollection();
        }

        private void BindCollection()
        {
            adapter = Vm.Images.GetRecyclerAdapter(BindViewHolder, Resource.Layout.Tmpl_SubredditThumbnail);
            SubGalleryRecyclerView.SetAdapter(adapter);
        }

        private void BindViewHolder(CachingViewHolder holder, GalleryItem item, int position)
        {
            var textView = holder.FindCachedViewById<TextView>(Resource.Id.textView1);
            textView.Text = item.Title;
        }

        public SubGalleryViewModel Vm { get { return App.Locator.SubGallery; } }

        private RecyclerView subGalleryRecyclerView;
        public RecyclerView SubGalleryRecyclerView
        {
            get
            {
                subGalleryRecyclerView = subGalleryRecyclerView ?? FindViewById<RecyclerView>(Resource.Id.SubGalleryRecyclerView);
                return subGalleryRecyclerView;
            }
        }
    }
}