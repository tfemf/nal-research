// Decompiled with JetBrains decompiler
// Type: Nal.Network.TcpListenerWrapper
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#nullable disable
namespace Nal.Network
{
  public class TcpListenerWrapper
  {
    private SynchronizationContext syncObject;
    private TcpListener tcpListener;
    private bool acceptAsSocket;

    public TcpListenerWrapper(bool acceptAsSocket)
    {
      this.acceptAsSocket = acceptAsSocket;
      this.syncObject = SynchronizationContext.Current;
      this.IpAddress = "0.0.0.0";
    }

    public event EventHandler<ClientConnectedEventArgs> ClientConnected;

    public string IpAddress { get; set; }

    public int Port { get; set; }

    public bool Running => this.tcpListener != null;

    public void Startup()
    {
      if (this.Running)
        return;
      this.tcpListener = new TcpListener(IPAddress.Parse(this.IpAddress), this.Port);
      try
      {
        this.tcpListener.Start();
        this.BeginAccept();
      }
      catch (Exception ex)
      {
        this.tcpListener = (TcpListener) null;
        throw;
      }
    }

    public void Shutdown()
    {
      if (!this.Running)
        return;
      lock (this)
      {
        this.tcpListener.Stop();
        this.tcpListener = (TcpListener) null;
      }
    }

    private void OnAcceptTcpClientCompleted(IAsyncResult ar)
    {
      object client;
      lock (this)
      {
        if (this.tcpListener == null)
          return;
        client = this.EndAccept(ar);
      }
      this.syncObject.Send((SendOrPostCallback) (s =>
      {
        if (this.tcpListener == null || this.ClientConnected == null)
        {
          try
          {
            this.CloseClient(client);
          }
          catch
          {
          }
        }
        else
          this.ClientConnected((object) this, this.CreateClientConnectedEventArgs(client));
      }), (object) null);
      lock (this)
      {
        if (this.tcpListener == null)
          return;
        this.BeginAccept();
      }
    }

    private void BeginAccept()
    {
      if (this.acceptAsSocket)
        this.tcpListener.BeginAcceptSocket(new AsyncCallback(this.OnAcceptTcpClientCompleted), (object) null);
      else
        this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(this.OnAcceptTcpClientCompleted), (object) null);
    }

    private object EndAccept(IAsyncResult ar)
    {
      return this.acceptAsSocket ? (object) this.tcpListener.EndAcceptSocket(ar) : (object) this.tcpListener.EndAcceptTcpClient(ar);
    }

    private void CloseClient(object client)
    {
      if (client is Socket)
        ((Socket) client).Close();
      else
        ((TcpClient) client).Close();
    }

    private ClientConnectedEventArgs CreateClientConnectedEventArgs(object client)
    {
      return this.acceptAsSocket ? new ClientConnectedEventArgs((Socket) client) : new ClientConnectedEventArgs((TcpClient) client);
    }
  }
}
