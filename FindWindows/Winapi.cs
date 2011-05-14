using System;
using System.Text;
using System.Runtime.InteropServices;

namespace FindWindows
{
    class Winapi
    {
        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

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
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int _GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr h, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool _EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

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
