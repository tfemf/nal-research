// Decompiled with JetBrains decompiler
// Type: Nal.Network.ClientConnectedEventArgs
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Net.Sockets;

#nullable disable
namespace Nal.Network
{
  public class ClientConnectedEventArgs : EventArgs
  {
    private Socket socket;
    private TcpClient client;

    public ClientConnectedEventArgs(TcpClient client) => this.client = client;

    public ClientConnectedEventArgs(Socket socket) => this.socket = socket;

    public Socket ConnectedSocket => this.socket;

    public TcpClient ConnectedClient => this.client;
  }
}
