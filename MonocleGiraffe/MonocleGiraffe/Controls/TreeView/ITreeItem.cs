using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Controls.TreeView
{
    public interface ITreeItem
    {
        object Content { get; set; }
        List<ITreeItem> Children { get; set; }
    }    
}
