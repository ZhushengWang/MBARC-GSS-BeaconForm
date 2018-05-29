namespace SPRL.Test
{
  partial class SequencerForm
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
      this.components = new System.ComponentModel.Container();
      this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
      this.ackTextBox = new System.Windows.Forms.TextBox();
      this.AckLabel = new System.Windows.Forms.Label();
      this.hkTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // ackTextBox
      // 
      this.ackTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.ackTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ackTextBox.Location = new System.Drawing.Point(2, 30);
      this.ackTextBox.Multiline = true;
      this.ackTextBox.Name = "ackTextBox";
      this.ackTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.ackTextBox.Size = new System.Drawing.Size(103, 360);
      this.ackTextBox.TabIndex = 1;
      this.ackTextBox.WordWrap = false;
      // 
      // AckLabel
      // 
      this.AckLabel.AutoSize = true;
      this.AckLabel.Location = new System.Drawing.Point(-1, 9);
      this.AckLabel.Name = "AckLabel";
      this.AckLabel.Size = new System.Drawing.Size(31, 13);
      this.AckLabel.TabIndex = 2;
      this.AckLabel.Text = "ACK:";
      // 
      // hkTextBox
      // 
      this.hkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.hkTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hkTextBox.Location = new System.Drawing.Point(111, 30);
      this.hkTextBox.Multiline = true;
      this.hkTextBox.Name = "hkTextBox";
      this.hkTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.hkTextBox.Size = new System.Drawing.Size(434, 360);
      this.hkTextBox.TabIndex = 3;
      this.hkTextBox.WordWrap = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(108, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(25, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "HK:";
      // 
      // SequencerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(548, 393);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.hkTextBox);
      this.Controls.Add(this.AckLabel);
      this.Controls.Add(this.ackTextBox);
      this.Name = "SequencerForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "QB50";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SequencerForm_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.IO.Ports.SerialPort serialPort1;
    private System.Windows.Forms.TextBox ackTextBox;
    private System.Windows.Forms.Label AckLabel;
    private System.Windows.Forms.TextBox hkTextBox;
    private System.Windows.Forms.Label label1;
  }
}