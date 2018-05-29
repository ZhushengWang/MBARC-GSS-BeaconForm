using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using System.Threading;

namespace SPRL.Test
{
  public class HP34970A
  {
    //private MessageBasedSession mbSession;
    private SerialSession serSession;
    //private string sRangeRes = "AUTO";

    public HP34970A(string sID)
    {
      serSession = new SerialSession(sID);
      serSession.BaudRate = 115200;
      serSession.StopBits = StopBitType.One;
      serSession.DataBits = 8;
      serSession.FlowControl = FlowControlTypes.DtrDsr;

      serSession.Clear();
      serSession.Write("*RST\n");
      //serSession.Write("ROUT:SCAN (@101)\n");
    }

    public void dispose()
    {
      serSession.Write("ABORT\n");
    }

    public void SetRange(int nRange, params int[] nChannels)
    {
      string sVolts;
      switch (nRange)
      {
        case 1:
          sVolts = "0.1";
          break;
        case 2:
          sVolts = "1";
          break;
        case 3:
          sVolts = "10";
          break;
        case 4:
          sVolts = "100";
          break;
        case 5:
          sVolts = "300";
          break;
        default:
          sVolts = "AUTO";
          break;
      }

      string sChannelString = "";
      int i;
      for (i = 0; i < (nChannels.Length - 1); i++)
      {
        sChannelString += (nChannels[i].ToString() + ",");
      }
      sChannelString += nChannels[i].ToString();

      serSession.Write("CONF:VOLT:DC " + sVolts + ", (@" + sChannelString + ")\n");
    }

    public double[] GetVolts(double dDelay, params int[] nChannels)
    {
      serSession.Write("ROUT:SCAN (@" + nChannels[0].ToString() + ")\n");
      double[] data = new double[nChannels.Length];
      string sResult = "0.0";

      string sChannelString = "";
      int i;
      for (i = 0; i < (nChannels.Length - 1); i++)
      {
        sChannelString += (nChannels[i].ToString() + ",");
      }
      sChannelString += nChannels[i].ToString();

      serSession.Write("TRIG:COUNT 1\n");

      if (dDelay <= 0)
      {
        serSession.Write("ROUT:CHAN:DELAY:AUTO ON\n");
      }
      else
      {
        string test = "ROUT:CHAN:DELAY " + dDelay.ToString("F3") + ",(@" + sChannelString + ")\n";
        serSession.Write(test);
      }

      serSession.Timeout = 60000; //Set timeout to 60 sec.
      
      sResult = serSession.Query("ROUT:SCAN (@" + sChannelString + ")\n INIT\n FETCH?\n");
      
      string[] split = sResult.Split(new char[] {','});

      for (int j = 0; j < nChannels.Length; j++)
      {
        data[j] = Convert.ToDouble(split[j]);
      }

      return data;
    }

    public double GetTemp(int nChannel)
    {
      string sResult = "0.0";

      sResult = serSession.Query("MEAS:TEMP? TCouple,J ,(@" + nChannel.ToString() + ")\n");
      return (Convert.ToDouble(sResult));
    }

    public void SetVolts(int nChannel,double dVolts)
    {
      serSession.Write("SOUR:VOLT " + dVolts.ToString() + ",(@" + nChannel.ToString() + ")\n");
    }

    public string GetID()
    {
      string sQuery = "*IDN?\n";
      string sResult = serSession.Query(sQuery);
      return (sResult);
    }

    public void SendCMD(string sCMD)
    {
      serSession.Write(sCMD);
    }
  }
}
