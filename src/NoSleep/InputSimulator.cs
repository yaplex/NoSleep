using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Yaplex.NoSleep.User32;

namespace Yaplex.NoSleep
{
    public class InputSimulator
    {
        public static TimeSpan GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint) System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            User32Native.GetLastInputInfo(ref lastInPut);

            var idleTicks = ((uint) Environment.TickCount - lastInPut.dwTime);
            TimeSpan timespent = TimeSpan.FromMilliseconds(idleTicks);
            return timespent;
        }

        private static int mouseX = 1;
        private static int mouseY = 1;
        private static bool moved = false;

        public static void SimulateInput()
        {
            if (moved)
            {
                mouseX = -mouseX;
                mouseY = -mouseY;
            }

            var inputsList = new List<INPUT>();
            inputsList.Add(MoveMouse(mouseX, mouseY));
            inputsList.Add(AddKeyDown(VirtualKeyCode.F19));
            inputsList.Add(AddKeyUp(VirtualKeyCode.F19));
            
            var inputs = inputsList.ToArray();

            var successful = User32Native.SendInput((UInt32)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

            moved = !moved;
        }

        public static INPUT MoveMouse(int x, int y)
        {

            var movement = new INPUT {Type = (UInt32) InputType.Mouse};
            movement.Data.Mouse.Flags = (UInt32) (MouseFlag.Move | MouseFlag.Absolute);
            movement.Data.Mouse.X = x;
            movement.Data.Mouse.Y = y;

            return movement;
        }

        public static bool IsExtendedKey(VirtualKeyCode keyCode)
        {
            if (keyCode == VirtualKeyCode.MENU ||
                keyCode == VirtualKeyCode.LMENU ||
                keyCode == VirtualKeyCode.RMENU ||
                keyCode == VirtualKeyCode.CONTROL ||
                keyCode == VirtualKeyCode.RCONTROL ||
                keyCode == VirtualKeyCode.INSERT ||
                keyCode == VirtualKeyCode.DELETE ||
                keyCode == VirtualKeyCode.HOME ||
                keyCode == VirtualKeyCode.END ||
                keyCode == VirtualKeyCode.PRIOR ||
                keyCode == VirtualKeyCode.NEXT ||
                keyCode == VirtualKeyCode.RIGHT ||
                keyCode == VirtualKeyCode.UP ||
                keyCode == VirtualKeyCode.LEFT ||
                keyCode == VirtualKeyCode.DOWN ||
                keyCode == VirtualKeyCode.NUMLOCK ||
                keyCode == VirtualKeyCode.CANCEL ||
                keyCode == VirtualKeyCode.SNAPSHOT ||
                keyCode == VirtualKeyCode.DIVIDE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static INPUT AddKeyDown(VirtualKeyCode keyCode)
        {
            var down =
                new INPUT
                {
                    Type = (UInt32) InputType.Keyboard,
                    Data =
                    {
                        Keyboard =
                            new KEYBDINPUT
                            {
                                KeyCode = (UInt16) keyCode,
                                Scan = (UInt16) (User32Native.MapVirtualKey((UInt32) keyCode, 0) & 0xFFU),
                                Flags = IsExtendedKey(keyCode) ? (UInt32) KeyboardFlag.ExtendedKey : 0,
                                Time = 0,
                                ExtraInfo = IntPtr.Zero
                            }
                    }
                };

            return down;
        }

        public static INPUT AddKeyUp(VirtualKeyCode keyCode)
        {
            var up =
                new INPUT
                {
                    Type = (UInt32) InputType.Keyboard,
                    Data =
                    {
                        Keyboard =
                            new KEYBDINPUT
                            {
                                KeyCode = (UInt16) keyCode,
                                Scan = (UInt16) (User32Native.MapVirtualKey((UInt32) keyCode, 0) & 0xFFU),
                                Flags = (UInt32) (IsExtendedKey(keyCode)
                                    ? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey
                                    : KeyboardFlag.KeyUp),
                                Time = 0,
                                ExtraInfo = IntPtr.Zero
                            }
                    }
                };

            return up;
        }

    }
}