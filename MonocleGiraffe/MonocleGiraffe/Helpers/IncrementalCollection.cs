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
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.Helpers
{
    public abstract class IncrementalCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading, INotifyPropertyChanged
    {
        public uint Page { get; private set; }

        public int ConsumedItemsIndex { get; private set; }

        public IncrementalCollection() : base()
        {
            Page = 0;
            ConsumedItemsIndex = 0;
        }

        public IncrementalCollection(IEnumerable<T> collection) : base(collection) { }

        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return HasMoreItemsImpl(); }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        #endregion

        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        private List<T> moreItems;

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            IsBusy = true;
            for (int i = 0; i < count; i++)
            {
                if (moreItems == null || moreItems.Count == ConsumedItemsIndex)
                {
                    Page++;
                    moreItems = await LoadMoreItemsImplAsync(c, Page - 1);
                    if (moreItems.Count == 0)
                        break;
                    ConsumedItemsIndex = 0;
                }
                if (moreItems.Count == 0)
                    break;
                Add(moreItems[ConsumedItemsIndex++]);
            }
            IsBusy = false;
            return new LoadMoreItemsResult { Count = count };
        }

        #region Abstracts

        protected abstract Task<List<T>> LoadMoreItemsImplAsync(CancellationToken c, uint page);

        protected abstract bool HasMoreItemsImpl();

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<E>(ref E storage, E value, [CallerMemberName] string propertyName = null)
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
