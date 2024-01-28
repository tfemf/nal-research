// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.SbdDirectIPClient
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class SbdDirectIPClient
  {
    private TcpClientWrapper client;
    private List<byte> messageData;
    private List<byte> receivedData;

    public SbdDirectIPClient()
    {
      this.client = new TcpClientWrapper();
      this.client.ConnectFailed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectFailed);
      this.client.ConnectionMade += new EventHandler(this.OnSocketConnectionMade);
      this.client.DataReceived += new EventHandler<DataReceivedEventArgs>(this.OnSocketDataReceived);
      this.client.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectionClosed);
      this.messageData = new List<byte>();
      this.receivedData = new List<byte>();
    }

    public event EventHandler<SbdDirectIPClientSendCompletedEventArgs> SendCompleted;

    public string Host { get; set; }

    public int Port { get; set; }

    public void Send(IEnumerable<byte> data, string imei)
    {
      this.SendInternal((IEnumerable<byte>) this.CreateMTMessage(data, imei));
    }

    public void SendClearMTQueue(string imei)
    {
      this.SendInternal((IEnumerable<byte>) this.CreateClearMTQueueMessage(imei));
    }

    public void Abort()
    {
      if (!this.client.Connecting && !this.client.Connected)
        return;
      this.client.Close();
    }

    private void OnSocketConnectFailed(object sender, ExceptionEventArgs e)
    {
      this.RaiseSendCompleted(e.Exception, new short?());
    }

    private void OnSocketConnectionMade(object sender, EventArgs e)
    {
      this.client.Send(this.messageData.ToArray());
    }

    private void OnSocketDataReceived(object sender, DataReceivedEventArgs e)
    {
      this.receivedData.AddRange((IEnumerable<byte>) e.Data);
    }

    private void OnSocketConnectionClosed(object sender, ExceptionEventArgs e)
    {
      if (e.Exception != null)
      {
        this.RaiseSendCompleted(e.Exception, new short?());
      }
      else
      {
        SbdDirectIPMessage message;
        if (!SbdDirectIPMessage.Parse((IList<byte>) this.receivedData, out message))
        {
          this.RaiseSendCompleted(new Exception("Received data not recognized."), new short?());
        }
        else
        {
          MTConfirmationIE mtConfirmationIe = (MTConfirmationIE) message.InfoElements.Find((Predicate<InfoElement>) (x => x.Id == (byte) 68));
          if (mtConfirmationIe == null)
            this.RaiseSendCompleted(new Exception("Missing confirmation IE."), new short?());
          else if (mtConfirmationIe.Status < (short) 0)
            this.RaiseSendCompleted(new Exception(this.LookupConfirmationStatusDescription((int) mtConfirmationIe.Status)), new short?(mtConfirmationIe.Status));
          else
            this.RaiseSendCompleted((Exception) null, new short?(mtConfirmationIe.Status));
        }
      }
    }

    private List<byte> CreateMTMessage(IEnumerable<byte> data, string imei)
    {
      return new SbdDirectIPMessage()
      {
        InfoElements = {
          (InfoElement) new MTHeaderIE() { Imei = imei },
          (InfoElement) new GenericIE((byte) 66, data)
        }
      }.GetBytes();
    }

    private List<byte> CreateClearMTQueueMessage(string imei)
    {
      return new SbdDirectIPMessage()
      {
        InfoElements = {
          (InfoElement) new MTHeaderIE()
          {
            Imei = imei,
            DispositionFlags = (ushort) 1
          }
        }
      }.GetBytes();
    }

    private void SendInternal(IEnumerable<byte> message)
    {
      if (this.client.Connecting || this.client.Connected)
        throw new InvalidOperationException("Already connected or connecting");
      this.messageData.Clear();
      this.messageData.AddRange(message);
      this.receivedData.Clear();
      this.client.Connect(this.Host, this.Port);
    }

    private void RaiseSendCompleted(Exception error, short? confirmationStatus)
    {
      if (this.SendCompleted == null)
        return;
      EventHandler<SbdDirectIPClientSendCompletedEventArgs> sendCompleted = this.SendCompleted;
      Exception error1 = error;
      short? nullable = confirmationStatus;
      int? confirmationStatus1 = nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?();
      SbdDirectIPClientSendCompletedEventArgs e = new SbdDirectIPClientSendCompletedEventArgs(error1, confirmationStatus1);
      sendCompleted((object) this, e);
    }

    private string LookupConfirmationStatusDescription(int confirmationStatus)
    {
      switch (confirmationStatus)
      {
        case -9:
          return "The given IMEI is not attached (not set to receive ring alerts)";
        case -8:
          return "Ring alerts to the given IMEI are disabled";
        case -7:
          return "Violation of MT DirectIP protocol";
        case -6:
          return "MT resources unavailable";
        case -5:
          return "MT message queue full (max of 50)";
        case -4:
          return "Payload expected, but none received";
        case -3:
          return "Payload size exceeded maximum allowed";
        case -2:
          return "Unknown IMEI – not provisioned on the GSS";
        case -1:
          return "Invalid IMEI – too few characters, non-numeric characters";
        case 0:
          return "Successful, no payload in message";
        default:
          return "Queued";
      }
    }
  }
}
