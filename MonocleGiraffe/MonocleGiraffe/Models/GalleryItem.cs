
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
using Windows.UI.Xaml.Input;

namespace MonocleGiraffe.Models
{
    public enum GalleryItemType { Image, Album, Animation }

    public class GalleryItem : BindableBase, IGalleryItem
    {
        private Image image;        

        public GalleryItem(Image image)
        {
            this.image = image;
            Init();
        }

        private void Init()
        {
            SetVote();
            SetPoints();
            SetFavourite();
        }

        #region Available members

        public string Id
        {
            get
            {
                return image.Id;
            }
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

        public double BigThumbRatio
        {
            get
            {
                if (ItemType == GalleryItemType.Album || Height / (double)Width > 2.5)
                    return 1;
                else
                    return Height / (double)Width;
            }
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
            IsLoadingComments = true;
            var commentsList = await Gallery.GetComments(image.Id);
            Comments = commentsList?.Select(c => new CommentItem(new CommentViewModel(c))).ToList();
            IsLoadingComments = false;
        }

        bool isLoadingComments = false;
        public bool IsLoadingComments { get { return isLoadingComments; } set { Set(ref isLoadingComments, value); } }

        private const string DUMMY = "ms-appx:///Assets/DummyImage.png";

        private string smallThumbnail = DUMMY;
        public string SmallThumbnail
        {
            get
            {
                if (smallThumbnail == DUMMY)
                    LoadThumbnails();
                return smallThumbnail;
            }
            set { Set(ref smallThumbnail, value); }
        }
        
        private string thumbnail = DUMMY;
        public string Thumbnail
        {
            get
            {
                if (thumbnail == DUMMY)
                    LoadThumbnails();
                return thumbnail;
            }
            set { Set(ref thumbnail, value); }
        }

        private string bigThumbnail = DUMMY;
        public string BigThumbnail
        {
            get
            {
                if (bigThumbnail == DUMMY)
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

        private void SetVote()
        {
            switch (image.Vote)
            {
                case "up":
                    IsUpVoted = true;
                    break;
                case "down":
                    IsDownVoted = true;
                    break;
            }
        }

        private void SetPoints()
        {
            Points = image.Points ?? 0;
        }

        long points = default(long);
        public long Points { get { return points; } set { Set(ref points, value); } }

        bool isUpVoted = false;
        public bool IsUpVoted { get { return isUpVoted; } set { Set(ref isUpVoted, value); } }

        bool isDownVoted = false;
        public bool IsDownVoted { get { return isDownVoted; } set { Set(ref isDownVoted, value); } }
        
        DelegateCommand<string> voteCommand;
        public DelegateCommand<string> VoteCommand
           => voteCommand ?? (voteCommand = new DelegateCommand<string>(async (string parameter) =>
           {
               switch (parameter)
               {
                   case "up":
                       await UpVote();
                       break;
                   case "down":
                       await DownVote();
                       break;
               }
           }));

        public async Task UpVote()
        {
            string toVote;
            if (IsUpVoted)
            {
                toVote = "veto";
                Points--;
            }
            else
            {
                if (IsDownVoted)
                    Points++;
                toVote = "up";
                Points++;
            }
            IsDownVoted = false;
            IsUpVoted = !IsUpVoted;
            await Gallery.Vote(Id, toVote);
        }

        public async Task DownVote()
        {
            if (IsUpVoted)
            {
                IsUpVoted = false;
                Points--;
            }
            string toVote;
            if (IsDownVoted)
            {
                toVote = "veto";
                Points++;
            }
            else
            {
                if (IsUpVoted)
                    Points++;
                toVote = "down";
                Points--;
            }
            IsDownVoted = !IsDownVoted;
            await Gallery.Vote(Id, toVote);
        }

        private void SetFavourite()
        {
            IsFavourited = image.Favorite;
        }

        bool isFavourited = false;
        public bool IsFavourited { get { return isFavourited; } set { Set(ref isFavourited, value); } }

        DelegateCommand favourite;
        public DelegateCommand Favourite
           => favourite ?? (favourite = new DelegateCommand(async () =>
           {
               if (ItemType == GalleryItemType.Album)
                   IsFavourited = await Gallery.FavouriteAlbum(Id);
               else
                   IsFavourited = await Gallery.FavouriteImage(Id);
           }));

        DelegateCommand share;
        public DelegateCommand ShareCommand
           => share ?? (share = new DelegateCommand(() =>
           {
               SharingHelper.ShareItem(this);
           }));
    }
}
