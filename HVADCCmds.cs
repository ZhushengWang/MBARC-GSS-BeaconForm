using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace SPRL.Test
{
  public class HVADCCmds
  {
    private static HVADCCmds _hvadccmds = null;
    private static object _instanceLock = new Object();
    private DI730[] adi730 = new DI730[9];
    private ScatterPlot scatterPlot;
    private LogFile logFile;
    //private static double[][] AvgArray;
    delegate double[] update(int nModule);

    // This is a singleton class
    // Return a reference to the only instance of cDAQCmds
    // Create the instance if necessary
    public static HVADCCmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_hvadccmds == null)
          {
            _hvadccmds = new HVADCCmds();
          }
          return _hvadccmds;
        }
      }
    }

    // Constructor
    public HVADCCmds()
    {
      logFile = LogFile.Instance;
    }

    public void dispose(int nDevID)
    {
      if (adi730[nDevID] != null)
      {
        adi730[nDevID].dataq.Stop();
        adi730[nDevID].dataq.Dispose();
        adi730[nDevID] = null;
      }
    }

    public void setToNull(int nDevID)
    {
      adi730[nDevID] = null;
    }

    public int Init(int nDevID)
    {
      if ((nDevID >= 0) && (nDevID <= 8))
      {
        if (adi730[nDevID] == null)
        {
          try
          {
            //Tries to kill DLL each time program runs
            KillDLL(nDevID);
            adi730[nDevID] = new DI730(nDevID);
            return 0;
          }
          catch (Exception e)
          {
            MainForm._mainform.PrintError("Error: " + e.Message + "\n");
            try
            {
              GC.Collect();
              //System.Windows.Forms.Application.Restart();
              adi730[nDevID].Dispose();
            }
            catch (Exception e2)
            {
              //MainForm._mainform.PrintError("Error: " + e2.Message + "\n");
            }
            adi730[nDevID] = null;
            return -1;
          }
        }
        else
        {
          //MainForm._mainform.PrintError("Error: module already init'd.");
        }
      }

      else
      {
        MainForm._mainform.PrintError("Error: DevID is 0 to 8\n");
        return -1;
      }
      logFile.WriteLog("cmd: HVADC.Init(" + nDevID.ToString() + ")");
      return 0;
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

    public void KillDLL(int nDevID)
    {
      adi730[nDevID] = null;

      Process[] toKill = Process.GetProcessesByName("discn72" + nDevID.ToString());

      foreach (Process proc in toKill)
      {
        MainForm._mainform.PrintMsg("Killing " + proc.ProcessName.ToString() + ".dll\n");
        proc.Kill();
      }
      
      logFile.WriteLog("cmd: HVADC.KillDLL(" + nDevID.ToString() + ")");
      return;
    }

    #region Plotting functions
    public void PlotScatter(int nDevID)
    {
      if (adi730[nDevID] != null)
      {
        MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
        {
          scatterPlot = new ScatterPlot(adi730[nDevID].outData, adi730[nDevID].channelsOn, adi730[nDevID].nSampleRate);
          scatterPlot.Show();
        });
        logFile.WriteLog("plot: HVADC.PlotScatter(" + nDevID.ToString() + ")");
      }
    }

    public void OffsetScatter(int nChannel, double dOffset)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        scatterPlot.shiftPlot(nChannel, (decimal)dOffset);
      });
      logFile.WriteLog("plot: HVADC.OffsetScatter(" + nChannel.ToString() + "," + dOffset.ToString() + ")");
    }
    #endregion

    public void SetSampleRate(int nDevID, int nSampleRate)
    {
      if (adi730[nDevID] != null)
      {
        adi730[nDevID].SetSampleRate(nSampleRate);
        logFile.WriteLog("cmd: HVADC.SetSampleRate(" + nDevID.ToString() + "," + nSampleRate.ToString() + ")");
      }
    }

    public void Start(int nDevID, params int[] channels)
    {
      if (adi730[nDevID] != null)
      {
        try
        {
          adi730[nDevID].Start(channels);
        }
        catch (Exception e)
        {
          MainForm._mainform.PrintError(e.Message + "\n");
        }
        logFile.WriteLog("cmd: HVADC.Start(" + nDevID.ToString() + "," + chanToString(channels) + ")");
      }
    }

    public double[,] Stop(int nDevID)
    {
      if (adi730[nDevID] != null)
      {
        try
        {
          double[,] stopArray = adi730[nDevID].stopArray();
          logFile.WriteLog("cmd: HVADC.Stop(" + nDevID.ToString() + ")");
          return stopArray;
        }
        catch (Exception e)
        {
          MainForm._mainform.PrintError(e.Message + "\n");
          return new double[0, 0];
        }
      }
      else
        return new double[0, 0];
    }

    public double[][] Stop2DArray(int nDevID)
    {
      double[,] tempArray = Stop(nDevID);
      double[][] returnArray;

      int numSamples = tempArray.GetLength(1);
      int numChannels = tempArray.GetLength(0);

      returnArray = new double[numChannels][];

      for (int i = 0; i < numChannels; i++)
      {
        returnArray[i] = new double[numSamples];

        for (int j = 0; j < numSamples; j++)
        {
          returnArray[i][j] = tempArray[i, j];
        }
      }

      return returnArray;
    }

    public double GetAvg(int nDevID, int channel, int numSamples)
    {
      if (adi730[nDevID] != null)
      {
        if ((channel >= 1) && (channel <= 8))
        {
          try
          {
            double average = adi730[nDevID].AverageInit(channel, numSamples);
            logFile.WriteLog("cmd: HVADC.GetAvg(" + nDevID.ToString() + "," + channel.ToString() + "," + numSamples.ToString() + ")");
            return average;
          }
          catch (Exception e)
          {
            MainForm._mainform.PrintError(e.Message);
            return 0.0;
          }
        }
        else
        {
          MainForm._mainform.PrintError("Error: Channel is 1 to 8\n");
          return (0.0);
        }
      }
      else
        return 0.0;
    }

    public double[] GetAvgArray(int nDevID, int numSamples, params int[] channels)
    {
      if (adi730[nDevID] != null)
      {
        foreach (int j in channels)
        {
          if ((1 > j) || (j > 8))
          {
            MainForm._mainform.PrintError("Error: Channel is 1 to 8\n");
            return new double[8];
          }
        }
        try
        {
          double[] average = adi730[nDevID].AverageArrayInit(numSamples, channels);
          logFile.WriteLog("cmd: HVADC.GetAvgArray(" + nDevID.ToString() + "," + numSamples.ToString() + "," + chanToString(channels) + ")");
          return average;
        }
        catch (Exception e)
        {
          MainForm._mainform.PrintError(e.Message + "\n");
          return new double[8];
        }
      }
      else
        return new double[8];
    }

    //public double[][] AllChAvg(int numSamples, params int[] nModList)
    //{
    //  AvgArray = new double[9][];

    //  for (int i = 0; i < nModList.Length; i++)
    //  {
    //    AvgArray[nModList[i]] = new double[8];

    //    update u = new update(hv_update);
    //    u.BeginInvoke(nModList[i], new AsyncCallback(save), i);
    //  }
    //  Thread.Sleep(2000);
    //  return AvgArray;
    //}

    //private static void save(IAsyncResult result)
    //{
    //  int mod = (int)result.AsyncState;
    //  AsyncResult async = (AsyncResult)result;
    //  update updatefn = (update)async.AsyncDelegate;
    //  AvgArray[mod] = updatefn.EndInvoke(result);
    //}

    //private double[] hv_update(int mod)
    //{
    //   return adi730[mod].AverageArrayInit(10, 1,2,3,4,5,6,7,8);
    //}

    public void SetGainList(int nDevID, params double[] volts)
    {
      adi730[nDevID].setGainList(volts);
    }

    public void SetChanList(int nDevID, params int[] chans)
    {
      adi730[nDevID].setChanList(chans);
    }

    public void SetMaxV(int nDevID, int nChannel, double dMaxV)
    {
      if (adi730[nDevID] != null)
      {
        if ((nChannel >= 1) && (nChannel <= 8))
        {
          if (dMaxV <= 0.01)
          {
            adi730[nDevID].SetGain(nChannel - 1, 3);
          }
          else
          {
            if (dMaxV <= 0.1)
            {
              adi730[nDevID].SetGain(nChannel - 1, 2);
            }
            else
            {
              if (dMaxV <= 1.0)
              {
                adi730[nDevID].SetGain(nChannel - 1, 1);
              }
              else
              {
                if (dMaxV <= 10)
                {
                  adi730[nDevID].SetGain(nChannel - 1, 0);
                }
                else
                {
                  if (dMaxV <= 100)
                  {
                    adi730[nDevID].SetGain(nChannel - 1, 5);
                  }
                  else
                  {
                    if (dMaxV <= 1000)
                    {
                      adi730[nDevID].SetGain(nChannel - 1, 4);  //1000V
                    }
                    else
                    {
                      MainForm._mainform.PrintError("Error:  Bad MaxV\n");
                    }
                  }
                }
              }
            }
          }
        }
        else
        {
          MainForm._mainform.PrintError("Error: Channel is 1 to 8\n");
        }
        logFile.WriteLog("cmd: HVADC.SetMaxV(" + nDevID.ToString() + "," + nChannel.ToString() + "," + dMaxV.ToString() + ")");
      }
    }
  }
}