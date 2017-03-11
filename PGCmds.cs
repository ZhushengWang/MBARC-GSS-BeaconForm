using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class PGCmds
  {
    private static PGCmds _pgcmds = null;
		private static object _instanceLock = new Object();
    private Agilent81130A ag81130a = null;
    private LogFile logFile;

		// This is a singleton class
		// Return a reference to the only instance of HPDAQCmds
		// Create the instance if necessary
		public static PGCmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_pgcmds == null)
					{
						_pgcmds = new PGCmds();
					}
					return _pgcmds;
				}
			}
		}

		// Constructor
		public PGCmds()
		{
      logFile = LogFile.Instance;
		}

    public void Init(int nAddress)
    {
      ag81130a = new Agilent81130A(nAddress);
      logFile.WriteLog("cmd: AG81130A.Init(" + nAddress.ToString() + ")");
    }

    public string GetID()
    {
      logFile.WriteLog("cmd: AG81130A.GetID()");
      return(ag81130a.GetID());
    }

    public void OutOn(int nChannel, bool bOn)
    {
      ag81130a.OutOn(nChannel, bOn, true);
      logFile.WriteLog("cmd: AG81130A.OutOn(" + nChannel.ToString() + "," + bOn.ToString() + ")");
    }

    public void OutOnN(int nChannel, bool bOn)
    {
      ag81130a.OutOn(nChannel, bOn, false);
      logFile.WriteLog("cmd: AG81130A.OutOnN(" + nChannel.ToString() + "," + bOn.ToString() + ")");
    }

    public void SetLevels(int nChannel, double dLow, double dHigh)
    {
      logFile.WriteLog("cmd: AG81130A.SetLevels(" + nChannel.ToString() + "," + dLow.ToString() + "," + dHigh.ToString() + ")");
      ag81130a.SetLevels(nChannel, dLow, dHigh);
    }

    //Period in nanosecods
    public void SetPeriod(double dPeriod)
    {
      logFile.WriteLog("cmd: AG81130A.SetPeriod(" + dPeriod.ToString() + ")");
      ag81130a.SetPeriod(dPeriod);
    }

    //Frequency in Hz
    public void SetFreq(double dFreq)
    {
      logFile.WriteLog("cmd: AG81130A.SetFreq(" + dFreq.ToString() + ")");
      ag81130a.SetPeriod(1 / dFreq * 1e9);
    }

    public void SetWidth(int nChannel, double dWidth)
    {
      logFile.WriteLog("cmd: AG81130A.SetWidth(" + nChannel.ToString() + "," + dWidth.ToString() + ")");
      ag81130a.SetWidth(nChannel, dWidth);
    }

    public void SetDuty(int nChannel, double dDuty)
    {
      logFile.WriteLog("cmd: AG81130A.SetDuty(" + nChannel.ToString() + "," + dDuty.ToString() + ")");
      ag81130a.SetDuty(nChannel, dDuty);
    }

    public void SetContinuous()
    {
      logFile.WriteLog("cmd: AG81130A.SetContinuous()");
      ag81130a.SetContinuous();
    }

    public void SetPattern(int nLength)
    {
      logFile.WriteLog("cmd: AG81130A.SetPattern(" + nLength.ToString() + ")");
      ag81130a.SetPattern(nLength);
    }

    public void SetPatternData(int nChan, string sData, int nLength)
    {
      logFile.WriteLog("cmd: AG81130A.SetPatternData(" + sData + "," + nLength.ToString() + ")");
      ag81130a.SetPatternData(nChan, sData, nLength);
    }

    public void WriteCmd(string sCmd)
    {
      logFile.WriteLog("cmd: AG81130A.WriteCmd(" + sCmd + ")");
      ag81130a.WriteCommand(sCmd);
    }

    public string Fetch()
    {
      return (ag81130a.Fetch());
    }
  }
}
