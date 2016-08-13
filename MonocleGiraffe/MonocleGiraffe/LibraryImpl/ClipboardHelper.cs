using MonocleGiraffe.Portable.Interfaces;
using Windows.ApplicationModel.DataTransfer;

namespace MonocleGiraffe.LibraryImpl
{
    public class ClipboardHelper : IClipboardHelper
    {
        public void Clip(string text)
        {
            DataPackage package = new DataPackage();
            package.SetText(text);
            Clipboard.SetContent(package);
        }
    }
}
