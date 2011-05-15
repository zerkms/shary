using System;
using System.Collections.Generic;
using System.Windows;

namespace FindWindows
{
    public class Lookup
    {
        private static WindowsCollection _windows;
        private static Window _taskbarWindow;

        public WindowsCollection Find()
        {
            _windows = new WindowsCollection();

            var enumfunc = new Winapi.EnumDelegate(EnumWindowsProc);
            IntPtr hDesktop = IntPtr.Zero; // current desktop
            bool success = Winapi._EnumDesktopWindows(hDesktop, enumfunc, IntPtr.Zero);

            return _windows;
        }

        private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            if (Winapi.IsWindowVisible(hWnd))
            {
                var title = Winapi.GetWindowText(hWnd);
                
                Winapi.RECT r;
                Winapi.GetWindowRect(hWnd, out r);

                var window = new Window(title, new Point(r.Left, r.Top), new Size(r.Right - r.Left, r.Bottom - r.Top), hWnd);

                _windows.Add(window);

                if (_taskbarWindow == null)
                {
                    var classname = Winapi.GetWindowClassName(hWnd);
                    if (classname == "Shell_TrayWnd")
                    {
                        _taskbarWindow = window;
                    }
                }
                else
                {
                    window.CropWithAnotherWindow(_taskbarWindow);
                }
            }

            return true;
        }
    }
}
