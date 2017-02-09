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
using Android.Util;
using MonocleGiraffe.Android.Helpers;

namespace MonocleGiraffe.Android.Controls
{
    public class GridAutofitLayoutManager : GridLayoutManager
    {
        private int ColumnWidth { get; set; }
        private Context Context { get; set; }
        private Dictionary<int, int> widthToColumnCount = new Dictionary<int, int>();

        public GridAutofitLayoutManager(Context context, int columnWidth) : base(context, 1)
        {
            ColumnWidth = columnWidth;
            Context = context;
        }

        public GridAutofitLayoutManager(Context context, int columnWidth, int orientation, bool reverseLayout) : base(context, columnWidth, orientation, reverseLayout)
        {
            ColumnWidth = columnWidth;
            Context = context;
        }

        private int oldWidth = 0;
        public override void OnMeasure(RecyclerView.Recycler recycler, RecyclerView.State state, int widthSpec, int heightSpec)
        {
            int width = Width;
            bool isWidthChanged = oldWidth != width;
            if (isWidthChanged && ColumnWidth > 0 && width > 0)
            {
                oldWidth = width;
                int totalWidth = width - PaddingRight - PaddingLeft;
                int totalWidthInDp = Utils.PxToDp(totalWidth, Context.Resources);
                if (!widthToColumnCount.ContainsKey(totalWidthInDp))
                    widthToColumnCount[totalWidthInDp] = Utils.CalculateColumnCount(ColumnWidth, totalWidthInDp);
                SpanCount = widthToColumnCount[totalWidthInDp];
            }
            base.OnMeasure(recycler, state, widthSpec, heightSpec);
        }
    }
}