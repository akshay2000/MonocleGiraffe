
using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
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

        //public GalleryItemType Type
        //{
        //    get
        //    {
        //        switch (image.Type)
        //        {
        //            case 
        //        }
        //    }
        //}

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

        public static async Task<GalleryItem> New(Image image)
        {
            GalleryItem item = new GalleryItem(image);
            Debug.WriteLine(image.Type);
            await item.SetThumbnailId();
            return item;
        }
    }
}
