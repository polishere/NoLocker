using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var wallpaperUri = "https://wallpapers.wallhaven.cc/wallpapers/full/wallhaven-429842.jpg";
            var currentWallpaperName = "current.jpg";
            var newWallpaperName = "new.jpg";
            var directFileName = "wallpaper.jpg";
            


            var currentWallpaperPath = Wallpaper.GetDesktopWallpaper();
            Bitmap currentWallpaperImage = (Bitmap)Image.FromFile(currentWallpaperPath);
            currentWallpaperImage.Save(currentWallpaperName);

            
            Wallpaper.DownloadRemoteImageFile(wallpaperUri, directFileName);
            Wallpaper.SetDesktopWallpaper(directFileName);
            Console.WriteLine("Tapeta zmieniona");
            Console.ReadKey();

            Display.TakeScreenShot();

            Thread.Sleep(2000);

            Display.ToggleDesktopIcons();

            Console.WriteLine("screenshot");
            Console.ReadKey();
            Display2.Rotate(1, Display2.Orientations.DEGREES_CW_180);

            Image image = Image.FromFile(Display.screenShotName);
            Image imageRotated = Wallpaper.RotateImage(image, 180);
            Bitmap newWallpaperImage = (Bitmap)imageRotated;
            newWallpaperImage.Save(newWallpaperName,ImageFormat.Jpeg);
            Wallpaper.SetDesktopWallpaper(newWallpaperName);
            Console.WriteLine("Tapeta zmieniona  z ikonami");

            Taskbar.Hide();


            Display.MinimizeAll();






            Console.WriteLine("Teraz odwrotnie");

            
            
            Console.ReadKey();
            Taskbar.Show();
            Display.UndoMinimize();

            Display2.Rotate(1, Display2.Orientations.DEGREES_CW_0);
            Display.ToggleDesktopIcons();

            Wallpaper.SetDesktopWallpaper(currentWallpaperName);

        
        }
    }
}
