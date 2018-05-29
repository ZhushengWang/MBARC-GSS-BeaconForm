using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPRL.Test
{
  public class TestEquityCmds
  {
    private static TestEquityCmds _testequitycmds = null;
		private static object _instanceLock = new Object();
    private ICS8099 ics8099;
    private LogFile logFile;

    // This is a singleton class
		// Return a reference to the only instance of HPDAQCmds
		// Create the instance if necessary
		public static TestEquityCmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_testequitycmds == null)
					{
						_testequitycmds = new TestEquityCmds();
					}
					return _testequitycmds;
				}
			}
		}

		// Constructor
		public TestEquityCmds()
		{
      logFile = LogFile.Instance;
		}

    public void dispose()
    {
      ics8099.dispose();
      ics8099 = null;
    }

    public void Init(string sDevID)
    {
      if (ics8099 != null)
      {
        ics8099 = null;
      }
      ics8099 = new ICS8099(sDevID);
    }

    public void SetTemp(double dTemp)
    {
      ics8099.SetTemp(dTemp);
    }

    public double GetTemp()
    {
      return (ics8099.GetTemp());
    }

    public void GetWatlowModel()
    {
      string sResult = ics8099.GetWatlowModel();
            
      MainForm._mainform.PrintMsg(sResult + "\n");
    }

    public void GetID()
    {
      string sResult = ics8099.GetID();

      MainForm._mainform.PrintMsg(sResult + "\n");
    }

    public void SendCMD(string sCMD)
    {
      ics8099.SendCMD(sCMD);
    }
  }
}


