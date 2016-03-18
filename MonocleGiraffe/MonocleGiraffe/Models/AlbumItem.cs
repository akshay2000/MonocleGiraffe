using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.Models
{
    public class AlbumItem : BindableBase
    {
        private Album album;
        public AlbumItem(Album album)
        {
            this.album = album;
            SetThumbnails(); 
        }

        public string Title
        {
            get
            {
                return album.Title;
            }
        }

        public string Link
        {
            get
            {
                return album.Link;
            }
        }

        public string Description
        {
            get
            {
                return album.Description;
            }
        }

        public string UploaderName
        {
            get
            {
                return album.AccountUrl;
            }
        }

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

        private void SetThumbnails()
        {
            const string baseUrl = "http://i.imgur.com/";
            string thumbnailId = album.Cover;            
            SmallThumbnail = baseUrl + thumbnailId + "s.jpg";
            Thumbnail = baseUrl + thumbnailId + "b.jpg";
            BigThumbnail = baseUrl + thumbnailId + "l.jpg";
        }
    }
}
