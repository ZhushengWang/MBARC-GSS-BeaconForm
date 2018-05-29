using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using System.Threading;

namespace SPRL.Test
{
  public class SCPI
  {
    private MessageBasedSession MBSession = null;

    public SCPI(string sID)
    {
      MBSession = new MessageBasedSession(sID);
      MBSession.DefaultBufferSize = 10000000;

      //USBSession.Clear();
      //USBSession.Write("*RST\n");
    }

    public void dispose()
    {
      MBSession.Dispose();
    }

    public string GetID()
    {
      string sQuery = "*IDN?\n";
      string sResult = MBSession.Query(sQuery);
      return (sResult);
    }

    public void SendCMD(string sCMD)
    {
      MBSession.Write(sCMD);
    }

    public string Query(string sCMD)
    {
      return(MBSession.Query(sCMD));
    }
  }
}
