using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;

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
        static public bool HasExtension()
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string extensionPath = Path.Combine(localPath, @"spicetify\Extensions\pandaLyrics.js");
            return File.Exists(extensionPath);
        }

        static public bool HasSpicetify()
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string spicetifyPath = Path.Combine(localPath, @"spicetify");

            if (!Directory.Exists(spicetifyPath) || !Directory.Exists(Path.Combine(localPath, @"spicetify\Extensions")))
            {
                return false;
            }
            return true;
        }

        static public bool InstallExtension()
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string spicetifyPath = Path.Combine(localPath, @"spicetify");
            string extensionPath = Path.Combine(localPath, @"spicetify\Extensions\pandaLyrics.js");

            if (!Directory.Exists(Path.Combine(localPath, @"spicetify\Extensions")))
            {
                MessageBox.Show("Spicetify가 제대로 설치되어있지 않습니다.", "PandaLyrics", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            string downloadURL = "https://github.com/vbalien/spicetify-extension-pandaLyrics/releases/latest/download/pandaLyrics.js";
            WebClient http = new WebClient();
            http.DownloadFile(downloadURL, extensionPath);

            if (!File.Exists(extensionPath))
            {
                MessageBox.Show("다운로드에 실패하였습니다.");
                return false;
            }

            Process proc;
            try
            {

                proc = new Process();
                proc.StartInfo.FileName = @"spicetify.exe";
                proc.StartInfo.Arguments = "config extensions pandaLyrics.js";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("'spicetify config extensions pandaLyrics.js' 실패");
            }

            try
            {
                proc = new Process();
                proc.StartInfo.FileName = @"spicetify.exe";
                proc.StartInfo.Arguments = "apply";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("'spicetify apply' 실패");
            }

            return true;
        }
    }
}
