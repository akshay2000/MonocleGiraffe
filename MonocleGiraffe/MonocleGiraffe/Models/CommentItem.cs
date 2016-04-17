using MonocleGiraffe.Controls;
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
        public CommentItem(Comment comment)
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
                ret.Add(new CommentItem(c));
            return ret;
        }
    }
}
