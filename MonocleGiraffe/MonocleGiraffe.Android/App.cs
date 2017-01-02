using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Android.Activities;
using MonocleGiraffe.Android.ViewModels;

namespace MonocleGiraffe.Android
{
    public static class App
    {
        static App()
        {

        }

        private static ViewModelLocator locator;

        public static ViewModelLocator Locator
        {
            get
            {
                if (locator == null)
                {
                    DispatcherHelper.Initialize();

                    var nav = new NavigationService();                    

                    nav.Configure(ViewModelLocator.FrontPageKey, typeof(FrontActivity));
                    nav.Configure(ViewModelLocator.SubGalleryPageKey, typeof(SubGalleryActivity));
                    nav.Configure(ViewModelLocator.BrowserPageKey, typeof(BrowserActivity));
                    nav.Configure(ViewModelLocator.SubredditBrowserPageKey, typeof(BrowserActivity));

                    SimpleIoc.Default.Register<INavigationService>(() => nav);
                    locator = new ViewModelLocator();
                }

                return locator;
            }
        }
    }
}