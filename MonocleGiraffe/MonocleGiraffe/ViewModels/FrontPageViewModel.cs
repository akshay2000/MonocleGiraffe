using MonocleGiraffe.ViewModels.FrontPage;
using SharpImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.ViewModels
{
    public class FrontPageViewModel : ViewModelBase
    {
        public GalleryViewModel GalleryVM { get; set; } = new GalleryViewModel();
        public SubredditsViewModel SubredditsVM { get; set; } = new SubredditsViewModel();
        public AccountViewModel AccountVM { get; set; } = new AccountViewModel();
    }
}
