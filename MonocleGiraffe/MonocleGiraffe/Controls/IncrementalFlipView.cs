using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.Controls
{
    public class IncrementalFlipView : FlipView
    {
        public IncrementalFlipView() : base()
        {
            SelectionChanged += IncrementalFlipView_SelectionChanged;
        }

        private async void IncrementalFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipView = (FlipView)sender;
            IEnumerable<object> itemsSource = flipView.ItemsSource as IEnumerable<object>;
            if (itemsSource != null && itemsSource is ISupportIncrementalLoading)
            {
                var incrementalSource = (ISupportIncrementalLoading)itemsSource;
                if (itemsSource.Count() < SelectedIndex + 5)
                {
                    if (incrementalSource.HasMoreItems)
                    {
                        await incrementalSource.LoadMoreItemsAsync(60);
                    }
                }
            }
        }
    }
}
