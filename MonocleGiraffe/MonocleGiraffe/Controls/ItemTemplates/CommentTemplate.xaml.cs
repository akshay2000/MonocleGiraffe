using MonocleGiraffe.Controls.Extensions;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Template10.Mvvm;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MonocleGiraffe.Controls.ItemTemplates
{
    public sealed partial class CommentTemplate : UserControl, INotifyPropertyChanged
    {
        public CommentTemplate()
        {
            this.InitializeComponent();
        }
        
        public event RoutedEventHandler CollapseRequested;

        private void OnCollapseRequested(object sender, RoutedEventArgs e)
        {
            CollapseRequested?.Invoke(sender, e);
        }

        private void ExpandToggle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsExpanded)
                OnCollapseRequested(this, e);
            else
                OnExpandRequested(this, e);
            IsExpanded = !IsExpanded;
        }

        public event RoutedEventHandler ExpandRequested;

        private void OnExpandRequested(object sender, RoutedEventArgs e)
        {
            ExpandRequested?.Invoke(sender, e);
        }

        private void ReplyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            OnReplyRequested(DataContext, e);
        }

        public event RoutedEventHandler ReplyRequested;

        private void OnReplyRequested(object sender, RoutedEventArgs e)
        {
            ReplyRequested?.Invoke(sender, e);
        }

        bool isExpanded = false;
        public bool IsExpanded { get { return isExpanded; } set { Set(ref isExpanded, value); } }
        
        private CommentViewModel Context
        {
            get
            {
                return (DataContext as TreeViewItem)?.Content as CommentViewModel;
            }
        }


        private void UpVote_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Context?.UpVote();
        }

        private void DownVote_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Context?.DownVote();
        }

        private void Share_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Context?.Share();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static readonly Regex regex = new Regex(@"([(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CommentTemplate), 
                new PropertyMetadata(string.Empty, (sender, e) => {
                    string text = e.NewValue as string;
                    var template = sender as CommentTemplate;
                    var textBl = template?.CommentTextBox;
                    if (textBl != null)
                    {
                        textBl.Inlines.Clear();
                        var splits = regex.Split(text).Where(s => !s.StartsWith("/")).ToList();
                        var matches = regex.Matches(text);
                        for (int i = 0; i < splits.Count; i++)
                        {
                            var split = splits[i];
                            if (i % 2 == 0)
                                textBl.Inlines.Add(new Run { Text = split });
                            else
                            {
                                Uri uri;
                                if (Uri.TryCreate(split, UriKind.Absolute, out uri))
                                {
                                    Hyperlink link = new Hyperlink();
                                    link.Click += template.Link_Click;
                                    link.Inlines.Add(new Run { Text = split });
                                    textBl.Inlines.Add(link);
                                }
                            }
                        }
                    }
                }));

        private async void Link_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var t = (sender.Inlines[0] as Run)?.Text;
            Uri uri = new Uri(t);
            await Launcher.LaunchUriAsync(uri);
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
