using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.DAQmx;
using NationalInstruments;

namespace SPRL.Test
{
  public class NI9264
  {
    private Task aoTask;
    private AnalogMultiChannelWriter aoWriter;
    public double[] daVolts;
    private String sModDev;

    public NI9264(string sDev)
    {
      daVolts = new double[16];
      
      for (int nI = 0; nI < 15; nI++)
      {
        daVolts[nI] = 0.0;
      }

      aoTask = new Task();

      aoTask.AOChannels.CreateVoltageChannel(sDev + "/ao0:"+sDev + "/ao15", "", -10.0, 10.0, AOVoltageUnits.Volts);
      aoWriter = new AnalogMultiChannelWriter(aoTask.Stream);

      aoWriter.WriteSingleSample(true, daVolts);  //Init to all 0V

      sModDev = sDev;
    }

    public void dispose()
    {
      aoTask.Stop();
      aoTask.Dispose();
      aoTask = null;
    }

    public void SetV(int nChan, double dVolts)
    {
      if (aoTask != null)
      {
        daVolts[nChan] = dVolts;

        aoWriter.WriteSingleSample(true, daVolts);
      }
    }
  }
}
