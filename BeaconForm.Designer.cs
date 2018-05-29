namespace SPRL.Test
{
  partial class BeaconForm
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
        this.dataGridView2.RowTemplate.Height = 23;
        this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.None;
        this.dataGridView2.Size = new System.Drawing.Size(992, 459);
        this.dataGridView2.TabIndex = 0;
        // 
        // radioHex
        // 
        this.radioHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.radioHex.AutoSize = true;
        this.radioHex.Checked = true;
        this.radioHex.Location = new System.Drawing.Point(3, 465);
        this.radioHex.Name = "radioHex";
        this.radioHex.Size = new System.Drawing.Size(41, 16);
        this.radioHex.TabIndex = 1;
        this.radioHex.TabStop = true;
        this.radioHex.Text = "Hex";
        this.radioHex.UseVisualStyleBackColor = true;
        // 
        // radioVolts
        // 
        this.radioVolts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.radioVolts.AutoSize = true;
        this.radioVolts.Location = new System.Drawing.Point(50, 465);
        this.radioVolts.Name = "radioVolts";
        this.radioVolts.Size = new System.Drawing.Size(53, 16);
        this.radioVolts.TabIndex = 2;
        this.radioVolts.TabStop = true;
        this.radioVolts.Text = "Volts";
        this.radioVolts.UseVisualStyleBackColor = true;
        // 
        // radioEng
        // 
        this.radioEng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.radioEng.AutoSize = true;
        this.radioEng.Location = new System.Drawing.Point(104, 465);
        this.radioEng.Name = "radioEng";
        this.radioEng.Size = new System.Drawing.Size(41, 16);
        this.radioEng.TabIndex = 3;
        this.radioEng.TabStop = true;
        this.radioEng.Text = "Eng";
        this.radioEng.UseVisualStyleBackColor = true;
        // 
        // BeaconForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(993, 483);
        this.Controls.Add(this.radioEng);
        this.Controls.Add(this.radioVolts);
        this.Controls.Add(this.radioHex);
        this.Controls.Add(this.dataGridView2);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.Name = "BeaconForm";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.Text = "Beacon Data";
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
  }
}