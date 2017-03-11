using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
  using NationalInstruments.VisaNS;
  using NationalInstruments.NI4882;
  using System.Threading;

namespace SPRL.Test
{
  public class ICS8099
  {
    private TcpipSession sessionTempController;

    public ICS8099(string sName)
    {
      sessionTempController = new TcpipSession(sName);
      sessionTempController.Write("*RST\n");
    }

    public void dispose()
    {
      //sessionTempController.Write("ABORT\n");
      sessionTempController.Dispose();
    }

    public void SetTemp(double dTemp)
    {
      int nSetTemp = (int)(dTemp * 10.0);
      string sTempCmd = "W 300, "+nSetTemp.ToString();
      sessionTempController.Write(sTempCmd);
    }

    public double GetTemp()
    {
      string sResult = "0.0";

      sResult = sessionTempController.Query("R? 100,1");

      return (Convert.ToDouble(sResult)/10.0);
    }

    public String GetWatlowModel()
    {
      string sResult="";

      sResult = sessionTempController.Query("R? 0,1");

      return (sResult);
    }

    public string GetID()
    {
      string sQuery = "*IDN?\n";
      string sResult = sessionTempController.Query(sQuery);
      return (sResult);
    }

    public void SendCMD(string sCMD)
    {
      sessionTempController.Write(sCMD);
    }
  }
}

