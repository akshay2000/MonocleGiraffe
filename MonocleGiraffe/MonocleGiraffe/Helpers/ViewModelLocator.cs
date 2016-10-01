using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Portable.ViewModels;
using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace MonocleGiraffe.Helpers
{
    public class ViewModelLocator : Portable.ViewModels.PageKeyHolder, IViewModelLocator
    {
        public ViewModelLocator()
        {
            if (DesignMode.DesignModeEnabled)
                InitDesignTime();
            else
                Init();
            
        }

        private void Init()
        {
            transfersPageViewModel = new Lazy<TransfersPageViewModel>();

            SimpleIoc.Default.Register<SplashPageViewModel>();
            SimpleIoc.Default.Register<SubGalleryPageViewModel>();
            SimpleIoc.Default.Register<FrontPageViewModel>();
            SimpleIoc.Default.Register<BrowserPageViewModel>();           
        }

        private void InitDesignTime()
        {
            var nav = new MergedNavigationService(App.Current.NavigationService);
            SimpleIoc.Default.Register<GalaSoft.MvvmLight.Views.INavigationService>(() => nav);
            Init();
        }

        //TODO: Remove this eventually
        private Lazy<TransfersPageViewModel> transfersPageViewModel;
        public TransfersPageViewModel TransfersPageViewModel { get { return transfersPageViewModel.Value; } }

        public SplashPageViewModel SplashPageViewModel { get { return SimpleIoc.Default.GetInstance<SplashPageViewModel>(); } }

        public FrontPageViewModel FrontPageViewModel { get { return SimpleIoc.Default.GetInstance<FrontPageViewModel>(); } }

        public BrowserViewModel BrowserViewModel { get { return SimpleIoc.Default.GetInstance<BrowserPageViewModel>(); } }

        public SubGalleryPageViewModel SubGalleryPageViewModel { get { return SimpleIoc.Default.GetInstance<SubGalleryPageViewModel>(); } }

        public TransfersViewModel TransfersViewModel { get { return TransfersPageViewModel; } }

        public static ViewModelLocator GetInstance()
        {
            return (ViewModelLocator)Template10.Common.BootStrapper.Current.Resources["Locator"];
        }
    }
}
