using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace MonocleGiraffe.Helpers
{
    [Obsolete]
    public static class ClipboardHelper
    {
        public static void Clip(string text)
        {
            DataPackage package = new DataPackage();
            package.SetText(text);
            Clipboard.SetContent(package);
        }
    }
}
