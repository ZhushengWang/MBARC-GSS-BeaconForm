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
  public partial class ScatterPlot : Form
  {
    private double valueToShift;
    private int channelToShift;
    private int[] channelsOn;
    //private List<AnalogWaveform<double>[]> rawData;
    private double[,] plotData;
    private double[,] oldPlotData;
    private double[] dXaxis;
    private decimal[] shiftTable;
    private Hashtable hashChannel;
    private int nTotalPoints;
    public bool bAxisTime = true;

    public ScatterPlot(double[,] data, int[] channels, double dSampleRate)
    {
      InitializeComponent();
      channelsOn = channels;
      shiftTable = new decimal[32];

      plotData = new double[data.GetLength(0), data.GetLength(1)];
      oldPlotData = new double[data.GetLength(0), data.GetLength(1)];

      for (int m = 0; m < data.GetLength(0); m++)
      {
        for (int n = 0; n < data.GetLength(1); n++)
        {
          oldPlotData[m, n] = data[m, n];
          plotData[m, n] = data[m, n];
        }
      }

      nTotalPoints = data.GetLength(1);

      #region Set up the x axis

      dXaxis = new double[nTotalPoints];

      for (int i = 0; i < nTotalPoints; i++)
      {
        if (bAxisTime)
        {
          dXaxis[i] = (double)(i / dSampleRate);
        }
        else
        {
          dXaxis[i] = i;
        }
      }

      if (bAxisTime)
      {
        this.xAxis1.Range = new NationalInstruments.UI.Range(0.0, nTotalPoints / dSampleRate);
      }
      else
      {
        this.xAxis1.Range = new NationalInstruments.UI.Range(0.0, (double)nTotalPoints);
      }

      #endregion

      hashChannel = new Hashtable();
      channelBox.Items.Clear();

      for (int i = 0; i < channelsOn.Length; i++)
      {
        hashChannel.Add(channelsOn[i], i);
        channelBox.Items.Add(channelsOn[i]);
        //hashChannel[channel #] --> index as an obj
      }

      channelBox.SelectedIndex = 0;
    }

    private void readScatterPlot()
    {
      #region initialization
      dateTimeLabel.Text = DateTime.Now.ToString();
      for (int i = 0; i < scatterGraph1.Plots.Count; i++)
      {
        scatterGraph1.Plots[i].HistoryCapacity = 1;
      }
      scatterGraph1.ClearData();
      scatterGraph1.Plots.Clear();
      legend1.Items.Clear();
      scatterGraph1.PlotAreaColor = Color.White;

      scatterGraph1.XAxes[0].Mode = AxisMode.AutoScaleExact;
      scatterGraph1.XAxes[0].ScaleType = ScaleType.Linear;
      scatterGraph1.XAxes[0].MajorDivisions.GridVisible = true;
      scatterGraph1.XAxes[0].MajorDivisions.GridColor = Color.Gray;
      scatterGraph1.XAxes[0].MinorDivisions.GridVisible = true;
      scatterGraph1.XAxes[0].MinorDivisions.GridColor = Color.LightGray;
      scatterGraph1.YAxes[0].MajorDivisions.GridVisible = true;
      scatterGraph1.YAxes[0].MajorDivisions.GridColor = Color.Gray;
      scatterGraph1.YAxes[0].MinorDivisions.GridVisible = true;
      scatterGraph1.YAxes[0].MinorDivisions.GridColor = Color.LightGray;
      #endregion

      for (int i = 0; i < channelsOn.Length; i++)
      {
        NationalInstruments.UI.ScatterPlot plot = new NationalInstruments.UI.ScatterPlot();

        //plot.DefaultWaveformPlotOptions = plotOptions;
        plot.HistoryCapacity = nTotalPoints;// 1000000;
        plot.LineWidth = 2;

        scatterGraph1.Plots.Add(plot);
        plot.LineColor = plotColors[i % plotColors.Length];
        legend1.Items.Add(new LegendItem(plot, "Ch" + channelsOn[i].ToString()));
      }
      //Actually plot the data
      scatterGraph1.PlotXYMultiple(dXaxis, plotData);
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

      for (int j = 0; j < plotData.GetLength(1); j++)
      {
        plotData[channelToShift, j] = oldPlotData[channelToShift, j] + valueToShift;
      }

      readScatterPlot();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      offsetUpDown.Value = shiftTable[(int)channelBox.SelectedIndex];  //Set the displayed shift value to the one in the array
      shiftPlot((int)channelBox.SelectedItem, offsetUpDown.Value);    //Shift the plot
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      shiftPlot((int)channelBox.SelectedItem, offsetUpDown.Value);       //Shift the plot 
      shiftTable[(int)channelBox.SelectedItem] = offsetUpDown.Value;     //Store the shift value in an array
    }

    private void button1_Click(object sender, EventArgs e)
    {
      CaptureScreen();
      memoryImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
      Clipboard.SetImage(memoryImage);
    }

    void Print1_Click(object sender, EventArgs e)
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

    Bitmap memoryImage;

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

    private void ScatterPlot_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Dispose();
      GC.Collect();
    }
  }
}
