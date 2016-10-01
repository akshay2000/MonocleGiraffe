using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.RemoteSystems;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonocleGiraffe.Controls
{
    public sealed partial class RemoteDevicesDialog : ContentDialog
    {
        RemoteDeviceHelper vm;
        public RemoteDevicesDialog()
        {
            this.InitializeComponent();
            vm = SimpleIoc.Default.GetInstance<RemoteDeviceHelper>();
            DataContext = vm;
            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Hide();
            await vm.LaunchOnSystem((RemoteSystem)e.ClickedItem);
        }
    }
}
