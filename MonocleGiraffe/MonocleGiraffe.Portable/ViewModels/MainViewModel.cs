using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            if (IsInDesignMode)
            {
                SampleText = "Hey, MVVMLight!";
            }
            else
            {
                this.navigationService = navigationService;
                SampleText = "Hey, MVVMLight!";
            }
        }

        string _SampleText = default(string);
        public string SampleText { get { return _SampleText; } set { Set(ref _SampleText, value); } }

        private RelayCommand navigateCommand;
        public RelayCommand NavigateCommand
        {
            get
            {
                return navigateCommand
                    ?? (navigateCommand =
                    new RelayCommand(() => navigationService.NavigateTo(PageKeyHolder.FrontPageKey)));
            }
        }
    }
}