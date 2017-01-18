using Newtonsoft.Json.Linq;
using XamarinImgur.APIWrappers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;

namespace MonocleGiraffe.ViewModels.Settings
{
    public class ImgurSettingsViewModel : Portable.ViewModels.Settings.ImgurSettingsViewModel
    {
        public ImgurSettingsViewModel() : base(DesignMode.DesignModeEnabled)
        { }
    }
}