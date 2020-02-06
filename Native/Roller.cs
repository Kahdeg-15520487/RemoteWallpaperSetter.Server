using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Microsoft.Win32;

namespace RosenHCMC.VPN.Native
{
    public class Roller
    {
        public enum Style
        {
            Fill,
            Fit,
            Span,
            Stretch,
            Tile,
            Center
        }

        public static bool PaintWall(string wallFilePath, Style style)
        {
            string primaryFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string destWallFilePath = Path.Combine(primaryFolder + @"\Microsoft\Windows\Themes", "rollerWallpaper.bmp");

            Image img = null;
            Bitmap imgTemp = null;
            try
            {
                img = Image.FromFile(Path.GetFullPath(wallFilePath));
                imgTemp = new Bitmap(img);
                imgTemp.Save(destWallFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
                Console.WriteLine("Wallpaper saved to primary path: " + destWallFilePath);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
                try
                {
                    string secondaryFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    destWallFilePath = Path.Combine(secondaryFolder, "rollerWallpaper.bmp");
                    imgTemp.Save(destWallFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
                    Console.WriteLine("Wallpaper saved to secondary path: " + destWallFilePath);
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2);
                    return false;
                }
            }

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                if (style == Style.Fill)
                {
                    key.SetValue(@"WallpaperStyle", 10.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Fit)
                {
                    key.SetValue(@"WallpaperStyle", 6.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Span) // Windows 8 or newer only!
                {
                    key.SetValue(@"WallpaperStyle", 22.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Stretch)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Tile)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }
                if (style == Style.Center)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                SysCall.SetSystemWallpaper(destWallFilePath);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
