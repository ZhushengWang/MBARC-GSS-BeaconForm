using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.DAQmx;



namespace SPRL.TestPXI
{
	class ExtDigIO : IDisposable
	{
		private bool bDisposed = false;

		private static ExtDigIO _extDigIO = null;
		private static object _instanceLock = new Object();

		private static object _CurTimeLock = new Object();

		private Task myExtDITask;
		private DigitalSingleChannelReader myExtDigitalReader;

		private Task myTask;
		private DigitalSingleChannelReader myDigitalReader;

		private Task myDITask;
		private DigitalSingleChannelReader mySubsecsReader;

		private bool[] bDILastState = { false, false, false };
		private bool _bExternalControl = false;
		private double dTime;  //Time to send out when simulating spacecraft
		private double dTimeRead;
		private double dCurrentTime;  //Current Time, updated on sync signal

		private DigOut _digOut = null;
		private SerialIO _serialIO = null;

		private bool bTimeUpdated = false;

		// This is a singleton class
		// Return a reference to the only instance of ExtDigIO
		// Create the instance if necessary
		public static ExtDigIO Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_extDigIO == null)
					{
						_extDigIO = new ExtDigIO();
					}
					return _extDigIO;
				}
			}
		}
		
		// Constructor
		public ExtDigIO()
		{
			try
			{
				_digOut = DigOut.Instance;  // Get instance of singleton
				_serialIO = SerialIO.Instance;

				// Set up external Digital input
				myExtDITask = new Task();// NI Task for reading NS_ON_EXT,SEL_LOAD_EXT,TS_VALID_ECHO

				myExtDITask.DIChannels.CreateChannel("Dev1/port0/line7,Dev1/port0/line11,Dev1/port0/line12",
					"Ext In", ChannelLineGrouping.OneChannelForAllLines);
				myExtDigitalReader = new DigitalSingleChannelReader(myExtDITask.Stream);
				myExtDITask.Start();

				//Get state of external signals
				bDILastState = myExtDigitalReader.ReadSingleSampleMultiLine(); // Get initial port state

				// Digital input for subseconds
				myDITask = new Task();   // NI Task for digital input of 12 Time Stamp sub second bits
				
				// Set up Digital Input
				myDITask.DIChannels.CreateChannel("Dev1/port0/line16:28",
					"Subsecs In", ChannelLineGrouping.OneChannelForAllLines);
				mySubsecsReader = new DigitalSingleChannelReader(myDITask.Stream);
				myDITask.Start();

				//Set up change detect signals
				myTask = new Task();     // NI Task for TS_VALID_ECHO,NS_ON_EXT,SEL_LOAD_EXT edge detection

				// Setup Digital input change channel
				myTask.DIChannels.CreateChannel("Dev1/port0/line0,Dev1/port0/line9:10", "",
						ChannelLineGrouping.OneChannelForAllLines);

				// Configure digital change detection timing
				myTask.Timing.ConfigureChangeDetection(
						"Dev1/port0/line0,Dev1/port0/line9:10",
						"Dev1/port0/line0,Dev1/port0/line9:10",
						SampleQuantityMode.ContinuousSamples, 1000);

				// Add the digital change detection event handler
#if NETFX2_0
				// For .NET Framework 2.0, use SynchronizeCallbacks to specify that the object 
				// marshals callbacks across threads appropriately.
				myTask.SynchronizeCallbacks = true;
#else
        // For .NET Framework 1.1, set SynchronizingObject to the Windows Form to specify 
        // that the object marshals callbacks across threads appropriately.
        myTask.SynchronizingObject = this;
#endif

				myTask.DigitalChangeDetection += new DigitalChangeDetectionEventHandler(myTask_DigitalChangeDetection);

				// Create the reader
				myDigitalReader = new DigitalSingleChannelReader(myTask.Stream);

				myTask.Start();
			}
			catch (DaqException exception)
			{
				MessageBox.Show(exception.Message);
			}
		}

		// Implement IDisposable
		public void Dispose()
		{
			// Check to see if Dispose has already been called.
			if (!bDisposed)
			{
				myDITask.Dispose();  // Stop the NI Tasks
				myExtDITask.Dispose();
				myTask.Dispose();

				bDisposed = true;
			}
		}

		void myTask_DigitalChangeDetection(object sender, DigitalChangeDetectionEventArgs e)
		{
			bool[] bNewData = new bool[3];
			double dTmpTime = 0.0;

			try
			{
				bNewData = myExtDigitalReader.ReadSingleSampleMultiLine();

#region DebugCode
				//if (bNewData[0])
				//{
				//  myUtil.PrintMsg("1");
				//}
				//else
				//{
				//  myUtil.PrintMsg("0");
				//}
				//if (bNewData[1])
				//{
				//  myUtil.PrintMsg("1");
				//}
				//else
				//{
				//  myUtil.PrintMsg("0");
				//}
				//if (bNewData[2])
				//{
				//  myUtil.PrintMsg("1\n");
				//}
				//else
				//{
				//  myUtil.PrintMsg("0\n");
				//}
#endregion

				// Bit definitions
			  //bData[0] = TS_VALID_ECHO
			  //bData[1] = NS_ON_EXT
			  //bData[2] = SEL_LOAD_EXT  rising edge=amb, falling edge=cold

				if ( (bNewData[1] != bDILastState[1]) && (_bExternalControl == true) )
				{
					MainForm._mainform.setNDLEDs(bNewData[1]);
				}

				if ( (bNewData[2] != bDILastState[2]) && (_bExternalControl == true) )
				{
				  MainForm._mainform.setLoadLEDs(!bNewData[2]);
				}

				if (bNewData[0] != bDILastState[0])
				{
					if (!bNewData[0])
					{
						// Falling edge
						// Write out new time code packet

						lock (_CurTimeLock)
						{
							dTmpTime = dTime;
							dTime += 2.0;
						}
						_serialIO.SendTimePacket(dTmpTime);
					}
					else
					{
						//Rising edge - update time code and acknowledge
						lock (_CurTimeLock)
						{
						  _digOut.setTimeAck(true);
					  	_digOut.setTimeAck(false);

							dCurrentTime = dTimeRead;
							bTimeUpdated = true;
						}	
					
						MainForm._mainform.SetTimeString(dTimeRead);
					}
				}
				bDILastState = bNewData;

      }
      catch (DaqException ex)
      {
        MessageBox.Show(ex.Message);
      }
		}

		public int GetSubSeconds()
		{
			UInt32 nPortVal1=0;
			UInt32 nPortVal2=0;

			int nSubSecs = 0;
			bool bKeepReading = true;

			while (bKeepReading)
			{
				nPortVal1 = mySubsecsReader.ReadSingleSamplePortUInt32();
				nPortVal2 = mySubsecsReader.ReadSingleSamplePortUInt32();

				//Read until we get 2 values the same
				//to make sure we're not reading during transition
				if (nPortVal1 == nPortVal2)
				{
					bKeepReading = false;
				}
			}

			nSubSecs = (int)((nPortVal1 >> 16) & 0x1FFF);

			//There is a small amount of time after the seconds part of the time is
			//updated when the subsecs have not reset.
			if (bTimeUpdated)
			{
				if (nSubSecs >= 2000)
				{
					nSubSecs -= 2000;
				}
				else
				{
					bTimeUpdated = false;
				}
			}

			return (nSubSecs);
		}

		//Public Methods

		public double GetTime()
		{
			double dCurTime;

			lock (_CurTimeLock)
			{
				dCurTime = dCurrentTime + (double)GetSubSeconds() / 1000.0;
				return (dCurTime);
			}
		}

		public int GetTimeAsMillisecs()
		{
			int nTimeMs;

			lock (_CurTimeLock)
			{
				nTimeMs = (int)(dCurrentTime * 1000.0) + GetSubSeconds();
				return (nTimeMs);
			}
		}

		public void SetExternalControl(bool bValue)
		{
			_bExternalControl = bValue;

			//Read external control lines and set state accordingly.
			if (bValue == true)
			{
				bool[] bNewData = new bool[3];

				bNewData = myExtDigitalReader.ReadSingleSampleMultiLine();

				MainForm._mainform.setNDLEDs(bNewData[1]);

				MainForm._mainform.setLoadLEDs(!bNewData[2]);

				bDILastState = bNewData;
			}

		}

		public void SetTimeRead(double dSetTime)
		{
			dTimeRead = dSetTime;
		}

		public void SetTime(double dSetTime)
		{
			lock (_CurTimeLock)
			{
				dTime = dSetTime;
			}
		}
	}
}
