using System;
using System.Text;
using Microsoft.Vsa;

namespace nScriptHost
{
    public class CompileErrorEventArgs : EventArgs
    {
        public IVsaError _error;
        public CompileErrorEventArgs(IVsaError error)
        {
            _error = error;
        }

        public IVsaError Error
        {
            get
            {
                return _error;
            }
        }
    }
}
