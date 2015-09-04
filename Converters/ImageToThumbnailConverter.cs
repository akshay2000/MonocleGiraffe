using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.Converters
{
    public class ImageToThumbnailConverter : IValueConverter
    {
        private const string baseUrl = "http://i.imgur.com/";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Image image = (Image)value;
            string sizeSuffix = (string)parameter ?? "s";
            if (image.IsAlbum)
            {
                return baseUrl + "BSa65e1.jpg";
            }
            else
            {
                return baseUrl + image.Id + sizeSuffix + ".jpg";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
