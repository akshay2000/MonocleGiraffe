using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using MonocleGiraffe.Portable.ViewModels.Front;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class SearchViewModel : Portable.ViewModels.Front.SearchViewModel
    {
        private SubredditsViewModel subredditsVM;

        public SearchViewModel(SubredditsViewModel subredditsVM, GalaSoft.MvvmLight.Views.INavigationService nav) : base(subredditsVM, nav, DesignMode.DesignModeEnabled)
        { }

        protected override Portable.ViewModels.Front.IncrementalPosts CreateIncrementalPosts(string query)
        {
            return new IncrementalPosts(query);
        }
    }

    public class IncrementalPosts : Portable.ViewModels.Front.IncrementalPosts, ISupportIncrementalLoading
    {
        public bool HasMoreItems { get { return HasMore; } }

        public IncrementalPosts(string query) : base(query)
        { }

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
