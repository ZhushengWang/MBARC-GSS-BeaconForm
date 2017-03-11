using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SPRL.Test
{
	class LogFile : IDisposable
	{
		private bool bDisposed = false;

		private static LogFile _logFile = null;
		private static object _instanceLock = new Object();

		private static object _FileLock = new Object();

		private System.Timers.Timer LogTimer = null;

		private string sFileName = "";
		private string sDirectory = "";

		private System.IO.TextWriter textLogFile = null;

		// This is a singleton class
		// Return a reference to the only instance of LogFile
		// Create the instance if necessary
		public static LogFile Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_logFile == null)
					{
						_logFile = new LogFile();
					}
					return _logFile;
				}
			}
		}
		
		// Constructor
		public LogFile()
		{
			LogTimer = new System.Timers.Timer();
			LogTimer.Elapsed += new System.Timers.ElapsedEventHandler(LogTimer_Elapsed);
			//LogTimer.Interval = 10000; 
			LogTimer.Interval = 3600000; // Checkpoint interval is 1 hour
			LogTimer.Enabled = false;
		}

		//Implement IDisposable
		public void Dispose()
		{
			if (!bDisposed)
			{
				bDisposed = true;
				LogTimer.Stop();
				if (textLogFile != null)
				{
					textLogFile.Close();
					textLogFile = null;
				}
			}
		}

		void LogTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			OpenNewFile();
		}

		public void SetDirectory(string sDir)
		{
			sDirectory = sDir;

			OpenNewFile();

			LogTimer.Start();  // Start the checkpoint timer when a new directory is specified
		}

		public void OpenNewFile()
		{
			string sFullPath;

			//Filename = YYYY-MM-DD-HH-MM-SS.log

			if (sDirectory != "")
			{
				System.DateTime date = System.DateTime.Now;

				string sFname = date.ToString("yyyy-MM-dd-HH-mm-ss");

				sFname += ".log";
				sFileName = sFname;

				lock (_FileLock)
				{
					if (textLogFile != null)
					{
						textLogFile.Close();
						textLogFile = null;
					}
				}

				sFullPath = System.IO.Path.Combine(sDirectory,sFileName);
				lock (_FileLock)
				{
					textLogFile = new System.IO.StreamWriter(sFullPath);
				}

				MainForm._mainform.SetLogFileName(sFileName);

				if (sDirectory != "")
				{
					MainForm._mainform.PrintMsg("New Log File:" + sFileName + "\n");
				}
			}
		}

		public void WriteLog(string sEntry)
		{
			lock (_FileLock)
			{
				if (textLogFile != null)
				{
          //If the string contains a non-ending newline,
          if (sEntry.Contains("\n") && (sEntry.IndexOf("\n") < (sEntry.Length - 3)))
          {
            //Replace it with a tab
            sEntry = sEntry.Replace("\n", "\t");
          }
          sEntry = sEntry.TrimEnd(new char[4] { '\r', '\n', ' ', '\t' });
          System.DateTime date = System.DateTime.Now;
          string sDate = date.ToString("yyyy/MM/dd HH:mm:ss");
          textLogFile.WriteLine(sDate + " > " + sEntry);
				}
			}
		}
    //Never actually used:
    public void WriteVoltsToLog(List<string[]> lValues)
    {
      for (int i = 0; i < lValues.Count; i++)
      {
        string sEntry;
        sEntry = lValues[i][0].ToString() + ", " + lValues[i][1].ToString() + ", " + lValues[i][2].ToString();
        WriteLog(sEntry);
      }
    }
	}
}
