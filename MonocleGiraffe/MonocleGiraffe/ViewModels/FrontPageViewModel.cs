using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using MonocleGiraffe.ViewModels.FrontPage;
using XamarinImgur.APIWrappers;
using XamarinImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace MonocleGiraffe.ViewModels
{
    public class FrontPageViewModel : ViewModelBase
    {
        private readonly GalaSoft.MvvmLight.Views.INavigationService navigationService;
        public FrontPageViewModel(GalaSoft.MvvmLight.Views.INavigationService nav)
        {
            navigationService = nav;
            if (!DesignMode.DesignModeEnabled)
                Init();
            else
                InitDesignTime();

        }

        private void Init()
        {
            PivotIndex = 0;
        }


        GalleryViewModel galleryVM = default(GalleryViewModel);
        public GalleryViewModel GalleryVM { get { return galleryVM; } set { Set(ref galleryVM, value); } }

        SubredditsViewModel subredditsVM = default(SubredditsViewModel);
        public SubredditsViewModel SubredditsVM { get { return subredditsVM; } set { Set(ref subredditsVM, value); } }

        SearchViewModel searchVM = default(SearchViewModel);
        public SearchViewModel SearchVM { get { return searchVM; } set { Set(ref searchVM, value); } }

        AccountViewModel accountVM = default(AccountViewModel);
        public AccountViewModel AccountVM { get { return accountVM; } set { Set(ref accountVM, value); } }

        private int pivotIndex;
        public int PivotIndex
        {
            get { return pivotIndex; }
            set
            {
                Set(ref pivotIndex, value);
                SetAppBarButtonVisibilities();
                CreateSubVMIfRequired();
            }
        }

        private void CreateSubVMIfRequired()
        {
            switch (PivotIndex)
            {
                case 0:
                    GalleryVM = GalleryVM ?? new GalleryViewModel(navigationService);
                    break;
                case 1:
                    SubredditsVM = SubredditsVM ?? new SubredditsViewModel(navigationService);
                    break;
                case 2:
                    SubredditsVM = SubredditsVM ?? new SubredditsViewModel(navigationService);
                    SearchVM = SearchVM ?? new SearchViewModel(SubredditsVM);
                    break;
                case 3:
                    AccountVM = AccountVM ?? new AccountViewModel();
                    break;
            }
        }

        #region Command Bar

        DelegateCommand uploadCommand;
        public DelegateCommand UploadCommand
           => uploadCommand ?? (uploadCommand = new DelegateCommand(async () =>
           {
               await PickFilesAndUpload();
           }));

        private async Task PickFilesAndUpload()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            var files = await picker.PickMultipleFilesAsync();
            var tasks = new List<Task>();
            if (files != null)
                foreach (var file in files)
                    tasks.Add(ViewModelLocator.GetInstance().TransfersPageViewModel.UploadsVM.Enqueqe(new UploadItem { File = file }));
            BootStrapper.Current.NavigationService.Navigate(typeof(TransfersPage));
            await Task.WhenAll(tasks);
            Debug.WriteLine($"All uploads complete at {DateTime.Now}!");
        }


        DelegateCommand transfersCommand;
        public DelegateCommand TransfersCommand
           => transfersCommand ?? (transfersCommand = new DelegateCommand(() =>
           {
               BootStrapper.Current.NavigationService.Navigate(typeof(TransfersPage));
           }));

        DelegateCommand feedbackCommand;
        public DelegateCommand FeedbackCommand
           => feedbackCommand ?? (feedbackCommand = new DelegateCommand(() =>
           {
               BootStrapper.Current.NavigationService.Navigate(typeof(FeedbackPage));
           }));

        DelegateCommand settingsCommand;
        public DelegateCommand SettingsCommand
           => settingsCommand ?? (settingsCommand = new DelegateCommand(() =>
           {
               BootStrapper.Current.NavigationService.Navigate(typeof(SettingsPage));
           }));

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
                    GalleryVM.Reload();
                    break;
                case 2:
                    await SearchVM.Refresh("default");
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
                    SortVisibility = Visibility.Collapsed;
                    RefreshVisibility = Visibility.Visible;
                    AddVisibility = Visibility.Collapsed;
                    break;
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
            AccountVM = new AccountViewModel();
        }

     
    }
}
