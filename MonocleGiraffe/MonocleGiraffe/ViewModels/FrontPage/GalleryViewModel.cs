using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using XamarinImgur.APIWrappers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.ApplicationModel;
using System.Threading;
using static XamarinImgur.APIWrappers.Enums;
using XamarinImgur.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MonocleGiraffe.LibraryImpl;
using MonocleGiraffe.Portable.Models;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;
using MonocleGiraffe.Portable.ViewModels.Front;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class GalleryViewModel : Portable.ViewModels.Front.GalleryViewModel
    {
        public GalleryViewModel(GalaSoft.MvvmLight.Views.INavigationService nav) : base(nav, DesignMode.DesignModeEnabled)
        { }

        public void ImageTapped(object sender, object parameter)
        {             
            var clickedItem = parameter as GalleryItem;
            ImageTapped(clickedItem);
            //TODO Remove this after refactor is done
            BootStrapper.Current.SessionState["GalleryInfo"] = galleryMetaInfo;
        }
        
        public void TopicTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as Topic;
            TopicTapped(clickedItem);
        }

        protected override Portable.ViewModels.Front.IncrementalGallery CreateGallery(Section section, Sort sort)
        {
            return new IncrementalGallery(section, sort);
        }

        protected override Portable.ViewModels.Front.IncrementalGallery CreateGallery(Sort sort, int topicId)
        {
            return new IncrementalGallery(sort, topicId);
        }
    }

    public class IncrementalGallery : Portable.ViewModels.Front.IncrementalGallery, ISupportIncrementalLoading
    {
        public IncrementalGallery(Sort sort, int topicId) : base(sort, topicId)
        { }

        public IncrementalGallery(Section section, Sort sort) : base(section, sort)
        { }

        public bool HasMoreItems
        {
            get
            {
                return HasMore;
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            var ret = await LoadMoreAsync(c, count);
            return new LoadMoreItemsResult { Count = count };
        }
    }    
}
