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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Fullscreen _fullscreenWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowFullscreenWindow()
        {
            if (_fullscreenWindow == null || !_fullscreenWindow.IsLoaded) _fullscreenWindow = new Fullscreen();

            this.WindowState = System.Windows.WindowState.Minimized;

            var scr = TakeAScreenshot();
            _fullscreenWindow.SetImage(scr);

            _fullscreenWindow.Show();
            _fullscreenWindow.Activate();
        }

        private void TakeScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFullscreenWindow();
        }

        private BitmapSource TakeAScreenshot()
        {
            System.Threading.Thread.Sleep(300);

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
