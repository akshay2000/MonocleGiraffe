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
    public sealed partial class CommentsUserControl : UserControl
    {
        public CommentsUserControl()
        {
            this.InitializeComponent();
        }          

        private void CommentWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ExpandComments(sender);
        }

        private void ExpandComments(object sender)
        {
            Grid grid = sender as Grid;
            var children = (grid.Parent as Grid).Children;
            var moreItem = children[1];
            moreItem.Visibility = moreItem.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
