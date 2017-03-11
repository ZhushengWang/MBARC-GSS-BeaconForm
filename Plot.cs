using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using AxDATAQSDKLib;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace SPRL.Test
{
  public class Plot : System.Windows.Forms.Form
  {
    private NationalInstruments.UI.XAxis xAxis1;
    private NationalInstruments.UI.YAxis yAxis1;
    private NationalInstruments.UI.WaveformPlot waveformPlot1;
    private System.Windows.Forms.Button startButton;
    private System.Windows.Forms.Button stopButton;
    private System.Windows.Forms.ComboBox taskComboBox;
    private Task runningTask;
    private Task continuousTask;
    private AnalogMultiChannelReader reader;
    private AsyncCallback callBack;
    private static readonly Color[] plotColors = {Color.Green, Color.Brown, Color.Aqua,
                                                       Color.Yellow, Color.DarkMagenta, Color.Beige, Color.Crimson,
                                                       Color.BurlyWood, Color.DarkTurquoise, Color.Blue};
    private System.Windows.Forms.GroupBox

    public Plot()
    {
    }
  }
}
