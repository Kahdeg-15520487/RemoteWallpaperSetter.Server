using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RosenHCMC.VPN.Native
{
    class SysCall
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        public enum ProcessDpiAwareness
        {
            ProcessDpiUnaware = 0,
            ProcessSystemDpiAware = 1,
            ProcessPerMonitorDpiAware = 2
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        internal static string LoadContent(string wallpaperFileName)
        {
            Image img = Image.FromFile(wallpaperFileName);
            Bitmap bitmap = new Bitmap(img);
            using (MemoryStream png = new MemoryStream())
            {
                bitmap.Save(png, ImageFormat.Png);
                byte[] byteImage = png.ToArray();
                return Convert.ToBase64String(byteImage);
            }
        }

        internal static string SaveContent(string wallpaperFileName, string wallpaperContent)
        {
            Bitmap bitmap = Base64StringToBitmap(wallpaperContent);
            if (File.Exists(wallpaperFileName))
            {
                File.Delete(wallpaperFileName);
            }
            using (FileStream fs = new FileStream(wallpaperFileName, FileMode.CreateNew))
            {
                bitmap.Save(fs, ImageFormat.Bmp);
            }
            return wallpaperFileName;
        }

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;

            byte[] byteBuffer = Convert.FromBase64String(base64String);
            using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
            {
                memoryStream.Position = 0;

                bmpReturn = (Bitmap)Image.FromStream(memoryStream);

                memoryStream.Close();

                return bmpReturn;
            }
        }

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(ProcessDpiAwareness value);

        public static bool EnableDpiAwareness()
        {
            try
            {
                if (Environment.OSVersion.Version.Major < 6)
                    return false;
                SetProcessDpiAwareness(ProcessDpiAwareness.ProcessPerMonitorDpiAware);
                return true;
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
                return false;
            }
        }

        public static bool SetSystemWallpaper(string wallpaperFilePath)
        {
            try
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperFilePath,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                return true;
            }
            catch
            {
                return false;
            }
        }


        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void Hide()
        {
            IntPtr hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_HIDE);
        }

        public static void Show()
        {
            IntPtr hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_SHOW);
        }
    }
}
