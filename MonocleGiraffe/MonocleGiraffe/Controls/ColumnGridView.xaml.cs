using MonocleGiraffe.Controls.ItemTemplates;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ColumnGridView : UserControl
    {
        public ColumnGridView()
        {
            this.InitializeComponent();
            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                return;
            var availableSize = new Size(Window.Current.Bounds.Width, double.PositiveInfinity);
            MeasureOneItem((GalleryItem)e.NewItems[0], availableSize);
            MainPanel.Height = finalHeight;
            ArrangeOneItem(items[e.NewStartingIndex]);
        }

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshView();
        }

        public IncrementalCollection<GalleryItem> ItemsSource
        {
            get { return (IncrementalCollection<GalleryItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IncrementalCollection<GalleryItem>), typeof(ColumnGridView), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));

        private static void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var view = o as ColumnGridView;
            view.SubscribeToCollectionChanged();
            view.RefreshView();
        }

        private void SubscribeToCollectionChanged()
        {
            ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void RefreshView()
        {
            var availableSize = new Size(Window.Current.Bounds.Width, double.PositiveInfinity);
            var canvasSize = MeasurePanel(availableSize);
            MainPanel.Height = canvasSize.Height;
            ArrangePanel();
        }

        private List<LayoutInfo> items = new List<LayoutInfo>();
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
            items = new List<LayoutInfo>();
            finalWidth = 0;
            finalHeight = 0;
            element = new GalleryThumbnailTemplate();
            currentX = 0;
            columnHeights = new List<double>();
            count = 0;
            isFirstRow = true;
            foreach (var g in ItemsSource)
            {
                MeasureOneItem(g, availableSize);
            }            
            Size finalSize = new Size(finalWidth, finalHeight);
            return finalSize;
        }

        private void ArrangeOneItem(LayoutInfo i)
        {
            var container = new GalleryThumbnailTemplate();
            container.Tapped += Container_Tapped;
            container.SetLayout(i.Content);
            Canvas.SetLeft(container, i.Left);
            Canvas.SetTop(container, i.Top);
            MainPanel.Children.Add(container);
        }

        private void ArrangePanel()
        {
            MainPanel.Children.Clear();
            foreach(var i in items)
            {
                ArrangeOneItem(i);
            }
        }

        private void Container_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnItemClick((sender as GalleryThumbnailTemplate).Item);
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
            public object Content { get; set; }
        }
    }
}
