using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using MonocleGiraffe.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class SubredditsViewModel : Portable.ViewModels.Front.SubredditsViewModel
    {
        public SubredditsViewModel(INavigationService nav) : base(nav, DesignMode.DesignModeEnabled)
        { }

        public void SubredditTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as SubredditItem;
            NavigateToSub(clickedItem);
        }
    }
}
