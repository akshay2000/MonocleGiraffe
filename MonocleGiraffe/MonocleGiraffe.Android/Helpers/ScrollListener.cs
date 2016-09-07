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
using MonocleGiraffe.Portable.Helpers;

namespace MonocleGiraffe.Android.Helpers
{
    public class ScrollListener : RecyclerView.OnScrollListener
    {
        private IEnumerable<object> collection;
        private const int treshold = 60;
        private bool isLoading = false;

        public ScrollListener(IEnumerable<object> collection)
        {
            this.collection = collection;
            if (collection is IIncrementalCollection && collection.Count() == 0)
                LoadMore();
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            if (!(collection is IIncrementalCollection))
                return;
            
            var layoutManager = recyclerView.GetLayoutManager();
            var lastItem = GetLastVisibleItem(layoutManager);
            if (lastItem + treshold > collection.Count() && dy > 0)
                LoadMore();
        }

        private async void LoadMore()
        {
            var incrementalCollection = collection as IIncrementalCollection;
            if (incrementalCollection.HasMore && !isLoading)
            {
                isLoading = true;
                await incrementalCollection.LoadMoreAsync(treshold);
                isLoading = false;
            }
        }

        private int GetLastVisibleItem(RecyclerView.LayoutManager layoutManager)
        {
            if (layoutManager is StaggeredGridLayoutManager)
                return (layoutManager as StaggeredGridLayoutManager).FindLastVisibleItemPositions(null).Max();
            if (layoutManager is LinearLayoutManager)
                return (layoutManager as LinearLayoutManager).FindLastVisibleItemPosition();
            if (layoutManager is GridLayoutManager)
                return (layoutManager as GridLayoutManager).FindLastVisibleItemPosition();
            throw new InvalidOperationException($"Can't find last item from {layoutManager.GetType().Name}");
        }
    }
}