using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using NationalInstruments;
using NationalInstruments.UI.WindowsForms;
using NationalInstruments.UI;

namespace SPRL.Test
{
  public partial class WaveformPlot : Form
  {
    private double valueToShift;
    private int channelToShift;
    private int[] channelsOn;
    private List<AnalogWaveform<double>[]> rawData;
    private List<AnalogWaveform<double>[]> plotData;
    private decimal[] shiftTable;
    private Hashtable hashChannel;
    private int nTotalPoints;

    public WaveformPlot(List<AnalogWaveform<double>[]> data, int[] channels)
    {
      InitializeComponent();
      channelsOn = channels;
      shiftTable = new decimal[32];
      rawData = new List<AnalogWaveform<double>[]>();
      rawData = data;     //The raw data - this shouldn't be changed after this assignment
      plotData = new List<AnalogWaveform<double>[]>();

      //Copy rawData into plotData - the long way because the compiler is crazy
      #region copying...
      nTotalPoints = 0;
      for (int i = 0; i < rawData.Count; i++)
      {
        AnalogWaveform<double>[] tempArray = new AnalogWaveform<double>[channelsOn.Length];

        for (int j = 0; j < channelsOn.Length; j++)
        {
          AnalogWaveform<double> tempWaveform = new AnalogWaveform<double>(rawData[i][j].SampleCount);

          for (int k = 0; k < rawData[i][j].SampleCount; k++)
          {
            tempWaveform.Samples[k].Value = rawData[i][j].Samples[k].Value;
            nTotalPoints++;
          }

          tempArray[j] = tempWaveform;
        }
      
        plotData.Add(tempArray);
        //plotData[i] = new NationalInstruments.AnalogWaveform<double>(rawData[0].SampleCount);
        //for (int j = 0; j < rawData[i].SampleCount; j++)
        //{
        //  plotData[i].Samples[j].Value = rawData[i].Samples[j].Value;
        //}
      }
      #endregion
      hashChannel = new Hashtable();
      comboChannel.Items.Clear();

      if (nTotalPoints == 0)
      {
        MainForm._mainform.PrintError("Error: No data to plot!\n");
      }
      else
      {
        for (int i = 0; i < channelsOn.Length; i++)
        {
          hashChannel.Add(channelsOn[i], i);
          comboChannel.Items.Add(channelsOn[i]);
          //hashChannel[channel #] --> index as an obj
        }

        comboChannel.SelectedIndex = 0;

        readWaveform();
      }
    }

    private void readWaveform()
    {
      dateTimeLabel.Text = DateTime.Now.ToString();
      //Clear old junk
      waveformGraph.ClearData();
      waveformGraph.Plots.Clear();
      channelLegend.Items.Clear();
      waveformGraph.PlotAreaColor = Color.White;

      for (int i = 0; i < waveformGraph.Plots.Count; i++)
      {
        waveformGraph.Plots[i].HistoryCapacity = 1;
        waveformGraph.Plots[i].Dispose();
      }

      #region not used
      //Combine all list elements into a single AnalogWaveform

      //First figure out how many samples we have
      //for (int i = 0; i < plotData.Count; i++)
      //{
      //  nTotalSamples += plotData[i][0].SampleCount;
      //}

      //// Create the final AnalogWaveform
      //AnalogWaveform<double> awAllData = new AnalogWaveform<double>(nTotalSamples);

      ////Copy the samples from each list element
      //for (int i = 0; i < plotData.Count; i++)
      //{
      //  AnalogWaveform.CopySamples(
      //  AnalogWaveform.CopySamples(plotData[i], 0, awAllData, nIndexplotData[i][0].SampleCount);
      //  nIndex += plotData[i][0].SampleCount;
      //}
      #endregion

      AnalogWaveformPlotOptions plotOptions = new AnalogWaveformPlotOptions(AnalogWaveformPlotDisplayMode.Samples,
                                                                            AnalogWaveformPlotScaleMode.Scaled,
                                                                            AnalogWaveformPlotTimingMode.Auto);
      waveformGraph.XAxes[0].MajorDivisions.GridVisible = true;
      waveformGraph.XAxes[0].MajorDivisions.GridColor = Color.Gray;
      waveformGraph.XAxes[0].MinorDivisions.GridVisible = true;
      waveformGraph.XAxes[0].MinorDivisions.GridColor = Color.LightGray;
      waveformGraph.YAxes[0].MajorDivisions.GridVisible = true;
      waveformGraph.YAxes[0].MajorDivisions.GridColor = Color.Gray;
      waveformGraph.YAxes[0].MinorDivisions.GridVisible = true;
      waveformGraph.YAxes[0].MinorDivisions.GridColor = Color.LightGray;

      for (int i = 0; i < channelsOn.Length; i++)
      {
        NationalInstruments.UI.WaveformPlot plot = new NationalInstruments.UI.WaveformPlot();

        plot.DefaultWaveformPlotOptions = plotOptions;
        plot.HistoryCapacity = nTotalPoints;// 1000000;
        plot.LineWidth = 2;

        waveformGraph.Plots.Add(plot);
        plot.LineColor = plotColors[i % plotColors.Length];
        channelLegend.Items.Add(new LegendItem(plot, "Ch" + channelsOn[i].ToString()));
      }

      for (int i = 0; i < plotData.Count; i++)
      {
        waveformGraph.PlotWaveformsAppend<double>(plotData[i]);
      }
    }

    private void shiftValue_ValueChanged(object sender, EventArgs e)
    {
      shiftPlot((int)comboChannel.SelectedItem, shiftValue.Value);       //Shift the plot 
      shiftTable[(int)comboChannel.SelectedItem] = shiftValue.Value;     //Store the shift value in an array
    }

    public void shiftPlot(decimal nChan, decimal dShiftValue)
    {
      //Re-cast values
      try
      {
        channelToShift = (int)hashChannel[(int)nChan];
      }
      catch
      {
        return;
      }
      valueToShift = (double)dShiftValue;

      for (int i = 0; i < plotData.Count; i++)
      {
        for (int j = 0; j < plotData[i][channelToShift].SampleCount; j++)
        {
          plotData[i][channelToShift].Samples[j].Value = rawData[i][channelToShift].Samples[j].Value + valueToShift;
        }
      }
      readWaveform();
    }

    private void comboChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
      shiftValue.Value = shiftTable[(int)comboChannel.SelectedIndex];  //Set the displayed shift value to the one in the array
      shiftPlot((int)comboChannel.SelectedItem, shiftValue.Value);    //Shift the plot
    }

    Bitmap memoryImage;

    private void printButton_Click(object sender, EventArgs e)
    {
      CaptureScreen();
      //Print preview:
      PrintPreviewDialog ppd = new PrintPreviewDialog();
      ppd.Document = new PrintDocument();
      ppd.Document.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
      ppd.ShowDialog();
      //Print:
      //PrintDocument printDocument1 = new PrintDocument();
      //printDocument1.PrintPage +=new PrintPageEventHandler(printDocument1_PrintPage);
      //printDocument1.Print();
    }

    private void copyToClipboard_Click(object sender, EventArgs e)
    {
      CaptureScreen();
      memoryImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
      Clipboard.SetImage(memoryImage);
    }

    private void CaptureScreen()
    {
      Graphics myGraphics = this.CreateGraphics();
      Size s = this.Size;
      memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
      Graphics memoryGraphics = Graphics.FromImage(memoryImage);
      memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
      memoryImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
    }

    private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
    {
      e.Graphics.DrawImage(memoryImage, 0, 0);
    }

    private void WaveformPlot_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Dispose();
      GC.Collect();
    }
  }
}
