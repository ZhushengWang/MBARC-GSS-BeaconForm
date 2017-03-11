using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using NationalInstruments.NI4882;

namespace SPRL.Test
{
  public class Agilent81130A
  {
    private Device devicePulseGen;
    //private MessageBasedSession mbSession;

    public Agilent81130A(int nAddress)
    {
      Address addr = new Address((byte)nAddress);

      devicePulseGen = new Device(0, addr);
      devicePulseGen.Write("*RST\n");
    }

    public string GetID()
    {
      string sQuery = "*IDN?\n";
      devicePulseGen.Write(sQuery);
      string sResult = devicePulseGen.ReadString();
      return (sResult);
    }

    public void OutOn(int nChannel, bool bOn, bool bNormal)
    {
      if (bNormal)
      {
        if (bOn)
        {
          devicePulseGen.Write(":OUTP" + nChannel.ToString() + " ON\n");
        }
        else
        {
          devicePulseGen.Write(":OUTP" + nChannel.ToString() + " OFF\n");
        }
      }
      else
      {
        if (bOn)
        {
          devicePulseGen.Write(":OUTP" + nChannel.ToString() + ":COMP ON\n");
        }
        else
        {
          devicePulseGen.Write(":OUTP" + nChannel.ToString() + ":COMP OFF\n");
        }
      }
    }

    public void SetLevels(int nChannel, double dLow, double dHigh)
    {
      devicePulseGen.Write(":HOLD VOLT\n");
      devicePulseGen.Write(":VOLT" + nChannel.ToString() + ":LOW " + dLow.ToString() + "V\n");
      devicePulseGen.Write(":VOLT" + nChannel.ToString() + ":HIGH " + dHigh.ToString() + "V\n");
    }

    public void SetPeriod(double dPeriod)
    {
      devicePulseGen.Write(":PULSE:PER " + dPeriod.ToString() + "NS\n");
    }

    public void SetWidth(int nChannel, double dWidth)
    {
      devicePulseGen.Write(":PULSE:WIDTH" + nChannel.ToString() + " " + dWidth.ToString() + "NS\n");
      devicePulseGen.Write(":PULSE:HOLD" + nChannel.ToString() + " " + "WIDTH\n");
    }

    public void SetDuty(int nChannel, double dDuty)
    {
      devicePulseGen.Write(":PULSE:DCYC" + nChannel.ToString() + " " + dDuty.ToString() + "PCT\n");
      devicePulseGen.Write(":PULSE:HOLD" + nChannel.ToString() + " " + "DCYC\n");
    }

    public void SetContinuous()
    {
      devicePulseGen.Write(":DIG:PATT OFF\n");
    }

    public void SetPattern(int nLength)
    {
      devicePulseGen.Write(":DIG:PATT:LOOP:INF ON\n");

      devicePulseGen.Write(":DIG:PATT ON\n");
      devicePulseGen.Write(":DIG:PATT:SEGM1:DATA #1830000000\n");
      devicePulseGen.Write(":DIG:PATT:SEGM1:LENG " + nLength.ToString() + "\n");
    }

    public void SetPatternData(int nChan, string sData, int nLength)
    {
      int nDataLen = sData.Length;
      string sDataLen = nDataLen.ToString();
      string sLenLen = sDataLen.Length.ToString();

      devicePulseGen.Write(":DIG:PATT:LOOP:INF ON\n");

      devicePulseGen.Write(":DIG:PATT ON\n");
      devicePulseGen.Write(":DIG:SIGNAL"+nChan.ToString()+":FORM NRZ\n");
      devicePulseGen.Write(":DIG:PATT:SEGM1:DATA"+nChan.ToString()+" #" + sLenLen + sDataLen + sData + "\n");
      devicePulseGen.Write(":DIG:PATT:SEGM1:LENG " + nLength.ToString() + "\n");
    }

    public void WriteCommand(string sCmd)
    {
      devicePulseGen.Write(sCmd + "\n");
    }

    public string Fetch()
    {
      //string sQuery = "FETCH?\n";
      //devicePulseGen.Write(sQuery);
      string sResult = devicePulseGen.ReadString();
      return (sResult);
    }
 
  }
}
