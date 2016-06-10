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
    public sealed partial class ImageHeaderTemplate : UserControl
    {
        public ImageHeaderTemplate()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler CommentToggleRequested;

        private void OnCommentToggleRequested(object sender, RoutedEventArgs e)
        {
            CommentToggleRequested?.Invoke(sender, e);
        }

        private void ToggleCommentsButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnCommentToggleRequested(sender, e);
        }
    }
}
