// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.RudicsAccount
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using Nal.ServerForTrackers.Common;
using System;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class RudicsAccount : CommLink
  {
    private TcpListenerWrapper listener;
    private string displayName;
    private bool enablePipe;
    private string pipeHost;
    private int pipePort;
    private CallProtocol clientHandlerCallProtocol;
    private string clientHost;
    private int clientPortsBegin;
    private int clientPortsEnd;
    private CallProtocol clientCallProtocol;

    public RudicsAccount()
    {
      this.listener = new TcpListenerWrapper(true);
      this.listener.ClientConnected += new EventHandler<ClientConnectedEventArgs>(this.OnListenerClientConnected);
    }

    public event EventHandler<ClientConnectedEventArgs> ClientConnected;

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

    public string DisplayName
    {
      get => this.displayName;
      set
      {
        this.displayName = value;
        this.Name = "RUDICS Account (" + this.displayName + ")";
      }
    }

    public bool EnablePipe
    {
      get => this.enablePipe;
      set => this.enablePipe = value;
    }

    public string PipeHost
    {
      get => this.pipeHost;
      set => this.pipeHost = value;
    }

    public int PipePort
    {
      get => this.pipePort;
      set => this.pipePort = value;
    }

    public CallProtocol ClientHandlerCallProtocol
    {
      get => this.clientHandlerCallProtocol;
      set => this.clientHandlerCallProtocol = value;
    }

    public string ClientHost
    {
      get => this.clientHost;
      set => this.clientHost = value;
    }

    public int ClientPortsBegin
    {
      get => this.clientPortsBegin;
      set => this.clientPortsBegin = value;
    }

    public int ClientPortsEnd
    {
      get => this.clientPortsEnd;
      set => this.clientPortsEnd = value;
    }

    public CallProtocol ClientCallProtocol
    {
      get => this.clientCallProtocol;
      set => this.clientCallProtocol = value;
    }

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
      this.Activity = "Connection: " + e.ConnectedSocket.RemoteEndPoint.ToString() + "\r\n";
      this.ClientConnected((object) this, e);
    }

    private void UpdateStatus()
    {
      this.Status = (this.listener.Running ? (object) "Listening" : (object) "Not Listening").ToString() + ", Port: " + (object) this.listener.Port;
    }
  }
}
