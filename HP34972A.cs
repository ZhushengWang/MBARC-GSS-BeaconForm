using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SPRL.Test
{
  public class HP34972A
  {
    private Socket socSocket;

    public HP34972A(string sIP)
    {
      socSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
      socSocket.Connect(sIP, 5025);

      if (socSocket.Connected == true)
      {
        MainForm._mainform.PrintMsg("Socket Connected!\n");
      }
      else
      {
        MainForm._mainform.PrintError("Socket Connection Failed!\n");
      }
      //serSession.Write("*RST\n");
    }

    public void dispose()
    {
      socSocket.Close();
      //serSession.Write("ABORT\n");
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

      SendCMD("CONF:VOLT:DC " + sVolts + ", (@" + sChannelString + ")\n");
    }

    public double[] GetVolts(double dDelay, params int[] nChannels)
    {
      SendCMD("ROUT:SCAN (@" + nChannels[0].ToString() + ")\n");
      double[] data = new double[nChannels.Length];
      string sResult = "0.0";

      string sChannelString = "";
      int i;
      for (i = 0; i < (nChannels.Length - 1); i++)
      {
        sChannelString += (nChannels[i].ToString() + ",");
      }
      sChannelString += nChannels[i].ToString();

      SendCMD("TRIG:COUNT 1\n");

      if (dDelay <= 0)
      {
        SendCMD("ROUT:CHAN:DELAY:AUTO ON\n");
      }
      else
      {
        string test = "ROUT:CHAN:DELAY " + dDelay.ToString("F3") + ",(@" + sChannelString + ")\n";
        SendCMD(test);
      }

      //serSession.Timeout = 60000; //Set timeout to 60 sec.
      
      sResult = Query("ROUT:SCAN (@" + sChannelString + ")\n INIT\n FETCH?\n");
      
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

      sResult = Query("MEAS:TEMP? TCouple,J ,(@" + nChannel.ToString() + ")\n");
      return (Convert.ToDouble(sResult));
    }

    public void SetVolts(int nChannel,double dVolts)
    {
      SendCMD("SOUR:VOLT " + dVolts.ToString() + ",(@" + nChannel.ToString() + ")\n");
    }

    public string GetID()
    {
      string sQuery = "*IDN?\n";
      string sResult = Query(sQuery);
      return (sResult);
    }

    public string Query(string sCMD)
    {
      SendCMD(sCMD);
      return (GetResp());
    }

    public string GetResp()
    {
      byte[] buffer= new byte[100];
      string sResp;

      socSocket.Receive(buffer);
      sResp = Encoding.UTF8.GetString(buffer);
      return (sResp);
    }

    public void SendCMD(string sCMD)
    {
      byte[] buffer;

      buffer = Encoding.UTF8.GetBytes(sCMD);

      socSocket.Send(buffer);
      //serSession.Write(sCMD);
    }
  }
}
