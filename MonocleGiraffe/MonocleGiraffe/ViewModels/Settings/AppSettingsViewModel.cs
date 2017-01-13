using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.LibraryImpl;
using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using XamarinImgur.Models;

namespace MonocleGiraffe.ViewModels.Settings
{
    public class AppSettingsViewModel : Portable.ViewModels.Settings.AppSettingsViewModel
    {
        public AppSettingsViewModel() : base(DesignMode.DesignModeEnabled)
        { }
        
        ObservableCollection<AddOnItem> addOns;
        public ObservableCollection<AddOnItem> AddOns { get { return addOns; } set { Set(ref addOns, value); } }
        
        string noAddOnsMessage = default(string);
        public string NoAddOnsMessage { get { return noAddOnsMessage; } set { Set(ref noAddOnsMessage, value); } }        

        private async void LoadAddOns()
        {
            IsBusy = true;
            AddOnsHelper helper = SimpleIoc.Default.GetInstance<AddOnsHelper>();
            Response<List<AddOnItem>> response = await helper.GetAllAddOns();
            if (!response.IsError && response.Content != null && response.Content.Count == 0)
                NoAddOnsMessage = "No add-ons are available for your version of Windows. Please upgrade to a newer version.";
            else
                AddOns = new ObservableCollection<AddOnItem>(response.Content);
            IsBusy = false;
        }

        protected override void InitDesignTime()
        {
            IsViralEnabled = true;
            AddOns = new ObservableCollection<AddOnItem>();
            AddOns.Add(new AddOnItem { Title = "Remove Ads for Free", Description = "This add-on removes ads for thirty days. Hey, it is free!", FormattedPrice = "₹ 0.00", IsActive = true, ExpiresIn="Expires in one day" });
            AddOns.Add(new AddOnItem { Title = "Support Monocle Giraffe", Description = "This add-on removes ads for thirty days. You also help the Giraffe buy more tuxedos", FormattedPrice = "₹ 100.00" });
        }
    }
}
