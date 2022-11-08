using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MotoSimEG_VRC;

namespace WinModConnect
{
    public partial class TestDlg : Form
    {
        private MotoSimEG_VRC.Application app = null;       // MotoSimEG-VRC object

        #region DllImport
        /// <summary>
        /// Settings of the parent
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        #endregion

        public TestDlg(MotoSimEG_VRC.Application MotoSimObj)
        {
            app = MotoSimObj;

            ShowInTaskbar = false;                  // Do not display the form to the taskbar
            IntPtr hWnd = new IntPtr(app.Hwnd);
            SetWindowLong(this.Handle, -8, hWnd);   // Change of the parent window
            InitializeComponent();
        }

        private void TestDlg_Shown(object sender, EventArgs e)
        {
            AttachEventHandlers();                  // Event handler registration
        }

        private void TestDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            DetachEventHandlers();                  // Event handler Detach 
        }

        #region Event Methods
        // Event handler registration
        bool AttachEventHandlers()
        {
            try
            {
                app.DestroyNotify += new IAppEvents_DestroyNotifyEventHandler(OnDestroyNotify);
                app.FileOpenNotify += new IAppEvents_FileOpenNotifyEventHandler(OnFileOpenNotify);
                app.FileCloseNotify += new IAppEvents_FileCloseNotifyEventHandler(OnFileCloseNotify);
                app.FileSaveNotify += new IAppEvents_FileSaveNotifyEventHandler(OnFileSaveNotify);
                app.JobPlaybackStart += new IAppEvents_JobPlaybackStartEventHandler(OnJobPlaybackStart);
                app.JobPlaybackEnd += new IAppEvents_JobPlaybackEndEventHandler(OnJobPlaybackEnd);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // Event handler Detach 
        bool DetachEventHandlers()
        {
            try
            {
                app.DestroyNotify -= new IAppEvents_DestroyNotifyEventHandler(OnDestroyNotify);
                app.FileOpenNotify -= new IAppEvents_FileOpenNotifyEventHandler(OnFileOpenNotify);
                app.FileCloseNotify -= new IAppEvents_FileCloseNotifyEventHandler(OnFileCloseNotify);
                app.FileSaveNotify -= new IAppEvents_FileSaveNotifyEventHandler(OnFileSaveNotify);
                app.JobPlaybackStart -= new IAppEvents_JobPlaybackStartEventHandler(OnJobPlaybackStart);
                app.JobPlaybackEnd -= new IAppEvents_JobPlaybackEndEventHandler(OnJobPlaybackEnd);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        #region Event Handlers
        //※ Note
        // The event handler is called from a separate thread from the main thread(UI thread).
        // Please DO NOT direct access to the form control in the event handler for that.
        // To access the form control from the event handler, you can use Invoke, BeginInvoke method.
        public void OnDestroyNotify()
        {
            //※ Note
            // Main thread will stop when the application terminates. You can not use the Invoke.
        }
        public void OnFileOpenNotify()
        {
            BeginInvoke(new MyEventHandler1(AppendLog), new object[] { "Event Cell Open\r\n" });
        }
        public void OnFileCloseNotify()
        {
            //※ Note
            // Main thread will stop when cell Close. You can not use the Invoke.
        }
        public void OnFileSaveNotify()
        {
            BeginInvoke(new MyEventHandler1(AppendLog), new object[] { "Event File Save\r\n" });
        }
        public void OnJobPlaybackStart()
        {
            Invoke(new MyEventHandler2(JobPlaybackStart));
        }
        public void OnJobPlaybackEnd()
        {
            Invoke(new MyEventHandler2(JobPlaybackEnd));
        }
        #endregion

        delegate void MyEventHandler1(String str);
        private void AppendLog(string text)
        {
            textBox1.Text += text;
        }

        delegate void MyEventHandler2();
        private void JobPlaybackStart()
        {
            string str = "Event Playback Start :" + app.Cell.Controllers[0].Jobs.CurrentJob + "\r\n";
            textBox1.Text += str;
        }

        private void JobPlaybackEnd()
        {
            string str = "Event Playback End :" + app.Cell.Controllers[0].Jobs.CurrentJob + "\r\n";
            textBox1.Text += str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
