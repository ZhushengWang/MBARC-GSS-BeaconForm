using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;

namespace SPRL.Test
{
  public class UtilCmds
  {
    private static UtilCmds _utilcmds = null;
		private static object _instanceLock = new Object();

		// This is a singleton class
		// Return a reference to the only instance of HPDAQCmds
		// Create the instance if necessary
		public static UtilCmds Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_utilcmds == null)
					{
						_utilcmds = new UtilCmds();
					}
					return _utilcmds;
				}
			}
		}

		// Constructor
		public UtilCmds()
		{
		}

    public void Print(string Msg)
    {
      MainForm._mainform.PrintMsg(Msg);
      System.Threading.Thread.Sleep(1); //Sleep 1 ms so we don't clog up the UI
    }

    public void Wait(double Secs)
    {
      try
      {
        System.Threading.Thread.Sleep((int)(Secs * 1000));
      }
      catch (System.Threading.ThreadInterruptedException)
      {
        //Do nothing because we just want to continue if interrupted
      }
    }

    public void WaitUntil(double dSecs)
    {
      double dWaitSecs = 0.0;

      //dWaitSecs = dSecs - ExtDigIO.Instance.GetTime();
      if (dWaitSecs <= 0)
      {
        MainForm._mainform.PrintWarning("Warning: Wait=" + dWaitSecs + "\n");
      }
      else
      {
        try
        {
          System.Threading.Thread.Sleep((int)(dWaitSecs * 1000));
        }
        catch (System.Threading.ThreadInterruptedException)
        {
          //Do nothing because we just want to continue if interrupted
        }
      }
    }

    public void Beep()
    {
      Console.Beep(750, 50);
    }

    public bool UserBreak()
    {
      return (MainForm._mainform.CheckUserBreak());
    }

    public void SaveBinaryData(byte[] ayData, string sFname)
    {
      FileStream fsStream = new FileStream(sFname, FileMode.Create);

      fsStream.Write(ayData, 0, ayData.Length);

      fsStream.Close();
    }

    public void SendEmailEx(string to, string msg)
    {
      MailMessage objeto_mail = new MailMessage();
      SmtpClient client = new SmtpClient();
      client.Port = 587;
      client.Host = "smtp-exchange.umich.edu";
      client.Timeout = 10000;
      client.DeliveryMethod = SmtpDeliveryMethod.Network;
      client.UseDefaultCredentials = false;
      client.EnableSsl = true;
      client.Credentials = new System.Net.NetworkCredential("rpmiller@umich.edu", "!Turkey0808","UMROOT");
      objeto_mail.From = new MailAddress("rpmiller@umich.edu");
      objeto_mail.To.Add(new MailAddress(to));
      objeto_mail.Subject = "SPRL Test Lab";
      objeto_mail.Body = msg;
      client.Send(objeto_mail);
    }

    public void SendEmail(string to, string msg)
    {
      MailMessage objeto_mail = new MailMessage();
      SmtpClient client = new SmtpClient();
      client.Port = 587;
      client.Host = "smtp.gmail.com";
      client.Timeout = 10000;
      client.DeliveryMethod = SmtpDeliveryMethod.Network;
      client.UseDefaultCredentials = false;
      client.EnableSsl = true;
      client.Credentials = new System.Net.NetworkCredential("rpmillerum@gmail.com", "Ijbanbl!");
      objeto_mail.From = new MailAddress("rpmillerum@gmail.com");
      objeto_mail.To.Add(new MailAddress(to));
      objeto_mail.Subject = "SPRL Test Lab";
      objeto_mail.Body = msg;
      client.Send(objeto_mail);
    }
  }
}
