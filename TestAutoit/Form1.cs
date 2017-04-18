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


namespace TestAutoit
{
    public partial class Form1 : Form
    {
        Point _point_State_color = new Point();
        string _coding_error_msg;
        CancellationTokenSource _tokenSrcCancel = new CancellationTokenSource();

        Coder _coder;
        uint _click_count = 0;

        Boolean _started = false;
        Boolean Started
        {
            get { return _started; }
            set
            {
                _started = value;

                string buttonText = "&Start";
                if (_started)
                    buttonText = "&Stop";
                synchronizedInvoke(buttonStart,
                    delegate ()
                    {
                        buttonStart.Text = buttonText;
                    });
            }
        }

        KeyboardHook _keyboard_hook = new KeyboardHook();

        public Form1()
        {
            InitializeComponent();

            numericUpDownCount.Minimum = 1;
            numericUpDownCount.Maximum = Decimal.MaxValue;

            labelStatus.Text = string.Format("Clicks = {0}", _click_count);

            _keyboard_hook.KeyPressed += _keyboard_hook_KeyPressed;
            _keyboard_hook.RegisterHotKey(global::ModifierKeys.Control, Keys.S);
            _keyboard_hook.RegisterHotKey(global::ModifierKeys.Control, Keys.D);
            _keyboard_hook.RegisterHotKey(global::ModifierKeys.Control, Keys.Up);
            _keyboard_hook.RegisterHotKey(global::ModifierKeys.Control, Keys.Down);

        }

        private void _keyboard_hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.Modifier == global::ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Keys.S:
                        start();
                        break;
                    case Keys.D:
                        stop();
                        break;
                    case Keys.Up:
                        numericUpDownCount.Value++;
                        break;
                    case Keys.Down:
                        numericUpDownCount.Value--;
                        break;
                }
            }
        }

        void stop()
        {
            if (_tokenSrcCancel != null)
                _tokenSrcCancel.Cancel();

            Started = false;
        }

        void done_handler(Task task)
        {
            Started = false;

            bool canceled = task.IsCanceled;
            var exception = task.Exception;
        }

        void coding_exception_handler(Task task)
        {
            Started = false;
            var exception = task.Exception;
            string errmsg = exception.InnerException.Message;
            _coding_error_msg = errmsg;
        }

        void start()
        {
            if (Started)
                return;

            Started = true;
            try
            {
                uint count = Convert.ToUInt32(numericUpDownCount.Value);
                _click_count = 0;

                _coder = new Coder();
                _coder.ClickEvent += Coder_ClickEvent;

                _tokenSrcCancel = new CancellationTokenSource();
                Task task = new Task(() => _coder.Code(count, _tokenSrcCancel.Token), _tokenSrcCancel.Token);
                task.ContinueWith(coding_exception_handler, TaskContinuationOptions.OnlyOnFaulted);
                task.ContinueWith(done_handler, TaskContinuationOptions.OnlyOnRanToCompletion);

                task.Start();
                Started = true;
            }
            catch
            {
                stop();
            }
        }
        private void start_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "&Start")
            {
                start();
            }
            else
            {
                stop();
            }

        }

        private void Coder_ClickEvent()
        {
            _click_count++;

            synchronizedInvoke(labelStatus,
                delegate ()
                {
                    labelStatus.Text = string.Format("Clicks = {0}, ToGo = {1}", _click_count, numericUpDownCount.Value - _click_count);
                });
        }

        void syncControlEnable(Control control, Boolean enabled)
        {
            synchronizedInvoke(control,
                delegate ()
                {
                    control.Enabled = enabled;
                });
        }

        void synchronizedInvoke(ISynchronizeInvoke sync, Action action)
        {
            // If the invoke is not required, then invoke here and get out.
            if (!sync.InvokeRequired)
            {
                // Execute action.
                action();

                // Get out.
                return;
            }

            try
            {
                // Marshal to the required context.
                sync.Invoke(action, new object[] { });
                //sync.BeginInvoke(action, new object[] { });
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }


        void MouseHook_MouseAction(object sender, EventArgs e)
        {
            MouseHook.Stop();

            MouseHook.POINT p = new MouseHook.POINT();
            MouseHook.GetCursorPos(out p);

            string label1 = string.Format("X = {0}, Y = {1}", p.x, p.y);

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
                Match match = Regex.Match(title, "Pixoto - Google Chrome");
                if (match.Success)
                {
                    win = item;
                    break;
                }
            }

            condition = new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_RenderWidgetHostHWND");
            AutomationElement wndvote = win.FindFirst(TreeScope.Children, condition);

            //wndvote.GetClickablePoint();

            InvokePattern click = wndvote.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            click.Invoke();


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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownCount_ValueChanged(object sender, EventArgs e)
        {
            if (_coder != null)
                _coder.Count = Convert.ToUInt32(numericUpDownCount.Value);
        }
    }

}
