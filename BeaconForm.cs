using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// The main file to be modified
namespace SPRL.Test
{
  public partial class BeaconForm : Form
  {
    private Timer pktTimer = new Timer();
    private DateTime dtPktTime = DateTime.Now;
   
    public int[] LastBeacon;

    public BeaconForm()
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

      dataGridView2.Rows[0].Cells[0].Value = "Phase:";
      dataGridView2.Rows[1].Cells[0].Value = "Time:";
      dataGridView2.Rows[2].Cells[0].Value = "Bcn Cnt:";
      dataGridView2.Rows[3].Cells[0].Value = "TX Cnt:";
      dataGridView2.Rows[4].Cells[0].Value = "RX Cnt:";
      dataGridView2.Rows[5].Cells[0].Value = "CDH 1.5V:";
      dataGridView2.Rows[6].Cells[0].Value = "Radio 3.3V:";
      dataGridView2.Rows[7].Cells[0].Value = "Radio VBatt:";
      dataGridView2.Rows[8].Cells[0].Value = "IMU1 3.3V:";
      dataGridView2.Rows[9].Cells[0].Value = "IMU2 3.3V:";
      dataGridView2.Rows[10].Cells[0].Value = "DAC 3.3V:";
      dataGridView2.Rows[11].Cells[0].Value = "ADC 3.3V";
      dataGridView2.Rows[12].Cells[0].Value = "CDH 3.3V:";
      dataGridView2.Rows[13].Cells[0].Value = "CDH Temp:";
      dataGridView2.Rows[14].Cells[0].Value = "EP1 BatCh:";
      dataGridView2.Rows[15].Cells[0].Value = "EP1 BatLd:";
      dataGridView2.Rows[16].Cells[0].Value = "EP1 5V I:";
      dataGridView2.Rows[17].Cells[0].Value = "EP1 3.3V I:";
      dataGridView2.Rows[18].Cells[0].Value = "EP1 1.5V I:";
      dataGridView2.Rows[19].Cells[0].Value = "EP1 SP V:";

      dataGridView2.Rows[0].Cells[2].Value = "EP1 VBATSAT:";
      dataGridView2.Rows[1].Cells[2].Value = "EP1 GND:";
      dataGridView2.Rows[2].Cells[2].Value = "EP1 Temp:";
      dataGridView2.Rows[3].Cells[2].Value = "EP2 FIPT:";
      dataGridView2.Rows[4].Cells[2].Value = "EP2 MTQT:";
      dataGridView2.Rows[5].Cells[2].Value = "EP2 BatT:";
      dataGridView2.Rows[6].Cells[2].Value = "EP2 5VRegT:";
      dataGridView2.Rows[7].Cells[2].Value = "EP2 GND:";
      dataGridView2.Rows[8].Cells[2].Value = "EP2 GND:";
      dataGridView2.Rows[9].Cells[2].Value = "EP2 GND:";
      dataGridView2.Rows[10].Cells[2].Value = "EP2 GND:";
      dataGridView2.Rows[11].Cells[2].Value = "EP2 Temp:";
      dataGridView2.Rows[12].Cells[2].Value = "SP1 D1:";
      dataGridView2.Rows[13].Cells[2].Value = "SP1 D2:";
      dataGridView2.Rows[14].Cells[2].Value = "SP1 D3:";
      dataGridView2.Rows[15].Cells[2].Value = "SP1 D5:";
      dataGridView2.Rows[16].Cells[2].Value = "SP1 D6:";
      dataGridView2.Rows[17].Cells[2].Value = "SP1 D4:";
      dataGridView2.Rows[18].Cells[2].Value = "SP1 VO:";
      dataGridView2.Rows[19].Cells[2].Value = "SP1 3.3V:";

      dataGridView2.Rows[0].Cells[4].Value = "SP1 Temp:";
      dataGridView2.Rows[1].Cells[4].Value = "SP2 D1:";
      dataGridView2.Rows[2].Cells[4].Value = "SP2 D2:";
      dataGridView2.Rows[3].Cells[4].Value = "SP2 D3:";
      dataGridView2.Rows[4].Cells[4].Value = "SP2 D5:";
      dataGridView2.Rows[5].Cells[4].Value = "SP2 D6:";
      dataGridView2.Rows[6].Cells[4].Value = "SP2 D4:";
      dataGridView2.Rows[7].Cells[4].Value = "SP2 VO:";
      dataGridView2.Rows[8].Cells[4].Value = "SP2 3.3V:";
      dataGridView2.Rows[9].Cells[4].Value = "SP2 Temp:";
      dataGridView2.Rows[10].Cells[4].Value = "SP3 D1:";
      dataGridView2.Rows[11].Cells[4].Value = "SP3 D2:";
      dataGridView2.Rows[12].Cells[4].Value = "SP3 D3:";
      dataGridView2.Rows[13].Cells[4].Value = "SP3 D5:";
      dataGridView2.Rows[14].Cells[4].Value = "SP3 D6:";
      dataGridView2.Rows[15].Cells[4].Value = "SP3 D4:";
      dataGridView2.Rows[16].Cells[4].Value = "SP3 VO:";
      dataGridView2.Rows[17].Cells[4].Value = "SP3 3.3V:";
      dataGridView2.Rows[18].Cells[4].Value = "SP3 Temp:";
      dataGridView2.Rows[19].Cells[4].Value = "BD4 D1:";

      dataGridView2.Rows[0].Cells[6].Value = "BD4 D2:";
      dataGridView2.Rows[1].Cells[6].Value = "BD4 D3:";
      dataGridView2.Rows[2].Cells[6].Value = "BD4 D5:";
      dataGridView2.Rows[3].Cells[6].Value = "BD4 D6:";
      dataGridView2.Rows[4].Cells[6].Value = "BD4 D4:";
      dataGridView2.Rows[5].Cells[6].Value = "BD4 VO:";
      dataGridView2.Rows[6].Cells[6].Value = "BD4 3.3V:";
      dataGridView2.Rows[7].Cells[6].Value = "BD4 Temp:";
      dataGridView2.Rows[8].Cells[6].Value = "SP1 MagX:";
      dataGridView2.Rows[9].Cells[6].Value = "SP1 MagY:";
      dataGridView2.Rows[10].Cells[6].Value = "SP1 MagZ:";
      dataGridView2.Rows[11].Cells[6].Value = "SP1 MagT:";
      dataGridView2.Rows[12].Cells[6].Value = "SP2 MagX:";
      dataGridView2.Rows[13].Cells[6].Value = "SP2 MagY:";
      dataGridView2.Rows[14].Cells[6].Value = "SP2 MagZ:";
      dataGridView2.Rows[15].Cells[6].Value = "SP2 MagT:";
      dataGridView2.Rows[16].Cells[6].Value = "SP3 MagX:";
      dataGridView2.Rows[17].Cells[6].Value = "SP3 MagY:";
      dataGridView2.Rows[18].Cells[6].Value = "SP3 MagZ:";
      dataGridView2.Rows[19].Cells[6].Value = "SP3 MagT:";

      dataGridView2.Rows[0].Cells[8].Value = "BD4 MagX:";
      dataGridView2.Rows[1].Cells[8].Value = "BD4 MagY:";
      dataGridView2.Rows[2].Cells[8].Value = "BD4 MagZ:";
      dataGridView2.Rows[3].Cells[8].Value = "BD4 MagT:";
      dataGridView2.Rows[4].Cells[8].Value = "IMU2 Temp:";
      dataGridView2.Rows[5].Cells[8].Value = "IMU2 GyroX:";
      dataGridView2.Rows[6].Cells[8].Value = "IMU2 GyroY:";
      dataGridView2.Rows[7].Cells[8].Value = "IMU2 GyroZ:";
      dataGridView2.Rows[8].Cells[8].Value = "IMU2 AcclX:";
      dataGridView2.Rows[9].Cells[8].Value = "IMU2 AcclY:";
      dataGridView2.Rows[10].Cells[8].Value = "IMU2 AcclZ:";
      dataGridView2.Rows[11].Cells[8].Value = "IMU1 Temp:";
      dataGridView2.Rows[12].Cells[8].Value = "IMU1 GyroX:";
      dataGridView2.Rows[13].Cells[8].Value = "IMU1 GyroY:";
      dataGridView2.Rows[14].Cells[8].Value = "IMU1 GyroZ:";
      dataGridView2.Rows[15].Cells[8].Value = "LI1 Op Cnt:";
      dataGridView2.Rows[16].Cells[8].Value = "LI1 Temp:";
      dataGridView2.Rows[17].Cells[8].Value = "LI1 Time:";
      dataGridView2.Rows[18].Cells[8].Value = "LI1 RX:";
      dataGridView2.Rows[19].Cells[8].Value = "LI1 TX:";
    }

    void pktTimer_Tick(object sender, EventArgs e)
    {
      TimeSpan tsSpan = DateTime.Now - dtPktTime;

      this.Text = "Beacon Data - " + tsSpan.Days.ToString()+"d "+tsSpan.Hours.ToString("D2")+":"+tsSpan.Minutes.ToString("D2")+":"+tsSpan.Seconds.ToString("D2");
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

    public void displayBeacon(List<byte> listPkt)
    {
      int[] Convert = new int[100];
      int nOff = 0;
      const int nHdrLen = 6;
      int nInc;

      dtPktTime = DateTime.Now;

      dataGridView2.SuspendLayout();
      //MainForm._mainform.PrintMsg("Got Beacon Packet: " + listPkt.Count + "\n");

      //Digital Status
      Convert[nOff++] = makeInt(listPkt, 6);
      Convert[nOff++] = makeInt4(listPkt, 8);
      Convert[nOff++] = makeInt(listPkt, 12);
      Convert[nOff++] = makeInt(listPkt, 14);
      Convert[nOff++] = makeInt(listPkt, 16);

      //Data for 6 ADCs
      nInc = 0;
      for (int j = 0; j < 6; j++)
      {
        for (int i = 0; i < 9; i++)
        {
          Convert[nOff++] = makeInt(listPkt, 18 + nInc);
          nInc += 2;
        }
      }
      for (int i = 0; i < 9; i++)
      {
        Convert[nOff++] = 0;
        nInc += 2;
      }
      //Data for 4 Mags
      nInc = 0;
      for (int j = 0; j < 4; j++)
      {
        for (int i = 0; i < 4; i++)
        {
          Convert[nOff++] = makeInt(listPkt, 126 +nInc);
          nInc += 2;
        }
      }
      //Data for IMU2 
      nInc = 0;
      for (int i = 0; i < 7; i++)
      {
        Convert[nOff++] = 0;
        nInc += 4;
      }
      //Data for IMU1
        //imu1 temp
      nInc = 0;
      Convert[nOff++] = listPkt[158];
        //imu1 gyro
      nInc = 0;
      for (int i = 0; i < 3; i++)
      {
        Convert[nOff++] = makeInt(listPkt, 159 + nInc);
        nInc += 2;
      }

      //LI Status
      Convert[nOff++] = makeIntLE(listPkt, 167);
      Convert[nOff++] = makeIntLE(listPkt, 169);
      Convert[nOff++] = makeInt3LE(listPkt, 171);
      //Convert[nOff++] = listPkt[174];
      Convert[nOff++] = makeInt4LE(listPkt, 175);
      Convert[nOff++] = makeInt4LE(listPkt, 179);
      
      LastBeacon = Convert;

      int c = 0;
      for (int j = 0; j < dataGridView2.ColumnCount; j += 2)
      {
        for (int k = 0; k < dataGridView2.RowCount; k++)
        {
          if (radioVolts.Checked)
          {
            if (((c >= 5) && (c <= 12)) ||
                 ((c >= 14) && (c <= 21)) ||
                 ((c >= 23) && (c <= 30)) ||
                 ((c >= 32) && (c <= 39)) ||
                 ((c >= 41) && (c <= 48)) ||
                 ((c >= 50) && (c <= 57)) ||
                 ((c >= 59) && (c <= 66)))
            {
              dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
            }
            else
            {
              if ((c == 13) || (c == 22) || (c == 31) || (c == 40) || (c == 49) || (c == 58) || (c == 67))
              {
                dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c])/4.0)).ToString("F3");
              }
              else
              {
                if (c >= 95)  //LI1 Status
                {
                  dataGridView2.Rows[k].Cells[j + 1].Value = Convert[c].ToString();
                }
                else
                {
                  dataGridView2.Rows[k].Cells[j + 1].Value = Convert[c].ToString("X4");
                }
              }
            }
          }
          else
          {
            if (radioEng.Checked)
            {
              switch (c)
              {
                  //CDH ADC
                case 5:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
                  break;

                case 6:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 2.0).ToString("F3");
                  break;

                case 7:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 50.2/10.0).ToString("F3");
                  break;

                case 8:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 2.0).ToString("F3");
                  break;

                case 9:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 2.0).ToString("F3");
                  break;

                case 10:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 2.0).ToString("F3");
                  break;

                case 11:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 2.0).ToString("F3");
                  break;

                case 12:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 2.0).ToString("F3");
                  break;

                case 13:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                  //EPS ADC1
                case 14:
                case 16:
                case 17:
                case 18:               
                case 21:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
                  break;

                case 15:  //Bat Load 0.5V/A
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 / 0.5).ToString("F3");
                  break;

                case 19:  //SP V
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 8.0).ToString("F3");
                  break;

                case 20: //VBATSAT
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0 * 4.0).ToString("F3");
                  break;

                case 22: //EP1 Temp
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                  //EPS ADC2
                case 25:
                case 27:
                case 28:
                case 29:
                case 30:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
                  break;

                case 23:  //FIPEX T
                  dataGridView2.Rows[k].Cells[j + 1].Value = (((double)Convert[c] * 2.5 / 4096.0)*84.655+187.76-273.15).ToString("F3");
                  break;

                case 24:  //MTQ T
                  dataGridView2.Rows[k].Cells[j + 1].Value = (ThermistorCalc(Convert[c])).ToString("F3");
                  break;

                case 26: //5VReg T (LTC3626)
                  dataGridView2.Rows[k].Cells[j + 1].Value = (((double)Convert[c] * 2.5 / 4096.0)*200.0 - 273.15).ToString("F3");
                  break;

                case 31: //EP2 Temp
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                //SP1 ADC
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
                  break;

                case 40:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                //SP2 ADC
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
                  break;

                case 49:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                //SP3 ADC
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 2.5 / 4096.0).ToString("F3");
                  break;

                case 58:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                //BD4 ADC
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)((Int16)Convert[c]) * 2.5 / 4096.0).ToString("F3");
                  break;

                case 67:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)(make12bitsigned(Convert[c]) / 4.0)).ToString("F3");
                  break;

                  //Magnetometer data
                case 68:
                case 69:
                case 70:
                case 72:
                case 73:
                case 74:
                case 76:
                case 77:
                case 78:
                case 80:
                case 81:
                case 82:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)((Int16)Convert[c]) / 6842.0).ToString("F3");
                  break;

                case 84:  //IMU2 Temp
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((0.0042725 * ((double)Convert[c] + 997064704.0) / 65536.0) + 25).ToString("F3");
                  break;

                case 85:  //IMU2 GyroX
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 0.005 / 65536.0).ToString("F3");
                  break;

                case 86: //IMU2 GyroY
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 0.005 / 65536.0).ToString("F3");
                  break;

                case 87: //IMU2 GyroZ
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 0.005 / 65536.0).ToString("F3");
                  break;

                case 88:  //IMU2 Accel X
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 0.125 / 65536.0).ToString("F3");
                  break;

                case 89:  //IMU2 Accel Y
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 0.125 / 65536.0).ToString("F3");
                  break;

                case 90:  //IMU2 Accel Z
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)Convert[c] * 0.125 / 65536.0).ToString("F3");
                  break;
                case 91:  //IMU1 Temp
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((int)Convert[c] + 25).ToString("D3");
                  break;
                case 92:  //IMU1 Gyro Data
                case 93:
                case 94:
                  dataGridView2.Rows[k].Cells[j + 1].Value = ((double)((Int16)(Convert[c])) * 0.00875).ToString("F3");
                  break;
                  
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                  dataGridView2.Rows[k].Cells[j + 1].Value = Convert[c].ToString();
                  break;

                default:
                  dataGridView2.Rows[k].Cells[j + 1].Value = Convert[c].ToString();
                  break;
              }
            }
            else
            {
              dataGridView2.Rows[k].Cells[j + 1].Value = Convert[c].ToString("X4");
            }
          }
          c++;
        }
      }
      dataGridView2.ResumeLayout(true);
    }

    private void HKForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }

    //3rd order poly fit to 10K thermistor. Signed ADC value is input, deg C is output
    //accurate to about -30C
    private double ThermistorCalc(int nADCVal)
    {
      return (-4.0734e-9*Math.Pow(nADCVal,3)+2.7553e-5*Math.Pow(nADCVal,2)-8.1786e-2*nADCVal+1.1155e2);
    }
  }
}
