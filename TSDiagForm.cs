using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SPRL.TestPXI
{
	public partial class TSDiagForm : Form
	{
		private bool bPaused = false;

		public TSDiagForm()
		{
			InitializeComponent();
			DiagTimer.Interval = 1000;
			DiagTimer.Start();
		}

		private void TSDiagForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			MainForm._mainform.UnCheckDiagButton();
		}

		private void DiagSaveButton_Click(object sender, EventArgs e)
		{
			diagSaveFileDialog.ShowDialog();
		}

		private void DiagCloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		public void DisplayBytes(byte[] abBytes)
		{
			DiagtextBox.Invoke((MethodInvoker)delegate
      {
 				string sValue;

				if (!bPaused)
				{
					foreach(byte bValue in abBytes)
					{
						sValue = bValue.ToString("X2");
						DiagtextBox.AppendText(sValue + " ");
					}

					DiagTimer.Stop();
					DiagTimer.Start();
				}
      });
		}

		private void DiagTimer_Tick(object sender, EventArgs e)
		{
			DiagtextBox.AppendText(Environment.NewLine+Environment.NewLine);
			DiagTimer.Stop();
		}

		private void DiagPauseButton_Click(object sender, EventArgs e)
		{
			if (bPaused)
			{
			  DiagPauseButton.Text = "Pause";
				bPaused = false;
				DiagTimer.Start();
				DiagtextBox.AppendText(Environment.NewLine + Environment.NewLine);
			}
			else
			{
			  DiagPauseButton.Text = "Resume";
				bPaused = true;
				DiagTimer.Stop();
			}
		}

		private void diagSaveFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			string fname = diagSaveFileDialog.FileName;

			System.IO.File.WriteAllText(fname, DiagtextBox.Text);
		}
	}
}