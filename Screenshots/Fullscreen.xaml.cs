using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Runtime.InteropServices;
using System.ComponentModel;
using SD = System.Drawing;
using Screenshots.Events;
using System.Threading;

using Screenshots.Effects;

namespace Screenshots
{
    /// <summary>
    /// Interaction logic for Fullscreen.xaml
    /// </summary>
    public partial class Fullscreen : Window
    {
        private FindWindows.WindowsCollection _windows;
        private FindWindows.Window _currentWindow;
        private FindWindows.Window _capturedWindow;
        private BitmapSource _imageBitmap;
        public event EventHandler<CapturedScreenshotEventArgs> Captured;
        private Background _backgroundWindow;

        private bool _handleShadow = true;

        public Fullscreen()
        {
            InitializeComponent();

            ForegroundRect.Width = SystemParameters.PrimaryScreenWidth;
            ForegroundRect.Height = SystemParameters.PrimaryScreenHeight;
        }

        private void KeyHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Space:
                case Key.Enter:
                    if (CroppedScreenshot.Visibility != Visibility.Hidden && _capturedWindow != null)
                    {
                        this.Hide();
                        this.CaptureScreenshot();
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
        }

        protected void OnCaptured(CapturedScreenshotEventArgs e)
        {
            EventHandler<CapturedScreenshotEventArgs> handler = this.Captured;
            if (handler != null)
                handler(this, e);
        }

        private void CaptureScreenshot()
        {
            int x = Convert.ToInt32(_capturedWindow.Position.X);
            int y = Convert.ToInt32(_capturedWindow.Position.Y);
            int width = Convert.ToInt32(_capturedWindow.Size.Width);
            int height = Convert.ToInt32(_capturedWindow.Size.Height);

            _backgroundWindow = new Screenshots.Background();
            _backgroundWindow.Visibility = Visibility.Visible;
            _backgroundWindow.SetDimensions(x, y, width, height);
            _backgroundWindow.Background = new SolidColorBrush(Colors.Black);

            _backgroundWindow.ContentRendered += (s, e) =>
            {
                
                _capturedWindow.BringToTop();

                Thread.Sleep(100);

                BitmapSource bitmap = TakeAScreenshot(x, y, width, height);
                OnCaptured(new CapturedScreenshotEventArgs(bitmap));
                        
                _backgroundWindow.Close();
                _backgroundWindow = null;
            };

            /*
            var filterChain = new Chain();
            filterChain.Register(new EffectBackground(x, y, width, height));
            filterChain.Register(new EffectScreenshot(this, _capturedWindow, x, y, width, height));
            var bitmap = filterChain.Run();
            OnCaptured(new CapturedScreenshotEventArgs(bitmap));*/
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
            SetImage(TakeFullScreenshot());
            CaptureWindows();
            Show();
            Activate();
        }

        private BitmapSource TakeFullScreenshot()
        {
            var result = TakeAScreenshot(0, 0, Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth), Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight));
            _imageBitmap = result;

            return result;
        }

        public BitmapSource TakeAScreenshot(int x, int y, int width, int height)
        {
            BitmapSource filteredImage;

            using (var image = new SD.Bitmap(width, height))
            {
                using (var graphics = SD.Graphics.FromImage(image))
                {
                    graphics.CopyFromScreen(x, y, 0, 0, new SD.Size(width, height));
                }
                filteredImage = ToBitmapSource(image);
            }

            return filteredImage;
        }

        private void CaptureWindows()
        {
            var searcher = new FindWindows.Lookup();
            _windows = searcher.Find();
        }

        public static BitmapSource ToBitmapSource(SD.Bitmap source)
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
            this.Close();
        }

        public new void Hide()
        {
            DetachLostfocusClose();
            base.Hide();
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            DetachLostfocusClose();
        }

        private void DetachLostfocusClose()
        {
            Deactivated -= Window_LostFocus;
            LostFocus -= Window_LostFocus;
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

        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            int x = Convert.ToInt32(_currentWindow.Position.X);
            int y = Convert.ToInt32(_currentWindow.Position.Y);

            var rect = new Int32Rect(Convert.ToInt32(_currentWindow.Position.X), Convert.ToInt32(_currentWindow.Position.Y), Convert.ToInt32(WindowFrame.Width), Convert.ToInt32(WindowFrame.Height));
            var croppedBitmap = new CroppedBitmap(_imageBitmap, rect);

            CroppedScreenshot.Source = croppedBitmap;
            CroppedScreenshot.Width = WindowFrame.Width;
            CroppedScreenshot.Height = WindowFrame.Height;

            CroppedScreenshot.Margin = new Thickness(_currentWindow.Position.X, _currentWindow.Position.Y, 0, 0);

            _capturedWindow = _currentWindow;

            if (CroppedScreenshot.Visibility != Visibility.Visible) CroppedScreenshot.Visibility = Visibility.Visible;
        }
    }
}
