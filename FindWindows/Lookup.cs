using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace FindWindows
{
    public class Lookup
    {
        private static List<Window> _windows;

        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        public List<Window> Find()
        {
            _windows = new List<Window>();

            EnumDelegate enumfunc = new EnumDelegate(EnumWindowsProc);
            IntPtr hDesktop = IntPtr.Zero; // current desktop
            bool success = _EnumDesktopWindows(hDesktop, enumfunc, IntPtr.Zero);

            return _windows;
        }

        private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                var title = GetWindowText(hWnd);
                if (title.Length > 0)
                {
                    RECT r;
                    GetWindowRect(hWnd, out r);

                    _windows.Add(new Window(title, new Point(r.Left, r.Top), new Size(r.Right - r.Left, r.Bottom - r.Top)));
                }
            }

            return true;
        }

        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder title = new StringBuilder(255);
            int titleLength = _GetWindowText(hWnd, title, title.Capacity + 1);
            title.Length = titleLength;

            return title.ToString();
        }

        public static string GetWindowClassName(IntPtr hWnd)
        {
            StringBuilder title = new StringBuilder(255);
            GetClassName(hWnd, title, title.Capacity + 1);


            return title.ToString();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int _GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr h, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool _EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
    }
}
