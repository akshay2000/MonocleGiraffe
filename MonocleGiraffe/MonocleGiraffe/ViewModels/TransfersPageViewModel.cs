using MonocleGiraffe.ViewModels.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{
    public class TransfersPageViewModel : ViewModelBase
    {
        public TransfersPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitDesignTime();
            }
            else
            {
                // runtime experience
            }
        }

        DownloadsViewModel downloadsVM = default(DownloadsViewModel);
        public DownloadsViewModel DownloadsVM { get { return downloadsVM; } set { Set(ref downloadsVM, value); } }

        #region Navigation

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
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

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // save state
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        #endregion

        private void InitDesignTime()
        {
            DownloadsVM = new DownloadsViewModel();
        }
    }

}
