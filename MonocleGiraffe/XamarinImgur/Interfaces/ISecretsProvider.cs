using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Interfaces
{
    public interface ISecretsProvider
    {
        Task<JObject> GetSecrets();
    }
}
