using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using NationalInstruments.NI4882;

namespace SPRL.Test
{
  public class PS325
  {
    private Device deviceHVPS;

    public PS325(int nAddress)
    {
      Address addr = new Address((byte)nAddress);

      deviceHVPS = new Device(0, addr);
      deviceHVPS.Write("*RST");
    }

    public string GetID()
    {
      deviceHVPS.Write("*IDN?");
      return deviceHVPS.ReadString();
    }

    public void SetV(double dVolts)
    {
      deviceHVPS.Write("VSET" + dVolts.ToString("0.0"));
    }

    public double GetSetV()
    {
      deviceHVPS.Write("VSET?");
      return Convert.ToDouble(deviceHVPS.ReadString());
    }

    public double GetOutV()
    {
      deviceHVPS.Write("VOUT?");
      return Convert.ToDouble(deviceHVPS.ReadString());
    }

    public double GetOutI()
    {
      deviceHVPS.Write("IOUT?");
      return Convert.ToDouble(deviceHVPS.ReadString());
    }

    public void SetMaxI(double dAmps)
    {
      deviceHVPS.Write("ILIM" + dAmps.ToString("0.0"));
    }

    public void SetMaxV(double dVolts)
    {
      deviceHVPS.Write("VLIM" + dVolts.ToString("0.0"));
    }

    public double GetMaxV()
    {
      deviceHVPS.Write("VLIM?");
      return Convert.ToDouble(deviceHVPS.ReadString());
    }

    public void TurnOn()
    {
      deviceHVPS.Write("HVON");
    }

    public void TurnOff()
    {
      deviceHVPS.Write("HVOF");
    }
  }
}
