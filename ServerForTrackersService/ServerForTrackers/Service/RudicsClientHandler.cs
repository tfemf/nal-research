// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.RudicsClientHandler
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using System;
using System.Net.Sockets;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class RudicsClientHandler : RudicsConnection
  {
    private SocketWrapper socket;

    public RudicsClientHandler()
    {
      this.socket = new SocketWrapper();
      this.socket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectionClosed);
      this.socket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnSocketDataReceived);
    }

    public void HandleClient(Socket connectedSocket)
    {
      this.socket.TakeOverConnection(connectedSocket);
      this.Name = "RUDICS Client Handler (" + this.socket.RemoteEndPoint.ToString() + ")";
      this.HandleConnect(false);
    }

    public override void Disconnect()
    {
      this.socket.Disconnect();
      this.HandleDisconnect(string.Empty);
    }

    private void OnSocketConnectionClosed(object sender, EventArgs e)
    {
      this.HandleDisconnect("Connection dropped.");
    }

    private void OnSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      this.HandleDataReceived(e.Data);
    }

    protected override void Send(byte[] data) => this.socket.Send(data);
  }
}
