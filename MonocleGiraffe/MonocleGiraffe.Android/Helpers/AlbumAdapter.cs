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
using MonocleGiraffe.Portable.Models;

namespace MonocleGiraffe.Android.Helpers
{
    public class AlbumAdapter : ObservableRecyclerAdapter<GalleryItem, CachingViewHolder>
    {
        public AlbumAdapter()
        {

        }

        public void OnScrollThreshold()
        {

        }
    }
}