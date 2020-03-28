using System;
using System.Runtime.InteropServices;

namespace Yaplex.NoSleep.User32
{
    public static class User32Native
    {
        [DllImport("User32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);

        [DllImport("user32.dll")]
        public static extern UInt32 MapVirtualKey(UInt32 uCode, UInt32 uMapType);
    }
}