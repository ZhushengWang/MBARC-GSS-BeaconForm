using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPRL.Test
{
  public class QB50Cmds
  {
    private static QB50Cmds _qb50cmds = null;
    private static object _instanceLock = new Object();

    private COMCmds comCmds = null;

    //Command constants
    private const byte RESET_CPU = 0x12;
    private const byte LOAD_PROM = 0x13;

    private const byte WRITE_BYTE     = 0x80;
    private const byte READ_BYTE      = 0x81;
    private const byte READ_CDH_ADC   = 0x82;
    private const byte READ_CDH_TEMP  = 0x83;
    private const byte SEND_LI1_NOP   = 0x84;
    private const byte READ_LI1_BYTES = 0x85;
    private const byte READ_LI1_CONFIG= 0x86;
    private const byte SET_LI1_CONFIG = 0x87;
    private const byte GO_BOOT_CMD    = 0x88;
    private const byte RESET_NV       = 0x89;
    private const byte RESET_CDH      = 0x8A;
    private const byte SEND_LI1_BEACON_CONFIG  =  0x8B;
    private const byte SEND_LI1_BEACON_DATA    =  0x8C;
    private const byte SEND_LI1_QUERY_TLM      =  0x8D;
    private const byte SEND_LI1_TRANSMIT       =  0x8E;
    private const byte SET_LI1_POWER           = 0x8F;
    private const byte SET_LI1_TEST_MODE       = 0x90;
    private const byte LI1_TEST_PACKET         = 0x91;  //Unused
    private const byte LI1_READ_FW_REV         = 0x92;
    private const byte ADC_READ                = 0x93;
    private const byte MAG_READ                = 0x94;
    private const byte MAG_READ_ID             = 0x95;
    private const byte READ_ADC_TEMP           = 0x96;
    private const byte CDH_SET_DAC             = 0x97;
    private const byte MTQ_SETX = 0x98;
    private const byte MTQ_SETY = 0x99;
    private const byte MTQ_SETZ = 0x9A;
    private const byte MTQ_SET_FREQ = 0x9B;
    private const byte IMU2_READ_DATA = 0x9C;
    private const byte IMU1_READ_DATA = 0x9D;
    private const byte MRAM_WRITE_DATA = 0x9E;
    private const byte MRAM_READ_DATA = 0x9F;
    private const byte FSW_VERIFY_CODE = 0xA0;
    private const byte FSW_COPY_CODE = 0xA1;
    private const byte FSW_BOOT = 0xA2;
    private const byte FSW_SET_BOOT_PART = 0xA3;
    private const byte FIP_READ_BYTES = 0xA4;
    private const byte FIP_PING_CMD = 0xA5;
    private const byte FIP_SC_CMD = 0xA6;
    private const byte FIP_HK_CMD = 0xA7;
    private const byte FIP_STDBY_CMD = 0xA8;
    private const byte LI1_TX_ENABLE  =      0xA9;
    private const byte MSG_TX_ENABLE  =      0xAA;
    private const byte ACK_TX_ENABLE  =      0xAB;
    private const byte SET_INIT_WAIT  =      0xAC;
    private const byte READ_OP_STATUS =      0xAD;
    private const byte SET_TIME = 0xAE;
    private const byte SET_LAUNCH = 0xAF;
    private const byte SET_ENABLE_DEPLOY =   0xB0;
    private const byte SET_PHASE = 0xB1;
    private const byte FIPEX_LOAD_SCRIPT = 0xB2;
    private const byte FIPEX_VERIFY_SCRIPT = 0xB3;
    private const byte FIPEX_SET_ACT_SCRIPT = 0xB4;
    private const byte FIPEX_PRINT_SCRIPT = 0xB5;
    private const byte FIPEX_LIST_SCRIPTS = 0xB6;
    private const byte FIPEX_ERASE_SCRIPT = 0xB7;
    private const byte FIPEX_SCRIPT_PROC = 0xB8;
    private const byte HK_TX_ENABLE = 0xB9;
    private const byte FIPEX_ERASE_DATA = 0xBA;
    private const byte FIPEX_RETRIEVE_DATA = 0xBB;
    private const byte FIPEX_INIT_RETRIEVAL = 0xBC;
    private const byte FIP_DP_CMD = 0xBD;
    private const byte INIT_DEPLOY_COUNT = 0xBE;
    private const byte RINGBUFFER_ENABLE = 0xBF;
    private const byte RINGBUFFER_READ = 0xC0;
  
    //Packet constants
    public const byte ACK_ID       = 1;
    public const byte BEACON_ID    = 8;
    public const byte MSG_ID       = 9;
    public const byte HK_ID        = 7;
    public const byte TEST_ID      = 6;
    public const byte SU_ID        = 2;
    public const byte SU_STORED_ID = 4;
    public const byte MAG_ID       = 10;

    public FileStream compile;
    public FileStream tm_log;

    private String sRetrieveFileName;
    private String sMagOneFileName = "mag_one_calibration_data";
    private String sMagTwoFileName = "mag_two_calibration_data";
    private String sMagThreeFileName = "mag_three_calibration_data";
    private String sMagFourFileName = "mag_four_calibration_data";

    private const byte SELECT_SP1_MAG = 0;
    private const byte SELECT_SP2_MAG = 1;
    private const byte SELECT_SP3_MAG = 2;
    private const byte SELECT_SP4_MAG = 3;

    // This is a singleton class
    // Return a reference to the only instance of HPDAQCmds
    // Create the instance if necessary
    public static QB50Cmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_qb50cmds == null)
          {
            _qb50cmds = new QB50Cmds();
          }
          return _qb50cmds;
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
        //for (int i = 0; i < ayCmd.Length; i++)
        //{
        //  MainForm._mainform.PrintMsg(ayCmd[i].ToString("X2") + "\n");
        //}
        comCmds.Send(ayCmd);
      }
    }

    public void Write(byte yAddr, byte yVal)
    {
      byte[] ayCmd = new byte[10];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = WRITE_BYTE;
      ayCmd[4] = 0;
      ayCmd[5] = 2;
      ayCmd[6] = yAddr;
      ayCmd[7] = yVal;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read(byte yAddr)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_BYTE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yAddr;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_CDH_ADC()
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_CDH_ADC;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = 0; //Dummy Byte

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_ADC(byte yADC)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = ADC_READ;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yADC; 

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_Mag(byte yMag)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MAG_READ;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yMag;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_Mag_ID(byte yMag)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MAG_READ_ID;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yMag;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_CDH_Temp()
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_CDH_TEMP;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = 0; //Dummy Byte

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_ADC_Temp(byte yADC)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_ADC_TEMP;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yADC; 

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_DAC(UInt16 wDacVal)
    {
      byte[] ayCmd = new byte[10];

      if (wDacVal < 4096)
      {
        AddSyncCode(ref ayCmd);
        ayCmd[3] = CDH_SET_DAC;
        ayCmd[4] = 0;
        ayCmd[5] = 2;
        ayCmd[6] = (byte)(wDacVal >> 8);
        ayCmd[7] = (byte)(wDacVal & 0xFF);

        AddChecksum(ref ayCmd);

        WriteOrSend(ayCmd);
      }
      else
      {
        MainForm._mainform.PrintError("Error:  Bad DAC value.\n");
      }
    }

    public void Set_LI1_TX_Enable(byte yOn)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = LI1_TX_ENABLE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yOn;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }
    public void Ringbuffer_Enable(byte yOn)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = RINGBUFFER_ENABLE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yOn;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);

    }
    public void Ringbuffer_Read(byte input)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = RINGBUFFER_READ;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = input;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);

    }
    public void Set_LI1_HK_Enable(byte yOn)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = HK_TX_ENABLE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yOn;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_LI1_Msg_Enable(byte yOn)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MSG_TX_ENABLE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yOn;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_LI1_Ack_Enable(byte yOn)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = ACK_TX_ENABLE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yOn;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_Initial_Wait(int nWait)
    {
      byte[] ayCmd = new byte[12];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_INIT_WAIT;
      ayCmd[4] = 0;
      ayCmd[5] = 4;

      byte[] ayTemp = MakeBytes(nWait);
      ayCmd[6] = ayTemp[3];
      ayCmd[7] = ayTemp[2];
      ayCmd[8] = ayTemp[1];
      ayCmd[9] = ayTemp[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_Time(int nTime)
    {
      byte[] ayCmd = new byte[12];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_TIME;
      ayCmd[4] = 0;
      ayCmd[5] = 4;

      byte[] ayTemp = MakeBytes(nTime);
      ayCmd[6] = ayTemp[3];
      ayCmd[7] = ayTemp[2];
      ayCmd[8] = ayTemp[1];
      ayCmd[9] = ayTemp[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_Op_Status(byte yStatStruct)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_OP_STATUS;
      ayCmd[4] = 0;
      ayCmd[5] = 1;

      ayCmd[6] = yStatStruct;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void InitDeployCount()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = INIT_DEPLOY_COUNT;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_Launch()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_LAUNCH;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_LI1_Test_Mode(byte yOn)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_LI1_TEST_MODE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yOn; 

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_LI1_Beacon_Data(byte[] ayData)
    {
      if (ayData.Length <= 255)
      {
        byte[] ayCmd = new byte[8 + ayData.Length];

        AddSyncCode(ref ayCmd);
        ayCmd[3] = SEND_LI1_BEACON_DATA;
        ayCmd[4] = 0;
        ayCmd[5] = (byte)ayData.Length;

        for (int i = 0; i < ayData.Length; i++)
        {
          ayCmd[6 + i] = ayData[i];
        }

        AddChecksum(ref ayCmd);

        WriteOrSend(ayCmd);
      }
      else
      {
        MainForm._mainform.PrintError("Error:  Beacon data must be 255 bytes or less.\n");
      }
    }

    public void Send_LI1_Beacon_Config(byte yInterval)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SEND_LI1_BEACON_CONFIG;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yInterval; 

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_LI1_Power(byte yPower)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_LI1_POWER;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yPower;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Send_LI1_NOP()
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SEND_LI1_NOP;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = 0; //Dummy Byte

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_LI1_Bytes()
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_LI1_BYTES;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = 0; //Dummy Byte

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_LI1_Config()
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = READ_LI1_CONFIG;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = 0; //Dummy Byte

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Query_LI1_TLM()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SEND_LI1_QUERY_TLM;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Read_LI1_FW()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = LI1_READ_FW_REV;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_LI1_Config(int nFreq, String sCall, Byte yPwr, int nConfig, int nConfig2)
    {
      byte[] ayCmd = new byte[23];
      byte[] ayTemp;

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_LI1_CONFIG;
      ayCmd[4] = 0;
      ayCmd[5] = 15;

      ayTemp = MakeBytes(nFreq);

      ayCmd[6] = ayTemp[3];  //Send as big endian
      ayCmd[7] = ayTemp[2];
      ayCmd[8] = ayTemp[1];
      ayCmd[9] = ayTemp[0];

      for (int i = 0; i < 6; i++)
      {
        if (i >= sCall.Length)
        {
          ayCmd[10 + i] = (byte)' ';
        }
        else
        {
          ayCmd[10 + i] = (byte)sCall[i];
        }
      }

      ayCmd[16] = yPwr;

      ayTemp = MakeBytes(nConfig);

      ayCmd[17] = ayTemp[1];  //Send as big endian
      ayCmd[18] = ayTemp[0];

      ayTemp = MakeBytes(nConfig2);

      ayCmd[19] = ayTemp[1];  //send as big endian
      ayCmd[20] = ayTemp[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void MTQ_SetX(Int16 i16Val)
    {
      byte[] ayCmd = new byte[10];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MTQ_SETX;
      ayCmd[4] = 0;
      ayCmd[5] = 2;
      ayCmd[6] = (byte)(i16Val >> 8);
      ayCmd[7] = (byte)(i16Val & 0xFF);

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void MTQ_SetY(Int16 i16Val)
    {
      byte[] ayCmd = new byte[10];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MTQ_SETY;
      ayCmd[4] = 0;
      ayCmd[5] = 2;
      ayCmd[6] = (byte)(i16Val >> 8);
      ayCmd[7] = (byte)(i16Val & 0xFF);

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void MTQ_SetZ(Int16 i16Val)
    {
      byte[] ayCmd = new byte[10];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MTQ_SETZ;
      ayCmd[4] = 0;
      ayCmd[5] = 2;
      ayCmd[6] = (byte)(i16Val >> 8);
      ayCmd[7] = (byte)(i16Val & 0xFF);

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void MTQ_SetFreq(UInt16 ui16Val)
    {
      byte[] ayCmd = new byte[10];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = MTQ_SET_FREQ;
      ayCmd[4] = 0;
      ayCmd[5] = 2;
      ayCmd[6] = (byte)(ui16Val >> 8);
      ayCmd[7] = (byte)(ui16Val & 0xFF);

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void IMU2_ReadData()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = IMU2_READ_DATA;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void IMU1_ReadData()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = IMU1_READ_DATA;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_ReadData()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIP_READ_BYTES;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_HK()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIP_HK_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_DP()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIP_DP_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_SC()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIP_SC_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_Ping()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIP_PING_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_Standby()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIP_STDBY_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_VerifyScript(byte yScript)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_VERIFY_SCRIPT;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yScript;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_SetActiveScript(byte yScript)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_SET_ACT_SCRIPT;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yScript;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_PrintScript(byte yScript)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_PRINT_SCRIPT;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yScript;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_ListScripts()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_LIST_SCRIPTS;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_EraseScript(byte yScript)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_ERASE_SCRIPT;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yScript;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_EnableScriptProc(byte yEnable)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_SCRIPT_PROC;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yEnable;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_EraseData(byte yMem, UInt32 ui32Time)
    {
      byte[] ayCmd = new byte[13];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_ERASE_DATA;
      ayCmd[4] = 0;
      ayCmd[5] = 5;
      ayCmd[6] = yMem;

      byte[] ayTemp = MakeBytes((int)ui32Time);

      ayCmd[7] = ayTemp[3];  //Send as big endian
      ayCmd[8] = ayTemp[2];
      ayCmd[9] = ayTemp[1];
      ayCmd[10] = ayTemp[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_InitRetrieval()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_INIT_RETRIEVAL;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_RetrieveData(byte yMem, String sFile)
    {
      byte[] ayCmd = new byte[9];

      sRetrieveFileName = sFile;

      AddSyncCode(ref ayCmd);
      ayCmd[3] = FIPEX_RETRIEVE_DATA;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yMem;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void FIP_ProcessData(List<byte> listPkt)
    {
      string sSDPName = sRetrieveFileName+".SDP";
      string sHKName  = sRetrieveFileName+".HK";

      int nPktLen = (listPkt[4] << 8) + listPkt[5];

      if (nPktLen == 0)
      {
        MainForm._mainform.PrintMsg("Retrieved Packet is 0 length.\n");
        return;
      }

      FileStream fsSDP = new FileStream(sSDPName, FileMode.Append);
      FileStream fsHK  = new FileStream(sHKName, FileMode.Append);

      byte[] ayAttData = listPkt.GetRange(6, 24).ToArray(); //Get attitude Data

      byte[] ayAttDataLE = new byte[24];

      ayAttDataLE[0] = ayAttData[3];  //Time to Little Endian
      ayAttDataLE[1] = ayAttData[2];
      ayAttDataLE[2] = ayAttData[1];
      ayAttDataLE[3] = ayAttData[0];

      ayAttDataLE[4] = ayAttData[5];  //Quaternion data to LE
      ayAttDataLE[5] = ayAttData[4];

      ayAttDataLE[6] = ayAttData[7];
      ayAttDataLE[7] = ayAttData[6];

      ayAttDataLE[8] = ayAttData[9];
      ayAttDataLE[9] = ayAttData[8];

      ayAttDataLE[10] = ayAttData[11];
      ayAttDataLE[11] = ayAttData[10];

      ayAttDataLE[12] = ayAttData[13];  //angular rates to LE
      ayAttDataLE[13] = ayAttData[12];

      ayAttDataLE[14] = ayAttData[15];
      ayAttDataLE[15] = ayAttData[14];

      ayAttDataLE[16] = ayAttData[17];
      ayAttDataLE[17] = ayAttData[16];

      ayAttDataLE[18] = ayAttData[19];  //Position to LE
      ayAttDataLE[19] = ayAttData[18];

      ayAttDataLE[20] = ayAttData[21];
      ayAttDataLE[21] = ayAttData[20];

      ayAttDataLE[22] = ayAttData[23];
      ayAttDataLE[23] = ayAttData[22];

      byte yType = listPkt[31];
      byte yLen = (byte)(listPkt[32] + 4);

      //Check the FIPEX packet XOR 
      //This is done over PktID,Len,Seq,Data,XOR
      //Since the XOR is included, it should result in 0
      byte yXOR_Check = 0;
      for (int i = 0; i < yLen; i++)
      {
        yXOR_Check = (byte)(yXOR_Check ^ listPkt[i + 31]);
      }

      byte[] ayPktData = listPkt.GetRange(31, yLen).ToArray(); //Get packet data

      UInt32 unTime = BitConverter.ToUInt32(ayAttDataLE, 0);
      int nTotalLen = yLen+24;

      if (yType == 0x20)
      {
        //Write HK Packet
        fsHK.Write(ayPktData, 0, yLen);
        fsHK.Write(ayAttDataLE, 0, 24);
        MainForm._mainform.PrintMsg("Saving SU_HK Packet, "+nTotalLen.ToString()+" Bytes, XOR="+yXOR_Check.ToString("X2")+" (T="+unTime.ToString()+").\n");
      }
      else
      {
        if (yType == 0x30)
        {
          //Write SDP Packet
          fsSDP.Write(ayPktData, 0, yLen);
          fsSDP.Write(ayAttDataLE, 0, 24);
          MainForm._mainform.PrintMsg("Saving SU_SDP Packet, " + nTotalLen.ToString() + " Bytes, XOR=" + yXOR_Check.ToString("X2") + " (T=" + unTime.ToString() + ").\n");
        }
        else
        {
          MainForm._mainform.PrintError("Error:  Bad Retrieved FIPEX Packet Type (" + yType.ToString("X2") + ")\n");
        }
      }
      
      fsSDP.Close();
      fsHK.Close();
    }
    public void Mag_StoreData(List<byte> listPkt)
    {
      int nPktLen = (listPkt[4] << 8) + listPkt[5];

      if (nPktLen == 0)
      {
        MainForm._mainform.PrintMsg("Retrieved Packet is 0 length.\n");
        return;
      }

      using (MemoryStream ms = new MemoryStream(listPkt.GetRange(6, nPktLen).ToArray())) {
      using (BinaryReader br = new BinaryReader(ms))
      {

        string sMAGname = sMagFileName + ".txt";

        FileStream fsMAG = new FileStream(sMAGname, FileMode.Append);

        //byte[] ayMagData = listPkt.GetRange(6, nPktLen).ToArray(); //Put magnetometer data into array

        //Write Mag Packet to file
        //fsMAG.Write(ayMagData, 0, nPktLen);

        using (StreamWriter sw = new StreamWriter(fsMAG))
        {
          while (ms.Position < ms.Length)
          {
            //
            // Convert UInt16 values into character-encoded values
            //
            //sw.Write(ReadUint16()); 
            sw.WriteLine(((float)((int)br.ReadUInt16()))/6842.0);
          }
        }
        MainForm._mainform.PrintMsg("Saving MAG Packet, " + nPktLen.ToString() + "\n");
        fsMAG.Close();
      }
      }
    }
    public string RemoveWhitespace(string input)
    {
      return new string(input.ToCharArray()
          .Where(c => !Char.IsWhiteSpace(c))
          .ToArray());
    }

    public byte[] ConvertHexStringToByteArray(string hexString1)
    {
      string hexString = RemoveWhitespace(hexString1);

      if (hexString.Length % 2 != 0)
      {
        MainForm._mainform.PrintError("Error:  File has an odd number of hex digits.\n");
        return null;
      }

      byte[] HexAsBytes = new byte[hexString.Length / 2];
      for (int index = 0; index < HexAsBytes.Length; index++)
      {
        string byteValue = hexString.Substring(index * 2, 2);
        HexAsBytes[index] = byte.Parse(byteValue, System.Globalization.NumberStyles.HexNumber, 
                                                  System.Globalization.CultureInfo.InvariantCulture);
      }

      return HexAsBytes;
    }

    public void FIP_Load_Script(string sScriptFile, byte yScript)
    {
      try
      {
        string fileContent = File.ReadAllText(sScriptFile);

        byte[] fileBytes1 = ConvertHexStringToByteArray(fileContent); 
        int nNumToSend;
        int nOffset;
        byte[] temp;
        int nCommandBytes;
        const int nScriptLoadSize = 264;

        byte[] fileBytes = new byte[nScriptLoadSize];
        Array.Copy(fileBytes1, fileBytes, fileBytes1.Length);
        int nNumbytes = fileBytes.Length;

        byte[] loadBytes = new byte[nNumbytes + 8];
        UInt16 wCRC;

        loadBytes[0] = 0x5A;
        loadBytes[1] = 0xA5;
        loadBytes[2] = 0x12;
        loadBytes[3] = 0x34;

        temp = MakeBytes(nNumbytes);

        loadBytes[4] = temp[1];
        loadBytes[5] = temp[0];

        for (int i = 0; i < nNumbytes; i++)
        {
          loadBytes[i + 6] = fileBytes[i];
        }
        wCRC = crc16_ccitt(fileBytes);
        loadBytes[nNumbytes + 6] = (byte)((wCRC >> 8) & 0xFF);
        loadBytes[nNumbytes + 7] = (byte)(wCRC & 0xFF);

        nNumbytes += 8;
        nOffset = 0;
        while (nNumbytes > 0)
        {
          if (nNumbytes > 220)
          {
            nNumToSend = 220;
          }
          else
          {
            nNumToSend = nNumbytes;
          }

          nCommandBytes = nNumToSend + 3;  //3 for parms
          byte[] ayCmd = new byte[nCommandBytes + 8];  //8 for header and CRC

          AddSyncCode(ref ayCmd);
          ayCmd[3] = FIPEX_LOAD_SCRIPT;
          ayCmd[4] = (byte)(nCommandBytes >> 8);
          ayCmd[5] = (byte)(nCommandBytes & 0xFF);

          ayCmd[6] = yScript;
          ayCmd[7] = (byte)nOffset;
          ayCmd[8] = (byte)nNumToSend;
          
          for (int i=0;i<nNumToSend;i++)
          {
            ayCmd[9+i] = loadBytes[nOffset+i];
          }

          AddChecksum(ref ayCmd);

          WriteOrSend(ayCmd);      
          
          nNumbytes -= nNumToSend;
          nOffset += nNumToSend;
          System.Threading.Thread.Sleep(200);
        }
      }
      catch
      {
        MainForm._mainform.PrintError("File IO Error \n");
      }
    }

    public void Set_Enable_Deploy(byte yEnable)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_ENABLE_DEPLOY;
      ayCmd[4] = 0;
      ayCmd[5] = 1;

      ayCmd[6] = yEnable;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void Set_Boot_Phase(byte yPhase)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = SET_PHASE;
      ayCmd[4] = 0;
      ayCmd[5] = 1;

      ayCmd[6] = yPhase;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    //Same as go_boot_cmd()
    public void go()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = GO_BOOT_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void go_boot_cmd()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = GO_BOOT_CMD;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void reset_nv()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = RESET_NV;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }
    
    public void reset_cdh()
    {
      byte[] ayCmd = new byte[8];

      AddSyncCode(ref ayCmd);
      ayCmd[3] = RESET_CDH;
      ayCmd[4] = 0;
      ayCmd[5] = 0;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    //This is used by the FPGA during PROM load
    public void reset_cpu(bool bRes)
    {
      byte[] ayCmd = new byte[9];
      byte yRes;

      if (bRes)
      {
        yRes = 0;
      }
      else
      {
        yRes = 1;
      }

      AddSyncCode(ref ayCmd);
      ayCmd[3] = RESET_CPU;
      ayCmd[4] = 0;
      ayCmd[5] = 1;
      ayCmd[6] = yRes;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void load_prom(string sFilename)
    {
      try
      {
        byte[] fileBytes = File.ReadAllBytes(sFilename);
        int nNumbytes = fileBytes.Length;
        int nNumToSend;
        int nTotalBytes;
        int nLoadAddress = 0;

        reset_cpu(true);  //Disable the CPU for the load

        while (nNumbytes > 0)
        {
          if (nNumbytes > 4000)
          {
            nNumToSend = 4000;
          }
          else
          {
            nNumToSend = nNumbytes;
          }

          nTotalBytes = nNumToSend + 3;
          byte[] ayCmd = new byte[nTotalBytes + 8];

          AddSyncCode(ref ayCmd);
          ayCmd[3] = LOAD_PROM;
          ayCmd[4] = (byte)(nTotalBytes >> 8);
          ayCmd[5] = (byte)(nTotalBytes & 0xFF);

          byte[] ayAddrBytes = MakeBytes(nLoadAddress);
          ayCmd[6] = ayAddrBytes[2];
          ayCmd[7] = ayAddrBytes[1];
          ayCmd[8] = ayAddrBytes[0];

          for (int i = 0; i < nNumToSend; i++)
          {
            ayCmd[9 + i] = fileBytes[nLoadAddress + i];
          }

          AddChecksum(ref ayCmd);

          WriteOrSend(ayCmd);

          nLoadAddress += nNumToSend;
          nNumbytes -= nNumToSend;
        }

        reset_cpu(false);  //Disable the CPU for the load
      }
      catch
      {
        MainForm._mainform.PrintError("File IO Error \n");
      }
    }

    public void MRAM_Write(byte yCode_Data, byte yChip, UInt32 ui32Addr, byte[] ayData)
    {
      byte[] ayCmd = new byte[ayData.Length + 8 + 5];
      int nTotalBytes = ayData.Length+5;

      AddSyncCode(ref ayCmd);

      ayCmd[3] = MRAM_WRITE_DATA;
      ayCmd[4] = (byte)((nTotalBytes >> 8) & 0xFF);
      ayCmd[5] = (byte)(nTotalBytes & 0xFF);

      ayCmd[6] = yCode_Data;
      ayCmd[7] = yChip;

      byte[] ayAddrBytes = MakeBytes((int)ui32Addr);
      ayCmd[8] = ayAddrBytes[2];
      ayCmd[9] = ayAddrBytes[1];
      ayCmd[10] = ayAddrBytes[0];

      for (int i = 0; i < ayData.Length; i++)
      {
        ayCmd[11 + i] = ayData[i];
      }

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void MRAM_Read(byte yCode_Data, byte yChip, UInt32 ui32Addr, byte yLen)
    {
      byte[] ayCmd = new byte[8 + 6];

      AddSyncCode(ref ayCmd);

      ayCmd[3] = MRAM_READ_DATA;
      ayCmd[4] = 0x00;
      ayCmd[5] = 0x06;

      ayCmd[6] = yCode_Data;
      ayCmd[7] = yChip;

      byte[] ayAddrBytes = MakeBytes((int)ui32Addr);
      ayCmd[8]  = ayAddrBytes[2];
      ayCmd[9]  = ayAddrBytes[1];
      ayCmd[10] = ayAddrBytes[0];

      ayCmd[11] = yLen; 

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void LoadFSWPart(string sFilename, int nTotal, int nFileOffset, int nPktSize,
                                   int nVer, byte yCode_Data, byte yChip, UInt32 uiAddr)
    {
      try
      {
        byte[] fileBytes = File.ReadAllBytes(sFilename);
        int nNumbytes = fileBytes.Length;
        int nNumToSend;
        int nOffset;
        int nLoadAddress = (int)uiAddr;
        byte[] loadBytes = new byte[nNumbytes + 14];
        UInt16 wCRC;
        bool bWaitForAck;

        loadBytes[0] = 0x1A;
        loadBytes[1] = 0xCF;
        loadBytes[2] = 0xFC;
        loadBytes[3] = 0x1D;

        byte[] temp = MakeBytes(nVer);

        loadBytes[4] = temp[3];
        loadBytes[5] = temp[2];
        loadBytes[6] = temp[1];
        loadBytes[7] = temp[0];

        temp = MakeBytes(nNumbytes);

        loadBytes[8] = temp[3];
        loadBytes[9] = temp[2];
        loadBytes[10] = temp[1];
        loadBytes[11] = temp[0];

        for (int i = 0; i < nNumbytes; i++)
        {
          loadBytes[i + 12] = fileBytes[i];
        }
        wCRC = crc16_ccitt(fileBytes);
        loadBytes[nNumbytes + 12] = (byte)((wCRC >> 8) & 0xFF);
        loadBytes[nNumbytes + 13] = (byte)(wCRC & 0xFF);


        nOffset = nFileOffset;

        if (nTotal == 0)
        {
          nNumbytes += 14;
        }
        else
        {
          if ((nTotal + nOffset) > (loadBytes.Length))
          {
            nNumbytes = loadBytes.Length - nOffset;
          }
          else
          {
            nNumbytes = nTotal;
          }
        }

        while (nNumbytes > 0)
        {
          if (nNumbytes > nPktSize)
          {
            nNumToSend = nPktSize;
          }
          else
          {
            nNumToSend = nNumbytes;
          }

          byte[] ayCmd = new byte[nNumToSend];
          Array.Copy(loadBytes, nOffset, ayCmd, 0, nNumToSend);

          //do
          //{
          MRAM_Write(yCode_Data, yChip, (UInt32)nLoadAddress, ayCmd);

          /*
          bWaitForAck = true;
          for (int i = 0; i < 20; i++)
          {
            System.Threading.Thread.Sleep(100);
            if (MainForm._mainform.AckReceived())
            {
              bWaitForAck = false;
              break;
            }
          }
        } while (bWaitForAck == true);
        */
          nLoadAddress += nNumToSend;
          nNumbytes -= nNumToSend;
          nOffset += nNumToSend;
          System.Threading.Thread.Sleep(50);
        }
      }
      catch
      {
        MainForm._mainform.PrintError("File IO Error \n");
      }
    }

    public int GetFileSize(string sFilename)
    {
      try
      {
        byte[] fileBytes = File.ReadAllBytes(sFilename);
        return (fileBytes.Length);
      }
      catch
      {
        MainForm._mainform.PrintError("File IO Error \n");
        return (0);
      }
    }

    public void LoadFSW(string sFilename, int nVer, byte yCode_Data, byte yChip, UInt32 uiAddr)
    {
      try
      {
        byte[] fileBytes = File.ReadAllBytes(sFilename);
        int nNumbytes = fileBytes.Length;
        int nNumToSend;
        int nOffset;
        int nLoadAddress = (int)uiAddr;
        byte[] loadBytes = new byte[nNumbytes + 14];
        UInt16 wCRC;

        loadBytes[0] = 0x1A;
        loadBytes[1] = 0xCF;
        loadBytes[2] = 0xFC;
        loadBytes[3] = 0x1D;

        byte[] temp = MakeBytes(nVer);

        loadBytes[4] = temp[3];
        loadBytes[5] = temp[2];
        loadBytes[6] = temp[1];
        loadBytes[7] = temp[0];

        temp = MakeBytes(nNumbytes);

        loadBytes[8] = temp[3];
        loadBytes[9] = temp[2];
        loadBytes[10] = temp[1];
        loadBytes[11] = temp[0];

        for (int i = 0; i < nNumbytes; i++)
        {
          loadBytes[i+12] = fileBytes[i];
        }
        wCRC = crc16_ccitt(fileBytes);
        loadBytes[nNumbytes + 12] = (byte)((wCRC >> 8) & 0xFF);
        loadBytes[nNumbytes + 13] = (byte)(wCRC & 0xFF);

        nNumbytes += 14;
        nOffset = 0;
        while (nNumbytes > 0)
        {
          if (nNumbytes > 220)
          {
            nNumToSend = 220;
          }
          else
          {
            nNumToSend = nNumbytes;
          }

          byte[] ayCmd = new byte[nNumToSend];
          Array.Copy(loadBytes, nOffset, ayCmd, 0, nNumToSend);
          MRAM_Write(yCode_Data, yChip, (UInt32)nLoadAddress, ayCmd);
          
          nLoadAddress += nNumToSend;
          nNumbytes -= nNumToSend;
          nOffset += nNumToSend;
          System.Threading.Thread.Sleep(50);
        }
      }
      catch
      {
        MainForm._mainform.PrintError("File IO Error \n");
      }
    }

    public void VerifyFSW(byte yCode_Data, byte yChip, UInt32 ui32Addr)
    {
      byte[] ayCmd = new byte[8 + 5];

      AddSyncCode(ref ayCmd);

      ayCmd[3] = FSW_VERIFY_CODE;
      ayCmd[4] = 0x00;
      ayCmd[5] = 0x05;

      ayCmd[6] = yCode_Data;
      ayCmd[7] = yChip;

      byte[] ayAddrBytes = MakeBytes((int)ui32Addr);
      ayCmd[8] = ayAddrBytes[2];
      ayCmd[9] = ayAddrBytes[1];
      ayCmd[10] = ayAddrBytes[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void CopyFSW(byte yDestCode_Data, byte yDestChip, UInt32 ui32DestAddr,
                        byte ySrcCode_Data, byte ySrcChip, UInt32 ui32SrcAddr)
    {
      byte[] ayCmd = new byte[8 + 10];

      AddSyncCode(ref ayCmd);

      ayCmd[3] = FSW_COPY_CODE;
      ayCmd[4] = 0x00;
      ayCmd[5] = 0x0A;

      ayCmd[6] = yDestCode_Data;
      ayCmd[7] = yDestChip;

      byte[] ayAddrBytes = MakeBytes((int)ui32DestAddr);
      ayCmd[8] = ayAddrBytes[2];
      ayCmd[9] = ayAddrBytes[1];
      ayCmd[10] = ayAddrBytes[0];

      ayCmd[11] = ySrcCode_Data;
      ayCmd[12] = ySrcChip;

      ayAddrBytes = MakeBytes((int)ui32SrcAddr);
      ayCmd[13] = ayAddrBytes[2];
      ayCmd[14] = ayAddrBytes[1];
      ayCmd[15] = ayAddrBytes[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void SetFSWBootPartition(byte yPart)
    {
      byte[] ayCmd = new byte[9];

      AddSyncCode(ref ayCmd);

      ayCmd[3] = FSW_SET_BOOT_PART;
      ayCmd[4] = 0x00;
      ayCmd[5] = 0x01;

      ayCmd[6] = yPart;

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
    }

    public void BootFSW(byte yCode_Data, byte yChip, UInt32 ui32Addr)
    {
      byte[] ayCmd = new byte[8 + 5];

      AddSyncCode(ref ayCmd);

      ayCmd[3] = FSW_BOOT;
      ayCmd[4] = 0x00;
      ayCmd[5] = 0x05;

      ayCmd[6] = yCode_Data;
      ayCmd[7] = yChip;

      byte[] ayAddrBytes = MakeBytes((int)ui32Addr);
      ayCmd[8] = ayAddrBytes[2];
      ayCmd[9] = ayAddrBytes[1];
      ayCmd[10] = ayAddrBytes[0];

      AddChecksum(ref ayCmd);

      WriteOrSend(ayCmd);
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

    public int[] get_last_beacon()
    {
      return (MainForm._mainform.beaconForm.LastBeacon);
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

    public ushort crc16_ccitt(byte[] buf)
    {
      int j = 0;
      int sz = buf.Length;
      ushort crc = 0;
      while (--sz >= 0)
      {
        int i;
        crc ^= (ushort)((ushort)buf[j++] << 8);
        for (i = 0; i < 8; i++)
        {
          if ((crc & 0x8000) != 0)
          {
            crc = (ushort)((crc << 1) ^ 0x1021);
          }
          else
          {
            crc <<= 1;
          }
        }
      }
      return crc;
    }

    public void SendString(String sSerialCmd)
    {
      comCmds.SendString(sSerialCmd);
    }

    public void SetASCIIRxMode(bool bASCII)
    {
      MainForm._mainform.bSerialDataisASCII = bASCII;
    }
  }
}
