using MonocleGiraffe.Controls;
using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MonocleGiraffe.Models
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
        }
                
        public long Id { get { return comment.Id; } }

        public string CommentText { get { return comment.CommentText; } }

        public string Author { get { return comment.Author; } }

        public long Ups { get { return comment.Ups; } }

        public IList<Comment> Children { get { return comment.Children; } }

        public async void UpVote()
        {
            await Comments.Vote(Id, "up");
        }

        public async void DownVote()
        {
            await Comments.Vote(Id, "down");
        }
    }
}
