using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace FindWindows
{
    public class WindowsCollection : List<Window>
    {
        public Window Find(Point point)
        {
            foreach (var w in this)
            {
                if (point.X >= w.Position.X &&
                point.Y >= w.Position.Y &&
                point.X <= w.BottomRight.X &&
                point.Y <= w.BottomRight.Y) return w;
            }

            return this.Where(i =>
                point.X >= i.Position.X &&
                point.Y >= i.Position.Y &&
                point.X <= i.BottomRight.X &&
                point.Y <= i.BottomRight.Y).FirstOrDefault();
        }
    }
}
