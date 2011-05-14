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

        public static readonly bool IsDropShadowEnabled;

        public Point BottomRight { get { return _bottomRight; } }

        public Window(string title, Point position, Size size, IntPtr hwnd)
        {
            _title = title;
            _position = position;
            _size = size;
            _hwnd = hwnd;

            _bottomRight = new Point(_position.X + _size.Width, _position.Y + _size.Height);
        }

        static Window()
        {
            IsDropShadowEnabled = IsDropShadowCurrentlyEnabled();
        }

        public void BringToTop()
        {
            Winapi.BringWindowToTop(_hwnd);
        }

        public static void EnableDropShadow()
        {
            bool isEnabled = false;
            Winapi.SystemParametersInfo(0x1025, 0, ref isEnabled, 2);
        }

        public static void DisableDropShadow()
        {
            Winapi.SystemParametersInfo(0x1025, 0, false, 2);
        }

        public static bool IsDropShadowCurrentlyEnabled()
        {
            bool isEnabled = false;
            Winapi.SystemParametersInfo(0x1024, 0, ref isEnabled, 0);
            return isEnabled;
        }

    }
}
