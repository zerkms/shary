using System.Windows.Media.Imaging;

namespace Storages.Interfaces
{
    public interface IStorage
    {
        void Store(BitmapSource image);
    }
}
