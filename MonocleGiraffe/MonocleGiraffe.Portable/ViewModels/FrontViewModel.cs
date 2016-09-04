using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.ViewModels.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class FrontViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        public FrontViewModel(INavigationService nav)
        {
            navigationService = nav;
        }

        private GalleryViewModel galleryVM;
        public GalleryViewModel GalleryVM
        {
            get
            {
                galleryVM = galleryVM ?? new GalleryViewModel(navigationService, IsInDesignMode);
                return galleryVM;
            }
        }

        private SubredditsViewModel subredditsVM;
        public SubredditsViewModel SubredditsVM
        {
            get
            {
                subredditsVM = subredditsVM ?? new SubredditsViewModel(navigationService, IsInDesignMode);
                return subredditsVM;
            }
        }

        private SearchViewModel searchVM;
        public SearchViewModel SearchVM
        {
            get
            {
                searchVM = searchVM ?? new SearchViewModel(SubredditsVM, navigationService, IsInDesignMode);
                return searchVM;
            }
        }

        private AccountViewModel accountVM;
        public AccountViewModel AccountVM
        {
            get
            {
                accountVM = accountVM ?? new AccountViewModel(navigationService, IsInDesignMode);
                return accountVM;
            }
        }

    }
}
