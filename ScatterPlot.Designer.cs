using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace SPRL.Test
{
  partial class ScatterPlot
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
      this.scatterGraph1 = new NationalInstruments.UI.WindowsForms.ScatterGraph();
      this.scatterPlot1 = new NationalInstruments.UI.ScatterPlot();
      this.xAxis1 = new NationalInstruments.UI.XAxis();
      this.yAxis1 = new NationalInstruments.UI.YAxis();
      this.legend1 = new NationalInstruments.UI.WindowsForms.Legend();
      this.legendItem1 = new NationalInstruments.UI.LegendItem();
      this.channelBox = new System.Windows.Forms.ComboBox();
      this.offsetUpDown = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.dateTimeLabel = new System.Windows.Forms.Label();
      this.Print1 = new System.Windows.Forms.Button();
      this.copyToClip1 = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.scatterGraph1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.legend1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.offsetUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // scatterGraph1
      // 
      this.scatterGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.scatterGraph1.Border = NationalInstruments.UI.Border.None;
      this.scatterGraph1.Location = new System.Drawing.Point(198, 12);
      this.scatterGraph1.Name = "scatterGraph1";
      this.scatterGraph1.PlotAreaBorder = NationalInstruments.UI.Border.None;
      this.scatterGraph1.PlotAreaColor = System.Drawing.Color.White;
      this.scatterGraph1.Plots.AddRange(new NationalInstruments.UI.ScatterPlot[] {
            this.scatterPlot1});
      this.scatterGraph1.Size = new System.Drawing.Size(587, 419);
      this.scatterGraph1.TabIndex = 0;
      this.scatterGraph1.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis1});
      this.scatterGraph1.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis1});
      // 
      // scatterPlot1
      // 
      this.scatterPlot1.XAxis = this.xAxis1;
      this.scatterPlot1.YAxis = this.yAxis1;
      // 
      // legend1
      // 
      this.legend1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.legend1.Border = NationalInstruments.UI.Border.SunkenLite;
      this.legend1.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.legendItem1});
      this.legend1.Location = new System.Drawing.Point(12, 12);
      this.legend1.Name = "legend1";
      this.legend1.Size = new System.Drawing.Size(167, 395);
      this.legend1.TabIndex = 1;
      // 
      // legendItem1
      // 
      this.legendItem1.Text = "Item 0";
      // 
      // channelBox
      // 
      this.channelBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.channelBox.FormattingEnabled = true;
      this.channelBox.Location = new System.Drawing.Point(12, 437);
      this.channelBox.Name = "channelBox";
      this.channelBox.Size = new System.Drawing.Size(43, 21);
      this.channelBox.TabIndex = 2;
      this.channelBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // offsetUpDown
      // 
      this.offsetUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.offsetUpDown.DecimalPlaces = 1;
      this.offsetUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.offsetUpDown.Location = new System.Drawing.Point(81, 437);
      this.offsetUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.offsetUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
      this.offsetUpDown.Name = "offsetUpDown";
      this.offsetUpDown.Size = new System.Drawing.Size(65, 20);
      this.offsetUpDown.TabIndex = 3;
      this.offsetUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(10, 413);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(46, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Channel";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(78, 413);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(35, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Offset";
      // 
      // dateTimeLabel
      // 
      this.dateTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.dateTimeLabel.Location = new System.Drawing.Point(623, 442);
      this.dateTimeLabel.Name = "dateTimeLabel";
      this.dateTimeLabel.Size = new System.Drawing.Size(160, 16);
      this.dateTimeLabel.TabIndex = 6;
      // 
      // Print1
      // 
      this.Print1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.Print1.Location = new System.Drawing.Point(152, 435);
      this.Print1.Name = "Print1";
      this.Print1.Size = new System.Drawing.Size(40, 23);
      this.Print1.TabIndex = 7;
      this.Print1.Text = "Print";
      this.Print1.UseVisualStyleBackColor = true;
      this.Print1.Click += new System.EventHandler(this.Print1_Click);
      // 
      // copyToClip1
      // 
      this.copyToClip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.copyToClip1.Location = new System.Drawing.Point(198, 434);
      this.copyToClip1.Name = "copyToClip1";
      this.copyToClip1.Size = new System.Drawing.Size(104, 23);
      this.copyToClip1.TabIndex = 8;
      this.copyToClip1.Text = "Copy to Clipboard";
      this.copyToClip1.UseVisualStyleBackColor = true;
      this.copyToClip1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(488, 443);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(81, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Time (Seconds)";
      // 
      // ScatterPlot
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(784, 462);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.copyToClip1);
      this.Controls.Add(this.Print1);
      this.Controls.Add(this.dateTimeLabel);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.offsetUpDown);
      this.Controls.Add(this.channelBox);
      this.Controls.Add(this.legend1);
      this.Controls.Add(this.scatterGraph1);
      this.Name = "ScatterPlot";
      this.Text = "ScatterPlot";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScatterPlot_FormClosed);
      ((System.ComponentModel.ISupportInitialize)(this.scatterGraph1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.legend1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.offsetUpDown)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private NationalInstruments.UI.WindowsForms.ScatterGraph scatterGraph1;
    private NationalInstruments.UI.ScatterPlot scatterPlot1;
    private NationalInstruments.UI.XAxis xAxis1;
    private NationalInstruments.UI.YAxis yAxis1;
    private NationalInstruments.UI.WindowsForms.Legend legend1;
    private NationalInstruments.UI.LegendItem legendItem1;
    private System.Windows.Forms.ComboBox channelBox;
    private System.Windows.Forms.NumericUpDown offsetUpDown;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private Color[] plotColors = {Color.Green, Color.Brown, Color.Aqua,
                                       Color.Yellow, Color.DarkMagenta, Color.Violet, Color.Crimson,
                                       Color.BurlyWood, Color.DarkTurquoise, Color.Blue};
    private Label dateTimeLabel;
    private Button Print1;
    private Button copyToClip1;
    private Label label3;
  }
}