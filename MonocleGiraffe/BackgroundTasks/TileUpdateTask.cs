using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace BackgroundTasks
{
    public sealed class TileUpdateTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var allTiles = await SecondaryTile.FindAllAsync();
            foreach (var tile in allTiles)
            {
                await UpdateRedditTile(tile.TileId);
            }
            deferral.Complete();
        }

        private SubredditGallery galleryWrapper;
        private SubredditGallery GalleryWrapper
        {
            get
            {
                return galleryWrapper = galleryWrapper ?? new SubredditGallery();
            }
        }

        private async Task UpdateRedditTile(string tileId)
        {
            var images = await GalleryWrapper.GetSubredditGallery(tileId);
            if (images == null || images.Count() == 0)
                return;
            TileContent content = GetTileContent(images);
            TileNotification tileNotification = new TileNotification(content.GetXml());
            SecondaryTile tile = new SecondaryTile(tileId);
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            updater.Clear();
            updater.Update(tileNotification);
        }

        private TileContent GetTileContent(IEnumerable<string> images)
        {
            var tileImages = images
                .Select(ToThumbnail)
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

        private string ToThumbnail(string thumbnailId)
        {
            return $"http://i.imgur.com/{thumbnailId}b.jpg";
        }
    }
}
