using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;


namespace SPRL.Test
{
  class E3632A_PS:IPSInterface
  {
    private SerialSession serSession;
    private Object lockObject;
    private string sSupply;

    public E3632A_PS(String sID)
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

    public void dispose()
    {
      serSession.Dispose();
    }

    public void SetV(int nSupply, double dVolts)
    {
      sSupply = "P15V";
      if (dVolts >= 15) sSupply = "P30V";
      {
        SendCmd("VOLT:RANG " + sSupply +";:VOLT "+ dVolts);
      }
    }

    public void SetI(int nSupply, double dAmps)
    {
      //string sSupply = GetSupplyString(nSupply);
      if (sSupply != "")
      {
        SendCmd("VOLT:RANG " + sSupply + ";:CURR " + dAmps);
      }
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

      string sQuery = "VOLT?";
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
      string sQuery = "MEAS:VOLT:DC? " + GetSupplyString(nSupply);
      string sresult;

      sresult = QueryData(sQuery);
      return (sresult);
    }

    public string GetAmps(int nSupply)
    {
      string sQuery = "MEAS:CURR:DC? " + GetSupplyString(nSupply);
      string sresult;

      sresult = QueryData(sQuery);
      return (sresult);
    }

    private string GetSupplyString(int nSupply)
    {
      string sSupply = "";

      if ((nSupply >= 0) && (nSupply <= 2))
      {
        switch (nSupply)
        {
          case 0:
            sSupply = "P15V";
            break;

          case 1:
            sSupply = "P30V";
            break;
        }
      }
      return (sSupply);
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
