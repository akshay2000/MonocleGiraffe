using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Helpers
{
    public abstract class IncrementalCollection<T> : ObservableCollection<T>, IIncrementalCollection, INotifyPropertyChanged
    {
        public uint Page { get; private set; }

        public int ConsumedItemsIndex { get; private set; }

        public IncrementalCollection() : base()
        {
            Page = 0;
            ConsumedItemsIndex = 0;
        }

        public IncrementalCollection(IEnumerable<T> collection) : base(collection) { }      

        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        #region IIncrementalCollection

        public bool HasMore
        {
            get
            {
                return HasMoreItemsImpl();
            }
        }

        public async Task LoadMoreAsync(uint count)
        {
            await LoadMoreAsync(default(CancellationToken), count);
        }

        #endregion

        private List<T> moreItems;

        protected async Task<uint> LoadMoreAsync(CancellationToken c, uint count)
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
            Debug.WriteLine("Done!");
            return count;
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
