using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments;

namespace SPRL.Test
{
  public class NI9205Cmds
  {
    private static NI9205Cmds _ni9205cmds = null;
		private static object _instanceLock = new Object();
    private NI9205[] ni9205 = new NI9205[8];
    private WaveformPlot waveformPlot;
    private ScatterPlot scatterPlot;
    private LogFile logFile;

    // This is a singleton class
		// Return a reference to the only instance of cDAQCmds
		// Create the instance if necessary
		public static NI9205Cmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_ni9205cmds == null)
					{
						_ni9205cmds = new NI9205Cmds();
					}
					return _ni9205cmds;
				}
			}
		}

		// Constructor
		public NI9205Cmds()
		{
      logFile = LogFile.Instance;
		}

    public void dispose(int nSlot)
    {
      ni9205[nSlot].dispose();
      ni9205[nSlot] = null;
    }

    public void Init(int nSlot, string sDevID)
    {
      ni9205[nSlot] = new NI9205(sDevID);
      logFile.WriteLog("cmd: NI9205.Init(" + sDevID + ")");
    }

    public void Cfg(int nSlot, int nCfg)
    {
      switch (nCfg)
      {
        case 0:
          ni9205[nSlot].aiTermCfg = NationalInstruments.DAQmx.AITerminalConfiguration.Rse;
          break;
        case 1:
          ni9205[nSlot].aiTermCfg = NationalInstruments.DAQmx.AITerminalConfiguration.Nrse;
          break;
        case 2:
          ni9205[nSlot].aiTermCfg = NationalInstruments.DAQmx.AITerminalConfiguration.Differential;
          break;
        case 3:
          ni9205[nSlot].aiTermCfg = NationalInstruments.DAQmx.AITerminalConfiguration.Pseudodifferential;
          break;
        default:
          ni9205[nSlot].aiTermCfg = NationalInstruments.DAQmx.AITerminalConfiguration.Rse;
          break;
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

    #region Plotting functions
    public void Plot(int nSlot)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        if (waveformPlot != null)
        {
          waveformPlot.Dispose();
        }
        waveformPlot = new WaveformPlot(ni9205[nSlot].bigData, ni9205[nSlot].channelsOn);
        waveformPlot.Show();
      });
      logFile.WriteLog("plot: NI9205.Plot(" + nSlot.ToString() + ")");
      GC.Collect();
    }

    public void OffsetPlot(int nChannel, double dOffset)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        waveformPlot.shiftPlot(nChannel, (decimal)dOffset);
      });
      logFile.WriteLog("plot: NI9205.OffsetPlot(" + nChannel.ToString() + "," + dOffset.ToString() + ")");
    }

    public void PlotScatter(int nSlot)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        if (scatterPlot != null)
        {
          scatterPlot.Dispose();
        }
        scatterPlot = new ScatterPlot(ni9205[nSlot].dArray, ni9205[nSlot].channelsOn, ni9205[nSlot].dClockRate);
        scatterPlot.Show();
      });
      logFile.WriteLog("plot: NI9205.PlotScatter(" + nSlot.ToString() + ")");
      GC.Collect();
    }

    public void ScatterPlot(int nSlot)
    {
      PlotScatter(nSlot);
    }

    public void OffsetScatter(int nChannel, double dOffset)
    {
      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        scatterPlot.shiftPlot(nChannel, (decimal)dOffset);
      });
      logFile.WriteLog("plot: NI9205.OffsetScatter(" + nChannel.ToString() + "," + dOffset.ToString() + ")");
    }

    #endregion

    //Sets Sample rate
    public void SetSampleRate(int nSlot, int sampleRate)
    {
      if (ni9205[nSlot].channelsOn != null)
      {
        if (250000 / ni9205[nSlot].channelsOn.Length < sampleRate)
        {
          MainForm._mainform.PrintMsg("Error, sample rate must be < 250,000/numChannels\n");
          ni9205[nSlot].dClockRate = 250000 / ni9205[nSlot].channelsOn.Length;
        }
      }
      else
      {
        ni9205[nSlot].dClockRate = sampleRate;
      }
      logFile.WriteLog("cmd: NI9205.SetSampleRate(" + nSlot.ToString() + "," + sampleRate.ToString() + ")");
    }
    
    //Returns the instantaneous voltage from a list of channels
    public double[] ReadV(int nSlot, params int[] channels)
    {
      logFile.WriteLog("cmd: NI9205.ReadV(" + nSlot.ToString() + "," + chanToString(channels) + ")");
      return ni9205[nSlot].readVoltage(channels);
    }
    
    //Returns the average voltage over a given number of samples for a list of channels
    public double[] AverageV(int nSlot, int numSamples, params int[] channels)
    {
      if (MainForm._mainform.bScriptStop) return new double[0];
      double[] daAverage = ni9205[nSlot].averageVoltage(numSamples, channels);
      logFile.WriteLog("cmd: NI9205.AverageV(" + nSlot.ToString() + "," + numSamples.ToString() + "," + chanToString(channels) + ")");
      return daAverage;
    }
    
    //Begins reading analog in values
    public void Start(int nSlot, params int[] channels)
    {
      if (MainForm._mainform.bScriptStop) return;
      ni9205[nSlot].start(channels);
      logFile.WriteLog("cmd: NI9205.Start(" + nSlot.ToString() + "," + chanToString(channels) + ")");
      return;
    }
    
    //Stops the reading of analog in values
    public void Stop(int nSlot)
    {
      ni9205[nSlot].stop();
      MainForm._mainform.PrintMsg("Acquired " + (ni9205[nSlot].bigData.Count * ni9205[nSlot].bigData[0][0].SampleCount).ToString() + " samples\n");
      logFile.WriteLog("cmd: NI9205.Stop(" + nSlot.ToString() + ")");
    }
    
    //Returns the data from a given channel
    public double[] GetData(int nSlot, int nChannel)
    {
      logFile.WriteLog("cmd: NI9205.GetData(" + nSlot.ToString() + "," + nChannel.ToString() + ")");
      return ni9205[nSlot].GetData(nChannel);
    }
    
    //Stops reading and returns the raw data
    public double[,] StopArray(int nSlot)
    {
      logFile.WriteLog("cmd: NI9205.StopArray(" + nSlot.ToString() + ")");
      return ni9205[nSlot].stopArray();
    }

  }
}
