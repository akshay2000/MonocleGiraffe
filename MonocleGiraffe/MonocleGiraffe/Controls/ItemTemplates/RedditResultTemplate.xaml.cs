using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
    public sealed partial class RedditResultTemplate : UserControl
    {
        public RedditResultTemplate()
        {
            this.InitializeComponent();
        }

        public ICommand ToggleFavoriteCommand
        {
            get { return (ICommand)GetValue(ToggleFavoriteCommandProperty); }
            set { SetValue(ToggleFavoriteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleFavoriteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleFavoriteCommandProperty =
            DependencyProperty.Register("ToggleFavoriteCommand", typeof(ICommand), typeof(RedditResultTemplate), null);

        private void ToggleWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleFavoriteCommand.Execute(DataContext);
            e.Handled = true;
        }

        public ICommand GoToSubCommand
        {
            get { return (ICommand)GetValue(GoToSubCommandProperty); }
            set { SetValue(GoToSubCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GoToSubCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToSubCommandProperty =
            DependencyProperty.Register("GoToSubCommand", typeof(ICommand), typeof(RedditResultTemplate), null);

        private void Wrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoToSubCommand.Execute(DataContext);
        }
    }
}
