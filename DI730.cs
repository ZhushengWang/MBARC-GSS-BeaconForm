using System;
using System.Collections.Generic;
using System.Text;
using AxDATAQSDKLib;
using System.IO;
using System.Threading;

namespace SPRL.Test
{
  public class DI730 : IDisposable
  {
    public AxDataqSdk dataq;
    private object oData;
    private int nCount = 1000;
    private short[] asGain;
    private short[] asChan;
    public int nSampleRate = 1000;
    private List<object> lArray;
    public int[] channelsOn;
    public double[,] outData;
    private double dAverage;
    private DateTime startTime;
    private bool bIsRunning = false;
    //Debugging:
    private DateTime timeNow;
    private DateTime timeThen;
    System.Timers.Timer timer = new System.Timers.Timer(1000);
 
    #region IDisposable Members
    public void  Dispose()
    {
      dataq.Dispose();
    }
    #endregion

    public DI730(int nDevID)
    {
      timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
      timer.AutoReset = false;
      
      asGain = new short[8];
      asChan = new short[8];
      
      lArray = new List<object>();

      timer.Enabled = true;
      timer.Start();

      MainForm._mainform.Invoke((System.Windows.Forms.MethodInvoker)delegate
      {
        dataq = new AxDataqSdk();
        dataq.CreateControl();
        dataq.Parent = null;
      });

      timer.Stop();

      dataq.ControlError += new _DDataqSdkEvents_ControlErrorEventHandler(dataq_ControlError);
      
      dataq.DeviceDriver = "DI72"+nDevID.ToString()+"NT.dll";
      dataq.DeviceID = nDevID.ToString();
      dataq.NewData += new _DDataqSdkEvents_NewDataEventHandler(dataq_NewData);

      timer.Start();
      if (AverageInit(1, 100) == -1.0)
      {
        timer.Stop();
        throw (new Exception("Could not initialize module " + nDevID.ToString() + "."));
      }
      timer.Stop();
    }

    void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      timer.Stop();
      throw (new Exception("Initialization timed out."));
    }

    private void dataq_ControlError(object sender, _DDataqSdkEvents_ControlErrorEvent e)
    {
      throw (new Exception("Control error: " + e.code.ToString()));
    }

    private void dataq_NewData(object sender, _DDataqSdkEvents_NewDataEvent e)
    {
      DateTime nowTime = DateTime.Now;
      if (new TimeSpan(startTime.Ticks - nowTime.Ticks).Seconds >= 20)
      {
        dataq.Stop();
        throw (new Exception("Error: HVADC callback ran over 20s."));
      }
      else
      {
        oData = dataq.GetData();
        lArray.Add(oData);
      }
    }

    public void Start(params int[] nChannelList)
    {
      if (bIsRunning)
      {
        dataq.Stop();
        bIsRunning = false;
      }
      startTime = DateTime.Now;
      lArray = new List<object>();
      short[] asGainList = new short[8];
      short[] asMethodList = new short[8];
      channelsOn = nChannelList;

      //Initialize the gain and channel lists
      for (int n = 0; n < nChannelList.Length; n++)
      {
        asGainList[n] = asGain[ nChannelList[n] - 1];
        asChan[n] = (short)(nChannelList[n] - 1);
        asMethodList[n] = 1;
      }

      //DATAQ settings:
      dataq.ADChannelCount = (short)nChannelList.Length;       //set the number of channels
      dataq.ADChannelList(asChan);                            //set the per-channel gain
      dataq.ADGainList(asGainList);                          //set the list of channels
      dataq.ADMethodList(asMethodList);                     //set the method to average
      dataq.MaxBurstRate = 250000;                         //set the max burst rate
      SetSampleRate(nSampleRate);                         //set the sample rate
      dataq.EventPoint = nCount;                         //set num samples per callback
      dataq.Start();                                    //start acquiring data
      bIsRunning = true;
    }
    public void setChanList(params int[] chanList)
    {
      short[] asChanList = new short[chanList.Length];
      
      for (int i = 0; i < chanList.Length; i++)
      {
        asChanList[i] = (short)(chanList[i] - 1);
      }

      asChan = asChanList;

      dataq.ADChannelCount = (short)chanList.Length;
      dataq.ADChannelList(asChanList);
    }

    public void setGainList(params double[] voltsList)
    {
      short[] gainList = new short[voltsList.Length];
      
      for (int i = 0; i < voltsList.Length; i++)
      {
        if (voltsList[i] <= 0.01) gainList[i] = 3;
        else if (voltsList[i] <= 0.1) gainList[i] = 2;
        else if (voltsList[i] <= 1) gainList[i] = 1;
        else if (voltsList[i] <= 10) gainList[i] = 0;
        else if (voltsList[i] <= 100) gainList[i] = 5;
        else if (voltsList[i] <= 1000) gainList[i] = 4;
        else MainForm._mainform.PrintError("Error: bad gain\n");
      }

      asGain = gainList;

      dataq.ADGainList(asGain);
    }

    public double AverageInit(int channel, int numSamples)
    {
      timeThen = DateTime.Now;
      lArray = new List<object>();
      dAverage = 0.0;

      short[] asAvgChan = new short[1];
      asAvgChan[0] = (short)(channel - 1);

      short[] asAvgGain = new short[1];
      asAvgGain[0] = asGain[channel - 1];

      short[] asAvgMethod = new short[1];
      asAvgMethod[0] = 1;

      dataq.ADChannelCount = 1;
      dataq.ADChannelList(asAvgChan);
      dataq.ADGainList(asAvgGain);
      dataq.ADMethodList(asAvgMethod);
      dataq.MaxBurstRate = 250000;
      SetSampleRate(nSampleRate);
      dataq.EventPoint = 0;
      dataq.Start();
      bIsRunning = true;
      int numLoops = 0;
      try
      {
        while (dataq.AvailableData < numSamples)
        {
          if ((numLoops > 20000) && (dataq.AvailableData == 0))
          {
            throw (new Exception("Could not acquire data."));
          }
          Thread.Sleep(0);
          System.Windows.Forms.Application.DoEvents();
          numLoops++;
        }
      }
      catch (Exception e)
      {
        throw (e);
      }

      oData = dataq.GetData();
      lArray.Add(oData);
      dataq.Stop();
      bIsRunning = false;

      timeNow = DateTime.Now;
      if (oData == null)
        return -1.0;
      else
        return AverageMath(channel - 1);
    }

    public double AverageMath(int channel)
    {
      double dSum = 0.0;
      double dMaxVolts = 1.0;
      double count = 0.0;

      for (int j = 0; j < lArray.Count; j++)
      {
        short[,] asData = (short[,])lArray[j];

        dMaxVolts = GetMaxVolts(asGain[channel]);
        for (int i = 0; i < asData.GetLength(1); i++)
        {
          dSum += convertVolts(asData[0, i], channel);
          count++;
        }
      }
      dAverage = dSum / count;
      //MainForm._mainform.PrintMsg("Average of " + dAverage.ToString() + "\n" + "Over " + count.ToString() + " samples.\n");
      return dAverage;
    }

    public double[] AverageArrayInit(int numSamples, params int[] channels)
    {
      timeThen = DateTime.Now;
      lArray = new List<object>();
      dAverage = 0.0;

      short[] asAvgChan = new short[channels.Length];
      short[] asAvgGain = new short[channels.Length];
      short[] asAvgMethod = new short[channels.Length];

      for (int i = 0; i < channels.Length; i++)
      {
        asAvgChan[i] = (short)(channels[i]-1);
        asAvgGain[i] = asGain[(channels[i]-1)];
        asAvgMethod[i] = 1;
      }

      dataq.ADChannelCount = (short)channels.Length;
      dataq.ADChannelList(asAvgChan);
      dataq.ADGainList(asAvgGain);
      dataq.ADMethodList(asAvgMethod);
      dataq.MaxBurstRate = nSampleRate;
      SetSampleRate(nSampleRate);
      dataq.EventPoint = 0;
      dataq.Start();
      bIsRunning = true;

      int numLoops = 0;

      while (dataq.AvailableData < numSamples*channels.Length)
      {
        numLoops++;
        Thread.Sleep(0);
        System.Windows.Forms.Application.DoEvents();
      }

      oData = dataq.GetData();
      lArray.Add(oData);
      dataq.Stop();
      bIsRunning = false;

      timeNow = DateTime.Now;
      return AverageArrayMath(channels);
    }

    public double[] AverageArrayMath(int[] channels)
    {
      double dSum = 0.0;
      double dMaxVolts = 1.0;
      double count = 0.0;
      double[] daAverage = new double[channels.Length];

      for (int m = 0; m < channels.Length; m++)
      {
        dSum = 0.0;
        count = 0.0;
        for (int j = 0; j < lArray.Count; j++)
        {
          short[,] asData = (short[,])lArray[j];
          if (asData != null)
          {
            dMaxVolts = GetMaxVolts(asGain[(channels[m] - 1)]);
            for (int i = 0; i < asData.GetLength(1); i++)
            {
              dSum += convertVolts(asData[m, i], (channels[m] - 1));
              count++;
            }
          }
          else
          {
            break;
          }
        }
        daAverage[m] = dSum / count;
      }
      return daAverage;
    }

    private double convertVolts(double voltage, int channel) 
    {
      return voltage * GetMaxVolts(asGain[channel]) / 32768.0;
    }

    public double[,] stopArray()
    {
      int i=0;
      int j=0;
      int k=0;
      double dMaxVolts = 1.0;
      int nTotalSamples = 0;

      dataq.Stop();
      bIsRunning = false;
      for (j = 0; j < lArray.Count; j++)
      {
        short[,] asData = (short[,])lArray[j];
        nTotalSamples += asData.GetLength(1);
      }

      outData = new double[channelsOn.Length, nTotalSamples];

      int nDataLengthPrev = 0;

      for (k = 0; k < channelsOn.Length; k++)
      {
        dMaxVolts = GetMaxVolts(asGain[k]);
        for (j = 0; j < lArray.Count; j++)
        {
          short[,] asData = (short[,])lArray[j];
          for (i = 0; i < asData.GetLength(1); i++)
          {
            outData[k, i + nDataLengthPrev] = convertVolts(asData[k, i], k);
          }
          nDataLengthPrev += asData.GetLength(1);
        }
        nDataLengthPrev = 0;
      }

      //OutputToExcel(outData);
      return outData;
    }

    private void OutputToExcel(double[,] data)
    {
      TextWriter tw = new StreamWriter("array.csv");
      for (int j = 0; j < data.GetLength(0); j++)
      {
        for (int i = 0; i < data.GetLength(1); i++)
        {
          tw.Write(data[j, i]);
          tw.Write(",");
        }
        tw.Write("\n");
      }
      tw.Close();
      return;
    }

    public void SetGain(int nChannel, short sGain)
    {
      asGain[nChannel] = sGain;
      short[] asLocalChan = { (short)nChannel };
      short[] asLocalGain = { sGain };

      dataq.ADChannelList(asLocalChan);
      dataq.ADGainList(asLocalGain);
    }

    public void SetSampleRate(int SampleRate)
    {
      nSampleRate = SampleRate;
      dataq.SampleRate = (double)nSampleRate; //attempt to set the sample rate, has to be a multiple of channel count
    }

    private double GetMaxVolts(short sGain)
    {
      double dMaxVolts;

      switch (sGain)
      {
        case 0:
          dMaxVolts = 10.0;
          break;

        case 1:
          dMaxVolts = 1.0;
          break;

        case 2:
          dMaxVolts = 0.1;
          break;

        case 3:
          dMaxVolts = 0.01;
          break;

        case 4:
          dMaxVolts = 1000.0;
          break;

        case 5:
          dMaxVolts = 100.0;
          break;

        default:
          dMaxVolts = 10.0;
          break;
      }

      return (dMaxVolts);

    }
  }
}
