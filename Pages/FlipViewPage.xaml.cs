using MonocleGiraffe.Helpers;
using MonocleGiraffe.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class FlipViewPage : Page
    {
        MainViewModel dataContext;
        bool isViewRendered = false;
        public FlipViewPage()
        {
            this.InitializeComponent();
            dataContext = StateHelper.ViewModel;
            DataContext = dataContext;
        }
          
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //MainFlipView.SelectedIndex = StateHelper.CurrentGalleryItemIndex;
        }

        private async void MainFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isViewRendered)
                ChangeView(GetScrollViewer(), dataContext.ZoomFactor, true);
        }

        private void ZoomedImageWrapper_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var imageScrollViewer = sender as ScrollViewer;
            if (imageScrollViewer.ZoomFactor > 0.99)
            {
                ChangeView(imageScrollViewer, dataContext.ZoomFactor);
            }
            else
            {
                ChangeView(imageScrollViewer, 1);
            }
        }        

        private void ScrollViewerWrapper_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var grid = sender as Grid;
            dataContext.ViewPortWidth = e.NewSize.Width;
            dataContext.ViewPortHeight = e.NewSize.Height;
        }

        #region Helpers
        private void ChangeView(ScrollViewer scrollViewer, float zoomFactor, bool disableAnimation = false)
        {
            if (scrollViewer == null)
            {
                Debug.WriteLine("---Scrollviewer was null!");
                return;
            }

            var period = TimeSpan.FromMilliseconds(10);
            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var Succes = scrollViewer.ChangeView(null, null, zoomFactor, disableAnimation);
                });
            }, period);
        }

        private List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }

        private ScrollViewer GetScrollViewer()
        {
            if (MainFlipView.SelectedItem == null)
                return null;
            var _Container = MainFlipView.ContainerFromItem(MainFlipView.SelectedItem);
            var _Children = AllChildren(_Container);
            var scrollViewer = _Children.OfType<ScrollViewer>().First();
            return scrollViewer;
        }

        #endregion

        private void ZoomedImageWrapper_Loaded(object sender, RoutedEventArgs e)
        {
            //await Task.Delay(1000);
            //ChangeView(GetScrollViewer(), dataContext.ZoomFactor);
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ChangeView(GetScrollViewer(), dataContext.ZoomFactor, true);
            isViewRendered = true;
        }
    }
}
