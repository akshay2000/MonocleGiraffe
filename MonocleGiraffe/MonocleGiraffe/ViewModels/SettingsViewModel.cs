using SharpImgur.Helpers;
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
    public class SettingsViewModel : ViewModelBase
    {
        const string IS_VIRAL_ENABLED = "IsViralEnabled";
        public SettingsViewModel()
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

        private void InitDesignTime()
        {
            IsViralEnabled = true;
        }

        private void Init()
        {
            IsViralEnabled = SettingsHelper.GetValue<bool>(IS_VIRAL_ENABLED, true);
        }

        bool isViralEnabled = default(bool);
        public bool IsViralEnabled { get { return isViralEnabled; } set { Set(ref isViralEnabled, value); } }

        public void ChangeViralEnabled()
        {
            SettingsHelper.SetValue(IS_VIRAL_ENABLED, IsViralEnabled);
        }

        #region Navigation

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
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
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // save state
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        #endregion

    }
}