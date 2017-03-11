using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Collections;

namespace SPRL.Test
{
  public class COMCmds
  {
    private static COMCmds _comcmds = null;
    private static object _instanceLock = new Object();
    private LogFile logFile;
    private SerialPort serialPort;
    private Queue syncedQueue = Queue.Synchronized(new Queue());
    public static int _numBytes = 50000;

    public static COMCmds Instance
    {
      get
      {
        lock (_instanceLock)
        {
          if (_comcmds == null)
          {
            _comcmds = new COMCmds();
          }
          return _comcmds;
        }
      }
    }

    public COMCmds()
    {
      logFile = LogFile.Instance;
    }

    //Init String:  COM1:2000000,n,8,1
    public void Init(string sInitString)
    {
      string [] sParmArray;
      string sCOMPort;
      int nBaudRate;
      Parity parity;
      int nDataBits;
      StopBits stopBits;

      sInitString = sInitString.ToLower();

      sParmArray = sInitString.Split(':', ',');

      if (sParmArray.Length != 5)
      {
        MainForm._mainform.PrintError("Error:  Invalid COM Port Init String.\n:");
        return;
      }
      else
      {
        sCOMPort = sParmArray[0];
        nBaudRate = Convert.ToInt32(sParmArray[1]);
        nDataBits = Convert.ToInt32(sParmArray[3]);

        // Figure out the parity
        if (sParmArray[2] == "o")
        {
          parity = Parity.Odd;
        }
        else
        {
          if (sParmArray[2] == "e")
          {
            parity = Parity.Even;
          }
          else
          {
            if (sParmArray[2] == "n")
            {
              parity = Parity.None;
            }
            else
            {
              MainForm._mainform.PrintError("Error:  Bad parity specification.\n");
              return;
            }
          }
        }

        if (sParmArray[4] == "1")
        {
          stopBits = StopBits.One;
        }
        else
        {
          if (sParmArray[4] == "2")
          {
            stopBits = StopBits.Two;
          }
          else
          {
            MainForm._mainform.PrintError("Error:  Bad Stop Bit specification.\n");
            return;
          }
        }

        if ((serialPort!=null) && (serialPort.IsOpen))
        {
          serialPort.Close();
        }


        serialPort = new SerialPort(sCOMPort, nBaudRate, parity, nDataBits, stopBits);
        serialPort.ReadBufferSize = 1000000;
        //serialPort.ReceivedBytesThreshold = 10;
        serialPort.Open();

        if (serialPort.IsOpen)
        {
          serialPort.DtrEnable = true;
          serialPort.RtsEnable = true;
          serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
          serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
        }
        else
        {
          MainForm._mainform.PrintError("Error:  Failed to open serial port.\n");
        }
      }
    }

    void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      //throw new NotImplementedException();
    }

    void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      byte[] baBytes;      
      int nNumbytes = serialPort.BytesToRead;

      if (nNumbytes > 0)
      {
        baBytes = new byte[nNumbytes];
        serialPort.Read(baBytes, 0, nNumbytes);
        syncedQueue.Enqueue(baBytes);
      }
    }

    public bool bSerialDataAvailable()
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

    public byte[] GetSerialData()
    {
      return ((byte[])syncedQueue.Dequeue());
    }

    public void Send(params byte[] bytes)
    {
      if (serialPort.IsOpen)
      {
        serialPort.Write(bytes, 0, bytes.Length);
      }
    }

    public void TestCont()
    {
      byte[] bytes = new byte[100000];
      bytes[0] = 0xF0;
      for(int i=1; i<(100000-1); i++){
        bytes[i] = (byte)(i % 255);
      }
      bytes[99999] = 0x0F;
      if (serialPort.IsOpen)
      {
        serialPort.Write(bytes, 0, bytes.Length);
      }
      MainForm._mainform.PrintError("Sent Continuous Test\n");
    }
    //Init Command: COM1:2000000,o,8,1
    public void TestTXRX(int nSize)
    {
      _numBytes=nSize;
      byte[] bytes = new byte[_numBytes];
      bytes[0] = 0xAA;

      for (int i = 1; i < (_numBytes-1); i++)
      {
        bytes[i] = (byte)(i % 255);
      }
      bytes[_numBytes-1] = 0xAA;
      if (serialPort.IsOpen)
      {
        serialPort.Write(bytes, 0, bytes.Length);
      }
      MainForm._mainform.PrintError("Sent TX/RX Test\n");
    }

    internal void SendString(string sSerialCmd)
    {
      serialPort.Write(sSerialCmd);
    }

  }
}
