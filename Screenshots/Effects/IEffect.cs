using System.Windows.Media.Imaging;

namespace Screenshots.Effects
{
    interface IEffect
    {
        void Process(Chain chain, ref BitmapSource result);
    }
}
