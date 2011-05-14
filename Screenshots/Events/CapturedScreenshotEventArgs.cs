using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Screenshots.Events
{
    public class CapturedScreenshotEventArgs: EventArgs
    {
        public readonly BitmapSource CapturedImage;

        public CapturedScreenshotEventArgs(BitmapSource capturedImage)
        {
            CapturedImage = capturedImage;
        }
    }
}
