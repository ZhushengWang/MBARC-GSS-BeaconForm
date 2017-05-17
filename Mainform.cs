/******************************************************************************
*
* Program:
*   LADEE_Test
**
 * Description:
 *  
 * 
 * Programmer:
 *     Ryan P. Miller
 * 
 * Revisions:
 *     1.0    Original Release     June, 2010
 *     
 * 
* I/O Connections Overview:
*
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;
using System.IO;

using NationalInstruments;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;

namespace SPRL.Test
{
  /// <summary>
  /// Summary description for MainForm.
  /// </summary>
  public class MainForm : System.Windows.Forms.Form
  {
    public bool bScriptStop = false;
    private bool bCompiling = false;

    #region Main Variables/Constants
    public static MainForm _mainform = null;
    private IContainer components;
    private ListBox HistorylistBox;
    private TextBox CommandtextBox;
    private Label label1;
    private Label label2;
    private Label label8;
    private Label label9;
    private ListBox listBox1;
    private TextBox textBox1;
		private GroupBox groupBox3;
		private Label label10;
		private Label label11;
		private GroupBox groupBox4;
		private Label label5;
    private Label label12;
		private RichTextBox ScriptrichTextBox;
		private Label label13;
		private Button LoadScriptButton;
		private Button RunScriptButton;
		private Button StopScriptButton;
    private DataGridView dataGridView1;
    private GroupBox groupBox7;

		private nScriptHost.ScriptingHost _ScriptHost;
		private nScriptHost.ScriptingLanguage _ScriptLanguage = nScriptHost.ScriptingLanguage.JavaScript;

		private nScriptHost.ScriptingHost _CmdLineScriptHost;
		
		private RichTextBox MsgRichTextBox;
		private Label label4;

		private Label label7;
		private Label TempUpdateLabel;
		private Button ScriptSavebutton;
		private OpenFileDialog ScriptopenFileDialog;
		private SaveFileDialog ScriptsaveFileDialog;
		private Label ScriptFileName;
		private Button LogDirectoryButton;
		private Label label17;
		private Label label16;
		private Button LogFileButton;

    //Define objects used by script host
		public PSCmds psCmds = null;
    public HPDAQCmds hpDAQCmds = null;
    public UtilCmds utilCmds = null;
    public PGCmds pgCmds = null;
    public UCCmds ucCmds = null;
    public NI9264Cmds ni9264Cmds = null;
    public HVADCCmds hvadcCmds = null;
    public NI6225Cmds ni6225Cmds = null;
    public NI9205Cmds ni9205Cmds = null;
    public NI9403Cmds ni9403Cmds = null;
    public HV_PS_cmds hvpsCmds = null;
    public UnivSrcCmds usrcCmds = null;
    public COMCmds comCmds = null;
    public QB50Cmds qb50Cmds = null;
    public SimCmds simCmds = null;
    public SCPICmds scpiCmds = null;
    public TestEquityCmds teCmds = null;

		private FolderBrowserDialog LogFolderBrowserDialog;
		private ToolTip toolTip1;
		private SplitContainer splitContainer1;
		private CheckBox ReadOnlyCheckBox;
		private ContextMenuStrip RichScriptcontextMenuStrip;
		private ToolStripMenuItem SavetoolStripMenuItem;
		private ToolStripMenuItem CleartoolStripMenuItem;
		private ToolStripMenuItem ReadOnlytoolStripMenuItem;

		private LogFile logFile = null;

		private bool bScriptModified = false;
		private CheckBox UserBreakToggle;
		private bool bScriptNew = true;

		private static object _UserBreakLock = new Object();
		private ToolStripMenuItem PrintStripMenuItem;
		private PrintDialog printScriptDialog;
		private System.Drawing.Printing.PrintDocument PrintScriptDocument;
		private bool bUserBreak = false;
		private ContextMenuStrip LogFilecontextMenuStrip;
		private ToolStripMenuItem ViewLogFileMenuItem;
		private PictureBox pictureBox2;
    private PictureBox pictureBox1;
    private Button seqButton;
    public SequencerForm seqForm;
    private ToolStripMenuItem undoToolStripMenuItem;
    private DataGridViewTextBoxColumn Set_V;
    private DataGridViewTextBoxColumn Actual_V;
    private DataGridViewTextBoxColumn I;
    private Button BeaconButton;
    public BeaconForm beaconForm;
    public HKForm hkForm;

    private System.IO.StringReader ScriptStringReader;
    #endregion

    private Button Compilebutton;
    private SaveFileDialog CompileFileDialog;

    private Timer timerSerialCom;
    private Timer timerTimeout;

    //List for serial packet bytes from CTL
    private List<byte> bTotal = new List<byte>();
    private int nBytesPerLine = 0;
    private int nExtraBytes = 0;
    private Button button1;
    public bool bSerialDataisASCII = false;

    //List for network packet bytes from RoverSim
    private List<byte> bRSIMTotal = new List<byte>();

    public MainForm()
    {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//            
			_mainform = this;

      string[] RowData1 = { "--", "--", "--" };
      string[] RowData2 = { "--", "--", "--" };
      string[] RowData3 = { "--", "--", "--" };
      string[] RowData4 = { "--", "--", "--" };
      string[] RowData5 = { "--", "--", "--" };
      string[] RowData6 = { "--", "--", "--" };
      string[] RowData7 = { "--", "--", "--" };
      string[] RowData8 = { "--", "--", "--" };
      string[] RowData9 = { "--", "--", "--" };
      string[] RowData10 = { "--", "--", "--" };
      string[] RowData11 = { "--", "--", "--" };
      string[] RowData12 = { "--", "--", "--" };

      dataGridView1.Rows.Add(RowData1);
			dataGridView1.Rows.Add(RowData2);
			dataGridView1.Rows.Add(RowData3);
			dataGridView1.Rows.Add(RowData4);
			dataGridView1.Rows.Add(RowData5);
			dataGridView1.Rows.Add(RowData6);
			dataGridView1.Rows.Add(RowData7);
			dataGridView1.Rows.Add(RowData8);
			dataGridView1.Rows.Add(RowData9);
			dataGridView1.Rows.Add(RowData10);
			dataGridView1.Rows.Add(RowData11);
			dataGridView1.Rows.Add(RowData12);

			dataGridView1.Rows[0].Cells[0].Selected = true;
			dataGridView1.ClearSelection();
			dataGridView1.Refresh();

      dataGridView1.Rows[0].Cells[0].Style.BackColor = Color.White;
      dataGridView1.Rows[0].Cells[1].Style.BackColor = Color.White;
      dataGridView1.Rows[0].Cells[2].Style.BackColor = Color.White;

      dataGridView1.Rows[1].Cells[0].Style.BackColor = Color.Silver;
      dataGridView1.Rows[1].Cells[1].Style.BackColor = Color.Silver;
      dataGridView1.Rows[1].Cells[2].Style.BackColor = Color.Silver;

      dataGridView1.Rows[2].Cells[0].Style.BackColor = Color.White;
      dataGridView1.Rows[2].Cells[1].Style.BackColor = Color.White;
      dataGridView1.Rows[2].Cells[2].Style.BackColor = Color.White;

      dataGridView1.Rows[3].Cells[0].Style.BackColor = Color.Silver;
      dataGridView1.Rows[3].Cells[1].Style.BackColor = Color.Silver;
      dataGridView1.Rows[3].Cells[2].Style.BackColor = Color.Silver;

      dataGridView1.Rows[4].Cells[0].Style.BackColor = Color.White;
      dataGridView1.Rows[4].Cells[1].Style.BackColor = Color.White;
      dataGridView1.Rows[4].Cells[2].Style.BackColor = Color.White;

      dataGridView1.Rows[5].Cells[0].Style.BackColor = Color.Silver;
      dataGridView1.Rows[5].Cells[1].Style.BackColor = Color.Silver;
      dataGridView1.Rows[5].Cells[2].Style.BackColor = Color.Silver;

      dataGridView1.Rows[6].Cells[0].Style.BackColor = Color.White;
      dataGridView1.Rows[6].Cells[1].Style.BackColor = Color.White;
      dataGridView1.Rows[6].Cells[2].Style.BackColor = Color.White;

      dataGridView1.Rows[7].Cells[0].Style.BackColor = Color.Silver;
      dataGridView1.Rows[7].Cells[1].Style.BackColor = Color.Silver;
      dataGridView1.Rows[7].Cells[2].Style.BackColor = Color.Silver;

      dataGridView1.Rows[8].Cells[0].Style.BackColor = Color.White;
      dataGridView1.Rows[8].Cells[1].Style.BackColor = Color.White;
      dataGridView1.Rows[8].Cells[2].Style.BackColor = Color.White;

      dataGridView1.Rows[9].Cells[0].Style.BackColor = Color.Silver;
      dataGridView1.Rows[9].Cells[1].Style.BackColor = Color.Silver;
      dataGridView1.Rows[9].Cells[2].Style.BackColor = Color.Silver;

      dataGridView1.Rows[10].Cells[0].Style.BackColor = Color.White;
      dataGridView1.Rows[10].Cells[1].Style.BackColor = Color.White;
      dataGridView1.Rows[10].Cells[2].Style.BackColor = Color.White;

      dataGridView1.Rows[11].Cells[0].Style.BackColor = Color.Silver;
      dataGridView1.Rows[11].Cells[1].Style.BackColor = Color.Silver;
      dataGridView1.Rows[11].Cells[2].Style.BackColor = Color.Silver;

			ScriptRunning(false);

			//Init scripting stuff
			_ScriptHost = new nScriptHost.ScriptingHost();
			_ScriptHost.CompileError += new nScriptHost.CompileErrorHandler(ScriptHost_CompileError);
			_ScriptHost.ScriptException += new nScriptHost.ScriptExeceptionHandler(ScriptHost_ScriptException);
			_ScriptHost.ExecutionComplete += new EventHandler(ScriptHost_ExecutionComplete);

			_CmdLineScriptHost = new nScriptHost.ScriptingHost();
			_CmdLineScriptHost.CompileError += new nScriptHost.CompileErrorHandler(_CmdLineScriptHost_CompileError);
			_CmdLineScriptHost.ScriptException += new nScriptHost.ScriptExeceptionHandler(_CmdLineScriptHost_ScriptException);
			_CmdLineScriptHost.ExecutionComplete += new EventHandler(_CmdLineScriptHost_ExecutionComplete);

      seqForm = new SequencerForm();
      beaconForm = new BeaconForm();
      hkForm = new HKForm();
      
      utilCmds = UtilCmds.Instance;
			psCmds = PSCmds.Instance;
      hpDAQCmds = HPDAQCmds.Instance;
      pgCmds = PGCmds.Instance;
      ucCmds = UCCmds.Instance;
      hvadcCmds = HVADCCmds.Instance;
      ni6225Cmds = NI6225Cmds.Instance;
      ni9205Cmds = NI9205Cmds.Instance;
      ni9403Cmds = NI9403Cmds.Instance;
      ni9264Cmds = NI9264Cmds.Instance;
      hvpsCmds = HV_PS_cmds.Instance;
      usrcCmds = UnivSrcCmds.Instance;
      comCmds = COMCmds.Instance;
      qb50Cmds = QB50Cmds.Instance;
      simCmds = SimCmds.Instance;
      scpiCmds = SCPICmds.Instance;
      teCmds = TestEquityCmds.Instance;

      timerSerialCom = new Timer();
      timerSerialCom.Interval = 200;
      timerSerialCom.Tick += new EventHandler(timerSerialCom_Tick);
      timerSerialCom.Enabled = true;

      timerTimeout = new Timer();
      timerTimeout.Interval = 200;
      timerTimeout.Tick += new EventHandler(timerTimeout_Tick);
      timerTimeout.Enabled = true;

    }
      
    //Rover Sim Network Data Processing
    void timerTimeout_Tick(object sender, EventArgs e)
    {
      byte[] bData = null;
      List<List<byte>> list = new List<List<byte>>();
      List<byte> listPkt;

      if (simCmds.bConnected() == false)
      {
        bRSIMTotal.Clear();
      }

      while (simCmds.bNetDataAvailable())
      {
        timerTimeout.Stop();

        bData = simCmds.GetNetData();
        bRSIMTotal.AddRange(bData);

        while (bRSIMTotal.Count >= MOMARSimPkt.MOMA_RSIM_PKT_SIZE)
        {
          listPkt = bRSIMTotal.GetRange(0, MOMARSimPkt.MOMA_RSIM_PKT_SIZE);

          bRSIMTotal.RemoveRange(0, MOMARSimPkt.MOMA_RSIM_PKT_SIZE);

          list.Add(listPkt);
        }
      }

      foreach (var subPkt in list)
      {
        MOMARSimPkt rsimPkt = new MOMARSimPkt(subPkt);

        //rsimForm.DisplayPacket(rsimPkt);
      }

      timerTimeout.Start();
    }

    bool verifyChecksum(List<byte> listPkt)
    {
      UInt16 nChecksum=0;
      UInt16 nPktChecksum = (UInt16)(listPkt[listPkt.Count-2]*256 + listPkt[listPkt.Count-1]);

      for (int i = 0; i < listPkt.Count-2; i++)
      {
        nChecksum += listPkt[i];
      }

      if (nChecksum == nPktChecksum)
      {
        return (true);
      }
      else
      {
        return (false);
      }
    }

    void timerSerialCom_Tick(object sender, EventArgs e)
    {
      byte[] bData = null;
      List<List<byte>> list = new List<List<byte>>();
      List<byte> listPkt;
      bool bOutofSync = false;

      bool bCommandLoopbackTest = false;


      while (comCmds.bSerialDataAvailable())
      {
        //timerTimeout.Stop(); //Restart the timeout timer
        timerSerialCom.Stop();

        bData = comCmds.GetSerialData();

        if (bSerialDataisASCII)
        {
          for (int i = 0; i < bData.Length; i++)  //Only add hex
          {
            if (((bData[i] >= (byte)'0') && (bData[i] <= (byte)'9')) || ((bData[i] >= (byte)'A') && (bData[i] <= (byte)'F')))
            {
              bTotal.Add(bData[i]);
            }
          }
        }
        else //Binary data
        {
          bTotal.AddRange(bData);
        }

        if (bCommandLoopbackTest)
        {
          //Print out bytes received
          for (int i = 0; i < bData.Length; i++)
          {
            PrintMsg(bData[i].ToString("X2"));
            nBytesPerLine++;

            if (nBytesPerLine == 8)
            {
              PrintMsg("\n");
              nBytesPerLine = 0;
            }
          }
        }
      }

      if (bSerialDataisASCII)
      {
        while ((bTotal.Count >= 12) && (!bCommandLoopbackTest))
        {
          if ((bTotal[0] == 'F') && (bTotal[1] == 'A') &&
              (bTotal[2] == 'F') && (bTotal[3] == '3') &&
              (bTotal[4] == '2') && (bTotal[5] == '0'))
          {
            int nDataCount = MakeByteFromASCII(bTotal[8], bTotal[9]) * 256 + MakeByteFromASCII(bTotal[10], bTotal[11]) + 2;

            if (bTotal.Count >= ((nDataCount + 6) * 2) + 33)  //Extra 33 bytes, thrown away later
            {
              listPkt = new List<byte>(nDataCount + 6);

              for (int i = 0; i < ((nDataCount + 6) * 2); i += 2)
              {
                listPkt.Add(MakeByteFromASCII(bTotal[i], bTotal[i + 1]));
              }

              bTotal.RemoveRange(0, (nDataCount + 6) * 2 + 33);
              //PrintMsg("Got Packet Type " + listPkt[3] + "\n");
              if (!verifyChecksum(listPkt))
              {
                PrintError("Error: Packet Type " + listPkt[3] + ", Bad Checksum.\n");
              }
              else
              {
                list.Add(listPkt);
                if (qb50Cmds.tm_log != null)
                {
                  qb50Cmds.tm_log.Write(listPkt.ToArray(), 0, listPkt.Count);
                }
              }
            }
            else
            {
              break;  //Display what we received (if any)
            }
          }
          else
          {
            if (bOutofSync == false)
            {
              PrintError("Out of Sync\n");
              bOutofSync = true;
            }
            bTotal.RemoveAt(0);  //remove one byte and try again
          }
        }
      }
      else  //Binary processing
      {
        while ((bTotal.Count >= 6) && (!bCommandLoopbackTest))
        {
          if ((bTotal[0] == 0xFA) && (bTotal[1] == 0xF3) && ((bTotal[2] == 0x20) || (bTotal[2] == 0x21)))
          {
            int nDataCount = bTotal[4] * 256 + bTotal[5] + 2;

            if (bTotal.Count >= (nDataCount + 6))
            {
              listPkt = bTotal.GetRange(0, nDataCount + 6);

              bTotal.RemoveRange(0, nDataCount + 6);
              //PrintMsg("Got Packet Type " + listPkt[3] + "\n");
              if (!verifyChecksum(listPkt))
              {
                PrintError("Error: Packet Type " + listPkt[3] + ", Bad Checksum.\n");
              }
              else
              {
                list.Add(listPkt);
                if (qb50Cmds.tm_log != null)
                {
                  qb50Cmds.tm_log.Write(listPkt.ToArray(), 0, listPkt.Count);
                }
              }
            }
            else
            {
              break;  //Display what we received (if any)
            }
          }
          else
          {
            if (bOutofSync == false)
            {
              PrintError("Out of Sync\n");
              bOutofSync = true;
            }
            bTotal.RemoveAt(0);  //remove one byte and try again
          }
        }
      }

      foreach (var subPkt in list)
      {
        SaveToFile(subPkt);

        switch (subPkt[3])
        {
          case QB50Cmds.ACK_ID:
            seqForm.displayAck(subPkt);
            break;

          case QB50Cmds.BEACON_ID:
            beaconForm.displayBeacon(subPkt);
            break;

          case QB50Cmds.MSG_ID:
            seqForm.displayMsg(subPkt);
            break;

          case QB50Cmds.MSG_TIME_ID:
            seqForm.displayMsgTime(subPkt);
            break;

          case QB50Cmds.TEST_ID:
            seqForm.displayTest(subPkt);
            break;

          case QB50Cmds.HK_ID:
            hkForm.displayHK(subPkt);
            break;

          case QB50Cmds.SU_STORED_ID:
            qb50Cmds.FIP_ProcessData(subPkt);
            break;

          case QB50Cmds.OPSTATUS_ID:
            qb50Cmds.displayOPSTATUS(subPkt);
            break;
          
          case QB50Cmds.DATA_ID:
        //    qb50Cmds.DAQ_StoreData(subPkt);
            break;
          default:
            PrintError("Error: Bad Packet Type" + subPkt[3] + "\n");
            break;
        }
      }

      timerSerialCom.Start();
    }

    private void SaveToFile(List<byte> subPkt)
    {
      DateTime dateDate = DateTime.Today ;
      String sFilename = "";
      String sFullName = "";
      String sDirectory = "C:\\Users\\rpmiller\\Desktop\\QB50_Data\\";

      //The sync code is 0xFAF320 for US04 and 0xFAF321 for US02
      if (subPkt[2] == 0x20)
      {
        sFilename = "US04-"+DateTime.Now.ToString("yyyy-MM-dd") + ".pkt";
      }
      else
      {
        if (subPkt[2] == 0x21)
        {
          sFilename = "US02-" + DateTime.Now.ToString("yyyy-MM-dd") + ".pkt";
        }
      }

      sFullName = sDirectory + sFilename;

      FileStream fsData = new FileStream(sFullName, FileMode.Append);

      fsData.Write(subPkt.ToArray(), 0, subPkt.Count);

      fsData.Close();
    }

    void _CmdLineScriptHost_ExecutionComplete(object sender, EventArgs e)
		{
			//Don't do anything for command line scripts
		}

		void _CmdLineScriptHost_ScriptException(object sender, nScriptHost.ScriptExceptionEventArgs e)
		{
      char[] cIndexChars = new char[1];

      cIndexChars[0] = '\n';
      int nIndex = e.Exception.Message.IndexOfAny(cIndexChars);

			PrintError("Command Error!\n");
      PrintError(e.Exception.Message.Substring(0,nIndex+1));
      //PrintError(e.Exception.Message+"\n");
		}

		void _CmdLineScriptHost_CompileError(object sender, nScriptHost.CompileErrorEventArgs e)
		{
			PrintError("Command Error!\n");
		}

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (components != null) 
        {
           components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      this.HistorylistBox = new System.Windows.Forms.ListBox();
      this.CommandtextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label10 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.ScriptrichTextBox = new System.Windows.Forms.RichTextBox();
      this.RichScriptcontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SavetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.PrintStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CleartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ReadOnlytoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.label13 = new System.Windows.Forms.Label();
      this.LoadScriptButton = new System.Windows.Forms.Button();
      this.RunScriptButton = new System.Windows.Forms.Button();
      this.StopScriptButton = new System.Windows.Forms.Button();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.Set_V = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Actual_V = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.I = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.groupBox7 = new System.Windows.Forms.GroupBox();
      this.LogFilecontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ViewLogFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.LogFileButton = new System.Windows.Forms.Button();
      this.LogDirectoryButton = new System.Windows.Forms.Button();
      this.label17 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.MsgRichTextBox = new System.Windows.Forms.RichTextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.TempUpdateLabel = new System.Windows.Forms.Label();
      this.ScriptSavebutton = new System.Windows.Forms.Button();
      this.ScriptopenFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.ScriptsaveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.ScriptFileName = new System.Windows.Forms.Label();
      this.LogFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.UserBreakToggle = new System.Windows.Forms.CheckBox();
      this.Compilebutton = new System.Windows.Forms.Button();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.button1 = new System.Windows.Forms.Button();
      this.BeaconButton = new System.Windows.Forms.Button();
      this.seqButton = new System.Windows.Forms.Button();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.ReadOnlyCheckBox = new System.Windows.Forms.CheckBox();
      this.printScriptDialog = new System.Windows.Forms.PrintDialog();
      this.PrintScriptDocument = new System.Drawing.Printing.PrintDocument();
      this.CompileFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.RichScriptcontextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.groupBox7.SuspendLayout();
      this.LogFilecontextMenuStrip.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // HistorylistBox
      // 
      this.HistorylistBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.HistorylistBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HistorylistBox.FormattingEnabled = true;
      this.HistorylistBox.HorizontalScrollbar = true;
      this.HistorylistBox.ItemHeight = 14;
      this.HistorylistBox.Location = new System.Drawing.Point(3, 212);
      this.HistorylistBox.Name = "HistorylistBox";
      this.HistorylistBox.ScrollAlwaysVisible = true;
      this.HistorylistBox.Size = new System.Drawing.Size(156, 144);
      this.HistorylistBox.TabIndex = 9;
      this.toolTip1.SetToolTip(this.HistorylistBox, "Commands typed in the command box are\r\ndispalyed here.  Use the up and down arrow" +
              "s\r\nor the mouse to select a command to re-execute.");
      this.HistorylistBox.SelectedIndexChanged += new System.EventHandler(this.HistorylistBox_SelectedIndexChanged);
      // 
      // CommandtextBox
      // 
      this.CommandtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.CommandtextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CommandtextBox.Location = new System.Drawing.Point(3, 376);
      this.CommandtextBox.Name = "CommandtextBox";
      this.CommandtextBox.Size = new System.Drawing.Size(156, 20);
      this.CommandtextBox.TabIndex = 1;
      this.toolTip1.SetToolTip(this.CommandtextBox, resources.GetString("CommandtextBox.ToolTip"));
      this.CommandtextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandtextBox_KeyDown);
      this.CommandtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CommandtextBox_KeyPress);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(1, 359);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(57, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Command:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 2);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Script:";
      this.toolTip1.SetToolTip(this.label2, "The Script window.  JScript scripts can be typed here.");
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(1, 292);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(57, 13);
      this.label8.TabIndex = 5;
      this.label8.Text = "Command:";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(1, 33);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(42, 13);
      this.label9.TabIndex = 5;
      this.label9.Text = "History:";
      // 
      // listBox1
      // 
      this.listBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listBox1.FormattingEnabled = true;
      this.listBox1.HorizontalScrollbar = true;
      this.listBox1.ItemHeight = 14;
      this.listBox1.Location = new System.Drawing.Point(4, 47);
      this.listBox1.Name = "listBox1";
      this.listBox1.ScrollAlwaysVisible = true;
      this.listBox1.Size = new System.Drawing.Size(230, 242);
      this.listBox1.TabIndex = 9;
      // 
      // textBox1
      // 
      this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBox1.Location = new System.Drawing.Point(4, 308);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(230, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandtextBox_KeyDown);
      this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CommandtextBox_KeyPress);
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label10);
      this.groupBox3.Controls.Add(this.label11);
      this.groupBox3.Location = new System.Drawing.Point(0, 0);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(200, 100);
      this.groupBox3.TabIndex = 0;
      this.groupBox3.TabStop = false;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 44);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(45, 13);
      this.label10.TabIndex = 12;
      this.label10.Text = "Ambient";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(6, 19);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(48, 13);
      this.label11.TabIndex = 12;
      this.label11.Text = "ColdFET";
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label5);
      this.groupBox4.Controls.Add(this.label12);
      this.groupBox4.Location = new System.Drawing.Point(0, 0);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(200, 100);
      this.groupBox4.TabIndex = 0;
      this.groupBox4.TabStop = false;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 44);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(45, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Ambient";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(6, 19);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(48, 13);
      this.label12.TabIndex = 12;
      this.label12.Text = "ColdFET";
      // 
      // ScriptrichTextBox
      // 
      this.ScriptrichTextBox.AcceptsTab = true;
      this.ScriptrichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ScriptrichTextBox.ContextMenuStrip = this.RichScriptcontextMenuStrip;
      this.ScriptrichTextBox.DetectUrls = false;
      this.ScriptrichTextBox.EnableAutoDragDrop = true;
      this.ScriptrichTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ScriptrichTextBox.Location = new System.Drawing.Point(3, 18);
      this.ScriptrichTextBox.Name = "ScriptrichTextBox";
      this.ScriptrichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
      this.ScriptrichTextBox.Size = new System.Drawing.Size(277, 326);
      this.ScriptrichTextBox.TabIndex = 15;
      this.ScriptrichTextBox.Text = "";
      this.ScriptrichTextBox.WordWrap = false;
      this.ScriptrichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScriptrichTextBox_KeyDown);
      this.ScriptrichTextBox.TextChanged += new System.EventHandler(this.ScriptrichTextBox_TextChanged);
      // 
      // RichScriptcontextMenuStrip
      // 
      this.RichScriptcontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SavetoolStripMenuItem,
            this.PrintStripMenuItem,
            this.CleartoolStripMenuItem,
            this.ReadOnlytoolStripMenuItem,
            this.undoToolStripMenuItem});
      this.RichScriptcontextMenuStrip.Name = "RichScriptcontextMenuStrip";
      this.RichScriptcontextMenuStrip.ShowCheckMargin = true;
      this.RichScriptcontextMenuStrip.ShowImageMargin = false;
      this.RichScriptcontextMenuStrip.Size = new System.Drawing.Size(129, 114);
      // 
      // SavetoolStripMenuItem
      // 
      this.SavetoolStripMenuItem.Name = "SavetoolStripMenuItem";
      this.SavetoolStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.SavetoolStripMenuItem.Text = "Save...";
      this.SavetoolStripMenuItem.ToolTipText = "Save the current script";
      this.SavetoolStripMenuItem.Click += new System.EventHandler(this.SavetoolStripMenuItem_Click);
      // 
      // PrintStripMenuItem
      // 
      this.PrintStripMenuItem.Name = "PrintStripMenuItem";
      this.PrintStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.PrintStripMenuItem.Text = "Print...";
      this.PrintStripMenuItem.ToolTipText = "Print the current script.";
      this.PrintStripMenuItem.Click += new System.EventHandler(this.PrintStripMenuItem_Click);
      // 
      // CleartoolStripMenuItem
      // 
      this.CleartoolStripMenuItem.Name = "CleartoolStripMenuItem";
      this.CleartoolStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.CleartoolStripMenuItem.Text = "New...";
      this.CleartoolStripMenuItem.ToolTipText = "Clear the script window";
      this.CleartoolStripMenuItem.Click += new System.EventHandler(this.CleartoolStripMenuItem_Click);
      // 
      // ReadOnlytoolStripMenuItem
      // 
      this.ReadOnlytoolStripMenuItem.CheckOnClick = true;
      this.ReadOnlytoolStripMenuItem.Name = "ReadOnlytoolStripMenuItem";
      this.ReadOnlytoolStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.ReadOnlytoolStripMenuItem.Text = "Read Only";
      this.ReadOnlytoolStripMenuItem.ToolTipText = "Toggle Read-Only";
      this.ReadOnlytoolStripMenuItem.Click += new System.EventHandler(this.ReadOnlytoolStripMenuItem_Click);
      // 
      // undoToolStripMenuItem
      // 
      this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
      this.undoToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.undoToolStripMenuItem.Text = "Undo";
      this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
      // 
      // label13
      // 
      this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(3, 195);
      this.label13.Margin = new System.Windows.Forms.Padding(0);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(42, 13);
      this.label13.TabIndex = 5;
      this.label13.Text = "History:";
      // 
      // LoadScriptButton
      // 
      this.LoadScriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.LoadScriptButton.Location = new System.Drawing.Point(3, 376);
      this.LoadScriptButton.Name = "LoadScriptButton";
      this.LoadScriptButton.Size = new System.Drawing.Size(54, 23);
      this.LoadScriptButton.TabIndex = 16;
      this.LoadScriptButton.Text = "Load";
      this.toolTip1.SetToolTip(this.LoadScriptButton, "Load a script.");
      this.LoadScriptButton.UseVisualStyleBackColor = true;
      this.LoadScriptButton.Click += new System.EventHandler(this.LoadScriptButton_Click);
      // 
      // RunScriptButton
      // 
      this.RunScriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.RunScriptButton.Location = new System.Drawing.Point(138, 376);
      this.RunScriptButton.Name = "RunScriptButton";
      this.RunScriptButton.Size = new System.Drawing.Size(38, 23);
      this.RunScriptButton.TabIndex = 16;
      this.RunScriptButton.Text = "Run";
      this.toolTip1.SetToolTip(this.RunScriptButton, "Run the current script.");
      this.RunScriptButton.UseVisualStyleBackColor = true;
      this.RunScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
      // 
      // StopScriptButton
      // 
      this.StopScriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StopScriptButton.Enabled = false;
      this.StopScriptButton.Location = new System.Drawing.Point(182, 376);
      this.StopScriptButton.Name = "StopScriptButton";
      this.StopScriptButton.Size = new System.Drawing.Size(38, 23);
      this.StopScriptButton.TabIndex = 16;
      this.StopScriptButton.Text = "Stop";
      this.toolTip1.SetToolTip(this.StopScriptButton, "Stop execution of the current script.");
      this.StopScriptButton.UseVisualStyleBackColor = true;
      this.StopScriptButton.Click += new System.EventHandler(this.StopScriptButton_Click);
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AllowUserToResizeColumns = false;
      this.dataGridView1.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Set_V,
            this.Actual_V,
            this.I});
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
      this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dataGridView1.Enabled = false;
      this.dataGridView1.GridColor = System.Drawing.SystemColors.Window;
      this.dataGridView1.Location = new System.Drawing.Point(286, 18);
      this.dataGridView1.MultiSelect = false;
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
      dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
      dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle8;
      this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
      this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
      this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window;
      this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.RowTemplate.Height = 20;
      this.dataGridView1.RowTemplate.ReadOnly = true;
      this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.ShowCellErrors = false;
      this.dataGridView1.ShowCellToolTips = false;
      this.dataGridView1.ShowEditingIcon = false;
      this.dataGridView1.ShowRowErrors = false;
      this.dataGridView1.Size = new System.Drawing.Size(188, 244);
      this.dataGridView1.TabIndex = 17;
      this.dataGridView1.TabStop = false;
      this.toolTip1.SetToolTip(this.dataGridView1, "This grid displays the voltage and current from power supplies.");
      // 
      // Set_V
      // 
      this.Set_V.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Silver;
      this.Set_V.DefaultCellStyle = dataGridViewCellStyle3;
      this.Set_V.DividerWidth = 2;
      this.Set_V.FillWeight = 33F;
      this.Set_V.HeaderText = "Set V";
      this.Set_V.MinimumWidth = 50;
      this.Set_V.Name = "Set_V";
      this.Set_V.ReadOnly = true;
      this.Set_V.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.Set_V.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // Actual_V
      // 
      this.Actual_V.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.NullValue = null;
      dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
      this.Actual_V.DefaultCellStyle = dataGridViewCellStyle4;
      this.Actual_V.FillWeight = 33F;
      this.Actual_V.HeaderText = "Actual V";
      this.Actual_V.Name = "Actual_V";
      this.Actual_V.ReadOnly = true;
      this.Actual_V.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.Actual_V.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // I
      // 
      this.I.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
      this.I.DefaultCellStyle = dataGridViewCellStyle5;
      this.I.FillWeight = 33F;
      this.I.HeaderText = "I";
      this.I.Name = "I";
      this.I.ReadOnly = true;
      this.I.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.I.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // groupBox7
      // 
      this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox7.ContextMenuStrip = this.LogFilecontextMenuStrip;
      this.groupBox7.Controls.Add(this.LogFileButton);
      this.groupBox7.Controls.Add(this.LogDirectoryButton);
      this.groupBox7.Controls.Add(this.label17);
      this.groupBox7.Controls.Add(this.label16);
      this.groupBox7.Location = new System.Drawing.Point(516, 9);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new System.Drawing.Size(151, 61);
      this.groupBox7.TabIndex = 20;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "Log File";
      this.toolTip1.SetToolTip(this.groupBox7, "Right-click to view log files\r\nin a text editor.  Click on the \r\nDirectory button" +
              " to set the Log\r\nFile Directory.");
      // 
      // LogFilecontextMenuStrip
      // 
      this.LogFilecontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewLogFileMenuItem});
      this.LogFilecontextMenuStrip.Name = "LogFilecontextMenuStrip";
      this.LogFilecontextMenuStrip.ShowImageMargin = false;
      this.LogFilecontextMenuStrip.Size = new System.Drawing.Size(137, 26);
      // 
      // ViewLogFileMenuItem
      // 
      this.ViewLogFileMenuItem.Name = "ViewLogFileMenuItem";
      this.ViewLogFileMenuItem.Size = new System.Drawing.Size(136, 22);
      this.ViewLogFileMenuItem.Text = "View a Log File...";
      this.ViewLogFileMenuItem.Click += new System.EventHandler(this.ViewLogFileMenuItem_Click);
      // 
      // LogFileButton
      // 
      this.LogFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.LogFileButton.AutoEllipsis = true;
      this.LogFileButton.Location = new System.Drawing.Point(65, 37);
      this.LogFileButton.Margin = new System.Windows.Forms.Padding(0);
      this.LogFileButton.Name = "LogFileButton";
      this.LogFileButton.Size = new System.Drawing.Size(82, 22);
      this.LogFileButton.TabIndex = 20;
      this.LogFileButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
      this.toolTip1.SetToolTip(this.LogFileButton, "Current Log File\r\nClick to \'checkpoint\' the file and start a new one.\r\nAutomatica" +
              "lly checkpoints every hour.");
      this.LogFileButton.UseVisualStyleBackColor = true;
      this.LogFileButton.Click += new System.EventHandler(this.LogFileButton_Click);
      // 
      // LogDirectoryButton
      // 
      this.LogDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.LogDirectoryButton.AutoEllipsis = true;
      this.LogDirectoryButton.Location = new System.Drawing.Point(65, 13);
      this.LogDirectoryButton.Margin = new System.Windows.Forms.Padding(0);
      this.LogDirectoryButton.Name = "LogDirectoryButton";
      this.LogDirectoryButton.Size = new System.Drawing.Size(82, 22);
      this.LogDirectoryButton.TabIndex = 20;
      this.LogDirectoryButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
      this.toolTip1.SetToolTip(this.LogDirectoryButton, "Change Log File Directory\r\nGreen means log file is being recorded.");
      this.LogDirectoryButton.UseVisualStyleBackColor = true;
      this.LogDirectoryButton.Click += new System.EventHandler(this.LogDirectoryButton_Click);
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(1, 41);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(63, 13);
      this.label17.TabIndex = 19;
      this.label17.Text = "Current File:";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(12, 18);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(52, 13);
      this.label16.TabIndex = 19;
      this.label16.Text = "Directory:";
      // 
      // MsgRichTextBox
      // 
      this.MsgRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.MsgRichTextBox.DetectUrls = false;
      this.MsgRichTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MsgRichTextBox.Location = new System.Drawing.Point(3, 18);
      this.MsgRichTextBox.Name = "MsgRichTextBox";
      this.MsgRichTextBox.ReadOnly = true;
      this.MsgRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
      this.MsgRichTextBox.Size = new System.Drawing.Size(156, 174);
      this.MsgRichTextBox.TabIndex = 15;
      this.MsgRichTextBox.Text = "";
      this.toolTip1.SetToolTip(this.MsgRichTextBox, "Error, Warning and informational messages\r\nare displayed here.");
      this.MsgRichTextBox.WordWrap = false;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 1);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(58, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Messages:";
      // 
      // label7
      // 
      this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(283, 0);
      this.label7.Margin = new System.Windows.Forms.Padding(0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(86, 13);
      this.label7.TabIndex = 22;
      this.label7.Text = "Temps Updated:";
      // 
      // TempUpdateLabel
      // 
      this.TempUpdateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.TempUpdateLabel.AutoSize = true;
      this.TempUpdateLabel.Location = new System.Drawing.Point(372, 1);
      this.TempUpdateLabel.Name = "TempUpdateLabel";
      this.TempUpdateLabel.Size = new System.Drawing.Size(13, 13);
      this.TempUpdateLabel.TabIndex = 22;
      this.TempUpdateLabel.Text = "--";
      this.toolTip1.SetToolTip(this.TempUpdateLabel, "Time of last temperature update.");
      // 
      // ScriptSavebutton
      // 
      this.ScriptSavebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ScriptSavebutton.Location = new System.Drawing.Point(63, 376);
      this.ScriptSavebutton.Name = "ScriptSavebutton";
      this.ScriptSavebutton.Size = new System.Drawing.Size(54, 23);
      this.ScriptSavebutton.TabIndex = 16;
      this.ScriptSavebutton.Text = "Save";
      this.toolTip1.SetToolTip(this.ScriptSavebutton, "Save the current script.");
      this.ScriptSavebutton.UseVisualStyleBackColor = true;
      this.ScriptSavebutton.Click += new System.EventHandler(this.ScriptSavebutton_Click);
      // 
      // ScriptopenFileDialog
      // 
      this.ScriptopenFileDialog.DefaultExt = "txt";
      this.ScriptopenFileDialog.Title = "Load Script";
      this.ScriptopenFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.ScriptopenFileDialog_FileOk);
      // 
      // ScriptsaveFileDialog
      // 
      this.ScriptsaveFileDialog.DefaultExt = "txt";
      this.ScriptsaveFileDialog.Title = "Save Script";
      this.ScriptsaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.ScriptsaveFileDialog_FileOk);
      // 
      // ScriptFileName
      // 
      this.ScriptFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ScriptFileName.AutoSize = true;
      this.ScriptFileName.Location = new System.Drawing.Point(42, 2);
      this.ScriptFileName.Name = "ScriptFileName";
      this.ScriptFileName.Size = new System.Drawing.Size(10, 13);
      this.ScriptFileName.TabIndex = 5;
      this.ScriptFileName.Text = " ";
      // 
      // toolTip1
      // 
      this.toolTip1.AutoPopDelay = 5000;
      this.toolTip1.InitialDelay = 1000;
      this.toolTip1.ReshowDelay = 500;
      this.toolTip1.ShowAlways = true;
      // 
      // UserBreakToggle
      // 
      this.UserBreakToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.UserBreakToggle.Appearance = System.Windows.Forms.Appearance.Button;
      this.UserBreakToggle.Location = new System.Drawing.Point(138, 347);
      this.UserBreakToggle.Name = "UserBreakToggle";
      this.UserBreakToggle.Size = new System.Drawing.Size(142, 23);
      this.UserBreakToggle.TabIndex = 24;
      this.UserBreakToggle.Text = "User Break";
      this.UserBreakToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTip1.SetToolTip(this.UserBreakToggle, "Clicking sets a script flag so that\\nJuno.UserBreak() returns true.");
      this.UserBreakToggle.UseVisualStyleBackColor = true;
      this.UserBreakToggle.CheckedChanged += new System.EventHandler(this.UserBreakToggle_CheckedChanged);
      // 
      // Compilebutton
      // 
      this.Compilebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.Compilebutton.Location = new System.Drawing.Point(226, 376);
      this.Compilebutton.Name = "Compilebutton";
      this.Compilebutton.Size = new System.Drawing.Size(54, 23);
      this.Compilebutton.TabIndex = 31;
      this.Compilebutton.Text = "Compile";
      this.toolTip1.SetToolTip(this.Compilebutton, "Run the current script.");
      this.Compilebutton.UseVisualStyleBackColor = true;
      this.Compilebutton.Click += new System.EventHandler(this.Compilebutton_Click);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.splitContainer1.Location = new System.Drawing.Point(4, 74);
      this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.label13);
      this.splitContainer1.Panel1.Controls.Add(this.label4);
      this.splitContainer1.Panel1.Controls.Add(this.MsgRichTextBox);
      this.splitContainer1.Panel1.Controls.Add(this.HistorylistBox);
      this.splitContainer1.Panel1.Controls.Add(this.CommandtextBox);
      this.splitContainer1.Panel1.Controls.Add(this.label1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.button1);
      this.splitContainer1.Panel2.Controls.Add(this.Compilebutton);
      this.splitContainer1.Panel2.Controls.Add(this.BeaconButton);
      this.splitContainer1.Panel2.Controls.Add(this.seqButton);
      this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
      this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
      this.splitContainer1.Panel2.Controls.Add(this.UserBreakToggle);
      this.splitContainer1.Panel2.Controls.Add(this.ReadOnlyCheckBox);
      this.splitContainer1.Panel2.Controls.Add(this.ScriptrichTextBox);
      this.splitContainer1.Panel2.Controls.Add(this.TempUpdateLabel);
      this.splitContainer1.Panel2.Controls.Add(this.label2);
      this.splitContainer1.Panel2.Controls.Add(this.label7);
      this.splitContainer1.Panel2.Controls.Add(this.LoadScriptButton);
      this.splitContainer1.Panel2.Controls.Add(this.ScriptSavebutton);
      this.splitContainer1.Panel2.Controls.Add(this.RunScriptButton);
      this.splitContainer1.Panel2.Controls.Add(this.ScriptFileName);
      this.splitContainer1.Panel2.Controls.Add(this.StopScriptButton);
      this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
      this.splitContainer1.Size = new System.Drawing.Size(663, 403);
      this.splitContainer1.SplitterDistance = 166;
      this.splitContainer1.TabIndex = 23;
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Location = new System.Drawing.Point(397, 277);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 32;
      this.button1.Text = "QB50 HK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // BeaconButton
      // 
      this.BeaconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.BeaconButton.Location = new System.Drawing.Point(286, 277);
      this.BeaconButton.Name = "BeaconButton";
      this.BeaconButton.Size = new System.Drawing.Size(87, 23);
      this.BeaconButton.TabIndex = 27;
      this.BeaconButton.Text = "QB50 Beacon";
      this.BeaconButton.UseVisualStyleBackColor = true;
      this.BeaconButton.Click += new System.EventHandler(this.HKButton_Click);
      // 
      // seqButton
      // 
      this.seqButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.seqButton.Location = new System.Drawing.Point(286, 301);
      this.seqButton.Name = "seqButton";
      this.seqButton.Size = new System.Drawing.Size(87, 23);
      this.seqButton.TabIndex = 24;
      this.seqButton.Text = "QB50 TM";
      this.seqButton.UseVisualStyleBackColor = true;
      this.seqButton.Click += new System.EventHandler(this.seqButton_Click);
      // 
      // pictureBox2
      // 
      this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
      this.pictureBox2.Location = new System.Drawing.Point(301, 327);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(90, 72);
      this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 26;
      this.pictureBox2.TabStop = false;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(397, 325);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(87, 74);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 25;
      this.pictureBox1.TabStop = false;
      // 
      // ReadOnlyCheckBox
      // 
      this.ReadOnlyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ReadOnlyCheckBox.AutoSize = true;
      this.ReadOnlyCheckBox.Location = new System.Drawing.Point(6, 353);
      this.ReadOnlyCheckBox.Name = "ReadOnlyCheckBox";
      this.ReadOnlyCheckBox.Size = new System.Drawing.Size(76, 17);
      this.ReadOnlyCheckBox.TabIndex = 23;
      this.ReadOnlyCheckBox.Text = "Read Only";
      this.ReadOnlyCheckBox.UseVisualStyleBackColor = true;
      this.ReadOnlyCheckBox.CheckedChanged += new System.EventHandler(this.ReadOnlyCheckBox_CheckedChanged);
      // 
      // printScriptDialog
      // 
      this.printScriptDialog.UseEXDialog = true;
      // 
      // PrintScriptDocument
      // 
      this.PrintScriptDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintScriptDocument_PrintPage);
      // 
      // CompileFileDialog
      // 
      this.CompileFileDialog.DefaultExt = "seq";
      this.CompileFileDialog.Title = "Save Seq";
      this.CompileFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.CompileFile_Ok);
      // 
      // MainForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(672, 486);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.groupBox7);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "QB50 Test Program v1.1";
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.RichScriptcontextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.LogFilecontextMenuStrip.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);

        }
        #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() 
    {
      Application.EnableVisualStyles();
      Application.DoEvents();
      Application.Run(new MainForm());
    }

		private void CommandtextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
			{
				if (CommandtextBox.Text != "")
				{
					int rc = ParseCommand(CommandtextBox.Text);
          HistorylistBox.SelectedIndex = -1;
					PrintHist(CommandtextBox.Text);
				}
				e.Handled = true;
			}
		}

		public void PrintHist(string sHist)
		{
			HistorylistBox.Items.Add(sHist);
			CommandtextBox.Clear();
			HistorylistBox.TopIndex = HistorylistBox.Items.Count - 1;
			HistorylistBox.SelectedIndex = -1;
			HistorylistBox.Update();
		}

		private int ParseCommand(string p)
		{
			string sCmd = "";

			sCmd = p;

			_CmdLineScriptHost.SetScript(sCmd, _ScriptLanguage);

      _CmdLineScriptHost.AddGlobalObject("UT", utilCmds);
			_CmdLineScriptHost.AddGlobalObject("PS", psCmds);
      _CmdLineScriptHost.AddGlobalObject("HP", hpDAQCmds);
      _CmdLineScriptHost.AddGlobalObject("PG", pgCmds);
      _CmdLineScriptHost.AddGlobalObject("UC", ucCmds);
      _CmdLineScriptHost.AddGlobalObject("HV", hvadcCmds);
      _CmdLineScriptHost.AddGlobalObject("HVADC", hvadcCmds);
      _CmdLineScriptHost.AddGlobalObject("NI6225", ni6225Cmds);
      _CmdLineScriptHost.AddGlobalObject("NI9205", ni9205Cmds);
      _CmdLineScriptHost.AddGlobalObject("NI9403", ni9403Cmds);
      _CmdLineScriptHost.AddGlobalObject("NI9264", ni9264Cmds);
      _CmdLineScriptHost.AddGlobalObject("HVPS", hvpsCmds);
      _CmdLineScriptHost.AddGlobalObject("USRC", usrcCmds);
      _CmdLineScriptHost.AddGlobalObject("COM", comCmds);
      _CmdLineScriptHost.AddGlobalObject("QB50", qb50Cmds);
      _CmdLineScriptHost.AddGlobalObject("SIM", simCmds);
      _CmdLineScriptHost.AddGlobalObject("SCPI", scpiCmds);
      _CmdLineScriptHost.AddGlobalObject("TE123", teCmds);

			_CmdLineScriptHost.RunAsynchronously = false;
			_CmdLineScriptHost.Run();

			return (0);
		}

		private void CommandtextBox_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
					if (HistorylistBox.Items.Count == 0)
					{
						break;
					}

					if (HistorylistBox.SelectedIndex == -1)
					{
						HistorylistBox.SelectedIndex = HistorylistBox.Items.Count - 1;
					}
					else
					{
						if (HistorylistBox.SelectedIndex > 0)
						{
							HistorylistBox.SelectedIndex--;
						}
					}
					CommandtextBox.Text = HistorylistBox.SelectedItem.ToString();
					CommandtextBox.Select(CommandtextBox.Text.Length, 0);
					HistorylistBox.Update();
					e.Handled = true;
					break;

				case Keys.Down:
					if (HistorylistBox.Items.Count == 0)
					{
						break;
					}

					if (HistorylistBox.SelectedIndex == -1)
					{
						if (HistorylistBox.Items.Count > 0)
						{
							HistorylistBox.SelectedIndex = HistorylistBox.Items.Count - 1;
							CommandtextBox.Text = HistorylistBox.SelectedItem.ToString();
							HistorylistBox.Update();
						}
					}
					else
					{
						if (HistorylistBox.SelectedIndex < (HistorylistBox.Items.Count - 1))
						{
							HistorylistBox.SelectedIndex++;
						}
					}
					CommandtextBox.Text = HistorylistBox.SelectedItem.ToString();
					CommandtextBox.Select(CommandtextBox.Text.Length, 0);
					HistorylistBox.Update();
					e.Handled = true;
					break;
			}
		}

		public void DisplayAIData(string[] sData)
		{
			dataGridView1.Invoke((MethodInvoker)delegate
			{	
				for (int i = 0; i < 12; i++)
				{
					dataGridView1.Rows[i].Cells[1].Value = sData[i];
				}
			});
			
			TempUpdateLabel.Invoke((MethodInvoker)delegate
			{
			  TempUpdateLabel.Text = " ";
			});
		}

		private void ScriptHost_CompileError(object sender, nScriptHost.CompileErrorEventArgs e)
		{
			ScriptRunning(false);

			PrintError("Line " + e.Error.Line.ToString() + ", Col " +
				               e.Error.StartColumn.ToString() + " : " + 
								  		 e.Error.Description +
									  	 "\n");
      ScriptrichTextBox.Focus();
      ScriptrichTextBox.SelectionStart = ScriptrichTextBox.GetFirstCharIndexFromLine(e.Error.Line-1);
      ScriptrichTextBox.SelectionLength = ScriptrichTextBox.Lines[e.Error.Line - 1].Length;
		}

		void ScriptHost_ExecutionComplete(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new EventHandler(ScriptHost_ExecutionComplete), new object[] { sender, e });
				return;
			}
			ScriptRunning(false);

			PrintMsg("Script Execution Completed!\n");

      if (bCompiling)
      {
        QB50Cmds.Instance.compile.Close();
        QB50Cmds.Instance.compile = null;
        bCompiling = false;
      }
		}

		void ScriptHost_ScriptException(object sender, nScriptHost.ScriptExceptionEventArgs e)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new nScriptHost.ScriptExeceptionHandler(ScriptHost_ScriptException), new object[] { sender, e });
				return;
			}

			ScriptRunning(false);

			if (e.Exception.InnerException != null)
			{
				if (e.Exception.InnerException.Message == "Function expected\n")
				{
					string sLine = e.Exception.InnerException.StackTrace;
					string[] sLines = sLine.Split('\n');
					sLine = sLines[3];
					int nIndex = sLine.LastIndexOf("Code:line");
					sLine = sLine.Substring(nIndex + 10);
					sLine = sLine.Trim();

					PrintError("Invalid Function at Script Line " + sLine + ".\n");
				}
				else
				{
					if (e.Exception.InnerException.Message == "Thread was being aborted.\n")
					{
						PrintWarning("User Break!  Script Stopped.\n");
					}
				}
			}
			else
			{
				PrintError(e.Exception.Message);
			}
		}

		private void RunScriptButton_Click(object sender, EventArgs e)
		{
			logFile.WriteLog("script: '" + ScriptFileName.Text + "'");
			ScriptRunning(true);
			_ScriptHost.SetScript(ScriptrichTextBox.Text, _ScriptLanguage);

      _ScriptHost.AddGlobalObject("UT", utilCmds);
			_ScriptHost.AddGlobalObject("PS", psCmds);
      _ScriptHost.AddGlobalObject("HP", hpDAQCmds);
      _ScriptHost.AddGlobalObject("PG", pgCmds);
      _ScriptHost.AddGlobalObject("UC", ucCmds);
      _ScriptHost.AddGlobalObject("HV", hvadcCmds);
      _ScriptHost.AddGlobalObject("HVADC", hvadcCmds);
      _ScriptHost.AddGlobalObject("NI6225", ni6225Cmds);
      _ScriptHost.AddGlobalObject("NI9205", ni9205Cmds);
      _ScriptHost.AddGlobalObject("NI9403", ni9403Cmds);
      _ScriptHost.AddGlobalObject("NI9264", ni9264Cmds);
      _ScriptHost.AddGlobalObject("HVPS", hvpsCmds);
      _ScriptHost.AddGlobalObject("USRC", usrcCmds);
      _ScriptHost.AddGlobalObject("COM", comCmds);
      _ScriptHost.AddGlobalObject("QB50", qb50Cmds);
      _ScriptHost.AddGlobalObject("SIM", simCmds);
      _ScriptHost.AddGlobalObject("SCPI", scpiCmds);
      _ScriptHost.AddGlobalObject("TE123", teCmds);

			_ScriptHost.RunAsynchronously = true;
			_ScriptHost.Run();
		}

		public void PrintMsg(string Msg)
		{
			MsgRichTextBox.Invoke((MethodInvoker)delegate
			{
        MsgRichTextBox.DeselectAll();
        MsgRichTextBox.SelectionStart = MsgRichTextBox.Text.Length;
        MsgRichTextBox.ScrollToCaret();
				MsgRichTextBox.SelectionColor = Color.Black;
				MsgRichTextBox.SelectedText = Msg;
				MsgRichTextBox.SelectionLength = 0;
				MsgRichTextBox.ScrollToCaret();
				logFile.WriteLog("msg: " + Msg);
			});
		}

		public void PrintError(string Msg)
		{
			MsgRichTextBox.Invoke((MethodInvoker)delegate
			{
        MsgRichTextBox.DeselectAll();
        MsgRichTextBox.SelectionStart = MsgRichTextBox.Text.Length;
        MsgRichTextBox.ScrollToCaret();
				MsgRichTextBox.SelectionColor = Color.Red;
				MsgRichTextBox.SelectedText = Msg;
				MsgRichTextBox.SelectionStart = MsgRichTextBox.Text.Length;
				MsgRichTextBox.SelectionLength = 0;
				MsgRichTextBox.ScrollToCaret();
				logFile.WriteLog("error: " + Msg);
			});
		}

		public void PrintWarning(string Msg)
		{
			MsgRichTextBox.Invoke((MethodInvoker)delegate
			{
        MsgRichTextBox.DeselectAll();
        MsgRichTextBox.SelectionStart = MsgRichTextBox.Text.Length;
        MsgRichTextBox.ScrollToCaret();
				MsgRichTextBox.SelectionColor = Color.Goldenrod;
				MsgRichTextBox.SelectedText = Msg;
				MsgRichTextBox.SelectionStart = MsgRichTextBox.Text.Length;
				MsgRichTextBox.SelectionLength = 0;
				MsgRichTextBox.ScrollToCaret();
				logFile.WriteLog("warning: " + Msg);
			});
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			this.Hide();
			// Set window size to last size
			if (Properties.Settings.Default.WindowSize != null)
			{
				this.Size = Properties.Settings.Default.WindowSize;
				splitContainer1.SplitterDistance = Properties.Settings.Default.SplitterDist;
				this.CenterToScreen();
			}
			this.Show();

			logFile = LogFile.Instance;

			InitHardware();

			LogDirectoryButton.Text = Properties.Settings.Default.LogDir;
			SetLogDirButtonColor();
			logFile.SetDirectory(LogDirectoryButton.Text);

			CleartoolStripMenuItem_Click(null, null); //Init Script Box as blank

			this.ActiveControl = CommandtextBox;

			SetInitialControls();
		}

		private void SetLogDirButtonColor()
		{
			if ((LogDirectoryButton.Text == "") || (LogFileButton.Text == ""))
			{
				LogDirectoryButton.BackColor = Color.Yellow;
				LogFileButton.BackColor = Color.Yellow;
			}
			else
			{
				LogDirectoryButton.BackColor = Color.LightGreen;
				LogFileButton.BackColor = Color.LightGreen;
			}
		}

		private void InitHardware()
		{
			//Init the hardware
		}

		private void SetInitialControls()
		{
		}

		private void StopScriptButton_Click(object sender, EventArgs e)
		{
      bScriptStop = true;

      System.Threading.Thread.Sleep(1000);
			_ScriptHost.BreakScript();
			ScriptRunning(false);

      bScriptStop = false;
		}

		private void ScriptRunning(bool bRunning)
		{
			if (bRunning)
			{
				RunScriptButton.Enabled = false;
				StopScriptButton.Enabled = true;
				CommandtextBox.Enabled = false;
				ScriptrichTextBox.ReadOnly = true;
				LoadScriptButton.Enabled = false;
				EnableSaveButton(false);
				UserBreakToggle.Checked = false;
				UserBreakToggle.Enabled = true;
			}
			else
			{
				RunScriptButton.Enabled = true;
				StopScriptButton.Enabled = false;
				CommandtextBox.Enabled = true;
				ScriptrichTextBox.ReadOnly = ReadOnlyCheckBox.Checked;
				LoadScriptButton.Enabled = true;
				EnableSaveButton(true);
				UserBreakToggle.Checked = false;
				UserBreakToggle.Enabled = false;
			}
		}

		private void LoadScriptButton_Click(object sender, EventArgs e)
		{
			if (bScriptModified)
			{
				CheckforSave();
			}

			if (Properties.Settings.Default.ScriptDir != "")
			{
				ScriptopenFileDialog.InitialDirectory = Properties.Settings.Default.ScriptDir;
			}

			ScriptopenFileDialog.ShowDialog();
		}

		private void ScriptopenFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			string fname = ScriptopenFileDialog.FileName;
			Properties.Settings.Default.ScriptDir = System.IO.Path.GetDirectoryName(fname);
			string strScript = System.IO.File.ReadAllText(fname);

			ScriptrichTextBox.Clear();
			bScriptModified = false;
			bScriptNew = true;
			ScriptrichTextBox.Text = strScript;
			ReadOnlyCheckBox.Checked = true;

			ScriptFileName.Text = fname;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (bScriptModified)
			{
				CheckforSave();
			}

			Properties.Settings.Default.WindowSize = this.Size;
			Properties.Settings.Default.SplitterDist = splitContainer1.SplitterDistance;

			Properties.Settings.Default.Save();

			logFile.Dispose();
		}

		private void ScriptSavebutton_Click(object sender, EventArgs e)
		{
			if (ScriptFileName.Text.Trim() == "")
			{
				ScriptsaveFileDialog.InitialDirectory = ScriptopenFileDialog.InitialDirectory;
				ScriptsaveFileDialog.FileName = "";
			}
			else
			{
				ScriptsaveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(ScriptFileName.Text);
				ScriptsaveFileDialog.FileName = System.IO.Path.GetFileName(ScriptFileName.Text);
			}

			ScriptsaveFileDialog.ShowDialog();
		}

		private void ScriptsaveFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			string fname = ScriptsaveFileDialog.FileName;

			System.IO.File.WriteAllText(fname, ScriptrichTextBox.Text);
			PrintMsg("Saving " + fname + "\n");

			Properties.Settings.Default.ScriptDir = System.IO.Path.GetDirectoryName(fname);
			ScriptFileName.Text = fname;

			bScriptModified = false;
			EnableSaveButton(false);
		}

		private void LogDirectoryButton_Click(object sender, EventArgs e)
		{
			LogFolderBrowserDialog.ShowNewFolderButton = true;
			LogFolderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			LogFolderBrowserDialog.SelectedPath = LogDirectoryButton.Text;

			if (LogFolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				LogDirectoryButton.Text = LogFolderBrowserDialog.SelectedPath;
				Properties.Settings.Default.LogDir = LogDirectoryButton.Text;
				SetLogDirButtonColor();
				logFile.SetDirectory(LogDirectoryButton.Text);
			}
		}

		public void SetLogFileName(string sLogFileName)
		{
			LogFileButton.Invoke((MethodInvoker)delegate
			{
				LogFileButton.Text = sLogFileName;
				SetLogDirButtonColor();
			});
		}

		private void LogFileButton_Click(object sender, EventArgs e)
		{
			logFile.OpenNewFile();
		}

		private void ReadOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			ScriptrichTextBox.ReadOnly = ReadOnlyCheckBox.Checked;
			ReadOnlytoolStripMenuItem.Checked = ReadOnlyCheckBox.Checked;
		}

		private void ReadOnlytoolStripMenuItem_Click(object sender, EventArgs e)
		{
			ReadOnlyCheckBox.Checked = ReadOnlytoolStripMenuItem.Checked;
		}

		private void SavetoolStripMenuItem_Click(object sender, EventArgs e)
		{
			ScriptSavebutton_Click(null, null);
		}

		private void CleartoolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (bScriptModified)
			{
				CheckforSave();
			}

			ScriptrichTextBox.Clear();
			bScriptModified = false;
			bScriptNew = true;
			EnableSaveButton(false);
			ScriptrichTextBox.ReadOnly = false;
			ReadOnlyCheckBox.Checked = false;
			ScriptFileName.Text = 
			System.IO.Path.Combine(Properties.Settings.Default.ScriptDir,
				                     "NewScript.txt");
		}

		private void CheckforSave()
		{
			if (MessageBox.Show("Save current script?",
													"Save Script",
													MessageBoxButtons.YesNo,
													MessageBoxIcon.Question,
													MessageBoxDefaultButton.Button1) == DialogResult.Yes)
			{
				ScriptSavebutton_Click(null, null);
			}			
		}

		private void ScriptrichTextBox_TextChanged(object sender, EventArgs e)
		{
			if (bScriptNew)
			{
				bScriptModified = false;
				bScriptNew = false;
				EnableSaveButton(false);
			}
			else
			{
				bScriptModified = true;
				EnableSaveButton(true);
			}
		}

		private void EnableSaveButton(bool bEnable)
		{
			if (bEnable == false)
			{
				ScriptSavebutton.Enabled = false;
			}
			else
			{
				if (bScriptModified)
				{
					ScriptSavebutton.Enabled = true;
				}
				else
				{
					ScriptSavebutton.Enabled = false;
				}
			}
		}

		private void UserBreakToggle_CheckedChanged(object sender, EventArgs e)
		{
			UserBreakToggle.Invoke((MethodInvoker)delegate
			{
				if (UserBreakToggle.Checked)
				{
					SetUserBreak();
				}
			});
		}

		public bool CheckUserBreak()
		{
			lock (_UserBreakLock)
			{
				if (bUserBreak)
				{
					bUserBreak = false;
					ResetUserBreak();
					return (true);
				}
				else
				{
					return (false);
				}
			}
		}

		private void SetUserBreak()
		{
			lock (_UserBreakLock)
			{
				bUserBreak = true;
			}
		}

		private void ResetUserBreak()
		{
			UserBreakToggle.Invoke((MethodInvoker)delegate
			{
				UserBreakToggle.Checked = false;
			});
		}

		private void PrintStripMenuItem_Click(object sender, EventArgs e)
		{
			PrintScriptDocument.DefaultPageSettings.Margins.Left   = 50;  //0.5 inches
			PrintScriptDocument.DefaultPageSettings.Margins.Right  = 50;
			PrintScriptDocument.DefaultPageSettings.Margins.Top    = 50;
			PrintScriptDocument.DefaultPageSettings.Margins.Bottom = 50;

			printScriptDialog.Document = PrintScriptDocument;
			string strText = this.ScriptrichTextBox.Text;
			ScriptStringReader = new System.IO.StringReader(strText);
 
			if (printScriptDialog.ShowDialog() == DialogResult.OK)
			{
				this.PrintScriptDocument.Print();
			}
 
		}

		private void PrintScriptDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			float linesPerPage = 0;
			float yPosition = 0;
			int count = 0;
			float leftMargin = e.MarginBounds.Left;
			float topMargin = e.MarginBounds.Top;
			string line = null;
			Font printFont = this.ScriptrichTextBox.Font;
			SolidBrush myBrush = new SolidBrush(Color.Black);
			// Work out the number of lines per page, using the MarginBounds.
			linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics);
			// Iterate over the string using the StringReader, printing each line.
			while (count < linesPerPage && ((line = this.ScriptStringReader.ReadLine()) != null))
			{
				// calculate the next line position based on the height of the font according to the printing device
				yPosition = topMargin + (count * printFont.GetHeight(e.Graphics));
				// draw the next line in the rich edit control
				e.Graphics.DrawString(line, printFont, myBrush, leftMargin, yPosition, new StringFormat());
				count++;
			}
			// If there are more lines, print another page.
			if (line != null)
				e.HasMorePages = true;
			else
				e.HasMorePages = false;
			myBrush.Dispose();
		}

		private void ViewLogFileMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog logfileOpenDialog = new OpenFileDialog();

			logfileOpenDialog.Multiselect = false;
			logfileOpenDialog.Title = "Open a Log File";
			if (LogDirectoryButton.Text == "")
			{
				logfileOpenDialog.InitialDirectory = "C:\\";
				if (logfileOpenDialog.ShowDialog() == DialogResult.OK)
				{
					System.Diagnostics.Process.Start("c:\\windows\\notepad.exe",logfileOpenDialog.FileName);
				}
			}
			else
			{
				logfileOpenDialog.InitialDirectory = LogDirectoryButton.Text;
				if (logfileOpenDialog.ShowDialog() == DialogResult.OK)
				{
					System.Diagnostics.Process.Start("c:\\windows\\notepad.exe",logfileOpenDialog.FileName);
				}
			}
		}

    public void UpdatePS(System.Collections.Generic.List<string[]> lValues)
    {
      this.InvokeIfNeeded(
        (MethodInvoker)delegate()
        {
          if (dataGridView1.Rows.Count != 0)
          {
            for (int nI = 0; nI < lValues.Count; nI++)
            {
              dataGridView1.Rows[nI].SetValues(lValues[nI]);
            }
            logFile.WriteVoltsToLog(lValues);
          }
        });
    }

    private void InvokeIfNeeded(MethodInvoker method)
    {
      // Only invoke on different thread if needed
      if (this.InvokeRequired)
      {
        this.BeginInvoke(method);
      }
      else
      {
        method();
      }
    }

    private void seqButton_Click(object sender, EventArgs e)
    {
      seqForm.Show();
      seqForm.BringToFront();
    }

    private void HistorylistBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (HistorylistBox.SelectedItem != null)
      {
        CommandtextBox.Text = HistorylistBox.SelectedItem.ToString();
        CommandtextBox.Focus();
        CommandtextBox.SelectionStart = CommandtextBox.TextLength;
        CommandtextBox.ScrollToCaret();
      }
    }

    private void reset_font()
    {
      ScriptrichTextBox.SelectAll();
      ScriptrichTextBox.SelectionFont = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      ScriptrichTextBox.SelectionColor = Color.Black;
      ScriptrichTextBox.DeselectAll();
      ScriptrichTextBox.SelectionStart = ScriptrichTextBox.TextLength;
      ScriptrichTextBox.ScrollToCaret();
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScriptrichTextBox.Undo();
    }

    private void ScriptrichTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.V)
      {
        ScriptrichTextBox.Paste(DataFormats.GetFormat("Text"));
        e.Handled = true;
      }
    }

    private void HKButton_Click(object sender, EventArgs e)
    {
      beaconForm.Show();
      beaconForm.BringToFront();
    }
     
    private void Compilebutton_Click(object sender, EventArgs e)
    {
      string CompileFile = ScriptFileName.Text.Replace(".txt", ".seq");
      QB50Cmds.Instance.compile = new FileStream(CompileFile, FileMode.Create);
      bCompiling = true;

      //logFile.WriteLog("script: '" + ScriptFileName.Text + "'");
      ScriptRunning(true);
      _ScriptHost.SetScript(ScriptrichTextBox.Text, _ScriptLanguage);

      _ScriptHost.AddGlobalObject("SEB", qb50Cmds);

      _ScriptHost.RunAsynchronously = true;

      _ScriptHost.Run();
    }

    private void CompileFile_Ok(object sender, CancelEventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
      hkForm.Show();
      hkForm.BringToFront();
    }

    private byte MakeByteFromASCII(byte sDigit1, byte sDigit2)
    {
      String sASCII = ((char)sDigit1).ToString() + ((char)sDigit2).ToString();

      return (Convert.ToByte(sASCII, 16));
    }
	}
}
