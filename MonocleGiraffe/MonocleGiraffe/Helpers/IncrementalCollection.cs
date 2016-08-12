using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.Helpers
{
    public abstract class IncrementalCollection<T> : Portable.Helpers.IncrementalCollection<T>, ISupportIncrementalLoading
    {
        public bool HasMoreItems
        {
            get
            {
                return HasMoreItemsImpl();
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            var ret = await LoadMoreAsync(c, count);
            return new LoadMoreItemsResult { Count = count };
        }
    }
}
