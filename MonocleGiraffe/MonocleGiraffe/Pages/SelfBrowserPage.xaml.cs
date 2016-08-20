using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelfBrowserPage : Page
    {
        public SelfBrowserPage()
        {
            this.InitializeComponent();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("Are you sure you want to delete this?", "Comfirm Delete");
            dialog.Commands.Add(new UICommand("Delete") { Id = 0 });
            dialog.Commands.Add(new UICommand("Cancel") { Id = 1 });
            var result = await dialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                (DataContext as BrowserPageViewModel)?.DeleteCommand?.Execute(null);
            }
        }
    }
}
