using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace MonocleGiraffe.Helpers
{
    public static class SharingHelper
    {
        private static DataTransferManager dataTransferManager;
        private static GalleryItem itemToShare;

        static SharingHelper()
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DTManager_DataRequested;
        }

        private static void DTManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.SetText(itemToShare.Title ?? "No title found");
            request.Data.Properties.Title = itemToShare.Title ?? "Sharing from Monocle Giraffe";
            request.Data.Properties.Description = "Sharing awesomeness";
            request.Data.SetWebLink(new Uri(itemToShare.Link));
        }

        public static void ShareItem(GalleryItem item)
        {
            itemToShare = item;
            DataTransferManager.ShowShareUI();
        }
    }
}
