using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.DAQmx;

namespace SPRL.TestPXI
{
	class DigOut : IDisposable
	{
		private bool bDisposed = false;

		private static DigOut _digOut = null;
		private static object _instanceLock = new Object();

		private Task myDOTask;
		private DigitalMultiChannelWriter myDigitalWriter;
		private bool[] bDOState = { false, false, false, false, 
																false, false, false};
		private enum bDONames { TS_HS, NSENB, LOENB, POS1, POS2, ENB_EXT_CTRL, DAT0 };

		private Task myTimerTask;  // Task to set up 0.5Hz timer output

		// This is a singleton class
		// Return a reference to the only instance of DigOut
		// Create the instance if necessary
		public static DigOut Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_digOut == null)
					{
						_digOut = new DigOut();
					}
					return _digOut;
				}
			}
		}

		//Constructor
		public DigOut()
		{
			myDOTask = new Task();   // NI Task for Digital output controls (NSENB,LOENB,POS1,POS2,ENB_EXT_CTRL,DAT0)
			
			// Setup digital output channels
			myDOTask.DOChannels.CreateChannel("Dev1/port0/line1:6,Dev1/port0/line8", "Digout",
					ChannelLineGrouping.OneChannelForEachLine);

			myDigitalWriter = new DigitalMultiChannelWriter(myDOTask.Stream);
			myDOTask.Start();

			myTimerTask = new Task();// NI Task for 0.5 Hz timer output for Time Stamp
			
			// Set up timer output
			myTimerTask.COChannels.CreatePulseChannelFrequency("Dev1/ctr0",
				"ContinuousPulseTrain", COPulseFrequencyUnits.Hertz, COPulseIdleState.Low, 0.0,
			0.5,
			0.5);

			myTimerTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000);
			myTimerTask.Start();
		}

		// Implement IDisposable
		public void Dispose()
		{
			// Check to see if Dispose has already been called.
			if (!bDisposed)
			{
				myTimerTask.Dispose(); // Stop the NI Tasks
				myDOTask.Dispose();

				bDisposed = true;
			}
		}

		public void setTimeAck(bool bValue)
		{
			bDOState[(int)bDONames.TS_HS] = bValue;
			myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
		}

		public void setColdFET(bool bValue)
		{
			//bValue = true means ColdFET on
			//Pulse POS1 for Ambient Load
			//Pulse POS2 for ColdFET
			//setAddress(nNum);

			if (bValue == true)
			{
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				bDOState[(int)bDONames.POS2] = true;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				System.Threading.Thread.Sleep(10);
				bDOState[(int)bDONames.POS2] = false;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
			}
			else
			{
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				bDOState[(int)bDONames.POS1] = true;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				System.Threading.Thread.Sleep(10);
				bDOState[(int)bDONames.POS1] = false;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
			}

			MainForm._mainform.setLoadLEDs(bValue);
		}

		public void setNoiseDiode(bool bValue)
		{
			bDOState[(int)bDONames.DAT0] = bValue;

			if (bValue == true)
			{
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				bDOState[(int)bDONames.NSENB] = true;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				bDOState[(int)bDONames.NSENB] = false;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
			}
			else
			{
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				bDOState[(int)bDONames.NSENB] = true;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
				bDOState[(int)bDONames.NSENB] = false;
				myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
			}
			MainForm._mainform.setNDLEDs(bDOState[(int)bDONames.DAT0]);
		}

		public void setLO(bool bValue)
		{
			bDOState[(int)bDONames.DAT0] = bValue;
			myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);

			bDOState[(int)bDONames.LOENB] = true;
			myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
			bDOState[(int)bDONames.LOENB] = false;
			myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);

			MainForm._mainform.setLOLed(bValue);
		}

		public void setEnaExt(bool bValue)
		{
			bDOState[(int)bDONames.ENB_EXT_CTRL] = bValue;
			myDigitalWriter.WriteSingleSampleSingleLine(false, bDOState);
		}

	}
}
