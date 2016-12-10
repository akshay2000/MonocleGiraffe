﻿using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.LibraryImpl;
using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Services.Store;
using XamarinImgur.Models;

namespace MonocleGiraffe.ViewModels.Settings
{
    public class AppSettingsViewModel : BindableBase
    {
        private const string IS_VIRAL_ENABLED = "IsViralEnabled";
        private readonly StoreContext storeContext = StoreContext.GetDefault();

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
        
        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        ObservableCollection<AddOnItem> addOns;
        public ObservableCollection<AddOnItem> AddOns { get { return addOns; } set { Set(ref addOns, value); } }

        public void ChangeViralEnabled()
        {
            Settings.SetValue(IS_VIRAL_ENABLED, IsViralEnabled);
        }

        private async void LoadAddOns()
        {
            IsBusy = true;
            AddOnsHelper helper = SimpleIoc.Default.GetInstance<AddOnsHelper>();
            Response<List<AddOnItem>> response = await helper.GetAllAddOns();
            AddOns = new ObservableCollection<AddOnItem>(response.Content);
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
            LoadAddOns();
        }

        private void InitDesignTime()
        {
            IsViralEnabled = true;
            AddOns = new ObservableCollection<AddOnItem>();
            AddOns.Add(new AddOnItem { Title = "Remove Ads for Free", Description = "This add-on removes ads for thirty days. Hey, it is free!", FormattedPrice = "₹ 0.00" });
            AddOns.Add(new AddOnItem { Title = "Support Monocle Giraffe", Description = "This add-on removes ads for thirty days. You also help the Giraffe buy more tuxedos", FormattedPrice = "₹ 100.00" });
        }
    }
}
