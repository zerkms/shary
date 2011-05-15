using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Screenshots.Effects
{
    class EffectBackground : IEffect
    {
        private Background _background;
        private int _x, _y, _width, _height;

        public EffectBackground(int x, int y, int width, int height)
        {
            _background = new Background();
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void Process(Chain chain, ref BitmapSource result)
        {
            _background.Visibility = Visibility.Visible;
            _background.SetDimensions(_x, _y, _width, _height);

            System.Threading.Thread.Sleep(3000);
            
            chain.Next();
            _background.Close();
            _background = null;
        }
    }
}
