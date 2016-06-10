using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Mvvm;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MonocleGiraffe.Controls
{
    public sealed partial class CommentsView : UserControl, INotifyPropertyChanged
    {
        public CommentsView()
        {
            this.InitializeComponent();
        }

        const int MAX_LENGTH = 140;

        int _RemainingChars = MAX_LENGTH;
        public int RemainingChars { get { return _RemainingChars; } set { Set(ref _RemainingChars, value); } }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var t = sender as TextBox;
            int currentLength = t?.Text?.Length ?? 0;
            RemainingChars = MAX_LENGTH - currentLength;
            if(e.Key== Windows.System.VirtualKey.Enter)
            {
                if (RemainingChars > -1)
                {
                    PostComment(t.Text);
                    t.Text = string.Empty;
                }
            }
        }

        private void PostComment(string comment)
        {
            
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
