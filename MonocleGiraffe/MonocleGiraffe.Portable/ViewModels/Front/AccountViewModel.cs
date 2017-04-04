using XamarinImgur.APIWrappers;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonocleGiraffe.Portable.Models;
using MonocleGiraffe.Portable.Helpers;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MonocleGiraffe.Portable.ViewModels.Settings;

namespace MonocleGiraffe.Portable.ViewModels.Front
{
    public class AccountViewModel : BindableBase
    {
        private readonly INavigationService navigationService;

        public AccountViewModel(INavigationService nav, bool isInDesignMode)
        {
            navigationService = nav;
            if (isInDesignMode)
                InitDesignTime();
            else
                Init();
        }

        private async Task Init()
        {
#if DEBUG
            UserName = "akshay2000";
#endif
            Messenger.Default.Register<NotificationMessage>(this, HandleMessege);
            if (Helpers.Initializer.AuthenticationHelper.IsAuthIntended())
            {
                await Load();
            }
        }

        private const string AUTHENTICATED = "Authenticated";
        private const string NOT_AUTHENTICATED = "NotAuthenticated";
        private const string BUSY = "Busy";
        string state = NOT_AUTHENTICATED;
        public string State { get { return state; } set { Set(ref state, value); OnPropertyChanged("IsBusy"); } }

        public async void HandleMessege(NotificationMessage message)
        {
            string payload = message.Notification;
            switch (payload)
            {
                case ImgurSettingsViewModel.SIGN_IN:
                    await Load();
                    break;
                case ImgurSettingsViewModel.SIGN_OUT:
                    State = NOT_AUTHENTICATED;
                    break;
            }
        }

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
            Helpers.Initializer.AuthenticationHelper.SetAuthIntention(true);
            var userName = await Helpers.Initializer.SecretsHelper.GetUserName();
            await Task.Delay(1000);
            Account account = await Helpers.Initializer.Accounts.GetAccount(userName);
            UserName = account.Url;
            Points = account.Reputation;
            GalleryProfile galleryProfile = await Helpers.Initializer.Accounts.GetGalleryProfile(userName);
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
        
        RelayCommand signInCommand;
        public RelayCommand SignInCommand
           => signInCommand ?? (signInCommand = new RelayCommand(async () =>
           {
               await Load();
               Messenger.Default.Send(new NotificationMessage(ImgurSettingsViewModel.SIGN_IN));
           }));


        RelayCommand<string> viewAllCommand;
        public RelayCommand<string> ViewAllCommand
           => viewAllCommand ?? (viewAllCommand = new RelayCommand<string>(ViewAllCommandExecute, ViewAllCommandCanExecute));
        bool ViewAllCommandCanExecute(string param) => true;
        void ViewAllCommandExecute(string param)
        {
            switch (param)
            {
                case "albums":
                    GoToBrowser((IEnumerable<IGalleryItem>)Albums, 0, PageKeyHolder.SelfBrowserPageKey);
                    break;
                case "images":
                    GoToBrowser(Images, 0, PageKeyHolder.SelfBrowserPageKey);
                    break;
                case "favourites":
                    GoToBrowser(Favourites, 0, PageKeyHolder.SubredditBrowserPageKey);
                    break;
            }
        }

        protected void ImageTapped(IGalleryItem clickedItem, object collection)
        {
            if (collection == Albums)
                GoToBrowser((IEnumerable<IGalleryItem>)Albums, Albums.IndexOf((AlbumItem)clickedItem), PageKeyHolder.SelfBrowserPageKey);
            else if (collection == Images)
                GoToBrowser(Images, Images.IndexOf((GalleryItem)clickedItem), PageKeyHolder.SelfBrowserPageKey);
            else if (collection == Favourites)
                GoToBrowser(Favourites, Favourites.IndexOf((GalleryItem)clickedItem), PageKeyHolder.SubredditBrowserPageKey);
        }

        private void GoToBrowser(IEnumerable<IGalleryItem> gallery, int index, string pageKey)
        {
            const string navigationParamName = "GalleryInfo";
            var galleryMetaInfo = new GalleryMetaInfo { Gallery = gallery, SelectedIndex = index };
            Helpers.StateHelper.SessionState[navigationParamName] = galleryMetaInfo;
            navigationService.NavigateTo(pageKey, navigationParamName);
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
            var albums = await Helpers.Initializer.Accounts.GetAlbums(userName);
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
            var images = await Helpers.Initializer.Accounts.GetImages(userName);
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
            var favourites = await Helpers.Initializer.Accounts.GetFavourites(userName);
            foreach (var i in favourites)
            {
                Favourites.Add(new GalleryItem(i));
            }
        }

        #endregion

        protected virtual void InitDesignTime()
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
