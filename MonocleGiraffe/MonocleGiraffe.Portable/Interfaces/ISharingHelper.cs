using MonocleGiraffe.Portable.Models;

namespace MonocleGiraffe.Portable.Interfaces
{
    public interface ISharingHelper
    {
        void ShareItem(IGalleryItem item);
        void ShareComment(CommentViewModel comment);
    }
}
