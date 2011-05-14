using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace FindWindows
{
    public class WindowsCollection : List<Window>
    {
        public Window Find(Point point)
        {
            return this.Where(i =>
                point.X >= i.Position.X &&
                point.Y >= i.Position.Y &&
                point.X <= i.BottomRight.X &&
                point.Y <= i.BottomRight.Y).FirstOrDefault();
        }
    }
}
