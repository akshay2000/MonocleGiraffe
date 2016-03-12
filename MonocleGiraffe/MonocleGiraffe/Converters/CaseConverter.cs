using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MonocleGiraffe.Converters
{
    public class CaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string text = (string)value ?? string.Empty;
            string outCase = (string)parameter;
            switch (outCase)
            {
                case "u":
                    return text.ToUpper();
                case "l":
                default:
                    return text.ToLower();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
