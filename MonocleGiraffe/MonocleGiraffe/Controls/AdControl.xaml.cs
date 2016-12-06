using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MonocleGiraffe.Controls
{
    public sealed partial class AdControl : UserControl
    {
        public AdControl()
        {
            this.InitializeComponent();
        }

        private void Close_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnCloseTapped(this, e);
        }

        public event RoutedEventHandler CloseTapped;

        private void OnCloseTapped(object sender, RoutedEventArgs e)
        {
            CloseTapped?.Invoke(sender, e);
        }

        public void Init(string appKey, string uid)
        {
            var ad = new AdDuplex.AdControl();
            ad.AdUnitId = uid;
            ad.AppKey = appKey;
            LayoutRoot.Children.Add(ad);
        }
    }
}
