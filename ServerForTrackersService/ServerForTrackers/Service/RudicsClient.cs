// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.RudicsClient
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class RudicsClient : RudicsConnection
  {
    private const int ComPortOption = 44;
    private const int CarriageReturn = 13;
    private const int Se = 240;
    private const int Sb = 250;
    private const int Will = 251;
    private const int Wont = 252;
    private const int Do = 253;
    private const int Dont = 254;
    private const int Iac = 255;
    private string host;
    private int portsBegin;
    private int portsEnd;
    private string peerNumber;
    private int currentPort;
    private SocketWrapper socket;
    private DataSerializer dataSerializer;
    private RudicsClient.TelnetStreamState telnetReceiveState;
    private List<byte> subOption;

    public RudicsClient()
    {
      this.subOption = new List<byte>();
      this.socket = new SocketWrapper();
      this.socket.ConnectFailed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectFailed);
      this.socket.ConnectionMade += new EventHandler(this.OnSocketConnectionMade);
      this.socket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.OnSocketConnectionClosed);
      this.socket.DataReceived += new EventHandler<Nal.Network.DataReceivedEventArgs>(this.OnSocketDataReceived);
      this.dataSerializer = new DataSerializer(new DataSerializer.SendDataDelegate(((RudicsConnection) this).Send));
      this.dataSerializer.TransactionCompleted += new DataSerializer.TransactionCompletedEventHandler(this.OnDataSerializerTransactionCompleted);
    }

    public string Host
    {
      set => this.host = value;
    }

    public int PortsBegin
    {
      set => this.portsBegin = value;
    }

    public int PortsEnd
    {
      set => this.portsEnd = value;
    }

    public void Connect(string peerNumber)
    {
      this.peerNumber = peerNumber;
      this.currentPort = this.portsBegin;
      this.Name = "RUDICS Client (Connecting...)";
      this.Status = "Connecting";
      this.Activity = "Trying " + this.host + ":" + (object) this.currentPort + " ... ";
      try
      {
        this.socket.Connect(this.host, this.currentPort);
      }
      catch (Exception ex)
      {
        this.SelfDestruct("Could not connect to the gateway - Exception: " + ex.Message);
      }
    }

    public override void Disconnect()
    {
      if (this.socket.Connected)
        this.socket.Disconnect();
      if (this.InConnection)
      {
        this.HandleDisconnect(string.Empty);
      }
      else
      {
        this.dataSerializer.CancelTransactions();
        this.SelfDestruct(string.Empty);
      }
    }

    protected override void Send(byte[] data)
    {
      this.socket.Send(this.FormatDataForTelnetStream(data));
    }

    private void OnSocketConnectFailed(object sender, EventArgs e)
    {
      this.Activity = "Failed\r\n";
      if (this.currentPort < this.portsEnd)
      {
        ++this.currentPort;
        this.Activity = "Trying " + this.host + ":" + (object) this.currentPort + " ... ";
        try
        {
          this.socket.Connect(this.host, this.currentPort);
        }
        catch (Exception ex)
        {
          this.SelfDestruct("Could not connect to the gateway - Exception: " + ex.Message);
        }
      }
      else
        this.SelfDestruct("Could not connect to the gateway - Out of ports.");
    }

    private void OnSocketConnectionMade(object sender, EventArgs e)
    {
      this.Activity = "Success\r\n\r\n";
      this.Name = "RUDICS Client (" + (object) this.socket.RemoteEndPoint + ")";
      this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
      this.subOption.Clear();
      this.SendWillComPortOption();
    }

    private void OnSocketConnectionClosed(object sender, EventArgs e)
    {
      if (this.InConnection)
        this.HandleDisconnect("Connection dropped.");
      else
        this.SelfDestruct("Connection dropped.");
    }

    private void OnSocketDataReceived(object sender, Nal.Network.DataReceivedEventArgs e)
    {
      this.ParseTelnetStreamForData(e.Data);
    }

    private void OnDataSerializerTransactionCompleted(
      object sender,
      DataSerializer.TransactionCompletedEventArgs e)
    {
      this.Status = "Idle";
      switch (e.TransactionID)
      {
        case "Setting Call Type":
          if (e.Result == 0)
          {
            this.SendAts57();
            break;
          }
          this.socket.Disconnect();
          this.SelfDestruct("Could not set call type.");
          break;
        case "Setting Call Speed":
          if (e.Result == 0)
          {
            this.SendAtdi();
            break;
          }
          this.socket.Disconnect();
          this.SelfDestruct("Could not set call speed.");
          break;
        case "Dialing":
          if (e.Result == 0)
            break;
          this.socket.Disconnect();
          string str;
          switch (e.Result)
          {
            case -1:
              str = "Timed out";
              break;
            case 1:
              str = "NO ANSWER";
              break;
            case 2:
              str = "BUSY";
              break;
            case 3:
              str = "NO CARRIER";
              break;
            case 4:
              str = "ABORTED";
              break;
            case 5:
              str = "ERROR";
              break;
            default:
              str = string.Empty;
              break;
          }
          this.SelfDestruct("Dialing failed - " + str);
          break;
      }
    }

    private void SendWillComPortOption()
    {
      this.socket.Send(new byte[3]
      {
        byte.MaxValue,
        (byte) 251,
        (byte) 44
      });
    }

    private void SendSetModemStateMask()
    {
      this.socket.Send(new byte[7]
      {
        byte.MaxValue,
        (byte) 250,
        (byte) 44,
        (byte) 11,
        (byte) 128,
        byte.MaxValue,
        (byte) 240
      });
    }

    private void SendAts29()
    {
      this.Status = "Setting Call Type...";
      this.dataSerializer.RequestTransaction("ATS29=8\r", "OK\r\n|ERROR\r\n", 3000, "Setting Call Type");
    }

    private void SendAts57()
    {
      this.Status = "Setting Call Speed...";
      this.dataSerializer.RequestTransaction("ATS57=9600\r", "OK\r\n|ERROR\r\n", 3000, "Setting Call Speed");
    }

    private void SendAtdi()
    {
      this.Status = "Dialing...";
      this.dataSerializer.RequestTransaction("ATDI" + this.peerNumber + "\r", "CONNECT 9600 /V110\r\n|NO ANSWER\r\n|BUSY\r\n|NO CARRIER\r\n|ABORTED\r\n|ERROR\r\n", 120000, "Dialing");
    }

    private void ParseTelnetStreamForData(byte[] data)
    {
      List<byte> byteList = new List<byte>();
      foreach (byte num in data)
      {
        switch (this.telnetReceiveState)
        {
          case RudicsClient.TelnetStreamState.Data:
            switch (num)
            {
              case 13:
                byteList.Add(num);
                this.telnetReceiveState = RudicsClient.TelnetStreamState.CarriageReturn;
                continue;
              case byte.MaxValue:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Iac;
                continue;
              default:
                byteList.Add(num);
                continue;
            }
          case RudicsClient.TelnetStreamState.CarriageReturn:
            if (num != (byte) 0)
              byteList.Add(num);
            this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
            break;
          case RudicsClient.TelnetStreamState.Iac:
            switch (num)
            {
              case 250:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.SubOption;
                continue;
              case 251:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Will;
                continue;
              case 252:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Wont;
                continue;
              case 253:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Do;
                continue;
              case 254:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Dont;
                continue;
              case byte.MaxValue:
                byteList.Add(num);
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
                continue;
              default:
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
                continue;
            }
          case RudicsClient.TelnetStreamState.Will:
          case RudicsClient.TelnetStreamState.Wont:
          case RudicsClient.TelnetStreamState.Dont:
            this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
            break;
          case RudicsClient.TelnetStreamState.Do:
            if (num == (byte) 44)
              this.SendSetModemStateMask();
            this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
            break;
          case RudicsClient.TelnetStreamState.SubOption:
            if (num == byte.MaxValue)
            {
              this.telnetReceiveState = RudicsClient.TelnetStreamState.SubOptionIac;
              break;
            }
            this.subOption.Add(num);
            break;
          case RudicsClient.TelnetStreamState.SubOptionIac:
            switch (num)
            {
              case 240:
                if (this.subOption.Count == 3 && this.subOption[0] == (byte) 44)
                {
                  if (this.subOption[1] == (byte) 111)
                    this.SendAts29();
                  else if (this.subOption[1] == (byte) 107)
                  {
                    if (((int) this.subOption[2] & 128) == 128)
                    {
                      if (!this.InConnection)
                      {
                        byte[] array = byteList.ToArray();
                        byteList.Clear();
                        this.HandleDataReceived(array);
                        this.dataSerializer.ProcessReceivedData(Encoding.GetEncoding(1252).GetString(array));
                        this.dataSerializer.CancelTransactions();
                        this.HandleConnect(true);
                      }
                    }
                    else if (this.InConnection)
                    {
                      this.HandleDataReceived(byteList.ToArray());
                      byteList.Clear();
                      this.socket.Disconnect();
                      this.HandleDisconnect(string.Empty);
                    }
                  }
                }
                this.subOption.Clear();
                this.telnetReceiveState = RudicsClient.TelnetStreamState.Data;
                continue;
              case byte.MaxValue:
                this.subOption.Add(num);
                break;
            }
            this.telnetReceiveState = RudicsClient.TelnetStreamState.SubOption;
            break;
        }
      }
      byte[] array1 = byteList.ToArray();
      this.HandleDataReceived(array1);
      if (this.InConnection)
        return;
      this.dataSerializer.ProcessReceivedData(Encoding.GetEncoding(1252).GetString(array1));
    }

    private byte[] FormatDataForTelnetStream(byte[] data)
    {
      List<byte> byteList = new List<byte>();
      foreach (byte num in data)
      {
        byteList.Add(num);
        switch (num)
        {
          case 13:
            byteList.Add((byte) 0);
            break;
          case byte.MaxValue:
            byteList.Add(byte.MaxValue);
            break;
        }
      }
      return byteList.ToArray();
    }

    private enum TelnetStreamState
    {
      Data,
      CarriageReturn,
      Iac,
      Will,
      Wont,
      Do,
      Dont,
      SubOption,
      SubOptionIac,
    }
  }
}
