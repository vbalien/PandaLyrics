using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace PandaLyrics
{
    internal class Utils
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_TRANSPARENT = 0x20;
        static public bool GetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string appname = System.Windows.Application.ResourceAssembly.GetName().Name;
            return rk.GetValue(appname) != null;
        }
        static public void SetStartup(bool value)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string appname = System.Windows.Application.ResourceAssembly.GetName().Name;

            if (value)
                rk.SetValue(appname, System.Reflection.Assembly.GetExecutingAssembly().Location);
            else
                rk.DeleteValue(appname, false);
        }

        static public void SetClickThruAble(IntPtr windowHandle, uint defaultFlag, bool value)
        {
            if (value)
            {
                SetWindowLong(windowHandle, GWL_EXSTYLE,
                    (IntPtr)(defaultFlag | WS_EX_LAYERED | WS_EX_TRANSPARENT));
            }
            else
            {
                SetWindowLong(windowHandle, GWL_EXSTYLE, (IntPtr)defaultFlag);
            }
        }

        static public uint GetWindowFlag(IntPtr windowHandle)
        {
            return GetWindowLong(windowHandle, GWL_EXSTYLE);
        }
        static public string Escape(string value)
        {
            return value.Replace("&", "&amp;");
        }
    }
}
