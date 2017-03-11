using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPRL.Test
{
  partial class RSimForm : Form
  {
    private int nGoodCSCount = 0;
    private int nBadCSCount = 0;

    public RSimForm()
    {
      InitializeComponent();

      dataGridView1.Columns.Add("Col1", "Col1");
      dataGridView1.Columns.Add("Col2", "Col2");
      dataGridView1.Columns.Add("Col3", "Col3");
      dataGridView1.Columns.Add("Col4", "Col4");
      dataGridView1.Columns.Add("Col5", "Col5");
      dataGridView1.Columns.Add("Col6", "Col6");

      dataGridView1.Rows.Add(13);

      dataGridView1.RowHeadersVisible = false;
      dataGridView1.ColumnHeadersVisible = false;

      for (int i = 0; i < dataGridView1.Rows.Count; i++)
      {
        for (int j = 0; j < dataGridView1.Columns.Count; j++)
        {
          if ((j % 2) == 0)
          {
            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Silver;
          }
          dataGridView1.Rows[i].Cells[j].ReadOnly = true;
        }
      }

      for (int i = 0; i < dataGridView1.Columns.Count; i++)
      {
        dataGridView1.Columns[i].Width = 80;
      }

      dataGridView1.Rows[0].Cells[0].Value = "Packet ID";
      dataGridView1.Rows[1].Cells[0].Value = "Time Stamp";
      dataGridView1.Rows[2].Cells[0].Value = "SubSecs";
      dataGridView1.Rows[3].Cells[0].Value = "Pkt Size";
      dataGridView1.Rows[4].Cells[0].Value = "TM Pkt ID";
      dataGridView1.Rows[5].Cells[0].Value = "Pkt Seq Cnt";
      dataGridView1.Rows[6].Cells[0].Value = "Pkt Length";
      dataGridView1.Rows[7].Cells[0].Value = "TS (unused)";
      dataGridView1.Rows[8].Cells[0].Value = "FPGA Rev";
      dataGridView1.Rows[9].Cells[0].Value = "Rabbit Rev";
      dataGridView1.Rows[10].Cells[0].Value = "RS Cmd Cnt";
      dataGridView1.Rows[11].Cells[0].Value = "FPGA Cmd Cnt";
      dataGridView1.Rows[12].Cells[0].Value = "PS V Set";
      dataGridView1.Rows[13].Cells[0].Value = "PS I Set";

      dataGridView1.Rows[0].Cells[2].Value = "PS State";
      dataGridView1.Rows[1].Cells[2].Value = "PS Volts";
      dataGridView1.Rows[2].Cells[2].Value = "PS Current";
      dataGridView1.Rows[3].Cells[2].Value = "Bus1 I Set";
      dataGridView1.Rows[4].Cells[2].Value = "Bus2 I Set";
      dataGridView1.Rows[5].Cells[2].Value = "Bus3 I Set";
      dataGridView1.Rows[6].Cells[2].Value = "Bus1 State";
      dataGridView1.Rows[7].Cells[2].Value = "Bus2 State";
      dataGridView1.Rows[8].Cells[2].Value = "Bus3 State";
      dataGridView1.Rows[9].Cells[2].Value = "Temp";
      dataGridView1.Rows[10].Cells[2].Value = "Humid";
      dataGridView1.Rows[11].Cells[2].Value = "Hours";
      dataGridView1.Rows[12].Cells[2].Value = "Laser";
      dataGridView1.Rows[13].Cells[2].Value = "TC Cnt";

      dataGridView1.Rows[0].Cells[4].Value = "RSIM NAK";
      dataGridView1.Rows[1].Cells[4].Value = "FPGA NAK";
      dataGridView1.Rows[2].Cells[4].Value = "OC Mode";
      dataGridView1.Rows[3].Cells[4].Value = "OC Time";
      dataGridView1.Rows[4].Cells[4].Value = "Bus1 Avg";
      dataGridView1.Rows[5].Cells[4].Value = "Bus2 Avg";
      dataGridView1.Rows[6].Cells[4].Value = "Bus3 Avg";
      dataGridView1.Rows[7].Cells[4].Value = "Bus1 V";
      dataGridView1.Rows[8].Cells[4].Value = "Checksum";
      dataGridView1.Rows[9].Cells[4].Value = "CS Calc";
      dataGridView1.Rows[10].Cells[4].Value = "Bad CS Cnt";
      dataGridView1.Rows[11].Cells[4].Value = "Good CS Cnt";

      int height = 0;
      foreach (DataGridViewRow row in dataGridView1.Rows)
      {
        height += row.Height;
      }

      //int width = 0;
      //foreach (DataGridViewColumn col in dataGridView1.Columns)
      //{
      //  width += col.Width;
      //}

      dataGridView1.ClientSize = new Size(dataGridView1.ClientSize.Width, height + 3);
    }

    public void DisplayPacket(MOMARSimPkt rsimPkt)
    {
      dataGridView1.Rows[0].Cells[1].Value = rsimPkt.ui16PktID.ToString("X4");
      dataGridView1.Rows[1].Cells[1].Value = rsimPkt.ui32TimeStamp.ToString("X8");
      dataGridView1.Rows[2].Cells[1].Value = rsimPkt.ui16SubSecs.ToString("X4");
      dataGridView1.Rows[3].Cells[1].Value = rsimPkt.ui32PktSize.ToString("X8");
      dataGridView1.Rows[4].Cells[1].Value = rsimPkt.yTMPktID.ToString("X2");
      dataGridView1.Rows[5].Cells[1].Value = rsimPkt.ySeqCount.ToString("X2");
      dataGridView1.Rows[6].Cells[1].Value = rsimPkt.ui16PktLen.ToString("X4");
      dataGridView1.Rows[7].Cells[1].Value = rsimPkt.ui32TS.ToString("X8");
      dataGridView1.Rows[8].Cells[1].Value = rsimPkt.yFPGARev.ToString("X2");
      dataGridView1.Rows[9].Cells[1].Value = rsimPkt.yRabbitRev.ToString("X2");
      dataGridView1.Rows[10].Cells[1].Value = rsimPkt.ui16RSimCmdCount.ToString("X4");
      dataGridView1.Rows[11].Cells[1].Value = rsimPkt.ui16FPGACmdCount.ToString("X4");
      dataGridView1.Rows[12].Cells[1].Value = rsimPkt.fPSVoltSet.ToString("F3");
      dataGridView1.Rows[13].Cells[1].Value = rsimPkt.fPSCurSet.ToString("F3");

      dataGridView1.Rows[0].Cells[3].Value = rsimPkt.yPSOutState.ToString();
      dataGridView1.Rows[1].Cells[3].Value = rsimPkt.fPSVoltReading.ToString("F3");
      dataGridView1.Rows[2].Cells[3].Value = rsimPkt.fPSCurReading.ToString("F3");
      dataGridView1.Rows[3].Cells[3].Value = rsimPkt.fBus1CurSet.ToString("F3");
      dataGridView1.Rows[4].Cells[3].Value = rsimPkt.fBus2CurSet.ToString("F3");
      dataGridView1.Rows[5].Cells[3].Value = rsimPkt.fBus3CurSet.ToString("F3");
      dataGridView1.Rows[6].Cells[3].Value = rsimPkt.yBus1OutState.ToString();
      dataGridView1.Rows[7].Cells[3].Value = rsimPkt.yBus2OutState.ToString();
      dataGridView1.Rows[8].Cells[3].Value = rsimPkt.yBus3OutState.ToString();
      dataGridView1.Rows[9].Cells[3].Value = rsimPkt.fTemp.ToString("F2");
      dataGridView1.Rows[10].Cells[3].Value = rsimPkt.fHum.ToString("F2");
      dataGridView1.Rows[11].Cells[3].Value = rsimPkt.fHours.ToString("F2");
      dataGridView1.Rows[12].Cells[3].Value = rsimPkt.yLaserStat.ToString();
      dataGridView1.Rows[13].Cells[3].Value = rsimPkt.ui16TCCount.ToString("X4");

      dataGridView1.Rows[0].Cells[5].Value = rsimPkt.ui16RSIMNakCount.ToString("X4");
      dataGridView1.Rows[1].Cells[5].Value = rsimPkt.ui16FPGANAKCount.ToString("X4");
      dataGridView1.Rows[2].Cells[5].Value = rsimPkt.yOCTripMode.ToString();
      dataGridView1.Rows[3].Cells[5].Value = rsimPkt.yOCTripTime.ToString();
      dataGridView1.Rows[4].Cells[5].Value = rsimPkt.fBus1Avg.ToString("F3");
      dataGridView1.Rows[5].Cells[5].Value = rsimPkt.fBus2Avg.ToString("F3");
      dataGridView1.Rows[6].Cells[5].Value = rsimPkt.fBus3Avg.ToString("F3");
      dataGridView1.Rows[7].Cells[5].Value = rsimPkt.fBus1VAvg.ToString("F3");
      dataGridView1.Rows[8].Cells[5].Value = rsimPkt.ui32Checksum.ToString("X8");
      dataGridView1.Rows[9].Cells[5].Value = rsimPkt.ui32CalculatedChecksum.ToString("X8");

      if (rsimPkt.ui32Checksum == rsimPkt.ui32CalculatedChecksum)
      {
        nGoodCSCount++;
      }
      else
      {
        nBadCSCount++;
      }

      dataGridView1.Rows[10].Cells[5].Value = nBadCSCount.ToString();
      dataGridView1.Rows[11].Cells[5].Value = nGoodCSCount.ToString();

      listBoxSpare1.Items.Clear();
      for (int i = 0; i < rsimPkt.aySpares1.Length; i++)
      {
        listBoxSpare1.Items.Add(rsimPkt.aySpares1[i].ToString("X2"));
      }

      listBoxSpare2.Items.Clear();
      for (int i = 0; i < rsimPkt.aySpares2.Length; i++)
      {
        listBoxSpare2.Items.Add(rsimPkt.aySpares2[i].ToString("X2"));
      }

      listBoxSamples.Items.Clear();
      String sLine;
      for (int i = 0; i < 20; i++)
      {
        sLine = "";
        for (int j = 0; j < 50; j++)
        {
          sLine += rsimPkt.ayi16Bus1Samples[i * 10 + j].ToString("X4");
          sLine += " ";
        }
        listBoxSamples.Items.Add(sLine);
      }
    }

    private void RSimForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }
  }
}
