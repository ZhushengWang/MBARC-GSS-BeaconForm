using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace SPRL.Test
{
  public partial class SequencerForm : Form
  {
    private SequencerData seqData = new SequencerData();

    public int[] LastAck = new int[2];

    public int[] LastRFStatus = new int[53];

    private int nLineNum = 0;
    private int nLastSeqCnt = 0;
    private int nSkippedCnts = 0;

    public SequencerForm()
    {
      InitializeComponent();
    }

 
    private string GetTimeStampString()
    {
      DateTime timeStamp = DateTime.Now;

      return (timeStamp.ToShortDateString()
      + " "
      + timeStamp.Hour.ToString("D2")
      + ":" + timeStamp.Minute.ToString("D2")
      + ":" + timeStamp.Second.ToString("D2")
        //+ "." + timeStamp.Millisecond.ToString("D3")
      );
    }

    private void InvokeIfNeeded(MethodInvoker method)
    {
      // Only invoke on different thread if needed
      if (this.InvokeRequired)
      {
        this.BeginInvoke(method);
      }
      else
      {
        method();
      }
    }

    private void SequencerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = true;
      this.Hide();
    }

    public void displayAck(List<byte> listPkt)
    {
      ackTextBox.AppendText(listPkt[6].ToString("X2")+","+listPkt[7].ToString("X2")+"\n");
      LastAck[0] = listPkt[6];
      LastAck[1] = listPkt[7];
    }

    public void displayHK(List<byte> listPkt)
    {
      hkTextBox.AppendText("Time:   " + makeInt4(listPkt, 6).ToString("X8") + "\r\n" +
                           "PktCnt: " + listPkt[12].ToString("X2") + "\r\n" +
                           "FPGAVer:" + listPkt[13].ToString("X2") + "\r\n" +
                           "Cmd Cnt:" + makeInt(listPkt, 14).ToString("X4") + "\r\n" +
                           "Nak Cnt:" + makeInt(listPkt, 16).ToString("X4") + "\r\n" +
                           "ADC0:   " + makeInt(listPkt, 18).ToString("X4") + "\r\n" +
                           "ADC1:   " + makeInt(listPkt, 20).ToString("X4") + "\r\n" +
                           "ADC2:   " + makeInt(listPkt, 22).ToString("X4") + "\r\n" +
                           "ADC3:   " + makeInt(listPkt, 24).ToString("X4") + "\r\n" +
                           "ADC4:   " + makeInt(listPkt, 26).ToString("X4") + "\r\n" +
                           "ADC5:   " + makeInt(listPkt, 28).ToString("X4") + "\r\n" +
                           "ADC6:   " + makeInt(listPkt, 30).ToString("X4") + "\r\n" +
                           "ADC7:   " + makeInt(listPkt, 32).ToString("X4") + "\r\n" +
                           "BEFM:   " + listPkt[34].ToString("X2") + "\r\n" +
                           "MOTILim:" + listPkt[35].ToString("X2") + "\r\n" +
                           "MOT_Flt:" + listPkt[36].ToString("X2") + "\r\n" +
                           "Pos_Sen:" + listPkt[37].ToString("X2") + "\r\n" +
                           "DAC_Set:" + makeInt(listPkt, 38).ToString("X4") + "\r\n" +
                           "PhDiv:  " + makeInt(listPkt, 40).ToString("X4") + "\r\n" +
                           "ModTime:" + listPkt[42].ToString("X2") + "\r\n" +
                           "ROTDat0:" + makeInt(listPkt, 43).ToString("X4") + "\r\n" +
                           "ROTDat1:" + makeInt(listPkt, 45).ToString("X4") + "\r\n" +
                           "ROTHK:  " + makeInt(listPkt, 47).ToString("X4") + "\r\n\r\n");

      //for (int i = 0; i < listPkt.Count; i++)
      //{
      //  hkTextBox.AppendText(listPkt[i].ToString("X2") + "\n");
      //}
    }

    private int makeInt(List<byte> listPkt, int nPos)
    {
      return (listPkt[nPos] * 256) + listPkt[nPos + 1];
    }

    private int makeInt3(List<byte> listPkt, int nPos)
    {
      return (listPkt[nPos] * 65536) + listPkt[nPos + 1] * 256 + listPkt[nPos+2];
    }

    private UInt32 makeInt4(List<byte> listPkt, int nPos)
    {
      return (UInt32)(((listPkt[nPos] << 24) + listPkt[nPos] * 65536) + listPkt[nPos + 1] * 256 + listPkt[nPos + 2]);
    }


    private void sciTextBox_TextChanged(object sender, EventArgs e)
    {

    }

    internal void displayMsg(List<byte> subPkt)
    {
      int nLen = makeInt(subPkt, 4);

      ASCIIEncoding encoding = new ASCIIEncoding();
      string sMsg = encoding.GetString(subPkt.ToArray(),6,nLen);
      MainForm._mainform.PrintMsg(sMsg);
    }

    internal void displayTest(List<byte> subPkt)
    {
      int nBytesPerLine = 0;
      String sMsg = "";
      int nPktCnt = (int)(subPkt[6] * 256 + subPkt[7]);

      /*
      for (int i = 0; i < subPkt.Count; i++)
      {
        sMsg += subPkt[i].ToString("X2");
        sMsg += " ";
        if (++nBytesPerLine == 8)
        {
          sMsg += "\n";
          nBytesPerLine = 0;
        }
      }
      sMsg += "\n";
      */

      sMsg = nPktCnt.ToString("X4") + "\n";
      MainForm._mainform.PrintMsg(sMsg);

      if (nPktCnt != (nLastSeqCnt + 1))
      {
        nSkippedCnts++;
        MainForm._mainform.PrintError("Error:  Skipped Packet Count " + nSkippedCnts.ToString() + "\n");
      }

      nLastSeqCnt = nPktCnt;
    }
  }
}
