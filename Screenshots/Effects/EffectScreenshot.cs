using System.Threading;
using System.Windows.Media.Imaging;

namespace Screenshots.Effects
{
    class EffectScreenshot : IEffect
    {
        private Fullscreen _fs;
        private FindWindows.Window _window;
        private int _x, _y, _width, _height;

        public EffectScreenshot(Fullscreen fs, FindWindows.Window window, int x, int y, int width, int height)
        {
            _fs = fs;
            _window = window;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void Process(Chain chain, ref BitmapSource result)
        {
            _window.BringToTop();

            Thread.Sleep(100);

            result = _fs.TakeAScreenshot(_x, _y, _width, _height);
        }
    }
}
