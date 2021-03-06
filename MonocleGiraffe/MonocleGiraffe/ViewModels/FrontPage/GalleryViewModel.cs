﻿using MonocleGiraffe.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using XamarinImgur.Models;
using static XamarinImgur.APIWrappers.Enums;

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

        public static IncrementalGallery fromJson(string s)
        {
            JObject o = JObject.Parse(s);
            bool isGallery = (bool)o["isGallery"];
            Sort sort = JsonConvert.DeserializeObject<Sort>((string)o["sort"]);
            if (isGallery)
            {
                Section section = JsonConvert.DeserializeObject<Section>((string)o["section"]);
                return new IncrementalGallery(section, sort);
            }
            else
            {
                return new IncrementalGallery(sort, (int)o["topicId"]);
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
