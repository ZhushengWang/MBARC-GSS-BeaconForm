using System;
using System.Collections.Generic;
using System.Text;

namespace nScriptHost
{
    public class ScriptExceptionEventArgs : EventArgs
    {
        private Exception _exception;

        public ScriptExceptionEventArgs(Exception exception)
            : base()
        {
            _exception = exception;
        }

        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }
    }
}
