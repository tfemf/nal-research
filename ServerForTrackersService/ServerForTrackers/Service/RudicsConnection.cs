// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.RudicsConnection
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.DataCallPackets;
using Nal.EncryptionModule;
using Nal.Network;
using Nal.ServerForTrackers.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public abstract class RudicsConnection : CommLink
  {
    private CallProtocol callProtocol;
    private bool enablePipe;
    private string pipeHost;
    private int pipePort;
    private SocketWrapper pipeSocket;
    private List<byte> pipeBuffer;
    private PacketDataCallHandler packetDataCallHandler;
    private List<byte> callReceivedBuffer;
    private DateTime startOfConnection;
    private bool inConnection;
    private Timer removeTimer;

    public RudicsConnection()
    {
      this.inConnection = false;
      this.callReceivedBuffer = new List<byte>();
      this.pipeBuffer = new List<byte>();
      this.packetDataCallHandler = new PacketDataCallHandler((ISynchronizeInvoke) ServerForTrackersService.HiddenForm);
      this.packetDataCallHandler.Send = new PacketDataCallHandler.SendDelegate(this.Send);
      this.packetDataCallHandler.Disconnect = new PacketDataCallHandler.DisconnectDelegate(this.Disconnect);
      this.packetDataCallHandler.DataReceived += new PacketDataCallHandler.DataEvent(this.OnPacketDataCallHandlerDataReceived);
      this.pipeSocket = new SocketWrapper();
      this.pipeSocket.ConnectionMade += new EventHandler(this.OnPipeSocketConnectionMade);
      this.pipeSocket.ConnectFailed += new EventHandler<ExceptionEventArgs>(this.OnPipeSocketConnectFailed);
      this.pipeSocket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnPipeSocketConnectionClosed);
      this.pipeSocket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnPipeSocketDataReceived);
      this.removeTimer = new Timer();
      this.removeTimer.Interval = 300000;
      this.removeTimer.Tick += new EventHandler(this.OnRemoveTimerTick);
      this.removeTimer.Start();
    }

    public CallProtocol CallProtocol
    {
      protected get => this.callProtocol;
      set => this.callProtocol = value;
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

    public bool InConnection => this.inConnection;

    public abstract void Disconnect();

    protected abstract void Send(byte[] data);

    protected void HandleConnect(bool isInitiator)
    {
      this.Status = "Connected";
      this.inConnection = true;
      this.startOfConnection = DateTime.UtcNow;
      this.callReceivedBuffer.Clear();
      if (this.EnablePipe)
      {
        this.pipeBuffer.Clear();
        try
        {
          this.pipeSocket.Connect(this.pipeHost, this.pipePort);
        }
        catch (Exception ex)
        {
          this.Activity = "Could not open the pipe - Exception: " + ex.Message + "\r\n";
        }
      }
      if (this.CallProtocol != CallProtocol.Packets)
        return;
      this.packetDataCallHandler.BeginHandshaking(isInitiator, CommLink.UseEncryption ? CommLink.EncryptionUser : (EncryptionUser) null);
    }

    protected void HandleDisconnect(string finalWords)
    {
      this.Status = "Disconnected";
      this.inConnection = false;
      if (this.callProtocol == CallProtocol.Packets)
        this.packetDataCallHandler.HandleDisconnect();
      this.pipeSocket.Disconnect();
      this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(DataType.Other), ModemInfoTypes.Unknown, string.Empty, Protocol.Call, this.startOfConnection, (IList<byte>) this.callReceivedBuffer));
      this.SelfDestruct(finalWords);
    }

    protected void HandleDataReceived(byte[] data)
    {
      this.removeTimer.Stop();
      this.removeTimer.Start();
      if (this.inConnection)
      {
        if (this.callProtocol == CallProtocol.Packets)
        {
          this.packetDataCallHandler.HandleReceivedData(data);
        }
        else
        {
          if (this.callProtocol != CallProtocol.None)
            return;
          this.HandleUnpackedData(data);
        }
      }
      else
        this.Activity = Nal.ServerForTrackers.Common.Utils.FormatForDisplay(Encoding.GetEncoding(1252).GetString(data));
    }

    protected void SelfDestruct(string finalWords)
    {
      this.removeTimer.Stop();
      this.TriggerRemoveMe(finalWords);
    }

    private void OnPacketDataCallHandlerDataReceived(
      object sender,
      PacketDataCallHandler.DataEventArgs e)
    {
      this.HandleUnpackedData(e.Data);
    }

    private void OnPipeSocketConnectionMade(object sender, EventArgs e)
    {
      this.Status = "Pipe Opened";
      if (this.pipeBuffer.Count <= 0)
        return;
      this.pipeSocket.Send(this.pipeBuffer.ToArray());
    }

    private void OnPipeSocketConnectFailed(object sender, EventArgs e)
    {
      this.Status = "Pipe Could Not Be Opened";
    }

    private void OnPipeSocketConnectionClosed(object sender, EventArgs e)
    {
      this.Status = "Pipe Connection Dropped";
    }

    private void OnPipeSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      if (!this.inConnection)
        return;
      if (this.callProtocol == CallProtocol.Packets)
      {
        this.packetDataCallHandler.QueueToSend(e.Data);
      }
      else
      {
        if (this.callProtocol != CallProtocol.None)
          return;
        this.Send(e.Data);
      }
    }

    private void OnRemoveTimerTick(object sender, EventArgs e) => this.Disconnect();

    private void HandleUnpackedData(byte[] data)
    {
      this.callReceivedBuffer.AddRange((IEnumerable<byte>) data);
      this.Activity = Nal.ServerForTrackers.Common.Utils.FormatForDisplay(Encoding.GetEncoding(1252).GetString(data));
      if (!this.enablePipe)
        return;
      if (this.pipeSocket.Connected)
        this.pipeSocket.Send(data);
      else
        this.pipeBuffer.AddRange((IEnumerable<byte>) data);
    }
  }
}
