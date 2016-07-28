using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinImgur.Interfaces
{
    public interface IAuthBroker
    {
        Task<AuthResult> AuthenticateAsync(Uri requestUri, Uri callbackUri);
    }

    public class AuthResult
    {
        public AuthResult(Dictionary<string, string> data, AuthResponseStatus status)
        {
            ResponseData = data;
            ResponseStatus = status;
        }

        public AuthResult(Dictionary<string, string> data, AuthResponseStatus status, uint errorDetail)
        {
            ResponseData = data;
            ResponseStatus = status;
            ErrorDetail = errorDetail;
        }

        public Dictionary<string, string> ResponseData { get; }
        public AuthResponseStatus ResponseStatus { get; }
        public uint ErrorDetail { get; }
    }

    public enum AuthResponseStatus
    {
        Success,
        UserCancel,
        ErrorHttp
    }    
}
