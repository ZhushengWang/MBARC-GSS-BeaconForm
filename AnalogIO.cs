using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.DAQmx;

namespace SPRL.TestPXI
{
	class AnalogIO : IDisposable
	{
		private bool bDisposed = false;

		private static AnalogIO _analogIO = null;
		private static object _instanceLock = new Object();

		private Task myAITask;
		private Task myAOTask;
		private AnalogMultiChannelReader myAnalogReader;
		private AnalogMultiChannelWriter myAnalogWriter;

		private System.Timers.Timer AITimer;

		private ExtDigIO _extDigIO = null;
		private LogFile  _logFile  = null;

		// This is a singleton class
		// Return a reference to the only instance of AnalogIO
		// Create the instance if necessary
		public static AnalogIO Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_analogIO == null)
					{
						_analogIO = new AnalogIO();
					}
					return _analogIO;
				}
			}
		}

		//Constructor
		public AnalogIO()
		{
			_extDigIO = ExtDigIO.Instance;
			_logFile  = LogFile.Instance;

			// Setup Analog output
			myAOTask = new Task();   // NI Task for DAC +10V outputs for Thermistor Drive
			
			myAOTask.AOChannels.CreateVoltageChannel("Dev1/ao0:1", "VRef", 0.0, 10.0, AOVoltageUnits.Volts);
			myAnalogWriter = new AnalogMultiChannelWriter(myAOTask.Stream);
			double[] dVref = { 10.0, 10.0 };
			myAnalogWriter.WriteSingleSample(true, dVref);  //Write out fixed 10V Reference for thermistors

			// Setup Analog Input
			myAITask = new Task();   // NI Task for 12 Thermistor voltage readings
			myAITask.AIChannels.CreateVoltageChannel("Dev1/ai0:7,Dev1/ai16:19", "AnalogIn",
																							 AITerminalConfiguration.Rse, 0.0, 10.0,
																							 AIVoltageUnits.Volts);

			myAITask.Timing.SampleClockSource = "Internal";
			myAITask.Timing.ConfigureSampleClock("", 1000, SampleClockActiveEdge.Rising,
																					 SampleQuantityMode.FiniteSamples, 100);
			myAITask.Timing.AIConvertRate = 100000.0;

			//myAITask.SampleComplete += new SampleCompleteEventHandler(myAITask_SampleComplete);

			myAITask.Control(TaskAction.Verify);

			myAnalogReader = new AnalogMultiChannelReader(myAITask.Stream);
			//myAnalogReader.BeginReadMultiSample(100, new AsyncCallback(AnalogInCallback), myAITask);

			AITimer = new System.Timers.Timer();
			AITimer.Interval = 10000;
			AITimer.Elapsed += new System.Timers.ElapsedEventHandler(AITimer_Elapsed);
			AITimer.Start();
		}

		void AITimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			RestartAIRead();
		}

		// Implement IDisposable
		public void Dispose()
		{
			// Check to see if Dispose has already been called.
			if (!bDisposed)
			{
				AITimer.Stop();
				myAOTask.Dispose();  // Stop the NI Tasks
				myAITask.Dispose();

				bDisposed = true;
			}
		}

		private void AnalogInCallback(IAsyncResult ar)
		{
			double[,] dData;
			double[] dAvgData;
			string[] sTemps;

			dData = new double[12, 100];
			dAvgData = new double[12];
			sTemps = new string[12];

			if (myAITask == ar.AsyncState)
			{
				//Read the available data from the channels
				dData = myAnalogReader.EndReadMultiSample(ar);

				//Average and display

				for (int i = 0; i < 12; i++)
				{
					for (int j = 0; j < 100; j++)
					{
						dAvgData[i] += dData[i, j];
					}
					dAvgData[i] /= 100.0;
					sTemps[i] = VoltsToC(dAvgData[i]).ToString("N3") + " C";
				}

			  MainForm._mainform.DisplayAIData(sTemps);
				_logFile.WriteTempsToLog(sTemps, _extDigIO.GetTime());
			}
		}

		public void RestartAIRead()
		{
			myAnalogReader.BeginReadMultiSample(100,
								AnalogInCallback, myAITask);

		}

		private double VoltsToC(double dVolts)
		{
			double R;
			double LogR;
			const double a = 9.354e-4;
			const double b = 2.211e-4;
			const double d = 1.275e-7;

			R = dVolts * 18700 / (10.0 - dVolts);

			if (R < 100)
			{
				R = 100;  // Default low value
			}

			LogR = Math.Log(R);

			return ((1 / (a + b * LogR + d * Math.Pow(LogR, 3.0))) - 273.15);
		}

	}
}
