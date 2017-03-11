using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class UnivSrcCmds
  {
    private static UnivSrcCmds _univsrccmds = null;
    private static object _instanceLock = new Object();
    private HP3245A hp3245a = null;

    // This is a singleton class
    // Return a reference to the only instance of HPDAQCmds
    // Create the instance if necessary
    public static UnivSrcCmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_univsrccmds == null)
          {
            _univsrccmds = new UnivSrcCmds();
          }
          return _univsrccmds;
        }
      }
    }

    public UnivSrcCmds()
    {
    }

    public void Init(int nAddress)
    {
      hp3245a = new HP3245A(nAddress);
    }

    public string GetID()
    {
      return hp3245a.GetID();
    }

    public void SetV(double dVolts)
    {
      hp3245a.SetV(dVolts);
    }

    public double GetV()
    {
      return hp3245a.GetV();
    }

    public void SetCh(int nChan) //nChan = 0 or 1
    {
      hp3245a.SetCh(nChan);
    }
  }
}
