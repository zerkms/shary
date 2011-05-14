using System;
using System.Collections.Generic;
using System.Windows;

namespace FindWindows
{
    public class Lookup
    {
        private static WindowsCollection _windows;

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
                if (title.Length > 0)
                {
                    Winapi.RECT r;
                    Winapi.GetWindowRect(hWnd, out r);

                    _windows.Add(new Window(title, new Point(r.Left, r.Top), new Size(r.Right - r.Left, r.Bottom - r.Top), hWnd));
                }
            }

            return true;
        }
    }
}
