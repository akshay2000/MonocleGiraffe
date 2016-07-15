using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Mvvm;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MonocleGiraffe.Controls.ItemTemplates
{
    public sealed partial class GalleryThumbnailTemplate : UserControl, INotifyPropertyChanged
    {
        public GalleryThumbnailTemplate()
        {
            this.InitializeComponent();
        }

        GalleryItem item = default(GalleryItem);
        public GalleryItem Item { get { return item; } set { Set(ref item, value); } }

        public void MeasureWith(Size availableSize, object item)
        {
            SetDimensions((GalleryItem)item, availableSize.Width);
            Measure(availableSize);
        }

        public void SetLayout(Size availableSize, object item)
        {
            Item = (GalleryItem)item;
            SetDimensions((GalleryItem)item, availableSize.Width);
        }

        private void SetDimensions(GalleryItem item, double availableWidth)
        {
            var thumbnailWidth = WidthManager.GetItemWidth(availableWidth);            
            Thumbnail.Width = thumbnailWidth;
            LayoutRoot.Width = thumbnailWidth;
            Thumbnail.Height = item.BigThumbRatio * thumbnailWidth;
        }        

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion


        private static class WidthManager
        {
            private static Dictionary<double, double> cache;

            static WidthManager()
            {
                cache = new Dictionary<double, double>();
                cache[320] = 142;
                cache[341] = 152;
                cache[360] = 162;
                cache[480] = 144;
            }

            public static double GetItemWidth(double availableWidth)
            {
                Debug.WriteLine($"Requested for {availableWidth}");
                if (!cache.ContainsKey(availableWidth))
                    cache[availableWidth] = CalculateWidth(availableWidth);
                return cache[availableWidth];
            }

            private static double CalculateWidth(double availableWidth)
            {                
                const double maxWidth = 187;
                const int margin = 12;
                var effectiveItemWidth = maxWidth + margin;
                int possibleNoOfColumns = (int)Math.Floor(availableWidth / effectiveItemWidth);
                int requiredColumns = possibleNoOfColumns + 1;
                var itemWidth = (availableWidth / requiredColumns) - margin;
                itemWidth = Math.Floor(itemWidth);
                return itemWidth;
            }
        }
    }
}
