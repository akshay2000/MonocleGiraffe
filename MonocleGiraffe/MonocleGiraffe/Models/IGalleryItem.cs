using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Models
{
    public interface IGalleryItem
    {
        string Title { get; }

        string Link { get; }

        string Mp4 { get; }

        string Description { get; }

        string UploaderName { get; }

        int? Ups { get; }

        int? CommentCount { get; }

        GalleryItemType ItemType { get; }

        int Width { get; }

        int Height { get; }

        bool IsAnimated { get; }

        double BigThumbRatio { get; }

        List<GalleryItem> AlbumImages { get; set; }

        List<CommentItem> Comments { get; set; }

        string SmallThumbnail { get; set; }

        string Thumbnail { get; set; }

        string BigThumbnail { get; set; }
    }
}
