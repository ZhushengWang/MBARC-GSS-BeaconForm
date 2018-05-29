using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.DAQmx;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments;

namespace SPRL.Test
{
  public class NI9205
  {
    private Task aiTask;
    private AnalogMultiChannelReader aiReader;
    public NationalInstruments.AnalogWaveform<double>[] data;
    public NationalInstruments.AnalogWaveform<double>[] contData;
    private AnalogMultiChannelReader analogInReader;
    private Task myTask;
    private Task runningTask;
    private Task aiAvgTask;
    private AsyncCallback analogCallback;
    public double dClockRate = 7800; //Max 250,000/32 = 7812.5
    private Int32 iSamplesPerChannel = 500;
    private string sDevLocal;
    public int[] channelsOn;
    public List< AnalogWaveform<double>[] > bigData;
    private Hashtable hashChannels;
    public double[,] dArray;
    public AITerminalConfiguration aiTermCfg = AITerminalConfiguration.Rse;

    public NI9205(string sDev)
    {
      sDevLocal = sDev;
      aiTask = new Task();
      //dataTable = new DataTable();
      aiTask.AIChannels.CreateVoltageChannel(sDev + "/ai0:" + sDev + "/ai31", "", aiTermCfg, -10.0, 10.0, AIVoltageUnits.Volts);

      aiTask.Timing.ConfigureSampleClock("", dClockRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, iSamplesPerChannel);

      aiTask.Control(TaskAction.Verify);
      aiReader = new AnalogMultiChannelReader(aiTask.Stream);

    }

    public void dispose()
    {
      if (aiTask != null)
      {
        aiTask.Stop();
        aiTask.Dispose();
        aiTask = null;
      }

      if (myTask != null)
      {
        myTask.Stop();
        myTask.Dispose();
        myTask = null;
      }

      if (runningTask != null)
      {
        runningTask.Stop();
        runningTask.Dispose();
        runningTask = null;
      }

      if (aiAvgTask != null)
      {
        aiAvgTask.Stop();
        aiAvgTask.Dispose();
        aiAvgTask = null;
      }
    }

    public double[] readVoltage(params int[] channels)
    {
      double[] instVoltage = new double[channels.Length];
      
      if (aiTask != null)
      {
        dClockRate = 250000.0 / channels.Length;
        if (aiTask != null) aiTask.Timing.ConfigureSampleClock("", dClockRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, iSamplesPerChannel);
        if (aiTask != null) data = aiReader.ReadWaveform(iSamplesPerChannel);

        for (int m = 0; m < channels.Length; m++)
        {
          instVoltage[m] = data[channels[m]].Samples[0].Value;
        }
      }
      return instVoltage;
    }
  
    public double[] averageVoltage(int numSamples, params int[] channels)
    {
      dClockRate = 50000.0 / channels.Length;
      int nI = 0;
      string channelString = "";
      double[] daAverages = new double[channels.Length];
      Hashtable hashChannels = new Hashtable();

      if (runningTask == null)
      {
        aiAvgTask = new Task();
        runningTask = aiAvgTask;

        AnalogMultiChannelReader aiAvgReader;

        for (nI = 0; nI < channels.Length - 1; nI++)
        {
          channelString += (sDevLocal + "/ai" + channels[nI].ToString() + ",");
        }
        channelString += (sDevLocal + "/ai" + channels[nI].ToString());

        aiAvgTask = new Task();
        aiAvgReader = new AnalogMultiChannelReader(aiAvgTask.Stream);
        aiAvgTask.AIChannels.CreateVoltageChannel(channelString, "", aiTermCfg, -10.0, 10.0, AIVoltageUnits.Volts);
        aiAvgTask.Timing.ConfigureSampleClock("", dClockRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, numSamples);
        AnalogWaveform<double>[] newData = aiAvgReader.ReadWaveform(numSamples);

        aiAvgTask.Stop();
        aiAvgTask.WaitUntilDone();
        aiAvgTask.Dispose();
        aiAvgTask = null;
        runningTask = null;

        double totalData = 0;

        //if(numSamples > data.Length) return 0;
        for (int j = 0; j < channels.Length; j++)
        {
          for (int i = 0; i < numSamples; i++)
          {
            totalData += newData[j].Samples[i].Value;
          }
          daAverages[j] = totalData / numSamples;
          totalData = 0;
        }
      }
      return daAverages;
    }

    public void start(params int[] channels)
    {
      int i;
      dClockRate = 250000.0 / channels.Length;
      channelsOn = channels;
      hashChannels = new Hashtable();

      for (i = 0; i < channels.Length; i++)
      {
        hashChannels.Add(channels[i], i);
      }

      if (runningTask == null)
      {
        myTask = new Task();

        //Create channel string (which channels are on)
        string channelString = "";
        for (i = 0; i < channels.Length-1; i++)
        {
          channelString += (sDevLocal + "/ai" + channels[i].ToString() + ",");
        }
        channelString += (sDevLocal + "/ai" + channels[channels.Length-1].ToString() );


        //Create the actual channels
        myTask.AIChannels.CreateVoltageChannel(channelString, "", aiTermCfg, -10.0, 10.0, AIVoltageUnits.Volts);
        //Configure sampling
        myTask.Timing.ConfigureSampleClock("", dClockRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, iSamplesPerChannel);
        //Start task
        myTask.Timing.SampleQuantityMode = SampleQuantityMode.ContinuousSamples;
        myTask.Control(TaskAction.Verify);
        runningTask = myTask;
        analogInReader = new AnalogMultiChannelReader(myTask.Stream);
        analogCallback = new AsyncCallback(AnalogInCallback);
        analogInReader.SynchronizeCallbacks = true;
        analogInReader.BeginReadWaveform(iSamplesPerChannel, analogCallback, myTask);   
        //Initialize bigData array
        bigData = new List< AnalogWaveform<double>[]>();
      }
    }

    private void AnalogInCallback(IAsyncResult ar)
    {
      if (runningTask == ar.AsyncState)
      {
        contData = analogInReader.EndReadWaveform(ar);
        analogInReader.BeginReadWaveform(iSamplesPerChannel, analogCallback, myTask);
        bigData.Add(contData);
      }
    }

    public void stop()
    {
      if (myTask != null)
      {
        runningTask = null;
        myTask.Stop();
        myTask.WaitUntilDone();
        myTask.Dispose();
        myTask = null;
      }
    }

    public double[,] stopArray()
    {
      if (myTask != null)
      {
        runningTask = null;
        myTask.Stop();
        myTask.WaitUntilDone();
        myTask.Dispose();
        myTask = null;
      }

      int totalSamples = bigData.Count * bigData[0][0].SampleCount;

      dArray = new double[channelsOn.Length, totalSamples];

      for (int a = 0; a < bigData[0].Length; a++)  //Num channels
      {
        for (int b = 0; b < bigData.Count; b++)    //Num sample groups
        {
          for (int c = 0; c < bigData[b][a].SampleCount; c++) //Num samples
          {
            dArray[a, (b * bigData[b][a].SampleCount + c)] = bigData[b][a].Samples[c].Value;
          }
        }
      }

#if debug 
      outputToExcel(bigData);
#endif 

      return dArray;
    }

    public double[] GetData(int nChannel)
    {
      if (myTask != null)
      {
        runningTask = null;
        myTask.Stop();
        myTask.WaitUntilDone();
        myTask.Dispose();
        myTask = null;
      }

      int nIndex = (int)hashChannels[nChannel];
      int nNumPts = 0;
      double[] dData;
      int nDataPt = 0;

      //Figure out how many points we have
      //Iterate thru the list of AnalogWaveforms
      for (int nListEntries = 0; nListEntries < bigData.Count; nListEntries++)
      {
        nNumPts += bigData[nListEntries][nIndex].SampleCount;
      }

      dData = new double[nNumPts];

      //Now get all the data out
      for (int nListEntries = 0; nListEntries < bigData.Count; nListEntries++)
      {
        for (int i = 0; i < bigData[nListEntries][nIndex].SampleCount; i++)
        {
          dData[nDataPt++] = bigData[nListEntries][nIndex].Samples[i].Value;
        }
      }

      return (dData);
    }

    /*private void outputToExcel(List<AnalogWaveform<double>[]> outList)
    {
      TextWriter tw = new StreamWriter("outData.csv");
      for (int a = 0; a < outList[0].Length; a++)  //Num channels
      {
        for (int b = 0; b < outList.Count; b++)    //Num sample groups
        {
          for (int c = 0; c < outList[0][0].SampleCount; c++) //Num samples
          {
            tw.Write(outList[b][a].Samples[c].Value.ToString() + ",");
          }
        }
        tw.Write("\n");
      }
      tw.Close();
      return;
    }*/
  }
}

