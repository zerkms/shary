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

using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Screenshots
{
    /// <summary>
    /// Interaction logic for Fullscreen.xaml
    /// </summary>
    public partial class Fullscreen : Window
    {
        private FindWindows.WindowsCollection _windows;
        private FindWindows.Window _currentWindow;
        private int _currentWindowFrame;

        public Fullscreen()
        {
            InitializeComponent();

            ForegroundRect.Width = SystemParameters.PrimaryScreenWidth;
            ForegroundRect.Height = SystemParameters.PrimaryScreenHeight;
        }

        private void KeyHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
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
            CaptureWindows();
            Show();
            Activate();
        }

        private BitmapSource TakeAScreenshot()
        {
            BitmapSource filteredImage;

            using (var bitmap = new System.Drawing.Bitmap(Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth), Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight)))
            {
                using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth), Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight)));
                }
                filteredImage = ToBitmapSource(bitmap);
            }

            return filteredImage;
        }

        private void CaptureWindows()
        {
            var searcher = new FindWindows.Lookup();
            _windows = searcher.Find();
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

        private void Window_LostFocus(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (InvalidOperationException) { }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var window = _windows.Find(e.GetPosition(ScreenshotImage));

            if (window == null) return;

            if (_currentWindow != null && _currentWindow.Hwnd == window.Hwnd) return;

            _currentWindow = window;

            WindowFrame.Width = window.Size.Width;
            WindowFrame.Height = window.Size.Height;
            WindowFrame.Margin = new Thickness(window.Position.X, window.Position.Y, 0, 0);

            if (WindowFrame.Visibility != Visibility.Visible) WindowFrame.Visibility = Visibility.Visible;
        }
    }
}
