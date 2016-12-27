using MonocleGiraffe.ViewModels.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{
    public class TransfersPageViewModel : Portable.ViewModels.TransfersViewModel, INavigable
    {
        public INavigationService NavigationService { get; set; }
        public IDispatcherWrapper Dispatcher { get; set; }
        public IStateItems SessionState { get; set; }

        public TransfersPageViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(new DownloadsViewModel(), new UploadsViewModel(nav))
        { }

        private void Init(GalaSoft.MvvmLight.Views.INavigationService nav)
        {
            //DownloadsVM = new DownloadsViewModel();
            //UploadsVM = new UploadsViewModel(nav);
        }

        #region Navigation

        public Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // restore state
                state.Clear();
            }
            else
            {
                // use parameter
            }
            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // save state
            }
            return Task.CompletedTask;
        }

        public Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        #endregion

        private void InitDesignTime(GalaSoft.MvvmLight.Views.INavigationService nav)
        {
            //DownloadsVM = new DownloadsViewModel();
            //UploadsVM = new UploadsViewModel(nav);
        }
    }

}
