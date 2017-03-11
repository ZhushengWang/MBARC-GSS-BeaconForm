using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace nScriptHostTest
{
    public class TestObject
    {
        public void foo(string msg)
        {
            MessageBox.Show(msg);
        }

        public void bar(int num)
        {
            MessageBox.Show("Bar: " + num.ToString());
        }
    }
}
