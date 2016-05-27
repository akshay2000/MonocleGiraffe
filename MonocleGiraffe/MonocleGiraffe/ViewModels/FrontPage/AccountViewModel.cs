using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using SharpImgur.APIWrappers;
using SharpImgur.Helpers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.ApplicationModel;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class AccountViewModel : BindableBase
    {
        public AccountViewModel()
        {
            if (DesignMode.DesignModeEnabled)
                InitDesignTime();
            else
                Init();
        }

        private async Task Init()
        {
            if (AuthenticationHelper.IsAuthIntended())
            {
                await Load();
            }
        }

        private const string AUTHENTICATED = "Authenticated";
        private const string NOT_AUTHENTICATED = "NotAuthenticated";
        private const string BUSY = "Busy";
        string state = NOT_AUTHENTICATED;
        public string State { get { return state; } set { Set(ref state, value); } }

        private async Task Load()
        {
            try
            {
                State = BUSY;
                await LoadUserData();
                State = AUTHENTICATED;
            }
            catch
            {
                State = NOT_AUTHENTICATED;
            }
        }
        
        private async Task LoadUserData()
        {
            var userName = await SecretsHelper.GetUserName();
            await Task.Delay(1000);
            Account account = await Accounts.GetAccount(userName);
            UserName = account.Url;
            Points = account.Reputation;
            GalleryProfile galleryProfile = await Accounts.GetGalleryProfile(userName);
            Trophies = new ObservableCollection<Trophy>(galleryProfile.Trophies);
            await Task.Delay(500);
            await LoadAlbums(userName);
            await Task.Delay(500);
            await LoadImages(userName);
            await Task.Delay(500);
            await LoadFavourites(userName);
        }

        public async Task Reload()
        {
            await Init();
        }

        public bool IsBusy
        {
            get
            {
                return State == BUSY;
            }
        }

        DelegateCommand signInCommand;
        public DelegateCommand SignInCommand
           => signInCommand ?? (signInCommand = new DelegateCommand(async () =>
           {
               await Load();
           }, () => !IsBusy));


        DelegateCommand<string> viewAllCommand;
        public DelegateCommand<string> ViewAllCommand
           => viewAllCommand ?? (viewAllCommand = new DelegateCommand<string>(ViewAllCommandExecute, ViewAllCommandCanExecute));
        bool ViewAllCommandCanExecute(string param) => true;
        void ViewAllCommandExecute(string param)
        {
            switch (param)
            {
                case "albums":
                    GoToBrowser(Albums, 0);
                    break;
                case "images":
                    GoToBrowser(Images, 0);
                    break;
                case "favourites":
                    GoToBrowser(Favourites, 0);
                    break;
            }
        }

        private void GoToBrowser(IEnumerable<IGalleryItem> gallery, int index)
        {
            const string navigationParamName = "GalleryInfo";
            var galleryMetaInfo = new GalleryMetaInfo { Gallery = gallery, SelectedIndex = index };
            BootStrapper.Current.SessionState[navigationParamName] = galleryMetaInfo;
            BootStrapper.Current.NavigationService.Navigate(typeof(BrowserPage), navigationParamName);
            return;
        }

        #region User

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { Set(ref userName, value); }
        }

        private long points;
        public long Points
        {
            get { return points; }
            set { Set(ref points, value); }
        }

        #endregion

        #region Trophies

        private ObservableCollection<Trophy> trophies;
        public ObservableCollection<Trophy> Trophies
        {
            get { return trophies; }
            set { Set(ref trophies, value); }
        }

        #endregion

        #region Albums

        private ObservableCollection<AlbumItem> albums;
        public ObservableCollection<AlbumItem> Albums
        {
            get { return albums; }
            set { Set(ref albums, value); }
        }

        private async Task LoadAlbums(string userName)
        {
            Albums = new ObservableCollection<AlbumItem>();
            var albums = await Accounts.GetAlbums(userName);
            foreach (var a in albums)
            {
                Albums.Add(new AlbumItem(a));
            }
        }        

        #endregion

        #region Images

        private ObservableCollection<GalleryItem> images;
        public ObservableCollection<GalleryItem> Images
        {
            get { return images; }
            set { Set(ref images, value); }
        }

        private async Task LoadImages(string userName)
        {
            Images = new ObservableCollection<GalleryItem>();
            var images = await Accounts.GetImages(userName);
            foreach (var i in images)
            {
                Images.Add(new GalleryItem(i));
            }
        }

        #endregion

        #region Favourites

        private ObservableCollection<GalleryItem> favourites;
        public ObservableCollection<GalleryItem> Favourites
        {
            get { return favourites; }
            set { Set(ref favourites, value); }
        }

        private async Task LoadFavourites(string userName)
        {
            Favourites = new ObservableCollection<GalleryItem>();
            var favourites = await Accounts.GetFavourites(userName);
            foreach (var i in favourites)
            {
                Favourites.Add(new GalleryItem(i));
            }
        }

        #endregion

        private void InitDesignTime()
        {
            UserName = "akshay2000";
            Points = 524545;
            State = AUTHENTICATED;
            Trophies = new ObservableCollection<Trophy> {
                new Trophy { Name = "Gone Mobile", Image = "http://s.imgur.com/images/trophies/3c4711.png" },
                new Trophy { Name = "3 Years", Image = "http://s.imgur.com/images/trophies/f09d7a.png" }
                };
            Albums = new ObservableCollection<AlbumItem>();
            Albums.Add(new AlbumItem(new Album { Cover = "vjpNYII" }));
            Albums.Add(new AlbumItem(new Album { Cover = "eZBrROO" }));
            Albums.Add(new AlbumItem(new Album { Cover = "FExPJrk" }));
            Albums.Add(new AlbumItem(new Album { Cover = "nqpaOvc" }));

            Images = new ObservableCollection<GalleryItem>();
            Images.Add(new GalleryItem(new Image { Id = "vjpNYII" }));
            Images.Add(new GalleryItem(new Image { Id = "eZBrROO" }));
            Images.Add(new GalleryItem(new Image { Id = "FExPJrk" }));
            Images.Add(new GalleryItem(new Image { Id = "nqpaOvc" }));
            Images.Add(new GalleryItem(new Image { Id = "vjpNYII" }));
        }
    }
}
