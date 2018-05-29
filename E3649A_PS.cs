using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;


namespace SPRL.Test
{
  class E3649A_PS:IPSInterface
  {
    private SerialSession serSession;
    private Object lockObject;
    private string[] saSupplies = { "P35V", "P35V" };
    private string[] saNames = { "OUT1", "OUT2" };

    public E3649A_PS(String sID)
    {
      if (lockObject == null)
      {
        lockObject = new Object();
      }
      try
      {
        serSession = new SerialSession(sID);
        serSession.BaudRate = 9600;
        serSession.StopBits = StopBitType.One;
        serSession.DataBits = 8;
        serSession.FlowControl = FlowControlTypes.DtrDsr;
        
        //serSession.Clear();
        SendCmd("*RST");
        SendCmd("*CLS");
        SendCmd("SYST:REM");
      }
      catch (Exception e)
      {
        MainForm._mainform.PrintError("Error:  Unable to open " + sID + "\n");
      }
    }

    public void SetV(int nSupply, double dVolts)
    {
      if (dVolts >= 35) saSupplies[nSupply] = "P60V";
      else saSupplies[nSupply] = "P35V";
      {
        SendCmd("INST " + saNames[nSupply] + ";:VOLT:RANG " + saSupplies[nSupply] +";:VOLT "+ dVolts);
      }
    }

    public void dispose()
    {
      serSession.Dispose();
    }

    public void SetI(int nSupply, double dAmps)
    {
      SendCmd("INST " + saNames[nSupply] + ";:VOLT:RANG " + saSupplies[nSupply] + ";:CURR " + dAmps);
    }

    public void SetOutOn(bool bOn)
    {
      if (bOn)
      {
        SendCmd("Output:State ON");
      }
      else
      {
        SendCmd("Output:State OFF");
      }
    }

    public string GetID()
    {
      string sQuery = "*IDN?\n";
      string sResult = QueryData(sQuery);
      return (sResult);
    }

    public string GetVSet(int nSupply)
    {
      char[] caSep = new char[] {',',' '};
      char[] caTrim = new char[] {'"','/','\\'};
      string sresult;

      string sQuery = "INST " + saNames[nSupply] + ";APPLY?";
      if (nSupply == 2)
      {
        sresult = "0";
      }
      else
      {
        sresult = QueryData(sQuery);
        sresult = sresult.Trim(caTrim);
      }
      return (sresult.Split(caSep)[0]);
    }

    public string GetVolts(int nSupply)
    {
      string sQuery = "INST " + saNames[nSupply] + ";:MEAS:VOLT:DC?";
      string sresult;

      sresult = QueryData(sQuery);
      return (sresult);
    }

    public string GetAmps(int nSupply)
    {
      string sQuery = "INST " + saNames[nSupply] + ";:MEAS:CURR:DC?";
      string sresult;

      sresult = QueryData(sQuery);
      return (sresult);
    }

    private void SendCmd(string sCmd)
    {
      lock (lockObject)
      {
        if (serSession != null)
        {
          serSession.Write(sCmd + "\n");
        }
      }
    }

    private string QueryData(string sCmd)
    {
      string sResult="";
      lock (lockObject)
      {
        if (serSession != null)
        {
          sResult = serSession.Query(sCmd + "\n");
        }
      }
      return(sResult);
    }
  }
}
