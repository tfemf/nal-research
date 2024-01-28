// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.SbdDirectIpClientHandler
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using Nal.SbdDirectIP;
using Nal.ServerForTrackers.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class SbdDirectIpClientHandler : CommLink
  {
    private SocketWrapper socket;
    private List<byte> receivedData;
    private bool sendMOAcknowledgement;
    private System.Windows.Forms.Timer removeTimer;

    public SbdDirectIpClientHandler(Socket connectedSocket, bool sendMODirectIPAcknowledgement)
    {
      this.socket = new SocketWrapper();
      this.socket.TakeOverConnection(connectedSocket);
      this.socket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnSocketDataReceived);
      this.socket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectionClosed);
      this.sendMOAcknowledgement = sendMODirectIPAcknowledgement;
      this.receivedData = new List<byte>();
      this.Name = "SBD DirectIP Client Handler (" + ((IPEndPoint) connectedSocket.RemoteEndPoint).ToString() + ")";
      this.Status = "Connected";
      this.removeTimer = new System.Windows.Forms.Timer();
      this.removeTimer.Interval = 60000;
      this.removeTimer.Tick += new EventHandler(this.OnRemoveTimerTick);
      this.removeTimer.Start();
    }

    public void Disconnect()
    {
      this.removeTimer.Stop();
      this.socket.Disconnect();
      this.TriggerRemoveMe(string.Empty);
    }

    private void OnSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      this.receivedData.AddRange((IEnumerable<byte>) e.Data);
      if (!this.sendMOAcknowledgement || !SbdDirectIPMessage.ReceivedAll((IList<byte>) this.receivedData))
        return;
      bool flag = false;
      SbdDirectIPMessage message;
      if (SbdDirectIPMessage.Parse((IList<byte>) this.receivedData, out message))
      {
        MOHeaderIE moHeaderIe = (MOHeaderIE) message.InfoElements.Find((Predicate<InfoElement>) (x => x.Id == (byte) 1));
        GenericIE genericIe = (GenericIE) message.InfoElements.Find((Predicate<InfoElement>) (x => x.Id == (byte) 2));
        if (moHeaderIe != null && genericIe != null)
        {
          flag = true;
          this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(), ModemInfoTypes.Imei, moHeaderIe.Imei, Protocol.Sbd, DateTime.UtcNow, (IList<byte>) genericIe.Content));
        }
      }
      this.socket.Send(new SbdDirectIPMessage()
      {
        InfoElements = {
          (InfoElement) new MOConfirmationIE()
          {
            Status = flag
          }
        }
      }.GetBytes().ToArray());
    }

    private void OnSocketConnectionClosed(object sender, EventArgs e)
    {
      this.removeTimer.Stop();
      SbdDirectIPMessage message;
      if (!this.sendMOAcknowledgement && SbdDirectIPMessage.Parse((IList<byte>) this.receivedData, out message))
      {
        MOHeaderIE moHeaderIe = (MOHeaderIE) message.InfoElements.Find((Predicate<InfoElement>) (x => x.Id == (byte) 1));
        GenericIE genericIe = (GenericIE) message.InfoElements.Find((Predicate<InfoElement>) (x => x.Id == (byte) 2));
        if (moHeaderIe != null && genericIe != null)
          this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(), ModemInfoTypes.Imei, moHeaderIe.Imei, Protocol.Sbd, DateTime.UtcNow, (IList<byte>) genericIe.Content));
      }
      this.Status = "Disconnected";
      this.TriggerRemoveMe(string.Empty);
    }

    private void OnRemoveTimerTick(object sender, EventArgs e) => this.Disconnect();
  }
}
