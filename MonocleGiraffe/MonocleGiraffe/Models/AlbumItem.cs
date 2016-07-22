using XamarinImgur.APIWrappers;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.Models
{
    public class AlbumItem : BindableBase, IGalleryItem
    {
        private Album album;
        public AlbumItem(Album album)
        {
            this.album = album;
            SetThumbnails(); 
        }

        public string Id
        {
            get { return album.Id; }
        }

        public string Title
        {
            get { return album.Title; }
            set { album.Title = value; }
        }

        public long Views { get { return album.Views; } }

        public string Privacy { get { return album.Privacy; } }

        public string Link { get { return album.Link; } }

        public string Description
        {
            get { return album.Description; }
            set { album.Description = value; }
        }

        public string UploaderName { get { return album.AccountUrl; } }

        private string smallThumbnail;
        public string SmallThumbnail
        {
            get { return smallThumbnail; }
            set { Set(ref smallThumbnail, value); }            
        }

        private string thumbnail;
        public string Thumbnail
        {
            get { return thumbnail; }
            set { Set(ref thumbnail, value); }
        }

        private string bigThumbnail;
        public string BigThumbnail
        {
            get { return bigThumbnail; }
            set { Set(ref bigThumbnail, value); }
        }

        public string Mp4 { get { return ""; } }

        public int? Ups { get { return 0; } }

        public int CommentCount { get { return 0; } }

        public GalleryItemType ItemType { get { return GalleryItemType.Album; } }

        public int Width { get { return 0; } }

        public int Height { get { return 0; } }

        public bool IsAnimated { get { return false; } }

        public double BigThumbRatio { get { return 1; } }

        public string Cover { get { return album.Cover; } }
        
        private List<GalleryItem> albumImages;
        public List<GalleryItem> AlbumImages
        {
            get
            {
                if (albumImages == null)
                    LoadAlbumImages();
                return albumImages;
            }
            set
            {
                Set(ref albumImages, value);
            }
        }

        private async Task LoadAlbumImages()
        {
            var images = album.Images ?? await Albums.GetImages(album.Id);
            AlbumImages = images.Select(i => new GalleryItem(i)).ToList();
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

            set
            {
                Set(ref comments, value);
            }
        }

        private void LoadComments()
        {
            //TODO
            Comments = new List<CommentItem>();
        }

        private void SetThumbnails()
        {
            const string baseUrl = "http://i.imgur.com/";
            string thumbnailId = album.Cover;            
            SmallThumbnail = baseUrl + thumbnailId + "s.jpg";
            Thumbnail = baseUrl + thumbnailId + "b.jpg";
            BigThumbnail = baseUrl + thumbnailId + "l.jpg";
        }

        public async Task<Comment> AddComment(string comment, long? parentId = null)
        {
            var response = await XamarinImgur.APIWrappers.Comments.CreateComment(comment, Id, parentId);
            if (!response.IsError)
            {
                var c = new Comment { Id = response.Content.Value, ImageId = Id, CommentText = comment, Author = await SecretsHelper.GetUserName(), Children = new List<Comment>() };
                return c;
            }
            return null;
        }
    }
}
