
using MonocleGiraffe.Helpers;
using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Models
{
    public enum GalleryItemType { Image, Album, Animation }

    public class GalleryItem : NotifyBase
    {
        private Image image;
            
        private const string baseUrl = "http://i.imgur.com/";

        public GalleryItem(Image image)
        {
            this.image = image;
            SetThumbnails();
        }

        public string Title
        {
            get
            {
                return image.Title;
            }
        }

        public string Link
        {
            get
            {
                return image.Link;
            }
        }

        public string Description
        {
            get
            {
                return image.Description;
            }
        }

        private string smallThumbnail;
        public string SmallThumbnail
        {
            get
            {
                return smallThumbnail;
            }
            set
            {
                if (smallThumbnail != value)
                {
                    smallThumbnail = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string bigThumbnail;
        public string BigThumbnail
        {
            get
            {
                return bigThumbnail;
            }
            set
            {
                if (bigThumbnail != value)
                {
                    bigThumbnail = value;
                    NotifyPropertyChanged();
                }
            }
        }
               
        private GalleryItemType GetImageType()
        {
            if (image.IsAlbum)
            {
                return GalleryItemType.Album;
            }
            else if (image.Animated)
            {
                return GalleryItemType.Animation;
            }
            else
            {
                return GalleryItemType.Image;
            }
        }        

        private ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
        public ObservableCollection<Comment> Comments
        {
            get
            {
                if (comments.Count == 0)
                    LoadComments(image.Id);
                return comments;
            }
        }

        private ObservableCollection<ImageItem> imageItems = new ObservableCollection<ImageItem>();
        public ObservableCollection<ImageItem> ImageItems
        {
            get
            {
                if (imageItems.Count == 0)
                    LoadImageItems();
                return imageItems;
            }
        }

        private async void LoadImageItems()
        {
            if (image.IsAlbum)
            {
                SharpImgur.Models.Album album = await SharpImgur.APIWrappers.Album.GetAlbum(image.Id);
                foreach (var image in album.Images)
                {
                    imageItems.Add(new ImageItem(image));
                }
            }
            else
            {
                imageItems.Add(new ImageItem(image));
            }
        }

        private async void SetThumbnails()
        {
            string thumbnailId;
            if (image.IsAlbum)
            {
                var album = await SharpImgur.APIWrappers.Album.GetAlbum(image.Id);
                thumbnailId = album.Cover;
            }
            else
            {
                thumbnailId = image.Id;
            }
            SmallThumbnail = baseUrl + thumbnailId + "s.jpg";
            BigThumbnail = baseUrl + thumbnailId + "b.jpg";
        }

        private async void LoadComments(string imageId)
        {
            var commentsList = await Gallery.GetComments(imageId);
            foreach (var comment in commentsList)
            {
                comments.Add(comment);
            }
        }        
    }
}
