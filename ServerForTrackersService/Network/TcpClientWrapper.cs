// Decompiled with JetBrains decompiler
// Type: Nal.Network.TcpClientWrapper
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace Nal.Network
{
  public class TcpClientWrapper
  {
    private int port;
    private string host;
    private TcpClient client;
    private List<byte> sendQueue;
    private byte[] sendBuffer;
    private int sendBufferAmt;
    private byte[] receiveBuffer;
    private BackgroundWorker connectWorker;
    private BackgroundWorker sendWorker;
    private BackgroundWorker receiveWorker;
    private bool isFinQueued;

    public TcpClientWrapper()
      : this((TcpClient) null)
    {
    }

    public TcpClientWrapper(TcpClient client)
    {
      this.sendQueue = new List<byte>();
      this.sendBuffer = new byte[256];
      this.receiveBuffer = new byte[256];
      if (client == null)
        return;
      if (this.Connected || this.Connecting)
        throw new Exception("Cannot take over a connection while connected or connecting.");
      this.client = client.Connected ? client : throw new Exception("The socket is not a connected socket.");
      this.InitializeSendingAndReceiving();
    }

    public event EventHandler<ExceptionEventArgs> ConnectFailed;

    public event EventHandler ConnectionMade;

    public event EventHandler<DataReceivedEventArgs> DataReceived;

    public event EventHandler<ExceptionEventArgs> ConnectionClosed;

    public bool Connected => this.client != null && !this.Connecting;

    public bool Connecting => this.connectWorker != null;

    public IPEndPoint LocalEndPoint => (IPEndPoint) this.client.Client.LocalEndPoint;

    public IPEndPoint RemoteEndPoint => (IPEndPoint) this.client.Client.RemoteEndPoint;

    public void Connect(string host, int port)
    {
      if (this.Connected || this.Connecting)
        return;
      this.host = host;
      this.port = port;
      this.client = new TcpClient();
      this.connectWorker = new BackgroundWorker();
      this.connectWorker.DoWork += new DoWorkEventHandler(this.ConnectWorkerDoWork);
      this.connectWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ConnectWorkerCompleted);
      this.connectWorker.RunWorkerAsync();
    }

    public void Close()
    {
      if (!this.Connecting && !this.Connected)
        return;
      this.client.Close();
    }

    public void Send(byte[] buffer)
    {
      if (!this.Connected || this.isFinQueued)
        return;
      this.sendQueue.AddRange((IEnumerable<byte>) buffer);
      if (this.sendWorker != null || this.sendQueue.Count <= 0)
        return;
      this.RunSendWorker();
    }

    public void SendFin()
    {
      if (!this.Connected || this.isFinQueued)
        return;
      this.isFinQueued = true;
      if (this.sendWorker != null || this.sendQueue.Count != 0)
        return;
      this.DoSendFin();
    }

    private void ConnectWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.client.Connect(this.host, this.port);
    }

    private void ConnectWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.connectWorker = (BackgroundWorker) null;
      if (e.Error != null || !this.client.Connected)
      {
        this.client.Close();
        this.client = (TcpClient) null;
        if (this.ConnectFailed == null)
          return;
        this.ConnectFailed((object) this, new ExceptionEventArgs(e.Error));
      }
      else
      {
        this.InitializeSendingAndReceiving();
        if (this.ConnectionMade == null)
          return;
        this.ConnectionMade((object) this, EventArgs.Empty);
      }
    }

    private void SendWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.client.GetStream().Write(this.sendBuffer, 0, this.sendBufferAmt);
    }

    private void SendWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.sendWorker = (BackgroundWorker) null;
      if (e.Error == null)
      {
        this.sendQueue.RemoveRange(0, this.sendBufferAmt);
        if (this.sendQueue.Count > 0)
        {
          this.RunSendWorker();
        }
        else
        {
          if (!this.isFinQueued)
            return;
          this.DoSendFin();
        }
      }
      else
      {
        if (this.client == null)
          return;
        this.CloseConnection(e.Error);
      }
    }

    private void ReceiveWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.client.GetStream().Read(this.receiveBuffer, 0, this.receiveBuffer.Length);
    }

    private void ReceiveWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        int result = (int) e.Result;
        if (result == 0)
        {
          if (this.sendQueue.Count == 0)
            this.CloseConnection();
          else
            this.CloseConnection(new Exception("Connection closed by remote side while data still in send queue."));
        }
        else
        {
          if (this.DataReceived != null)
            this.DataReceived((object) this, new DataReceivedEventArgs(this.receiveBuffer, 0, result));
          if (!this.Connected)
            return;
          this.RunReceiveWorker();
        }
      }
      else
      {
        if (this.client == null)
          return;
        this.CloseConnection(e.Error);
      }
    }

    private void InitializeSendingAndReceiving()
    {
      this.isFinQueued = false;
      this.RunReceiveWorker();
    }

    private void RunSendWorker()
    {
      this.sendBufferAmt = Math.Min(this.sendBuffer.Length, this.sendQueue.Count);
      for (int index = 0; index < this.sendBufferAmt; ++index)
        this.sendBuffer[index] = this.sendQueue[index];
      this.sendWorker = new BackgroundWorker();
      this.sendWorker.DoWork += new DoWorkEventHandler(this.SendWorkerDoWork);
      this.sendWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SendWorkerCompleted);
      this.sendWorker.RunWorkerAsync();
    }

    private void RunReceiveWorker()
    {
      this.receiveWorker = new BackgroundWorker();
      this.receiveWorker.DoWork += new DoWorkEventHandler(this.ReceiveWorkerDoWork);
      this.receiveWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ReceiveWorkerCompleted);
      this.receiveWorker.RunWorkerAsync();
    }

    private void DoSendFin() => this.client.Client.Shutdown(SocketShutdown.Send);

    private void CloseConnection() => this.CloseConnection((Exception) null, true);

    private void CloseConnection(Exception exception) => this.CloseConnection(exception, true);

    private void CloseConnection(Exception exception, bool raiseEvent)
    {
      try
      {
        this.client.Close();
      }
      catch
      {
      }
      this.client = (TcpClient) null;
      if (!raiseEvent || this.ConnectionClosed == null)
        return;
      this.ConnectionClosed((object) this, new ExceptionEventArgs(exception));
    }
  }
}
