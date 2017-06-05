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
  public partial class HKForm : Form
  {
    public int[] LastHK;

    private Timer pktTimer = new Timer();
    private DateTime dtPktTime = DateTime.Now;

    public HKForm()
    {
      InitializeComponent();

      pktTimer.Interval = 1000;

      pktTimer.Tick += new EventHandler(pktTimer_Tick);
      pktTimer.Start();

      dataGridView2.Columns.Add("Col1", "Col1");
      dataGridView2.Columns.Add("Col2", "Col2");
      dataGridView2.Columns.Add("Col3", "Col3");
      dataGridView2.Columns.Add("Col4", "Col4");
      dataGridView2.Columns.Add("Col5", "Col5");
      dataGridView2.Columns.Add("Col6", "Col6");
      dataGridView2.Columns.Add("Col7", "Col7");
      dataGridView2.Columns.Add("Col8", "Col8");
      dataGridView2.Columns.Add("Col9", "Col9");
      dataGridView2.Columns.Add("Col10", "Col10");
      
      dataGridView2.Rows.Add(19);

      dataGridView2.RowHeadersVisible = false;
      dataGridView2.ColumnHeadersVisible = false;

      for (int i = 0; i < dataGridView2.Rows.Count; i++)
      {
        for (int j = 0; j < dataGridView2.Columns.Count; j++)
        {
          if ((j % 2) == 0)
          {
            dataGridView2.Rows[i].Cells[j].Style.BackColor = Color.Silver;
          }
          dataGridView2.Rows[i].Cells[j].ReadOnly = true;
        }
      }

      dataGridView2.Rows[0].Cells[0].Value = "Time:";
      dataGridView2.Rows[1].Cells[0].Value = "HK Cnt:";
      dataGridView2.Rows[2].Cells[0].Value = "Pkts Rx:";
      dataGridView2.Rows[3].Cells[0].Value = "Radio Pkts Rx:";
      dataGridView2.Rows[4].Cells[0].Value = "Radio Pkts Tx:";
      dataGridView2.Rows[5].Cells[0].Value = "FSW Load Pkts";
      dataGridView2.Rows[6].Cells[0].Value = "Acks Rx:";
      dataGridView2.Rows[7].Cells[0].Value = "Nacks Rx:";
      dataGridView2.Rows[8].Cells[0].Value = "Hdr CS Err:";
      dataGridView2.Rows[9].Cells[0].Value = "Pkt CS Err:";
      dataGridView2.Rows[10].Cells[0].Value = "Pkt Type Err:";
      dataGridView2.Rows[11].Cells[0].Value = "PL Size Err:";
      dataGridView2.Rows[12].Cells[0].Value = "Sync Err";
      dataGridView2.Rows[13].Cells[0].Value = "Script1";
      dataGridView2.Rows[14].Cells[0].Value = "Script2";
      dataGridView2.Rows[15].Cells[0].Value = "Script3";
      dataGridView2.Rows[16].Cells[0].Value = "Script4";
      dataGridView2.Rows[17].Cells[0].Value = "Script5";
      dataGridView2.Rows[18].Cells[0].Value = "Script6";
      dataGridView2.Rows[19].Cells[0].Value = "Script7";

      dataGridView2.Rows[0].Cells[2].Value = "Boot Time:";
      dataGridView2.Rows[1].Cells[2].Value = "Boot Ver:";
      dataGridView2.Rows[2].Cells[2].Value = "FSW Ver:";
      dataGridView2.Rows[3].Cells[2].Value = "Active Part:";
      dataGridView2.Rows[4].Cells[2].Value = "Boot Cnt:";
      dataGridView2.Rows[5].Cells[2].Value = "Comm Cnt:";
      dataGridView2.Rows[6].Cells[2].Value = "Deploy Cnt:";
      dataGridView2.Rows[7].Cells[2].Value = "FIP Act. Scr.:";
      dataGridView2.Rows[8].Cells[2].Value = "FIP_Script Cnt:";
      dataGridView2.Rows[9].Cells[2].Value = "TX Enable:";
      dataGridView2.Rows[10].Cells[2].Value = "Init. Wait:";
      dataGridView2.Rows[11].Cells[2].Value = "Enable Dep:";
      dataGridView2.Rows[12].Cells[2].Value = "Script Enable:";
      dataGridView2.Rows[13].Cells[2].Value = "FIP M1 WPtr:";
      dataGridView2.Rows[14].Cells[2].Value = "FIP M1 RPtr:";
      dataGridView2.Rows[15].Cells[2].Value = "FIP M2 WPtr:";
      dataGridView2.Rows[16].Cells[2].Value = "FIP M2 RPtr:";
      dataGridView2.Rows[17].Cells[2].Value = "FIP M1 SDP:";
      dataGridView2.Rows[18].Cells[2].Value = "FIP M1 HK:";
      dataGridView2.Rows[19].Cells[2].Value = "FIP M2 SDP:";

      dataGridView2.Rows[0].Cells[4].Value = "FIP M2 HK:";
      dataGridView2.Rows[1].Cells[4].Value = "OP Spare1:";
      dataGridView2.Rows[2].Cells[4].Value = "OP Spare2:";
      dataGridView2.Rows[3].Cells[4].Value = "OP Spare3:";
      dataGridView2.Rows[4].Cells[4].Value = "OP Spare4:";
      dataGridView2.Rows[5].Cells[4].Value = "PWR EPS ADC:";
      dataGridView2.Rows[6].Cells[4].Value = "PWR FIPEX:";
      dataGridView2.Rows[7].Cells[4].Value = "PWR RADIO:";
      dataGridView2.Rows[8].Cells[4].Value = "PWR CDH ADC:";
      dataGridView2.Rows[9].Cells[4].Value = "PWR DAC12";
      dataGridView2.Rows[10].Cells[4].Value = "PWR IMU2:";
      dataGridView2.Rows[11].Cells[4].Value = "PWR IMU1:";
      dataGridView2.Rows[12].Cells[4].Value = "PWR2 BD4Mag:";
      dataGridView2.Rows[13].Cells[4].Value = "PWR2 BD4ADC:";
      dataGridView2.Rows[14].Cells[4].Value = "PWR2 SP3Mag:";
      dataGridView2.Rows[15].Cells[4].Value = "PWR2 SP3ADC:";
      dataGridView2.Rows[16].Cells[4].Value = "PWR2 SP2Mag:";
      dataGridView2.Rows[17].Cells[4].Value = "PWR2 SP2ADC:";
      dataGridView2.Rows[18].Cells[4].Value = "PWR2 SP1Mag:";
      dataGridView2.Rows[19].Cells[4].Value = "PWR2 SP1ADC:";

      dataGridView2.Rows[0].Cells[6].Value = "STATUS1:";
      dataGridView2.Rows[1].Cells[6].Value = "STATUS2:";
      dataGridView2.Rows[2].Cells[6].Value = "STATUS3:";
      dataGridView2.Rows[3].Cells[6].Value = "STATUS4:";
      dataGridView2.Rows[4].Cells[6].Value = "CONTROL1:";
      dataGridView2.Rows[5].Cells[6].Value = "CONTROL2:";
      dataGridView2.Rows[6].Cells[6].Value = "FPGA Ver:";
      dataGridView2.Rows[7].Cells[6].Value = "DAC12:";
      dataGridView2.Rows[8].Cells[6].Value = "Burn Pwr Status:";
      dataGridView2.Rows[9].Cells[6].Value = "Radio Baud:";
      dataGridView2.Rows[10].Cells[6].Value = "Radio Power";
      dataGridView2.Rows[11].Cells[6].Value = "RF Rx Baud:";
      dataGridView2.Rows[12].Cells[6].Value = "RF Tx Baud:";
      dataGridView2.Rows[13].Cells[6].Value = "Rx Mod:";
      dataGridView2.Rows[14].Cells[6].Value = "Tx Mod:";
      dataGridView2.Rows[15].Cells[6].Value = "Rx Freq:";
      dataGridView2.Rows[16].Cells[6].Value = "Tx Freq:";
      dataGridView2.Rows[17].Cells[6].Value = "Dest Call:";
      dataGridView2.Rows[18].Cells[6].Value = "Src Call:";
      dataGridView2.Rows[19].Cells[6].Value = "Tx Preamble:";

      dataGridView2.Rows[0].Cells[8].Value = "Tx Postamble:";
      dataGridView2.Rows[1].Cells[8].Value = "Config:";
      dataGridView2.Rows[2].Cells[8].Value = "Config2:";
      dataGridView2.Rows[3].Cells[8].Value = "FIPEX Script State";
      dataGridView2.Rows[4].Cells[8].Value = "FIPEX Script Step";
      dataGridView2.Rows[5].Cells[8].Value = "FIPEX Ack Cnt";
      dataGridView2.Rows[6].Cells[8].Value = "FIPEX Nack Cnt";
      dataGridView2.Rows[7].Cells[8].Value = "FIPEX SDP Cnt";
      dataGridView2.Rows[8].Cells[8].Value = "FIPEX HK Cnt";
      dataGridView2.Rows[9].Cells[8].Value = "";
      dataGridView2.Rows[10].Cells[8].Value = "";
      dataGridView2.Rows[11].Cells[8].Value = "";
      dataGridView2.Rows[12].Cells[8].Value = "";
      dataGridView2.Rows[13].Cells[8].Value = "";
      dataGridView2.Rows[14].Cells[8].Value = "";
      dataGridView2.Rows[15].Cells[8].Value = "";
      dataGridView2.Rows[16].Cells[8].Value = "";
      dataGridView2.Rows[17].Cells[8].Value = "";
      dataGridView2.Rows[18].Cells[8].Value = "";
      dataGridView2.Rows[19].Cells[8].Value = "";
    }

    void pktTimer_Tick(object sender, EventArgs e)
    {
      TimeSpan tsSpan = DateTime.Now - dtPktTime;

      this.Text = "HK Data - " + tsSpan.Days.ToString() + "d " + tsSpan.Hours.ToString("D2") + ":" + tsSpan.Minutes.ToString("D2") + ":" + tsSpan.Seconds.ToString("D2");
    }

    private int makeInt(List<byte> listPkt, int nPos)
    {
      return (listPkt[nPos] * 256) + listPkt[nPos + 1];
    }

    private int makeIntLE(List<byte> listPkt, int nPos)
    {
      return (listPkt[nPos+1] * 256) + listPkt[nPos];
    }

    private int makeInt3(List<byte> listPkt, int nPos)
    {
      return (int)(((int)listPkt[nPos] << 16) + ((int)listPkt[nPos + 1] << 8) + (int)listPkt[nPos + 2]);
    }

    private int makeInt3LE(List<byte> listPkt, int nPos)
    {
      return (int)(((int)listPkt[nPos+2] << 16) + ((int)listPkt[nPos + 1] << 8) + (int)listPkt[nPos]);
    }

    private int makeInt4(List<byte> listPkt, int nPos)
    {
      return (int)(((int)listPkt[nPos] << 24) +
                   ((int)listPkt[nPos + 1] << 16) + 
                   ((int)listPkt[nPos + 2] << 8) +
                   ((int)listPkt[nPos + 3]));
    }

    private UInt32 makeUInt4(List<byte> listPkt, int nPos)
    {
      return (UInt32)(((UInt32)listPkt[nPos] << 24) +
                   ((UInt32)listPkt[nPos + 1] << 16) +
                   ((UInt32)listPkt[nPos + 2] << 8) +
                   ((UInt32)listPkt[nPos + 3]));
    }

    private int makeInt4LE(List<byte> listPkt, int nPos)
    {
      return (int)(((int)listPkt[nPos+3] << 24) +
                   ((int)listPkt[nPos + 2] << 16) +
                   ((int)listPkt[nPos + 1] << 8) +
                   ((int)listPkt[nPos]));
    }

    private int make12bitsigned(int nValue)
    {
      UInt16 wValue = (UInt16)nValue;
      Int16 wsValue;

      wValue &= 0x0FFF; //Its only 12 bits

      if ((wValue & 0x800) != 0)
      {
        wValue |= 0xF000;  //Sign Extend
      }

      wsValue = (Int16)wValue;

      return ((int)wsValue);
    }

    public void displayHK(List<byte> listPkt)
    {
      dtPktTime = DateTime.Now;

      if (!backgroundWorker1.IsBusy)
      {
        backgroundWorker1.RunWorkerAsync(listPkt);
      }
    }

    private void HKForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      List<List<string>> listConverted = e.Result as List<List<string>>;

      int c = 0;
      for (int j = 0; j < dataGridView2.ColumnCount; j += 2)
      {
        for (int k = 0; k < dataGridView2.RowCount; k++)
        {
          if (c < listConverted[0].Count)
          {
            dataGridView2.Rows[k].Cells[j + 1].Value = listConverted[0][c++];
          }

        }
      }
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      List<byte> listPkt = e.Argument as List<byte>;

      List<List<string>> listConverted = new List<List<string>>();

      listConverted.Add(new List<String>());  //Raw
      listConverted.Add(new List<String>());  //Volts/decimal
      listConverted.Add(new List<String>());  //Engineering Units

      listConverted[0].Add(((UInt32)(makeInt4(listPkt,6))).ToString());  //Time
      listConverted[0].Add(makeInt(listPkt, 10).ToString());  //Count

      
      //Comm Stats
      for (int i = 0; i < 11; i++)
      {
        listConverted[0].Add(makeInt(listPkt, 12 + i*2).ToString());
      }

      //Script Times
      for (int i = 0; i < 7; i++)
      {
        listConverted[0].Add(makeInt4(listPkt, 34 + i * 4).ToString());
      }

      //OpStats
      for (int i = 0; i < 24; i++)
      {
        listConverted[0].Add(makeInt4(listPkt, 62 + i * 4).ToString());
      }
     
      listConverted[0].Add(listPkt[161].ToString("X2"));

      //Power Control 1
      listConverted[0].Add(((listPkt[162] >> 6) & 0x01).ToString());
      listConverted[0].Add(((listPkt[162] >> 5) & 0x01).ToString());
      listConverted[0].Add(((listPkt[162] >> 4) & 0x01).ToString());
      listConverted[0].Add(((listPkt[162] >> 3) & 0x01).ToString());
      listConverted[0].Add(((listPkt[162] >> 2) & 0x01).ToString());
      listConverted[0].Add(((listPkt[162] >> 1) & 0x01).ToString());
      listConverted[0].Add(((listPkt[162]) & 0x01).ToString());

      //Power Control 2
      listConverted[0].Add(((listPkt[163] >> 7) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163] >> 6) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163] >> 5) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163] >> 4) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163] >> 3) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163] >> 2) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163] >> 1) & 0x01).ToString());
      listConverted[0].Add(((listPkt[163]) & 0x01).ToString());

      //Status Registers
      listConverted[0].Add(listPkt[164].ToString("X2"));
      listConverted[0].Add(listPkt[165].ToString("X2"));
      listConverted[0].Add(listPkt[166].ToString("X2"));
      listConverted[0].Add(listPkt[167].ToString("X2"));

      //Control Registers
      listConverted[0].Add(listPkt[168].ToString("X2"));
      listConverted[0].Add(listPkt[169].ToString("X2"));

      //FPGA Version
      listConverted[0].Add(listPkt[170].ToString());

      listConverted[0].Add(makeInt(listPkt,171).ToString("X4"));
      listConverted[0].Add(listPkt[173].ToString());

      //Radio Config
      listConverted[0].Add(listPkt[174].ToString());
      listConverted[0].Add(listPkt[175].ToString());
      listConverted[0].Add(listPkt[176].ToString());
      listConverted[0].Add(listPkt[177].ToString());
      listConverted[0].Add(listPkt[178].ToString());
      listConverted[0].Add(listPkt[179].ToString());

      listConverted[0].Add(makeInt4LE(listPkt, 180).ToString());
      listConverted[0].Add(makeInt4LE(listPkt, 184).ToString());

      ASCIIEncoding encoding = new ASCIIEncoding();
      string sCall  = encoding.GetString(listPkt.GetRange(188,6).ToArray());
      listConverted[0].Add(sCall);
      sCall = encoding.GetString(listPkt.GetRange(194, 6).ToArray());
      listConverted[0].Add(sCall);

      listConverted[0].Add(makeIntLE(listPkt, 200).ToString());
      listConverted[0].Add(makeIntLE(listPkt, 202).ToString());
      listConverted[0].Add(makeIntLE(listPkt, 204).ToString("X4"));
      listConverted[0].Add(makeIntLE(listPkt, 206).ToString("X4"));

      listConverted[0].Add(makeInt(listPkt, 208).ToString());
      listConverted[0].Add(makeInt(listPkt, 210).ToString());
      listConverted[0].Add(makeInt(listPkt, 212).ToString());
      listConverted[0].Add(makeInt(listPkt, 214).ToString());
      listConverted[0].Add(makeInt(listPkt, 216).ToString());
      listConverted[0].Add(makeInt(listPkt, 218).ToString());

      e.Result = listConverted;
    }
  }
}
