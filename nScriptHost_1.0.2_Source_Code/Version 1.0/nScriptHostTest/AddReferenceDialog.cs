using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace nScriptHostTest
{

    public delegate void AddRowDelegate(DataRow dr);

    public partial class AddReferenceDialog : Form
    {
        private DataTable       _gacDataTable = null;

        private List<string>    _gacItems = new List<string>();
        private List<string>    _pathedItems = new List<string>();

        public AddReferenceDialog()
        {
            InitializeComponent();
            BuildGACDataTable();
            
            Thread thread = new Thread(new ThreadStart(GACLoaderThread));
            thread.IsBackground = true;
            thread.Start();
            
        }

        public string[] GACItems
        {
            get
            {
                string[] gacItems = new string[_gacItems.Count];
                _gacItems.CopyTo(gacItems);
                return gacItems;
            }
        }

        public string[] PathedItems
        {
            get
            {
                string[] pathedItems = new string[_pathedItems.Count];
                _pathedItems.CopyTo(pathedItems);
                return pathedItems;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gacItems.Clear();
            ListViewItem[] rows = new ListViewItem[_gacListView.SelectedItems.Count];
            _gacListView.SelectedItems.CopyTo(rows, 0);
            foreach(ListViewItem lvi in rows)
            {
                _gacItems.Add(lvi.Text);
            }

            _pathedItems.Clear();
            string[] newPathedItems = new string[_filesListBox.Items.Count];
            _filesListBox.Items.CopyTo(newPathedItems, 0);
            _pathedItems.AddRange(newPathedItems);
            base.OnClosing(e);
        }


        #region Private Properties

        private string GACDirectory
        {
            get
            {
                return (new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System))).Parent.FullName + @"\assembly\";
            }
        }

        #endregion

        #region Private Methods

        private void GACLoaderThread()
        {
            LoadGACDataTable();
        }

        private void BuildGACDataTable()
        {
            // Create DataTable
            _gacDataTable = new DataTable("GAC");
            _gacDataTable.Columns.Add("Assembly", typeof(string));
            _gacDataTable.Columns.Add("Version", typeof(string));
            _gacDataTable.Columns.Add("Runtime", typeof(string));
            _gacDataTable.Columns.Add("Path", typeof(string));
        }

        private void LoadGACDataTable()
        {
            AppDomain domain = AppDomain.CreateDomain("junkDomain");
            ObjectHandle oh = domain.CreateInstance("nScriptHostTest", "nScriptHostTest.AssemblyLoader");
            AssemblyLoader loader = oh.Unwrap() as AssemblyLoader;
            // Get GAC items
            List<string> gacItems = GetGACItems();
            // LoadDataTable
            foreach (string gacItem in gacItems)
            {
                string version = "";
                string runtime = "";
                loader.GetAssemblyInfo(gacItem, ref version, ref runtime);
                Debug.WriteLine("Getting Info for Assembly: " + gacItem);
                Debug.WriteLine("Version: " + version + "   Runtime: " + runtime);
                DataRow dr = _gacDataTable.NewRow();
                dr["Path"] = gacItem;
                dr["Version"] = version;
                dr["Runtime"] = runtime;
                dr["Assembly"] = Path.GetFileNameWithoutExtension(gacItem);
                AddRow(dr);
            }
            
            AppDomain.Unload(domain);
        }

        private void AddRow(DataRow dr)
        {
            Debug.Assert(_gacDataTable != null, "_gacDataTable is null!");
            if (InvokeRequired)
            {
                BeginInvoke(new AddRowDelegate(AddRow), new object[] { dr });
                return;
            }
            _gacDataTable.Rows.Add(dr);
            ListViewItem item = new ListViewItem(dr["Assembly"].ToString());
            item.SubItems.Add(dr["Version"].ToString());
            item.SubItems.Add(dr["Runtime"].ToString());
            item.SubItems.Add(dr["Path"].ToString());
            _gacListView.Items.Add(item);
        }

        private List<string> GetGACItems()
        {
            List<string> gacItems = new List<string>();
            string[] rootDirs = Directory.GetDirectories(GACDirectory);
            foreach(string rootDir in rootDirs)
            {
                if (rootDir.ToLower().EndsWith("gac"))
                {
                    string[] subDirs = Directory.GetDirectories(rootDir);
                    foreach (string subDir in subDirs)
                    {
                        string[] asmDirs = Directory.GetDirectories(subDir);
                        foreach (string asmDir in asmDirs)
                        {
                            string[] files = Directory.GetFiles(asmDir);
                            files = TrimFileList(files);
                            foreach (string filename in files)
                            {
                                gacItems.Add(filename);
                            }
                        }
                    }
                }
            }
            Debug.WriteLine("Returning " + gacItems.Count.ToString() + " gacItems");
            return gacItems;
        }

        private string[] TrimFileList(string[] files)
        {
            ArrayList newList = new ArrayList();
            foreach(string filename in files)
            {
                string ext = Path.GetExtension(filename).ToLower();
                if (ext == ".dll" || ext == ".exe")
                {
                    newList.Add(filename);
                }
            }

            return newList.ToArray(typeof(string)) as string[];
        }

        #endregion

        private void AddFileButton_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "dll files (*.dll)|*.dll|All files (*.*)|*.*";
                ofd.Multiselect = true;

                if (DialogResult.OK == ofd.ShowDialog())
                {
                    _filesListBox.Items.AddRange(ofd.FileNames);
                }

            }
        }

        private void RemoveFileButton_Click(object sender, EventArgs e)
        {
            ArrayList toRemove = new ArrayList();
            foreach(object ob in _filesListBox.SelectedItems)
            {
                toRemove.Add(ob);
            }

            foreach(object ob in toRemove)
            {
                _filesListBox.Items.Remove(ob);
            }
        }

    }
}