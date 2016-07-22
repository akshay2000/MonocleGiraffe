using MonocleGiraffe.Models;
using XamarinImgur.Helpers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        int remainingChars = MAX_LENGTH;
        public int RemainingChars { get { return remainingChars; } set { Set(ref remainingChars, value); } }
        
        bool isCharCountVisible = default(bool);
        public bool IsCharCountVisible { get { return isCharCountVisible; } set { Set(ref isCharCountVisible, value); } }

        private async void CommentTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var t = sender as TextBox;
            int currentLength = t?.Text?.Length ?? 0;
            RemainingChars = MAX_LENGTH - currentLength;
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (RemainingChars > -1)
                {
                    await PostComment(t.Text);
                    ResetView(t);
                }
            }
        }

        private void ResetView(TextBox t)
        {
            t.Text = string.Empty;
            RemainingChars = MAX_LENGTH;
        }

        private async Task PostComment(string comment)
        {
            Comments.IsLoading = true;            
            long? parentId = parent == null ? null : (long?)parent.Id;
            bool isChildComment = parentId != null;

            var newComment = await (DataContext as IGalleryItem)?.AddComment(comment, parentId);
            if (newComment != null)
            {
                var cvm = new CommentViewModel(newComment) { IsUpVoted = true, Points = 1 };
                var newCI = new CommentItem(cvm);
                Comments.InsertComment(newCI, parentId);                
            }
            parent = null;
            Comments.IsLoading = false;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IsCharCountVisible = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsCharCountVisible = false;
        }

        CommentViewModel parent;
        private void TreeView_ReplyRequested(object sender, RoutedEventArgs e)
        {
            parent = ((sender as TreeViewItem)?.Content as CommentViewModel);
            CommentTextBox.Focus(FocusState.Keyboard);
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
