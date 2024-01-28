// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.ServerModem
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.DataCallPackets;
using Nal.EncryptionModule;
using Nal.ServerForTrackers.Common;
using Nal.Sms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class ServerModem : CommLink
  {
    private SerialPort serialPort;
    private bool autoAnswer;
    private CallProtocol callProtocol;
    private PacketDataCallHandler packetDataCallHandler;
    private DataSerializer dataSerializer;
    private CharProcessor smsCharProcessor;
    private bool manageConnection;
    private Timer manageConnectionTimer;
    private Timer pauseBetweenSignalChecksTimer;
    private string port;
    private int portBitsPerSecond;
    private int portDataBits;
    private Parity portParity;
    private StopBits portStopBits;
    private bool receiveSms;
    private int signalChecks;
    private CallProtocol currentCallProtocol;
    private string peerNumber;
    private bool initializingAfterDisconnect;
    private int ats0Attempts;
    private int cnmiAttempts;
    private bool handleConnectionStartedCalled;
    private bool isInitiator;
    private List<byte> receivedCallData;
    private DateTime startOfConnection;

    public ServerModem()
    {
      this.callProtocol = CallProtocol.Packets;
      this.ats0Attempts = 0;
      this.cnmiAttempts = 0;
      this.initializingAfterDisconnect = false;
      this.handleConnectionStartedCalled = false;
      this.isInitiator = false;
      this.Status = "Port Closed";
      this.receivedCallData = new List<byte>();
      this.serialPort = new SerialPort();
      this.serialPort.ReceivedBytesThreshold = 1;
      this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.OnSerialPortDataReceived);
      this.serialPort.PinChanged += new SerialPinChangedEventHandler(this.OnSerialPortPinChanged);
      this.packetDataCallHandler = new PacketDataCallHandler((ISynchronizeInvoke) ServerForTrackersService.HiddenForm);
      this.packetDataCallHandler.Send = new PacketDataCallHandler.SendDelegate(this.Send);
      this.packetDataCallHandler.Disconnect = new PacketDataCallHandler.DisconnectDelegate(this.Disconnect);
      this.packetDataCallHandler.DataReceived += new PacketDataCallHandler.DataEvent(this.OnPacketDataCallHandlerDataReceived);
      this.dataSerializer = new DataSerializer(new DataSerializer.SendDataDelegate(this.Send));
      this.dataSerializer.TransactionCompleted += new DataSerializer.TransactionCompletedEventHandler(this.OnDataSerializerTransactionCompleted);
      this.smsCharProcessor = new CharProcessor();
      this.smsCharProcessor.LookupFinished += new CharProcessor.LookupFinishedEventHandler(this.OnSmsCharProcessorLookupFinished);
      this.manageConnectionTimer = new Timer();
      this.manageConnectionTimer.Interval = 900000;
      this.manageConnectionTimer.Tick += new EventHandler(this.OnManageCopsTimerTick);
      this.pauseBetweenSignalChecksTimer = new Timer();
      this.pauseBetweenSignalChecksTimer.Interval = 10000;
      this.pauseBetweenSignalChecksTimer.Tick += new EventHandler(this.OnPauseBetweenSignalChecksTimerTick);
    }

    public string Port
    {
      get => this.port;
      set
      {
        this.port = value;
        this.Name = "Server Modem (" + this.port + ")";
      }
    }

    public int PortBitsPerSecond
    {
      get => this.portBitsPerSecond;
      set => this.portBitsPerSecond = value;
    }

    public StopBits PortStopBits
    {
      get => this.portStopBits;
      set => this.portStopBits = value;
    }

    public Parity PortParity
    {
      get => this.portParity;
      set => this.portParity = value;
    }

    public int PortDataBits
    {
      get => this.portDataBits;
      set => this.portDataBits = value;
    }

    public bool ReceiveSms
    {
      get => this.receiveSms;
      set => this.receiveSms = value;
    }

    public bool ManageConn
    {
      get => this.manageConnection;
      set
      {
        if (this.manageConnection == value)
          return;
        this.manageConnection = value;
        if (this.manageConnection)
        {
          if (!this.serialPort.IsOpen)
            return;
          this.manageConnectionTimer.Start();
        }
        else
        {
          this.manageConnectionTimer.Stop();
          this.pauseBetweenSignalChecksTimer.Stop();
        }
      }
    }

    public bool AutoAnswer
    {
      get => this.autoAnswer;
      set => this.autoAnswer = value;
    }

    public CallProtocol CallProtocol
    {
      get => this.callProtocol;
      set => this.callProtocol = value;
    }

    public bool PortOpened => this.serialPort.IsOpen;

    public void AbortDialing()
    {
      if (!this.serialPort.IsOpen)
        throw new InvalidOperationException("The port is not open.");
      if (!this.dataSerializer.TransactionInProgress() || !(this.dataSerializer.GetInProcessTransactionID() == "Dialing"))
        return;
      this.serialPort.Write("x");
    }

    public void Close()
    {
      if (this.manageConnection)
      {
        this.manageConnectionTimer.Stop();
        this.pauseBetweenSignalChecksTimer.Stop();
      }
      this.serialPort.Close();
      this.serialPort.DtrEnable = false;
      this.serialPort.RtsEnable = false;
      this.Status = "Port Closed";
      this.StopLookingForSmsMessage();
      this.dataSerializer.CancelTransactions();
    }

    public void Connect(string phoneNumber)
    {
      if (!this.serialPort.IsOpen)
        throw new InvalidOperationException("The port is not open.");
      if (this.dataSerializer.TransactionInProgress())
        throw new InvalidOperationException("The modem is busy.");
      if (this.serialPort.CDHolding)
        throw new InvalidOperationException("Already connected.");
      this.peerNumber = phoneNumber;
      this.SendCbst();
    }

    public void Disconnect()
    {
      if (!this.serialPort.IsOpen)
        throw new InvalidOperationException("The port is not open.");
      if (!this.serialPort.CDHolding)
        return;
      if (this.dataSerializer.TransactionInProgress())
        throw new InvalidOperationException("The modem is busy.");
      this.SendPpp();
    }

    public void Open()
    {
      this.serialPort.PortName = this.port;
      this.serialPort.BaudRate = this.portBitsPerSecond;
      this.serialPort.Parity = this.portParity;
      this.serialPort.DataBits = this.portDataBits;
      this.serialPort.StopBits = this.portStopBits;
      this.serialPort.Open();
      this.serialPort.DtrEnable = true;
      this.serialPort.RtsEnable = true;
      this.Status = "Port Open";
      this.handleConnectionStartedCalled = false;
      this.initializingAfterDisconnect = false;
      if (this.serialPort.CDHolding)
        this.Disconnect();
      else if (this.serialPort.CtsHolding)
      {
        this.StartLookingForSmsMessageStart();
        this.SendAts0();
      }
      if (!this.manageConnection)
        return;
      this.manageConnectionTimer.Start();
    }

    public void Send(string data)
    {
      if (!this.serialPort.IsOpen)
        throw new InvalidOperationException("The port is not open.");
      if (!this.serialPort.CDHolding)
        throw new InvalidOperationException("There is no connection.");
      byte[] bytes = Encoding.GetEncoding(1252).GetBytes(data);
      if (this.currentCallProtocol == CallProtocol.Packets)
      {
        this.packetDataCallHandler.QueueToSend(bytes);
      }
      else
      {
        if (this.currentCallProtocol != CallProtocol.None)
          return;
        this.Send(bytes);
      }
    }

    private void HandleDataReceivedOutOfConnection(byte[] data)
    {
      string str = Encoding.GetEncoding(1252).GetString(data);
      this.dataSerializer.ProcessReceivedData(str);
      this.smsCharProcessor.HandleReceivedData(str);
      this.Activity = Nal.ServerForTrackers.Common.Utils.FormatForDisplay(str);
    }

    private void HandleDataReceivedDuringConnection(byte[] data)
    {
      this.dataSerializer.ProcessReceivedData(Encoding.GetEncoding(1252).GetString(data));
      if (!this.handleConnectionStartedCalled)
        return;
      if (this.currentCallProtocol == CallProtocol.Packets)
      {
        this.packetDataCallHandler.HandleReceivedData(data);
      }
      else
      {
        if (this.currentCallProtocol != CallProtocol.None)
          return;
        this.HandleUnpackedData(data);
      }
    }

    private void HandleCdWentHigh()
    {
      this.handleConnectionStartedCalled = true;
      this.StopLookingForSmsMessage();
      this.dataSerializer.CancelTransactions();
      this.currentCallProtocol = this.callProtocol;
      this.receivedCallData.Clear();
      this.startOfConnection = DateTime.UtcNow;
      if (!this.serialPort.CDHolding)
        return;
      this.Status = "Connected";
      if (this.currentCallProtocol != CallProtocol.Packets)
        return;
      this.packetDataCallHandler.BeginHandshaking(this.isInitiator, CommLink.UseEncryption ? CommLink.EncryptionUser : (EncryptionUser) null);
    }

    private void HandleCdWentLow()
    {
      if (this.handleConnectionStartedCalled && this.currentCallProtocol == CallProtocol.Packets)
        this.packetDataCallHandler.HandleDisconnect();
      this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(DataType.Other), ModemInfoTypes.Unknown, string.Empty, Protocol.Call, this.startOfConnection, (IList<byte>) this.receivedCallData));
      this.handleConnectionStartedCalled = false;
      this.initializingAfterDisconnect = true;
      this.isInitiator = false;
      this.StartLookingForSmsMessageStart();
      this.SendAts0();
    }

    private void HandleCtsWentHigh()
    {
      if (this.serialPort.CDHolding)
        return;
      this.StartLookingForSmsMessageStart();
      this.SendAts0();
    }

    private void HandleCtsWentLow()
    {
      this.Status = "Modem Offline";
      this.StopLookingForSmsMessage();
      this.dataSerializer.CancelTransactions();
    }

    private void OnDataSerializerTransactionCompleted(
      object sender,
      DataSerializer.TransactionCompletedEventArgs e)
    {
      this.Status = this.serialPort.IsOpen ? "Port Open" : "Port Closed";
      string transactionId = e.TransactionID;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(transactionId))
      {
        case 658687313:
          if (!(transactionId == "Escaping") || !this.serialPort.CDHolding)
            break;
          this.SendAth();
          break;
        case 774171580:
          if (!(transactionId == "Checking Signal Strength"))
            break;
          if ((e.Result == -1 || e.Result == 6 ? 0 : e.Result) >= 2 || this.serialPort.CDHolding)
          {
            this.manageConnectionTimer.Start();
            break;
          }
          if (++this.signalChecks < 3)
          {
            this.pauseBetweenSignalChecksTimer.Start();
            break;
          }
          this.SendCops();
          break;
        case 1214153010:
          if (!(transactionId == "Wait Before CNMI"))
            break;
          this.SendCnmi();
          break;
        case 2430810060:
          if (!(transactionId == "Initializing"))
            break;
          ++this.ats0Attempts;
          if (e.Result == -1)
          {
            if (this.initializingAfterDisconnect || this.ats0Attempts < 10)
            {
              this.SendAts0();
            }
            else
            {
              this.Activity = "Notice: Unable to initialize the server modem.\r\n";
              this.ats0Attempts = 0;
            }
          }
          else
            this.ats0Attempts = 0;
          if (this.ats0Attempts != 0 || !this.receiveSms)
            break;
          this.SendCnmi();
          break;
        case 3421077119:
          if (!(transactionId == "Selecting Bearer Service") || e.Result != 0)
            break;
          this.SendAtd();
          break;
        case 3494671189:
          if (!(transactionId == "Dialing") || e.Result == 0)
            break;
          this.isInitiator = false;
          break;
        case 3606364672:
          if (!(transactionId == "Hanging Up") || !this.serialPort.CDHolding)
            break;
          this.SendPpp();
          break;
        case 4076461381:
          if (!(transactionId == "Setting SMS Indications"))
            break;
          ++this.cnmiAttempts;
          bool flag = false;
          if ((e.Result == 1 || e.Result == 2) && !this.initializingAfterDisconnect)
            flag = true;
          if (e.Result != 0)
          {
            if (this.receiveSms && (this.initializingAfterDisconnect || this.cnmiAttempts < 5))
            {
              if (flag)
              {
                this.WaitBeforeCnmi();
                break;
              }
              this.SendCnmi();
              break;
            }
            this.cnmiAttempts = 0;
            this.Activity = "Warning: Unable to set the SMS indications.\r\nPlease check that a valid SIM card is inserted and that the SMS storage memory is not full.\r\n";
            break;
          }
          this.cnmiAttempts = 0;
          break;
        case 4203872657:
          if (!(transactionId == "Selecting Operator"))
            break;
          this.manageConnectionTimer.Start();
          break;
      }
    }

    private void OnManageCopsTimerTick(object sender, EventArgs e)
    {
      if (!this.serialPort.IsOpen || this.serialPort.CDHolding || !this.serialPort.CtsHolding)
        return;
      this.manageConnectionTimer.Stop();
      this.signalChecks = 0;
      this.SendCsq();
    }

    private void OnPauseBetweenSignalChecksTimerTick(object sender, EventArgs e)
    {
      this.pauseBetweenSignalChecksTimer.Stop();
      if (this.serialPort.IsOpen && !this.serialPort.CDHolding && this.serialPort.CtsHolding)
        this.SendCsq();
      else
        this.manageConnectionTimer.Start();
    }

    private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      int bytesToRead = this.serialPort.BytesToRead;
      bool cdHolding = this.serialPort.CDHolding;
      if (bytesToRead <= 0)
        return;
      byte[] buffer = new byte[bytesToRead];
      this.serialPort.Read(buffer, 0, buffer.Length);
      if (cdHolding)
        ServerForTrackersService.HiddenForm.BeginInvoke((Delegate) new ServerModem.HandleDataReceivedDelegate(this.HandleDataReceivedDuringConnection), (object) buffer);
      else
        ServerForTrackersService.HiddenForm.BeginInvoke((Delegate) new ServerModem.HandleDataReceivedDelegate(this.HandleDataReceivedOutOfConnection), (object) buffer);
    }

    private void OnSerialPortPinChanged(object sender, SerialPinChangedEventArgs e)
    {
      if (e.EventType == SerialPinChange.CDChanged)
      {
        if (this.serialPort.CDHolding)
          ServerForTrackersService.HiddenForm.BeginInvoke((Delegate) new ServerModem.HandlePinChangeDelegate(this.HandleCdWentHigh));
        else
          ServerForTrackersService.HiddenForm.BeginInvoke((Delegate) new ServerModem.HandlePinChangeDelegate(this.HandleCdWentLow));
      }
      else
      {
        if (e.EventType != SerialPinChange.CtsChanged)
          return;
        if (this.serialPort.CtsHolding)
          ServerForTrackersService.HiddenForm.BeginInvoke((Delegate) new ServerModem.HandlePinChangeDelegate(this.HandleCtsWentHigh));
        else
          ServerForTrackersService.HiddenForm.BeginInvoke((Delegate) new ServerModem.HandlePinChangeDelegate(this.HandleCtsWentLow));
      }
    }

    private void OnSmsCharProcessorLookupFinished(
      object sender,
      CharProcessor.LookupFinishedEventArgs e)
    {
      switch (this.smsCharProcessor.LookupID)
      {
        case "SMS Message Start":
          this.StartLookingForSmsMessagePdu();
          break;
        case "SMS Message PDU":
          if (e.Result == 0)
          {
            this.StartLookingForSmsMessageEnd();
            break;
          }
          this.Status = "Idle";
          this.StartLookingForSmsMessageStart();
          break;
        case "SMS Message End":
          this.Status = "Idle";
          if (e.Result == 0)
          {
            try
            {
              MTSmsMessage mtSmsMessage = MTSmsMessage.Parse(e.LogStr);
              string sender1 = mtSmsMessage.OriginatorAddress;
              if (sender1.Length > 2)
                sender1 = sender1.Remove(0, 2);
              this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(), ModemInfoTypes.PhoneNumber, sender1, Protocol.Sms, DateTime.UtcNow, mtSmsMessage.UserData));
            }
            catch
            {
            }
          }
          this.StartLookingForSmsMessageStart();
          break;
      }
    }

    private void OnPacketDataCallHandlerDataReceived(
      object sender,
      PacketDataCallHandler.DataEventArgs e)
    {
      this.HandleUnpackedData(e.Data);
    }

    private void HandleUnpackedData(byte[] data)
    {
      this.receivedCallData.AddRange((IEnumerable<byte>) data);
      this.Activity = Nal.ServerForTrackers.Common.Utils.FormatForDisplay(Encoding.GetEncoding(1252).GetString(data));
    }

    private void Send(byte[] data) => this.serialPort.Write(data, 0, data.Length);

    private void SendCsq()
    {
      this.Status = "Checking Signal Strength";
      this.dataSerializer.RequestTransaction("AT+CSQ\r", "+CSQ:0\r\n|+CSQ:1\r\n|+CSQ:2\r\n|+CSQ:3\r\n|+CSQ:4\r\n|+CSQ:5\r\n|ERROR\r\n", 60000, "Checking Signal Strength");
    }

    private void SendCops()
    {
      this.Status = "Selecting Operator";
      this.dataSerializer.RequestTransaction("AT+COPS=1\r", "OK\r\n|ERROR\r\n", 2000, "Selecting Operator");
    }

    private void SendAts0()
    {
      this.Status = "Initializing...";
      this.dataSerializer.RequestTransaction("ATS0=" + (this.autoAnswer ? "1" : "0") + "V1E1\r", "OK\r\n", 2000, "Initializing");
    }

    private void SendCnmi()
    {
      this.Status = "Setting SMS Indications...";
      this.dataSerializer.RequestTransaction("AT+CNMI=2,2\r", "OK\r\n|+CMS ERROR:|ERROR\r\n", 15000, "Setting SMS Indications");
    }

    private void SendCbst()
    {
      this.Status = "Selecting Bearer Service...";
      this.dataSerializer.RequestTransaction("AT+CBST=71,0,1\r", "OK\r\n|ERROR\r\n", 2000, "Selecting Bearer Service");
    }

    private void SendAtd()
    {
      this.isInitiator = true;
      this.Status = "Dialing...";
      this.dataSerializer.RequestTransaction("ATD" + this.peerNumber + "\r", "CONNECT|NO CARRIER\r\n|BUSY\r\n|NO ANSWER\r\n|OK\r\n|ERROR\r\n", 120000, "Dialing");
    }

    private void SendPpp()
    {
      this.Status = "Escaping...";
      this.dataSerializer.RequestTransaction("+++", "\r\nOK\r\n", 2000, "Escaping");
    }

    private void SendAth()
    {
      this.Status = "Hanging Up...";
      this.dataSerializer.RequestTransaction("ATH\r", "OK\r\n", 10000, "Hanging Up");
    }

    private void StartLookingForSmsMessageStart()
    {
      this.smsCharProcessor.LookUp("+CMT:", 0, false, "SMS Message Start");
    }

    private void StartLookingForSmsMessagePdu()
    {
      this.Status = "Receiving SMS Message...";
      this.smsCharProcessor.LookUp("\r\n", 2000, false, "SMS Message PDU");
    }

    private void StartLookingForSmsMessageEnd()
    {
      this.Status = "Receiving SMS Message...";
      this.smsCharProcessor.LookUp("\r\n", 5000, true, "SMS Message End");
    }

    private void StopLookingForSmsMessage() => this.smsCharProcessor.CancelLookUp();

    private void WaitBeforeCnmi()
    {
      this.Status = "Setting SMS Indications...";
      this.dataSerializer.RequestTransaction(string.Empty, "ldfv4v5", 5000, "Wait Before CNMI");
    }

    private delegate void HandleDataReceivedDelegate(byte[] data);

    private delegate void HandlePinChangeDelegate();
  }
}
