using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MonocleGiraffe.Controls.Extensions
{
    public class ToggleMenuFlyoutItemExtensions
    {
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterAttached("IsChecked",
                                          typeof(bool),
                                          typeof(ToggleMenuFlyoutItemExtensions),
                                          new PropertyMetadata(false, new PropertyChangedCallback(OnChanged)));

        public static bool GetIsChecked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCheckedProperty);
        }
        public static void SetIsChecked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCheckedProperty, value);
        }

        private static void OnChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            ToggleMenuFlyoutItem tb = o as ToggleMenuFlyoutItem;
            if (null != tb)
                tb.IsChecked = (bool)args.NewValue;
        }
    }
}
