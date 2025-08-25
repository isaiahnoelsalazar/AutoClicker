using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class MainForm : Form
    {
        bool isStart = false;
        Keys[] Numbers = { 
            Keys.D0, 
            Keys.D1, 
            Keys.D2, 
            Keys.D3, 
            Keys.D4, 
            Keys.D5, 
            Keys.D6, 
            Keys.D7, 
            Keys.D8,
            Keys.D9,
            Keys.NumPad0,
            Keys.NumPad1,
            Keys.NumPad2,
            Keys.NumPad3,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad6,
            Keys.NumPad7,
            Keys.NumPad8,
            Keys.NumPad9,
            Keys.Back, 
            Keys.Delete 
        };

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int hid = 1;

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        public void leftClick(Point p)
        {
            Cursor.Position = p;
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        public void rightClick(Point p)
        {
            Cursor.Position = p;
            mouse_event((int)(MouseEventFlags.RIGHTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.RIGHTUP), 0, 0, 0, 0);
        }

        public MainForm()
        {
            InitializeComponent();
            KeyPreview = true;
            RegisterHotKey(Handle, hid, 0, (int)Keys.F6);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == hid)
            {
                if (isStart)
                {
                    isStart = false;
                }
                else
                {
                    isStart = true;
                }
                StartAuto();
            }
            base.WndProc(ref m);
        }

        async private void StartAuto()
        {
            try
            {
                int time = Convert.ToInt32(textBox1.Text);
                if (time < 5)
                {
                    while (isStart)
                    {
                        if (radioButton1.Checked)
                        {
                            leftClick(Cursor.Position);
                            await Task.Delay(5);
                        }
                        else if (radioButton2.Checked)
                        {
                            rightClick(Cursor.Position);
                            await Task.Delay(5);
                        }
                    }
                }
                while (isStart && time > 5)
                {
                    if (radioButton1.Checked)
                    {
                        leftClick(Cursor.Position);
                        await Task.Delay(time);
                    }
                    else if (radioButton2.Checked)
                    {
                        rightClick(Cursor.Position);
                        await Task.Delay(time);
                    }
                }
            }
            catch
            {
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Numbers.Contains(e.KeyCode))
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
