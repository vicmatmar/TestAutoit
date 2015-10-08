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
        Point _point_State_color;
        string _coding_error_msg;
        CancellationTokenSource _tokenSrcCancel = new CancellationTokenSource();

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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _tokenSrcCancel.Cancel();
        }
    }

}
