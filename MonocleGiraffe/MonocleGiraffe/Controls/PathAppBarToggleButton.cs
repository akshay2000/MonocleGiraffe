using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace MonocleGiraffe.Controls
{
    public sealed class PathAppBarToggleButton : AppBarToggleButton
    {
        public PathAppBarToggleButton()
        {
            this.DefaultStyleKey = typeof(PathAppBarToggleButton);
        }



        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(PathAppBarToggleButton), null);



        public Brush CheckedFill
        {
            get { return (Brush)GetValue(CheckedFillProperty); }
            set { SetValue(CheckedFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckedFill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedFillProperty =
            DependencyProperty.Register("CheckedFill", typeof(Brush), typeof(PathAppBarToggleButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));


    }
}
