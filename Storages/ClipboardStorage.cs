using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Storages
{
    public class ClipboardStorage : Interfaces.IStorage
    {
        public void Store(BitmapSource image)
        {
            Clipboard.SetImage(image);
        }
    }
}
