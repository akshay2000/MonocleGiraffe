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

        public TransfersPageViewModel() : base(new DownloadsViewModel())
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        private void Init()
        {
            UploadsVM = new UploadsViewModel();
        }

        UploadsViewModel uploadsVM = default(UploadsViewModel);
        public UploadsViewModel UploadsVM { get { return uploadsVM; } set { Set(ref uploadsVM, value); } }

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

        private void InitDesignTime()
        {
            DownloadsVM = new DownloadsViewModel();
            UploadsVM = new UploadsViewModel();
        }
    }

}
