using System;
using System.Runtime.InteropServices;


namespace SmarTIAtumProjeX.Utility
{
    internal class ConsoleWindowTweaks
    {
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int HWND_TOPMOST = -1;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x80000;

        public static void SetAlwaysOnTop()
        {
            IntPtr handle = GetConsoleWindow();

            // Сделать окно поверх всех
            SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);

            // Оставить только "Свернуть" и "Закрыть"
            //int style = GetWindowLong(handle, GWL_STYLE);
            //style &= ~(0x00CF0000); // убрать кнопки "развернуть" и "изменение размера"
            //style |= (WS_MINIMIZEBOX | WS_SYSMENU);
            //SetWindowLong(handle, GWL_STYLE, style);
        }
    }

}
