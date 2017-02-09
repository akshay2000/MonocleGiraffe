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
using Android.Support.V7.Widget;
using MonocleGiraffe.Portable.Models;
using System.Collections.ObjectModel;

namespace MonocleGiraffe.Android.Helpers
{
    class AlbumScrollListener : RecyclerView.OnScrollListener
    {
        private List<GalleryItem> source;
        private ObservableCollection<GalleryItem> collection;

        private readonly int threshold = 3;

        private int currentIndex = 0;

        public AlbumScrollListener(List<GalleryItem> source, ObservableCollection<GalleryItem> collection)
        {
            this.source = source;
            this.collection = collection;
            LoadMore(threshold);
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            LinearLayoutManager layoutManager = (LinearLayoutManager)recyclerView.GetLayoutManager();
            if (layoutManager.FindLastVisibleItemPosition() + threshold > collection.Count && dy > 0)
                LoadMore(threshold);
        }
        
        private bool isLoading;
        private void LoadMore(int moreCount)
        {
            if (isLoading)
                return;
            isLoading = true;
            for (int i = 0; i < moreCount; i++)
            {
                if (currentIndex >= source.Count)
                    break;
                collection.Add(source[currentIndex]);
                currentIndex++;
            }
            isLoading = false;
        }
    }
}