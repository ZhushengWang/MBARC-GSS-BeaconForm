using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPRL.Test
{
  public class EFSCmds
  {
    private static EFSCmds _efscmds = null;
    private static object _instanceLock = new Object();

    private COMCmds comCmds = null;

    //Command constants
    private const byte CLR_ERR = 0x10;
    private const byte RESET = 0x11;
    private const byte SET_BIN_TIME = 0x12;
    private const byte SET_PKT_EN = 0x13;
    private const byte DUMP_MEMORY = 0x14;


    //Packet constants
    public const byte ACK_ID = 1;
    public const byte HK_ID = 8;

    public FileStream compile;
    public FileStream tm_log;

    // This is a singleton class
    // Return a reference to the only instance of HPDAQCmds
    // Create the instance if necessary
    public static EFSCmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_efscmds == null)
          {
            _efscmds = new EFSCmds();
          }
          return _efscmds;
        }
      }
    }

    public void Init(string sInitString)
    {
      comCmds = COMCmds.Instance;

      comCmds.Init(sInitString);
    }

    private byte[] MakeBytes(int nValue)
    {
      byte[] ayBytes = new byte[4];

      ayBytes[3] = (byte)((nValue & 0xFF000000) >> 24);
      ayBytes[2] = (byte)((nValue & 0x00FF0000) >> 16);
      ayBytes[1] = (byte)((nValue & 0x0000FF00) >> 8);
      ayBytes[0] = (byte)((nValue & 0x000000FF));

      return (ayBytes);
    }

    private void AddSyncCode(ref byte[] ayCmd)
    {
      ayCmd[0] = 0xFA;
      ayCmd[1] = 0xF3;
      ayCmd[2] = 0x20;
    }

    private void AddChecksum(ref byte[] ayCmd)
    {
      int nChecksum = 0;
      byte[] ayBytes = new byte[4];

      for (int i = 0; i < ayCmd.Length - 2; i++)
      {
        nChecksum += ayCmd[i];
      }

      ayBytes = MakeBytes(nChecksum);

      ayCmd[ayCmd.Length - 2] = ayBytes[1];
      ayCmd[ayCmd.Length - 1] = ayBytes[0];
    }

    private void WriteOrSend(byte[] ayCmd)
    {
      if (compile != null)
      {
        compile.Write(ayCmd, 0, ayCmd.Length);
      }
      else
      {
        comCmds.Send(ayCmd);
      }
    }

    public void reset()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = RESET;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void set_bin_time(double dBinTime)
    {
      byte[] ayCmd = new byte[10];
      byte[] temp = new byte[4];

      if ((dBinTime >= 1.0) && (dBinTime <= 102.4))
      {
        AddSyncCode(ref ayCmd);
        ayCmd[3] = SET_BIN_TIME;
        ayCmd[4] = 0;
        ayCmd[5] = 2;
        int dBT = (int)(dBinTime * 10);
        temp = MakeBytes(dBT);

        ayCmd[6] = temp[1];
        ayCmd[7] = temp[0];

        AddChecksum(ref ayCmd);

        WriteOrSend(ayCmd);
      }
      else
      {
        MainForm._mainform.PrintError("Error:  Bad Parameter:" + dBinTime.ToString() + "\n");
      }
    }

    public void send_bytes(params byte[] byCmd)
    {
      byte[] ayCmd = new byte[byCmd.Length+5];

      AddSyncCode(ref ayCmd);

      for (int i = 0; i < byCmd.Length; i++)
      {
        ayCmd[i + 3] = byCmd[i];
      }

      AddChecksum(ref ayCmd);
      WriteOrSend(ayCmd);
    }

    public void send_byte_arr(byte[] byCmd)
    {
      byte[] ayCmd = new byte[byCmd.Length + 5];

      AddSyncCode(ref ayCmd);

      for (int i = 0; i < byCmd.Length; i++)
      {
        ayCmd[i + 3] = byCmd[i];
      }

      AddChecksum(ref ayCmd);
      WriteOrSend(ayCmd);
    }

    public int[] get_last_hk()
    {
      return (MainForm._mainform.hkForm.LastHK);
    }

    public int[] get_last_ack()
    {
      return (MainForm._mainform.seqForm.LastAck);
    }

    public void enable_tm_log(string sLogFile, bool bOpen)
    {
      if (bOpen)
      {
        if (tm_log != null)
        {
          tm_log.Close();
        }
        tm_log = new FileStream(sLogFile, FileMode.Create);
      }
      else
      {
        if (tm_log != null)
        {
          tm_log.Close();
          tm_log = null;
        }
      }
    }
 }
}
