using XamarinImgur.APIWrappers;
using XamarinImgur.Models;
using System.Collections.Generic;
using MonocleGiraffe.Portable.Helpers;

namespace MonocleGiraffe.Portable.Models
{
    public class CommentItem : BindableBase, ITreeItem
    {
        public CommentItem(CommentViewModel comment)
        {
            Content = comment;
            Children = Translate(comment.Children);
        }

        object content = default(object);
        public object Content { get { return content; } set { Set(ref content, value); } }       

        List<ITreeItem> children = default(List<ITreeItem>);
        public List<ITreeItem> Children { get { return children; } set { Set(ref children, value); } }
        
        private List<ITreeItem> Translate(IList<Comment> children)
        {
            List<ITreeItem> ret = new List<ITreeItem>();
            if (children != null)
                foreach (var c in children)
                    ret.Add(new CommentItem(new CommentViewModel(c)));
            return ret;
        }
    }

    public class CommentViewModel : BindableBase
    {
        private Comment comment;
        public CommentViewModel(Comment comment)
        {
            this.comment = comment;
            SetPoints();
            SetVote();
        }

        private void SetVote()
        {
            switch(comment.Vote)
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
            Points = comment.Points;
        }

        public long Id { get { return comment.Id; } }

        public string CommentText { get { return comment.CommentText; } }

        public string Author { get { return comment.Author; } }

        public string Link { get { return $"http://imgur.com/gallery/{comment.ImageId}/comment/{Id}"; } }

        long points = default(long);
        public long Points { get { return points; } set { Set(ref points, value); } }

        bool isUpVoted = false;
        public bool IsUpVoted { get { return isUpVoted; } set { Set(ref isUpVoted, value); } }

        bool isDownVoted = false;
        public bool IsDownVoted { get { return isDownVoted; } set { Set(ref isDownVoted, value); } }

        public IList<Comment> Children { get { return comment.Children; } }

        public async void UpVote()
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
            await Comments.Vote(Id, toVote);
        }

        public async void DownVote()
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
            await Comments.Vote(Id, toVote);
        }

        public void Share()
        {
            Initializer.SharingHelper.ShareComment(this);
        }        
    }

    public interface ITreeItem
    {
        object Content { get; set; }
        List<ITreeItem> Children { get; set; }
    }
}
