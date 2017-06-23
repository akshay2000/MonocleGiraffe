using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using static MonocleGiraffe.Portable.Helpers.Initializer;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class SplashViewModel : ViewModelBase, INavigable
    {
        protected INavigationService nav;
        protected LaunchType launchType;

        public SplashViewModel(INavigationService navigationService)
        {
            nav = navigationService;
            if (IsInDesignMode)
            {
                State = BUSY;
                Message = "Connecting...";
            }
        }

        string state = default(string);
        public string State { get { return state; } set { Set(ref state, value); } }

        string message = default(string);
        public string Message { get { return message; } set { Set(ref message, value); } }

        private const string BUSY = "Busy";
        private const string ANON_ERROR = "AnonError";
        private const string AUTH_ERROR = "AuthError";

        public async Task<bool> ShakeHands()
        {
            State = BUSY;
            Message = "Connecting...";
            bool anonSuccess = await ShakeAnonHands();
            if (!anonSuccess)
            {
                State = ANON_ERROR;
                return false;
            }

            if (Helpers.Initializer.AuthenticationHelper.IsAuthIntended())
            {
                bool authSuccess = await ShakeAuthHands();
                if (!authSuccess)
                {
                    State = AUTH_ERROR;
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> ShakeAuthHands()
        {
            JObject result = null;
            bool isSuccess = false;

            try
            {
                result = await GetUserImageCount();
            }
            catch (AuthException e)
            {
                if (e.Reason == AuthException.AuthExceptionReason.HttpError)
                    Message = e.Message;
                else
                    isSuccess = true;
            }

            if (result != null && result.HasValues)
            {
                isSuccess = (bool)result["success"];
                if (!isSuccess)
                {
                    //TODO: flush http clients here
                    SimpleIoc.Default.GetInstance<NetworkHelper>().FlushHttpClients();
                    await Helpers.Initializer.SecretsHelper.RefreshAccessToken();
                    result = await GetUserImageCount();
                    isSuccess = (bool)result["success"];
                    Message = JsonConvert.SerializeObject(result["data"], Formatting.Indented);
                }
            }
            if (!isSuccess)
                Message = "Connection Error";
            return isSuccess;
        }

        private async Task<JObject> GetUserImageCount()
        {
            string userName = await Helpers.Initializer.SecretsHelper.GetUserName();
            const string urlPattern = "account/{0}/images/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await Helpers.Initializer.NetworkHelper.ExecuteRequest(url);
            return result;
        }

        private async Task<bool> ShakeAnonHands()
        {
            bool wasAuthIntended = Helpers.Initializer.AuthenticationHelper.IsAuthIntended();
            Helpers.Initializer.AuthenticationHelper.SetAuthIntention(false);
            bool isSuccess = false;
            try
            {
                var response = await GetCredis();
                if (response.IsError)
                {
                    isSuccess = false;
                    Message = string.IsNullOrWhiteSpace(response.Message) ? "Connection Error" : response.Message;
                }
                else
                {
                    var result = response.Content;
                    if (result.ClientRemaining == 0)
                    {
                        isSuccess = false;
                        Message = "Monocle Giraffe is overloaded. Try again later.";
                    }
                    else if (result.UserRemaining == 0)
                    {
                        isSuccess = false;
                        Message = "You seem to be using app too much. Are you sure you're not a bot?";
                    }
                    else
                    {
                        isSuccess = true;
                    }
                }
            }
            catch
            {
                isSuccess = false;
            }
            finally
            {
                Helpers.Initializer.AuthenticationHelper.SetAuthIntention(wasAuthIntended);
            }
            return isSuccess;
        }

        private async Task<Response<CreditResult>> GetCredis()
        {
            const string url = "credits";
            Response<CreditResult> result = await Portable.Helpers.Initializer.NetworkHelper.GetRequest<CreditResult>(url);
            return result;
        }

        public async Task<bool> SignIn()
        {
            State = BUSY;
            try
            {
                await Helpers.Initializer.SecretsHelper.RefreshSecrets();
            }
            catch
            {
                Message = "Authentication Error";
                State = AUTH_ERROR;
                return false;
            }
            bool isSuccess = await ShakeAuthHands();
            if (!isSuccess)
                State = AUTH_ERROR;
            return isSuccess;
        }

        public async Task ShakeHandsAndNavigate()
        {
            if (!(await ShakeHands()))
                return;
            await Navigate();
        }

        public async Task SignInAndNavigate()
        {
            if (!(await SignIn()))
                return;
            await Navigate();
        }

        public virtual async Task Navigate()
        {
            switch (launchType)
            {
                case LaunchType.Url:
                    nav.NavigateTo(PageKeyHolder.BrowserPageKey);
                    break;
                case LaunchType.SecondaryTile:
                    nav.NavigateTo(PageKeyHolder.SubGalleryPageKey);
                    break;                        
                case LaunchType.AppTile:
                default:
                    nav.NavigateTo(PageKeyHolder.FrontPageKey);
                    break;
            }
        }

        public void Activate(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Deactivate()
        {
            throw new NotImplementedException();
        }
    }
}
