using System;
using System.Collections.Generic;
using System.Text;

namespace SPRL.TestPXI
{
	class AWGControl
	{
		private static AWGControl _awgControl = null;
		private static object _instanceLock = new Object();

		private AGN6030A awgN6030A = null;
		private int[] nAWGHandles;

		private double dLastGain = 0.17;

		// This is a singleton class
		// Return a reference to the only instance of AWGControl
		// Create the instance if necessary
		public static AWGControl Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_awgControl == null)
					{
						_awgControl = new AWGControl();
					}
					return _awgControl;
				}
			}
		}

		//Constructor
		public AWGControl()
		{
			nAWGHandles = new int[1000];

  		try
			{
				awgN6030A = new AGN6030A("PXI16::10::0::INSTR", true, true);

				InitAWG(false);
			}
			catch (Exception)
			{
				awgN6030A = null;
				MainForm._mainform.PrintError("AWG Initialization Failed!\n");
			}
		}

		public void InitAWG(bool bReset)
		{
			try
			{
				if (awgN6030A != null)
				{
					if (bReset == true)
					{
						awgN6030A.reset();
					}
					awgN6030A.ConfigureOperationMode("1", AGN6030AConstants.OperateContinuous);
					awgN6030A.ConfigureOperationMode("2", AGN6030AConstants.OperateContinuous);
					awgN6030A.ConfigureOutputImpedance("1", 50.0);
					awgN6030A.ConfigureOutputImpedance("2", 50.0);
					awgN6030A.ConfigureOutputMode(AGN6030AConstants.OutputArb);
					awgN6030A.ConfigureRefClockSource(AGN6030AConstants.RefClockInternal);
					awgN6030A.ConfigureSampleClock(AGN6030AConstants.ClockInternal, 1250e6);
					awgN6030A.ConfigureOutputConfiguration("1", AGN6030AConstants.ConfigurationSingleEnded, false, 250e6);
					awgN6030A.ConfigureOutputConfiguration("2", AGN6030AConstants.ConfigurationSingleEnded, false, 250e6);

					//awgN6030A.SetString(AGN6030AProperties.ActiveMarker, "1", "1");
					//awgN6030A.SetInt32(AGN6030AProperties.MarkerSource, "1", AGN6030AConstants.MarkerWfmStart);
					//awgN6030A.SetDouble(AGN6030AProperties.MarkerDelay, "1", 0.0);
					//awgN6030A.SetDouble(AGN6030AProperties.MarkerPulseWidth, "1", 125e-9);
					//awgN6030A.SetBoolean(AGN6030AProperties.MarkerPolarity, "1", true);
					//awgN6030A.SetString(AGN6030AProperties.ActiveMarker, "2", "1");
					//awgN6030A.SetInt32(AGN6030AProperties.MarkerSource, "2", AGN6030AConstants.MarkerWfmStart);

					awgN6030A.ConfigureOutputEnabled("1", false);  // Both channels initially off
					awgN6030A.ConfigureOutputEnabled("2", false);  // Don't use channel 2

					SetGain(0.17, "1");
					SetOffset(0.0, "1");
					SetGain(0.17, "2");
					SetOffset(0.0, "2");
				}
			}
			catch (Exception)
			{
				MainForm._mainform.PrintError("AWG Reset Failed!\n");
			}
		}

		public void LoadArray(double[] daValues,int nCount, int nTable)
		{
			int nCh2Handle = 0;

			// Handles must be created in pairs.  Odd handles (1,3,5) are for channel 1
			// while even handles (2,4,6) are for channel 2.  This routine throws out the
			// channel 2 handles.
			if (awgN6030A != null)
			{
				awgN6030A.CreateArbWaveform(nCount, daValues, out nAWGHandles[nTable]);
				awgN6030A.CreateArbWaveform(nCount, daValues, out nCh2Handle);  //Discard Ch 2
			}
		}

		public void RunTable(int nTable)
		{
			if (awgN6030A != null)
			{
				//awgN6030A.SetInt32(AGN6030AProperties.ArbWaveformHandle, "1", nAWGHandles[nTable]);
				awgN6030A.ConfigureArbWaveform("1", nAWGHandles[nTable], dLastGain, 0.0);
			}
		}

		public void SetGain(double dGain, string sChan)
		{
			dLastGain = dGain;
			if (awgN6030A != null)
			{
				awgN6030A.SetDouble(AGN6030AProperties.ArbGain, sChan, dGain);
			}
		}

		public void SetOffset(double dOffset, string sChan)
		{
			if (awgN6030A != null)
			{
				awgN6030A.SetDouble(AGN6030AProperties.ArbOffset, sChan, dOffset);
			}
		}

		public void SetAWGOutput(bool bOn)
		{
			if (awgN6030A != null)
			{
				awgN6030A.ConfigureOutputEnabled("1", bOn);
			}
		}
	}
}
