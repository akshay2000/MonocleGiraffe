using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Interfaces
{
    public interface ISettingsHelper
    {
        void SetValue(string key, object value);
        T GetValue<T>(string key);
        T GetValue<T>(string key, object defaultValue);
        void SetLocalValue(string key, object value);
        T GetLocalValue<T>(string key);
        T GetLocalValue<T>(string key, object defaultValue);
        void RemoveLocalValue(string key);
    }
}
