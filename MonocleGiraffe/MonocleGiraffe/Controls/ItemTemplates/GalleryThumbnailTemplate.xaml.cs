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
            SetDimensions((GalleryItem)item);
            Measure(availableSize);
        }

        public void SetLayout(object item)
        {
            Item = (GalleryItem)item;
            SetDimensions((GalleryItem)item);
        }

        private void SetDimensions(GalleryItem item)
        {
            var windowBounds = Window.Current.Bounds;
            if (windowBounds.Width >= 360)
            {
                Thumbnail.Width = 162;
                LayoutRoot.Width = 162;
                Thumbnail.Height = item.BigThumbRatio * 162;
            }
            else if (windowBounds.Width >= 341)
            {
                Thumbnail.Width = 152;
                LayoutRoot.Width = 152;
                Thumbnail.Height = item.BigThumbRatio * 152;
            }
            else if (windowBounds.Width >= 320)
            {
                Thumbnail.Width = 142;
                LayoutRoot.Width = 142;
                Thumbnail.Height = item.BigThumbRatio * 142;
            }
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
    }
}
