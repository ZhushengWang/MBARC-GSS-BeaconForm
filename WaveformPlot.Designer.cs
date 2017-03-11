using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace SPRL.Test
{
  partial class WaveformPlot
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.channelLegend = new NationalInstruments.UI.WindowsForms.Legend();
      this.waveformGraph = new NationalInstruments.UI.WindowsForms.WaveformGraph();
      this.waveformPlot1 = new NationalInstruments.UI.WaveformPlot();
      this.xAxis1 = new NationalInstruments.UI.XAxis();
      this.yAxis1 = new NationalInstruments.UI.YAxis();
      this.shiftValue = new System.Windows.Forms.NumericUpDown();
      this.ChannelLabel = new System.Windows.Forms.Label();
      this.OffsetLabel = new System.Windows.Forms.Label();
      this.comboChannel = new System.Windows.Forms.ComboBox();
      this.dateTimeLabel = new System.Windows.Forms.Label();
      this.printButton = new System.Windows.Forms.Button();
      this.copyToClipboard = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.channelLegend)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.waveformGraph)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.shiftValue)).BeginInit();
      this.SuspendLayout();
      // 
      // channelLegend
      // 
      this.channelLegend.Location = new System.Drawing.Point(1, 1);
      this.channelLegend.Name = "channelLegend";
      this.channelLegend.Size = new System.Drawing.Size(149, 368);
      this.channelLegend.TabIndex = 3;
      this.channelLegend.TabStop = false;
      // 
      // waveformGraph
      // 
      this.waveformGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.waveformGraph.Location = new System.Drawing.Point(156, 1);
      this.waveformGraph.Name = "waveformGraph";
      this.waveformGraph.PlotAreaColor = System.Drawing.Color.White;
      this.waveformGraph.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.waveformPlot1});
      this.waveformGraph.Size = new System.Drawing.Size(639, 398);
      this.waveformGraph.TabIndex = 0;
      this.waveformGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis1});
      this.waveformGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis1});
      // 
      // waveformPlot1
      // 
      this.waveformPlot1.HistoryCapacity = 10000000;
      this.waveformPlot1.XAxis = this.xAxis1;
      this.waveformPlot1.YAxis = this.yAxis1;
      // 
      // xAxis1
      // 
      this.xAxis1.Range = new NationalInstruments.UI.Range(0, 250000);
      // 
      // shiftValue
      // 
      this.shiftValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.shiftValue.DecimalPlaces = 1;
      this.shiftValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.shiftValue.Location = new System.Drawing.Point(77, 404);
      this.shiftValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.shiftValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
      this.shiftValue.Name = "shiftValue";
      this.shiftValue.Size = new System.Drawing.Size(74, 20);
      this.shiftValue.TabIndex = 4;
      this.shiftValue.ValueChanged += new System.EventHandler(this.shiftValue_ValueChanged);
      // 
      // ChannelLabel
      // 
      this.ChannelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ChannelLabel.AutoSize = true;
      this.ChannelLabel.Location = new System.Drawing.Point(4, 386);
      this.ChannelLabel.Name = "ChannelLabel";
      this.ChannelLabel.Size = new System.Drawing.Size(46, 13);
      this.ChannelLabel.TabIndex = 6;
      this.ChannelLabel.Text = "Channel";
      // 
      // OffsetLabel
      // 
      this.OffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.OffsetLabel.AutoSize = true;
      this.OffsetLabel.Location = new System.Drawing.Point(74, 386);
      this.OffsetLabel.Name = "OffsetLabel";
      this.OffsetLabel.Size = new System.Drawing.Size(35, 13);
      this.OffsetLabel.TabIndex = 7;
      this.OffsetLabel.Text = "Offset";
      // 
      // comboChannel
      // 
      this.comboChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.comboChannel.FormattingEnabled = true;
      this.comboChannel.Location = new System.Drawing.Point(5, 402);
      this.comboChannel.Name = "comboChannel";
      this.comboChannel.Size = new System.Drawing.Size(45, 21);
      this.comboChannel.TabIndex = 8;
      this.comboChannel.SelectedIndexChanged += new System.EventHandler(this.comboChannel_SelectedIndexChanged);
      // 
      // dateTimeLabel
      // 
      this.dateTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.dateTimeLabel.Location = new System.Drawing.Point(656, 404);
      this.dateTimeLabel.Name = "dateTimeLabel";
      this.dateTimeLabel.Size = new System.Drawing.Size(125, 20);
      this.dateTimeLabel.TabIndex = 9;
      // 
      // printButton
      // 
      this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.printButton.Location = new System.Drawing.Point(157, 399);
      this.printButton.Name = "printButton";
      this.printButton.Size = new System.Drawing.Size(38, 23);
      this.printButton.TabIndex = 10;
      this.printButton.Text = "Print";
      this.printButton.UseVisualStyleBackColor = true;
      this.printButton.Click += new System.EventHandler(this.printButton_Click);
      // 
      // copyToClipboard
      // 
      this.copyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.copyToClipboard.Location = new System.Drawing.Point(201, 399);
      this.copyToClipboard.Name = "copyToClipboard";
      this.copyToClipboard.Size = new System.Drawing.Size(111, 23);
      this.copyToClipboard.TabIndex = 11;
      this.copyToClipboard.Text = "Copy to Clipboard";
      this.copyToClipboard.UseVisualStyleBackColor = true;
      this.copyToClipboard.Click += new System.EventHandler(this.copyToClipboard_Click);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(485, 406);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(82, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Sample Number";
      // 
      // WaveformPlot
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(793, 424);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.copyToClipboard);
      this.Controls.Add(this.printButton);
      this.Controls.Add(this.dateTimeLabel);
      this.Controls.Add(this.comboChannel);
      this.Controls.Add(this.OffsetLabel);
      this.Controls.Add(this.ChannelLabel);
      this.Controls.Add(this.shiftValue);
      this.Controls.Add(this.waveformGraph);
      this.Controls.Add(this.channelLegend);
      this.Name = "WaveformPlot";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "Waveform Plot";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WaveformPlot_FormClosed);
      ((System.ComponentModel.ISupportInitialize)(this.channelLegend)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.waveformGraph)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.shiftValue)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private NationalInstruments.UI.WindowsForms.WaveformGraph waveformGraph;
    private NationalInstruments.UI.WaveformPlot waveformPlot1;
    private NationalInstruments.UI.XAxis xAxis1;
    private NationalInstruments.UI.YAxis yAxis1;
    private Color [] plotColors = {Color.Green, Color.Brown, Color.Aqua,
                                       Color.Yellow, Color.DarkMagenta, Color.Beige, Color.Crimson,
                                       Color.BurlyWood, Color.DarkTurquoise, Color.Blue};
    private NationalInstruments.UI.WindowsForms.Legend channelLegend;
    private NumericUpDown shiftValue;
    private Label ChannelLabel;
    private Label OffsetLabel;
    private ComboBox comboChannel;
    private Label dateTimeLabel;
    private Button printButton;
    private Button copyToClipboard;
    private Label label1;
  }
}