
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
using Template10.Mvvm;

namespace MonocleGiraffe.Models
{
    public enum GalleryItemType { Image, Album, Animation }

    public class GalleryItem : BindableBase
    {
        private Image image;        

        public GalleryItem(Image image)
        {
            this.image = image;
        }

        #region Available members

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

        public string Mp4
        {
            get
            {
                return image.Mp4;
            }
        }

        public string Description
        {
            get
            {
                return image.Description;
            }
        }

        public string UploaderName
        {
            get
            {
                return image.AccountUrl;
            }
        }       

        public int? Ups
        {
            get { return image.Ups; }
        }

        public int? CommentCount
        {
            get { return image.CommentCount; }
        }
        
        public GalleryItemType ItemType
        {
            get { return GetImageType(); }
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

        public int Width
        {
            get { return image.Width; }
        }

        public int Height
        {
            get { return image.Height; }
        }

        public bool IsAnimated
        {
            get { return image.Animated; }
        }

        #endregion

        #region Lazy members

        private const string baseUrl = "http://i.imgur.com/";

        private List<GalleryItem> albumImages;
        public List<GalleryItem> AlbumImages
        {
            get
            {
                if (albumImages == null)
                    LoadAlbumImages();
                return albumImages;
            }
            set { Set(ref albumImages, value); }
        }

        private async Task LoadAlbumImages()
        {
            if (image.IsAlbum)
            {
                var album = await GetAlbum();
                AlbumImages = album?.Images?.Select(i => new GalleryItem(i)).ToList();
            }
        }

        private List<CommentItem> comments;
        public List<CommentItem> Comments
        {
            get
            {
                if (comments == null)
                    LoadComments();
                return comments;
            }
            set { Set(ref comments, value); }
        }

        private async Task LoadComments()
        {
            var commentsList = await Gallery.GetComments(image.Id);
            Comments = commentsList?.Select(c => new CommentItem(c)).ToList();
        }

        private string smallThumbnail;
        public string SmallThumbnail
        {
            get
            {
                if (smallThumbnail == default(string))
                    LoadThumbnails();
                return smallThumbnail;
            }
            set { Set(ref smallThumbnail, value); }
        }
        
        private string thumbnail;
        public string Thumbnail
        {
            get
            {
                if (thumbnail == default(string))
                    LoadThumbnails();
                return thumbnail;
            }
            set { Set(ref thumbnail, value); }
        }

        private string bigThumbnail;
        public string BigThumbnail
        {
            get
            {
                if (bigThumbnail == default(string))
                    LoadThumbnails();
                return bigThumbnail;
            }
            set { Set(ref bigThumbnail, value); }
        }

        private async Task LoadThumbnails()
        {
            string thumbnailId;
            if (image.IsAlbum)
            {
                var album = await GetAlbum();
                thumbnailId = album?.Cover;
            }
            else
            {
                thumbnailId = image.Id;
            }
            SmallThumbnail = baseUrl + thumbnailId + "s.jpg";
            Thumbnail = baseUrl + thumbnailId + "b.jpg";
            if (ItemType == GalleryItemType.Album || Height / (double)Width > 2.5)
                BigThumbnail = baseUrl + thumbnailId + "b.jpg";
            else
                BigThumbnail = baseUrl + thumbnailId + "l.jpg";
        }

        Album album = null;
        private async Task<Album> GetAlbum()
        {
            if (album == null)
                album = await Albums.GetAlbum(image.Id);
            return album;
        }

        #endregion
    }
}
