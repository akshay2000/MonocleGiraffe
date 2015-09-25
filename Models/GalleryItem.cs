
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

    public class GalleryItem
    {
        private Image image;
        private string thumbnailId;
            
        private const string baseUrl = "http://i.imgur.com/";

        private GalleryItem(Image image)
        {
            this.image = image;
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

        public string SmallThumbnail
        {
            get
            {
                return baseUrl + thumbnailId + "s.jpg";
            }
        }

        public string BigThumbnail
        {
            get
            {
                return baseUrl + thumbnailId + "b.jpg";
            }
        }

        public GalleryItemType Type
        {
            get
            {
                return GetImageType();
            }
        }

        public int Width
        {
            get
            {
                return image.Width;
            }
        }

        public int Height
        {
            get
            {
                return image.Height;
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

        private async Task SetThumbnailId()
        {
            if (image.IsAlbum)
            {
                var album = await SharpImgur.APIWrappers.Album.GetAlbum(image.Id);
                thumbnailId = album.Cover;
            }
            else
            {
                thumbnailId = image.Id;
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

        private async void LoadComments(string imageId)
        {
            if (imageId == null)
            {
                return;
            }
            var commentsList = await Gallery.GetComments(imageId);
            foreach (var comment in commentsList)
            {
                comments.Add(comment);
            }
        }

        public static async Task<GalleryItem> New(Image image)
        {
            GalleryItem item = new GalleryItem(image);
            await item.SetThumbnailId();
            return item;
        }
    }
}
