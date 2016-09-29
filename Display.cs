using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleApplication1
{

    static class Display
    {

        public static string screenShotName = "screenshot.jpg";

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);
        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_COMMAND = 0x111;

        public static void ToggleDesktopIcons()
        {
            var toggleDesktopCommand = new IntPtr(0x7402);
            IntPtr hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
            SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler)
                : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }

        }

        public static bool areVisibleIcons()
        {
            IntPtr hWnd = GetWindow(GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD), GetWindow_Cmd.GW_CHILD);
            WINDOWINFO info = new WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            GetWindowInfo(hWnd, ref info);
            return (info.dwStyle & 0x10000000) == 0x10000000;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean EnumDisplaySettings(
            [param: MarshalAs(UnmanagedType.LPTStr)]
            string lpszDeviceName,
            [param: MarshalAs(UnmanagedType.U4)]
            int iModeNum,
            [In, Out]
            ref DEVMODE lpDevMode);


        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;

        
        public static void MinimizeAll()
        {

            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);

        }

        public static void TakeScreenShot()
        {
            MinimizeAll();
            // Create an instance of the shell class

            Thread.Sleep(2000);

            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(screenShotName, ImageFormat.Jpeg);

            UndoMinimize();
        }

        public static void UndoMinimize()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
        }


    }
}
