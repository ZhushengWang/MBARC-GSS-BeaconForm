using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class SCPICmds
  {
    private static SCPICmds _scpicmds = null;
    private static object _instanceLock = new Object();
    private SCPI scpi;
    private LogFile logFile;

    // This is a singleton class
    // Return a reference to the only instance of HPDAQCmds
    // Create the instance if necessary
    public static SCPICmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_scpicmds == null)
          {
            _scpicmds = new SCPICmds();
          }
          return _scpicmds;
        }
      }
    }

    // Constructor
    public SCPICmds()
    {
      logFile = LogFile.Instance;
    }

    public void dispose()
    {
      scpi.dispose();
      scpi = null;
    }

    public void Init(string sDevID)
    {
      scpi = new SCPI(sDevID);
      logFile.WriteLog("cmd: SCPICmds.Init(" + sDevID + ")");
    }

    public void SendCMD(string sCMD)
    {
      scpi.SendCMD(sCMD);
      logFile.WriteLog("cmd: SCPICmds.SendCMD(" + sCMD + ")");
    }

    public string Query(string sCmd)
    {
      string sResult = scpi.Query(sCmd);
      sResult = sResult.Trim('"','\n','\r');
      return(sResult);
    }

    public void GetID()
    {
      string sResult = scpi.GetID();
      MainForm._mainform.PrintMsg(sResult + "\n");
      logFile.WriteLog("cmd: SCPICmds.GetID()");
    }
  }
}
