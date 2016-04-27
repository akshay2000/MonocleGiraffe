using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace MonocleGiraffe.Controls
{
    public class ColumnPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            double availableWidth = availableSize.Width;
            double finalWidth = 0;
            double finalHeight = 0;

            List<double> columnHeights = new List<double>();
            int count = 0;
            bool isFirstRow = true;
            foreach (var child in Children)
            {               
                child.Measure(availableSize);
                if (isFirstRow)
                {
                    double newWidth = finalWidth + child.DesiredSize.Width;
                    if (newWidth <= availableWidth)
                    {
                        columnHeights.Add(child.DesiredSize.Height);
                        finalWidth = newWidth;
                    }
                    else
                    {
                        columnHeights[0] += child.DesiredSize.Height;
                        count++;
                        isFirstRow = false;
                    }
                }
                else
                {
                    int noOfColumns = columnHeights.Count;
                    int columnIndex = count % noOfColumns;
                    columnHeights[columnIndex] += child.DesiredSize.Height;
                    count++;
                }
            }
            finalHeight = columnHeights.Count > 0 ? columnHeights.Max() : 0;
            Size finalSize = new Size(finalWidth, finalHeight);
            return finalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double availableWidth = finalSize.Width;
            double currentX = 0;
            List<double> columnHeights = new List<double>();
            int count = 0;
            bool isFirstRow = true;
            foreach (var child in Children)
            {
                if (isFirstRow)
                {
                    double newWidth = currentX + child.DesiredSize.Width;
                    if (newWidth <= availableWidth)
                    {
                        child.Arrange(new Rect(new Point(currentX, 0), child.DesiredSize));
                        columnHeights.Add(child.DesiredSize.Height);
                        currentX += child.DesiredSize.Width;
                    }
                    else
                    {
                        //new Row
                        currentX = 0;
                        double currentY = columnHeights.Count > 0 ? columnHeights[0] : 0;
                        child.Arrange(new Rect(new Point(currentX, currentY), child.DesiredSize));
                        count++;
                        currentX += child.DesiredSize.Width;
                        columnHeights[0] += child.DesiredSize.Height;
                        isFirstRow = false;
                    }
                }
                else
                {
                    int noOfColumns = columnHeights.Count;
                    int columnIndex = count % noOfColumns;
                    currentX = columnIndex == 0 ? 0 : currentX;
                    double currentY = columnHeights[columnIndex];
                    child.Arrange(new Rect(new Point(currentX, currentY), child.DesiredSize));
                    columnHeights[columnIndex] += child.DesiredSize.Height;
                    currentX += child.DesiredSize.Width;
                    count++;
                }
            }
            return finalSize;
        }       
    }
}
