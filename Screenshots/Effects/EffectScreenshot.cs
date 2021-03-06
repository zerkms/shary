﻿using System;
using System.Threading;
using System.Windows.Media.Imaging;
using Screenshots.Events;

namespace Screenshots.Effects
{
    class EffectScreenshot
    {
        private Fullscreen _fs;
        private FindWindows.Window _window;
        private int _x, _y, _width, _height;
        private EffectBackground _effectBackground;

        public event EventHandler<CapturedScreenshotEventArgs> Filtered;

        public void OnFiltered(CapturedScreenshotEventArgs e)
        {
            EventHandler<CapturedScreenshotEventArgs> handler = this.Filtered;
            if (handler != null)
                handler(this, e);
        }

        public EffectScreenshot(Fullscreen fs, FindWindows.Window window, int x, int y, int width, int height)
        {
            _fs = fs;
            _window = window;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public EffectScreenshot(Fullscreen fs, FindWindows.Window window, int x, int y, int width, int height, EffectBackground background)
            : this(fs, window, x, y, width, height)
        {
            _effectBackground = background;
        }

        public void Process(object sender, CapturedScreenshotEventArgs image)
        {
            EventHandler<CapturedScreenshotEventArgs> action = (s, e) =>
                {
                    _window.BringToTop();

                    Thread.Sleep(100);

                    var result = _fs.TakeAScreenshot(_x, _y, _width, _height);

                    OnFiltered(new CapturedScreenshotEventArgs(result));
                };

            if (_effectBackground != null)
            {
                _effectBackground.Filtered += action;
                _effectBackground.Process(this, image);
            }
            else
            {
                action(this, image);
            }
        }
    }
}
