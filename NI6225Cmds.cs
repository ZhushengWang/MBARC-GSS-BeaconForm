using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class NI6225Cmds
  {
    private static NI6225Cmds _ni6225cmds = null;
		private static object _instanceLock = new Object();
    private NI6225 ni6225;
    private WaveformPlot waveformPlot;
    private ScatterPlot scatterPlot;
    private LogFile logFile;

    // This is a singleton class
		// Return a reference to the only instance of NI6225Cmds
		// Create the instance if necessary
		public static NI6225Cmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_ni6225cmds == null)
					{
						_ni6225cmds = new NI6225Cmds();
					}
					return _ni6225cmds;
				}
			}
		}

		// Constructor
		public NI6225Cmds()
		{
      logFile = LogFile.Instance;
		}

    public void Init(string sDevID)
    {
      if (ni6225 != null)
      {
        ni6225.Dispose();
      }
      ni6225 = new NI6225(sDevID);
      logFile.WriteLog("cmd: NI6225.Init(" + sDevID +")");
    }

    public void InitDO(string sChans)
    {
      logFile.WriteLog("cmd: NI6225.InitDO(" + sChans + ")");

      if (ni6225 == null)
      {
        MainForm._mainform.PrintError("ERROR:  Run NI6225.Init(\"Dev1\") first.\n");
      }
      else
      {
        ni6225.InitDO(sChans);
      }
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

    public void COut(double freq, double duty) //duty cycle AS A PERCENTAGE
    {
      ni6225.StartCounterOut(freq, duty);
     logFile.WriteLog("cmd: NI6225.COut(" + freq.ToString() + "," + duty.ToString() + ")");
    }

    public void StopCOut()
    {
      ni6225.StopCounterOut();
     logFile.WriteLog("cmd: NI6225.StopCOut()");
    }

    public void Pulse(int nChan, int nPeriod)
    {
      ni6225.Pulse(nChan, nPeriod);
      logFile.WriteLog("cmd: NI6225.Pulse(" + nChan.ToString() + "," + nPeriod.ToString() + ")");
    }

    public void DOWrite(int nValue, params int[] channels)
    {
      ni6225.DigOut(nValue, channels);
      logFile.WriteLog("cmd: NI6225.DOWrite(" + nValue.ToString() + "," + chanToString(channels) + ")");
    }

    public void SetDACV(double dVolts, params int[] channels)
    {
      ni6225.SetV(dVolts, channels);
      logFile.WriteLog("cmd: NI6225.SetDACV(" + dVolts.ToString() + "," + chanToString(channels) + ")");
    }

    public void Plot()
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        if (waveformPlot != null)
        {
          waveformPlot.Dispose();
        }
        waveformPlot = new WaveformPlot(ni6225.aiData, ni6225.channelsOn);
        waveformPlot.Show();
      });
      logFile.WriteLog("plot: NI6225.Plot()");
      GC.Collect();
    }

    public void OffsetPlot(int nChannel, double dOffset)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        waveformPlot.shiftPlot(nChannel, (decimal)dOffset);
      });
      logFile.WriteLog("plot: NI6225.OffsetPlot(" + nChannel.ToString() + "," + dOffset.ToString() + ")");
    }

    public void PlotScatter()
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        if (scatterPlot != null)
        {
          scatterPlot.Dispose();
        }
        scatterPlot = new ScatterPlot(ni6225.dArray, ni6225.channelsOn, ni6225.dClockRate);
        scatterPlot.Show();
      });
      logFile.WriteLog("plot: NI6225.PlotScatter()");
      GC.Collect();
    }

    public void ScatterPlot()
    {
      PlotScatter();
    }

    public void OffsetScatter(int nChannel, double dOffset)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        scatterPlot.shiftPlot(nChannel, (decimal)dOffset);
      });
      logFile.WriteLog("plot: NI6225.OffsetScatter(" + nChannel.ToString() + "," + dOffset.ToString() + ")");
    }

    public double[] AverageVoltage(int numSamples, bool bDifferential, params int[] channels)
    {
      if (MainForm._mainform.bScriptStop) return new double[0];
      double[] daAverage = ni6225.averageVoltage(numSamples, bDifferential, channels);
      logFile.WriteLog("cmd: NI6225.AverageVoltage(" + numSamples.ToString() + "," + bDifferential.ToString() + "," + chanToString(channels) + ")");
      return daAverage;
    }

    public void SetSampleClock(double dSampleClk)
    {
      ni6225.SetSampleClock(dSampleClk);
      logFile.WriteLog("cmd: NI6225.SetSampleClock(" + dSampleClk.ToString() + ")");
    }

    public void Start(params int[] channels)
    {
      if (MainForm._mainform.bScriptStop) return;
      ni6225.AIstart(channels);
      logFile.WriteLog("cmd: NI6225.Start(" + chanToString(channels) + ")");
    }

    public void Stop()
    {
      ni6225.stop();
      logFile.WriteLog("cmd: NI6225.Stop()");
    }

    public double[] GetData(int nChannel)
    {
      logFile.WriteLog("cmd: NI6225.GetData(" + nChannel.ToString() + ")");
      return ni6225.GetData(nChannel);
    }

    //public double[,] StopArray()
    //{
    //  logFile.WriteLog("cmd: NI6225.StopArray()");
    //  return ni6225.stopArray();
    //}

  }
}
