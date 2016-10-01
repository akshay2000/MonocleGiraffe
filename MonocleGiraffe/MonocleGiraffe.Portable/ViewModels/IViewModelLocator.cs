using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.ViewModels
{
    public interface IViewModelLocator
    {
        TransfersViewModel TransfersViewModel { get; } 

        BrowserViewModel BrowserViewModel { get; }
    }
}
