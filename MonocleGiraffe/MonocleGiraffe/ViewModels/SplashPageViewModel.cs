using MonocleGiraffe.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Template10.Common;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Helpers;
using MonocleGiraffe.Portable.Helpers;

namespace MonocleGiraffe.ViewModels
{
    public class SplashPageViewModel : SplashViewModel, INavigable
    {
        public INavigationService NavigationService { get; set; }
        public IDispatcherWrapper Dispatcher { get; set; }
        public IStateItems SessionState { get; set; }

        public SplashPageViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav)
        { }

        public Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            return Task.CompletedTask;
        }

        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            (nav as MergedNavigationService).Clear();
            JObject param = (JObject)Portable.Helpers.StateHelper.SessionState["LaunchData"];
            Portable.Helpers.StateHelper.SessionState.Remove("LaunchData");
            launchType = param["launchType"].ToObject<LaunchType>();

            switch (launchType)
            {
                case LaunchType.Url:
                    HandleUrl(param, state);                     
                    break;
                case LaunchType.SecondaryTile:
                    HandleSecondaryTile(param);
                    break;
                case LaunchType.AppTile:
                default:
                    break;
            }          

            if ((bool)param["isNewLaunch"])
                await base.ShakeHandsAndNavigate();
            else
                await Navigate();
        }

        private void HandleSecondaryTile(JObject param)
        {
            string subreddit = ((string)param["tileArgs"]).Substring(12);
            //TODO: Populate Subreddit gallery args
        }

        private void HandleUrl(JObject param, IDictionary<string, object> state)
        {
            string url = (string)param["url"];
            var query = Uri.UnescapeDataString(new Uri(url).Query);
            query = query.StartsWith("?") ? query.Substring(1) : query;
            string[] frags = query.Split('&');
            foreach (var frag in frags)
            {
                string[] splits = frag.Split('=');
                state.Add(splits[0], splits[1]);
            }
            (SimpleIoc.Default.GetInstance<IViewModelLocator>().BrowserViewModel as BrowserPageViewModel).State = state;
        }

        public new async void ShakeHandsAndNavigate()
        {
            await base.ShakeHandsAndNavigate();
        }

        public new async void SignInAndNavigate()
        {
            await base.SignInAndNavigate();
        }

        public async override Task Navigate()
        {
            await base.Navigate();
            await Task.Delay(100);
            (nav as MergedNavigationService).Clear();
        }

        public Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
