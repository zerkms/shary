using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Screenshots.Events;

namespace Screenshots.Effects
{
    class EffectBackground
    {
        private Background _background;
        private int _x, _y, _width, _height;

        public event EventHandler<CapturedScreenshotEventArgs> Filtered;

        public void OnFiltered(CapturedScreenshotEventArgs e)
        {
            EventHandler<CapturedScreenshotEventArgs> handler = this.Filtered;
            if (handler != null)
                handler(this, e);
        }

        public EffectBackground(int x, int y, int width, int height)
        {
            _background = new Background();
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void SetBackground(Brush brush)
        {
            _background.Background = brush;
        }

        public void Process(object sender, CapturedScreenshotEventArgs image)
        {
            _background.Visibility = Visibility.Visible;
            _background.SetDimensions(_x, _y, _width, _height);

            _background.ContentRendered += (s, e) =>
                {
                    //System.Threading.Thread.Sleep(1000);

                    OnFiltered(null);

                    _background.Close();
                    _background = null;
                };
        }
    }
}
