using System.Windows;
using System;

namespace FindWindows
{
    public class Window
    {
        private Point _position;
        private Point _bottomRight;
        private Size _size;
        private string _title;
        private IntPtr _hwnd;

        public Point Position { get { return _position; } }
        public Size Size { get { return _size; } }
        public string Title { get { return _title; } }
        public IntPtr Hwnd { get { return _hwnd;  } }

        public Point BottomRight { get { return _bottomRight; } }

        public Window(string title, Point position, Size size, IntPtr hwnd)
        {
            CorrectBoundaries(ref position, ref size);

            _title = title;
            _position = position;
            _size = size;
            _hwnd = hwnd;

            RecalculateBottomRight();
        }

        private void RecalculateBottomRight()
        {
            _bottomRight = new Point(_position.X + _size.Width, _position.Y + _size.Height);
        }

        private void CorrectBoundaries(ref Point position, ref Size size)
        {
            if (position.X < 0 && position.X > -1000)
            {
                size.Width += position.X;
                position.X = 0;
            }
            if (position.Y < 0 && position.Y > -1000)
            {
                size.Height += position.Y;
                position.Y = 0;
            }

            if (position.X + size.Width > SystemParameters.PrimaryScreenWidth)
            {
                size.Width = SystemParameters.PrimaryScreenWidth - position.X;
            }
        }

        public void BringToTop()
        {
            Winapi.BringWindowToTop(_hwnd);
        }

        public void CropWithAnotherWindow(Window window)
        {
            if (window.Position.Y > this.Position.Y && window.Position.Y < this.BottomRight.Y)
            {
                _size.Height = window.Position.Y - this.Position.Y;
                RecalculateBottomRight();
            }
        }
    }
}
