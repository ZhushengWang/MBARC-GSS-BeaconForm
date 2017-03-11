using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class UCCmds
  {
    private static UCCmds _uccmds = null;
    private static object _instanceLock = new Object();
    private HP5316B hp5316b = null;
    private LogFile logFile;

    // This is a singleton class
    // Return a reference to the only instance of HPDAQCmds
    // Create the instance if necessary
    public static UCCmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_uccmds == null)
          {
            _uccmds = new UCCmds();
          }
          return _uccmds;
        }
      }
    }

    // Constructor
    public UCCmds()
    {
      logFile = LogFile.Instance;
    }

    public void Init(int nAddress)
    {
      hp5316b = new HP5316B(nAddress);
      logFile.WriteLog("cmd: HP5316B.Init(" + nAddress.ToString() + ")");
    }

    public void SetTrig(string sChan, bool bPos)
    {
      if (sChan == "A")
      {
        if (bPos)
        {
          hp5316b.TrigAPos();
        }
        else
        {
          hp5316b.TrigANeg();
        }
      }
      else if (sChan == "B")
      {
        if (bPos)
        {
          hp5316b.TrigBPos();
        }
        else
        {
          hp5316b.TrigBNeg();
        }
      }
      else
      {
        MainForm._mainform.PrintError("Bad Channel Parameter\n");
      }

      logFile.WriteLog("cmd: HP5316B.SetTrig(" + sChan + "," + bPos.ToString() + ")");
    }

    public void WriteCmd(string sCmd)
    {
      logFile.WriteLog("cmd: HP5316B.WriteCmd(" + sCmd + ")");
      hp5316b.WriteCommand(sCmd);
    }

    public double ReadFreq()
    {
      string sRtnStr = hp5316b.Fetch();
      string sFreq = sRtnStr.Substring(1);

      return (Convert.ToDouble(sFreq));
    }

    public string Read()
    {
      return (hp5316b.Fetch());
    }
  }
}
