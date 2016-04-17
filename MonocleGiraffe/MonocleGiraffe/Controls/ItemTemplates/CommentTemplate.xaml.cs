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

namespace MonocleGiraffe.Controls.ItemTemplates
{
    public sealed partial class CommentTemplate : UserControl
    {
        public CommentTemplate()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler CollapseRequested;

        private void OnCollapseRequested(object sender, RoutedEventArgs e)
        {
            if (CollapseRequested != null)
                CollapseRequested(sender, e);
        }

        private void CommentCollapse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnCollapseRequested(this, e);
        }

        public event RoutedEventHandler ExpandRequested;

        private void OnExpandRequested(object sender, RoutedEventArgs e)
        {
            if (ExpandRequested != null)
                ExpandRequested(sender, e);
        }

        private void CommentExpand_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnExpandRequested(this, e);
        }
    }
}
