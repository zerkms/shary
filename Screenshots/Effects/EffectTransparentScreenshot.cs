using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using Screenshots.Events;

namespace Screenshots.Effects
{
    class EffectTransparentScreenshot
    {
        private Fullscreen _fs;
        private FindWindows.Window _window;
        private int _x, _y, _width, _height;
        private EffectBackground _effectBackground;
        private EffectBackground _effectBackgroundBlack;

        private BitmapSource _black;
        private BitmapSource _white;

        public event EventHandler<CapturedScreenshotEventArgs> Filtered;
        public event EventHandler<CapturedScreenshotEventArgs> BothFiltered;

        public void OnFiltered(CapturedScreenshotEventArgs e)
        {
            EventHandler<CapturedScreenshotEventArgs> handler = this.Filtered;
            if (handler != null)
                handler(this, e);
        }

        public void OnBothFiltered(CapturedScreenshotEventArgs e)
        {
            EventHandler<CapturedScreenshotEventArgs> handler = this.BothFiltered;
            if (handler != null)
                handler(this, e);
        }
    
        public EffectTransparentScreenshot(Fullscreen fs, FindWindows.Window window, int x, int y, int width, int height, EffectBackground background)
        {
            _fs = fs;
            _window = window;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _effectBackground = background;
        }

        public void Process(object sender, CapturedScreenshotEventArgs image)
        {
            EventHandler<CapturedScreenshotEventArgs> actionWhite = (s, e) =>
            {
                _white = GetScreenshot();
                _effectBackground.SetBackground(new SolidColorBrush(Colors.Black));

                _effectBackgroundBlack.Process(this, image);
            };

            EventHandler<CapturedScreenshotEventArgs> actionBlack = (s2, e2) =>
            {
                _black = GetScreenshot();

                OnBothFiltered(new CapturedScreenshotEventArgs(null));
            };

            _effectBackground.Filtered += actionWhite;
            _effectBackground.Process(this, image);

            _effectBackgroundBlack = new EffectBackground(_effectBackground.X, _effectBackground.Y, _effectBackground.Width, _effectBackground.Height);
            _effectBackgroundBlack.SetBackground(new SolidColorBrush(Colors.Black));
            _effectBackgroundBlack.Filtered += actionBlack;

            this.BothFiltered += ProcessTransparency;
        }

        private void ProcessTransparency(object sender, CapturedScreenshotEventArgs image)
        {
            var transparency = new TransparencyCalc(_white, _black);
            var transparent = transparency.GetTransparent();
            OnFiltered(new CapturedScreenshotEventArgs(transparent));
        }

        private BitmapSource GetScreenshot()
        {
            _window.BringToTop();

            Thread.Sleep(100);

            return _fs.TakeAScreenshot(_x, _y, _width, _height);
        }
    }
}
