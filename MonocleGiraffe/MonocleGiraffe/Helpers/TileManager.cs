using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using XamarinImgur.Models;

namespace MonocleGiraffe.Helpers
{
    public class TileManager
    {
        public async Task UpdateTile()
        {
            var t = await Portable.Helpers.Initializer.Gallery.GetSubreddditGallery("earthporn", XamarinImgur.APIWrappers.Enums.Sort.Time, 0);
            if (t.IsError)
                return;
            var images = t.Content;
            var tileImages = images
                .Select(ToThumbnail)
                .Select(i => new TileBasicImage() { Source = i })
                .Take(9);

            var photosContent = new TileBindingContentPhotos();

            foreach(var tileBasicImage in tileImages)
            {
                photosContent.Images.Add(tileBasicImage);
            }

            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = new TileBinding() { Content = photosContent },
                    TileWide = new TileBinding { Content = photosContent },
                    TileLarge = new TileBinding { Content = photosContent }
                }
            };

            TileNotification tileNotification = new TileNotification(content.GetXml());
            tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddDays(1);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }        

        private string ToThumbnail(Image image)
        {
            const string baseUrl = "http://i.imgur.com/";
            string thumbnailId = image.IsAlbum ? image.Cover : image.Id;
            return baseUrl + thumbnailId + "b.jpg";            
        }
    }
}
