// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.SbdDirectIpClient
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using Nal.SbdDirectIP;
using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class SbdDirectIpClient : CommLink
  {
    private SocketWrapper socket;
    private string imei;
    private byte[] data;
    private List<byte> receivedData;
    private bool confirmationStatusReceived;
    private short confirmationStatus;

    public SbdDirectIpClient()
    {
      this.Status = "Disconnected";
      this.Name = "SBD DirectIP Client";
      this.confirmationStatusReceived = false;
      this.receivedData = new List<byte>();
      this.socket = new SocketWrapper();
      this.socket.ConnectFailed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectFailed);
      this.socket.ConnectionMade += new EventHandler(this.OnSocketConnectionMade);
      this.socket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectionClosed);
      this.socket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnSocketDataReceived);
    }

    public event EventHandler ConnectFailed;

    public event EventHandler ConnectionDropped;

    public bool Sending => this.socket.Connecting || this.socket.Connected;

    public bool ConfirmationStatusReceived => this.confirmationStatusReceived;

    public short ConfirmationStatus => this.confirmationStatus;

    public string Imei => this.imei;

    public byte[] Data => this.data;

    public void Send(string host, int port, string imei, byte[] data)
    {
      this.Status = "Connecting...";
      this.confirmationStatusReceived = false;
      this.imei = imei;
      this.data = data;
      this.socket.Connect(host, port);
    }

    private void OnSocketConnectFailed(object sender, EventArgs e)
    {
      this.Status = "Disconnected - Connect Failed";
      if (this.ConnectFailed == null)
        return;
      this.ConnectFailed((object) this, EventArgs.Empty);
    }

    private void OnSocketConnectionMade(object sender, EventArgs e)
    {
      SbdDirectIPMessage sbdDirectIpMessage = new SbdDirectIPMessage();
      sbdDirectIpMessage.InfoElements.Add((InfoElement) new MTHeaderIE()
      {
        Imei = this.imei
      });
      sbdDirectIpMessage.InfoElements.Add((InfoElement) new GenericIE((byte) 66, (IEnumerable<byte>) this.data));
      this.Status = "Sending message";
      this.socket.Send(sbdDirectIpMessage.GetBytes().ToArray());
    }

    private void OnSocketConnectionClosed(object sender, EventArgs e)
    {
      if (this.confirmationStatusReceived)
        this.Status = "Disconnected - Confirmation Status: " + (object) this.confirmationStatus + (this.confirmationStatus > (short) 0 ? (object) " (Sent)" : (object) " (Error)");
      else
        this.Status = "Disconnected - No Confirmation";
      if (this.ConnectionDropped == null)
        return;
      this.ConnectionDropped((object) this, EventArgs.Empty);
    }

    private void OnSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      if (this.confirmationStatusReceived)
        return;
      this.receivedData.AddRange((IEnumerable<byte>) e.Data);
      SbdDirectIPMessage message;
      if (!SbdDirectIPMessage.ReceivedAll((IList<byte>) this.receivedData) || !SbdDirectIPMessage.Parse((IList<byte>) this.receivedData, out message))
        return;
      MTConfirmationIE mtConfirmationIe = (MTConfirmationIE) message.InfoElements.Find((Predicate<InfoElement>) (x => x.Id == (byte) 68));
      if (mtConfirmationIe == null)
        return;
      this.confirmationStatus = mtConfirmationIe.Status;
      this.confirmationStatusReceived = true;
    }
  }
}
