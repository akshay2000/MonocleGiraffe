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
        }        

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

        private ObservableCollection<Trophy> trophies;
        public ObservableCollection<Trophy> Trophies
        {
            get { return trophies; }
            set { Set(ref trophies, value); }
        }

        private void InitDesignTime()
        {
            UserName = "akshay2000";
            Points = 524545;
            Trophies = new ObservableCollection<Trophy> {
                new Trophy { Name = "Gone Mobile", Image = "http://s.imgur.com/images/trophies/3c4711.png" },
                new Trophy { Name = "3 Years", Image = "http://s.imgur.com/images/trophies/f09d7a.png" }
                };
        }

    }
}
