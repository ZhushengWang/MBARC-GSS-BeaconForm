namespace SPRL.TestPXI
{
	partial class TSDiagForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TSDiagForm));
			this.DiagtextBox = new System.Windows.Forms.TextBox();
			this.DiagSaveButton = new System.Windows.Forms.Button();
			this.DiagCloseButton = new System.Windows.Forms.Button();
			this.DiagTimer = new System.Windows.Forms.Timer(this.components);
			this.DiagPauseButton = new System.Windows.Forms.Button();
			this.diagSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// DiagtextBox
			// 
			this.DiagtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.DiagtextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DiagtextBox.Location = new System.Drawing.Point(2, 2);
			this.DiagtextBox.Multiline = true;
			this.DiagtextBox.Name = "DiagtextBox";
			this.DiagtextBox.ReadOnly = true;
			this.DiagtextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.DiagtextBox.Size = new System.Drawing.Size(288, 235);
			this.DiagtextBox.TabIndex = 0;
			// 
			// DiagSaveButton
			// 
			this.DiagSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.DiagSaveButton.Location = new System.Drawing.Point(2, 241);
			this.DiagSaveButton.Name = "DiagSaveButton";
			this.DiagSaveButton.Size = new System.Drawing.Size(75, 23);
			this.DiagSaveButton.TabIndex = 1;
			this.DiagSaveButton.Text = "Save";
			this.DiagSaveButton.UseVisualStyleBackColor = true;
			this.DiagSaveButton.Click += new System.EventHandler(this.DiagSaveButton_Click);
			// 
			// DiagCloseButton
			// 
			this.DiagCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.DiagCloseButton.Location = new System.Drawing.Point(195, 241);
			this.DiagCloseButton.Name = "DiagCloseButton";
			this.DiagCloseButton.Size = new System.Drawing.Size(75, 23);
			this.DiagCloseButton.TabIndex = 1;
			this.DiagCloseButton.Text = "Close";
			this.DiagCloseButton.UseVisualStyleBackColor = true;
			this.DiagCloseButton.Click += new System.EventHandler(this.DiagCloseButton_Click);
			// 
			// DiagTimer
			// 
			this.DiagTimer.Interval = 1000;
			this.DiagTimer.Tick += new System.EventHandler(this.DiagTimer_Tick);
			// 
			// DiagPauseButton
			// 
			this.DiagPauseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.DiagPauseButton.Location = new System.Drawing.Point(100, 241);
			this.DiagPauseButton.Name = "DiagPauseButton";
			this.DiagPauseButton.Size = new System.Drawing.Size(75, 23);
			this.DiagPauseButton.TabIndex = 1;
			this.DiagPauseButton.Text = "Pause";
			this.DiagPauseButton.UseVisualStyleBackColor = true;
			this.DiagPauseButton.Click += new System.EventHandler(this.DiagPauseButton_Click);
			// 
			// diagSaveFileDialog
			// 
			this.diagSaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.diagSaveFileDialog_FileOk);
			// 
			// TSDiagForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.DiagCloseButton);
			this.Controls.Add(this.DiagPauseButton);
			this.Controls.Add(this.DiagSaveButton);
			this.Controls.Add(this.DiagtextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TSDiagForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TSDiagForm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TSDiagForm_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox DiagtextBox;
		private System.Windows.Forms.Button DiagSaveButton;
		private System.Windows.Forms.Button DiagCloseButton;
		private System.Windows.Forms.Timer DiagTimer;
		private System.Windows.Forms.Button DiagPauseButton;
		private System.Windows.Forms.SaveFileDialog diagSaveFileDialog;
	}
}