using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows;
using System.Diagnostics;

using System.Runtime.InteropServices;
using AutoIt;

using System.Windows.Automation;
using System.Text.RegularExpressions;

using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;



namespace TestAutoit
{
    public partial class Form1 : Form
    {

        int _cred = 0xFF0000;
        int _cgreen = 0x008000;

        Point _point_State_color;

        public Form1()
        {
            InitializeComponent();

            //button4_Click(this, EventArgs.Empty);
        }

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

        private void button2_Click(object sender, EventArgs e)
        {
            string win_desc = "[REGEXPTITLE:Ember Bootloader and Range Test .*]";
            IntPtr hwnd = activate_win(win_desc);

            AutoItX.Send("{SPACE}");

            Thread.Sleep(500);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int pixel_color = 0;
            string msg;
            while (true)
            {
                pixel_color = AutoItX.PixelGetColor(_point_State_color.X, _point_State_color.Y);

                if (pixel_color == _cred)
                {
                    AutoItX.MouseMove(_point_State_color.X, _point_State_color.Y, 0);
                    msg = string.Format("Red pixel at location X={0}, Y={1} after {2:F2} s",
                        _point_State_color.X, _point_State_color.Y, watch.Elapsed.TotalSeconds);
                    throw new Exception(msg);
                }
                else if (pixel_color == _cgreen)
                {
                    break;
                }


                Thread.Sleep(500);
                if(watch.Elapsed.TotalMinutes > 2)
                {
                    AutoItX.MouseMove(_point_State_color.X, _point_State_color.Y, 0);
                    msg = string.Format("Unable to detect either green or read pixel at location X={0}, Y={1} after {2} minutes",
                        _point_State_color.X, _point_State_color.Y, watch.Elapsed.TotalMinutes);
                    throw new Exception(msg);
                }
            }

            AutoItX.MouseMove(_point_State_color.X, _point_State_color.Y, 0);
            return;

        }

        void MouseHook_MouseAction(object sender, EventArgs e)
        {
            MouseHook.Stop();

            MouseHook.POINT p = new MouseHook.POINT();
            MouseHook.GetCursorPos(out p);

            label1.Text = string.Format("X = {0}, Y = {1}", p.x, p.y);

            _point_State_color = new Point(p.x, p.y);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AutomationElement desktop = AutomationElement.RootElement;

            PropertyCondition condition = new PropertyCondition(AutomationElement.ControlTypeProperty, System.Windows.Automation.ControlType.Window);
            AutomationElementCollection list = desktop.FindAll(TreeScope.Children, condition);
            AutomationElement win = null;
            foreach (AutomationElement item in list)
            {
                object name = item.GetCurrentPropertyValue(AutomationElement.NameProperty, false);
                string title = (string)name;
                Match match = Regex.Match(title, "Ember Bootloader and Range Test.*");
                if (match.Success)
                {
                    win = item;
                    break;
                }
            }

            condition = new PropertyCondition(AutomationElement.NameProperty, "State Row 0");
            AutomationElement sr0 = win.FindFirst(TreeScope.Descendants, condition);

            condition = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            list = win.FindAll(TreeScope.Descendants, condition);

            condition = new PropertyCondition(AutomationElement.NameProperty, "toolStrip1");
            AutomationElement toolstrip = win.FindFirst(TreeScope.Children, condition);

            list = toolstrip.FindAll(TreeScope.Subtree, System.Windows.Automation.Condition.TrueCondition);


            Debug.Assert(list.Count > 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<TestStack.White.UIItems.WindowItems.Window> list = TestStack.White.Desktop.Instance.Windows();
            TestStack.White.UIItems.WindowItems.Window win = null;
            foreach (TestStack.White.UIItems.WindowItems.Window item in list)
            {
                Match match = Regex.Match(item.Title, "Ember Bootloader and Range Test.*");
                if (match.Success)
                {
                    win = item;
                    break;
                }
            }

            TestStack.White.UIItems.WindowStripControls.ToolStrip toolstrip = win.GetToolStrip("toolStrip1");

            TestStack.White.UIItems.Button b = win.Get<TestStack.White.UIItems.Button>("UnLoad");
            bool t2 = win.HasPopup();


        }

        private void buttonGetXY_Click(object sender, EventArgs e)
        {
            MouseHook.Start();
            MouseHook.MouseAction += MouseHook_MouseAction;

        }
    }

}
