using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MonocleGiraffe.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool flag = value as bool? ?? false;
            SolidColorBrush ret = new SolidColorBrush(FromHex((string)parameter));
            if (flag)
                return ret;
            else
                return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private Color FromHex(string hex)
        {
            byte a = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
            byte r = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(7, 2), NumberStyles.HexNumber);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
