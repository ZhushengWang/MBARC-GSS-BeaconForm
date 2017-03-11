using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Microsoft.Vsa;
using nScriptHost;

namespace nScriptHostTest
{
    public partial class ScriptTest : Form
    {
        private ScriptingHost       _scriptHost;
        private ScriptingLanguage   _language = ScriptingLanguage.JavaScript;
        private List<string>        _gacItems = new List<string>();
        private List<string>        _pathedItems = new List<string>();
        private TestObject          _testOb = new TestObject();
        public  Util                myUtil;


        public ScriptTest()
        {
            _scriptHost = new ScriptingHost();
            InitializeComponent();
            _languageComboBox.SelectedIndex = 0;
            _scriptHost.CompileError += new CompileErrorHandler(ScriptHost_CompileError);
            _scriptHost.ScriptException += new ScriptExeceptionHandler(ScriptHost_Exception);
            _scriptHost.ExecutionComplete += new EventHandler(ScriptHost_ExecutionComplete);
            _haltButton.Enabled = false;
            myUtil = new Util(MsgTextBox, _scriptHost);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            myUtil.PrintMsg("Starting Script...");
            _compileErrorTextBox.Text = "";
            _scriptHost.SetScript(_sourceCodeTextBox.Text, _language);
            foreach(string item in _gacItems)
            {
                _scriptHost.AddReference(item);
            }
            foreach(string item in _pathedItems)
            {
                _scriptHost.AddReference(Path.GetFullPath(item), Path.GetFileName(item));
            }
            AddGlobalOb();
            _scriptHost.RunAsynchronously = true;
            _haltButton.Enabled = true;
            _scriptHost.Run();
        }

        private void ScriptHost_CompileError(object sender, CompileErrorEventArgs e)
        {
            _compileErrorTextBox.Text += "(" + e.Error.Line.ToString() + ", " + e.Error.StartColumn.ToString() + ") : " + e.Error.Description + "\r\n";
        }

        private void ScriptHost_Exception(object sender, ScriptExceptionEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ScriptExeceptionHandler(ScriptHost_Exception), new object[] {sender, e});
                return;
            }
            //_compileErrorTextBox.Text += "Exception in script execution: " + e.Exception.ToString() + "\r\n";

            if (e.Exception.InnerException != null)
            {
                if (e.Exception.InnerException.Message == "Function expected")
                {
                    string sLine = e.Exception.InnerException.StackTrace;
                    string[] sLines = sLine.Split('\n');
                    sLine = sLines[3];
                    int nIndex = sLine.LastIndexOf("Code:line");
                    sLine = sLine.Substring(nIndex + 10);
                    sLine = sLine.Trim();

                    myUtil.PrintMsg("Invalid Function at Script Line " + sLine + ".");
                }
                else
                {
                    if (e.Exception.InnerException.Message == "Thread was being aborted.")
                    {
                        myUtil.PrintMsg("User Break!  Script Stopped.");
                    }
                }
            }
            else
            {
                myUtil.PrintMsg(e.Exception.Message);
            }

            _haltButton.Enabled = false;
        }

        void ScriptHost_ExecutionComplete(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(ScriptHost_ExecutionComplete), new object[] { sender, e });
                return;
            }
            myUtil.PrintMsg( "Execution Completed!");
            _haltButton.Enabled = false;
        }

        private void ReferencesButton_Click(object sender, EventArgs e)
        {
            using (AddReferenceDialog ard = new AddReferenceDialog())
            {
                if (DialogResult.OK == ard.ShowDialog(this))
                {
                    _gacItems.Clear();
                    _pathedItems.Clear();
                    _gacItems.AddRange(ard.GACItems);
                    _pathedItems.AddRange(ard.PathedItems);
                }
            }
        }

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_languageComboBox.SelectedIndex == 0)
            {
                _language = ScriptingLanguage.JavaScript;
            }
            else
            {
                _language = ScriptingLanguage.VBScript;
            }
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            _compileErrorTextBox.Text = "";
            _scriptHost.SetScript(_sourceCodeTextBox.Text, _language);
            foreach (string item in _gacItems)
            {
                _scriptHost.AddReference(item);
            }
            foreach (string item in _pathedItems)
            {
                _scriptHost.AddReference(Path.GetFullPath(item), Path.GetFileName(item));
            }
            AddGlobalOb();
            _scriptHost.Compile();
        }

        private void AddGlobalOb()
        {
            _scriptHost.AddGlobalObject("TestObject", _testOb);
            _scriptHost.AddGlobalObject("Util", myUtil);
        }

        private void HaltButton_Click(object sender, EventArgs e)
        {
            _scriptHost.BreakScript();
        }

    }
}