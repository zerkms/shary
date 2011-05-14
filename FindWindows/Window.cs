using System.Drawing;

namespace FindWindows
{
    public class Window
    {
        private Point _position;
        private Size _size;
        private string _title;

        public Point Position { get { return _position; } }
        public Size Size { get { return _size; } }
        public string Title { get { return _title; } }

        public Window(string title, Point position, Size size)
        {
            _title = title;
            _position = position;
            _size = size;
        }
    }
}
