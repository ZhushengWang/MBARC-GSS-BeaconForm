using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.Test
{
  public class SequencerData
  {
    public uint uiSync;
    public uint uiSerialNum;
    public uint uiCount1;
    public uint uiCount2;
    public uint uiRFFreq;
    public uint uiAmux1;
    public uint uiAmux2;
    public uint uiADCStatus;
    public uint uiPECount;
    public byte[] ayCTLStatus = new byte[8];
    public uint uiAmux1_Sum;
    public uint uiAmux2_Sum;
    public uint uiPESum;

    public SequencerData()
    {

    }

  }
}
