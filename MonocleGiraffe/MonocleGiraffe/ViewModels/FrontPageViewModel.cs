using MonocleGiraffe.ViewModels.FrontPage;
using SharpImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;

namespace MonocleGiraffe.ViewModels
{
    public class FrontPageViewModel : ViewModelBase
    {
        public FrontPageViewModel()
        {
            //if (!DesignMode.DesignModeEnabled)
            //    SecretsHelper.RefreshSecrets();
        }
        public GalleryViewModel GalleryVM { get; set; } = new GalleryViewModel();
        public SubredditsViewModel SubredditsVM { get; set; } = new SubredditsViewModel();
        public AccountViewModel AccountVM { get; set; } = new AccountViewModel();

        private int pivotIndex;
        public int PivotIndex
        {
            get { return pivotIndex; }
            set { Set(ref pivotIndex, value); }
        }
        
        DelegateCommand refreshCommand;
        public DelegateCommand RefreshCommand
           => refreshCommand ?? (refreshCommand = new DelegateCommand(async () =>
           {
               await Refresh();
           }, () => true));

        private async Task Refresh()
        {
            switch (PivotIndex)
            {
                case 0:
                    await GalleryVM.Reload();
                    break;
                case 3:
                    await AccountVM.Reload();
                    break;
            }
        }
    }
}
