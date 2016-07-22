using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using XamarinImgur.Interfaces;

namespace MonocleGiraffe.LibraryImpl
{
    public class AuthBroker : IAuthBroker
    {
        public async Task<AuthResult> AuthenticateAsync(Uri requestUri, Uri callbackUri)
        {
            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);
            AuthResult ret = new AuthResult(result.ResponseData, (AuthResponseStatus)Enum.Parse(typeof(AuthResponseStatus), result.ResponseStatus.ToString()), result.ResponseErrorDetail);
            return ret;
        }
    }
}
