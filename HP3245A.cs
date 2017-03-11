using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using NationalInstruments.NI4882;

namespace SPRL.Test
{
  public class HP3245A
  {
    private Device deviceUSRC;

    public HP3245A(int nAddress)
    {
      Address addr = new Address((byte)nAddress);

      deviceUSRC = new Device(0, addr);
      deviceUSRC.SetEndOnWrite = true;
      deviceUSRC.Write("RST");

      deviceUSRC.Write("END ON;");
    }

    public string GetID()
    {
      deviceUSRC.Write("IDN?");
      return deviceUSRC.ReadString();
    }

    public void SetV(double dVolts)
    {
      deviceUSRC.Write("APPLY DCV " + dVolts.ToString() + ";");
    }

    public double GetV()
    {
      deviceUSRC.Write("OUTPUT?");
      return Convert.ToDouble(deviceUSRC.ReadString());
    }

    public void SetCh(int nChan) //nChan = 0 or 1
    {
      deviceUSRC.Write("USE " + nChan.ToString() + ";");
    }
  }
}
