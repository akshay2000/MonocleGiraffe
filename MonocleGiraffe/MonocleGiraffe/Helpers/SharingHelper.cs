using MonocleGiraffe.Models;
using MonocleGiraffe.Portable.Models;
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
        private const string COMMENT = "Comment";
        private const string IMAGE = "Image";

        private static DataTransferManager dataTransferManager;

        private static IGalleryItem itemToShare;
        private static CommentViewModel commentToShare;

        private static string shareType;

        static SharingHelper()
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DTManager_DataRequested;
        }

        private static void DTManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            switch (shareType)
            {
                case IMAGE:
                    request.Data.SetText(itemToShare.Title ?? "No title found");
                    request.Data.Properties.Title = itemToShare.Title ?? "Sharing from Monocle Giraffe";
                    request.Data.Properties.Description = "Sharing awesomeness";
                    request.Data.SetWebLink(new Uri(itemToShare.Link));
                    break;
                case COMMENT:
                    request.Data.SetText(commentToShare.CommentText);
                    request.Data.Properties.Title = commentToShare.CommentText;
                    request.Data.SetWebLink(new Uri(commentToShare.Link));
                    break;
            }
        }

        public static void ShareItem(IGalleryItem item)
        {
            itemToShare = item;
            shareType = IMAGE;
            DataTransferManager.ShowShareUI();
        }

        public static void ShareComment(CommentViewModel comment)
        {
            commentToShare = comment;
            shareType = COMMENT;
            DataTransferManager.ShowShareUI();
        }
    }
}
