using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Vsa;

namespace nScriptHost
{
    public delegate void CompileErrorHandler(object sender, CompileErrorEventArgs e);
    public delegate void ScriptExeceptionHandler(object sender, ScriptExceptionEventArgs e);

    public class ScriptingHost : IVsaSite, IDisposable
    {

        
        #region Private Fields

        private IVsaEngine              _engine = null;
        private ScriptingLanguage       _language = ScriptingLanguage.JavaScript;
        private string                  _scriptText = "";

        private Hashtable               _globalObs = new Hashtable();
        private static int              _instanceID = 0;

        private object                  _lockOb = new object();

        private bool                    _disposed = false;

        private bool                    _useTemporaryFiles = true;

        private string                  _workingDirectory;

        private bool                    _runAsynchronously = false;

        private List<ReferenceItem>     _references = new List<ReferenceItem>();
        private List<GlobalObItem>      _globalObItems = new List<GlobalObItem>();

        private List<string>            _copiedReferences = new List<string>();

        private Thread                  _runThread = null;

        private bool                    _isInitialized = false;

        #endregion


        #region Constructors and Finalizer

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScriptingHost()
        {

        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~ScriptingHost()
        {
            Dispose(false);
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Whether or not to copy references to a temporary folder.
        /// </summary>
        /// <remarks>
        /// <para>The VBScript engine does not accept paths to referenced .DLLs,
        /// but the JScript engine does. To Ensure that VBScript works properly,
        /// when the VBScript engine is used, references can be copied to a
        /// temporary directory where the engine can access them. Setting this
        /// to false requires that the WorkingDirectory be set and that the
        /// dlls referenced (other than what's in the GAC) goes into that directory.</para>
        /// </remarks>
        public bool UseTemporaryFiles
        {
            get
            {
                return _useTemporaryFiles;
            }
            set
            {
                _useTemporaryFiles = value;
            }
        }


        /// <summary>
        /// Whether or not the script is executed in the main thread or a separate thread.
        /// </summary>
        public bool RunAsynchronously
        {
            get
            {
                return _runAsynchronously;
            }
            set
            {
                _runAsynchronously = value;
            }
        }

        /// <summary>
        /// Gets or sets the working directory where referenced DLLs are located.
        /// </summary>
        /// <remarks>This only has meaning for VBScript. It's not used in JScript.</remarks>
        public string WorkingDirectory
        {
            get
            {
                return _workingDirectory;
            }
            set
            {
                if (IsCompiled)
                {
                    throw new InvalidOperationException("You can't set the working directory after a script has been compiled.");
                }
                if (!Directory.Exists(_workingDirectory))
                {
                    throw new DirectoryNotFoundException("The working directory does not exist.");
                }
                _workingDirectory = value;
            }
        }

        /// <summary>
        /// Gets a unique ID for the script.
        /// </summary>
        public int NewInstanceID
        {
            get
            {
                lock(_lockOb)
                {
                    return ++_instanceID;
                }
            }
        }

        /// <summary>
        /// Whether or not the script has been compiled.
        /// </summary>
        /// <remarks>
        /// For multiple executions of the same script, there's no need to recompile.
        /// </remarks>
        public bool IsCompiled
        {
            get
            {
                if (_engine == null)
                {
                    return false;
                }
                return _engine.IsCompiled;
            }
        }

        /// <summary>
        /// Whether or not the script is currently executing.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_engine == null)
                {
                    return false;
                }

                return _engine.IsRunning;
            }
        }
        #endregion


        #region Public Methods

        /// <summary>
        /// Loads the script from a file.
        /// </summary>
        /// <param name="filename">File to open.</param>
        public void LoadScriptFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Unable to find the script file.", filename);
            }

            string ext = Path.GetExtension(filename);
            ScriptingLanguage language = ScriptingLanguage.JavaScript;
            if (ext == ".vb" || ext == ".vbe" || ext == ".vbs")
            {
                language = ScriptingLanguage.VBScript;
            }
            using (StreamReader sr = new StreamReader(filename))
            {
                string scriptText = sr.ReadToEnd();
                SetScript(scriptText, language);
            }
        }

        /// <summary>
        /// Sets the script from a string
        /// </summary>
        /// <param name="scriptText">Script text</param>
        /// <param name="language">Script language</param>
        public void SetScript(string scriptText, ScriptingLanguage language)
        {
            _isInitialized = false;
            // Make sure the last allocated engine is stopped.
            if (_engine != null)
            {
                _engine.Close();
            }

            // Delete references copied from the last script
            if (_copiedReferences.Count > 0)
            {
                CleanupOldReferences();
            }
            _scriptText = scriptText;
            _language = language;

            // Initialize globale vars
            _globalObs.Clear();
//----------------
            //// Create engine
            //if (_language == ScriptingLanguage.JavaScript)
            //{
            //    _engine = new Microsoft.JScript.Vsa.VsaEngine();
            //}
            //else
            //{
            //    _engine = new Microsoft.VisualBasic.Vsa.VsaEngine();
            //}

            //// Initialize Engine
            //_engine.RootMoniker = "nScriptHost://VSAScript/Instance" + NewInstanceID.ToString();
            //_engine.Site = this;
            //_engine.InitNew();
            //_engine.RootNamespace = "__Script__";

            //_references.Clear();
            //_globalObItems.Clear();

            //AddDefaultReferences();

//----------------

            // Create engine
            if (_language == ScriptingLanguage.JavaScript)
            {
                _engine = new Microsoft.JScript.Vsa.VsaEngine();
            }
            else
            {
                _engine = new Microsoft.VisualBasic.Vsa.VsaEngine();
            }


            // Initialize Engine
            _engine.RootMoniker = "nScriptHost://VSAScript/Instance" + NewInstanceID.ToString();
            _engine.Site = this;
            _engine.InitNew();
            _engine.RootNamespace = "__Script__";
            _engine.GenerateDebugInfo = true;

            _references.Clear();
            _globalObItems.Clear();

            AddDefaultReferences();

            // Set the script code.
            IVsaCodeItem item = _engine.Items.CreateItem("Code", VsaItemType.Code, VsaItemFlag.None) as IVsaCodeItem;
            item.SourceText = _scriptText;
        }


        /// <summary>
        /// Adds a reference from the GAC.
        /// </summary>
        /// <param name="assemblyName">Name of assembly.</param>
        public void AddReference(string assemblyName)
        {
            AddReference("", assemblyName);
        }

        /// <summary>
        /// Adds a reference from a file path.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="assemblyName">Name of assembly file</param>
        public void AddReference(string path, string assemblyName)
        {
            ReferenceItem refItem = new ReferenceItem(path, assemblyName);
            _references.Add(refItem);
        }

        /// <summary>
        /// Makes a managed object available to scripts.
        /// </summary>
        /// <param name="variableName">Name of globale variable to assign to object.</param>
        /// <param name="globalOb">The object.</param>
        public void AddGlobalObject(string variableName, object globalOb)
        {
            GlobalObItem item = new GlobalObItem(variableName, globalOb);
            _globalObItems.Add(item);
        }

        /// <summary>
        /// Compiles the script
        /// </summary>
        /// <returns><b>true</b> if compilation was successful, otherwise <b>false</b>.</returns>
        public bool Compile()
        {
            if (!_isInitialized)
            {
                InitializeEngine();
            }
            return _engine.Compile();
        }

        /// <summary>
        /// Compiles (if necessary) the script and executes it.
        /// </summary>
        public void Run()
        {
            if (IsRunning)
            {
                return;
            }
            if (!IsCompiled)
            {
                if (!_isInitialized)
                {
                    InitializeEngine();
                }
                if (!_engine.Compile())
                {
                    return;
                }
            }

            if (_runAsynchronously)
            {
                _runThread = new Thread(new ThreadStart(InternalRun));
                _runThread.IsBackground = true;
                _runThread.Name = "SCRIPTRUNTHREAD";
                _runThread.Start();
            }
            else
            {
                InternalRun();
            }
        }

        /// <summary>
        /// Cancels an asynchronous script
        /// </summary>
        public void BreakScript()
        {
            if (_runThread != null)
            {
                _runThread.Abort();
                _runThread = null;
            }
        }

        

        #endregion


        #region Protected Virtual Methods

        /// <summary>
        /// Triggers the CompileError event to inform the runtime.
        /// </summary>
        /// <param name="error">Error to report.</param>
        public void OnCompileError(IVsaError error)
        {
            CompileErrorHandler handler = CompileError;
            if (handler != null)
            {
                handler(this, new CompileErrorEventArgs(error));
            }
        }

        /// <summary>
        /// Triggers a script exception event.
        /// </summary>
        /// <param name="e"></param>
        public void OnScriptException(Exception e)
        {
            ScriptExeceptionHandler handler = ScriptException;
            if (handler != null)
            {
                handler(this, new ScriptExceptionEventArgs(e));
            }
        }

        /// <summary>
        /// Called when a script's execution has completed.
        /// </summary>
        public void OnExecutionComplete()
        {
            EventHandler handler = ExecutionComplete;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion


        #region IVsaSite Members

        /// <summary>
        ///  Returns the Event Source
        /// </summary>
        /// <remarks>
        /// We don't handle this yet. I think what we'll need to do is process this through
        /// an event handler.
        /// </remarks>
        /// <param name="itemName"></param>
        /// <param name="eventSourceName"></param>
        /// <returns></returns>
        public object GetEventSourceInstance(string itemName, string eventSourceName)
        {
            Debug.WriteLine("GetEventSourceInstance for " + itemName + "  eventSource - " + eventSourceName);
            return null;
        }

        /// <summary>
        /// When the engine calls back the IVsaSite to ask for a global item, return the instance if we've cached it previously
        /// </summary>
        /// <param name="name">The name of the global item to which an object instance is requested</param>
        /// <returns></returns>
        public object GetGlobalInstance(string name)
        {
            Debug.WriteLine("GetGlobalInstance for " + name);
            if (_globalObs.ContainsKey(name))
            {
                return _globalObs[name];
            }
            Debug.WriteLine("Unable to find Global Instance of " + name.ToString());
            return null;
        }

        /// <summary>
        /// Notifications about events generated by the engine.
        /// </summary>
        /// <param name="notify"></param>
        /// <param name="info"></param>
        public void Notify(string notify, object info)
        {
            Debug.WriteLine("Notify: " + notify + " - info Object Type: " + info.GetType().Name);
        }

        /// <summary>
        /// Called when there's a compiler error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool OnCompilerError(IVsaError error)
        {
            OnCompileError(error);
            return true;
        }

        /// <summary>
        /// We don't deal with compile states..
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="debugInfo"></param>
        public void GetCompiledState(out byte[] pe, out byte[] debugInfo)
        {
            pe = null;
            debugInfo = null;
        }

        #endregion


        #region IDisposable Members

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                    if (_engine != null)
                    {
                        _engine.Close();
                    }
                    if (_copiedReferences.Count > 0)
                    {
                        CleanupOldReferences();
                        Directory.Delete(_workingDirectory);
                    }
                }

                _disposed = true;
            }
        }

        #endregion


        #region Private Methods


        private void InitializeEngine()
        {
            _isInitialized = true;
            // Add our global objects
            foreach(GlobalObItem item in _globalObItems)
            {
                InternalAddGlobalObject(item.VariableName, item.GlobalOb);
            }

            // If we're using VBScript, set the ApplicationBase directory option.
            if (_language == ScriptingLanguage.VBScript)
            {
                _workingDirectory = Path.GetTempPath() + @"\";
                _engine.SetOption("ApplicationBase", _workingDirectory);
            }


            // If we're using VBScript and temporary files, then copy non GAC references.
            if (_language == ScriptingLanguage.VBScript &&
                UseTemporaryFiles)
            {
                CopyReferenceFiles();
            }

            // Add reference items
            foreach (ReferenceItem item in _references)
            {
                if (_language == ScriptingLanguage.VBScript)
                {
                    // VB uses ApplicationBase, not a path.
                    InternalAddReference("", item.AssemblyName);
                }
                else
                {
                    InternalAddReference(item.Path, item.AssemblyName);
                }
            }
        }

        /// <summary>
        /// Copies references to the temporary directory.
        /// </summary>
        private void CopyReferenceFiles()
        {
            _copiedReferences.Clear();
            foreach(ReferenceItem item in _references)
            {
                if (item.Path.Trim().Length > 0)
                {
                    File.Copy(item.Path + item.AssemblyName, _workingDirectory + item.AssemblyName);
                    _copiedReferences.Add(item.AssemblyName);
                }
            }
            
        }

        /// <summary>
        /// Deletes previously copied references from the temporary directory.
        /// </summary>
        private void CleanupOldReferences()
        {
            foreach(string filename in _copiedReferences)
            {
                File.Delete(_workingDirectory + filename);
            }
            _copiedReferences.Clear();
            _workingDirectory = "";
            _isInitialized = false;
        }
        

        /// <summary>
        /// Adds the reference item to the engine.
        /// </summary>
        /// <param name="path">Path to reference.</param>
        /// <param name="assemblyName">AssemblyName of reference.</param>
        private void InternalAddReference(string path, string assemblyName)
        {
            if (path.Trim().Length > 0 && !path.EndsWith(@"\"))
            {
                path += @"\";
            }
            path += assemblyName;
            Debug.Assert(_engine != null, "You cannot add references until a script has been set or loaded.");
            IVsaReferenceItem item = _engine.Items.CreateItem(path, VsaItemType.Reference, VsaItemFlag.None) as IVsaReferenceItem;
            item.AssemblyName = assemblyName;
        }

        /// <summary>
        /// Adds a global objec to the engine
        /// </summary>
        /// <param name="variableName">Name of variable.</param>
        /// <param name="globalOb">Object to add to Engine</param>
        private void InternalAddGlobalObject(string variableName, object globalOb)
        {
            Debug.Assert(_engine != null, "You cannot add global objects until a script has been set or loaded.");
            lock (_globalObs.SyncRoot)
            {
                _globalObs.Add(variableName, globalOb);
            }

            IVsaGlobalItem item = _engine.Items.CreateItem(variableName, VsaItemType.AppGlobal, VsaItemFlag.None) as IVsaGlobalItem;
            item.TypeString = globalOb.GetType().Name;
            item.ExposeMembers = true;
        }

        /// <summary>
        /// Adds the default references.
        /// </summary>
        private void AddDefaultReferences()
        {
            AddReference("mscorlib.dll");
            AddReference("system.dll");
            AddReference("system.drawing.dll");
            AddReference("system.data.dll");
            AddReference("system.windows.forms.dll");
        }


        private void InternalRun()
        {
            try
            {
                if (_language == ScriptingLanguage.VBScript)
                {
                    _engine.Run();
                    _engine.Assembly.GetType("__Script__.Script").GetMethod("Main").Invoke(null, null);
                }
                else
                {
									  _engine.Run();
                }
                _runThread = null;
            }
            catch (Exception ex)
            {
                OnScriptException(ex);
            }

            OnExecutionComplete();
        }

        #endregion


        #region Events

        public event CompileErrorHandler CompileError;

        public event ScriptExeceptionHandler ScriptException;

        public event EventHandler ExecutionComplete;

        #endregion


        #region Private Classes

        private class ReferenceItem
        {
            public string Path;
            public string AssemblyName;

            public ReferenceItem(string path, string assemblyName)
            {
                Path = path;
                AssemblyName = assemblyName;
            }
        }

        private class GlobalObItem
        {
            public string VariableName;
            public object GlobalOb;

            public GlobalObItem(string variableName, object globalOb)
            {
                VariableName = variableName;
                GlobalOb = globalOb;
            }
        }

        #endregion

    }
}
