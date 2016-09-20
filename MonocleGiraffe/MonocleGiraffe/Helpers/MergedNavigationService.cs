using System;
using System.Collections.Generic;

namespace MonocleGiraffe.Helpers
{
    public class MergedNavigationService : GalaSoft.MvvmLight.Views.INavigationService
    {
        Template10.Services.NavigationService.INavigationService t10Nav;
        private Dictionary<string, Type> keyConfiguration;

        public MergedNavigationService(Template10.Services.NavigationService.INavigationService nav)
        {
            t10Nav = nav;
            keyConfiguration = new Dictionary<string, Type>();
        }

        public void Configure(string pageKey, Type pageType)
        {
            keyConfiguration[pageKey] = pageType;            
        }

        public void Clear()
        {
            t10Nav.ClearHistory();
        }

        #region INavigationService

        public string CurrentPageKey { get { return t10Nav.CurrentPageType.Name; } }

        public void GoBack()
        {
            if (t10Nav.CanGoBack)
                t10Nav.GoBack();
        }

        public void NavigateTo(string pageKey)
        {
            t10Nav.Navigate(keyConfiguration[pageKey]);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            t10Nav.Navigate(keyConfiguration[pageKey], parameter);
        }

        #endregion
    }
}
