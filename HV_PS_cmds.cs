using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class HV_PS_cmds
  {
    private static HV_PS_cmds _hvpscmds = null;
    private static object _instanceLock = new Object();
    private PS325 ps325 = null;
    //private LogFile logFile;

    // This is a singleton class
    // Return a reference to the only instance of HPDAQCmds
    // Create the instance if necessary
    public static HV_PS_cmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_hvpscmds == null)
          {
            _hvpscmds = new HV_PS_cmds();
          }
          return _hvpscmds;
        }
      }
    }

    public HV_PS_cmds()
    {
      //logFile = LogFile.Instance;
    }

    public void Init(int nAddress)
    {
      ps325 = new PS325(nAddress);
    }

    public string GetID()
    {
      return ps325.GetID();
    }

    public void SetV(double dVolts)
    {
      ps325.SetV(dVolts);
    }

    public double GetSetV()
    {
      return ps325.GetSetV();
    }

    public double GetOutV()
    {
      return ps325.GetOutV();
    }

    public double GetOutI()
    {
      return ps325.GetOutI();
    }

    public void SetMaxI(double dAmps)
    {
      ps325.SetMaxI(dAmps);
    }

    public void SetMaxV(double dVolts)
    {
      ps325.SetMaxV(dVolts);
    }

    public double GetMaxV()
    {
      return ps325.GetMaxV();
    }

    public void On()
    {
      ps325.TurnOn();
    }

    public void Off()
    {
      ps325.TurnOff();
    }
  }
}
