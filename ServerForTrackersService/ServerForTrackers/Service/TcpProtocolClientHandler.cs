// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.TcpProtocolClientHandler
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.EncryptionModule;
using Nal.Network;
using Nal.ServerForTrackers.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class TcpProtocolClientHandler : CommLink
  {
    private SocketWrapper socket;
    private TcpServerQueues queues;
    private Timer timeoutTimer;
    private DateTime connectionStartTime;
    private bool waitingForAck;
    private TcpProtocolClientHandler.ReceiveState receiveState;
    private ulong imei;
    private List<byte> data;
    private int length;
    private int count;
    private int subLength;
    private int subCount;
    private byte tag;
    private TcpProtocolClientHandler.PacketFlags packetFlags;

    public TcpProtocolClientHandler(Socket socket, TcpServerQueues queues)
    {
      this.connectionStartTime = DateTime.UtcNow;
      this.data = new List<byte>();
      this.socket = new SocketWrapper();
      this.socket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnSocketDataReceived);
      this.socket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectionClosed);
      this.socket.TakeOverConnection(socket);
      this.queues = queues;
      this.timeoutTimer = new Timer();
      this.timeoutTimer.Tick += new EventHandler(this.OnTimeoutTimerTick);
      this.timeoutTimer.Interval = 60000;
      this.timeoutTimer.Start();
      this.Name = "TCP Protocol Client Handler (" + socket.RemoteEndPoint.ToString() + ")";
      this.Status = "Connected";
      this.Activity = "Connected\r\n";
    }

    public event TcpProtocolClientHandler.PacketSentEventHandler PacketSent;

    public void Disconnect()
    {
      this.timeoutTimer.Stop();
      this.socket.Disconnect();
      this.TriggerRemoveMe(string.Empty);
    }

    private void OnSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      foreach (byte num in e.Data)
      {
        if ((this.receiveState == TcpProtocolClientHandler.ReceiveState.PacketHeaderIETag || this.receiveState == TcpProtocolClientHandler.ReceiveState.PacketHeaderIELen || this.receiveState == TcpProtocolClientHandler.ReceiveState.PacketHeaderIEData) && ++this.count > this.length)
        {
          this.Disconnect();
          break;
        }
        switch (this.receiveState)
        {
          case TcpProtocolClientHandler.ReceiveState.ProtocolVersion:
            if (num == (byte) 0)
            {
              this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketFlags;
              break;
            }
            this.Disconnect();
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketFlags:
            this.packetFlags = (TcpProtocolClientHandler.PacketFlags) num;
            this.length = 0;
            this.count = 0;
            this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketHeaderLen;
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketHeaderLen:
            this.length = this.length * 256 + (int) num;
            if (++this.count == 2)
            {
              this.count = 0;
              this.HandleHeaderIEProcessed();
              break;
            }
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketHeaderIETag:
            this.tag = num;
            this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketHeaderIELen;
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketHeaderIELen:
            this.subLength = (int) num;
            this.data.Clear();
            if (this.subLength > 0)
            {
              this.subCount = 0;
              this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketHeaderIEData;
              break;
            }
            this.ProcessPacketHeaderIE();
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketHeaderIEData:
            this.data.Add(num);
            if (++this.subCount == this.subLength)
            {
              this.ProcessPacketHeaderIE();
              break;
            }
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketPayloadLen:
            this.length = this.length * 256 + (int) num;
            if (++this.count == 2)
            {
              this.data.Clear();
              if (this.length > 0)
              {
                this.count = 0;
                this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketPayload;
                break;
              }
              this.ProcessPacket();
              break;
            }
            break;
          case TcpProtocolClientHandler.ReceiveState.PacketPayload:
            this.data.Add(num);
            if (++this.count == this.length)
            {
              this.ProcessPacket();
              break;
            }
            break;
        }
        if (!this.socket.Connected)
          break;
      }
    }

    private void OnSocketConnectionClosed(object sender, EventArgs args)
    {
      this.timeoutTimer.Stop();
      this.TriggerRemoveMe(string.Empty);
    }

    private void OnTimeoutTimerTick(object sender, EventArgs e) => this.Disconnect();

    private void ProcessPacketHeaderIE()
    {
      if (this.tag == (byte) 0)
      {
        if (this.data.Count == 8)
        {
          this.imei = 0UL;
          foreach (ulong num in this.data)
            this.imei = this.imei * 256UL + num;
          if (this.imei > 999999999999999UL)
            this.Disconnect();
        }
        else
          this.Disconnect();
      }
      if (!this.socket.Connected)
        return;
      this.HandleHeaderIEProcessed();
    }

    private void HandleHeaderIEProcessed()
    {
      if (this.count < this.length)
      {
        this.count = 0;
        this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketHeaderIETag;
      }
      else
      {
        this.length = 0;
        this.count = 0;
        this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketPayloadLen;
      }
    }

    private void ProcessPacket()
    {
      string str = this.imei == 0UL ? string.Empty : this.imei.ToString().PadLeft(15, '0');
      if (this.data.Count > 0)
        this.TriggerDataReceived(new CommLink.DataReceivedEventArgs(new DataType?(), this.imei == 0UL ? ModemInfoTypes.Unknown : ModemInfoTypes.Imei, str, Protocol.Tcp, this.connectionStartTime, (IList<byte>) this.data));
      if (this.waitingForAck && (this.packetFlags & TcpProtocolClientHandler.PacketFlags.Ack) != TcpProtocolClientHandler.PacketFlags.None)
      {
        byte[] data = this.queues.Peek(str);
        if (data != null)
        {
          this.PacketSent((object) this, new TcpProtocolClientHandler.PacketSentEventArgs(str, (IList<byte>) data));
          this.queues.Dequeue(str);
          this.queues.Save();
        }
        this.waitingForAck = false;
      }
      TcpProtocolClientHandler.PacketFlags packetFlags = TcpProtocolClientHandler.PacketFlags.None;
      if ((this.packetFlags & TcpProtocolClientHandler.PacketFlags.NeedsAck) != TcpProtocolClientHandler.PacketFlags.None)
        packetFlags |= TcpProtocolClientHandler.PacketFlags.Ack;
      byte[] numArray = (byte[]) null;
      if (!this.waitingForAck)
      {
        numArray = this.queues.Peek(str);
        if (CommLink.UseEncryption)
        {
          byte[] encryptedBytes = (byte[]) null;
          numArray = CommLink.EncryptionUser.Encrypt(str, "I", numArray, ref encryptedBytes) != Erc.Success ? (byte[]) null : encryptedBytes;
        }
        if (numArray == null)
        {
          packetFlags |= TcpProtocolClientHandler.PacketFlags.Fin;
        }
        else
        {
          packetFlags |= TcpProtocolClientHandler.PacketFlags.NeedsAck;
          this.waitingForAck = true;
        }
      }
      if ((packetFlags & TcpProtocolClientHandler.PacketFlags.Fin) != TcpProtocolClientHandler.PacketFlags.Fin)
      {
        int num = this.queues.Count(str);
        if (num == 1 && !this.waitingForAck || num > 1)
          packetFlags |= TcpProtocolClientHandler.PacketFlags.More;
      }
      if (packetFlags != TcpProtocolClientHandler.PacketFlags.None)
      {
        List<byte> byteList = new List<byte>();
        byteList.Add((byte) packetFlags);
        byteList.Add((byte) 0);
        byteList.Add((byte) 0);
        if (numArray == null)
        {
          byteList.Add((byte) 0);
          byteList.Add((byte) 0);
        }
        else
        {
          byteList.Add((byte) (numArray.Length / 256));
          byteList.Add((byte) (numArray.Length % 256));
          byteList.AddRange((IEnumerable<byte>) numArray);
        }
        this.socket.Send(byteList.ToArray());
      }
      this.receiveState = TcpProtocolClientHandler.ReceiveState.PacketFlags;
    }

    private enum ReceiveState
    {
      ProtocolVersion,
      PacketFlags,
      PacketHeaderLen,
      PacketHeaderIETag,
      PacketHeaderIELen,
      PacketHeaderIEData,
      PacketPayloadLen,
      PacketPayload,
    }

    [Flags]
    private enum PacketFlags
    {
      None = 0,
      Ack = 1,
      NeedsAck = 2,
      Fin = 4,
      More = 8,
    }

    public delegate void PacketSentEventHandler(
      object sender,
      TcpProtocolClientHandler.PacketSentEventArgs e);

    public class PacketSentEventArgs
    {
      private string imei;
      private byte[] data;

      public PacketSentEventArgs(string imei, IList<byte> data)
      {
        this.imei = imei;
        this.data = new byte[data.Count];
        data.CopyTo(this.data, 0);
      }

      public string Imei => this.imei;

      public byte[] Data => this.data;
    }
  }
}
