using MonocleGiraffe.ViewModels.Settings;
using XamarinImgur.APIWrappers;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
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
using MonocleGiraffe.LibraryImpl;

namespace MonocleGiraffe.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private const string IS_VIRAL_ENABLED = "IsViralEnabled";
        
        public SettingsPageViewModel()
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
            IsViralEnabled = Settings.GetValue<bool>(IS_VIRAL_ENABLED, true);
        }

        private int pivotIndex;
        public int PivotIndex
        {
            get { return pivotIndex; }
            set
            {
                Set(ref pivotIndex, value);
                CreateSubVMIfRequired();
            }
        }

        private void CreateSubVMIfRequired()
        {
            switch (PivotIndex)
            {                
                case 1:
                    ImgurSettings = ImgurSettings ?? new ImgurSettingsViewModel();
                    break;
            }
        }

        ImgurSettingsViewModel imgurSettings;
        public ImgurSettingsViewModel ImgurSettings { get { return imgurSettings; } set { Set(ref imgurSettings, value); } }


        bool isViralEnabled = default(bool);
        public bool IsViralEnabled { get { return isViralEnabled; } set { Set(ref isViralEnabled, value); } }

        public void ChangeViralEnabled()
        {
            Settings.SetValue(IS_VIRAL_ENABLED, IsViralEnabled);
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
                if (ImgurSettings != null)
                    await ImgurSettings.Refresh();
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

        private void InitDesignTime()
        {
            IsViralEnabled = true;
            ImgurSettings = new ImgurSettingsViewModel();
        }

        private SettingsHelper settings;
        public SettingsHelper Settings
        {
            get
            {
                settings = settings ?? new SettingsHelper();
                return settings;
            }
        }
    }
}