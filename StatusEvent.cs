using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SPRL.Test
{
	/// <summary>
	/// Summary description for StatusEventClass.
	/// </summary>
	[Serializable]
	public class StatusEventClass
	{
		public enum StatusType
		{
			NORMAL,
			ERROR
		}

		public DateTime			timeStamp;
		public StatusType		statusType;
		public String			status;


		public StatusEventClass()
		{
			timeStamp = new DateTime(1,1,1);
			statusType = StatusType.NORMAL;
			status = "";
		}

		public void SetTimeStampToNow()
		{
			timeStamp = DateTime.Now;
		}

		public string GetTimeStampString()
		{
			return (timeStamp.ToShortDateString()
				+ " "
				+ timeStamp.Hour.ToString("D2")
				+ ":" + timeStamp.Minute.ToString("D2")
				+ ":" + timeStamp.Second.ToString("D2")
				+ "." + timeStamp.Millisecond.ToString("D3")
				);
		}

		// Perform a clone and deep copy.
		// Return a reference to the new clone.
		public StatusEventClass DeepClone()
		{
			BinaryFormatter BF = new BinaryFormatter();
			MemoryStream memStream = new MemoryStream();

			BF.Serialize(memStream, this);
			memStream.Flush();
			memStream.Position = 0;

			return ((StatusEventClass)BF.Deserialize(memStream));
		}


	}
}
