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
            PivotIndex = 0;
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
                case 0:
                    AppSettings = AppSettings ?? new AppSettingsViewModel();
                    break;
                case 1:
                    ImgurSettings = ImgurSettings ?? new ImgurSettingsViewModel();
                    break;
            }
        }

        AppSettingsViewModel appSettings;
        public AppSettingsViewModel AppSettings { get { return appSettings; } set { Set(ref appSettings, value); } }

        ImgurSettingsViewModel imgurSettings;
        public ImgurSettingsViewModel ImgurSettings { get { return imgurSettings; } set { Set(ref imgurSettings, value); } }        

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
            AppSettings = new AppSettingsViewModel();
            ImgurSettings = new ImgurSettingsViewModel();
        }       
    }
}