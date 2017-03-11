namespace SPRL.Test
{
  partial class RSimForm
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
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.listBoxSpare1 = new System.Windows.Forms.ListBox();
      this.listBoxSpare2 = new System.Windows.Forms.ListBox();
      this.listBoxSamples = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridView1
      // 
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.ColumnHeadersVisible = false;
      this.dataGridView1.Location = new System.Drawing.Point(1, 1);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.Size = new System.Drawing.Size(555, 288);
      this.dataGridView1.TabIndex = 0;
      // 
      // listBoxSpare1
      // 
      this.listBoxSpare1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.listBoxSpare1.FormattingEnabled = true;
      this.listBoxSpare1.Location = new System.Drawing.Point(1, 317);
      this.listBoxSpare1.Name = "listBoxSpare1";
      this.listBoxSpare1.ScrollAlwaysVisible = true;
      this.listBoxSpare1.Size = new System.Drawing.Size(120, 264);
      this.listBoxSpare1.TabIndex = 1;
      // 
      // listBoxSpare2
      // 
      this.listBoxSpare2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.listBoxSpare2.FormattingEnabled = true;
      this.listBoxSpare2.Location = new System.Drawing.Point(127, 317);
      this.listBoxSpare2.Name = "listBoxSpare2";
      this.listBoxSpare2.ScrollAlwaysVisible = true;
      this.listBoxSpare2.Size = new System.Drawing.Size(120, 264);
      this.listBoxSpare2.TabIndex = 2;
      // 
      // listBoxSamples
      // 
      this.listBoxSamples.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxSamples.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listBoxSamples.FormattingEnabled = true;
      this.listBoxSamples.HorizontalScrollbar = true;
      this.listBoxSamples.ItemHeight = 14;
      this.listBoxSamples.Location = new System.Drawing.Point(253, 316);
      this.listBoxSamples.Name = "listBoxSamples";
      this.listBoxSamples.ScrollAlwaysVisible = true;
      this.listBoxSamples.Size = new System.Drawing.Size(303, 256);
      this.listBoxSamples.TabIndex = 3;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(1, 300);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(49, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Spares1:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(128, 300);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(49, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Spares2:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(253, 299);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(77, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Bus1 Samples:";
      // 
      // RSimForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ClientSize = new System.Drawing.Size(558, 583);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.listBoxSamples);
      this.Controls.Add(this.listBoxSpare2);
      this.Controls.Add(this.listBoxSpare1);
      this.Controls.Add(this.dataGridView1);
      this.Name = "RSimForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "RSimForm";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RSimForm_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.ListBox listBoxSpare1;
    private System.Windows.Forms.ListBox listBoxSpare2;
    private System.Windows.Forms.ListBox listBoxSamples;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
  }
}