using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPRL.Test
{
  class MOMARSimPkt
  {
    public const int MOMA_RSIM_PKT_SIZE = 2151;
    public bool bChecksumOK;
    public UInt32 ui32CalculatedChecksum;

    public UInt16 ui16PktID;
    public UInt32 ui32TimeStamp;
    public UInt16 ui16SubSecs;
    public UInt32 ui32PktSize;
    public byte yTMPktID;
    public byte ySeqCount;
    public UInt16 ui16PktLen;
    public UInt32 ui32TS;
    public byte yFPGARev;
    public byte yRabbitRev;
    public UInt16 ui16RSimCmdCount;
    public UInt16 ui16FPGACmdCount;
    public float fPSVoltSet;
    public float fPSCurSet;
    public byte yPSOutState;
    public float fPSVoltReading;
    public float fPSCurReading;
    public float fBus1CurSet;
    public float fBus2CurSet;
    public float fBus3CurSet;
    public byte yBus1OutState;
    public byte yBus2OutState;
    public byte yBus3OutState;
    public float fTemp;
    public float fHum;
    public float fHours;
    public byte yLaserStat;
    public UInt16 ui16TCCount;
    public UInt16 ui16RSIMNakCount;
    public UInt16 ui16FPGANAKCount;
    public byte yOCTripMode;
    public byte yOCTripTime;

    public byte[] aySpares1 = new byte[99 - 78 + 1];

    public UInt16[] ayi16Bus1Samples = new UInt16[1000];

    public float fBus1Avg;
    public float fBus2Avg;
    public float fBus3Avg;
    public float fBus1VAvg;

    public byte[] aySpares2 = new byte[2146 - 2116 + 1];

    public UInt32 ui32Checksum;

    public MOMARSimPkt(List<byte> listPkt)
    {
      ui16PktID = makeInt(listPkt, 0);
      ui32TimeStamp = makeInt4(listPkt, 2);
      ui16SubSecs = makeInt(listPkt, 6);
      ui32PktSize = makeInt3(listPkt, 8);
      yTMPktID = listPkt[11];
      ySeqCount = listPkt[12];
      ui16PktLen = makeInt(listPkt, 13);
      ui32TS = makeInt4(listPkt, 15);
      yFPGARev = listPkt[19];
      yRabbitRev = listPkt[20];
      ui16RSimCmdCount = makeInt(listPkt, 21);
      ui16FPGACmdCount = makeInt(listPkt, 23);
      fPSVoltSet = makeFloat(listPkt, 25);
      fPSCurSet = makeFloat(listPkt, 29);
      yPSOutState = listPkt[33];
      fPSVoltReading = makeFloat(listPkt, 34);
      fPSCurReading = makeFloat(listPkt, 38);
      fBus1CurSet = makeFloat(listPkt, 42);
      fBus2CurSet = makeFloat(listPkt, 46);
      fBus3CurSet = makeFloat(listPkt, 50);
      yBus1OutState = listPkt[54];
      yBus2OutState = listPkt[55];
      yBus3OutState = listPkt[56];
      fTemp = makeFloat(listPkt, 57);
      fHum = makeFloat(listPkt, 61);
      fHours = makeFloat(listPkt, 65);
      yLaserStat = listPkt[69];
      ui16TCCount = makeInt(listPkt, 70);
      ui16RSIMNakCount = makeInt(listPkt, 72);
      ui16FPGANAKCount = makeInt(listPkt, 74);
      yOCTripMode = listPkt[76];
      yOCTripTime = listPkt[77];

      for (int i = 78; i <= 99; i++)
      {
        aySpares1[i - 78] = listPkt[i];
      }

      for (int i = 100; i <= 2099; i+=2)
      {
        UInt16 uiSample = makeInt(listPkt,i);
        ayi16Bus1Samples[(i - 100)/2] = uiSample;
      }

      fBus1Avg = makeFloat(listPkt, 2100);
      fBus2Avg = makeFloat(listPkt, 2104);
      fBus3Avg = makeFloat(listPkt, 2108);
      fBus1VAvg = makeFloat(listPkt, 2112);

      for (int i = 2116; i <= 2146; i++)
      {
        aySpares2[i - 2116] = listPkt[i];
      }

      ui32Checksum = makeInt4(listPkt, 2147);

      ui32CalculatedChecksum = CalcFletcherChecksum(0x0000FFFF, listPkt, 11, 2146);

      if (ui32CalculatedChecksum == ui32Checksum)
      {
        bChecksumOK = true;
      }
      else
      {
        bChecksumOK = false;
      }
    }

    private UInt16 makeInt(List<byte> listPkt, int nPos)
    {
      return (UInt16)((listPkt[nPos] * 256) + listPkt[nPos + 1]);
    }

    private UInt32 makeInt3(List<byte> listPkt, int nPos)
    {
      return (UInt32)((listPkt[nPos] * 65536) + listPkt[nPos + 1] * 256 + listPkt[nPos + 2]);
    }

    private UInt32 makeInt4(List<byte> listPkt, int nPos)
    {
      return (UInt32)((listPkt[nPos] * 16777216) +
                      (listPkt[nPos + 1] * 65536) +
                       listPkt[nPos + 2] * 256 +
                       listPkt[nPos + 3]);
    }

    private float makeFloat(List<byte> listPkt, int nPos)
    {
      byte[] ayBytes = new byte[4];

      ayBytes[0] = listPkt[nPos + 3];
      ayBytes[1] = listPkt[nPos + 2];
      ayBytes[2] = listPkt[nPos + 1];
      ayBytes[3] = listPkt[nPos];

      return (BitConverter.ToSingle(ayBytes, 0));
    }

    /* U16 is 16-bit unsigned int, U32 is 32-bit unsigned int.
     * Like util_fletcher_checksum, this one sums U16s, but this one
     * can count an odd number of them.  When n below is even, the
     * two versions yield the same checksum, but not when n is odd.
     */
    private UInt32 CalcFletcherChecksum(
                    UInt32 prior,      /* checksum from earlier data */
                    List<byte> listPkt,  /* where we’ll start checksumming this time */
                    int nStart,
                    int nEnd)
    {
      int nIndex;

      /* Extract the two halves of the previous checksum */
      UInt16 a = (UInt16)(0xFFFF & prior);
      UInt16 b = (UInt16)(0xFFFF & (prior >> 16));

      /* Add, ignoring any overflows */
      for (nIndex = nStart; nIndex <= nEnd; nIndex += 2)
      {
        a+=makeInt(listPkt,nIndex);
        b+=a;
      }

      /* Concatenate a,b into a U32 */
      return ((UInt32)b << 16) | (UInt32)a;
    }
  }
}
