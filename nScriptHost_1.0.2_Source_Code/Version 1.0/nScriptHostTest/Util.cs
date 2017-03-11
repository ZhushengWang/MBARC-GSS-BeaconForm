using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using nScriptHost;

namespace nScriptHostTest
{
    public class Util
    {
        private System.Windows.Forms.TextBox MsgTextBox;
        private ScriptingHost ScriptHost;

        public Util(System.Windows.Forms.TextBox TextBox, ScriptingHost _scriptHost)
        {
            MsgTextBox = TextBox;
            ScriptHost = _scriptHost;
        }

        public void PrintMsg(string Msg)
        {
//            if (MsgTextBox.InvokeRequired)
//            {
                MsgTextBox.Invoke((MethodInvoker)delegate
                {
                    MsgTextBox.AppendText(Environment.NewLine+Msg);
                });
//            }
//            else
//            {
//                MsgTextBox.AppendText(Environment.NewLine+Msg);
//            }
        }

        public void Wait(double Secs)
        {
            try
            {
                System.Threading.Thread.Sleep((int)(Secs * 1000));
            }
            catch (System.Threading.ThreadInterruptedException)
            {
                //
            }
        }

        public void Readfile(string fname)
        {
            PrintMsg("Error:  Can't find file '" + fname + "'");
            ScriptHost.BreakScript();
        }
    }
}
