using MonocleGiraffe.Models;
using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private async void Init()
        {
            await Task.Delay(1000);
            Account account = await Accounts.GetAccount();
            UserName = account.Url;
            Points = account.Reputation;
            GalleryProfile galleryProfile = await Accounts.GetGalleryProfile();
            Trophies = new ObservableCollection<Trophy>(galleryProfile.Trophies);
            await Task.Delay(500);
            await LoadAlbums();
            await Task.Delay(500);
            //await LoadImages();
        }

        public async Task Reload()
        {
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
            set { points = value; }
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

        private async Task LoadAlbums()
        {
            Albums = new ObservableCollection<AlbumItem>();
            var albums = await Accounts.GetAlbums();
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

        private async Task LoadImages()
        {
            Images = new ObservableCollection<GalleryItem>();
            var images = await Accounts.GetImages();
            foreach (var i in images)
            {
                Images.Add(new GalleryItem(i));
            }
        }

        #endregion

        private void InitDesignTime()
        {
            UserName = "akshay2000";
            Points = 524545;
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
