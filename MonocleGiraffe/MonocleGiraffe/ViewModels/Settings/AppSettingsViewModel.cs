using MonocleGiraffe.LibraryImpl;
using MonocleGiraffe.Portable.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.ViewModels.Settings
{
    public class AppSettingsViewModel : BindableBase
    {
        private const string IS_VIRAL_ENABLED = "IsViralEnabled";

        public AppSettingsViewModel()
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

        bool isViralEnabled = default(bool);
        public bool IsViralEnabled { get { return isViralEnabled; } set { Set(ref isViralEnabled, value); } }

        public void ChangeViralEnabled()
        {
            Settings.SetValue(IS_VIRAL_ENABLED, IsViralEnabled);
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

        private void Init()
        {
            IsViralEnabled = Settings.GetValue<bool>(IS_VIRAL_ENABLED, false);
        }

        private void InitDesignTime()
        {
            IsViralEnabled = true;
        }
    }
}
