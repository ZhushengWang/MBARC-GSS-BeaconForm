using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;

namespace nScriptHostTest
{
    public class AssemblyLoader : MarshalByRefObject
    {

        public AssemblyLoader()
        {

        }

        public void GetAssemblyInfo(string assemblyName, ref string version, ref string runtime)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(assemblyName);
                runtime = asm.ImageRuntimeVersion;
                version = asm.GetName().Version.ToString();
            }
            catch
            {
                // If it throws, then we just ignore runtime and version info.
            }
        }
    }


}
