using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class TabHeader : UserControl
    {
        public TabHeader()
        {
            this.InitializeComponent();
            //this.DataContext = this;            
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(TabHeader), null);
        public string Glyph
        {
            get { return GetValue(GlyphProperty) as string; }
            set { SetValue(GlyphProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(TabHeader), null);
        public string Label
        {
            get { return GetValue(LabelProperty) as string; }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register("IconPath", typeof(Geometry), typeof(TabHeader), null);
        public Geometry IconPath
        {
            get { return (Geometry)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }


        public static readonly DependencyProperty PayloadProperty = DependencyProperty.Register("Payload", typeof(TabHeaderContent), typeof(TabHeader), new PropertyMetadata(null, new PropertyChangedCallback(PayloadChanged)));
        public TabHeaderContent Payload
        {
            get { return (TabHeaderContent)GetValue(PayloadProperty); }
            set { SetValue(PayloadProperty, value); }
        }

        private static void PayloadChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TabHeader te = (TabHeader)obj;
            TabHeaderContent newValue = (TabHeaderContent)e.NewValue;
            te.IconPath = StringToPath(newValue.IconPath);
            te.Label = newValue.Label;
        }

        private static Geometry StringToPath(string pathData)
        {
            string xamlPath =
                "<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + pathData + "</Geometry>";

            return Windows.UI.Xaml.Markup.XamlReader.Load(xamlPath) as Geometry;
        }
    }

    public class TabHeaderContent
    {
        public String Label { get; set; }
        public String IconPath { get; set; }
    }
}
