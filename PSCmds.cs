using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using nScriptHost;

namespace SPRL.Test
{
  public class PSCmds
  {
    const int MAXSUPPLIES = 4;
    private static PSCmds _pscmds = null;
		private static object _instanceLock = new Object();
    private System.Timers.Timer timerPS;
    private const int TimeOut = 5000;
    private LogFile logFile;
    private List<string[]> globallValues;
    private bool bGVreadable = false;

    private IPSInterface[] PSArray = new IPSInterface[MAXSUPPLIES];

		// This is a singleton class
		// Return a reference to the only instance of PSCmds
		// Create the instance if necessary
		public static PSCmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_pscmds == null)
					{
						_pscmds = new PSCmds();
					}
					return _pscmds;
				}
			}
		}

    public void dispose()
    {
      timerPS.Stop();
      for (int i = 0; i < MAXSUPPLIES; i++)
      {
        PSArray[i].dispose();
      }
      PSArray = null;
    }

    public List<string[]> GetLastData()
    {
      if(bGVreadable) return globallValues;
      else return new List<string[]>();
    }

		// Constructor
		public PSCmds()
		{
      logFile = LogFile.Instance;
      timerPS = new System.Timers.Timer(TimeOut);
      timerPS.Elapsed += new System.Timers.ElapsedEventHandler(timerPS_Elapsed);
      timerPS.AutoReset = true;
      timerPS.Enabled = true;
      globallValues = new List<string[]>();
      for(int i=0; i < (MAXSUPPLIES*3);i++)
      {
        globallValues.Add(new string[3]);
      }
		}

    void timerPS_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      string[][] asValues;
      //string[,] asValues = {{"", "", ""},{"", "", ""},{"", "", ""},{"", "", ""},{"", "", ""},{"", "", ""}};
      List<string[]> lValues = new List<string[]>();
      int nRow = 0;

      asValues = new string[MAXSUPPLIES*3][];
      for (int nI=0;nI<asValues.Length;nI++)
      {
        asValues[nI] = new string[3];
      }
      int supplyType;

      for (int i = 0; i < MAXSUPPLIES; i++)
      {
        if (PSArray[i] != null)
        {
          if (PSArray[i] is E3631A_PS)
          {
            supplyType = 2;
          }
          else if (PSArray[i] is E3649A_PS)
          {
            supplyType = 1;
          }
          else
          {
            supplyType = 0;
          }

          for (int nSupply = supplyType; nSupply >= 0; nSupply--)
          {
            asValues[nRow][0] = string.Format("{0:0.000}", Convert.ToDouble(PSArray[i].GetVSet(nSupply)));
            asValues[nRow][1] = string.Format("{0:0.000}", Convert.ToDouble(PSArray[i].GetVolts(nSupply)));
            asValues[nRow][2] = string.Format("{0:0.000}", Convert.ToDouble(PSArray[i].GetAmps(nSupply)));
            lValues.Add(asValues[nRow]);
            nRow++;
          }
        }
        bGVreadable = false;
        for (int k = 0; k < lValues.Count; k++)
        {
          for (int j = 0; j < lValues[k].GetLength(0); j++)
          {
            globallValues[k][j] = lValues[k][j];
          }
        }
        bGVreadable = true;
      }
      //if (PSArray[1] != null)
      //{
      //  if (PSArray[1] is E3631A_PS) supplyType = 2;
      //  else supplyType = 0;
      //  for (int nSupply = supplyType; nSupply >= 0; nSupply--)
      //  {
      //    asValues[nSupply+3][0] = string.Format("{0:0.000}", Convert.ToDouble(PSArray[1].GetVSet(nSupply)));
      //    asValues[nSupply+3][1] = string.Format("{0:0.000}", Convert.ToDouble(PSArray[1].GetVolts(nSupply)));
      //    asValues[nSupply+3][2] = string.Format("{0:0.000}", Convert.ToDouble(PSArray[1].GetAmps(nSupply)));
      //    lValues.Add(asValues[nSupply+3]);
      //  }
      //}

      MainForm._mainform.UpdatePS(lValues);

      //timerPS.Enabled = true;
    }

    public void Init3631(int nPSNum, string sDevID)
    {
      if (nPSNum < 0 || nPSNum >=MAXSUPPLIES)
      {
        MainForm._mainform.PrintError("Error: Max Supply number is " + (MAXSUPPLIES - 1).ToString() + "\n");
      }
      else
      {
        if (PSArray[nPSNum] == null)
        {
          PSArray[nPSNum] = new E3631A_PS(sDevID);
          logFile.WriteLog("cmd: PS.Init3631(" + nPSNum.ToString() + "," + sDevID + ")");
        }
      }
      if (!timerPS.Enabled) timerPS.Enabled = true;
    }

    public void Init3632(int nPSNum, string sDevID)
    {
      if (nPSNum<0 || nPSNum>=MAXSUPPLIES)
      {
        MainForm._mainform.PrintError("Error: Max Supply number is " + (MAXSUPPLIES - 1).ToString() + "\n");
      }
      else
      {
        if(PSArray[nPSNum] == null)
        {
          PSArray[nPSNum] = new E3632A_PS(sDevID);
          logFile.WriteLog("cmd: PS.Init3632(" + nPSNum.ToString() + "," + sDevID + ")");
        }
      }
      if (!timerPS.Enabled) timerPS.Enabled = true;
    }

    public void Init3649(int nPSNum, string sDevID)
    {
      if (nPSNum < 0 || nPSNum >= MAXSUPPLIES)
      {
        MainForm._mainform.PrintError("Error: Max Supply number is " + (MAXSUPPLIES - 1).ToString() + "\n");
      }
      else
      {
        if (PSArray[nPSNum] == null)
        {
          PSArray[nPSNum] = new E3649A_PS(sDevID);
          logFile.WriteLog("cmd: PS.Init3649(" + nPSNum.ToString() + "," + sDevID + ")");
        }
      }
      if (!timerPS.Enabled) timerPS.Enabled = true;
    }

    public string GetIDN(int nPSNum)
    {
      string sResponse;

      sResponse = PSArray[nPSNum].GetID();
      MainForm._mainform.PrintMsg(sResponse);
      logFile.WriteLog("cmd: PS.GetIDN(" + nPSNum.ToString() + ")");
      return sResponse;
    }

    public void On()
    {
      if (!timerPS.Enabled) timerPS.Enabled = true;
      for (int i = 0; i < MAXSUPPLIES; i++)
      {
        if (PSArray[i] != null)
        {
          PSArray[i].SetOutOn(true);
        }
      }
      logFile.WriteLog("cmd: PS.On()");
    }

    public void On(int nSupply)
    {
      if (!timerPS.Enabled) timerPS.Enabled = true;
      if (PSArray[nSupply] != null)
      {
        PSArray[nSupply].SetOutOn(true);
      }
      logFile.WriteLog("cmd: PS.On(" + nSupply.ToString() + ")");
    }


    public void Off()
    {
      for (int i = 0; i < MAXSUPPLIES; i++)
      {
        if (PSArray[i] != null)
        {
          PSArray[i].SetOutOn(false);
        }
      }
      logFile.WriteLog("cmd: PS.Off()");
    }

    public void Off(int nSupply)
    {
      if (PSArray[nSupply] != null)
      {
        PSArray[nSupply].SetOutOn(false);
      }
      logFile.WriteLog("cmd: PS.Off(" + nSupply.ToString() + ")");
    }

    public void SetV(int nSupply ,int nOutput, double dVolts)
    {
      PSArray[nSupply].SetV(nOutput, dVolts);
      logFile.WriteLog("cmd: PS.SetV(" + nSupply.ToString() + "," + nOutput.ToString() + "," + dVolts.ToString() + ")");
    }

    public string GetV(int nSupply, int nOutput)
    {
      return PSArray[nSupply].GetVolts(nOutput);
    }

    public string GetSetV(int nSupply, int nOutput)
    {
      return PSArray[nSupply].GetVSet(nOutput);
    }

    public string GetI(int nSupply, int nOutput)
    {
      return PSArray[nSupply].GetAmps(nOutput);
    }

    public void SetI(int nSupply, int nOutput, double dAmps)
    {
      PSArray[nSupply].SetI(nOutput, dAmps);
      logFile.WriteLog("cmd: PS.SetI(" + nSupply.ToString() + "," + nOutput.ToString() + "," + dAmps.ToString() + ")");
    }
  }
}
