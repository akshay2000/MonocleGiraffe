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
    public sealed partial class ImageUserControl : UserControl
    {
        public ImageUserControl()
        {
            this.InitializeComponent();
        }

        private void LayoutRoot_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (IsHolding)
                return;
            var targetElement = sender as FrameworkElement;
            MenuFlyout flyout = (MenuFlyout)FlyoutBase.GetAttachedFlyout(targetElement);
            flyout.ShowAt(targetElement, e.GetPosition(targetElement));
        }

        public bool IsHolding { get; set; }
        private void LayoutRoot_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState != Windows.UI.Input.HoldingState.Started)
                return;
            IsHolding = true;
            var targetElement = sender as FrameworkElement;
            MenuFlyout flyout = (MenuFlyout)FlyoutBase.GetAttachedFlyout(targetElement);
            flyout.ShowAt(targetElement, e.GetPosition(targetElement));          
        }
    }
}
