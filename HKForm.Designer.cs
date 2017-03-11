namespace SPRL.Test
{
  partial class HKForm
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
      this.dataGridView2 = new System.Windows.Forms.DataGridView();
      this.radioHex = new System.Windows.Forms.RadioButton();
      this.radioVolts = new System.Windows.Forms.RadioButton();
      this.radioEng = new System.Windows.Forms.RadioButton();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridView2
      // 
      this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView2.ColumnHeadersVisible = false;
      this.dataGridView2.Location = new System.Drawing.Point(0, 0);
      this.dataGridView2.Name = "dataGridView2";
      this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.dataGridView2.Size = new System.Drawing.Size(992, 444);
      this.dataGridView2.TabIndex = 0;
      // 
      // radioHex
      // 
      this.radioHex.AutoSize = true;
      this.radioHex.Checked = true;
      this.radioHex.Location = new System.Drawing.Point(3, 450);
      this.radioHex.Name = "radioHex";
      this.radioHex.Size = new System.Drawing.Size(44, 17);
      this.radioHex.TabIndex = 1;
      this.radioHex.TabStop = true;
      this.radioHex.Text = "Hex";
      this.radioHex.UseVisualStyleBackColor = true;
      // 
      // radioVolts
      // 
      this.radioVolts.AutoSize = true;
      this.radioVolts.Location = new System.Drawing.Point(50, 450);
      this.radioVolts.Name = "radioVolts";
      this.radioVolts.Size = new System.Drawing.Size(48, 17);
      this.radioVolts.TabIndex = 2;
      this.radioVolts.TabStop = true;
      this.radioVolts.Text = "Volts";
      this.radioVolts.UseVisualStyleBackColor = true;
      // 
      // radioEng
      // 
      this.radioEng.AutoSize = true;
      this.radioEng.Location = new System.Drawing.Point(104, 450);
      this.radioEng.Name = "radioEng";
      this.radioEng.Size = new System.Drawing.Size(44, 17);
      this.radioEng.TabIndex = 3;
      this.radioEng.TabStop = true;
      this.radioEng.Text = "Eng";
      this.radioEng.UseVisualStyleBackColor = true;
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // HKForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(993, 470);
      this.Controls.Add(this.radioEng);
      this.Controls.Add(this.radioVolts);
      this.Controls.Add(this.radioHex);
      this.Controls.Add(this.dataGridView2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "HKForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "HK Data";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HKForm_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView2;
    private System.Windows.Forms.RadioButton radioHex;
    private System.Windows.Forms.RadioButton radioVolts;
    private System.Windows.Forms.RadioButton radioEng;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
  }
}