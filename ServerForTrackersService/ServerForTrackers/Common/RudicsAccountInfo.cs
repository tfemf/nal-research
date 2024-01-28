// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.RudicsAccountInfo
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class RudicsAccountInfo : CommLinkInfo
  {
    private string displayName;
    private bool enablePipe;
    private string pipeHost;
    private int pipePort;
    private int serverPort;
    private bool serverListen;
    private CallProtocol clientHandlerCallProtocol = CallProtocol.Packets;
    private string clientHost;
    private int clientPortsBegin;
    private int clientPortsEnd;
    private CallProtocol clientCallProtocol = CallProtocol.Packets;

    public string DisplayName
    {
      get => this.displayName;
      set => this.Name = this.displayName = value;
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

    public int ServerPort
    {
      get => this.serverPort;
      set => this.serverPort = value;
    }

    public bool ServerListen
    {
      get => this.serverListen;
      set => this.serverListen = value;
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
  }
}
