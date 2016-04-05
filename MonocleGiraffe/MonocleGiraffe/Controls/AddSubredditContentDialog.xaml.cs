using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
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
    public sealed partial class AddSubredditContentDialog : ContentDialog
    {
        public AddSubredditContentDialog()
        {
            this.InitializeComponent();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text.Length > 0 && FriendlyNameTextBox.Text.Length > 0)
            {
                IsPrimaryButtonEnabled = true;
            }
            else
            {
                IsPrimaryButtonEnabled = false;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //string name = NameTextBox.Text;
            //string friendlyName = FriendlyNameTextBox.Text.Length > 0 ? FriendlyNameTextBox.Text : name;
            //StateHelper.ViewModel.AddSubreddit(new Subreddit(name, friendlyName));
        }
    }
}
