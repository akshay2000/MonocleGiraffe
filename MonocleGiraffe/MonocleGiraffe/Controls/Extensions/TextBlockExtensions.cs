using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace MonocleGiraffe.Controls.Extensions
{
    public class TextBlockExtensions
    {
        public static readonly DependencyProperty TextProperty =
               DependencyProperty.RegisterAttached("Text",
                                             typeof(string),
                                             typeof(TextBlockExtensions),
                                             new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnChanged)));

        public static bool GetText(DependencyObject obj)
        {
            return (bool)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, bool value)
        {
            obj.SetValue(TextProperty, value);
        }

        private static readonly Regex regex = new Regex(@"([(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static void OnChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            TextBlock textBl = o as TextBlock;
            var text = args.NewValue as string;
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
                            link.Click += Link_Click;
                            link.Inlines.Add(new Run { Text = split });
                            textBl.Inlines.Add(link);
                        }
                    }
                }
            }
        }

        private static async void Link_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var t = (sender.Inlines[0] as Run)?.Text;
            Uri uri = new Uri(t);
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
