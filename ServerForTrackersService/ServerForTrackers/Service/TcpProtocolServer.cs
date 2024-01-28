// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.TcpProtocolServer
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using System;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class TcpProtocolServer : CommLink
  {
    private string displayName;
    private TcpListenerWrapper listener;

    public TcpProtocolServer()
    {
      this.listener = new TcpListenerWrapper(true);
      this.listener.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnListenerClientConnected);
    }

    public event EventHandler<ClientConnectedEventArgs> ClientConnected;

    public string DisplayName
    {
      get => this.displayName;
      set
      {
        this.displayName = value;
        this.Name = "TCP Protocol Server (" + this.displayName + ")";
      }
    }

    public int ServerPort
    {
      get => this.listener.Port;
      set
      {
        if (this.listener.Port == value)
          return;
        this.listener.Port = value;
        this.UpdateStatus();
      }
    }

    public bool Listening => this.listener.Running;

    public void StopListening()
    {
      this.listener.Shutdown();
      this.UpdateStatus();
    }

    public void StartListening()
    {
      this.listener.Startup();
      this.UpdateStatus();
    }

    private void OnListenerClientConnected(object sender, ClientConnectedEventArgs e)
    {
      if (this.ClientConnected == null)
        return;
      this.Activity = "Connection: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " " + e.ConnectedSocket.RemoteEndPoint.ToString() + "\r\n";
      this.ClientConnected((object) this, e);
    }

    private void UpdateStatus()
    {
      this.Status = (this.listener.Running ? (object) "Listening" : (object) "Not Listening").ToString() + ", Port: " + (object) this.listener.Port;
    }
  }
}
