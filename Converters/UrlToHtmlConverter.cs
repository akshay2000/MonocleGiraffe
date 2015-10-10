using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.Converters
{
    public class UrlToHtmlConverter : IValueConverter
    {
        private const string baseHtml = @"<html style=""overflow: hidden;""><body><video style=""margin: -8px;"" preload=""auto"" autoplay=""autoplay"" loop=""loop"" muted=""muted""><source src=""{MP4LINK}"" type=""video/mp4""></video></body></html>";
        private const string replaceKey = "{MP4LINK}";
        public object Convert(object value, Type targetType, object parameter, string language)
        {            
            string url = value as string;
            if (url == null || url == "")
                return "";
            return baseHtml.Replace(replaceKey, url);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
