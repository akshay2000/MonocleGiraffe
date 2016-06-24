using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace MonocleGiraffe.Helpers
{
    public class ViewModelLocator
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
            frontPageViewModel = new Lazy<FrontPageViewModel>();
            transfersPageViewModel = new Lazy<TransfersPageViewModel>();
        }

        private void InitDesignTime()
        {
            Init();
        }

        private Lazy<TransfersPageViewModel> transfersPageViewModel;
        public TransfersPageViewModel TransfersPageViewModel { get { return transfersPageViewModel.Value; } }

        private Lazy<FrontPageViewModel> frontPageViewModel;
        public FrontPageViewModel FrontPageViewModel { get { return frontPageViewModel.Value; } }

        public static ViewModelLocator GetInstance()
        {
            return (ViewModelLocator)Template10.Common.BootStrapper.Current.Resources["Locator"];
        }
    }
}
