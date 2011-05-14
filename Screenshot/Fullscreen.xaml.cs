using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Screenshot
{
    /// <summary>
    /// Interaction logic for Fullscreen.xaml
    /// </summary>
    public partial class Fullscreen : Window
    {
        public Fullscreen()
        {
            InitializeComponent();

            ForegroundRect.Width = SystemParameters.PrimaryScreenWidth;
            ForegroundRect.Height = SystemParameters.PrimaryScreenHeight;
        }

        public void SetImage(BitmapSource img)
        {
            ScreenshotImage.Source = img;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScreenshotImage.Visibility = Visibility.Visible;
        }

        public void TakeScreenshot()
        {
            SetImage(TakeAScreenshot());
            Show();
            Activate();
        }

        private BitmapSource TakeAScreenshot()
        {
            BitmapSource filteredImage;

            using (var bitmap = new Bitmap(Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth), Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight)))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth), Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight)));
                }
                filteredImage = ToBitmapSource(bitmap);
            }

            return filteredImage;
        }

        public static BitmapSource ToBitmapSource(System.Drawing.Bitmap source)
        {
            var hBitmap = source.GetHbitmap();

            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                return null;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}
