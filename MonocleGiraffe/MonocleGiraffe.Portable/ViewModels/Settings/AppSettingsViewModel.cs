using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Portable.Helpers;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.Portable.ViewModels.Settings
{
    public class AppSettingsViewModel : BindableBase
    {
        private const string IS_VIRAL_ENABLED = "IsViralEnabled";
        public const string IS_MATURE_ENABLED = "IsMatureEnabled";

        private ISettingsHelper Settings { get { return SimpleIoc.Default.GetInstance<ISettingsHelper>(); } }

        public AppSettingsViewModel(bool isInDesignMode)
        {
            if (isInDesignMode)
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
        
        bool isMatureEnabled = default(bool);
        public bool IsMatureEnabled { get { return isMatureEnabled; } set { Set(ref isMatureEnabled, value); } }

        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        //ObservableCollection<AddOnItem> addOns;
        //public ObservableCollection<AddOnItem> AddOns { get { return addOns; } set { Set(ref addOns, value); } }

        //string noAddOnsMessage = default(string);
        //public string NoAddOnsMessage { get { return noAddOnsMessage; } set { Set(ref noAddOnsMessage, value); } }

        public void ChangeViralEnabled()
        {
            Settings.SetValue(IS_VIRAL_ENABLED, IsViralEnabled);
        }

        public void ChangeMatureEnabled()
        {
            Settings.SetValue(IS_MATURE_ENABLED, IsMatureEnabled);
        }

        //private async void LoadAddOns()
        //{
        //    IsBusy = true;
        //    AddOnsHelper helper = SimpleIoc.Default.GetInstance<AddOnsHelper>();
        //    Response<List<AddOnItem>> response = await helper.GetAllAddOns();
        //    if (!response.IsError && response.Content != null && response.Content.Count == 0)
        //        NoAddOnsMessage = "No add-ons are available for your version of Windows. Please upgrade to a newer version.";
        //    else
        //        AddOns = new ObservableCollection<AddOnItem>(response.Content);
        //    IsBusy = false;
        //}

        private void Init()
        {
            IsViralEnabled = Settings.GetValue<bool>(IS_VIRAL_ENABLED, false);
            IsMatureEnabled = Settings.GetValue<bool>(IS_MATURE_ENABLED, false);
            //LoadAddOns();
        }

        protected virtual void InitDesignTime()
        {
            IsViralEnabled = true;
            IsMatureEnabled = false;
            //AddOns = new ObservableCollection<AddOnItem>();
            //AddOns.Add(new AddOnItem { Title = "Remove Ads for Free", Description = "This add-on removes ads for thirty days. Hey, it is free!", FormattedPrice = "₹ 0.00", IsActive = true, ExpiresIn = "Expires in one day" });
            //AddOns.Add(new AddOnItem { Title = "Support Monocle Giraffe", Description = "This add-on removes ads for thirty days. You also help the Giraffe buy more tuxedos", FormattedPrice = "₹ 100.00" });
        }
    }
}
