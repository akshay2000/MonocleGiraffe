using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Helpers;

namespace MonocleGiraffe.Portable.ViewModels
{
    public class SplashViewModel : ViewModelBase, INavigable
    {        
        string state = default(string);
        public string State { get { return state; } set { Set(ref state, value); } }
        
        string message = default(string);
        public string Message { get { return message; } set { Set(ref message, value); } }

        private const string BUSY = "Busy";
        private const string ERROR = "Error";

        public async Task<bool> ShakeHands()
        {
            State = BUSY;
            Message = "Connecting...";
            bool anonSuccess = await ShakeAnonHands();
            if (!anonSuccess)
            {
                State = ERROR;
                return false;
            }

            if (AuthenticationHelper.IsAuthIntended())
            {
                bool authSuccess = await ShakeAuthHands();
                if (!authSuccess)
                {
                    State = ERROR;
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
                    await SecretsHelper.RefreshAccessToken();
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
            string userName = await SecretsHelper.GetUserName();
            const string urlPattern = "account/{0}/images/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result;
        }

        private async Task<bool> ShakeAnonHands()
        {
            bool wasAuthIntended = AuthenticationHelper.IsAuthIntended();
            AuthenticationHelper.SetAuthIntention(false);
            JObject result;
            bool isSuccess = false;
            try
            {
                result = await GetCredis();
                isSuccess = (bool)result["success"];
            }
            catch
            {
                isSuccess = false;
            }
            finally
            {
                AuthenticationHelper.SetAuthIntention(wasAuthIntended);
            }
            return isSuccess;
        }

        private async Task<JObject> GetCredis()
        {
            const string url = "credits";
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result;
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
