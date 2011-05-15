using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Screenshots.Effects
{
    class Chain
    {
        private List<IEffect> _effects = new List<IEffect>();
        private int _pointer = 0;
        private BitmapSource _result;

        public void Register(IEffect effect)
        {
            _effects.Add(effect);
        }

        public void Next()
        {
            if (_pointer < _effects.Count)
            {
                _pointer++;
                _effects[_pointer - 1].Process(this, ref _result);
            }
        }

        public BitmapSource Run()
        {
            Next();

            return _result;
        }
    }
}
