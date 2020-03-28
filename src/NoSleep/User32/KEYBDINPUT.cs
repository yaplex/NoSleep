using System;

namespace Yaplex.NoSleep.User32
{
    public struct KEYBDINPUT
    {
        public UInt16 KeyCode;

        public UInt16 Scan;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }
}