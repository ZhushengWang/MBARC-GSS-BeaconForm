using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using BufUtilsLib;
using System.IO.Ports;
using System.Collections;

namespace SPRL.Test
{
  public class SimCmds
  {
    private static SimCmds _simcmds = null;
		private static object _instanceLock = new Object();
    private bool bSCSIM_Output_Enabled = true;
    private Socket socSocket;

    private AsyncCallback pfnCallBack;
    
    private byte[] m_DataBuffer = new byte[5000];
    private IAsyncResult m_asynResult;
    private UInt16 TCCounter = 0;

    private Queue syncedQueue = Queue.Synchronized(new Queue());

		// This is a singleton class
		// Return a reference to the only instance of HPDAQCmds
		// Create the instance if necessary
		public static SimCmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_simcmds == null)
					{
						_simcmds = new SimCmds();
					}
					return _simcmds;
				}
			}
		}

		// Constructor
		public SimCmds()
		{
		}

    public void Disc()
    {
      if (socSocket != null)
      {
        if (socSocket.Connected)
        {
          socSocket.Shutdown(SocketShutdown.Both);
          socSocket.Disconnect(false);
        }
        socSocket = null;
        pfnCallBack = null;
        MainForm._mainform.PrintMsg("Socket Disconnected.\n");
      }
      syncedQueue.Clear();
    }

    public void Conn()
    {
      Conn("192.168.1.193");  //Connect to default IP
    }

    public void Conn(string sIP)
    {
      Disc();

      if (socSocket == null)
      {
        socSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        socSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
      }

      try
      {
        socSocket.Connect(sIP, 28000);

        if (socSocket.Connected == true)
        {
          MainForm._mainform.PrintMsg("Socket Connected!\n");
          WaitForData();
        }
        else
        {
          MainForm._mainform.PrintError("Socket Connection Failed!\n");
          Disc();
        }
      }
      catch (SocketException e)
      {
        MainForm._mainform.PrintError("Socket Connection Failed: "+e.Message+"\n");
        Disc();
      }
    }

    public void WaitForData()
    {
      if (pfnCallBack == null)
        pfnCallBack = new AsyncCallback(OnDataReceived);
      // now start to listen for any data...
      m_asynResult =
      socSocket.BeginReceive(m_DataBuffer, 0, m_DataBuffer.Length, SocketFlags.None, pfnCallBack, null);
    }

    public void OnDataReceived(IAsyncResult asyn)
    {
      if (socSocket == null)
      {
        Disc();
      }
      //end receive...
      //Data is in m_Databuffer
      //iRx is number of bytes received
      try
      {
        int iRx = 0;
        if ((asyn == m_asynResult) && (socSocket != null))
        {
          iRx = socSocket.EndReceive(asyn);

          if (iRx > 0)
          {
            byte[] localbuf = new byte[iRx];
            Array.Copy(m_DataBuffer, localbuf, iRx);
            syncedQueue.Enqueue(localbuf);
            WaitForData();
          }
          else
          {
            Disc();
          }
        }
        else
        {
          Disc();
        }
      }
      catch (SocketException e)
      {
        MainForm._mainform.PrintMsg("Socket Exception: "+e.Message+"\n");
        Disc();
      }
    }

    public bool bConnected()
    {
      if (socSocket != null)
      {
        return (socSocket.Connected);
      }
      else
      {
        return (false);
      }
    }

    public bool bNetDataAvailable()
    {
      if (syncedQueue.Count > 0)
      {
        return (true);
      }
      else
      {
        return (false);
      }
    }

    public byte[] GetNetData()
    {
      return ((byte[])syncedQueue.Dequeue());
    }

    public void SendBytes(byte [] ayBytes)
    {
      if ((socSocket != null) && (socSocket.Connected == true))
      {
        socSocket.Send(ayBytes);
      }
      else
      {
        for (int i = 0; i < ayBytes.Length; i++)
        {
          MainForm._mainform.PrintMsg("Cmd Byte" + i + ": 0x" + ayBytes[i].ToString("X2") + "\n");
        }
        MainForm._mainform.PrintError("Socket Not Connected!\n");
      }
    }

    private byte[] GetCommandArray(byte OpCode)
    {
      byte[] CmdArray = new byte[9];

      CmdArray[0] = (byte)(0xC8 | ((OpCode & 0xE0) >> 5));
      CmdArray[1] = (byte)(((TCCounter & 0x0700) >> 8) | (byte)((OpCode & 0x1F) << 3)); 
      CmdArray[2] = (byte)(TCCounter & 0xFF);

      CmdArray[3] = 0x00;
      CmdArray[4] = 0x04;  //Length is fixed
      
      TCCounter++;
      return (CmdArray);
    }

    private void SendCommand(byte OpCode, UInt32 ui32Data)
    {
      byte[] Cmd = GetCommandArray(OpCode);
      byte[] Parm = BitConverter.GetBytes(ui32Data);//Returns bytes in little-endian

      Cmd[5] = Parm[3];
      Cmd[6] = Parm[2];
      Cmd[7] = Parm[1];
      Cmd[8] = Parm[0];

      SendBytes(Cmd);
    }

    private void SendCommand(byte OpCode, float fData)
    {
      byte[] Cmd = GetCommandArray(OpCode);
      byte[] Parm = BitConverter.GetBytes(fData);//Returns bytes in little-endian

      Cmd[5] = Parm[3];
      Cmd[6] = Parm[2];
      Cmd[7] = Parm[1];
      Cmd[8] = Parm[0];

      SendBytes(Cmd);
    }

    public void nop(UInt32 dummy)
    {
      SendCommand(0x80, dummy);
    }

    public void reset()
    {
      SendCommand(0x81, 0);
    }

    public void reset_fpga()
    {
      SendCommand(0x82, 0);
    }

    public void set_rover_time(UInt32 secs)
    {
      SendCommand(0x83, secs);
    }

    public void set_ps_voltage(float volts)
    {
      SendCommand(0x84, volts);
    }

    public void set_ps_i_limit(float current)
    {
      SendCommand(0x85, current);
    }

    public void enable_ps_output()
    {
      SendCommand(0x86, 0);
    }
    public void disable_ps_output()
    {
      SendCommand(0x87, 0);
    }

    public void set_bus1_i_limit(float current)
    {
      SendCommand(0x88, current);
    }
    public void set_bus2_i_limit(float current)
    {
      SendCommand(0x89, current);
    }
    public void set_bus3_i_limit(float current)
    {
      SendCommand(0x8A, current);
    }

    public void enable_bus1()
    {
      SendCommand(0x8B, 0);
    }
    public void disable_bus1()
    {
      SendCommand(0x8C,0);
    }

    public void enable_bus2()
    {
      SendCommand(0x8D, 0);
    }
    public void disable_bus2()
    {
      SendCommand(0x8E, 0);
    }

    public void enable_bus3()
    {
      SendCommand(0x8F, 0);
    }
    public void disable_bus3()
    {
      SendCommand(0x90, 0);
    }

    public void enable_tm()
    {
      SendCommand(0x91, 0);
    }

    public void disable_tm()
    {
      SendCommand(0x92, 0);
    }

    public void set_hour_meter(float time)
    {
      SendCommand(0x93, time);
    }

    public void set_oc_trip_mode(UInt32 mode)
    {
      SendCommand(0x94, mode);
    }

    public void set_oc_trip_time(UInt32 time)
    {
      SendCommand(0x95, time);
    }

    public void reset_oc_trip()
    {
      SendCommand(0x96, 0);
    }

    public void OutputEnable(bool bEnable)
    {
      bSCSIM_Output_Enabled = bEnable;
    }

    public void CalcCSUM(params byte[] bData)
    {
      int csum = ComputeIpChecksum(bData, 0, bData.Length);
      csum = ~csum;
      MainForm._mainform.PrintMsg("CSum = "+csum.ToString("X4")+"\n");
    }

    private ushort ComputeIpChecksum(byte[] buf, int start, int length)
    {
      try
      {
        ushort word16;
        long sum = 0;
        for (int i = start; i < (length + start); i += 2)
        {
          word16 = (ushort)(((buf[i] << 8) & 0xFF00)
              + (buf[i + 1] & 0xFF));
          sum += (long)word16;
        }
        while ((sum >> 16) != 0)
        {
          sum = (sum & 0xFFFF) + (sum >> 16);
        }
        return (ushort)sum;
      }
      catch (IndexOutOfRangeException)
      {
        return 0;
      }
    }
  }
}
