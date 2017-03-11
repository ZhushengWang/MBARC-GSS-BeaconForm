using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class HPDAQCmds
  {
    private static HPDAQCmds _hpdaqcmds = null;
		private static object _instanceLock = new Object();
    private HP34970A hp34970A;
    private HP34972A hp34972A;
    private LogFile logFile;

    // This is a singleton class
		// Return a reference to the only instance of HPDAQCmds
		// Create the instance if necessary
		public static HPDAQCmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_hpdaqcmds == null)
					{
						_hpdaqcmds = new HPDAQCmds();
					}
					return _hpdaqcmds;
				}
			}
		}

		// Constructor
		public HPDAQCmds()
		{
      logFile = LogFile.Instance;
		}

    public void dispose()
    {
      hp34970A.dispose();
      hp34970A = null;

      hp34972A.dispose();
      hp34972A = null;
    }

    private string chanToString(int[] channels)
    {
      string sChannels = "";
      for (int i = 0; i < channels.Length; i++)
      {
        sChannels += channels[i].ToString();
        if (i < (channels.Length - 1))
        {
          sChannels += ",";
        }
      }
      return sChannels;
    }

    public void Init72(string sIP)
    {
      hp34972A = new HP34972A(sIP);
    }

    public void Init(string sDevID)
    {
      hp34970A = new HP34970A(sDevID);
      logFile.WriteLog("cmd: HPDAQ.Init(" + sDevID + ")");
    }

    public void SetRange(int range, params int[] nChannels)
    /* 0 - AUTO
     * 1 - 100mV
     * 2 - 1V
     * 3 - 10V
     * 4 - 100V
     * 5 - 300V
     */
    {
      hp34970A.SetRange(range, nChannels);
    }

    public void SetRangeAll(int Range)
    {
      hp34970A.SetRange(Range, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120);
    }

    public void SetRangeAll2(int Range)
    {
      hp34970A.SetRange(Range, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220);
    }

    public double[] ReadV(params int[] nChannels)
    {
      double[] dResult;

      dResult = hp34970A.GetVolts(-1, nChannels);
      logFile.WriteLog("cmd: HPDAQ.ReadV(" + chanToString(nChannels) + ")");
      return (dResult);
      //MainForm._mainform.PrintMsg(nChannel.ToString() + ":" + dResult.ToString()+"\n");
    }

    public double[] ReadVDelay(double dDelay, params int[] nChannels)
    {
      double[] dResult;

      dResult = hp34970A.GetVolts(dDelay, nChannels);
      logFile.WriteLog("cmd: HPDAQ.ReadV(" + dDelay.ToString() + "," + chanToString(nChannels) + ")");
      return (dResult);
    }

    public double ReadT(int nChannel)
    {
      double dResult;

      dResult = hp34970A.GetTemp(nChannel);
      logFile.WriteLog("cmd: HPDAQ.ReadT(" + nChannel.ToString() + ")");
      return (dResult);
    }

    public void WriteV(int nChannel, double dVolts)
    {
      hp34970A.SetVolts(nChannel, dVolts);
      logFile.WriteLog("cmd: HPDAQ.WriteV(" + nChannel.ToString() + "," + dVolts + ")");
    }

    public void SendCMD(string sCMD)
    {
      hp34970A.SendCMD(sCMD);
      logFile.WriteLog("cmd: HPDAQ.SendCMD(" + sCMD + ")");
    }

    public void GetID72()
    {
      string sResult = hp34972A.GetID();
      MainForm._mainform.PrintMsg(sResult + "\n");
    }

    public void GetID()
    {
      string sResult = hp34970A.GetID();
      MainForm._mainform.PrintMsg(sResult + "\n");
      logFile.WriteLog("cmd: HPDAQ.GetID()");
    }
  }
}
