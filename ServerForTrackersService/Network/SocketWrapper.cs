// Decompiled with JetBrains decompiler
// Type: Nal.Network.SocketWrapper
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
  public class SocketWrapper
  {
    private int port;
    private string host;
    private IPAddress ipAddr;
    private bool isIpAddress;
    private Socket socket;
    private List<byte> sendQueue;
    private byte[] sendBuffer;
    private int sendBufferAmt;
    private byte[] receiveBuffer;
    private BackgroundWorker connectWorker;
    private BackgroundWorker sendWorker;
    private BackgroundWorker receiveWorker;
    private bool isShuttingDown;
    private bool isDoneSending;
    private bool isOtherSideDoneSending;

    public SocketWrapper()
    {
      this.sendQueue = new List<byte>();
      this.sendBuffer = new byte[256];
      this.receiveBuffer = new byte[256];
      this.isDoneSending = true;
      this.isOtherSideDoneSending = true;
    }

    public event EventHandler<ExceptionEventArgs> ConnectFailed;

    public event EventHandler ConnectionMade;

    public event EventHandler<DataReceivedEventArgs> DataReceived;

    public event EventHandler FinReceived;

    public event EventHandler<ExceptionEventArgs> ConnectionClosed;

    public bool Connected => this.socket != null && !this.Connecting;

    public bool Connecting => this.connectWorker != null && this.connectWorker.IsBusy;

    public IPEndPoint LocalEndPoint => (IPEndPoint) this.socket.LocalEndPoint;

    public IPEndPoint RemoteEndPoint => (IPEndPoint) this.socket.RemoteEndPoint;

    public void TakeOverConnection(Socket connectedSocket)
    {
      if (this.Connected || this.Connecting)
        throw new Exception("Cannot take over a connection while connected or connecting.");
      this.socket = connectedSocket.Connected ? connectedSocket : throw new Exception("The socket is not a connected socket.");
      this.InitializeSendingAndReceiving();
    }

    public void Connect(string host, int port)
    {
      if (this.Connected)
        return;
      if (this.Connecting)
        return;
      try
      {
        this.ipAddr = IPAddress.Parse(host);
        this.isIpAddress = true;
      }
      catch (Exception ex)
      {
        this.host = host;
        this.isIpAddress = false;
      }
      this.port = port;
      this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.connectWorker = new BackgroundWorker();
      this.connectWorker.DoWork += new DoWorkEventHandler(this.ConnectWorkerDoWork);
      this.connectWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ConnectWorkerCompleted);
      this.connectWorker.RunWorkerAsync();
    }

    public void Shutdown()
    {
      if (!this.Connected || this.isShuttingDown)
        return;
      this.isShuttingDown = true;
      if (this.sendWorker != null || this.sendQueue.Count != 0)
        return;
      this.DoShutdown();
    }

    public void Disconnect()
    {
      if (!this.Connected)
        return;
      this.socket.Shutdown(SocketShutdown.Both);
      this.socket.Close();
      this.socket = (Socket) null;
    }

    public void Send(byte[] buffer) => this.Send(buffer, 0, buffer.Length);

    public void Send(byte[] buffer, int offset, int size)
    {
      if (!this.Connected || this.isShuttingDown)
        return;
      int num = offset + size;
      for (int index = offset; index < num; ++index)
        this.sendQueue.Add(buffer[index]);
      if (this.sendWorker != null || this.sendQueue.Count <= 0)
        return;
      this.RunSendWorker();
    }

    private void ConnectWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      if (this.isIpAddress)
        this.socket.Connect(this.ipAddr, this.port);
      else
        this.socket.Connect(this.host, this.port);
    }

    private void ConnectWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        this.InitializeSendingAndReceiving();
        if (this.ConnectionMade == null)
          return;
        this.ConnectionMade((object) this, EventArgs.Empty);
      }
      else
      {
        this.socket.Close();
        this.socket = (Socket) null;
        if (this.ConnectFailed == null)
          return;
        this.ConnectFailed((object) this, new ExceptionEventArgs(e.Error));
      }
    }

    private void SendWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.socket.Send(this.sendBuffer, 0, this.sendBufferAmt, SocketFlags.None);
    }

    private void SendWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.sendWorker = (BackgroundWorker) null;
      if (e.Error == null)
      {
        this.sendQueue.RemoveRange(0, (int) e.Result);
        if (this.sendQueue.Count > 0)
        {
          this.RunSendWorker();
        }
        else
        {
          if (!this.isShuttingDown)
            return;
          this.DoShutdown();
        }
      }
      else
      {
        if (this.socket == null)
          return;
        this.EndConnection(e.Error);
      }
    }

    private void ReceiveWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = (object) this.socket.Receive(this.receiveBuffer);
    }

    private void ReceiveWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        int result = (int) e.Result;
        if (result == 0)
        {
          this.isOtherSideDoneSending = true;
          if (this.FinReceived != null)
            this.FinReceived((object) this, EventArgs.Empty);
          if (!this.Connected)
            return;
          if (this.isDoneSending)
            this.EndConnection((Exception) null);
          else
            this.Shutdown();
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
        if (this.socket == null)
          return;
        this.EndConnection(e.Error);
      }
    }

    private void InitializeSendingAndReceiving()
    {
      this.isShuttingDown = false;
      this.isDoneSending = false;
      this.isOtherSideDoneSending = false;
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

    private void DoShutdown()
    {
      this.socket.Shutdown(SocketShutdown.Send);
      this.isDoneSending = true;
      if (!this.isOtherSideDoneSending)
        return;
      this.EndConnection((Exception) null);
    }

    private void EndConnection(Exception exception)
    {
      try
      {
        this.Disconnect();
      }
      finally
      {
        if (this.ConnectionClosed != null)
          this.ConnectionClosed((object) this, new ExceptionEventArgs(exception));
      }
    }
  }
}
