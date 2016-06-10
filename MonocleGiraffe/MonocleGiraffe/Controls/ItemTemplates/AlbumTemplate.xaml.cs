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
    public sealed partial class AlbumTemplate : UserControl
    {
        public AlbumTemplate()
        {
            this.InitializeComponent();
        }
                
        private void ShowComments()
        {
            CommentsView.Visibility = Visibility.Visible;
            ImagePanel.Visibility = Visibility.Collapsed;
        }

        private void HideComments()
        {
            CommentsView.Visibility = Visibility.Collapsed;
            ImagePanel.Visibility = Visibility.Visible;
        }

        private void ImageHeaderTemplate_CommentToggleRequested(object sender, RoutedEventArgs e)
        {
            if ((CommentsView ?? (FrameworkElement)FindName(nameof(CommentsView))).Visibility == Visibility.Collapsed)
                ShowComments();
            else
                HideComments();
        }
    }
}
