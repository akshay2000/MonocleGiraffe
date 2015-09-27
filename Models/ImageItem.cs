using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Models
{
    public class ImageItem
    {
        //Definitely single image
        private Image image;

        public ImageItem(Image image)
        {
            this.image = image;
        }

        public int Width
        {
            get
            {
                return image.Width;
            }
        }

        public int Height
        {
            get
            {
                return image.Height;
            }
        }

        public string Link
        {
            get
            {
                return image.Link;
            }
        }

        public string Title
        {
            get
            {
                return image.Title;
            }
        }

        public string Description
        {
            get
            {
                return image.Description;
            }
        }
    }
}
