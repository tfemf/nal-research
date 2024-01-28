// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.ServerModemInfo
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.IO.Ports;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class ServerModemInfo : CommLinkInfo
  {
    private string port;
    private bool openPort;
    private int portBitsPerSec = 19200;
    private StopBits portStopBits = StopBits.One;
    private Parity portParity;
    private int portDataBits = 8;
    private bool recvSms;
    private bool manageConn;
    private bool autoAnswer;
    private CallProtocol callProtocol = CallProtocol.Packets;

    public string Port
    {
      get => this.port;
      set => this.Name = this.port = value;
    }

    public bool OpenPort
    {
      get => this.openPort;
      set => this.openPort = value;
    }

    public int PortBitsPerSec
    {
      get => this.portBitsPerSec;
      set => this.portBitsPerSec = value;
    }

    public StopBits PortStopBits
    {
      get => this.portStopBits;
      set => this.portStopBits = value;
    }

    public Parity PortParity
    {
      get => this.portParity;
      set => this.portParity = value;
    }

    public int PortDataBits
    {
      get => this.portDataBits;
      set => this.portDataBits = value;
    }

    public bool RecvSms
    {
      get => this.recvSms;
      set => this.recvSms = value;
    }

    public bool ManageConn
    {
      get => this.manageConn;
      set => this.manageConn = value;
    }

    public bool AutoAnswer
    {
      get => this.autoAnswer;
      set => this.autoAnswer = value;
    }

    public CallProtocol CallProtocol
    {
      get => this.callProtocol;
      set => this.callProtocol = value;
    }
  }
}
