using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Screenshots.Events
{
    public class FilterEventArgs : EventArgs
    {
        public readonly BitmapSource Image;

        public FilterEventArgs(BitmapSource image)
        {
            Image = image;
        }
    }
}
