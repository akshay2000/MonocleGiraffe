using MonocleGiraffe.Controls.ItemTemplates;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace MonocleGiraffe.Controls
{
    public sealed partial class ColumnGridView : UserControl, INotifyPropertyChanged
    {
        public ColumnGridView()
        {
            this.InitializeComponent();
            MainScrollViewer.ViewChanged += MainScrollViewer_ViewChanged;
            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
        }


        bool isLoadingMore = default(bool);
        public bool IsLoadingMore { get { return isLoadingMore; } set { Set(ref isLoadingMore, value); } }

        private async Task LoadMoreItems()
        {
            if (!IsLoadingMore)
            {
                IsLoadingMore = true;
                await ItemsSource.LoadMoreItemsAsync(100);
                IsLoadingMore = false;
            }
        }

        private async Task<Size> GetAvailableSize()
        {
            while (MainPanel.ActualWidth == 0)
                await Task.Delay(50);
            return new Size(MainPanel.ActualWidth, double.PositiveInfinity);
        }

        private double oldVerticalOffset = 0;
        private async void MainScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {            
            bool isDown = (MainScrollViewer.VerticalOffset - oldVerticalOffset) > 0;
            oldVerticalOffset = MainScrollViewer.VerticalOffset;
            Render(isDown);
            UnrealizeItems();
            bool isEdge = (GetRealizationWindow().Bottom + MainScrollViewer.ViewportHeight) >= MainScrollViewer.ScrollableHeight;
            if (isEdge)
                await LoadMoreItems();
        }

        private RealizationWindow GetRealizationWindow()
        {
            double top = MainScrollViewer.VerticalOffset - MainScrollViewer.ViewportHeight;
            double bottom = MainScrollViewer.VerticalOffset + 2 * MainScrollViewer.ViewportHeight;
            return new RealizationWindow { Top = top, Bottom = bottom };
        }

        private RealizationWindow oldWindow = new RealizationWindow { Top = 0, Bottom = 0 };
        private void Render(bool isDown)
        {
            var currentWindow = GetRealizationWindow();
            if (isDown)
                RealizeWindow(oldWindow.Bottom, currentWindow.Bottom);
            else
                RealizeWindow(currentWindow.Top, oldWindow.Top);
            oldWindow = currentWindow;         
        }

        private async void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                return;
            var availableSize = await GetAvailableSize();
            MeasureOneItem((GalleryItem)e.NewItems[0], availableSize);
            MainPanel.Height = finalHeight;
        }

        double previousWidth = -1;
        private async void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (previousWidth != -1 && e.NewSize.Width != previousWidth)                
                await RedrawView();
            previousWidth = e.NewSize.Width;
        }

        private async Task RedrawView()
        {
            ResetLayout();
            MainPanel.Children.Clear();
            var availableSize = await GetAvailableSize();
            MainPanel.Height = MeasurePanel(availableSize).Height;
            var window = GetRealizationWindow();
            RealizeWindow(window.Top, window.Bottom);
            oldWindow = window;
        }

        public IncrementalCollection<GalleryItem> ItemsSource
        {
            get { return (IncrementalCollection<GalleryItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IncrementalCollection<GalleryItem>), typeof(ColumnGridView), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));

        private static async void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var view = o as ColumnGridView;
            view.SubscribeToCollectionChanged();
            await view.ReloadEverything();
        }

        private void SubscribeToCollectionChanged()
        {
            ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void RealizeWindow(double top, double bottom)
        {
            if (top >= bottom)
                return;
            var itemsToRealize = items.GetViewBetween(new LayoutInfo { Top = top }, new LayoutInfo { Top = bottom});
            foreach (var item in itemsToRealize)
                RealizeOneItem(item);
        }

        private async Task ReloadEverything()
        {
            ResetLayout();
            MainPanel.Height = 0;
            MainPanel.Children.Clear();
            if (ItemsSource.Count == 0)
                await LoadMoreItems();
            var availableSize = await GetAvailableSize();
            var window = GetRealizationWindow();
            RealizeWindow(window.Top, window.Bottom);
            oldWindow = window;
        }

        private void ResetLayout()
        {
            items = new SortedSet<LayoutInfo>(new LayoutInfoComparer());
            currentX = 0;
            isFirstRow = true;
            finalWidth = 0;
            finalHeight = 0;
            columnHeights = new List<double>();
            count = 0;
        }
        private SortedSet<LayoutInfo> items = new SortedSet<LayoutInfo>(new LayoutInfoComparer());
        double currentX = 0;
        bool isFirstRow = true;
        double finalWidth = 0;
        double finalHeight = 0;
        GalleryThumbnailTemplate element = new GalleryThumbnailTemplate();
        List<double> columnHeights = new List<double>();
        int count = 0;

        private void MeasureOneItem(GalleryItem g, Size availableSize)
        {
            double availableWidth = availableSize.Width;
            element.Margin = new Thickness(0, 0, 12, 12);
            element.MeasureWith(availableSize, g);
            if (isFirstRow)
            {
                double newWidth = finalWidth + element.DesiredSize.Width;
                if (newWidth <= availableWidth)
                {
                    items.Add(new LayoutInfo
                    {
                        Height = element.DesiredSize.Height,
                        Width = element.DesiredSize.Width,
                        Left = currentX,
                        Top = 0,
                        Content = g
                    });
                    columnHeights.Add(element.DesiredSize.Height);
                    finalWidth = newWidth;
                    currentX += element.DesiredSize.Width;
                }
                else
                {
                    currentX = 0;
                    double currentY = columnHeights.Count > 0 ? columnHeights[0] : 0;
                    items.Add(new LayoutInfo
                    {
                        Height = element.DesiredSize.Height,
                        Width = element.DesiredSize.Width,
                        Left = currentX,
                        Top = currentY,
                        Content = g
                    });
                    columnHeights[0] += element.DesiredSize.Height;
                    currentX += element.DesiredSize.Width;
                    count++;
                    isFirstRow = false;
                }
            }
            else
            {
                int noOfColumns = columnHeights.Count;
                int columnIndex = count % noOfColumns;
                currentX = columnIndex == 0 ? 0 : currentX;
                double currentY = columnHeights[columnIndex];
                items.Add(new LayoutInfo
                {
                    Height = element.DesiredSize.Height,
                    Width = element.DesiredSize.Width,
                    Left = currentX,
                    Top = currentY,
                    Content = g
                });
                columnHeights[columnIndex] += element.DesiredSize.Height;
                currentX += element.DesiredSize.Width;
                count++;
            }
            finalHeight = columnHeights.Count > 0 ? columnHeights.Max() : 0;
        }

        private Size MeasurePanel(Size availableSize)
        {            
            foreach (var g in ItemsSource)
            {
                MeasureOneItem(g, availableSize);
            }            
            Size finalSize = new Size(finalWidth, finalHeight);
            return finalSize;
        }

        ContainerCache<GalleryThumbnailTemplate> containerCache = new ContainerCache<GalleryThumbnailTemplate>();

        private void RealizeOneItem(LayoutInfo i)
        {
            //if (i.IsRendered)
            //    return;
            var container = containerCache.Get();
            if (container == null)
                container = new GalleryThumbnailTemplate();
            container.Tapped += Container_Tapped;
            container.SetLayout(i.Content);
            container.Tag = i;
            Canvas.SetLeft(container, i.Left);
            Canvas.SetTop(container, i.Top);
            MainPanel.Children.Add(container);
            i.IsRendered = true;
        }
        
        private void UnrealizeItems()
        {
            var window = GetRealizationWindow();
            foreach (var item in MainPanel.Children)
            {
                var itemTop = Canvas.GetTop(item);
                if (itemTop < window.Top || itemTop > window.Bottom)
                {
                    MainPanel.Children.Remove(item);
                    GalleryThumbnailTemplate container = item as GalleryThumbnailTemplate;                    
                    (container.Tag as LayoutInfo).IsRendered = false;
                    containerCache.Put(container);
                }
            }
        }
        
        private void Container_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnItemClick((sender as GalleryThumbnailTemplate).Item);
            e.Handled = true;
        }

        public event EventHandler<GalleryItem> ItemClick;
        private void OnItemClick(GalleryItem clickedItem)
        {
            ItemClick?.Invoke(this, clickedItem);
        }

        private class LayoutInfo
        {
            public double Left { get; set; }
            public double Top { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public bool IsRendered { get; set; }
            public object Content { get; set; }
        }

        private class LayoutInfoComparer : IComparer<LayoutInfo>
        {
            public int Compare(LayoutInfo x, LayoutInfo y)
            {
                int result = x.Top.CompareTo(y.Top);
                return result == 0 ? -1 : result;
            }
        }

        private class ContainerCache<T> where T : new()
        {
            private Queue<T> queue = new Queue<T>();
            public T Get()
            {
                if (queue.Count == 0)
                    return default(T);
                else
                    return queue.Dequeue();
            }

            public void Put(T toPut)
            {
                queue.Enqueue(toPut);
            }
        }

        private class RealizationWindow
        {
            public double Top { get; set; }
            public double Bottom { get; set; }
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
