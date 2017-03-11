using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class NI9264Cmds
  {
    private static NI9264Cmds _ni9264cmds = null;
		private static object _instanceLock = new Object();
    private NI9264[] ni9264 = new NI9264[8];
    private LogFile logFile;

    // This is a singleton class
		// Return a reference to the only instance of cDAQCmds
		// Create the instance if necessary
		public static NI9264Cmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_ni9264cmds == null)
					{
						_ni9264cmds = new NI9264Cmds();
					}
					return _ni9264cmds;
				}
			}
		}

		// Constructor
		public NI9264Cmds()
		{
      logFile = LogFile.Instance;
		}

    public void dispose(int nSlot)
    {
      ni9264[nSlot].dispose();
      ni9264[nSlot] = null;
    }

    public void Init(int nSlot, string sDevID)
    {
      ni9264[nSlot] = new NI9264(sDevID);
      logFile.WriteLog("cmd: NI9264.Init(" + nSlot.ToString() + "," + sDevID + ")");
    }

    public void SetV(int nSlot, int nChan, double dVolts)
    {
      ni9264[nSlot].SetV(nChan, dVolts);
      logFile.WriteLog("cmd: NI9264.SetV(" + nSlot.ToString() + "," + nChan.ToString() + "," + dVolts.ToString() + ")");
    }

    public double GetV(int nSlot, int nChan)
    {
      logFile.WriteLog("cmd: NI9264.GetV(" + nSlot.ToString() + "," + nChan.ToString() + ")");
      return ni9264[nSlot].daVolts[nChan];
    }
  }
}
