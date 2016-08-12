using MonocleGiraffe.Portable.Interfaces;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace MonocleGiraffe.LibraryImpl
{
    public class SharingHelper : ISharingHelper
    {
        private const string COMMENT = "Comment";
        private const string IMAGE = "Image";

        private DataTransferManager dataTransferManager;

        private IGalleryItem itemToShare;
        private CommentViewModel commentToShare;

        private string shareType;

        public SharingHelper()
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DTManager_DataRequested;
        }

        private void DTManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
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

        public void ShareItem(IGalleryItem item)
        {
            itemToShare = item;
            shareType = IMAGE;
            DataTransferManager.ShowShareUI();
        }

        public void ShareComment(CommentViewModel comment)
        {
            commentToShare = comment;
            shareType = COMMENT;
            DataTransferManager.ShowShareUI();
        }
    }
}
