using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using NationalInstruments.NI4882;

namespace SPRL.Test
{
  public class HP5316B
  {
    private Device devicePulseGen;
    //private MessageBasedSession mbSession;

    public HP5316B(int nAddress)
    {
      Address addr = new Address((byte)nAddress);

      devicePulseGen = new Device(0, addr);
      devicePulseGen.EndOfStringCharacter = 10;
      devicePulseGen.TerminateReadOnEndOfString = true;
      devicePulseGen.Write("IN\n");
    }

    public void Reset()
    {
      string SQuery = "RE\n";
      devicePulseGen.Write(SQuery);
    }

    public void TrigAPos()
    {
      string SQuery = "AS0\n";
      devicePulseGen.Write(SQuery);
    }

    public void TrigANeg()
    {
      string SQuery = "AS1\n";
      devicePulseGen.Write(SQuery);
    }

    public void TrigBPos()
    {
      string SQuery = "BS0\n";
      devicePulseGen.Write(SQuery);
    }

    public void TrigBNeg()
    {
      string SQuery = "BS1\n";
      devicePulseGen.Write(SQuery);
    }

    public void GateCont()
    {
      string SQuery = "WA0\n";
      devicePulseGen.Write(SQuery);
    }

    public void GateOnce()
    {
      string SQuery = "WA1\n";
      devicePulseGen.Write(SQuery);
    }

    public void GateALong()
    {
      string SQuery = "GA0\n";
      devicePulseGen.Write(SQuery);
    }

    public void GateAShort()
    {
      string SQuery = "GA1\n";
      devicePulseGen.Write(SQuery);
    }

    public void GateBLong()
    {
      string SQuery = "GB0\n";
      devicePulseGen.Write(SQuery);
    }

    public void GateBShort()
    {
      string SQuery = "GB1\n";
      devicePulseGen.Write(SQuery);
    }

    public void SetATrigLvl(double dTrig)
    {
      string sQuery = "TR1AT" + dTrig.ToString("F2") + "\n";
      devicePulseGen.Write(sQuery);
    }

    public void SetBTrigLvl(double dTrig)
    {
      string sQuery = "TR1BT" + dTrig.ToString("F2")+ "\n";
      devicePulseGen.Write(sQuery);
    }

    public void MeasFreq()
    {
      string sQuery = "FNA\n";
      devicePulseGen.Write(sQuery);
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
