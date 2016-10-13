/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MonocleGiraffe.Android"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MonocleGiraffe.Portable.ViewModels;

namespace MonocleGiraffe.Android.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator : PageKeyHolder
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<FrontViewModel>();
            SimpleIoc.Default.Register<SubGalleryViewModel>();
            SimpleIoc.Default.Register<BrowserViewModel>();
            SimpleIoc.Default.Register<SplashViewModel>();
        }

        public MainViewModel Main { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }

        public FrontViewModel Front { get { return ServiceLocator.Current.GetInstance<FrontViewModel>(); } }

        public SubGalleryViewModel SubGallery { get { return ServiceLocator.Current.GetInstance<SubGalleryViewModel>(); } }

        public BrowserViewModel Browser { get { return ServiceLocator.Current.GetInstance<BrowserViewModel>(); } }

        public SplashViewModel Splash { get { return ServiceLocator.Current.GetInstance<SplashViewModel>(); } }
        
        public static void Cleanup()
        {
        }
    }
}