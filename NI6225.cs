using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NationalInstruments.DAQmx;
using NationalInstruments;
using System.Threading;

namespace SPRL.Test
{
  public class NI6225 : IDisposable
  {
    private string sDevLocal;

    private Task aoTask;  //Analog out
    private Task aiTask;  //Analog in
    private Task doTask;  //Digital out
    private Task coTask;  //Counter out
    private Task runningTask;
    private AnalogMultiChannelWriter aoWriter;
    private AnalogMultiChannelReader aiReader;
    private DigitalSingleChannelWriter doWriter;
    private COPulseIdleState idleState;
    private double[] daVolts;
    private bool[] baDOArray;

    public List<AnalogWaveform<double>[]> aiData;
    private AsyncCallback analogCallback;
    public AnalogWaveform<double>[] contData;
    public double dClockRate = 3000; //Max 250,000/80 = min 3125
    private Int32 iSamplesPerChannel = 500;
    public int[] channelsOn;
    private double coFreq = 0.0;
    private double coDutyCycle = 0.0;
    public double[,] dArray;

    private Hashtable hashChannels;

    public NI6225(string sDev) 
    {
      sDevLocal = sDev;
      //Initialize Analog out
      daVolts = new double[2];

      for (int nI = 0; nI < 2; nI++)
      {
        daVolts[nI] = 0.0;
      }

      aoTask = new Task();
      aoTask.AOChannels.CreateVoltageChannel(sDev + "/ao0:" + sDev + "/ao1", "", -5.0, 5.0, AOVoltageUnits.Volts);
      aoWriter = new AnalogMultiChannelWriter(aoTask.Stream);
      aoWriter.WriteSingleSample(true, daVolts);  //Init to all 0V

    }

    public void InitDO (string sChans)
    {
      int nNumChans = 0;

      try
      {
        doTask = new Task();
        doTask.DOChannels.CreateChannel(sChans, "", ChannelLineGrouping.OneChannelForAllLines);
        doWriter = new DigitalSingleChannelWriter(doTask.Stream);
        doTask.Control(TaskAction.Verify);
        nNumChans = (int)doTask.DOChannels[0].NumberOfLines;
        baDOArray = new bool[nNumChans];

        for (int j = 0; j < nNumChans; j++)
        {
          baDOArray[j] = false;
        }

        doWriter.WriteSingleSampleMultiLine(true, baDOArray);
      }
      catch (NationalInstruments.DAQmx.DaqException e)
      {
        MainForm._mainform.PrintError("Error: " + e.Message + "\n");
      }
    }

    //Counter out function
    public void StartCounterOut(double freq, double duty)
    {
      if (coTask != null)
      {
        StopCounterOut();
      }

      //Initialize counter out
      idleState = COPulseIdleState.Low; //Changes the default (idle) state
      coTask = new Task();
      coFreq = freq;
      coDutyCycle = duty;
      coTask.COChannels.CreatePulseChannelFrequency(sDevLocal + "/ctr1", "ContinuousPulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, coFreq, coDutyCycle*0.01);
      coTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000);
      coTask.Control(TaskAction.Verify);
      coTask.Start();
    }

    public void Pulse(int nChan, int nPeriod) //period in ms
    {
      //if (doTask != null)
      //{
      //  doTask.Stop();
      //  doTask.WaitUntilDone();
      //  doTask.Dispose();
      //}

      //for (int j = 0; j < 8; j++)
      //{
      //  baDOArray[j] = false;
      //}

      //doTask = new Task();

      //doTask.DOChannels.CreateChannel(sDevLocal + "/port0/line0:" + sDevLocal + "/port0/line7", "", ChannelLineGrouping.OneChannelForAllLines);
      //doWriter = new DigitalSingleChannelWriter(doTask.Stream);
      //doWriter.WriteSingleSampleMultiLine(true, baDOArray);
      if (doTask != null)
      {
        baDOArray[nChan] = true;

        doWriter.WriteSingleSampleMultiLine(true, baDOArray);

        Thread.Sleep(nPeriod);

        baDOArray[nChan] = false;

        doWriter.WriteSingleSampleMultiLine(true, baDOArray);
      }
      else
      {
        MainForm._mainform.PrintError("Error:  Digital output not initialized!\n");
      }
    }

    //Counter stop function
    public void StopCounterOut()
    {
      coTask.Stop();
      coTask.WaitUntilDone();
      coTask.Dispose();
    }

    //Callback function for analog in
    private void AnalogInCallback(IAsyncResult ar)
    {
      if (runningTask == ar.AsyncState)
      {
        contData = aiReader.EndReadWaveform(ar);
        aiReader.BeginReadWaveform(iSamplesPerChannel, analogCallback, aiTask);
        aiData.Add(contData);
      }
    }
    
    //Starts the analog in function
    public void AIstart(params int[] channels)
    {
      dClockRate = 250000.0 / channels.Length;
      int i;

      channelsOn = channels;
      hashChannels = new Hashtable();

      for (i = 0; i < channels.Length; i++)
      {
        hashChannels.Add(channels[i], i);
      }

      if (aiTask != null)
      {
        aiTask.Stop();
        aiTask.Dispose();
        aiData.Clear();
      }
      aiTask = new Task();
      dArray = null;
      GC.Collect();

      aiData = new List<AnalogWaveform<double>[]>();
      
      //Create channel string (which channels are on)
      string channelString = "";
      for (i = 0; i < channels.Length - 1; i++)
      {
        channelString += (sDevLocal + "/ai" + channels[i].ToString() + ",");
      }
      channelString += (sDevLocal + "/ai" + channels[i].ToString());

      //Create the actual channels
      aiTask.AIChannels.CreateVoltageChannel(channelString, "", AITerminalConfiguration.Rse, -10.0, 10.0, AIVoltageUnits.Volts);

      //Configure sampling
      aiTask.Timing.ConfigureSampleClock("", dClockRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, iSamplesPerChannel);

      //Start task
      aiTask.Control(TaskAction.Verify);
      runningTask = aiTask;
      aiReader = new AnalogMultiChannelReader(aiTask.Stream);
      analogCallback = new AsyncCallback(AnalogInCallback);
      aiReader.SynchronizeCallbacks = true;
      aiReader.BeginReadWaveform(iSamplesPerChannel, analogCallback, aiTask);
    }
    
    //Sets a voltage with analog out
    public void SetV(double dVolts, params int[] channels)
    {
      for (int i = 0; i < channels.Length; i++)
      {
        daVolts[ channels[i] ] = dVolts;
      }

      aoWriter.WriteSingleSample(true, daVolts);
    }

    //Writes digital output
    public void DigOut(int nValue, params int[] channels)
    {
      for (int bit = 0; bit < channels.Length; bit++)
      {
        if (((nValue >> bit) & 0x01) == 1)
        {
          baDOArray[channels[channels.Length - 1 - bit]] = true;
        }
        else
        {
          baDOArray[channels[channels.Length - 1 - bit]] = false;
        }
      }
      doWriter.WriteSingleSampleMultiLine(true, baDOArray);
      //doTask.Stop();
      return;
    }

    //Stops analog in, doesn't return anything
    public void stop()
    {
      if (aiTask != null)
      {
        runningTask = null;
        aiTask.Stop();
        aiTask.WaitUntilDone();
        aiReader = null;
        aiTask.Dispose();
        aiTask = null;
        GC.Collect();
      }

      int totalSamples = aiData.Count * aiData[0][0].SampleCount;

      dArray = new double[channelsOn.Length, totalSamples];

      for (int a = 0; a < aiData[0].Length; a++)  //Num channels
      {
        for (int b = 0; b < aiData.Count; b++)    //Num sample groups
        {
          for (int c = 0; c < aiData[b][a].SampleCount; c++) //Num samples
          {
            dArray[a, (b * aiData[b][a].SampleCount + c)] = aiData[b][a].Samples[c].Value;
          }
        }
      }
    }
    /*
    //Stops analog in and returns a double[,] for scatter-plotting
    public double[,] stopArray()
    {
      stop();

      int totalSamples = aiData.Count * aiData[0][0].SampleCount;

      dArray = new double[channelsOn.Length, totalSamples];

      for (int a = 0; a < aiData[0].Length; a++)  //Num channels
      {
        for (int b = 0; b < aiData.Count; b++)    //Num sample groups
        {
          for (int c = 0; c < aiData[b][a].SampleCount; c++) //Num samples
          {
            dArray[a, (b * aiData[b][a].SampleCount + c)] = aiData[b][a].Samples[c].Value;
          }
        }
      }
      return dArray;
    }
    */
    //Stops analog in and returns the data for a given channel
    public double[] GetData(int nChannel)
    {
      stop();

      int nIndex = (int)hashChannels[nChannel];
      int nNumPts = 0;
      double[] dData;
      int nDataPt = 0;

      //Figure out how many points we have
      //Iterate thru the list of AnalogWaveforms
      for (int nListEntries = 0; nListEntries < aiData.Count; nListEntries++)
      {
        nNumPts += aiData[nListEntries][nIndex].SampleCount;
      }

      dData = new double[nNumPts];

      //Now get all the data out
      for (int nListEntries = 0; nListEntries < aiData.Count; nListEntries++)
      {
        for (int i = 0; i < aiData[nListEntries][nIndex].SampleCount; i++)
        {
          dData[nDataPt++] = aiData[nListEntries][nIndex].Samples[i].Value;
        }
      }

      return (dData);
    }

    //Outuputs an array to a CSV file
    /*private void outputToExcel(List<double>[] outList)
    {
      TextWriter tw = new StreamWriter("outData.csv");
      for (int i = 0; i < outList.Length; i++)
      {
        for (int j = 0; j < outList[i].Count; j++)
        {
          tw.Write(outList[i][j]);
          tw.Write(",");
        }
        tw.Write("\n");
      }
      tw.Close();
      return;
    }*/

    //Starts + stops the analog in function and returns an average over a given number of samples
    public double[] averageVoltage(int numSamples, bool bDifferential, params int[] channels)
    {
      dClockRate = 250000.0 / channels.Length;
      string channelString="";
      int nI;
      double[] daAverages = new double[channels.Length];
      Hashtable hashChannels = new Hashtable();
      Task aiAvgTask;
      AnalogMultiChannelReader aiAvgReader;

      //Init hash table
      for (nI = 0; nI < channels.Length; nI++)
      {
        hashChannels.Add(channels[nI], nI);
      }

      aiAvgTask = new Task();

      for (nI = 0; nI < channels.Length - 1; nI++)
      {
        channelString += (sDevLocal + "/ai" + channels[nI].ToString() + ",");
      }
      channelString += (sDevLocal + "/ai" + channels[nI].ToString());

      //Create the actual channels
      if (bDifferential)
      {
        aiAvgTask.AIChannels.CreateVoltageChannel(channelString, "", AITerminalConfiguration.Differential, -10.0, 10.0, AIVoltageUnits.Volts);
      }
      else
      {
        aiAvgTask.AIChannels.CreateVoltageChannel(channelString, "", AITerminalConfiguration.Rse, -10.0, 10.0, AIVoltageUnits.Volts);
      }

      aiAvgTask.Timing.ConfigureSampleClock("", dClockRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, numSamples);
      aiAvgReader = new AnalogMultiChannelReader(aiAvgTask.Stream);

      NationalInstruments.AnalogWaveform<double>[] newData = aiAvgReader.ReadWaveform(numSamples);

      aiAvgTask.Stop();
      aiAvgTask.WaitUntilDone();
      aiAvgTask.Dispose();
      aiAvgTask = null;

      double totalData = 0;

      for (int j = 0; j < channels.Length; j++)
      {
        for (int i = 0; i < numSamples; i++)
        {
          totalData += newData[(int)hashChannels[channels[j]]].Samples[i].Value;
        }
        daAverages[j] = totalData / numSamples;
        totalData = 0;
      }

      return daAverages;
    }

    //Sets the sample clock to a given freq.
    public void SetSampleClock(double dSampleClk)
    {
      if (dSampleClk > 250000)
      {
        dClockRate = 250000;
        MainForm._mainform.PrintWarning("Warning: Sample clock set to 250,000 (max.)\n");
      }
      else
      {
        if (dSampleClk < 10)
        {
          dClockRate = 10;
          MainForm._mainform.PrintWarning("Warning: Sample clock set to 10 (min.)\n");
        }
        else
        {
          dClockRate = dSampleClk;
        }
      }
    }

    #region IDisposable Members

    public void Dispose()
    {
      if (aoTask != null)
      {
        aoTask.Dispose();
      }

      if (aiTask != null)
      {
        aiTask.Dispose();
      }

      if (doTask != null)
      {
        doTask.Dispose();
      }

      if (coTask != null)
      {
        coTask.Dispose();
      }
    }

    #endregion
  }
}
