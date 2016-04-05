using MonocleGiraffe.ViewModels.FrontPage;
using SharpImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace MonocleGiraffe.ViewModels
{
    public class FrontPageViewModel : ViewModelBase
    {
        public FrontPageViewModel()
        {
            if (!DesignMode.DesignModeEnabled)
                Init();
            else
                InitDesignTime();

        }

        private void Init()
        {
            PivotIndex = 0;
            GalleryVM = new GalleryViewModel();
            SubredditsVM = new SubredditsViewModel();
            SearchVM = new SearchViewModel(SubredditsVM);
            AccountVM = new AccountViewModel();
        }

        public GalleryViewModel GalleryVM { get; set; }
        public SubredditsViewModel SubredditsVM { get; set; }
        public SearchViewModel SearchVM { get; set; } 
        public AccountViewModel AccountVM { get; set; }

        private int pivotIndex;
        public int PivotIndex
        {
            get { return pivotIndex; }
            set
            {
                Set(ref pivotIndex, value);
                SetAppBarButtonVisibilities();
            }
                    
        }

        #region Command Bar

        DelegateCommand refreshCommand;
        public DelegateCommand RefreshCommand
           => refreshCommand ?? (refreshCommand = new DelegateCommand(async () =>
           {
               await Refresh();
           }));

        DelegateCommand addCommand;
        public DelegateCommand AddCommand
           => addCommand ?? (addCommand = new DelegateCommand(() =>
           {
               PivotIndex = 2;
           }));

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
        
        private void SetAppBarButtonVisibilities()
        {
            switch (PivotIndex)
            {
                case 0:
                    SortVisibility = Visibility.Visible;
                    RefreshVisibility = Visibility.Visible;
                    AddVisibility = Visibility.Collapsed;
                    break;
                case 1:
                    SortVisibility = Visibility.Collapsed;
                    RefreshVisibility = Visibility.Collapsed;
                    AddVisibility = Visibility.Visible;
                    break;
                case 2:
                case 3:
                    SortVisibility = Visibility.Collapsed;
                    RefreshVisibility = Visibility.Visible;
                    AddVisibility = Visibility.Collapsed;
                    break;
            }
        }

        Visibility refreshVisibility;
        public Visibility RefreshVisibility
        {
            get { return refreshVisibility; }
            set { Set(ref refreshVisibility, value); }
        }

        Visibility sortVisibility;
        public Visibility SortVisibility
        {
            get { return sortVisibility; }
            set { Set(ref sortVisibility, value); }
        }

        Visibility addVisibility;
        public Visibility AddVisibility
        {
            get { return addVisibility; }
            set { Set(ref addVisibility, value); }
        }

        #endregion

        private void InitDesignTime()
        {
            PivotIndex = 0;
        }

     
    }
}
