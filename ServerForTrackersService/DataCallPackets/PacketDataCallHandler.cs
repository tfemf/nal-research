// Decompiled with JetBrains decompiler
// Type: Nal.DataCallPackets.PacketDataCallHandler
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.EncryptionModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;

#nullable disable
namespace Nal.DataCallPackets
{
  public class PacketDataCallHandler
  {
    private const int WindowSize = 4;
    private const int SendTimerInterval = 2000;
    private const int ResendTimerInterval = 5000;
    private const int SendThreshold = 45;
    private const int MaxPacketNumbers = 16;
    private const int Ack = 0;
    private const int PassiveInitial = 1;
    private const int Initial = 2;
    private const int AckInitial = 3;
    private const int Data = 4;
    private const int AckData = 5;
    private const byte XOffByte = 19;
    private const byte XOnByte = 17;
    private const byte StartByte = 1;
    private const byte EndByte = 4;
    private const byte EscapeByte = 27;
    private PacketDataCallHandler.SendDelegate send;
    private PacketDataCallHandler.DisconnectDelegate disconnect;
    private List<byte> sendBuffer;
    private PacketDataCallHandler.SendWindowSlot[] sendWindow;
    private int sendWindowStart = -1;
    private int sendWindowEnd = -1;
    private TimerWrapper sendTimer;
    private List<byte> receiveBuffer;
    private List<byte>[] receiveWindow;
    private int receiveWindowStart = -1;
    private int receiveWindowEnd = -1;
    private EncryptionUser encryptionUser;
    private byte[] known;
    private bool startByteSeen;
    private string remoteImei;
    private bool handshaking;

    public PacketDataCallHandler(ISynchronizeInvoke synchronizationObject)
    {
      this.known = new byte[12]
      {
        (byte) 0,
        (byte) 12,
        (byte) 7,
        (byte) 81,
        (byte) 14,
        (byte) 111,
        (byte) 9,
        (byte) 200,
        (byte) 251,
        (byte) 16,
        (byte) 82,
        byte.MaxValue
      };
      this.sendTimer = new TimerWrapper(synchronizationObject);
      this.sendTimer.Elapsed += new ElapsedEventHandler(this.OnSendTimerElapsed);
      this.sendTimer.Interval = 2000.0;
      this.sendBuffer = new List<byte>();
      this.receiveBuffer = new List<byte>();
      this.sendWindow = new PacketDataCallHandler.SendWindowSlot[4];
      this.receiveWindow = new List<byte>[4];
      for (int index = 0; index < 4; ++index)
      {
        this.receiveWindow[index] = (List<byte>) null;
        this.sendWindow[index] = new PacketDataCallHandler.SendWindowSlot();
        this.sendWindow[index].PacketData = (byte[]) null;
        this.sendWindow[index].ResendTimer = new TimerWrapper(synchronizationObject);
        this.sendWindow[index].ResendTimer.Elapsed += new ElapsedEventHandler(this.OnResendTimerElapsed);
        this.sendWindow[index].ResendTimer.Interval = 5000.0;
      }
    }

    public event PacketDataCallHandler.DataEvent DataReceived;

    public PacketDataCallHandler.SendDelegate Send
    {
      set => this.send = value;
    }

    public PacketDataCallHandler.DisconnectDelegate Disconnect
    {
      set => this.disconnect = value;
    }

    public void BeginHandshaking(bool isInitiator, EncryptionUser encryptionUser)
    {
      this.encryptionUser = encryptionUser;
      this.handshaking = true;
      this.sendTimer.Start();
      if (!isInitiator)
        return;
      List<byte> passiveInitialPacket = this.CreatePassiveInitialPacket(this.GetNextOutgoingPacketNumber());
      this.AddPacketToSendWindow(passiveInitialPacket);
      this.SendPacket(passiveInitialPacket);
    }

    public void HandleDisconnect()
    {
      this.sendTimer.Stop();
      this.sendBuffer.Clear();
      this.ClearSendWindow();
      this.ClearReceiveWindow();
    }

    public void HandleReceivedData(byte[] data)
    {
      foreach (byte num in data)
      {
        if (num == (byte) 1)
        {
          if (this.startByteSeen)
            this.receiveBuffer.Clear();
          else
            this.startByteSeen = true;
        }
        if (this.startByteSeen)
        {
          this.receiveBuffer.Add(num);
          if (num == (byte) 4)
          {
            if (this.receiveBuffer.Count >= 5)
              this.ProcessPacket(new List<byte>((IEnumerable<byte>) this.receiveBuffer));
            this.receiveBuffer.Clear();
            this.startByteSeen = false;
          }
        }
      }
    }

    public void QueueToSend(byte[] data)
    {
      this.sendBuffer.AddRange((IEnumerable<byte>) data);
      if (this.handshaking)
        return;
      this.SendMaxSizeDataPackets();
    }

    private void AddChecksum(List<byte> packet)
    {
      ushort crc = this.ComputeCrc(packet);
      packet.Add((byte) ((uint) crc >> 8));
      packet.Add((byte) ((uint) crc & (uint) byte.MaxValue));
    }

    private void AddPacketTags(List<byte> packet)
    {
      packet.Insert(0, (byte) 1);
      packet.Add((byte) 4);
    }

    private void AddEscapeSequences(List<byte> packet)
    {
      for (int index = packet.Count - 2; index >= 1; --index)
      {
        if (packet[index] == (byte) 17)
        {
          packet[index] = (byte) 49;
          packet.Insert(index, (byte) 27);
        }
        else if (packet[index] == (byte) 19)
        {
          packet[index] = (byte) 48;
          packet.Insert(index, (byte) 27);
        }
        else if (packet[index] == (byte) 1)
        {
          packet[index] = (byte) 115;
          packet.Insert(index, (byte) 27);
        }
        else if (packet[index] == (byte) 4)
        {
          packet[index] = (byte) 101;
          packet.Insert(index, (byte) 27);
        }
        else if (packet[index] == (byte) 27)
        {
          packet[index] = (byte) 120;
          packet.Insert(index, (byte) 27);
        }
      }
    }

    private void ClearSendWindow()
    {
      this.sendWindowStart = this.sendWindowEnd;
      for (int index = 0; index < this.sendWindow.Length; ++index)
      {
        PacketDataCallHandler.SendWindowSlot sendWindowSlot = this.sendWindow[index];
        if (sendWindowSlot.PacketData != null)
        {
          sendWindowSlot.PacketData = (byte[]) null;
          sendWindowSlot.ResendTimer.Stop();
        }
      }
    }

    private void ClearReceiveWindow()
    {
      this.receiveWindowStart = -1;
      this.receiveWindowEnd = -1;
      for (int index = 0; index < this.receiveWindow.Length; ++index)
        this.receiveWindow[index] = (List<byte>) null;
    }

    private void RemoveEscapeSequences(List<byte> packet)
    {
      bool flag = false;
      int index = 1;
      while (index < packet.Count - 1)
      {
        if (flag)
        {
          switch (packet[index])
          {
            case 48:
              packet[index] = (byte) 19;
              break;
            case 49:
              packet[index] = (byte) 17;
              break;
            case 101:
              packet[index] = (byte) 4;
              break;
            case 115:
              packet[index] = (byte) 1;
              break;
            case 120:
              packet[index] = (byte) 27;
              break;
            default:
              packet.Clear();
              break;
          }
          flag = false;
          ++index;
        }
        else if (packet[index] == (byte) 27)
        {
          packet.RemoveAt(index);
          flag = true;
        }
        else
          ++index;
      }
    }

    private bool CompareBytes(byte[] bytesA, int startA, byte[] bytesB, int startB, int count)
    {
      for (int index = 0; index < count; ++index)
      {
        if ((int) bytesA[startA + index] != (int) bytesB[startB + index])
          return false;
      }
      return true;
    }

    private ushort ComputeCrc(List<byte> data)
    {
      ushort num1 = 4129;
      ushort crc = ushort.MaxValue;
      for (int index1 = 0; index1 < data.Count; ++index1)
      {
        ushort num2 = (ushort) ((uint) data[index1] << 8);
        for (ushort index2 = 0; index2 < (ushort) 8; ++index2)
        {
          if ((((int) crc ^ (int) num2) & 32768) != 0)
            crc = (ushort) ((uint) crc << 1 ^ (uint) num1);
          else
            crc <<= 1;
          num2 <<= 1;
        }
      }
      return crc;
    }

    private bool IsInReceiveWindow(int packetNumber)
    {
      if (this.receiveWindowStart <= this.receiveWindowEnd)
        return packetNumber > this.receiveWindowStart && packetNumber <= this.receiveWindowEnd;
      if (packetNumber > this.receiveWindowStart && packetNumber < 16)
        return true;
      return packetNumber <= this.receiveWindowEnd && packetNumber >= 0;
    }

    private bool IsInSendWindow(int packetNumber)
    {
      if (this.sendWindowStart <= this.sendWindowEnd)
        return packetNumber > this.sendWindowStart && packetNumber <= this.sendWindowEnd;
      if (packetNumber > this.sendWindowStart && packetNumber < 16)
        return true;
      return packetNumber <= this.sendWindowEnd && packetNumber >= 0;
    }

    private List<byte> ExtractUptoThresholdBytesFromBuffer()
    {
      List<byte> range = this.sendBuffer.GetRange(0, Math.Min(this.sendBuffer.Count, 45));
      this.sendBuffer.RemoveRange(0, range.Count);
      return range;
    }

    private void SendDataPackets()
    {
      while (this.sendBuffer.Count > 0 && this.GetActualSendWindowSize() < 4)
      {
        List<byte> thresholdBytesFromBuffer = this.ExtractUptoThresholdBytesFromBuffer();
        List<byte> dataPacket = this.CreateDataPacket(this.GetNextOutgoingPacketNumber(), thresholdBytesFromBuffer);
        this.AddPacketToSendWindow(dataPacket);
        this.SendPacket(dataPacket);
      }
    }

    private void SendMaxSizeDataPackets()
    {
      while (this.sendBuffer.Count >= 45 && this.GetActualSendWindowSize() < 4)
      {
        List<byte> range = this.sendBuffer.GetRange(0, 45);
        this.sendBuffer.RemoveRange(0, 45);
        List<byte> dataPacket = this.CreateDataPacket(this.GetNextOutgoingPacketNumber(), range);
        this.AddPacketToSendWindow(dataPacket);
        this.SendPacket(dataPacket);
        this.sendTimer.Restart();
      }
    }

    private void SendPacket(List<byte> packet)
    {
      if (this.send == null)
        return;
      this.send(packet.ToArray());
    }

    private List<byte> CreatePassiveInitialPacket(int packetNumber)
    {
      List<byte> packet = new List<byte>();
      packet.Add((byte) (16 + packetNumber));
      this.AddChecksum(packet);
      this.AddPacketTags(packet);
      this.AddEscapeSequences(packet);
      return packet;
    }

    private List<byte> CreateAckPacket(int packetNumber)
    {
      List<byte> packet = new List<byte>();
      packet.Add((byte) packetNumber);
      this.AddChecksum(packet);
      this.AddPacketTags(packet);
      this.AddEscapeSequences(packet);
      return packet;
    }

    private List<byte> CreateAckInitialPacket(int packetNumber, int ackNumber)
    {
      List<byte> packet = new List<byte>();
      packet.Add((byte) (48 + packetNumber));
      packet.Add((byte) ackNumber);
      packet.Add(this.encryptionUser != null ? (byte) 2 : (byte) 0);
      if (this.encryptionUser != null)
      {
        byte[] bytes = new byte[16];
        Random random = new Random();
        this.known.CopyTo((Array) bytes, 0);
        for (int index = 12; index < 16; ++index)
          bytes[index] = (byte) random.Next(0, 256);
        byte[] encryptedBytes = (byte[]) null;
        if (this.encryptionUser.Encrypt(this.remoteImei, "I", bytes, ref encryptedBytes) != Erc.Success)
          return (List<byte>) null;
        packet.AddRange((IEnumerable<byte>) encryptedBytes);
      }
      this.AddChecksum(packet);
      this.AddPacketTags(packet);
      this.AddEscapeSequences(packet);
      return packet;
    }

    private List<byte> CreateDataPacketBody(List<byte> payload)
    {
      List<byte> dataPacketBody = new List<byte>();
      if (this.encryptionUser == null)
      {
        dataPacketBody.AddRange((IEnumerable<byte>) payload);
      }
      else
      {
        dataPacketBody.Add((byte) ((16 - payload.Count % 16) % 16));
        byte[] encryptedBytes = (byte[]) null;
        if (this.encryptionUser.Encrypt(this.remoteImei, "I", payload.ToArray(), ref encryptedBytes) != Erc.Success)
          return (List<byte>) null;
        dataPacketBody.AddRange((IEnumerable<byte>) encryptedBytes);
      }
      return dataPacketBody;
    }

    private List<byte> CreateDataPacket(int packetNumber, List<byte> payload)
    {
      List<byte> packet = new List<byte>();
      packet.Add((byte) (64 + packetNumber));
      packet.AddRange((IEnumerable<byte>) this.CreateDataPacketBody(payload));
      this.AddChecksum(packet);
      this.AddPacketTags(packet);
      this.AddEscapeSequences(packet);
      return packet;
    }

    private List<byte> CreateAckDataPacket(int packetNumber, int ackNumber, List<byte> payload)
    {
      List<byte> packet = new List<byte>();
      packet.Add((byte) (80 + packetNumber));
      packet.Add((byte) ackNumber);
      packet.AddRange((IEnumerable<byte>) this.CreateDataPacketBody(payload));
      this.AddChecksum(packet);
      this.AddPacketTags(packet);
      this.AddEscapeSequences(packet);
      return packet;
    }

    private int GetActualSendWindowSize()
    {
      return this.sendWindowEnd < this.sendWindowStart ? this.sendWindowEnd + 16 - this.sendWindowStart : this.sendWindowEnd - this.sendWindowStart;
    }

    private int GetNextOutgoingPacketNumber() => (this.sendWindowEnd + 1) % 16;

    private int GetWindowIndex(int packetNumber) => packetNumber % 4;

    private int GetPacketType(List<byte> packet) => (int) packet[1] >> 4;

    private int GetPacketNumber(List<byte> packet) => (int) packet[1] & 15;

    private int GetPacketAckNumber(List<byte> packet) => (int) packet[2];

    private ushort GetPacketCrc(List<byte> packet)
    {
      return (ushort) (((uint) packet[packet.Count - 3] << 8) + (uint) packet[packet.Count - 2]);
    }

    private void OnResendTimerElapsed(object sender, ElapsedEventArgs e)
    {
      foreach (PacketDataCallHandler.SendWindowSlot sendWindowSlot in this.sendWindow)
      {
        if (sendWindowSlot.ResendTimer == sender)
        {
          if (this.send == null)
            break;
          this.send(sendWindowSlot.PacketData);
          break;
        }
      }
    }

    private void OnSendTimerElapsed(object sender, ElapsedEventArgs e)
    {
      if (this.handshaking)
        return;
      this.SendDataPackets();
    }

    private void ProcessPacket(List<byte> packet)
    {
      this.RemoveEscapeSequences(packet);
      if ((int) this.GetPacketCrc(packet) != (int) this.ComputeCrc(packet.GetRange(1, packet.Count - 4)))
        return;
      switch (this.GetPacketType(packet))
      {
        case 0:
          this.ProcessAck(this.GetPacketNumber(packet));
          break;
        case 2:
          this.ProcessInitialPacket(packet);
          break;
        case 4:
          this.AddPacketToReceiveWindow(packet);
          break;
        case 5:
          this.ProcessAck(this.GetPacketAckNumber(packet));
          this.AddPacketToReceiveWindow(packet);
          break;
      }
    }

    private void AddPacketToSendWindow(List<byte> packet)
    {
      this.sendWindowEnd = this.GetPacketNumber(packet);
      int windowIndex = this.GetWindowIndex(this.sendWindowEnd);
      this.sendWindow[windowIndex].PacketData = packet.ToArray();
      this.sendWindow[windowIndex].ResendTimer.Start();
    }

    private void AddPacketToReceiveWindow(List<byte> packet)
    {
      int packetNumber = this.GetPacketNumber(packet);
      if (this.sendBuffer.Count > 0 && this.GetActualSendWindowSize() < 4)
      {
        int outgoingPacketNumber = this.GetNextOutgoingPacketNumber();
        List<byte> thresholdBytesFromBuffer = this.ExtractUptoThresholdBytesFromBuffer();
        List<byte> ackDataPacket = this.CreateAckDataPacket(outgoingPacketNumber, packetNumber, thresholdBytesFromBuffer);
        List<byte> dataPacket = this.CreateDataPacket(outgoingPacketNumber, thresholdBytesFromBuffer);
        this.SendPacket(ackDataPacket);
        this.AddPacketToSendWindow(dataPacket);
        this.sendTimer.Restart();
      }
      else
        this.SendPacket(this.CreateAckPacket(packetNumber));
      if (!this.IsInReceiveWindow(packetNumber))
        return;
      this.receiveWindow[this.GetWindowIndex(packetNumber)] = packet;
      int index = (this.receiveWindowStart + 1) % 4;
      while (this.receiveWindow[index] != null)
      {
        List<byte> packet1 = this.receiveWindow[index];
        this.receiveWindow[index] = (List<byte>) null;
        index = (index + 1) % 4;
        this.receiveWindowStart = (this.receiveWindowStart + 1) % 16;
        this.receiveWindowEnd = (this.receiveWindowEnd + 1) % 16;
        switch (this.GetPacketType(packet1))
        {
          case 4:
            this.ProcessDataPacket(packet1, false);
            continue;
          case 5:
            this.ProcessDataPacket(packet1, true);
            continue;
          default:
            continue;
        }
      }
    }

    private void DumpSendWindow()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SEND WINDOW " + (object) this.sendWindowStart + " - " + (object) this.sendWindowEnd + "\r\n");
      for (int index = 0; index < 4; ++index)
      {
        PacketDataCallHandler.SendWindowSlot sendWindowSlot = this.sendWindow[index];
        stringBuilder.Append("SLOT" + index.ToString() + "- " + (sendWindowSlot.ResendTimer.Enabled ? "T" : "F") + ": ");
        if (sendWindowSlot.PacketData != null)
        {
          foreach (byte num in sendWindowSlot.PacketData)
            stringBuilder.Append(num.ToString("X").PadLeft(2, '0') + ".");
        }
        stringBuilder.Append("\r\n");
      }
      stringBuilder.Append("\r\n");
    }

    private void DumpReceiveWindow()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("RECEIVE WINDOW " + (object) this.receiveWindowStart + " - " + (object) this.receiveWindowEnd + "\r\n");
      for (int index = 0; index < 4; ++index)
      {
        List<byte> byteList = this.receiveWindow[index];
        stringBuilder.Append("SLOT" + index.ToString() + ": ");
        if (byteList != null)
        {
          foreach (byte num in byteList.ToArray())
            stringBuilder.Append(num.ToString("X").PadLeft(2, '0') + ".");
        }
        stringBuilder.Append("\r\n");
      }
      stringBuilder.Append("\r\n");
    }

    private void ProcessAck(int ackNumber)
    {
      if (!this.IsInSendWindow(ackNumber))
        return;
      int windowIndex = this.GetWindowIndex(ackNumber);
      this.sendWindow[windowIndex].PacketData = (byte[]) null;
      this.sendWindow[windowIndex].ResendTimer.Stop();
      while (this.sendWindow[(this.sendWindowStart + 1) % 4].PacketData == null && this.GetActualSendWindowSize() > 0)
        this.sendWindowStart = (this.sendWindowStart + 1) % 16;
      if (this.handshaking)
        this.handshaking = false;
      this.SendMaxSizeDataPackets();
    }

    private void ProcessInitialPacket(List<byte> packet)
    {
      this.handshaking = true;
      bool flag1 = false;
      bool flag2 = ((int) packet[2] & 2) == 2;
      if (flag2)
      {
        byte[] numArray = (byte[]) null;
        if (flag2 && packet.Count == 22)
        {
          numArray = new byte[16];
          packet.CopyTo(3, numArray, 0, 16);
        }
        byte[] decryptedBytes = (byte[]) null;
        this.remoteImei = string.Empty;
        for (bool flag3 = this.encryptionUser.GotoFirstRecord(); flag3; flag3 = this.encryptionUser.GotoNextRecord())
        {
          if (this.encryptionUser.GetCurrentRecordModemInfoType() == "I" && this.encryptionUser.DecryptWithCurrentRecord(numArray, ref decryptedBytes) == Erc.Success && this.CompareBytes(decryptedBytes, 0, this.known, 0, this.known.Length))
          {
            if (this.remoteImei != string.Empty)
            {
              this.remoteImei = string.Empty;
              break;
            }
            this.remoteImei = this.encryptionUser.GetCurrentRecordModemInfo();
          }
        }
        if (this.remoteImei != string.Empty)
          flag1 = true;
      }
      else if (this.encryptionUser == null)
        flag1 = true;
      if (flag1)
      {
        int packetNumber = this.GetPacketNumber(packet);
        this.ClearSendWindow();
        this.ClearReceiveWindow();
        this.receiveWindowStart = packetNumber;
        this.receiveWindowEnd = (this.receiveWindowStart + 4) % 16;
        List<byte> ackInitialPacket = this.CreateAckInitialPacket(this.GetNextOutgoingPacketNumber(), packetNumber);
        if (ackInitialPacket != null)
        {
          this.AddPacketToSendWindow(ackInitialPacket);
          this.SendPacket(ackInitialPacket);
        }
        else
          this.disconnect();
      }
      else
        this.disconnect();
    }

    private void ProcessDataPacket(List<byte> packet, bool includesAck)
    {
      bool flag = this.encryptionUser != null;
      int index = 2 + (includesAck ? 1 : 0) + (flag ? 1 : 0);
      int count = packet.Count - 3 - index;
      byte[] numArray = packet.GetRange(index, count).ToArray();
      if (flag)
      {
        byte[] decryptedBytes = (byte[]) null;
        if (this.encryptionUser.Decrypt(this.remoteImei, "I", numArray, ref decryptedBytes) == Erc.Success)
        {
          int num = (int) packet[includesAck ? 3 : 2];
          if (num < 16)
          {
            int length = decryptedBytes.Length - num;
            numArray = new byte[length];
            Array.Copy((Array) decryptedBytes, (Array) numArray, length);
          }
          else
            numArray = (byte[]) null;
        }
        else
          numArray = (byte[]) null;
      }
      if (this.DataReceived == null || numArray == null)
        return;
      this.DataReceived((object) this, new PacketDataCallHandler.DataEventArgs(numArray));
    }

    public delegate void DataEvent(object sender, PacketDataCallHandler.DataEventArgs e);

    public delegate void SendDelegate(byte[] data);

    public delegate void DisconnectDelegate();

    private class SendWindowSlot
    {
      public TimerWrapper ResendTimer;
      public byte[] PacketData;
    }

    public class DataEventArgs : EventArgs
    {
      private byte[] data;

      public DataEventArgs(byte[] data) => this.data = data;

      public byte[] Data => this.data;
    }
  }
}
