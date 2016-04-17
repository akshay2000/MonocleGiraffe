using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace MonocleGiraffe.Controls
{
    public sealed partial class TreeView : UserControl, INotifyPropertyChanged
    {
        public TreeView()
        {
            this.InitializeComponent();
        }

        public IEnumerable<ITreeItem> ItemsSource
        {
            get { return (IEnumerable<ITreeItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ITreeItem>), typeof(TreeView), new PropertyMetadata(null, new PropertyChangedCallback(ItemsSourceChanged)));

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;
            var newValue = e.NewValue as IEnumerable<ITreeItem>;
            var panel = (TreeView)d;
            panel.Items = new ObservableCollection<TreeViewItem>(Translate(newValue.ToList(), 0));
        }

        ObservableCollection<TreeViewItem> items = default(ObservableCollection<TreeViewItem>);
        public ObservableCollection<TreeViewItem> Items { get { return items; } set { Set(ref items, value); } }

        private static List<TreeViewItem> Translate<T>(List<T> input, int depth) where T : ITreeItem
        {
            List<TreeViewItem> ret = new List<TreeViewItem>();
            for (int i = 0; i < input.Count(); i++)
            {
                var item = input[i];
                TreeViewItem t = new TreeViewItem();
                t.Content = item.Content;
                t.Depth = depth;
                if (item.Children != null)
                {
                    t.Children = Translate(item.Children, t.Depth + 1);
                }
                ret.Add(t);
            }
            return ret;
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
        
        private void CommentTemplate_ExpandRequested(object sender, RoutedEventArgs e)
        {
            TreeViewItem tappedItem = (sender as FrameworkElement).Tag as TreeViewItem;
            int index = Items.IndexOf(tappedItem);
            if (!tappedItem.IsExpanded)
            {
                foreach (var item in tappedItem.Children)
                {
                    Items.Insert(++index, item);
                }
                tappedItem.IsExpanded = true;
            }
        }

        private void CommentTemplate_CollapseRequested(object sender, RoutedEventArgs e)
        {
            TreeViewItem tappedItem = (sender as FrameworkElement).Tag as TreeViewItem;
            int index = Items.IndexOf(tappedItem);
            if (tappedItem.IsExpanded)
            {
                int depth = tappedItem.Depth;
                index++;
                int nextDepth = index < Items.Count ? Items[index].Depth : 0;
                while (nextDepth > depth && index < Items.Count)
                {
                    Items.RemoveAt(index);
                    if (Items.Count > index)
                        nextDepth = Items[index].Depth;
                }
                tappedItem.IsExpanded = false;
            }
        }
    }

    public class TreeViewItem : BindableBase
    {
        public object Content { get; set; }
        
        public int Depth { get; set; }

        public List<TreeViewItem> Children { get; set; }

        bool isExpanded;
        public bool IsExpanded { get { return isExpanded; } set { Set(ref isExpanded, value); } }
    }

    public class DepthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int multiplier = int.Parse((string)parameter);
            return new GridLength((int)value * multiplier);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void InsertRange(int index, IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Insert(++index, item);
            }
            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public interface ITreeItem
    {
        object Content { get; set; }
        List<ITreeItem> Children { get; set; }
    }
}
