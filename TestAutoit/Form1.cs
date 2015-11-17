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


using powercal;

namespace TestAutoit
{
    public partial class Form1 : Form
    {
        Point _point_State_color;
        string _coding_error_msg;
        CancellationTokenSource _tokenSrcCancel = new CancellationTokenSource();

        string _win_desc = "[REGEXPTITLE:Ember Bootloader and Range Test .*]";
        IntPtr _hwnd;


        public Form1()
        {
            InitializeComponent();

            //button4_Click(this, EventArgs.Empty);
        }

        void coding_done_handler(Task task)
        {
            bool canceled = task.IsCanceled;
            var exception = task.Exception;
        }

        void coding_exception_handler(Task task)
        {
            var exception = task.Exception;
            string errmsg = exception.InnerException.Message;
            _coding_error_msg = errmsg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Coder coder = new Coder(_point_State_color, new TimeSpan(0,2,0));
            CancellationToken token = _tokenSrcCancel.Token;
            Task task = new Task( () => coder.Code(token), token);
            task.ContinueWith(coding_exception_handler, TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(coding_done_handler, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.Start();
        }

        void MouseHook_MouseAction(object sender, EventArgs e)
        {
            MouseHook.Stop();

            MouseHook.POINT p = new MouseHook.POINT();
            MouseHook.GetCursorPos(out p);

            label1.Text = string.Format("X = {0}, Y = {1}", p.x, p.y);

            _point_State_color = new Point(p.x, p.y);

        }

        void testAutomation()
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            activate();
            Point p = guessStartTest_AbortTestPos();
            AutoItX.MouseClick("LEFT", p.X, p.Y, 1, 10);
        }

        void testWhite()
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

        private void buttonRetry_Click(object sender, EventArgs e)
        {
            activate();
            Point p = guessRetryPos();
            AutoItX.MouseClick("LEFT", p.X, p.Y, 1, 10);

        }

        private void buttonGetXY_Click(object sender, EventArgs e)
        {
            MouseHook.Start();
            MouseHook.MouseAction += MouseHook_MouseAction;

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _tokenSrcCancel.Cancel();
        }

        private void buttonGuessXY_Click(object sender, EventArgs e)
        {

            activate();

            Rectangle rec_grid = getGridPos();

            AutoItX.MouseMove(0, 0, 5);

            Point point_status = guessStatusPos();
            mouseMove(point_status, 20);

            Point point_start_btn = guessStartTest_AbortTestPos();
            mouseMove(point_start_btn, 20);

            Point point_retry = guessRetryPos();
            mouseMove(point_retry, 20);

        }

        void mouseMove(Point point, int speed=0)
        {
            AutoItX.MouseMove(point.X, point.Y, speed);
        }

        void activate()
        {
            _hwnd = Coder.activate_win(_win_desc);
        }

        IntPtr getMenu()
        {
            return AutoItX.ControlGetHandle(_hwnd, "[NAME:menuStrip1]");
        }

        IntPtr getGrid()
        {
            return  AutoItX.ControlGetHandle(_hwnd, "[NAME:gridTesting]");
        }

        IntPtr getComboBox4()
        {
            return AutoItX.ControlGetHandle(_hwnd, "[REGEXPCLASS:.*COMBOBOX.*; INSTANCE:4]");
        }

        Rectangle getGridPos()
        {
            Rectangle rec_wnd = AutoItX.WinGetPos(_hwnd);

            IntPtr hmenu = getMenu();
            Rectangle rec_menu = AutoItX.ControlGetPos(_hwnd, hmenu);

            IntPtr hgrid = getGrid();
            Rectangle rec_grid = AutoItX.ControlGetPos(_hwnd, hgrid);

            int x = rec_wnd.X + rec_grid.X;
            int y = rec_wnd.Y + rec_grid.Y + rec_menu.Height;

            Rectangle rec = new Rectangle(x, y, rec_grid.Width, rec_grid.Height);
            return rec;

        }

        Point guessStatusPos()
        {
            Rectangle rec_grid = getGridPos();

            int x = rec_grid.X + 215;
            int y = rec_grid.Y + 30;

            return new Point(x, y);
        }

        Rectangle getCombo4Pos()
        {
            IntPtr hc = getComboBox4();
            Rectangle rec_combo4 = AutoItX.ControlGetPos(_hwnd, hc);

            Rectangle rec_wnd = AutoItX.WinGetPos(_hwnd);
            IntPtr hmenu = getMenu();
            Rectangle rec_menu = AutoItX.ControlGetPos(_hwnd, hmenu);

            int x = rec_wnd.X + rec_combo4.X;
            int y = rec_wnd.Y + rec_combo4.Y + rec_menu.Height;

            Rectangle rec = new Rectangle(x, y, rec_combo4.Width, rec_combo4.Height);
            return rec;

        }

        Point guessStartTest_AbortTestPos()
        {
            Rectangle rec_combo4 = getCombo4Pos();

            int x = rec_combo4.X + 200;
            int y = rec_combo4.Y + rec_combo4.Height / 2 + 5;

            return new Point(x, y);
        }

        Point guessRetryPos()
        {
            Point point = guessStartTest_AbortTestPos();

            return new Point(point.X + 65, point.Y);
        }

    }

}
