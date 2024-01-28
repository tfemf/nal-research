// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.EmailAccount
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.ServerForTrackers.Common;
using Nal.Sms;
using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Timers;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class EmailAccount : CommLink
  {
    private BackgroundWorker connectWorker;
    private BackgroundWorker authenticateWorker;
    private BackgroundWorker getMessageCountWorker;
    private BackgroundWorker getMessageSizeWorker;
    private BackgroundWorker getMessageUidWorker;
    private BackgroundWorker getMessageHeadersWorker;
    private BackgroundWorker getMessageWorker;
    private BackgroundWorker deleteWorker;
    private BackgroundWorker disconnectWorker;
    private Pop3Client pop3Client;
    private SmtpClient smtpClient;
    private System.Timers.Timer autoRetrieveTimer;
    private List<string> retrievedUids;
    private string retrievedUidsFileName;
    private int numOfSbdEmails;
    private int numOfSmsEmails;
    private int numOfFilteredOutEmails;
    private int numOfAlreadyRtrvdEmails;
    private string displayName;
    private bool autoRetrieve;
    private int autoRetrieveFreq;
    private string smtpStatus;
    private string pop3Status;
    private bool currMsgAlreadyRtrvd;
    private Protocol? protocol;
    private EmailAccount.SmsNetwork smsNetwork;
    private string modemInfo;
    private bool connecting;
    private bool abortRequested;
    private int messageCount;
    private int messageNumber;
    private string uid;
    private int size;
    private bool wasLastSendSuccessful;

    public EmailAccount()
    {
      this.connectWorker = new BackgroundWorker();
      this.connectWorker.DoWork += new DoWorkEventHandler(this.OnConnectWorkerDoWork);
      this.connectWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnConnectWorkerDone);
      this.authenticateWorker = new BackgroundWorker();
      this.authenticateWorker.DoWork += new DoWorkEventHandler(this.OnAuthenticateWorkerDoWork);
      this.authenticateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnAuthenticateWorkerDone);
      this.getMessageCountWorker = new BackgroundWorker();
      this.getMessageCountWorker.DoWork += new DoWorkEventHandler(this.OnGetMessageCountWorkerDoWork);
      this.getMessageCountWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnGetMessageCountWorkerDone);
      this.getMessageSizeWorker = new BackgroundWorker();
      this.getMessageSizeWorker.DoWork += new DoWorkEventHandler(this.OnGetMessageSizeWorkerDoWork);
      this.getMessageSizeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnGetMessageSizeWorkerDone);
      this.getMessageUidWorker = new BackgroundWorker();
      this.getMessageUidWorker.DoWork += new DoWorkEventHandler(this.OnGetMessageUidlWorkerDoWork);
      this.getMessageUidWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnGetMessageUidlWorkerDone);
      this.getMessageHeadersWorker = new BackgroundWorker();
      this.getMessageHeadersWorker.DoWork += new DoWorkEventHandler(this.OnGetMessageHeadersWorkerDoWork);
      this.getMessageHeadersWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnGetMessageHeadersWorkerDone);
      this.getMessageWorker = new BackgroundWorker();
      this.getMessageWorker.DoWork += new DoWorkEventHandler(this.OnGetMessageWorkerDoWork);
      this.getMessageWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnGetMessageWorkerDone);
      this.deleteWorker = new BackgroundWorker();
      this.deleteWorker.DoWork += new DoWorkEventHandler(this.OnDeleteWorkerDoWork);
      this.deleteWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnDeleteWorkerDone);
      this.disconnectWorker = new BackgroundWorker();
      this.disconnectWorker.DoWork += new DoWorkEventHandler(this.OnDisconnectWorkerDoWork);
      this.disconnectWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnDisconnectWorkerDone);
      this.pop3Client = new Pop3Client();
      this.Pop3Server = string.Empty;
      this.Pop3Port = 110;
      this.Pop3UserName = string.Empty;
      this.Pop3Password = string.Empty;
      this.Pop3SizeFilter = 6000;
      this.smtpClient = new SmtpClient();
      this.smtpClient.SendCompleted += new SendCompletedEventHandler(this.SmtpClientSendCompleted);
      this.SmtpServer = string.Empty;
      this.SmtpPort = 25;
      this.SmtpUserName = string.Empty;
      this.SmtpPassword = string.Empty;
      this.FromAddress = string.Empty;
      this.retrievedUids = new List<string>();
      this.autoRetrieveTimer = new System.Timers.Timer();
      this.autoRetrieveTimer.Elapsed += new ElapsedEventHandler(this.AutoRetrieveTimerElapsed);
      this.autoRetrieveTimer.AutoReset = false;
      this.autoRetrieveTimer.SynchronizingObject = (ISynchronizeInvoke) ServerForTrackersService.HiddenForm;
      this.AutoRetrieveFrequency = 10;
      this.AutoRetrieve = false;
      this.displayName = string.Empty;
      this.Pop3Status = "Disconnected";
      this.SmtpStatus = "Disconnected";
      this.AppendActivity = false;
      this.UpdateActivity();
    }

    public event EventHandler SendCompleted;

    public string DisplayName
    {
      get => this.displayName;
      set
      {
        this.displayName = value;
        this.Name = "E-mail Account (" + this.displayName + ")";
      }
    }

    public string Pop3Server { get; set; }

    public int Pop3Port { get; set; }

    public string Pop3UserName { get; set; }

    public string Pop3Password { get; set; }

    public bool Pop3UseSsl { get; set; }

    public int Pop3SizeFilter { get; set; }

    public bool DeleteMailOnServer { get; set; }

    public bool DeleteAll { get; set; }

    public bool AutoRetrieve
    {
      get => this.autoRetrieve;
      set
      {
        this.autoRetrieve = value;
        this.autoRetrieveTimer.Enabled = this.autoRetrieve;
        this.UpdateActivity();
      }
    }

    public int AutoRetrieveFrequency
    {
      get => this.autoRetrieveFreq;
      set
      {
        this.autoRetrieveFreq = value;
        this.autoRetrieveTimer.Interval = this.autoRetrieveFreq <= 0 ? 1.0 : (double) (this.autoRetrieveFreq * 60000);
        this.UpdateActivity();
      }
    }

    public string SmtpServer { get; set; }

    public int SmtpPort { get; set; }

    public bool SmtpRequiresAuthentication { get; set; }

    public bool SmtpUsePop3Credentials { get; set; }

    public string SmtpUserName { get; set; }

    public string SmtpPassword { get; set; }

    public string FromAddress { get; set; }

    private string SmtpStatus
    {
      set
      {
        this.smtpStatus = value;
        this.Status = "SMTP - " + this.smtpStatus + " / POP3 - " + this.pop3Status;
      }
    }

    private string Pop3Status
    {
      set
      {
        this.pop3Status = value;
        this.Status = "SMTP - " + this.smtpStatus + " / POP3 - " + this.pop3Status;
      }
    }

    public bool WasLastSendSuccessful => this.wasLastSendSuccessful;

    public bool Retrieving() => this.connecting || this.pop3Client.Connected;

    public bool Sending() => this.smtpStatus == "Sending message";

    public void AbortRetrieve()
    {
      if (!this.Retrieving())
        return;
      this.abortRequested = true;
    }

    public void AbortSend()
    {
      if (!this.Sending())
        return;
      this.smtpClient.SendAsyncCancel();
    }

    public void Retrieve()
    {
      if (this.Retrieving())
        return;
      this.LoadRetrievedUids(this.Pop3Server, this.Pop3UserName);
      this.ResetCounters();
      this.messageNumber = 0;
      this.abortRequested = false;
      this.connecting = true;
      this.Pop3Status = "Connecting...";
      this.connectWorker.RunWorkerAsync();
    }

    public void Send(string protocol, string remoteModemInfo, byte[] data)
    {
      if (this.Sending())
        return;
      MailMessage message;
      switch (protocol)
      {
        case "SMS":
          string smsBase64Str = SmsUtilities.ConvertBytesToSmsBase64Str((IEnumerable<byte>) data);
          message = new MailMessage(this.FromAddress, remoteModemInfo + "@msg.iridium.com", "NO", smsBase64Str);
          break;
        case "SBD":
          MemoryStream contentStream = new MemoryStream();
          contentStream.Write(data, 0, data.Length);
          contentStream.Seek(0L, SeekOrigin.Begin);
          Attachment attachment = new Attachment((Stream) contentStream, new ContentType()
          {
            MediaType = "application/x-zip-compressed",
            Name = "RemoteUpdate.sbd"
          });
          attachment.TransferEncoding = TransferEncoding.Base64;
          message = new MailMessage(this.FromAddress, "data@sbd.iridium.com", remoteModemInfo, string.Empty);
          message.Attachments.Add(attachment);
          break;
        default:
          return;
      }
      this.SmtpStatus = "Sending message";
      this.smtpClient.Host = this.SmtpServer;
      this.smtpClient.Port = this.SmtpPort;
      this.smtpClient.Credentials = !this.SmtpRequiresAuthentication ? (ICredentialsByHost) null : (!this.SmtpUsePop3Credentials ? (ICredentialsByHost) new NetworkCredential(this.SmtpUserName, this.SmtpPassword) : (ICredentialsByHost) new NetworkCredential(this.Pop3UserName, this.Pop3Password));
      this.smtpClient.SendAsync(message, (object) null);
    }

    private void OnConnectWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.pop3Client.Connect(this.Pop3Server, this.Pop3Port, this.Pop3UseSsl);
    }

    private void OnConnectWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      this.connecting = false;
      if (e.Error != null)
        this.Pop3Status = "Connect failed";
      else if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        this.Pop3Status = "Logging in...";
        this.authenticateWorker.RunWorkerAsync();
      }
    }

    private void OnAuthenticateWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.pop3Client.Authenticate(this.Pop3UserName, this.Pop3Password);
    }

    private void OnAuthenticateWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        this.Pop3Status = e.Error.Message;
        this.disconnectWorker.RunWorkerAsync();
      }
      else if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        this.Pop3Status = "Getting mail stats...";
        this.getMessageCountWorker.RunWorkerAsync();
      }
    }

    private void OnGetMessageCountWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.pop3Client.GetMessageCount();
    }

    private void OnGetMessageCountWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        this.messageCount = (int) e.Result;
        this.GetNextMessage();
      }
    }

    private void OnGetMessageSizeWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.pop3Client.GetMessageSize(this.messageNumber);
    }

    private void OnGetMessageSizeWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        this.GetNextMessage();
      else if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        this.size = (int) e.Result;
        this.Pop3Status = "Retrieving (" + (object) this.messageNumber + " of " + (object) this.messageCount + ")";
        this.getMessageUidWorker.RunWorkerAsync();
      }
    }

    private void OnGetMessageUidlWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.pop3Client.GetMessageUid(this.messageNumber);
    }

    private void OnGetMessageUidlWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        this.uid = (string) e.Result;
        this.currMsgAlreadyRtrvd = this.retrievedUids.Contains(this.uid);
        if (this.currMsgAlreadyRtrvd)
        {
          ++this.numOfAlreadyRtrvdEmails;
          if (this.DeleteMailOnServer)
          {
            if (this.DeleteAll)
              this.deleteWorker.RunWorkerAsync();
            else
              this.getMessageHeadersWorker.RunWorkerAsync();
          }
          else
            this.GetNextMessage();
        }
        else
          this.getMessageHeadersWorker.RunWorkerAsync();
      }
    }

    private void OnGetMessageHeadersWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.pop3Client.GetMessageHeaders(this.messageNumber);
    }

    private void OnGetMessageHeadersWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        this.protocol = new Protocol?();
        if (this.size <= this.Pop3SizeFilter)
        {
          MessageHeader result = (MessageHeader) e.Result;
          if (result.Subject != null)
          {
            Match match1 = Regex.Match(result.Subject, "(?<=SBD Msg From Unit: )(?<IMEI>\\d\\d\\d\\d\\d\\d\\d\\d\\d\\d\\d\\d\\d\\d\\d)");
            if (match1.Success)
            {
              this.modemInfo = match1.Groups["IMEI"].Captures[0].Value;
              this.protocol = new Protocol?(Protocol.Sbd);
            }
            if (!this.protocol.HasValue)
            {
              Match match2 = Regex.Match(result.Subject, "(?<=SMS from )(?<Number>\\+?\\d+)(?=@msg.iridium.com)");
              if (match2.Success)
              {
                this.modemInfo = match2.Groups["Number"].Captures[0].Value;
                this.protocol = new Protocol?(Protocol.Sms);
                this.smsNetwork = EmailAccount.SmsNetwork.Iridium;
              }
            }
          }
          if (!this.protocol.HasValue && result.From != null && result.From.Address != null)
          {
            Match match = Regex.Match(result.From.Address, "(?<Number>\\+?\\d+)(?=@tmomail.net)");
            if (match.Success)
            {
              this.modemInfo = match.Groups["Number"].Captures[0].Value;
              this.protocol = new Protocol?(Protocol.Sms);
              this.smsNetwork = EmailAccount.SmsNetwork.TMobile;
            }
          }
        }
        if (this.currMsgAlreadyRtrvd)
        {
          if (this.protocol.HasValue)
            this.deleteWorker.RunWorkerAsync();
          else
            this.GetNextMessage();
        }
        else if (!this.protocol.HasValue)
          this.HandleFilteredMessage();
        else
          this.getMessageWorker.RunWorkerAsync();
      }
    }

    private void OnGetMessageWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.pop3Client.GetMessage(this.messageNumber);
    }

    private void OnGetMessageWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      if (this.abortRequested)
      {
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        OpenPop.Mime.Message result = (OpenPop.Mime.Message) e.Result;
        Protocol? protocol1 = this.protocol;
        Protocol protocol2 = Protocol.Sbd;
        if ((protocol1.GetValueOrDefault() == protocol2 ? (protocol1.HasValue ? 1 : 0) : 0) != 0)
        {
          List<MessagePart> allAttachments = result.FindAllAttachments();
          if (allAttachments.Count == 0)
          {
            this.HandleFilteredMessage();
            return;
          }
          byte[] body = allAttachments[0].Body;
          if (body.Length == 0)
          {
            this.HandleFilteredMessage();
            return;
          }
          ++this.numOfSbdEmails;
          this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(), ModemInfoTypes.Imei, this.modemInfo, this.protocol.Value, DateTime.UtcNow, (IList<byte>) body));
        }
        else
        {
          MessagePart plainTextVersion = result.FindFirstPlainTextVersion();
          if (plainTextVersion == null)
          {
            this.HandleFilteredMessage();
            return;
          }
          ++this.numOfSmsEmails;
          string str = plainTextVersion.GetBodyAsText();
          if (this.smsNetwork == EmailAccount.SmsNetwork.TMobile && str.EndsWith("\r\n"))
            str = str.Remove(str.Length - 2);
          this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(), ModemInfoTypes.PhoneNumber, this.modemInfo, this.protocol.Value, DateTime.UtcNow, (IList<byte>) SmsUtilities.ConvertStrToGsmBytes(str)));
        }
        this.retrievedUids.Add(this.uid);
        if (this.DeleteMailOnServer)
          this.deleteWorker.RunWorkerAsync();
        else
          this.GetNextMessage();
      }
    }

    private void OnDeleteWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.pop3Client.DeleteMessage(this.messageNumber);
    }

    private void OnDeleteWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      this.retrievedUids.Remove(this.uid);
      this.GetNextMessage();
    }

    private void OnDisconnectWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.pop3Client.Disconnect();
    }

    private void OnDisconnectWorkerDone(object sender, RunWorkerCompletedEventArgs e)
    {
      this.SaveRtrvdUids();
      this.Pop3Status = "Disconnected";
      if (this.abortRequested || !this.DeleteMailOnServer || !this.DeleteAll || this.messageNumber >= this.messageCount)
        return;
      this.Retrieve();
    }

    private void AutoRetrieveTimerElapsed(object sender, ElapsedEventArgs e)
    {
      this.autoRetrieveTimer.Enabled = true;
      this.Retrieve();
    }

    private void SmtpClientSendCompleted(object sender, AsyncCompletedEventArgs e)
    {
      if (e.Cancelled)
      {
        this.SmtpStatus = "Disconnected - Cancelled";
        this.wasLastSendSuccessful = false;
      }
      else if (e.Error != null)
      {
        this.SmtpStatus = "Disconnected - Exception: " + e.Error.Message;
        this.wasLastSendSuccessful = false;
      }
      else
      {
        this.SmtpStatus = "Disconnected";
        this.wasLastSendSuccessful = true;
      }
      if (this.SendCompleted == null)
        return;
      this.SendCompleted((object) this, EventArgs.Empty);
    }

    private void GetNextMessage()
    {
      this.UpdateActivity();
      if (this.abortRequested || this.DeleteMailOnServer && this.DeleteAll && this.messageNumber == 20 || this.messageNumber >= this.messageCount)
      {
        this.Pop3Status = "Logging out...";
        this.disconnectWorker.RunWorkerAsync();
      }
      else
      {
        ++this.messageNumber;
        this.getMessageSizeWorker.RunWorkerAsync();
      }
    }

    private void HandleFilteredMessage()
    {
      this.retrievedUids.Add(this.uid);
      ++this.numOfFilteredOutEmails;
      if (this.DeleteMailOnServer && this.DeleteAll)
        this.deleteWorker.RunWorkerAsync();
      else
        this.GetNextMessage();
    }

    private void UpdateActivity()
    {
      this.Activity = "Auto retrieve: " + (this.autoRetrieve ? (object) "On" : (object) "Off") + "\r\nAuto retrieve frequency: " + (this.autoRetrieveFreq == 0 ? (object) "Continuously" : (object) (this.autoRetrieveFreq.ToString() + " minute(s)")) + "\r\n\r\nRetrieve Summary\r\n---------------------------------------------\r\n" + "SBD:".PadRight(19) + (object) this.numOfSbdEmails + "\r\n" + "SMS:".PadRight(19) + (object) this.numOfSmsEmails + "\r\n" + "Non SBD \\ SMS:".PadRight(19) + (object) this.numOfFilteredOutEmails + "\r\n" + "Already Retrieved:".PadRight(19) + (object) this.numOfAlreadyRtrvdEmails;
    }

    private void ResetCounters()
    {
      this.numOfSbdEmails = 0;
      this.numOfSmsEmails = 0;
      this.numOfFilteredOutEmails = 0;
      this.numOfAlreadyRtrvdEmails = 0;
    }

    private void SaveRtrvdUids()
    {
      string contents = string.Join("\r\n", this.retrievedUids.ToArray());
      try
      {
        System.IO.File.WriteAllText(Utils.GetServiceDataDirectory((string) null) + "\\" + this.retrievedUidsFileName, contents);
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadRetrievedUids(string host, string userName)
    {
      this.retrievedUids.Clear();
      this.retrievedUidsFileName = string.Empty;
      string str = host + "_" + userName + ".uidls";
      for (int index = 0; index < str.Length; ++index)
      {
        switch (str[index])
        {
          case '"':
            this.retrievedUidsFileName += "#d";
            break;
          case '*':
            this.retrievedUidsFileName += "#a";
            break;
          case '/':
            this.retrievedUidsFileName += "#f";
            break;
          case ':':
            this.retrievedUidsFileName += "#c";
            break;
          case '<':
            this.retrievedUidsFileName += "#l";
            break;
          case '>':
            this.retrievedUidsFileName += "#g";
            break;
          case '?':
            this.retrievedUidsFileName += "#q";
            break;
          case '\\':
            this.retrievedUidsFileName += "#b";
            break;
          case '|':
            this.retrievedUidsFileName += "#v";
            break;
          default:
            this.retrievedUidsFileName += str[index].ToString();
            break;
        }
      }
      try
      {
        this.retrievedUids.AddRange((IEnumerable<string>) System.IO.File.ReadAllText(Utils.GetServiceDataDirectory((string) null) + "\\" + this.retrievedUidsFileName).Split(new string[1]
        {
          "\r\n"
        }, StringSplitOptions.RemoveEmptyEntries));
      }
      catch (Exception ex)
      {
      }
    }

    private enum SmsNetwork
    {
      Iridium,
      TMobile,
    }
  }
}
