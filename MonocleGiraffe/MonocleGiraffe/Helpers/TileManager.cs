using Microsoft.Toolkit.Uwp.Notifications;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using XamarinImgur.Models;

namespace MonocleGiraffe.Helpers
{
    public class TileManager
    {
        private TileContent GetTileContent(string tileId, IList<GalleryItem> images)
        {
            var tileImages = images
                .Select(i => i.Thumbnail)
                .Select(i => new TileBasicImage() { Source = i })
                .Take(9);

            var photosContent = new TileBindingContentPhotos();

            foreach (var tileBasicImage in tileImages)
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
            return content;
        }

        public void ScheduleRedditTileUpdate(string tileId, IList<GalleryItem> images)
        {
            TileContent content = GetTileContent(tileId, images);
            var dueTime = DateTime.Now.AddSeconds(5);
            var futureTile = new Windows.UI.Notifications.ScheduledTileNotification(content.GetXml(), dueTime);
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).AddToSchedule(futureTile);
        }

        public void UpdateRedditTile(string tileId, IList<GalleryItem> images)
        {
            TileContent content = GetTileContent(tileId, images);
            TileNotification tileNotification = new TileNotification(content.GetXml());
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Update(tileNotification);
        }

        private string ToThumbnail(Image image)
        {
            const string baseUrl = "http://i.imgur.com/";
            string thumbnailId = image.IsAlbum ? image.Cover : image.Id;
            return baseUrl + thumbnailId + "b.jpg";            
        }
    }
}
