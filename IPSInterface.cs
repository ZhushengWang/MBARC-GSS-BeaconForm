using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;


namespace SPRL.Test
{
  public interface IPSInterface
  {
    void SetV(int nSupply, double dVolts);
    void SetI(int nSupply, double dAmps);
    void SetOutOn(bool bOn);
    void dispose();
    string GetID();
    string GetVSet(int nSupply);
    string GetVolts(int nSupply);
    string GetAmps(int nSupply);
  }
}
