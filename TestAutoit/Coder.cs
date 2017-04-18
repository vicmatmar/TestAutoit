using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

using AutoIt;

namespace TestAutoit
{
    class Coder
    {

        public Coder()
        {
        }

        public delegate void ClickEventHandler();
        public event ClickEventHandler ClickEvent;

        uint _count = 0;
        public uint Count { get { return _count; } set { _count = value; } }

        IntPtr getWin(string win_desc)
        {
            string msg;

            if (AutoItX.WinExists(win_desc) != 1)
            {
                msg = string.Format("Unable to find window \"{0}\"", win_desc);
                throw new Exception(msg);
            }

            IntPtr hwnd = AutoItX.WinGetHandle(win_desc);
            if (AutoItX.WinExists(win_desc) != 1)
            {
                msg = string.Format("Unable to get handle for window \"{0}\"", win_desc);
                throw new Exception(msg);
            }

            return hwnd;
        }

        IntPtr activate_win(string win_desc)
        {
            IntPtr hwnd = getWin(win_desc);

            string msg;
            int n = 0;
            while (AutoItX.WinActive(hwnd) != 1)
            {
                AutoItX.WinActivate(hwnd);
                Thread.Sleep(250);
                if (n++ > 10)
                {
                    msg = string.Format("Unable to activate window \"{0}\"", win_desc);
                    throw new Exception(msg);
                }
            }

            return hwnd;
        }

        public void Code(uint count, CancellationToken cancel)
        {
            Count = count;

            if (cancel.IsCancellationRequested)
                return;

            string win_desc = "[REGEXPTITLE:.*Pixoto - Google Chrome.*]";
            IntPtr hwnd = activate_win(win_desc);

            Rectangle pos = AutoItX.WinGetPos(hwnd);
            Rectangle rec = AutoItX.WinGetClientSize(hwnd);

            int left = pos.X + rec.Width / 2 - 200;
            int right = pos.X + rec.Width / 2 + 200;

            Random random = new Random();
            int mod = random.Next(3, 5);

            int right_or_left = right;
            for (int i = 0; i < Count; i++)
            {
                if (cancel.IsCancellationRequested)
                    return;

                AutoItX.MouseMove(right_or_left, pos.Y + 500, 2);
                AutoItX.MouseClick();

                if (ClickEvent != null)
                    ClickEvent();

                Thread.Sleep(2500);

                if (i % mod == 0)
                {
                    mod = random.Next(3, 5);

                    if (right_or_left == right)
                        right_or_left = left;
                    else
                        right_or_left = right;
                }
            }

        }
    }
}
