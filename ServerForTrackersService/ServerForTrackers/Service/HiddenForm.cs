// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.HiddenForm
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.EncryptionModule;
using Nal.GpsReport;
using Nal.Network;
using Nal.RemoteUpdate;
using Nal.ServerForTrackers.Common;
using Nal.ServerForTrackers.Plugins;
using Nal.Sms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class HiddenForm : Form
  {
    private string encryptionUserName;
    private string encryptionUserPassword;
    private List<LogFile> logFiles;
    private List<LoggingDirectory> loggingDirectories;
    private List<BatchFile> batchFiles;
    private List<Plugin> plugins;
    private bool dataServerIncludeSbd;
    private bool dataServerIncludeSms;
    private bool dataServerIncludeCalls;
    private bool dataServerIncludeTcp;
    private List<DataTypeSettings> dataServerDataTypeSettingsList;
    private TcpListenerWrapper dataServer;
    private TcpListenerWrapper controlServer;
    private List<CommLink> commLinks;
    private List<SocketWrapper> dataSockets;
    private SocketWrapper controlSocket;
    private ControlMessageHandler cntrlMsgHandler;
    private StringBuilder fileLogBuffer;
    private StringBuilder controllerLogBuffer;
    private Dictionary<object, RURequestInfo> ruRequestsBeingSent;
    private List<RUPair> ruPairs;
    private List<HiddenForm.MultipartSmsBuffer> multipartSmsBuffers;
    private TcpServerQueues tcpServerQueues;
    private HiddenForm.OutputCache outputCache;
    private IContainer components;
    private Timer waitForFileTimer;
    private FileSystemWatcher settingsFileSystemWatcher;
    private Timer checkMultipartSmsTimer;

    public HiddenForm() => this.InitializeComponent();

    private void OnHiddenFormLoad(object sender, EventArgs e)
    {
      CommLink.EncryptionUser = new EncryptionUser();
      int num = (int) CommLink.EncryptionUser.SetKeysDirectory(Nal.ServerForTrackers.Common.Utils.GetServiceDataDirectory((string) null) + "\\Keys");
      this.encryptionUserName = string.Empty;
      this.encryptionUserPassword = string.Empty;
      this.fileLogBuffer = new StringBuilder();
      this.controllerLogBuffer = new StringBuilder();
      this.dataServer = new TcpListenerWrapper(true);
      this.controlServer = new TcpListenerWrapper(true);
      this.controlSocket = new SocketWrapper();
      this.dataSockets = new List<SocketWrapper>();
      this.commLinks = new List<CommLink>();
      this.logFiles = new List<LogFile>();
      this.loggingDirectories = new List<LoggingDirectory>();
      this.batchFiles = new List<BatchFile>();
      this.plugins = new List<Plugin>();
      this.dataServerDataTypeSettingsList = new List<DataTypeSettings>();
      this.cntrlMsgHandler = new ControlMessageHandler(this.controlSocket);
      this.ruRequestsBeingSent = new Dictionary<object, RURequestInfo>();
      this.ruPairs = new List<RUPair>();
      this.multipartSmsBuffers = new List<HiddenForm.MultipartSmsBuffer>();
      this.tcpServerQueues = new TcpServerQueues();
      this.outputCache = new HiddenForm.OutputCache();
      this.dataServer.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnDataServerClientConnected);
      this.controlServer.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnControlServerClientConnected);
      this.controlSocket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnControlSocketDataReceived);
      this.controlSocket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnControlSocketConnectionClosed);
      this.cntrlMsgHandler.SndMessageReceived += new SndMessageReceivedEventHandler(this.OnCntrlMsgHandlerSndMessageReceived);
      this.cntrlMsgHandler.QryMessageReceived += new EventHandler(this.OnCntrlMsgHandlerQryMessageReceived);
      this.cntrlMsgHandler.AboMessageReceived += new EventHandler(this.OnCntrlMsgHandlerAboMessageReceived);
      this.cntrlMsgHandler.CmdMessageReceived += new SimpleMessageReceivedEventHandler(this.OnCntrlMsgHandlerCmdMessageReceived);
      this.cntrlMsgHandler.ConMessageReceived += new ConMessageReceivedEventHandler(this.OnCntrlMsgHandlerConMessageReceived);
      this.Log("Service started.\r\n", HiddenForm.LogDest.File, HiddenForm.LogOpt.FlushFile);
      this.LoadRemoteUpdates();
      this.settingsFileSystemWatcher.Path = Nal.ServerForTrackers.Common.Utils.GetServiceDataDirectory((string) null);
      this.settingsFileSystemWatcher.EnableRaisingEvents = true;
      this.LoadSettings();
      this.checkMultipartSmsTimer.Start();
      this.tcpServerQueues.Load();
    }

    private void OnHiddenFormClosing(object sender, FormClosingEventArgs e)
    {
      this.dataServer.Shutdown();
      this.controlServer.Shutdown();
      this.controlSocket.Disconnect();
      foreach (SocketWrapper dataSocket in this.dataSockets)
        dataSocket.Disconnect();
      foreach (CommLink commLink in this.commLinks)
        this.CleanupCommLink(commLink);
      this.Log("Service stopped.\r\n\r\n", HiddenForm.LogDest.File, HiddenForm.LogOpt.FlushFile);
    }

    private void OnSettingsFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
    {
      this.waitForFileTimer.Start();
    }

    private void OnWaitForFileTimerTick(object sender, EventArgs e)
    {
      this.waitForFileTimer.Stop();
      this.LoadSettings();
    }

    private void OnCheckMultipartSmsTimerTick(object sender, EventArgs e)
    {
      int index = 0;
      while (index < this.multipartSmsBuffers.Count)
      {
        if ((DateTime.UtcNow - this.multipartSmsBuffers[index].StartTime).TotalMinutes > 10.0)
          this.multipartSmsBuffers.RemoveAt(index);
        else
          ++index;
      }
    }

    private void OnEmailAccountSendCompleted(object sender, EventArgs e)
    {
      EmailAccount emailAccount = sender as EmailAccount;
      RURequestInfo requestInfoBeingSent = this.GetRURequestInfoBeingSent((object) emailAccount);
      if (requestInfoBeingSent == null)
        return;
      this.ruRequestsBeingSent.Remove((object) emailAccount);
      if (!emailAccount.WasLastSendSuccessful)
        return;
      this.SaveRURequestInfo(requestInfoBeingSent);
    }

    private void OnSbdDirectIpAccountClientConnected(object sender, ClientConnectedEventArgs e)
    {
      SbdDirectIpAccount sbdDirectIpAccount = (SbdDirectIpAccount) sender;
      SbdDirectIpClientHandler directIpClientHandler = new SbdDirectIpClientHandler(e.ConnectedSocket, sbdDirectIpAccount.SendMODirectIPAcknowledgement);
      directIpClientHandler.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
      directIpClientHandler.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
      directIpClientHandler.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
      directIpClientHandler.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
      directIpClientHandler.RemoveMe += new EventHandler(this.OnCommLinkRemoveMe);
      directIpClientHandler.ID = Guid.NewGuid().ToString();
      this.commLinks.Add((CommLink) directIpClientHandler);
      this.cntrlMsgHandler.SendAddMessage(directIpClientHandler.ID, directIpClientHandler.Type, directIpClientHandler.Name, directIpClientHandler.Status, directIpClientHandler.Activity);
    }

    private void OnSbdDirectIpClientConnectFailed(object sender, EventArgs e)
    {
      this.ruRequestsBeingSent.Remove(sender);
    }

    private void OnSbdDirectIpClientConnectionDropped(object sender, EventArgs e)
    {
      SbdDirectIpClient sbdDirectIpClient = (SbdDirectIpClient) sender;
      RURequestInfo requestInfoBeingSent = this.GetRURequestInfoBeingSent((object) sbdDirectIpClient);
      if (requestInfoBeingSent == null)
        return;
      this.ruRequestsBeingSent.Remove((object) sbdDirectIpClient);
      if (!sbdDirectIpClient.ConfirmationStatusReceived || sbdDirectIpClient.ConfirmationStatus <= (short) 0)
        return;
      this.SaveRURequestInfo(requestInfoBeingSent);
    }

    private void OnTcpProtocolServerClientConnected(object sender, ClientConnectedEventArgs e)
    {
      try
      {
        TcpProtocolClientHandler protocolClientHandler = new TcpProtocolClientHandler(e.ConnectedSocket, this.tcpServerQueues);
        protocolClientHandler.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
        protocolClientHandler.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
        protocolClientHandler.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
        protocolClientHandler.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
        protocolClientHandler.RemoveMe += new EventHandler(this.OnCommLinkRemoveMe);
        protocolClientHandler.PacketSent += new TcpProtocolClientHandler.PacketSentEventHandler(this.OnTcpProtocolClientHandlerPacketSent);
        protocolClientHandler.ID = Guid.NewGuid().ToString();
        this.commLinks.Add((CommLink) protocolClientHandler);
        this.cntrlMsgHandler.SendAddMessage(protocolClientHandler.ID, protocolClientHandler.Type, protocolClientHandler.Name, protocolClientHandler.Status, protocolClientHandler.Activity);
      }
      catch (Exception ex)
      {
        this.Log(HiddenForm.FormatForLog(ex), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
      }
    }

    private void OnTcpProtocolClientHandlerPacketSent(
      object sender,
      TcpProtocolClientHandler.PacketSentEventArgs e)
    {
      this.SaveRURequestInfo(new RURequestInfo()
      {
        Data = (IList<byte>) new List<byte>((IEnumerable<byte>) e.Data),
        Recipient = e.Imei,
        RecipientType = ModemInfoTypes.Imei
      });
    }

    private void OnRudicsAccountClientConnected(object sender, ClientConnectedEventArgs e)
    {
      RudicsAccount rudicsAccount = (RudicsAccount) sender;
      RudicsClientHandler rudicsClientHandler = new RudicsClientHandler();
      rudicsClientHandler.CallProtocol = rudicsAccount.ClientHandlerCallProtocol;
      rudicsClientHandler.EnablePipe = rudicsAccount.EnablePipe;
      rudicsClientHandler.PipeHost = rudicsAccount.PipeHost;
      rudicsClientHandler.PipePort = rudicsAccount.PipePort;
      rudicsClientHandler.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
      rudicsClientHandler.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
      rudicsClientHandler.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
      rudicsClientHandler.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
      rudicsClientHandler.RemoveMe += new EventHandler(this.OnCommLinkRemoveMe);
      rudicsClientHandler.ID = Guid.NewGuid().ToString();
      this.commLinks.Add((CommLink) rudicsClientHandler);
      this.cntrlMsgHandler.SendAddMessage(rudicsClientHandler.ID, rudicsClientHandler.Type, rudicsClientHandler.Name, rudicsClientHandler.Status, rudicsClientHandler.Activity);
      rudicsClientHandler.HandleClient(e.ConnectedSocket);
    }

    private void OnDataServerClientConnected(object sender, ClientConnectedEventArgs e)
    {
      SocketWrapper socketWrapper = new SocketWrapper();
      socketWrapper.TakeOverConnection(e.ConnectedSocket);
      socketWrapper.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnDataSocketConnectionClosed);
      this.dataSockets.Add(socketWrapper);
    }

    private void OnControlServerClientConnected(object sender, ClientConnectedEventArgs e)
    {
      try
      {
        this.controlSocket.TakeOverConnection(e.ConnectedSocket);
      }
      catch
      {
        e.ConnectedSocket.Close();
        this.cntrlMsgHandler.SendInfMessage("Only one controller is allowed to be connected to the service.\r\n");
        return;
      }
      foreach (CommLink commLink in this.commLinks)
        this.cntrlMsgHandler.SendAddMessage(commLink.ID, commLink.Type, commLink.Name, commLink.Status, commLink.Activity);
    }

    private void OnDataSocketConnectionClosed(object sender, EventArgs e)
    {
      this.dataSockets.Remove((SocketWrapper) sender);
    }

    private void OnControlSocketConnectionClosed(object sender, EventArgs e)
    {
    }

    private void OnControlSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      this.cntrlMsgHandler.ProcessIncommingData(e.Data);
    }

    private void OnCommLinkDataReceived(object eventSender, CommLink.DataReceivedEventArgs e)
    {
      try
      {
        CommLink commLink = (CommLink) eventSender;
        ModemInfoTypes senderType = e.SenderType;
        string sender = e.Sender;
        List<byte> data = new List<byte>((IEnumerable<byte>) e.Data);
        DataType? nullable1 = e.Type;
        this.outputCache.Clear();
        if (e.Protocol == Protocol.Sms)
        {
          SmsUserData userData;
          if (!SmsUserData.Parse((IEnumerable<byte>) e.Data, out userData))
            return;
          ImeiSmsHeaderElement smsHeaderElement1 = (ImeiSmsHeaderElement) userData.HeaderElements.Where<SmsHeaderElement>((Func<SmsHeaderElement, bool>) (x => x.Id == (byte) 1)).LastOrDefault<SmsHeaderElement>();
          if (smsHeaderElement1 != null)
          {
            senderType = ModemInfoTypes.Imei;
            sender = smsHeaderElement1.Imei.ToString().PadLeft(15, '0');
          }
          MultipartSmsHeaderElement smsHeaderElement2 = (MultipartSmsHeaderElement) userData.HeaderElements.Where<SmsHeaderElement>((Func<SmsHeaderElement, bool>) (x => x.Id == (byte) 0)).LastOrDefault<SmsHeaderElement>();
          if (smsHeaderElement2 != null)
          {
            HiddenForm.MultipartSmsBuffer multipartSmsBuffer1 = (HiddenForm.MultipartSmsBuffer) null;
            foreach (HiddenForm.MultipartSmsBuffer multipartSmsBuffer2 in this.multipartSmsBuffers)
            {
              if (multipartSmsBuffer2.SenderType == senderType && multipartSmsBuffer2.Sender == sender && (int) multipartSmsBuffer2.RefNum == (int) smsHeaderElement2.ReferenceNumber)
              {
                if ((int) smsHeaderElement2.Total != multipartSmsBuffer2.Parts.Length)
                {
                  this.multipartSmsBuffers.Remove(multipartSmsBuffer2);
                  break;
                }
                multipartSmsBuffer1 = multipartSmsBuffer2;
                break;
              }
            }
            if (multipartSmsBuffer1 == null)
            {
              multipartSmsBuffer1 = new HiddenForm.MultipartSmsBuffer(senderType, sender, smsHeaderElement2.ReferenceNumber, smsHeaderElement2.Total);
              this.multipartSmsBuffers.Add(multipartSmsBuffer1);
            }
            int index = (int) smsHeaderElement2.SequenceNumber - 1;
            if (multipartSmsBuffer1.Parts[index] == null)
            {
              multipartSmsBuffer1.Parts[index] = new byte[data.Count];
              data.CopyTo(multipartSmsBuffer1.Parts[index]);
            }
            if (((IEnumerable<byte[]>) multipartSmsBuffer1.Parts).Any<byte[]>((Func<byte[], bool>) (x => x == null)))
              return;
            List<byte> collection = new List<byte>();
            foreach (byte[] part in multipartSmsBuffer1.Parts)
              collection.AddRange((IEnumerable<byte>) part);
            data.Clear();
            data.AddRange((IEnumerable<byte>) collection);
            this.multipartSmsBuffers.Remove(multipartSmsBuffer1);
          }
          else
          {
            data.Clear();
            data.AddRange((IEnumerable<byte>) userData.Payload);
          }
        }
        bool isEncrypted;
        ulong imei;
        byte[] innerData;
        if (!nullable1.HasValue && PecosMessage.ParseOuter((IList<byte>) data, out isEncrypted, out imei, out innerData))
        {
          bool flag = false;
          string modemInfo = imei.ToString("000000000000000");
          if (isEncrypted == CommLink.UseEncryption)
          {
            if (!isEncrypted)
              flag = true;
            else if (CommLink.EncryptionUser.Decrypt(modemInfo, "I", innerData, ref innerData) == Erc.Success)
              flag = true;
          }
          PecosMessage message;
          if (flag && PecosMessage.ParseInner(isEncrypted, imei, (IList<byte>) innerData, out message))
          {
            if (message is PecosP3Message)
            {
              this.outputCache.GpsReport = (Nal.GpsReport.GpsReport) new PecosP3GpsReport((PecosP3Message) message);
              nullable1 = new DataType?(DataType.PecosP3GpsReport);
            }
            else if (message is PecosP4Message)
            {
              this.outputCache.GpsReport = (Nal.GpsReport.GpsReport) new PecosP4GpsReport((PecosP4Message) message);
              nullable1 = new DataType?(DataType.PecosP4GpsReport);
            }
            if (nullable1.HasValue)
            {
              if (isEncrypted)
              {
                data.Clear();
                data.AddRange((IEnumerable<byte>) message.GetBytes((byte[]) null));
              }
              senderType = ModemInfoTypes.Imei;
              sender = modemInfo;
            }
          }
        }
        if (nullable1.HasValue)
        {
          DataType? nullable2 = nullable1;
          DataType dataType = DataType.Other;
          if ((nullable2.GetValueOrDefault() == dataType ? (nullable2.HasValue ? 1 : 0) : 0) == 0 || e.Protocol != Protocol.Call)
            goto label_47;
        }
        if (CommLink.UseEncryption)
        {
          byte[] source = (byte[]) null;
          EncryptionUser encryptionUser = CommLink.EncryptionUser;
          string modemInfo = sender;
          string modemInfoType;
          switch (senderType)
          {
            case ModemInfoTypes.PhoneNumber:
              modemInfoType = "P";
              break;
            case ModemInfoTypes.Imei:
              modemInfoType = "I";
              break;
            default:
              modemInfoType = string.Empty;
              break;
          }
          byte[] array = data.ToArray();
          ref byte[] local = ref source;
          if (encryptionUser.Decrypt(modemInfo, modemInfoType, array, ref local) != Erc.Success)
            return;
          data.Clear();
          data.AddRange((IEnumerable<byte>) ((IEnumerable<byte>) source).ToList<byte>());
        }
label_47:
        NalGpsReport report;
        if (!nullable1.HasValue && NalGpsReport.Parse(data.ToArray(), out report))
        {
          this.outputCache.GpsReport = (Nal.GpsReport.GpsReport) report;
          switch (report)
          {
            case Nal10ByteGpsReport0 _:
              nullable1 = new DataType?(DataType.Nal10ByteGpsReport0);
              break;
            case NalGpsReport3 _:
              nullable1 = new DataType?(DataType.NalGpsReport3);
              break;
            case NalGpsReport4 _:
              nullable1 = new DataType?(DataType.NalGpsReport4);
              break;
            case NalGpsReport5 _:
              nullable1 = new DataType?(DataType.NalGpsReport5);
              break;
            case NalGpsReport6 _:
              nullable1 = new DataType?(DataType.NalGpsReport6);
              break;
            case NalGpsReport7 _:
              nullable1 = new DataType?(DataType.NalGpsReport7);
              break;
          }
        }
        UpdateResponse response;
        if (!nullable1.HasValue && UpdateResponse.Parse((IList<byte>) data, out response))
        {
          this.outputCache.UpdateResponse = response;
          RUPair ruPair = (RUPair) null;
          switch (response)
          {
            case UpdateResponse0 _:
              nullable1 = new DataType?(DataType.UpdateResponse0);
              ruPair = this.FindBestMatchForUpdateResponse0((UpdateResponse0) response, senderType, sender);
              break;
            case UpdateResponse1 _:
              nullable1 = new DataType?(DataType.UpdateResponse1);
              ruPair = this.FindBestMatchForUpdateResponse1((UpdateResponse1) response, senderType, sender);
              break;
            case UpdateResponse2 _:
              nullable1 = new DataType?(DataType.UpdateResponse2);
              ruPair = this.FindBestMatchForUpdateResponse2((UpdateResponse2) response, senderType, sender);
              break;
            case UpdateResponse3 _:
              nullable1 = new DataType?(DataType.UpdateResponse3);
              ruPair = this.FindBestMatchForUpdateResponse3((UpdateResponse3) response, senderType, sender);
              break;
          }
          if (ruPair == null)
          {
            ruPair = new RUPair();
            this.ruPairs.Add(ruPair);
          }
          ruPair.ResponseInfo = new RUResponseInfo()
          {
            TimeReceived = DateTime.UtcNow,
            SenderType = senderType,
            Sender = sender,
            Data = (IList<byte>) data
          };
          try
          {
            RUPairsFile.Save(Nal.ServerForTrackers.Common.Utils.GetRUPairsFileName((string) null), this.ruPairs);
          }
          catch
          {
          }
        }
        StatusReport0 update;
        if (!nullable1.HasValue && StatusReport0.Parse((IList<byte>) data, out update))
        {
          this.outputCache.StatusReport = update;
          nullable1 = new DataType?(DataType.StatusReport0);
        }
        if (!nullable1.HasValue)
          nullable1 = new DataType?(DataType.Other);
        if (this.outputCache.GpsReport != null && this.outputCache.GpsReport.IsEmergency())
          this.Log("EMERGENCY GPS REPORT RECEIVED!\r\n  Sender - " + (object) senderType + "(" + sender + ")\r\n  Receiver - " + commLink.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
        this.outputCache.Data = data;
        this.outputCache.MetaElement = this.CreateMetaElement(commLink, e.SessionStart, senderType, sender, e.Protocol, nullable1.Value);
        foreach (LogFile logFile in this.logFiles)
          this.OutputToLogFile(logFile, e.Protocol, nullable1.Value);
        foreach (LoggingDirectory loggingDirectory in this.loggingDirectories)
          this.OutputToLoggingDirectory(loggingDirectory, e.Protocol, nullable1.Value);
        foreach (BatchFile batchFile in this.batchFiles)
          this.OutputToBatchFile(batchFile, e.Protocol, nullable1.Value);
        this.OutputToDataSocket(e.Protocol, nullable1.Value);
        foreach (Plugin plugin in this.plugins)
          this.OutputToPlugin(plugin, e.Protocol, nullable1.Value);
      }
      catch (Exception ex)
      {
        this.Log("Exception occured during processing. Please send the following details to contact@nalresearch.com so that the problem can be fixed.\r\n" + HiddenForm.FormatForLog(ex), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
      }
    }

    public static string FormatForLog(Exception exception)
    {
      string str = string.Format("Exception ({0}): {1}", (object) exception.GetType().Name, (object) exception.Message);
      if (!string.IsNullOrEmpty(exception.StackTrace))
        str = str + "\r\nStack Trace:\r\n" + exception.StackTrace;
      while (exception.InnerException != null)
      {
        exception = exception.InnerException;
        str += string.Format("\r\nInner Exception ({0}): {1}", (object) exception.GetType().Name, (object) exception.Message);
        if (!string.IsNullOrEmpty(exception.StackTrace))
          str = str + "\r\nStack Trace:\r\n" + exception.StackTrace;
      }
      return str;
    }

    private void OnCommLinkStatusChanged(object sender, EventArgs e)
    {
      CommLink commLink = (CommLink) sender;
      this.cntrlMsgHandler.SendStaMessage(commLink.ID, commLink.Status);
    }

    private void OnCommLinkActivityOccured(object sender, EventArgs e)
    {
      CommLink commLink = (CommLink) sender;
      this.cntrlMsgHandler.SendActMessage(commLink.ID, commLink.AppendActivity, commLink.Activity);
    }

    private void OnCommLinkNameChanged(object sender, EventArgs e)
    {
      CommLink commLink = (CommLink) sender;
      this.cntrlMsgHandler.SendNamMessage(commLink.ID, commLink.Name);
    }

    private void OnCommLinkRemoveMe(object sender, EventArgs e)
    {
      CommLink commLink = (CommLink) sender;
      this.commLinks.Remove(commLink);
      if (commLink.FinalWords != string.Empty)
        this.cntrlMsgHandler.SendInfMessage(commLink.Name + ": " + commLink.FinalWords + "\r\n");
      this.cntrlMsgHandler.SendRemMessage(commLink.ID);
    }

    private void OnCntrlMsgHandlerSndMessageReceived(object sender, SndMessageReceivedEventArgs e)
    {
      List<byte> list;
      if (CommLink.UseEncryption)
      {
        string modemInfoType = e.protocol == "SMS" ? "P" : "I";
        byte[] encryptedBytes = (byte[]) null;
        if (CommLink.EncryptionUser.Encrypt(e.remoteModemInfo, modemInfoType, e.data, ref encryptedBytes) != Erc.Success)
          return;
        list = ((IEnumerable<byte>) encryptedBytes).ToList<byte>();
      }
      else
        list = ((IEnumerable<byte>) e.data).ToList<byte>();
      if (e.sendBackUpdate)
        this.cntrlMsgHandler.SendPduMessage(SmsUtilities.ConvertBytesToSmsBase64Str((IEnumerable<byte>) list));
      else if (e.protocol == "TCP")
      {
        string str;
        if (e.remoteModemInfo != null && e.remoteModemInfo.Length == 15 && e.remoteModemInfo.All<char>((Func<char, bool>) (x => char.IsDigit(x))))
        {
          this.tcpServerQueues.Queue(e.remoteModemInfo, list.ToArray());
          this.tcpServerQueues.Save();
          str = "Successful";
        }
        else
          str = "Invalid IMEI";
        this.Log("Adding update to TCP server queue: " + str + "\r\n", HiddenForm.LogDest.Controller, HiddenForm.LogOpt.FlushBoth);
      }
      else
      {
        CommLink commLink = this.GetCommLink(e.id);
        switch (commLink)
        {
          case EmailAccount _:
            EmailAccount key1 = (EmailAccount) commLink;
            if (key1.Sending())
            {
              this.cntrlMsgHandler.SendInfMessage("E-mail Account: Cannot send - Already sending.\r\n");
              break;
            }
            try
            {
              key1.Send(e.protocol, e.remoteModemInfo, list.ToArray());
              this.ruRequestsBeingSent[(object) key1] = new RURequestInfo()
              {
                Data = (IList<byte>) new List<byte>((IEnumerable<byte>) e.data),
                Recipient = e.remoteModemInfo,
                RecipientType = e.protocol == "SMS" ? ModemInfoTypes.PhoneNumber : ModemInfoTypes.Imei
              };
              break;
            }
            catch (Exception ex)
            {
              this.cntrlMsgHandler.SendInfMessage("E-mail Account: Cannot send - " + ex.Message + "\r\n");
              break;
            }
          case SbdDirectIpAccount _:
            SbdDirectIpAccount sbdDirectIpAccount = (SbdDirectIpAccount) commLink;
            SbdDirectIpClient key2 = new SbdDirectIpClient();
            key2.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
            key2.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
            key2.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
            key2.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
            key2.RemoveMe += new EventHandler(this.OnCommLinkRemoveMe);
            key2.ConnectFailed += new EventHandler(this.OnSbdDirectIpClientConnectFailed);
            key2.ConnectionDropped += new EventHandler(this.OnSbdDirectIpClientConnectionDropped);
            key2.ID = Guid.NewGuid().ToString();
            this.commLinks.Add((CommLink) key2);
            this.cntrlMsgHandler.SendAddMessage(key2.ID, key2.Type, key2.Name, key2.Status, key2.Activity);
            try
            {
              key2.Send(sbdDirectIpAccount.ClientHost, sbdDirectIpAccount.ClientPort, e.remoteModemInfo, list.ToArray());
              this.ruRequestsBeingSent[(object) key2] = new RURequestInfo()
              {
                Data = (IList<byte>) new List<byte>((IEnumerable<byte>) e.data),
                Recipient = e.remoteModemInfo,
                RecipientType = ModemInfoTypes.Imei
              };
              break;
            }
            catch (InvalidOperationException ex)
            {
              this.cntrlMsgHandler.SendInfMessage("SBD DirectIP Client: Cannot send - " + ex.Message + "\r\n");
              break;
            }
        }
      }
    }

    private void OnCntrlMsgHandlerQryMessageReceived(object sender, EventArgs e)
    {
      int totalWidth = 45;
      this.Log("Reporting Current Status...\r\n--------------------------------------------------\r\n", HiddenForm.LogDest.Controller, HiddenForm.LogOpt.FlushController);
      foreach (CommLink commLink in this.commLinks)
      {
        switch (commLink)
        {
          case ServerModem _:
            ServerModem serverModem = (ServerModem) commLink;
            string str1 = serverModem.PortOpened ? "Port opened" : "Port closed";
            this.cntrlMsgHandler.SendInfMessage(serverModem.Name.PadRight(totalWidth) + str1 + "\r\n");
            continue;
          case SbdDirectIpAccount _:
            SbdDirectIpAccount sbdDirectIpAccount = (SbdDirectIpAccount) commLink;
            string str2 = sbdDirectIpAccount.Listening ? "Listening" : "Not listening";
            this.cntrlMsgHandler.SendInfMessage(sbdDirectIpAccount.Name.PadRight(totalWidth) + str2 + "\r\n");
            continue;
          case RudicsAccount _:
            RudicsAccount rudicsAccount = (RudicsAccount) commLink;
            string str3 = rudicsAccount.Listening ? "Listening" : "Not listening";
            this.cntrlMsgHandler.SendInfMessage(rudicsAccount.Name.PadRight(totalWidth) + str3 + "\r\n");
            continue;
          default:
            continue;
        }
      }
      string str4 = this.controlServer.Running ? "Listening" : "Not listening";
      string str5 = this.dataServer.Running ? "Listening" : "Not listening";
      string str6 = CommLink.EncryptionUser.IsLoggedIn() ? "Logged in" : "Logged out";
      this.cntrlMsgHandler.SendInfMessage("Control server".PadRight(totalWidth) + str4 + "\r\n");
      this.cntrlMsgHandler.SendInfMessage("Data server".PadRight(totalWidth) + str5 + "\r\n");
      this.cntrlMsgHandler.SendInfMessage("Encryption user".PadRight(totalWidth) + str6 + "\r\n\r\n");
    }

    private void OnCntrlMsgHandlerAboMessageReceived(object sender, EventArgs e)
    {
      string toLog;
      try
      {
        string title = ((AssemblyTitleAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false)[0]).Title;
        string str = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        string description = ((AssemblyDescriptionAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false)[0]).Description;
        string product = ((AssemblyProductAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false)[0]).Product;
        string copyright = ((AssemblyCopyrightAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false)[0]).Copyright;
        string company = ((AssemblyCompanyAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false)[0]).Company;
        toLog = "About...\r\n--------------------------------------------------\r\n" + product + "\r\nVersion " + str + "\r\n" + copyright + " " + company + "\r\n\r\ncontact@nalresearch.com\r\nwww.nalresearch.com\r\n\r\nPhone: 1-703-392-1136\r\nFax: 1-703-392-6795\r\n\r\n9385 Discovery Boulevard, Suite 300\r\nManassas, Virginia 20109\r\nUSA\r\n\r\n";
      }
      catch
      {
        toLog = string.Empty;
      }
      this.Log(toLog, HiddenForm.LogDest.Controller, HiddenForm.LogOpt.FlushController);
    }

    private void OnCntrlMsgHandlerConMessageReceived(object sender, ConMessageReceivedEventArgs e)
    {
      CommLink commLink = this.GetCommLink(e.id);
      switch (commLink)
      {
        case ServerModem _:
          ServerModem serverModem = (ServerModem) commLink;
          if (!(e.protocol == "Data Call"))
            break;
          try
          {
            serverModem.Connect(e.remoteModemInfo);
            break;
          }
          catch (InvalidOperationException ex)
          {
            this.cntrlMsgHandler.SendInfMessage("Could not contact - Exception: " + ex.Message + "\r\n");
            break;
          }
        case RudicsAccount _:
          RudicsAccount rudicsAccount = (RudicsAccount) commLink;
          if (!(e.protocol == "Data Call"))
            break;
          RudicsClient rudicsClient = new RudicsClient();
          rudicsClient.CallProtocol = rudicsAccount.ClientCallProtocol;
          rudicsClient.Host = rudicsAccount.ClientHost;
          rudicsClient.PortsBegin = rudicsAccount.ClientPortsBegin;
          rudicsClient.PortsEnd = rudicsAccount.ClientPortsEnd;
          rudicsClient.EnablePipe = rudicsAccount.EnablePipe;
          rudicsClient.PipeHost = rudicsAccount.PipeHost;
          rudicsClient.PipePort = rudicsAccount.PipePort;
          rudicsClient.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
          rudicsClient.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
          rudicsClient.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
          rudicsClient.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
          rudicsClient.RemoveMe += new EventHandler(this.OnCommLinkRemoveMe);
          rudicsClient.ID = Guid.NewGuid().ToString();
          this.commLinks.Add((CommLink) rudicsClient);
          this.cntrlMsgHandler.SendAddMessage(rudicsClient.ID, rudicsClient.Type, rudicsClient.Name, rudicsClient.Status, rudicsClient.Activity);
          rudicsClient.Connect(e.remoteModemInfo);
          break;
      }
    }

    private void OnCntrlMsgHandlerCmdMessageReceived(
      object sender,
      SimpleMessageReceivedEventArgs e)
    {
      CommLink commLink = this.GetCommLink(e.id);
      switch (commLink)
      {
        case EmailAccount _:
          EmailAccount emailAccount = (EmailAccount) commLink;
          if (e.value == "Retrieve")
          {
            if (!emailAccount.Retrieving())
            {
              emailAccount.Retrieve();
              break;
            }
            this.cntrlMsgHandler.SendInfMessage("E-mail Account: Cannot retrieve - Already retrieving.\r\n");
            break;
          }
          if (e.value == "Abort Retrieve")
          {
            if (emailAccount.Retrieving())
            {
              emailAccount.AbortRetrieve();
              break;
            }
            this.cntrlMsgHandler.SendInfMessage("E-mail Account: Cannot abort - Not retrieving.\r\n");
            break;
          }
          if (!(e.value == "Abort Send"))
            break;
          if (emailAccount.Sending())
          {
            emailAccount.AbortSend();
            break;
          }
          this.cntrlMsgHandler.SendInfMessage("E-mail Account: Cannot abort - Not sending.\r\n");
          break;
        case ServerModem _:
          ServerModem serverModem = (ServerModem) commLink;
          if (e.value == "Disconnect")
          {
            try
            {
              serverModem.Disconnect();
              break;
            }
            catch (InvalidOperationException ex)
            {
              this.cntrlMsgHandler.SendInfMessage("Server Modem: Cannot disconnect - " + ex.Message + "\r\n");
              break;
            }
          }
          else
          {
            if (!(e.value == "Abort Dialing"))
              break;
            try
            {
              serverModem.AbortDialing();
              break;
            }
            catch (InvalidOperationException ex)
            {
              this.cntrlMsgHandler.SendInfMessage("Server Modem: Cannot abort dialing - " + ex.Message + "\r\n");
              break;
            }
          }
        case RudicsClientHandler _:
          RudicsClientHandler rudicsClientHandler = (RudicsClientHandler) commLink;
          if (!(e.value == "Disconnect"))
            break;
          rudicsClientHandler.Disconnect();
          break;
        case RudicsClient _:
          RudicsClient rudicsClient = (RudicsClient) commLink;
          if (!(e.value == "Disconnect"))
            break;
          rudicsClient.Disconnect();
          break;
        case SbdDirectIpClient _:
          SbdDirectIpClient sbdDirectIpClient = (SbdDirectIpClient) commLink;
          if (!(e.value == "Remove"))
            break;
          if (sbdDirectIpClient.Sending)
          {
            this.cntrlMsgHandler.SendInfMessage("SBD DirectIP Client: Cannot be removed while sending\r\n");
            break;
          }
          this.commLinks.Remove((CommLink) sbdDirectIpClient);
          this.cntrlMsgHandler.SendRemMessage(sbdDirectIpClient.ID);
          break;
      }
    }

    private CommLink GetCommLink(string id)
    {
      CommLink commLink = (CommLink) null;
      for (int index = 0; index < this.commLinks.Count; ++index)
      {
        if (this.commLinks[index].ID == id)
        {
          commLink = this.commLinks[index];
          break;
        }
      }
      return commLink;
    }

    private LogFile GetLogFile(string file)
    {
      LogFile logFile = (LogFile) null;
      for (int index = 0; index < this.logFiles.Count; ++index)
      {
        if (this.logFiles[index].File == file)
        {
          logFile = this.logFiles[index];
          break;
        }
      }
      return logFile;
    }

    private LoggingDirectory GetLoggingDirectory(string directory)
    {
      LoggingDirectory loggingDirectory = (LoggingDirectory) null;
      for (int index = 0; index < this.loggingDirectories.Count; ++index)
      {
        if (this.loggingDirectories[index].Directory == directory)
        {
          loggingDirectory = this.loggingDirectories[index];
          break;
        }
      }
      return loggingDirectory;
    }

    private BatchFile GetBatchFile(string file)
    {
      BatchFile batchFile = (BatchFile) null;
      for (int index = 0; index < this.batchFiles.Count; ++index)
      {
        if (this.batchFiles[index].File == file)
        {
          batchFile = this.batchFiles[index];
          break;
        }
      }
      return batchFile;
    }

    private Plugin GetPlugin(string file)
    {
      Plugin plugin = (Plugin) null;
      for (int index = 0; index < this.plugins.Count; ++index)
      {
        if (this.plugins[index].File == file)
        {
          plugin = this.plugins[index];
          break;
        }
      }
      return plugin;
    }

    private RURequestInfo GetRURequestInfoBeingSent(object sender)
    {
      foreach (KeyValuePair<object, RURequestInfo> keyValuePair in this.ruRequestsBeingSent)
      {
        if (keyValuePair.Key == sender)
          return keyValuePair.Value;
      }
      return (RURequestInfo) null;
    }

    private void SaveRURequestInfo(RURequestInfo requestInfo)
    {
      requestInfo.TimeSent = DateTime.UtcNow;
      this.ruPairs.Add(new RUPair()
      {
        RequestInfo = requestInfo
      });
      try
      {
        RUPairsFile.Save(Nal.ServerForTrackers.Common.Utils.GetRUPairsFileName((string) null), this.ruPairs);
      }
      catch
      {
      }
    }

    private void Log(string toLog, HiddenForm.LogDest logDestination, HiddenForm.LogOpt logOption)
    {
      if (logDestination == HiddenForm.LogDest.Controller || logDestination == HiddenForm.LogDest.Both)
      {
        this.controllerLogBuffer.Append(toLog);
        if (logOption == HiddenForm.LogOpt.FlushController || logOption == HiddenForm.LogOpt.FlushBoth)
        {
          this.cntrlMsgHandler.SendInfMessage(this.controllerLogBuffer.ToString());
          this.controllerLogBuffer.Length = 0;
        }
      }
      if (logDestination != HiddenForm.LogDest.File && logDestination != HiddenForm.LogDest.Both)
        return;
      this.fileLogBuffer.Append(toLog);
      if (logOption != HiddenForm.LogOpt.FlushFile && logOption != HiddenForm.LogOpt.FlushBoth)
        return;
      try
      {
        this.fileLogBuffer.Insert(0, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] "));
        File.AppendAllText(Nal.ServerForTrackers.Common.Utils.GetServiceDataDirectory((string) null) + "\\ServiceLog.txt", this.fileLogBuffer.ToString());
      }
      catch (Exception ex)
      {
      }
      this.fileLogBuffer.Length = 0;
    }

    private void CleanupCommLink(CommLink commLink)
    {
      switch (commLink)
      {
        case SbdDirectIpClientHandler _:
          ((SbdDirectIpClientHandler) commLink).Disconnect();
          break;
        case RudicsClient _:
          ((RudicsConnection) commLink).Disconnect();
          break;
        case RudicsClientHandler _:
          ((RudicsConnection) commLink).Disconnect();
          break;
        case RudicsAccount _:
          RudicsAccount rudicsAccount = (RudicsAccount) commLink;
          if (!rudicsAccount.Listening)
            break;
          rudicsAccount.StopListening();
          break;
        case SbdDirectIpAccount _:
          SbdDirectIpAccount sbdDirectIpAccount = (SbdDirectIpAccount) commLink;
          if (!sbdDirectIpAccount.Listening)
            break;
          sbdDirectIpAccount.StopListening();
          break;
        case ServerModem _:
          ServerModem serverModem = (ServerModem) commLink;
          if (!serverModem.PortOpened)
            break;
          serverModem.Close();
          break;
        case EmailAccount _:
          EmailAccount emailAccount = (EmailAccount) commLink;
          if (emailAccount.AutoRetrieve)
            emailAccount.AutoRetrieve = false;
          if (!emailAccount.Retrieving())
            break;
          emailAccount.AbortRetrieve();
          break;
      }
    }

    private void LoadRemoteUpdates()
    {
      try
      {
        this.ruPairs = RUPairsFile.Load(Nal.ServerForTrackers.Common.Utils.GetRUPairsFileName((string) null));
      }
      catch (XmlException ex)
      {
        this.Log("Failed to load the remote updates file. XML Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
        return;
      }
      catch (Exception ex)
      {
        this.Log("Failed to load the remote updates file. Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
        return;
      }
      this.Log("Loaded " + (object) this.ruPairs.Count + " entries from the remote updates file.\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
    }

    private void LoadSettings()
    {
      ServiceSettings serviceSettings = new ServiceSettings();
      try
      {
        serviceSettings.Load(Nal.ServerForTrackers.Common.Utils.GetServiceDataDirectory((string) null) + "\\ServiceSettings.xml");
      }
      catch (XmlException ex)
      {
        this.Log("Failed to update settings. XML Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
        return;
      }
      catch (Exception ex)
      {
        this.Log("Failed to update settings. Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
        return;
      }
      this.Log("Updating Settings...\r\n--------------------------------------------------\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
      int num1 = 45;
      foreach (EmailAccountInfo emailAccount1 in serviceSettings.EmailAccounts)
      {
        EmailAccount commLink = (EmailAccount) this.GetCommLink(emailAccount1.ID);
        bool flag;
        if (commLink != null)
        {
          string str1 = string.Empty;
          if (commLink.DisplayName != emailAccount1.DisplayName)
          {
            commLink.DisplayName = emailAccount1.DisplayName;
            str1 = str1 + "  Display name".PadRight(num1) + commLink.DisplayName + "\r\n";
          }
          if (commLink.AutoRetrieve != emailAccount1.AutoRetrieve)
          {
            commLink.AutoRetrieve = emailAccount1.AutoRetrieve;
            string str2 = str1;
            string str3 = "  Auto retrieve".PadRight(num1);
            flag = commLink.AutoRetrieve;
            string str4 = flag.ToString();
            str1 = str2 + str3 + str4 + "\r\n";
          }
          if (commLink.AutoRetrieveFrequency != emailAccount1.AutoRetrieveFrequency)
          {
            commLink.AutoRetrieveFrequency = emailAccount1.AutoRetrieveFrequency;
            str1 = str1 + "  Auto retrieve frequency".PadRight(num1) + (object) commLink.AutoRetrieveFrequency + "\r\n";
          }
          if (commLink.Pop3Server != emailAccount1.Pop3Server)
          {
            commLink.Pop3Server = emailAccount1.Pop3Server;
            str1 = str1 + "  POP3 server".PadRight(num1) + commLink.Pop3Server + "\r\n";
          }
          if (commLink.Pop3Port != emailAccount1.Pop3Port)
          {
            commLink.Pop3Port = emailAccount1.Pop3Port;
            str1 = str1 + "  POP3 port".PadRight(num1) + (object) commLink.Pop3Port + "\r\n";
          }
          if (commLink.Pop3UserName != emailAccount1.Pop3UserName)
          {
            commLink.Pop3UserName = emailAccount1.Pop3UserName;
            str1 = str1 + "  POP3 user name".PadRight(num1) + commLink.Pop3UserName + "\r\n";
          }
          if (commLink.Pop3Password != emailAccount1.Pop3Password)
          {
            commLink.Pop3Password = emailAccount1.Pop3Password;
            str1 = str1 + "  POP3 Password".PadRight(num1) + commLink.Pop3Password + "\r\n";
          }
          if (commLink.Pop3UseSsl != emailAccount1.Pop3UseSsl)
          {
            commLink.Pop3UseSsl = emailAccount1.Pop3UseSsl;
            string str5 = str1;
            string str6 = "  POP3 use SSL".PadRight(num1);
            flag = commLink.Pop3UseSsl;
            string str7 = flag.ToString();
            str1 = str5 + str6 + str7 + "\r\n";
          }
          if (commLink.Pop3SizeFilter != emailAccount1.Pop3SizeFilter)
          {
            commLink.Pop3SizeFilter = emailAccount1.Pop3SizeFilter;
            str1 = str1 + "  POP3 size filter".PadRight(num1) + (object) commLink.Pop3SizeFilter + "\r\n";
          }
          if (commLink.DeleteMailOnServer != emailAccount1.DeleteMailOnServer)
          {
            commLink.DeleteMailOnServer = emailAccount1.DeleteMailOnServer;
            string str8 = str1;
            string str9 = "  POP3 delete mail".PadRight(num1);
            flag = commLink.DeleteMailOnServer;
            string str10 = flag.ToString();
            str1 = str8 + str9 + str10 + "\r\n";
          }
          if (commLink.DeleteAll != emailAccount1.DeleteAll)
          {
            commLink.DeleteAll = emailAccount1.DeleteAll;
            string str11 = str1;
            string str12 = "  POP3 delete all".PadRight(num1);
            flag = commLink.DeleteAll;
            string str13 = flag.ToString();
            str1 = str11 + str12 + str13 + "\r\n";
          }
          if (commLink.SmtpServer != emailAccount1.SmtpServer)
          {
            commLink.SmtpServer = emailAccount1.SmtpServer;
            str1 = str1 + "  SMTP server".PadRight(num1) + commLink.SmtpServer + "\r\n";
          }
          if (commLink.SmtpPort != emailAccount1.SmtpPort)
          {
            commLink.SmtpPort = emailAccount1.SmtpPort;
            str1 = str1 + "  SMTP port".PadRight(num1) + (object) commLink.SmtpPort + "\r\n";
          }
          if (commLink.FromAddress != emailAccount1.FromAddress)
          {
            commLink.FromAddress = emailAccount1.FromAddress;
            str1 = str1 + "  SMTP from address".PadRight(num1) + commLink.FromAddress + "\r\n";
          }
          if (commLink.SmtpRequiresAuthentication != emailAccount1.SmtpRequiresAuthentication)
          {
            commLink.SmtpRequiresAuthentication = emailAccount1.SmtpRequiresAuthentication;
            string str14 = str1;
            string str15 = "  SMTP requires authentication".PadRight(num1);
            flag = commLink.SmtpRequiresAuthentication;
            string str16 = flag.ToString();
            str1 = str14 + str15 + str16 + "\r\n";
          }
          if (commLink.SmtpUsePop3Credentials != emailAccount1.SmtpUsePop3Credentials)
          {
            commLink.SmtpUsePop3Credentials = emailAccount1.SmtpUsePop3Credentials;
            string str17 = str1;
            string str18 = "  SMTP use POP3 credentials".PadRight(num1);
            flag = commLink.SmtpUsePop3Credentials;
            string str19 = flag.ToString();
            str1 = str17 + str18 + str19 + "\r\n";
          }
          if (commLink.SmtpUserName != emailAccount1.SmtpUserName)
          {
            commLink.SmtpUserName = emailAccount1.SmtpUserName;
            str1 = str1 + "  SMTP user name".PadRight(num1) + commLink.SmtpUserName + "\r\n";
          }
          if (commLink.SmtpPassword != emailAccount1.SmtpPassword)
          {
            commLink.SmtpPassword = emailAccount1.SmtpPassword;
            str1 = str1 + "  SMTP password".PadRight(num1) + commLink.SmtpPassword + "\r\n";
          }
          if (str1 != string.Empty)
            this.Log("Updated " + commLink.Name + "\r\n" + str1, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          EmailAccount emailAccount2 = new EmailAccount();
          emailAccount2.ID = emailAccount1.ID;
          emailAccount2.DisplayName = emailAccount1.DisplayName;
          emailAccount2.AutoRetrieve = emailAccount1.AutoRetrieve;
          emailAccount2.AutoRetrieveFrequency = emailAccount1.AutoRetrieveFrequency;
          emailAccount2.Pop3Server = emailAccount1.Pop3Server;
          emailAccount2.Pop3Port = emailAccount1.Pop3Port;
          emailAccount2.Pop3UserName = emailAccount1.Pop3UserName;
          emailAccount2.Pop3Password = emailAccount1.Pop3Password;
          emailAccount2.Pop3UseSsl = emailAccount1.Pop3UseSsl;
          emailAccount2.Pop3SizeFilter = emailAccount1.Pop3SizeFilter;
          emailAccount2.DeleteMailOnServer = emailAccount1.DeleteMailOnServer;
          emailAccount2.DeleteAll = emailAccount1.DeleteAll;
          emailAccount2.SmtpServer = emailAccount1.SmtpServer;
          emailAccount2.SmtpPort = emailAccount1.SmtpPort;
          emailAccount2.FromAddress = emailAccount1.FromAddress;
          emailAccount2.SmtpRequiresAuthentication = emailAccount1.SmtpRequiresAuthentication;
          emailAccount2.SmtpUsePop3Credentials = emailAccount1.SmtpUsePop3Credentials;
          emailAccount2.SmtpUserName = emailAccount1.SmtpUserName;
          emailAccount2.SmtpPassword = emailAccount1.SmtpPassword;
          emailAccount2.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
          emailAccount2.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
          emailAccount2.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
          emailAccount2.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
          emailAccount2.SendCompleted += new EventHandler(this.OnEmailAccountSendCompleted);
          this.commLinks.Add((CommLink) emailAccount2);
          this.cntrlMsgHandler.SendAddMessage(emailAccount2.ID, emailAccount2.Type, emailAccount2.Name, emailAccount2.Status, emailAccount2.Activity);
          this.Log("Added " + emailAccount2.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Display name".PadRight(num1) + emailAccount2.DisplayName + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str20 = "  Auto retrieve".PadRight(num1);
          flag = emailAccount2.AutoRetrieve;
          string str21 = flag.ToString();
          this.Log(str20 + str21 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Auto retrieve frequency".PadRight(num1) + (object) emailAccount2.AutoRetrieveFrequency + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  POP3 server".PadRight(num1) + emailAccount2.Pop3Server + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  POP3 port".PadRight(num1) + (object) emailAccount2.Pop3Port + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  POP3 user name".PadRight(num1) + emailAccount2.Pop3UserName + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  POP3 password".PadRight(num1) + emailAccount2.Pop3Password + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str22 = "  POP3 use SSL".PadRight(num1);
          flag = emailAccount2.Pop3UseSsl;
          string str23 = flag.ToString();
          this.Log(str22 + str23 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  POP3 size filter".PadRight(num1) + (object) emailAccount2.Pop3SizeFilter + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str24 = "  POP3 delete mail".PadRight(num1);
          flag = emailAccount2.DeleteMailOnServer;
          string str25 = flag.ToString();
          this.Log(str24 + str25 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str26 = "  POP3 delete all".PadRight(num1);
          flag = emailAccount2.DeleteAll;
          string str27 = flag.ToString();
          this.Log(str26 + str27 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  SMTP server".PadRight(num1) + emailAccount2.SmtpServer + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  SMTP port".PadRight(num1) + (object) emailAccount2.SmtpPort + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  SMTP from address".PadRight(num1) + emailAccount2.FromAddress + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str28 = "  SMTP requires authentication".PadRight(num1);
          flag = emailAccount2.SmtpRequiresAuthentication;
          string str29 = flag.ToString();
          this.Log(str28 + str29 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str30 = "  SMTP use POP3 credentials".PadRight(num1);
          flag = emailAccount2.SmtpUsePop3Credentials;
          string str31 = flag.ToString();
          this.Log(str30 + str31 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  SMTP user name".PadRight(num1) + emailAccount2.SmtpUserName + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  SMTP password".PadRight(num1) + emailAccount2.SmtpPassword + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      foreach (ServerModemInfo serverModem1 in serviceSettings.ServerModems)
      {
        ServerModem commLink = (ServerModem) this.GetCommLink(serverModem1.ID);
        bool flag1;
        if (commLink != null)
        {
          string str32 = string.Empty;
          int num2 = commLink.PortOpened != serverModem1.OpenPort ? 1 : 0;
          bool flag2 = num2 != 0 && commLink.PortOpened;
          bool flag3 = num2 != 0 && !commLink.PortOpened;
          bool flag4 = num2 == 0 && commLink.PortOpened && (commLink.Port != serverModem1.Port || commLink.PortBitsPerSecond != serverModem1.PortBitsPerSec || commLink.PortStopBits != serverModem1.PortStopBits || commLink.PortParity != serverModem1.PortParity || commLink.PortDataBits != serverModem1.PortDataBits);
          if (flag2 | flag4)
          {
            string str33 = str32 + "Closing the port...".PadRight(num1);
            try
            {
              commLink.Close();
              str32 = str33 + "Success\r\n";
            }
            catch (Exception ex)
            {
              str32 = str33 + "Failed - Exception: " + ex.Message + "\r\n";
            }
          }
          if (commLink.Port != serverModem1.Port)
          {
            commLink.Port = serverModem1.Port;
            str32 = str32 + "  Port".PadRight(num1) + commLink.Port + "\r\n";
          }
          if (commLink.PortBitsPerSecond != serverModem1.PortBitsPerSec)
          {
            commLink.PortBitsPerSecond = serverModem1.PortBitsPerSec;
            str32 = str32 + "  Port bits per sec".PadRight(num1) + (object) commLink.PortBitsPerSecond + "\r\n";
          }
          if (commLink.PortStopBits != serverModem1.PortStopBits)
          {
            commLink.PortStopBits = serverModem1.PortStopBits;
            str32 = str32 + "  Port stop bits".PadRight(num1) + (object) commLink.PortStopBits + "\r\n";
          }
          if (commLink.PortParity != serverModem1.PortParity)
          {
            commLink.PortParity = serverModem1.PortParity;
            str32 = str32 + "  Port parity".PadRight(num1) + (object) commLink.PortParity + "\r\n";
          }
          if (commLink.PortDataBits != serverModem1.PortDataBits)
          {
            commLink.PortDataBits = serverModem1.PortDataBits;
            str32 = str32 + "  Port data bits".PadRight(num1) + (object) commLink.PortDataBits + "\r\n";
          }
          if (flag3 | flag4)
          {
            string str34 = str32 + "Opening the port...".PadRight(num1);
            try
            {
              commLink.Open();
              str32 = str34 + "Success\r\n";
            }
            catch (Exception ex)
            {
              str32 = str34 + "Failed - Exception: " + ex.Message + "\r\n";
            }
          }
          if (commLink.ReceiveSms != serverModem1.RecvSms)
          {
            commLink.ReceiveSms = serverModem1.RecvSms;
            string str35 = str32;
            string str36 = "  Receive SMS".PadRight(num1);
            flag1 = commLink.ReceiveSms;
            string str37 = flag1.ToString();
            str32 = str35 + str36 + str37 + "\r\n";
          }
          if (commLink.ManageConn != serverModem1.ManageConn)
          {
            commLink.ManageConn = serverModem1.ManageConn;
            string str38 = str32;
            string str39 = "  Manage connection".PadRight(num1);
            flag1 = commLink.ManageConn;
            string str40 = flag1.ToString();
            str32 = str38 + str39 + str40 + "\r\n";
          }
          if (commLink.AutoAnswer != serverModem1.AutoAnswer)
          {
            commLink.AutoAnswer = serverModem1.AutoAnswer;
            string str41 = str32;
            string str42 = "  Auto answer".PadRight(num1);
            flag1 = commLink.AutoAnswer;
            string str43 = flag1.ToString();
            str32 = str41 + str42 + str43 + "\r\n";
          }
          if (commLink.CallProtocol != serverModem1.CallProtocol)
          {
            commLink.CallProtocol = serverModem1.CallProtocol;
            str32 = str32 + "  Call protocol".PadRight(num1) + (object) commLink.CallProtocol + "\r\n";
          }
          if (str32 != string.Empty)
            this.Log("Updated " + commLink.Name + "\r\n" + str32, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          ServerModem serverModem2 = new ServerModem();
          serverModem2.ID = serverModem1.ID;
          serverModem2.Port = serverModem1.Port;
          serverModem2.PortBitsPerSecond = serverModem1.PortBitsPerSec;
          serverModem2.PortStopBits = serverModem1.PortStopBits;
          serverModem2.PortParity = serverModem1.PortParity;
          serverModem2.PortDataBits = serverModem1.PortDataBits;
          serverModem2.ReceiveSms = serverModem1.RecvSms;
          serverModem2.ManageConn = serverModem1.ManageConn;
          serverModem2.AutoAnswer = serverModem1.AutoAnswer;
          serverModem2.CallProtocol = serverModem1.CallProtocol;
          serverModem2.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
          serverModem2.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
          serverModem2.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
          serverModem2.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
          this.commLinks.Add((CommLink) serverModem2);
          this.cntrlMsgHandler.SendAddMessage(serverModem2.ID, serverModem2.Type, serverModem2.Name, serverModem2.Status, serverModem2.Activity);
          this.Log("Added " + serverModem2.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Port".PadRight(num1) + serverModem2.Port + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Port bits per sec".PadRight(num1) + (object) serverModem2.PortBitsPerSecond + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Port stop bits".PadRight(num1) + (object) serverModem2.PortStopBits + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Port parity".PadRight(num1) + (object) serverModem2.PortParity + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Port data bits".PadRight(num1) + (object) serverModem2.PortDataBits + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str44 = "  Receive SMS".PadRight(num1);
          flag1 = serverModem2.ReceiveSms;
          string str45 = flag1.ToString();
          this.Log(str44 + str45 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str46 = "  Manage connection".PadRight(num1);
          flag1 = serverModem2.ManageConn;
          string str47 = flag1.ToString();
          this.Log(str46 + str47 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str48 = "  Auto answer".PadRight(num1);
          flag1 = serverModem2.AutoAnswer;
          string str49 = flag1.ToString();
          this.Log(str48 + str49 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Call protocol".PadRight(num1) + (object) serverModem2.CallProtocol + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          if (serverModem1.OpenPort)
          {
            this.Log("  Opening the port...".PadRight(num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            try
            {
              serverModem2.Open();
              this.Log("Success\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
            catch (Exception ex)
            {
              this.Log("Failed - Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
          }
        }
      }
      foreach (SbdDirectIPAccountInfo sbdDirectIpAccount1 in serviceSettings.SbdDirectIpAccounts)
      {
        SbdDirectIpAccount commLink = (SbdDirectIpAccount) this.GetCommLink(sbdDirectIpAccount1.ID);
        bool ipAcknowledgement;
        if (commLink != null)
        {
          string str50 = string.Empty;
          if (commLink.DisplayName != sbdDirectIpAccount1.DisplayName)
          {
            commLink.DisplayName = sbdDirectIpAccount1.DisplayName;
            str50 = str50 + "  Display name".PadRight(num1) + sbdDirectIpAccount1.DisplayName + "\r\n";
          }
          int num3 = commLink.Listening != sbdDirectIpAccount1.ServerListen ? 1 : 0;
          bool flag5 = num3 != 0 && commLink.Listening;
          bool flag6 = num3 != 0 && !commLink.Listening;
          bool flag7 = num3 == 0 && commLink.Listening && commLink.ServerPort != sbdDirectIpAccount1.ServerPort;
          if (flag7 | flag5)
          {
            string str51 = str50 + "  Shutting down listener...".PadRight(num1);
            commLink.StopListening();
            str50 = str51 + "Success\r\n";
          }
          if (commLink.ServerPort != sbdDirectIpAccount1.ServerPort)
          {
            commLink.ServerPort = sbdDirectIpAccount1.ServerPort;
            str50 = str50 + "  Server port".PadRight(num1) + (object) commLink.ServerPort + "\r\n";
          }
          if (flag7 | flag6)
          {
            try
            {
              str50 += "  Starting up listener...".PadRight(num1);
              commLink.StartListening();
              str50 += "Success\r\n";
            }
            catch (Exception ex)
            {
              str50 = str50 + "Failed - Exception: " + ex.Message + "\r\n";
            }
          }
          if (commLink.ClientHost != sbdDirectIpAccount1.ClientHost)
          {
            commLink.ClientHost = sbdDirectIpAccount1.ClientHost;
            str50 = str50 + "  Client host".PadRight(num1) + commLink.ClientHost + "\r\n";
          }
          if (commLink.ClientPort != sbdDirectIpAccount1.ClientPort)
          {
            commLink.ClientPort = sbdDirectIpAccount1.ClientPort;
            str50 = str50 + "  Client port".PadRight(num1) + (object) commLink.ClientPort + "\r\n";
          }
          if (commLink.SendMODirectIPAcknowledgement != sbdDirectIpAccount1.SendMODirectIPAcknowledgement)
          {
            commLink.SendMODirectIPAcknowledgement = sbdDirectIpAccount1.SendMODirectIPAcknowledgement;
            string str52 = str50;
            string str53 = "  Send MO Direct IP acknowledgement".PadRight(num1);
            ipAcknowledgement = commLink.SendMODirectIPAcknowledgement;
            string str54 = ipAcknowledgement.ToString();
            str50 = str52 + str53 + str54 + "\r\n";
          }
          if (str50 != string.Empty)
            this.Log("Updated " + commLink.Name + "\r\n" + str50, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          SbdDirectIpAccount sbdDirectIpAccount2 = new SbdDirectIpAccount();
          sbdDirectIpAccount2.ID = sbdDirectIpAccount1.ID;
          sbdDirectIpAccount2.DisplayName = sbdDirectIpAccount1.DisplayName;
          sbdDirectIpAccount2.ServerPort = sbdDirectIpAccount1.ServerPort;
          sbdDirectIpAccount2.ClientHost = sbdDirectIpAccount1.ClientHost;
          sbdDirectIpAccount2.ClientPort = sbdDirectIpAccount1.ClientPort;
          sbdDirectIpAccount2.SendMODirectIPAcknowledgement = sbdDirectIpAccount1.SendMODirectIPAcknowledgement;
          sbdDirectIpAccount2.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
          sbdDirectIpAccount2.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
          sbdDirectIpAccount2.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
          sbdDirectIpAccount2.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
          sbdDirectIpAccount2.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnSbdDirectIpAccountClientConnected);
          this.commLinks.Add((CommLink) sbdDirectIpAccount2);
          this.cntrlMsgHandler.SendAddMessage(sbdDirectIpAccount2.ID, sbdDirectIpAccount2.Type, sbdDirectIpAccount2.Name, sbdDirectIpAccount2.Status, sbdDirectIpAccount2.Activity);
          this.Log("Added " + sbdDirectIpAccount2.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Display name".PadRight(num1) + sbdDirectIpAccount2.DisplayName + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Server port".PadRight(num1) + (object) sbdDirectIpAccount2.ServerPort + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client host".PadRight(num1) + sbdDirectIpAccount2.ClientHost + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client port".PadRight(num1) + (object) sbdDirectIpAccount2.ClientPort + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str55 = "  Send MO Direct IP acknowledgement".PadRight(num1);
          ipAcknowledgement = sbdDirectIpAccount2.SendMODirectIPAcknowledgement;
          string str56 = ipAcknowledgement.ToString();
          this.Log(str55 + str56 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          if (sbdDirectIpAccount1.ServerListen)
          {
            this.Log("  Starting up listener...".PadRight(num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            try
            {
              sbdDirectIpAccount2.StartListening();
              this.Log("Success\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
            catch (Exception ex)
            {
              this.Log("Failed - Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
          }
        }
      }
      foreach (TcpProtocolServerInfo tcpProtocolServer1 in serviceSettings.TcpProtocolServers)
      {
        TcpProtocolServer commLink = (TcpProtocolServer) this.GetCommLink(tcpProtocolServer1.ID);
        if (commLink != null)
        {
          string str57 = string.Empty;
          if (commLink.DisplayName != tcpProtocolServer1.DisplayName)
          {
            commLink.DisplayName = tcpProtocolServer1.DisplayName;
            str57 = str57 + "  Display name".PadRight(num1) + tcpProtocolServer1.DisplayName + "\r\n";
          }
          int num4 = commLink.Listening != tcpProtocolServer1.ServerListen ? 1 : 0;
          bool flag8 = num4 != 0 && commLink.Listening;
          bool flag9 = num4 != 0 && !commLink.Listening;
          bool flag10 = num4 == 0 && commLink.Listening && commLink.ServerPort != tcpProtocolServer1.ServerPort;
          if (flag10 | flag8)
          {
            string str58 = str57 + "  Shutting down listener...".PadRight(num1);
            commLink.StopListening();
            str57 = str58 + "Success\r\n";
          }
          if (commLink.ServerPort != tcpProtocolServer1.ServerPort)
          {
            commLink.ServerPort = tcpProtocolServer1.ServerPort;
            str57 = str57 + "  Server port".PadRight(num1) + (object) commLink.ServerPort + "\r\n";
          }
          if (flag10 | flag9)
          {
            try
            {
              str57 += "  Starting up listener...".PadRight(num1);
              commLink.StartListening();
              str57 += "Success\r\n";
            }
            catch (Exception ex)
            {
              str57 = str57 + "Failed - Exception: " + ex.Message + "\r\n";
            }
          }
          if (str57 != string.Empty)
            this.Log("Updated " + commLink.Name + "\r\n" + str57, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          TcpProtocolServer tcpProtocolServer2 = new TcpProtocolServer();
          tcpProtocolServer2.ID = tcpProtocolServer1.ID;
          tcpProtocolServer2.DisplayName = tcpProtocolServer1.DisplayName;
          tcpProtocolServer2.ServerPort = tcpProtocolServer1.ServerPort;
          tcpProtocolServer2.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
          tcpProtocolServer2.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
          tcpProtocolServer2.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
          tcpProtocolServer2.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
          tcpProtocolServer2.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnTcpProtocolServerClientConnected);
          this.commLinks.Add((CommLink) tcpProtocolServer2);
          this.cntrlMsgHandler.SendAddMessage(tcpProtocolServer2.ID, tcpProtocolServer2.Type, tcpProtocolServer2.Name, tcpProtocolServer2.Status, tcpProtocolServer2.Activity);
          this.Log("Added " + tcpProtocolServer2.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Display name".PadRight(num1) + tcpProtocolServer2.DisplayName + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Server port".PadRight(num1) + (object) tcpProtocolServer2.ServerPort + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          if (tcpProtocolServer1.ServerListen)
          {
            this.Log("  Starting up listener...".PadRight(num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            try
            {
              tcpProtocolServer2.StartListening();
              this.Log("Success\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
            catch (Exception ex)
            {
              this.Log("Failed - Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
          }
        }
      }
      foreach (RudicsAccountInfo rudicsAccount1 in serviceSettings.RudicsAccounts)
      {
        RudicsAccount commLink = (RudicsAccount) this.GetCommLink(rudicsAccount1.ID);
        bool enablePipe;
        if (commLink != null)
        {
          string str59 = string.Empty;
          if (commLink.DisplayName != rudicsAccount1.DisplayName)
          {
            commLink.DisplayName = rudicsAccount1.DisplayName;
            str59 = str59 + "  Display name".PadRight(num1) + commLink.DisplayName + "\r\n";
          }
          if (commLink.EnablePipe != rudicsAccount1.EnablePipe)
          {
            commLink.EnablePipe = rudicsAccount1.EnablePipe;
            string str60 = str59;
            string str61 = "  Enable pipe".PadRight(num1);
            enablePipe = commLink.EnablePipe;
            string str62 = enablePipe.ToString();
            str59 = str60 + str61 + str62 + "\r\n";
          }
          if (commLink.PipeHost != rudicsAccount1.PipeHost)
          {
            commLink.PipeHost = rudicsAccount1.PipeHost;
            str59 = str59 + "  Pipe host".PadRight(num1) + commLink.PipeHost + "\r\n";
          }
          if (commLink.PipePort != rudicsAccount1.PipePort)
          {
            commLink.PipePort = rudicsAccount1.PipePort;
            str59 = str59 + "  Pipe port".PadRight(num1) + (object) commLink.PipePort + "\r\n";
          }
          int num5 = commLink.Listening != rudicsAccount1.ServerListen ? 1 : 0;
          bool flag11 = num5 != 0 && commLink.Listening;
          bool flag12 = num5 != 0 && !commLink.Listening;
          bool flag13 = num5 == 0 && commLink.Listening && commLink.ServerPort != rudicsAccount1.ServerPort;
          if (flag13 | flag11)
          {
            string str63 = str59 + "  Shutting down listener...".PadRight(num1);
            commLink.StopListening();
            str59 = str63 + "Success\r\n";
          }
          if (commLink.ServerPort != rudicsAccount1.ServerPort)
          {
            commLink.ServerPort = rudicsAccount1.ServerPort;
            str59 = str59 + "  Server port".PadRight(num1) + (object) commLink.ServerPort + "\r\n";
          }
          if (flag13 | flag12)
          {
            try
            {
              str59 += "  Starting up listener...".PadRight(num1);
              commLink.StartListening();
              str59 += "Success\r\n";
            }
            catch (Exception ex)
            {
              str59 = str59 + "Failed - Exception: " + ex.Message + "\r\n";
            }
          }
          if (commLink.ClientHandlerCallProtocol != rudicsAccount1.ClientHandlerCallProtocol)
          {
            commLink.ClientHandlerCallProtocol = rudicsAccount1.ClientHandlerCallProtocol;
            str59 = str59 + "  Client handler call protocol".PadRight(num1) + (object) commLink.ClientHandlerCallProtocol + "\r\n";
          }
          if (commLink.ClientHost != rudicsAccount1.ClientHost)
          {
            commLink.ClientHost = rudicsAccount1.ClientHost;
            str59 = str59 + "  Client host".PadRight(num1) + commLink.ClientHost + "\r\n";
          }
          if (commLink.ClientPortsBegin != rudicsAccount1.ClientPortsBegin)
          {
            commLink.ClientPortsBegin = rudicsAccount1.ClientPortsBegin;
            str59 = str59 + "  Client ports begin".PadRight(num1) + (object) commLink.ClientPortsBegin + "\r\n";
          }
          if (commLink.ClientPortsEnd != rudicsAccount1.ClientPortsEnd)
          {
            commLink.ClientPortsEnd = rudicsAccount1.ClientPortsEnd;
            str59 = str59 + "  Client ports end".PadRight(num1) + (object) commLink.ClientPortsEnd + "\r\n";
          }
          if (commLink.ClientCallProtocol != rudicsAccount1.ClientCallProtocol)
          {
            commLink.ClientCallProtocol = rudicsAccount1.ClientCallProtocol;
            str59 = str59 + "  Client call protocol".PadRight(num1) + (object) commLink.ClientCallProtocol + "\r\n";
          }
          if (str59 != string.Empty)
            this.Log("Updated " + commLink.Name + "\r\n" + str59, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          RudicsAccount rudicsAccount2 = new RudicsAccount();
          rudicsAccount2.ID = rudicsAccount1.ID;
          rudicsAccount2.DisplayName = rudicsAccount1.DisplayName;
          rudicsAccount2.EnablePipe = rudicsAccount1.EnablePipe;
          rudicsAccount2.PipeHost = rudicsAccount1.PipeHost;
          rudicsAccount2.PipePort = rudicsAccount1.PipePort;
          rudicsAccount2.ServerPort = rudicsAccount1.ServerPort;
          rudicsAccount2.ClientHandlerCallProtocol = rudicsAccount1.ClientHandlerCallProtocol;
          rudicsAccount2.ClientHost = rudicsAccount1.ClientHost;
          rudicsAccount2.ClientPortsBegin = rudicsAccount1.ClientPortsBegin;
          rudicsAccount2.ClientPortsEnd = rudicsAccount1.ClientPortsEnd;
          rudicsAccount2.ClientCallProtocol = rudicsAccount1.ClientCallProtocol;
          rudicsAccount2.StatusChanged += new EventHandler(this.OnCommLinkStatusChanged);
          rudicsAccount2.ActivityOccured += new EventHandler(this.OnCommLinkActivityOccured);
          rudicsAccount2.DataReceived += new CommLink.DataReceivedEventHandler(this.OnCommLinkDataReceived);
          rudicsAccount2.NameChanged += new EventHandler(this.OnCommLinkNameChanged);
          rudicsAccount2.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnRudicsAccountClientConnected);
          this.commLinks.Add((CommLink) rudicsAccount2);
          this.cntrlMsgHandler.SendAddMessage(rudicsAccount2.ID, rudicsAccount2.Type, rudicsAccount2.Name, rudicsAccount2.Status, rudicsAccount2.Activity);
          this.Log("Added " + rudicsAccount2.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Display name".PadRight(num1) + rudicsAccount2.DisplayName + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Server port".PadRight(num1) + (object) rudicsAccount2.ServerPort + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str64 = "  Enable pipe".PadRight(num1);
          enablePipe = rudicsAccount2.EnablePipe;
          string str65 = enablePipe.ToString();
          this.Log(str64 + str65 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Pipe host".PadRight(num1) + rudicsAccount2.PipeHost + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Pipe port".PadRight(num1) + (object) rudicsAccount2.PipePort + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client handler call protocol".PadRight(num1) + (object) rudicsAccount2.ClientHandlerCallProtocol + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client host".PadRight(num1) + rudicsAccount2.ClientHost + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client ports begin".PadRight(num1) + (object) rudicsAccount2.ClientPortsBegin + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client ports end".PadRight(num1) + (object) rudicsAccount2.ClientPortsEnd + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Client call protocol".PadRight(num1) + (object) rudicsAccount2.ClientCallProtocol + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          if (rudicsAccount1.ServerListen)
          {
            this.Log("  Starting up listener...".PadRight(num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            try
            {
              rudicsAccount2.StartListening();
              this.Log("Success\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
            catch (Exception ex)
            {
              this.Log("Failed - Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
          }
        }
      }
      for (int index = this.commLinks.Count - 1; index >= 0; --index)
      {
        CommLink commLink = this.commLinks[index];
        bool flag = false;
        switch (commLink)
        {
          case EmailAccount _:
            using (List<EmailAccountInfo>.Enumerator enumerator = serviceSettings.EmailAccounts.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                EmailAccountInfo current = enumerator.Current;
                if (commLink.ID == current.ID)
                {
                  flag = true;
                  break;
                }
              }
              break;
            }
          case ServerModem _:
            using (List<ServerModemInfo>.Enumerator enumerator = serviceSettings.ServerModems.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                ServerModemInfo current = enumerator.Current;
                if (commLink.ID == current.ID)
                {
                  flag = true;
                  break;
                }
              }
              break;
            }
          case SbdDirectIpAccount _:
            using (List<SbdDirectIPAccountInfo>.Enumerator enumerator = serviceSettings.SbdDirectIpAccounts.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                SbdDirectIPAccountInfo current = enumerator.Current;
                if (commLink.ID == current.ID)
                {
                  flag = true;
                  break;
                }
              }
              break;
            }
          case TcpProtocolServer _:
            using (List<TcpProtocolServerInfo>.Enumerator enumerator = serviceSettings.TcpProtocolServers.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                TcpProtocolServerInfo current = enumerator.Current;
                if (commLink.ID == current.ID)
                {
                  flag = true;
                  break;
                }
              }
              break;
            }
          case RudicsAccount _:
            using (List<RudicsAccountInfo>.Enumerator enumerator = serviceSettings.RudicsAccounts.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                RudicsAccountInfo current = enumerator.Current;
                if (commLink.ID == current.ID)
                {
                  flag = true;
                  break;
                }
              }
              break;
            }
          default:
            flag = true;
            break;
        }
        if (!flag)
        {
          this.commLinks.Remove(commLink);
          this.cntrlMsgHandler.SendRemMessage(commLink.ID);
          this.CleanupCommLink(commLink);
          this.Log("Removed " + commLink.Name + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      foreach (LogFile logFile1 in serviceSettings.LogFiles)
      {
        LogFile logFile2 = this.GetLogFile(logFile1.File);
        bool flag;
        if (logFile2 != null)
        {
          string str66 = string.Empty;
          if (logFile2.File != logFile1.File)
          {
            logFile2.File = logFile1.File;
            str66 = str66 + "  File".PadRight(num1) + logFile2.File + "\r\n";
          }
          if (logFile2.IncludeSbd != logFile1.IncludeSbd)
          {
            logFile2.IncludeSbd = logFile1.IncludeSbd;
            string str67 = str66;
            string str68 = "  Include SBD".PadRight(num1);
            flag = logFile2.IncludeSbd;
            string str69 = flag.ToString();
            str66 = str67 + str68 + str69 + "\r\n";
          }
          if (logFile2.IncludeSms != logFile1.IncludeSms)
          {
            logFile2.IncludeSms = logFile1.IncludeSms;
            string str70 = str66;
            string str71 = "  Include SMS".PadRight(num1);
            flag = logFile2.IncludeSms;
            string str72 = flag.ToString();
            str66 = str70 + str71 + str72 + "\r\n";
          }
          if (logFile2.IncludeCalls != logFile1.IncludeCalls)
          {
            logFile2.IncludeCalls = logFile1.IncludeCalls;
            string str73 = str66;
            string str74 = "  Include calls".PadRight(num1);
            flag = logFile2.IncludeCalls;
            string str75 = flag.ToString();
            str66 = str73 + str74 + str75 + "\r\n";
          }
          if (logFile2.IncludeTcp != logFile1.IncludeTcp)
          {
            logFile2.IncludeTcp = logFile1.IncludeTcp;
            string str76 = str66;
            string str77 = "  Include TCP".PadRight(num1);
            flag = logFile2.IncludeTcp;
            string str78 = flag.ToString();
            str66 = str76 + str77 + str78 + "\r\n";
          }
          string str79 = str66 + this.LoadDataTypeSettingsList((IList<DataTypeSettings>) logFile2.DataTypeSettingsList, (IList<DataTypeSettings>) logFile1.DataTypeSettingsList, num1);
          if (str79 != string.Empty)
            this.Log("Updated Log File - " + logFile2.File + "\r\n" + str79, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          this.logFiles.Add(logFile1);
          this.Log("Added Log File\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  File".PadRight(num1) + logFile1.File + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str80 = "  Include SBD".PadRight(num1);
          flag = logFile1.IncludeSbd;
          string str81 = flag.ToString();
          this.Log(str80 + str81 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str82 = "  Include SMS".PadRight(num1);
          flag = logFile1.IncludeSms;
          string str83 = flag.ToString();
          this.Log(str82 + str83 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str84 = "  Include calls".PadRight(num1);
          flag = logFile1.IncludeCalls;
          string str85 = flag.ToString();
          this.Log(str84 + str85 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str86 = "  Include TCP".PadRight(num1);
          flag = logFile1.IncludeTcp;
          string str87 = flag.ToString();
          this.Log(str86 + str87 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log(this.LoadDataTypeSettingsList((IList<DataTypeSettings>) new List<DataTypeSettings>(), (IList<DataTypeSettings>) logFile1.DataTypeSettingsList, num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      for (int index = this.logFiles.Count - 1; index >= 0; --index)
      {
        LogFile logFile3 = this.logFiles[index];
        bool flag = false;
        foreach (LogFile logFile4 in serviceSettings.LogFiles)
        {
          if (logFile3.File == logFile4.File)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.logFiles.RemoveAt(index);
          this.Log("Removed Log File - " + logFile3.File + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      foreach (LoggingDirectory loggingDirectory1 in serviceSettings.LoggingDirectories)
      {
        LoggingDirectory loggingDirectory2 = this.GetLoggingDirectory(loggingDirectory1.Directory);
        bool flag;
        if (loggingDirectory2 != null)
        {
          string str88 = string.Empty;
          if (loggingDirectory2.FilePrefix != loggingDirectory1.FilePrefix)
          {
            loggingDirectory2.FilePrefix = loggingDirectory1.FilePrefix;
            str88 = str88 + "  File prefix".PadRight(num1) + loggingDirectory2.FilePrefix + "\r\n";
          }
          if (loggingDirectory2.IncludeSbd != loggingDirectory1.IncludeSbd)
          {
            loggingDirectory2.IncludeSbd = loggingDirectory1.IncludeSbd;
            string str89 = str88;
            string str90 = "  Include SBD".PadRight(num1);
            flag = loggingDirectory2.IncludeSbd;
            string str91 = flag.ToString();
            str88 = str89 + str90 + str91 + "\r\n";
          }
          if (loggingDirectory2.IncludeSms != loggingDirectory1.IncludeSms)
          {
            loggingDirectory2.IncludeSms = loggingDirectory1.IncludeSms;
            string str92 = str88;
            string str93 = "  Include SMS".PadRight(num1);
            flag = loggingDirectory2.IncludeSms;
            string str94 = flag.ToString();
            str88 = str92 + str93 + str94 + "\r\n";
          }
          if (loggingDirectory2.IncludeCalls != loggingDirectory1.IncludeCalls)
          {
            loggingDirectory2.IncludeCalls = loggingDirectory1.IncludeCalls;
            string str95 = str88;
            string str96 = "  Include calls".PadRight(num1);
            flag = loggingDirectory2.IncludeCalls;
            string str97 = flag.ToString();
            str88 = str95 + str96 + str97 + "\r\n";
          }
          if (loggingDirectory2.IncludeTcp != loggingDirectory1.IncludeTcp)
          {
            loggingDirectory2.IncludeTcp = loggingDirectory1.IncludeTcp;
            string str98 = str88;
            string str99 = "  Include TCP".PadRight(num1);
            flag = loggingDirectory2.IncludeTcp;
            string str100 = flag.ToString();
            str88 = str98 + str99 + str100 + "\r\n";
          }
          string str101 = str88 + this.LoadDataTypeSettingsList((IList<DataTypeSettings>) loggingDirectory2.DataTypeSettingsList, (IList<DataTypeSettings>) loggingDirectory1.DataTypeSettingsList, num1);
          if (str101 != string.Empty)
            this.Log("Updated Logging Directory - " + loggingDirectory2.Directory + "\r\n" + str101, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          this.loggingDirectories.Add(loggingDirectory1);
          this.Log("Added Logging Directory\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  Directory".PadRight(num1) + loggingDirectory1.Directory + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  File prefix".PadRight(num1) + loggingDirectory1.FilePrefix + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str102 = "  Include SBD".PadRight(num1);
          flag = loggingDirectory1.IncludeSbd;
          string str103 = flag.ToString();
          this.Log(str102 + str103 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str104 = "  Include SMS".PadRight(num1);
          flag = loggingDirectory1.IncludeSms;
          string str105 = flag.ToString();
          this.Log(str104 + str105 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str106 = "  Include calls".PadRight(num1);
          flag = loggingDirectory1.IncludeCalls;
          string str107 = flag.ToString();
          this.Log(str106 + str107 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str108 = "  Include TCP".PadRight(num1);
          flag = loggingDirectory1.IncludeTcp;
          string str109 = flag.ToString();
          this.Log(str108 + str109 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log(this.LoadDataTypeSettingsList((IList<DataTypeSettings>) new List<DataTypeSettings>(), (IList<DataTypeSettings>) loggingDirectory1.DataTypeSettingsList, num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      for (int index = this.loggingDirectories.Count - 1; index >= 0; --index)
      {
        LoggingDirectory loggingDirectory3 = this.loggingDirectories[index];
        bool flag = false;
        foreach (LoggingDirectory loggingDirectory4 in serviceSettings.LoggingDirectories)
        {
          if (loggingDirectory3.Directory == loggingDirectory4.Directory)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.loggingDirectories.RemoveAt(index);
          this.Log("Removed Logging Directory - " + loggingDirectory3.Directory + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      foreach (BatchFile batchFile1 in serviceSettings.BatchFiles)
      {
        BatchFile batchFile2 = this.GetBatchFile(batchFile1.File);
        bool flag;
        if (batchFile2 != null)
        {
          string str110 = string.Empty;
          if (batchFile2.File != batchFile1.File)
          {
            batchFile2.File = batchFile1.File;
            str110 = str110 + "  File".PadRight(num1) + batchFile2.File + "\r\n";
          }
          if (batchFile2.IncludeSbd != batchFile1.IncludeSbd)
          {
            batchFile2.IncludeSbd = batchFile1.IncludeSbd;
            string str111 = str110;
            string str112 = "  Include SBD".PadRight(num1);
            flag = batchFile2.IncludeSbd;
            string str113 = flag.ToString();
            str110 = str111 + str112 + str113 + "\r\n";
          }
          if (batchFile2.IncludeSms != batchFile1.IncludeSms)
          {
            batchFile2.IncludeSms = batchFile1.IncludeSms;
            string str114 = str110;
            string str115 = "  Include SMS".PadRight(num1);
            flag = batchFile2.IncludeSms;
            string str116 = flag.ToString();
            str110 = str114 + str115 + str116 + "\r\n";
          }
          if (batchFile2.IncludeCalls != batchFile1.IncludeCalls)
          {
            batchFile2.IncludeCalls = batchFile1.IncludeCalls;
            string str117 = str110;
            string str118 = "  Include calls".PadRight(num1);
            flag = batchFile2.IncludeCalls;
            string str119 = flag.ToString();
            str110 = str117 + str118 + str119 + "\r\n";
          }
          if (batchFile2.IncludeTcp != batchFile1.IncludeTcp)
          {
            batchFile2.IncludeTcp = batchFile1.IncludeTcp;
            string str120 = str110;
            string str121 = "  Include TCP".PadRight(num1);
            flag = batchFile2.IncludeTcp;
            string str122 = flag.ToString();
            str110 = str120 + str121 + str122 + "\r\n";
          }
          string str123 = str110 + this.LoadDataTypeSettingsList((IList<DataTypeSettings>) batchFile2.DataTypeSettingsList, (IList<DataTypeSettings>) batchFile1.DataTypeSettingsList, num1);
          if (str123 != string.Empty)
            this.Log("Updated Batch File - " + batchFile2.File + "\r\n" + str123, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          this.batchFiles.Add(batchFile1);
          this.Log("Added Batch File\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  File".PadRight(num1) + batchFile1.File + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str124 = "  Include SBD".PadRight(num1);
          flag = batchFile1.IncludeSbd;
          string str125 = flag.ToString();
          this.Log(str124 + str125 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str126 = "  Include SMS".PadRight(num1);
          flag = batchFile1.IncludeSms;
          string str127 = flag.ToString();
          this.Log(str126 + str127 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str128 = "  Include calls".PadRight(num1);
          flag = batchFile1.IncludeCalls;
          string str129 = flag.ToString();
          this.Log(str128 + str129 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str130 = "  Include TCP".PadRight(num1);
          flag = batchFile1.IncludeTcp;
          string str131 = flag.ToString();
          this.Log(str130 + str131 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log(this.LoadDataTypeSettingsList((IList<DataTypeSettings>) new List<DataTypeSettings>(), (IList<DataTypeSettings>) batchFile1.DataTypeSettingsList, num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      for (int index = this.batchFiles.Count - 1; index >= 0; --index)
      {
        BatchFile batchFile3 = this.batchFiles[index];
        bool flag = false;
        foreach (BatchFile batchFile4 in serviceSettings.BatchFiles)
        {
          if (batchFile3.File == batchFile4.File)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.batchFiles.RemoveAt(index);
          this.Log("Removed Batch File - " + batchFile3.File + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      foreach (Plugin plugin1 in serviceSettings.Plugins)
      {
        Plugin plugin2 = this.GetPlugin(plugin1.File);
        bool flag;
        if (plugin2 != null)
        {
          string str132 = string.Empty;
          if (plugin2.File != plugin1.File)
          {
            plugin2.File = plugin1.File;
            str132 = str132 + "  File".PadRight(num1) + plugin2.File + "\r\n";
          }
          if (plugin2.IncludeSbd != plugin1.IncludeSbd)
          {
            plugin2.IncludeSbd = plugin1.IncludeSbd;
            string str133 = str132;
            string str134 = "  Include SBD".PadRight(num1);
            flag = plugin2.IncludeSbd;
            string str135 = flag.ToString();
            str132 = str133 + str134 + str135 + "\r\n";
          }
          if (plugin2.IncludeSms != plugin1.IncludeSms)
          {
            plugin2.IncludeSms = plugin1.IncludeSms;
            string str136 = str132;
            string str137 = "  Include SMS".PadRight(num1);
            flag = plugin2.IncludeSms;
            string str138 = flag.ToString();
            str132 = str136 + str137 + str138 + "\r\n";
          }
          if (plugin2.IncludeCalls != plugin1.IncludeCalls)
          {
            plugin2.IncludeCalls = plugin1.IncludeCalls;
            string str139 = str132;
            string str140 = "  Include calls".PadRight(num1);
            flag = plugin2.IncludeCalls;
            string str141 = flag.ToString();
            str132 = str139 + str140 + str141 + "\r\n";
          }
          if (plugin2.IncludeTcp != plugin1.IncludeTcp)
          {
            plugin2.IncludeTcp = plugin1.IncludeTcp;
            string str142 = str132;
            string str143 = "  Include TCP".PadRight(num1);
            flag = plugin2.IncludeTcp;
            string str144 = flag.ToString();
            str132 = str142 + str143 + str144 + "\r\n";
          }
          string str145 = str132 + this.LoadDataTypeSettingsList((IList<DataTypeSettings>) plugin2.DataTypeSettingsList, (IList<DataTypeSettings>) plugin1.DataTypeSettingsList, num1);
          if (str145 != string.Empty)
            this.Log("Updated Plugin - " + plugin2.File + "\r\n" + str145, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
        else
        {
          this.plugins.Add(plugin1);
          this.Log("Added Plugin\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log("  File".PadRight(num1) + plugin1.File + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str146 = "  Include SBD".PadRight(num1);
          flag = plugin1.IncludeSbd;
          string str147 = flag.ToString();
          this.Log(str146 + str147 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str148 = "  Include SMS".PadRight(num1);
          flag = plugin1.IncludeSms;
          string str149 = flag.ToString();
          this.Log(str148 + str149 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str150 = "  Include calls".PadRight(num1);
          flag = plugin1.IncludeCalls;
          string str151 = flag.ToString();
          this.Log(str150 + str151 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          string str152 = "  Include TCP".PadRight(num1);
          flag = plugin1.IncludeTcp;
          string str153 = flag.ToString();
          this.Log(str152 + str153 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          this.Log(this.LoadDataTypeSettingsList((IList<DataTypeSettings>) new List<DataTypeSettings>(), (IList<DataTypeSettings>) plugin1.DataTypeSettingsList, num1), HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      for (int index = this.plugins.Count - 1; index >= 0; --index)
      {
        Plugin plugin3 = this.plugins[index];
        bool flag = false;
        foreach (Plugin plugin4 in serviceSettings.Plugins)
        {
          if (plugin3.File == plugin4.File)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.plugins.RemoveAt(index);
          this.Log("Removed Plugin - " + plugin3.File + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
        }
      }
      string str154 = string.Empty;
      int num6 = this.controlServer.Running != serviceSettings.ControlServerListen ? 1 : 0;
      bool flag14 = num6 != 0 && this.controlServer.Running;
      bool flag15 = num6 != 0 && !this.controlServer.Running;
      bool flag16 = num6 == 0 && this.controlServer.Running && this.controlServer.Port != serviceSettings.ControlServerPort;
      if (flag16 | flag14)
      {
        string str155 = str154 + "  Shutting down...".PadRight(num1);
        this.controlServer.Shutdown();
        this.controlSocket.Disconnect();
        str154 = str155 + "Success\r\n";
      }
      if (this.controlServer.Port != serviceSettings.ControlServerPort)
      {
        this.controlServer.Port = serviceSettings.ControlServerPort;
        str154 = str154 + "  Port".PadRight(num1) + (object) this.controlServer.Port + "\r\n";
      }
      if (flag16 | flag15)
      {
        try
        {
          str154 += "  Starting up...".PadRight(num1);
          this.controlServer.Startup();
          str154 += "Success\r\n";
        }
        catch (Exception ex)
        {
          str154 = str154 + "Failed - Exception: " + ex.Message + "\r\n";
        }
      }
      if (str154 != string.Empty)
        this.Log("Updated Control Server\r\n" + str154, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
      DataServerInfo dataServerInfo = serviceSettings.DataServers.Count == 0 ? new DataServerInfo() : serviceSettings.DataServers[0];
      string str156 = string.Empty;
      int num7 = this.dataServer.Running != dataServerInfo.Listen ? 1 : 0;
      bool flag17 = num7 != 0 && this.dataServer.Running;
      bool flag18 = num7 != 0 && !this.dataServer.Running;
      bool flag19 = num7 == 0 && this.dataServer.Running && this.dataServer.Port != dataServerInfo.Port;
      if (flag19 | flag17)
      {
        string str157 = str156 + "  Shutting down...".PadRight(num1);
        this.dataServer.Shutdown();
        foreach (SocketWrapper dataSocket in this.dataSockets)
          dataSocket.Disconnect();
        str156 = str157 + "Success\r\n";
      }
      if (this.dataServer.Port != dataServerInfo.Port)
      {
        this.dataServer.Port = dataServerInfo.Port;
        str156 = str156 + "  Port".PadRight(num1) + (object) this.dataServer.Port + "\r\n";
      }
      if (flag19 | flag18)
      {
        try
        {
          str156 += "  Starting up...".PadRight(num1);
          this.dataServer.Startup();
          str156 += "Success\r\n";
        }
        catch (Exception ex)
        {
          str156 = str156 + "Failed - Exception: " + ex.Message + "\r\n";
        }
      }
      if (this.dataServerIncludeSbd != dataServerInfo.IncludeSbd)
      {
        this.dataServerIncludeSbd = dataServerInfo.IncludeSbd;
        str156 = str156 + "  Include SBD".PadRight(num1) + this.dataServerIncludeSbd.ToString() + "\r\n";
      }
      if (this.dataServerIncludeSms != dataServerInfo.IncludeSms)
      {
        this.dataServerIncludeSms = dataServerInfo.IncludeSms;
        str156 = str156 + "  Include SMS".PadRight(num1) + this.dataServerIncludeSms.ToString() + "\r\n";
      }
      if (this.dataServerIncludeCalls != dataServerInfo.IncludeCalls)
      {
        this.dataServerIncludeCalls = dataServerInfo.IncludeCalls;
        str156 = str156 + "  Include calls".PadRight(num1) + this.dataServerIncludeCalls.ToString() + "\r\n";
      }
      if (this.dataServerIncludeTcp != dataServerInfo.IncludeTcp)
      {
        this.dataServerIncludeTcp = dataServerInfo.IncludeTcp;
        str156 = str156 + "  Include TCP".PadRight(num1) + this.dataServerIncludeTcp.ToString() + "\r\n";
      }
      string str158 = str156 + this.LoadDataTypeSettingsList((IList<DataTypeSettings>) this.dataServerDataTypeSettingsList, (IList<DataTypeSettings>) dataServerInfo.DataTypeSettingsList, num1);
      if (str158 != string.Empty)
        this.Log("Updated Data Server\r\n" + str158, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
      string str159 = string.Empty;
      if (CommLink.UseEncryption != serviceSettings.UseEncryption)
      {
        CommLink.UseEncryption = serviceSettings.UseEncryption;
        str159 = str159 + "  Use encryption".PadRight(num1) + CommLink.UseEncryption.ToString() + "\r\n";
      }
      int num8 = CommLink.EncryptionUser.IsLoggedIn() != serviceSettings.LogInForEncryption ? 1 : 0;
      bool flag20 = num8 != 0 && CommLink.EncryptionUser.IsLoggedIn();
      bool flag21 = num8 != 0 && !CommLink.EncryptionUser.IsLoggedIn();
      bool flag22 = num8 == 0 && CommLink.EncryptionUser.IsLoggedIn() && (this.encryptionUserName != serviceSettings.EncryptionUserName || this.encryptionUserPassword != serviceSettings.EncryptionUserPassword);
      if (flag20 | flag22)
      {
        string str160 = str159 + "  Logging out...".PadRight(num1);
        Erc code = CommLink.EncryptionUser.Logout();
        str159 = code != Erc.Success ? str160 + ErcStr.GetStringForCode(code) + "\r\n" : str160 + "Success\r\n";
      }
      if (this.encryptionUserName != serviceSettings.EncryptionUserName)
      {
        this.encryptionUserName = serviceSettings.EncryptionUserName;
        str159 = str159 + "  User".PadRight(num1) + this.encryptionUserName + "\r\n";
      }
      if (this.encryptionUserPassword != serviceSettings.EncryptionUserPassword)
      {
        this.encryptionUserPassword = serviceSettings.EncryptionUserPassword;
        str159 = str159 + "  Password".PadRight(num1) + "********\r\n";
      }
      if (flag21 | flag22)
      {
        string str161 = str159 + "  Logging in...".PadRight(num1);
        Erc code = CommLink.EncryptionUser.Login(this.encryptionUserName, this.encryptionUserPassword);
        str159 = code != Erc.Success ? str161 + ErcStr.GetStringForCode(code) + "\r\n" : str161 + "Success\r\n";
      }
      if (str159 != string.Empty)
        this.Log("Updated Encryption Settings\r\n" + str159, HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
      List<string> stylesheets = new List<string>();
      stylesheets.AddRange(this.logFiles.SelectMany<LogFile, DataTypeSettings>((Func<LogFile, IEnumerable<DataTypeSettings>>) (x => (IEnumerable<DataTypeSettings>) x.DataTypeSettingsList)).Select<DataTypeSettings, string>((Func<DataTypeSettings, string>) (x => x.Stylesheet)));
      stylesheets.AddRange(this.loggingDirectories.SelectMany<LoggingDirectory, DataTypeSettings>((Func<LoggingDirectory, IEnumerable<DataTypeSettings>>) (x => (IEnumerable<DataTypeSettings>) x.DataTypeSettingsList)).Select<DataTypeSettings, string>((Func<DataTypeSettings, string>) (x => x.Stylesheet)));
      stylesheets.AddRange(this.batchFiles.SelectMany<BatchFile, DataTypeSettings>((Func<BatchFile, IEnumerable<DataTypeSettings>>) (x => (IEnumerable<DataTypeSettings>) x.DataTypeSettingsList)).Select<DataTypeSettings, string>((Func<DataTypeSettings, string>) (x => x.Stylesheet)));
      stylesheets.AddRange(this.plugins.SelectMany<Plugin, DataTypeSettings>((Func<Plugin, IEnumerable<DataTypeSettings>>) (x => (IEnumerable<DataTypeSettings>) x.DataTypeSettingsList)).Select<DataTypeSettings, string>((Func<DataTypeSettings, string>) (x => x.Stylesheet)));
      stylesheets.AddRange(this.dataServerDataTypeSettingsList.Select<DataTypeSettings, string>((Func<DataTypeSettings, string>) (x => x.Stylesheet)));
      stylesheets = stylesheets.Where<string>((Func<string, bool>) (x => x != string.Empty)).Distinct<string>().ToList<string>();
      foreach (string str162 in stylesheets)
      {
        if (this.outputCache.CompiledStylesheets.ContainsKey(str162))
        {
          HiddenForm.CompiledStylesheet compiledStylesheet = this.outputCache.CompiledStylesheets[str162];
          try
          {
            DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(str162);
            if (compiledStylesheet.LastModified != lastWriteTimeUtc)
            {
              compiledStylesheet.Xsl.Load(str162);
              compiledStylesheet.LastModified = lastWriteTimeUtc;
              this.Log("Loaded stylesheet - " + str162 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
            }
          }
          catch (Exception ex)
          {
            this.outputCache.CompiledStylesheets.Remove(str162);
            this.Log("Failed to load stylesheet - " + str162 + ". Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
          }
        }
        else
        {
          try
          {
            HiddenForm.CompiledStylesheet compiledStylesheet = new HiddenForm.CompiledStylesheet();
            compiledStylesheet.Xsl.Load(str162);
            compiledStylesheet.LastModified = File.GetLastWriteTimeUtc(str162);
            this.outputCache.CompiledStylesheets.Add(str162, compiledStylesheet);
            this.Log("Loaded stylesheet - " + str162 + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
          }
          catch (Exception ex)
          {
            this.Log("Failed to load stylesheet - " + str162 + ". Exception: " + ex.Message + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
          }
        }
      }
      foreach (string key in this.outputCache.CompiledStylesheets.Where<KeyValuePair<string, HiddenForm.CompiledStylesheet>>((Func<KeyValuePair<string, HiddenForm.CompiledStylesheet>, bool>) (x => !stylesheets.Contains(x.Key))).Select<KeyValuePair<string, HiddenForm.CompiledStylesheet>, string>((Func<KeyValuePair<string, HiddenForm.CompiledStylesheet>, string>) (x => x.Key)))
      {
        this.outputCache.CompiledStylesheets.Remove(key);
        this.Log("Unloaded stylesheet - " + key + "\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushController);
      }
      this.Log("Configuration completed.\r\n\r\n", HiddenForm.LogDest.Both, HiddenForm.LogOpt.FlushBoth);
    }

    private string LoadDataTypeSettingsList(
      IList<DataTypeSettings> currList,
      IList<DataTypeSettings> newList,
      int colWidth)
    {
      string str1 = string.Empty;
      foreach (DataTypeSettings dataTypeSettings1 in (IEnumerable<DataTypeSettings>) newList)
      {
        DataTypeSettings entrySettings = dataTypeSettings1;
        DataTypeSettings dataTypeSettings2 = currList.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x =>
        {
          if (x.IsCategory != entrySettings.IsCategory)
            return false;
          return !x.IsCategory ? x.DataType == entrySettings.DataType : x.DataCategory == entrySettings.DataCategory;
        })).FirstOrDefault<DataTypeSettings>();
        if (dataTypeSettings2 != null)
        {
          string str2 = string.Empty;
          if (dataTypeSettings2.Format != entrySettings.Format)
          {
            dataTypeSettings2.Format = entrySettings.Format;
            str2 = str2 + "    Format".PadRight(colWidth) + (object) dataTypeSettings2.Format + "\r\n";
          }
          if (dataTypeSettings2.Stylesheet != entrySettings.Stylesheet)
          {
            dataTypeSettings2.Stylesheet = entrySettings.Stylesheet;
            str2 = str2 + "    Stylesheet".PadRight(colWidth) + dataTypeSettings2.Stylesheet + "\r\n";
          }
          if (str2 != string.Empty)
          {
            str1 += "  Updated Data Type Settings - ";
            if (dataTypeSettings2.IsCategory)
              str1 = str1 + "Category (" + (object) dataTypeSettings2.DataCategory + ")\r\n";
            else
              str1 = str1 + "Type (" + (object) dataTypeSettings2.DataType + ")\r\n";
            str1 += str2;
          }
        }
        else
        {
          currList.Add(entrySettings);
          str1 += "  Added Data Type Settings\r\n";
          if (entrySettings.IsCategory)
            str1 = str1 + "    Category".PadRight(colWidth) + (object) entrySettings.DataCategory + "\r\n";
          else
            str1 = str1 + "    Type".PadRight(colWidth) + (object) entrySettings.DataType + "\r\n";
          str1 = str1 + "    Format".PadRight(colWidth) + (object) entrySettings.Format + "\r\n";
          str1 = str1 + "    Stylesheet".PadRight(colWidth) + entrySettings.Stylesheet + "\r\n";
        }
      }
      for (int index = currList.Count - 1; index >= 0; --index)
      {
        DataTypeSettings dataType = currList[index];
        if (newList.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x =>
        {
          if (x.IsCategory != dataType.IsCategory)
            return false;
          return !x.IsCategory ? x.DataType == dataType.DataType : x.DataCategory == dataType.DataCategory;
        })).FirstOrDefault<DataTypeSettings>() == null)
        {
          currList.RemoveAt(index);
          string str3 = str1 + "  Removed Data Type Settings - ";
          if (dataType.IsCategory)
            str1 = str3 + "Category (" + (object) dataType.DataCategory + ")\r\n";
          else
            str1 = str3 + "Type (" + (object) dataType.DataType + ")\r\n";
        }
      }
      return str1;
    }

    private RUPair FindBestMatchForUpdateResponse0(
      UpdateResponse0 response,
      ModemInfoTypes senderType,
      string sender)
    {
      RUPair forUpdateResponse0 = (RUPair) null;
      foreach (RUPair ruPair in this.ruPairs.Where<RUPair>((Func<RUPair, bool>) (x => x.RequestInfo != null && x.ResponseInfo == null)))
      {
        RURequestInfo requestInfo = ruPair.RequestInfo;
        UpdateRequest request;
        if ((requestInfo.RecipientType != senderType || requestInfo.Recipient == sender) && UpdateRequest.Parse(ruPair.RequestInfo.Data.ToArray<byte>(), out request) && request is UpdateRequest0 && (request as UpdateRequest0).ResponseMatches(response))
        {
          forUpdateResponse0 = ruPair;
          break;
        }
      }
      return forUpdateResponse0;
    }

    private RUPair FindBestMatchForUpdateResponse1(
      UpdateResponse1 response,
      ModemInfoTypes senderType,
      string sender)
    {
      RUPair forUpdateResponse1 = (RUPair) null;
      foreach (RUPair ruPair in this.ruPairs.Where<RUPair>((Func<RUPair, bool>) (x => x.RequestInfo != null && x.ResponseInfo == null)))
      {
        RURequestInfo requestInfo = ruPair.RequestInfo;
        UpdateRequest request;
        if ((requestInfo.RecipientType != senderType || requestInfo.Recipient == sender) && UpdateRequest.Parse(ruPair.RequestInfo.Data.ToArray<byte>(), out request) && request is UpdateRequest1 && (request as UpdateRequest1).ResponseMatches(response))
        {
          forUpdateResponse1 = ruPair;
          break;
        }
      }
      return forUpdateResponse1;
    }

    private RUPair FindBestMatchForUpdateResponse2(
      UpdateResponse2 response,
      ModemInfoTypes senderType,
      string sender)
    {
      RUPair forUpdateResponse2 = (RUPair) null;
      foreach (RUPair ruPair in this.ruPairs.Where<RUPair>((Func<RUPair, bool>) (x => x.RequestInfo != null && x.ResponseInfo == null)))
      {
        RURequestInfo requestInfo = ruPair.RequestInfo;
        UpdateRequest request;
        if ((requestInfo.RecipientType != senderType || requestInfo.Recipient == sender) && UpdateRequest.Parse(ruPair.RequestInfo.Data.ToArray<byte>(), out request) && request is UpdateRequest2 && (request as UpdateRequest2).ResponseMatches(response))
        {
          forUpdateResponse2 = ruPair;
          break;
        }
      }
      return forUpdateResponse2;
    }

    private RUPair FindBestMatchForUpdateResponse3(
      UpdateResponse3 response,
      ModemInfoTypes senderType,
      string sender)
    {
      RUPair forUpdateResponse3 = (RUPair) null;
      int num1 = 0;
      bool flag1 = false;
      foreach (RUPair ruPair in this.ruPairs.Where<RUPair>((Func<RUPair, bool>) (x => x.RequestInfo != null && x.ResponseInfo == null)))
      {
        RURequestInfo requestInfo = ruPair.RequestInfo;
        bool flag2 = requestInfo.RecipientType == senderType && requestInfo.Recipient == sender;
        UpdateRequest request;
        if (requestInfo.RecipientType != senderType | flag2 && UpdateRequest.Parse(ruPair.RequestInfo.Data.ToArray<byte>(), out request) && request is UpdateRequest3)
        {
          UpdateRequest3 updateRequest3 = request as UpdateRequest3;
          if (response.Items.Count <= updateRequest3.Items.Count)
          {
            int num2 = updateRequest3.Items.Count - response.Items.Count;
            int index;
            for (index = 0; index < response.Items.Count; ++index)
            {
              UpdateRequest3Item updateRequest3Item = updateRequest3.Items[index];
              UpdateResponse3Item updateResponse3Item = response.Items[index];
              if (updateRequest3Item.TagTable != updateResponse3Item.TagTable || (int) updateRequest3Item.TagValue != (int) updateResponse3Item.TagValue)
                break;
            }
            if (index == response.Items.Count && (forUpdateResponse3 == null || !flag1 & flag2 || flag1 == flag2 && num2 < num1))
            {
              forUpdateResponse3 = ruPair;
              num1 = num2;
              flag1 = flag2;
            }
          }
        }
      }
      return forUpdateResponse3;
    }

    private XElement CreateMetaElement(
      CommLink commLink,
      DateTime sessionStart,
      ModemInfoTypes senderType,
      string sender,
      Protocol protocol,
      DataType type)
    {
      string[] strArray = commLink.Name.Split(new string[2]
      {
        " (",
        ")"
      }, StringSplitOptions.None);
      string str1 = strArray[0];
      string str2 = strArray[1];
      string format = "yyyy'-'MM'-'ddTHH':'mm':'ssZ";
      string content = sessionStart.ToUniversalTime().ToString(format);
      XElement metaElement = new XElement((XName) "meta", new object[4]
      {
        (object) new XElement((XName) "receiver", new object[2]
        {
          (object) new XAttribute((XName) nameof (type), (object) str1),
          (object) str2
        }),
        (object) new XElement((XName) "time", (object) DateTime.UtcNow.ToString(format)),
        (object) new XElement((XName) nameof (protocol), (object) protocol.GetDescription()),
        (object) new XElement((XName) nameof (type), (object) type.GetDescription())
      });
      if (senderType != ModemInfoTypes.Unknown || sender != string.Empty)
        metaElement.AddFirst((object) new XElement((XName) nameof (sender), new object[2]
        {
          (object) new XAttribute((XName) nameof (type), (object) senderType.GetDescription()),
          (object) sender
        }));
      if (protocol == Protocol.Call || protocol == Protocol.Tcp)
        metaElement.Add((object) new XElement((XName) nameof (sessionStart), (object) content));
      return metaElement;
    }

    private DataTypeSettings LookUpDataTypeSettings(
      IEnumerable<DataTypeSettings> settingsList,
      DataType type)
    {
      DataTypeSettings dataTypeSettings = settingsList.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => !x.IsCategory && x.DataType == type)).FirstOrDefault<DataTypeSettings>();
      if (dataTypeSettings == null)
      {
        IEnumerable<DataTypeSettings> source = settingsList.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => x.IsCategory));
        switch (type)
        {
          case DataType.NalGpsReport3:
          case DataType.NalGpsReport4:
          case DataType.NalGpsReport5:
          case DataType.NalGpsReport6:
          case DataType.NalGpsReport7:
          case DataType.Nal10ByteGpsReport0:
          case DataType.PecosP3GpsReport:
          case DataType.PecosP4GpsReport:
            dataTypeSettings = source.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => x.DataCategory == DataCategory.GpsReports)).FirstOrDefault<DataTypeSettings>() ?? source.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => x.DataCategory == DataCategory.AllKnown)).FirstOrDefault<DataTypeSettings>();
            break;
          case DataType.UpdateResponse0:
          case DataType.UpdateResponse1:
          case DataType.UpdateResponse2:
          case DataType.UpdateResponse3:
            dataTypeSettings = source.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => x.DataCategory == DataCategory.UpdateResponses)).FirstOrDefault<DataTypeSettings>() ?? source.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => x.DataCategory == DataCategory.AllKnown)).FirstOrDefault<DataTypeSettings>();
            break;
          case DataType.StatusReport0:
            dataTypeSettings = source.Where<DataTypeSettings>((Func<DataTypeSettings, bool>) (x => x.DataCategory == DataCategory.AllKnown)).FirstOrDefault<DataTypeSettings>();
            break;
        }
      }
      return dataTypeSettings;
    }

    private void OutputToLogFile(LogFile entry, Protocol protocol, DataType type)
    {
      if ((protocol != Protocol.Sbd || !entry.IncludeSbd) && (protocol != Protocol.Sms || !entry.IncludeSms) && (protocol != Protocol.Call || !entry.IncludeCalls) && (protocol != Protocol.Tcp || !entry.IncludeTcp))
        return;
      DataTypeSettings dataTypeSettings = this.LookUpDataTypeSettings((IEnumerable<DataTypeSettings>) entry.DataTypeSettingsList, type);
      if (dataTypeSettings == null)
        return;
      string str1 = string.Empty;
      switch (dataTypeSettings.Format)
      {
        case LoggingFormat.Hex:
          str1 = this.outputCache.GetHex(dataTypeSettings.Stylesheet);
          break;
        case LoggingFormat.Xml:
          str1 = this.outputCache.GetXml(dataTypeSettings.Stylesheet);
          break;
      }
      if (!(str1 != string.Empty))
        return;
      string str2 = str1 + "\r\n\r\n";
      try
      {
        StreamWriter streamWriter = new StreamWriter(entry.File, true);
        streamWriter.Write(str2);
        streamWriter.Flush();
        streamWriter.Close();
      }
      catch
      {
      }
    }

    private void OutputToLoggingDirectory(LoggingDirectory entry, Protocol protocol, DataType type)
    {
      if ((protocol != Protocol.Sbd || !entry.IncludeSbd) && (protocol != Protocol.Sms || !entry.IncludeSms) && (protocol != Protocol.Call || !entry.IncludeCalls) && (protocol != Protocol.Tcp || !entry.IncludeTcp))
        return;
      DataTypeSettings dataTypeSettings = this.LookUpDataTypeSettings((IEnumerable<DataTypeSettings>) entry.DataTypeSettingsList, type);
      if (dataTypeSettings == null)
        return;
      string contents = string.Empty;
      switch (dataTypeSettings.Format)
      {
        case LoggingFormat.Hex:
          contents = this.outputCache.GetHex(dataTypeSettings.Stylesheet);
          break;
        case LoggingFormat.Xml:
          contents = this.outputCache.GetXml(dataTypeSettings.Stylesheet);
          break;
      }
      string str = entry.Directory + "\\" + entry.FilePrefix + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff");
      if (File.Exists(str + ".xml"))
      {
        int num = 0;
        do
        {
          ++num;
        }
        while (File.Exists(str + "_" + num.ToString() + ".xml"));
        str = str + "_" + num.ToString();
      }
      if (dataTypeSettings.Format == LoggingFormat.Binary)
      {
        try
        {
          File.WriteAllBytes(str + ".xml", this.outputCache.Data.ToArray());
        }
        catch
        {
        }
      }
      else
      {
        if (!(contents != string.Empty))
          return;
        try
        {
          File.WriteAllText(str + ".xml", contents);
        }
        catch
        {
        }
      }
    }

    private void OutputToBatchFile(BatchFile entry, Protocol protocol, DataType type)
    {
      if ((protocol != Protocol.Sbd || !entry.IncludeSbd) && (protocol != Protocol.Sms || !entry.IncludeSms) && (protocol != Protocol.Call || !entry.IncludeCalls) && (protocol != Protocol.Tcp || !entry.IncludeTcp))
        return;
      DataTypeSettings dataTypeSettings = this.LookUpDataTypeSettings((IEnumerable<DataTypeSettings>) entry.DataTypeSettingsList, type);
      if (dataTypeSettings == null)
        return;
      string str = string.Empty;
      switch (dataTypeSettings.Format)
      {
        case LoggingFormat.Hex:
          str = this.outputCache.GetHex(dataTypeSettings.Stylesheet);
          break;
        case LoggingFormat.Xml:
          str = this.outputCache.GetXml(dataTypeSettings.Stylesheet);
          break;
      }
      if (!(str != string.Empty))
        return;
      try
      {
        Process.Start(entry.File, "\"" + entry.File + "\" \"" + str.Replace("\"", "\\\"") + "\"");
      }
      catch
      {
      }
    }

    private void OutputToDataSocket(Protocol protocol, DataType type)
    {
      byte[] buffer = new byte[4]
      {
        (byte) 13,
        (byte) 10,
        (byte) 13,
        (byte) 10
      };
      if ((protocol != Protocol.Sbd || !this.dataServerIncludeSbd) && (protocol != Protocol.Sms || !this.dataServerIncludeSms) && (protocol != Protocol.Call || !this.dataServerIncludeCalls) && (protocol != Protocol.Tcp || !this.dataServerIncludeTcp))
        return;
      DataTypeSettings dataTypeSettings = this.LookUpDataTypeSettings((IEnumerable<DataTypeSettings>) this.dataServerDataTypeSettingsList, type);
      if (dataTypeSettings == null)
        return;
      string s = string.Empty;
      switch (dataTypeSettings.Format)
      {
        case LoggingFormat.Hex:
          s = this.outputCache.GetHex(dataTypeSettings.Stylesheet);
          break;
        case LoggingFormat.Xml:
          s = this.outputCache.GetXml(dataTypeSettings.Stylesheet);
          break;
      }
      if (s == null)
        return;
      byte[] bytes = Encoding.GetEncoding(1252).GetBytes(s);
      foreach (SocketWrapper dataSocket in this.dataSockets)
      {
        dataSocket.Send(bytes, 0, bytes.Length);
        dataSocket.Send(buffer, 0, buffer.Length);
      }
    }

    private void OutputToPlugin(Plugin entry, Protocol protocol, DataType type)
    {
      if ((protocol != Protocol.Sbd || !entry.IncludeSbd) && (protocol != Protocol.Sms || !entry.IncludeSms) && (protocol != Protocol.Call || !entry.IncludeCalls) && (protocol != Protocol.Tcp || !entry.IncludeTcp))
        return;
      DataTypeSettings dataTypeSettings = this.LookUpDataTypeSettings((IEnumerable<DataTypeSettings>) entry.DataTypeSettingsList, type);
      if (dataTypeSettings == null)
        return;
      string dataXmlString = string.Empty;
      switch (dataTypeSettings.Format)
      {
        case LoggingFormat.Hex:
          dataXmlString = this.outputCache.GetHex(dataTypeSettings.Stylesheet);
          break;
        case LoggingFormat.Xml:
          dataXmlString = this.outputCache.GetXml(dataTypeSettings.Stylesheet);
          break;
      }
      if (!(dataXmlString != string.Empty))
        return;
      try
      {
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (!(assembly2 is AssemblyBuilder) && assembly2.Location.EndsWith(entry.File))
          {
            assembly1 = assembly2;
            break;
          }
        }
        if (assembly1 == (Assembly) null && File.Exists(entry.File))
          assembly1 = Assembly.LoadFile(entry.File);
        if (!(assembly1 != (Assembly) null))
          return;
        foreach (System.Type type1 in assembly1.GetTypes())
        {
          if (type1.IsDefined(typeof (ProcessDataPluginAttribute), true))
          {
            ((IProcessDataPlugin) Activator.CreateInstance(type1)).ProcessData(dataXmlString);
            break;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.waitForFileTimer = new Timer(this.components);
      this.settingsFileSystemWatcher = new FileSystemWatcher();
      this.checkMultipartSmsTimer = new Timer(this.components);
      this.settingsFileSystemWatcher.BeginInit();
      this.SuspendLayout();
      this.waitForFileTimer.Interval = 1000;
      this.waitForFileTimer.Tick += new EventHandler(this.OnWaitForFileTimerTick);
      this.settingsFileSystemWatcher.EnableRaisingEvents = true;
      this.settingsFileSystemWatcher.Filter = "ServiceSettings.xml";
      this.settingsFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
      this.settingsFileSystemWatcher.SynchronizingObject = (ISynchronizeInvoke) this;
      this.settingsFileSystemWatcher.Changed += new FileSystemEventHandler(this.OnSettingsFileSystemWatcherChanged);
      this.checkMultipartSmsTimer.Interval = 30000;
      this.checkMultipartSmsTimer.Tick += new EventHandler(this.OnCheckMultipartSmsTimerTick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(256, 164);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (HiddenForm);
      this.WindowState = FormWindowState.Minimized;
      this.Load += new EventHandler(this.OnHiddenFormLoad);
      this.FormClosing += new FormClosingEventHandler(this.OnHiddenFormClosing);
      this.settingsFileSystemWatcher.EndInit();
      this.ResumeLayout(false);
    }

    private enum LogDest
    {
      File,
      Controller,
      Both,
    }

    private enum LogOpt
    {
      FlushFile,
      FlushController,
      FlushBoth,
      None,
    }

    private class MultipartSmsBuffer
    {
      private DateTime startTime;
      private ModemInfoTypes senderType;
      private string sender;
      private byte refNum;
      private byte[][] parts;

      public MultipartSmsBuffer(
        ModemInfoTypes senderType,
        string sender,
        byte refNum,
        byte totalParts)
      {
        this.startTime = DateTime.UtcNow;
        this.senderType = senderType;
        this.sender = sender;
        this.refNum = refNum;
        this.parts = new byte[(int) totalParts][];
      }

      public DateTime StartTime => this.startTime;

      public ModemInfoTypes SenderType => this.senderType;

      public string Sender => this.sender;

      public byte RefNum => this.refNum;

      public byte[][] Parts => this.parts;
    }

    private class OutputCache
    {
      private string xml;
      private string hex;
      private Dictionary<string, string> styledXml;
      private Dictionary<string, string> styledHex;
      private Dictionary<string, HiddenForm.CompiledStylesheet> compiledStylesheets;

      public OutputCache()
      {
        this.styledXml = new Dictionary<string, string>();
        this.styledHex = new Dictionary<string, string>();
        this.compiledStylesheets = new Dictionary<string, HiddenForm.CompiledStylesheet>();
      }

      public List<byte> Data { get; set; }

      public Nal.GpsReport.GpsReport GpsReport { get; set; }

      public UpdateResponse UpdateResponse { get; set; }

      public StatusReport0 StatusReport { get; set; }

      public XElement MetaElement { get; set; }

      public Dictionary<string, HiddenForm.CompiledStylesheet> CompiledStylesheets
      {
        get => this.compiledStylesheets;
      }

      public void Clear()
      {
        this.xml = string.Empty;
        this.hex = string.Empty;
        this.styledXml.Clear();
        this.styledHex.Clear();
        this.Data = (List<byte>) null;
        this.GpsReport = (Nal.GpsReport.GpsReport) null;
        this.UpdateResponse = (UpdateResponse) null;
        this.StatusReport = (StatusReport0) null;
      }

      public string GetXml(string stylesheet)
      {
        if (this.xml == string.Empty)
        {
          XElement valueElement = (XElement) null;
          if (this.GpsReport != null)
            valueElement = this.GpsReport.ToXElement();
          else if (this.UpdateResponse != null)
            valueElement = this.UpdateResponse.ToXElement();
          else if (this.StatusReport != null)
            valueElement = this.StatusReport.ToXElement();
          if (valueElement == null)
            return this.xml;
          this.xml = this.FormData(valueElement);
        }
        if (stylesheet == string.Empty)
          return this.xml;
        if (this.styledXml.ContainsKey(stylesheet))
          return this.styledXml[stylesheet];
        string xml = this.Transform(this.xml, stylesheet);
        this.styledXml[stylesheet] = xml;
        return xml;
      }

      public string GetHex(string stylesheet)
      {
        if (this.hex == string.Empty)
          this.hex = this.FormData(new XElement((XName) "hex", (object) string.Concat(this.Data.Select<byte, string>((Func<byte, string>) (x => x.ToString("X2"))).ToArray<string>())));
        if (stylesheet == string.Empty)
          return this.hex;
        if (this.styledHex.ContainsKey(stylesheet))
          return this.styledHex[stylesheet];
        string hex = this.Transform(this.hex, stylesheet);
        this.styledHex[stylesheet] = hex;
        return hex;
      }

      private string FormData(XElement valueElement)
      {
        XDocument xdocument = new XDocument(new object[1]
        {
          (object) new XElement((XName) "data", new object[2]
          {
            (object) this.MetaElement,
            (object) valueElement
          })
        });
        StringBuilder output = new StringBuilder();
        using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings()
        {
          Indent = true,
          OmitXmlDeclaration = true
        }))
          xdocument.WriteTo(writer);
        return output.ToString();
      }

      private string Transform(string str, string stylesheet)
      {
        string empty = string.Empty;
        if (this.compiledStylesheets.ContainsKey(stylesheet))
        {
          HiddenForm.CompiledStylesheet compiledStylesheet = this.compiledStylesheets[stylesheet];
          using (XmlReader input = XmlReader.Create((TextReader) new StringReader(str)))
          {
            using (StringWriter results = new StringWriter())
            {
              try
              {
                compiledStylesheet.Xsl.Transform(input, new XsltArgumentList(), (TextWriter) results);
                empty = results.ToString();
              }
              catch (XsltException ex)
              {
              }
            }
          }
        }
        return empty;
      }
    }

    private class CompiledStylesheet
    {
      private XslCompiledTransform xsl;

      public CompiledStylesheet() => this.xsl = new XslCompiledTransform();

      public DateTime LastModified { get; set; }

      public XslCompiledTransform Xsl => this.xsl;
    }
  }
}
