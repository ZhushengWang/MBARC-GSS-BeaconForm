using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class NI9403Cmds
  {
    private static NI9403Cmds _ni9403cmds = null;
		private static object _instanceLock = new Object();
    private NI9403[] ni9403 = new NI9403[8];
    private int[] dispArray;
    private LogFile logFile;

    // This is a singleton class
		// Return a reference to the only instance of cDAQCmds
		// Create the instance if necessary
		public static NI9403Cmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_ni9403cmds == null)
					{
						_ni9403cmds = new NI9403Cmds();
					}
					return _ni9403cmds;
				}
			}
		}

		// Constructor
		public NI9403Cmds()
		{
      dispArray = new int[32];
      logFile = LogFile.Instance;
		}

    public void dispose(int nSlot)
    {
      ni9403[nSlot].dispose();
      ni9403[nSlot] = null;
    }

    private string chanToString(int[] channels)
    {
      string sChannels = "";
      for (int i = 0; i < channels.Length; i++)
      {
        sChannels += channels[i].ToString();
        if (i < (channels.Length - 1))
        {
          sChannels += ",";
        }
      }
      return sChannels;
    }

    public void Init(int nSlot, string sDevID)
    {
      ni9403[nSlot] = new NI9403(sDevID);
      logFile.WriteLog("cmd: NI9403.Init(" + nSlot.ToString() + "," + sDevID + ")");
    }

    public void WriteWord(int nSlot, int nWord, params int[] channels)
    {
      ni9403[nSlot].writeWord(nWord, channels);
      logFile.WriteLog("cmd: NI9403.WriteWord(" + nSlot.ToString() + "," + nWord.ToString() + "," + chanToString(channels) + ")");
    }

    public void Pulse(int nSlot, int nDuration, int nChannel) 
    {
      ni9403[nSlot].pulse(nDuration, nChannel);
      logFile.WriteLog("cmd: NI9403.Pulse(" + nSlot.ToString() + "," + nDuration.ToString() + "," + nChannel.ToString() + ")");
    }

    public void SetChannelsOn(int nSlot, params int[] channels)  //Sets given channels to 1
    {
      ni9403[nSlot].setChannels(true, channels);
      logFile.WriteLog("cmd: NI9403.SetChannelsOn(" + nSlot.ToString() + "," + chanToString(channels) + ")");
    }

    public void SetChannelsOff(int nSlot, params int[] channels) //Sets given channels to 0
    {
      ni9403[nSlot].setChannels(false, channels);
      logFile.WriteLog("cmd: NI9403.SetChannelsOff(" + nSlot.ToString() + "," + chanToString(channels) + ")");
    }

    public void SetAllChannelsOff(int nSlot)
    {
      ni9403[nSlot].setChannels(false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31);
      logFile.WriteLog("cmd: NI9403.SetAllChannelsOff(" + nSlot.ToString() + ")");
    }

    public bool[] ChannelList(int nSlot)                       //Outputs the channels' statuses 
    {
      for (int i = 0; i < 32; i++)
      {
        if (ni9403[nSlot].dataArray[i])
        {
          dispArray[i] = 1;
        }
        else
        {
          dispArray[i] = 0;
        }

        MainForm._mainform.PrintMsg(dispArray[i].ToString());
      }
      MainForm._mainform.PrintMsg("\n");
      logFile.WriteLog("cmd: NI9403.channelList(" + nSlot.ToString() + ")");
      return ni9403[nSlot].dataArray;
    }

  }
}
