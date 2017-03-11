using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.DAQmx;
using NationalInstruments;

namespace SPRL.Test
{
  public class NI9403
  {
    private Task digitalWriteTask;
    public bool[] dataArray;
    private DigitalSingleChannelWriter writer;

    public NI9403(string sDev)
    {
      dataArray = new bool[32];
      digitalWriteTask = new Task();
      digitalWriteTask.DOChannels.CreateChannel(sDev + "/port0/line0:" + sDev + "/port0/line31", "", ChannelLineGrouping.OneChannelForAllLines);
      digitalWriteTask.Timing.SampleClockRate = 10000;

      for (int i = 0; i < 32; i++)
      {
        dataArray[i] = false;
      }
      writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
      writer.WriteSingleSampleMultiLine(true, dataArray);
    }

    public void dispose()
    {
      digitalWriteTask.Stop();
      digitalWriteTask.Dispose();
      digitalWriteTask = null;
    }

    public void setChannels(bool bStatus, params int[] channels)
    {
      if (digitalWriteTask != null)
      {
        for (int j = 0; j < channels.Length; j++)
        {
          dataArray[channels[j]] = bStatus;
        }
        writer.WriteSingleSampleMultiLine(true, dataArray);
        return;
      }
    }

    public void writeWord(int nWord, params int[] nChannels)
    {
      if (digitalWriteTask != null)
      {
        for (int bit = 0; bit < nChannels.Length; bit++)
        {
          if (((nWord >> bit) & 0x01) == 1)
            dataArray[nChannels[nChannels.Length - 1 - bit]] = true;
          else
            dataArray[nChannels[nChannels.Length - 1 - bit]] = false;
        }
        if (digitalWriteTask != null) writer.WriteSingleSampleMultiLine(true, dataArray);
      }
    }

    public void pulse(int nDuration, int channel)
    {
      //Change state
      dataArray[channel] = !dataArray[channel];
      writer.WriteSingleSampleMultiLine(true, dataArray);
      //Wait
      System.Threading.Thread.Sleep(nDuration);
      //Return to default state
      dataArray[channel] = !dataArray[channel];
      writer.WriteSingleSampleMultiLine(true, dataArray);
    }
  }
}
