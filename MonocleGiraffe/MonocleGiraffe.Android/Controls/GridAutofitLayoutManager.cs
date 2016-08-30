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

namespace MonocleGiraffe.Android.Controls
{
    public class GridAutofitLayoutManager : GridLayoutManager
    {
        private int mColumnWidth;
        private bool mColumnWidthChanged = true;

        public GridAutofitLayoutManager(Context context, int columnWidth) : base(context, 1)
        {
            SetColumnWidth(CheckedColumnWidth(context, columnWidth));
        }

        public GridAutofitLayoutManager(Context context, int columnWidth, int orientation, bool reverseLayout) : base(context, columnWidth, orientation, reverseLayout)
        {
            SetColumnWidth(CheckedColumnWidth(context, columnWidth));
        }

        private int CheckedColumnWidth(Context context, int columnWidth)
        {
            if (columnWidth <= 0)
            {
                /* Set default columnWidth value (48dp here). It is better to move this constant
                to static constant on top, but we need context to convert it to dp, so can't really
                do so. */
                columnWidth = (int)TypedValue.ApplyDimension(TypedValue.DensityDefault, 48,
                        context.Resources.DisplayMetrics);
            }
            return columnWidth;
        }

        public void SetColumnWidth(int newColumnWidth)
        {
            if (newColumnWidth > 0 && newColumnWidth != mColumnWidth)
            {
                mColumnWidth = newColumnWidth;
                mColumnWidthChanged = true;
            }
        }

        public override void OnLayoutChildren(RecyclerView.Recycler recycler, RecyclerView.State state)
        {
            int width = Width;
            int height = Height;
            if (mColumnWidthChanged && mColumnWidth > 0 && width > 0 && height > 0)
            {
                int totalSpace;
                if (Orientation == Vertical)
                    totalSpace = width - PaddingRight - PaddingLeft;
                else
                    totalSpace = height - PaddingTop - PaddingBottom;

                int spanCount = Math.Max(1, totalSpace / mColumnWidth);
                SpanCount = spanCount;
                mColumnWidthChanged = false;
            }
            base.OnLayoutChildren(recycler, state);
        }
    }
}