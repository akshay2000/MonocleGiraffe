using System;
using GalaSoft.MvvmLight;
using MonocleGiraffe.Portable.ViewModels.Settings;

namespace MonocleGiraffe.Portable.ViewModels
{
	public class SettingsViewModel : ViewModelBase
	{
		public SettingsViewModel()
		{
		}

		private AppSettingsViewModel appSettingsViewModel;
		public AppSettingsViewModel AppSettingsViewModel
		{
			get
			{
				appSettingsViewModel = appSettingsViewModel ?? new AppSettingsViewModel(IsInDesignMode);
				return appSettingsViewModel;
			}
		}
	}
}
