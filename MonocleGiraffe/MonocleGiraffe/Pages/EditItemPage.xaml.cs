using MonocleGiraffe.Models;
using MonocleGiraffe.Portable.Models;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditItemPage : Page
    {
        public EditItemPage()
        {
            this.InitializeComponent();
        }

        private async void Image_Drop(object sender, DragEventArgs e)
        {
            var def = e.GetDeferral();
            var data = await e.DataView.GetTextAsync();
        }

        private void Image_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {

        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private void GridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            //e.Data.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            var t = (IGalleryItem)e.Items[0];
            e.Data.SetText(t.Id);
        }
    }
}
