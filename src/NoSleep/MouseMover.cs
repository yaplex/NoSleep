using System;
using System.Runtime.InteropServices;

namespace NoSleep
{
    public class MouseMover
    {
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
 
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static TimeSpan GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            var idleTicks  = ((uint)Environment.TickCount - lastInPut.dwTime);
            TimeSpan timespent = TimeSpan.FromMilliseconds(idleTicks);
            return timespent;
        }

        private static int x = 1;
        private static int y = 1;
        private static bool moved = false;

        public static void MakeActive()
        {
            if (moved)
            {
                x = -x;
                y = -y;
            }

            MoveMouse(x, y);
            moved = !moved;
        }

        [DllImport("User32.dll", SetLastError = true)]
        private static extern uint SendInput(int nInputs, ref INPUT_TYPE pImputs, int cbSize);

        const int INPUT_MOUSE = 0;

        const int MOUSEEVENTF_MOVE = 0x1;
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint dwtime;
            public IntPtr dwExtraInfo;
        }

        public struct INPUT_TYPE
        {
            public int dwType;
            public MOUSEINPUT xi;
        }
 

    public static void MoveMouse(int x, int y)
        {
            INPUT_TYPE inputEvents = default(INPUT_TYPE);
            MOUSEINPUT xi = new MOUSEINPUT();

            xi.dx = x;
            xi.dy = y;
            xi.mouseData = 0;
            xi.dwtime = 0;
            xi.dwFlags = MOUSEEVENTF_MOVE;
            xi.dwExtraInfo = IntPtr.Zero;

            inputEvents.dwType = INPUT_MOUSE;
            inputEvents.xi = xi;

            SendInput(1, ref inputEvents, Marshal.SizeOf(inputEvents));
        }

    }
}