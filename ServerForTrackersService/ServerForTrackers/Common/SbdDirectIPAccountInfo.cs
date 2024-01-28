// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.SbdDirectIPAccountInfo
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class SbdDirectIPAccountInfo : CommLinkInfo
  {
    private string displayName;
    private int serverPort;
    private bool serverListen;
    private string clientHost;
    private int clientPort;
    private bool sendMODirectIPAcknowledgement;

    public string DisplayName
    {
      get => this.displayName;
      set => this.Name = this.displayName = value;
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

    public string ClientHost
    {
      get => this.clientHost;
      set => this.clientHost = value;
    }

    public int ClientPort
    {
      get => this.clientPort;
      set => this.clientPort = value;
    }

    public bool SendMODirectIPAcknowledgement
    {
      get => this.sendMODirectIPAcknowledgement;
      set => this.sendMODirectIPAcknowledgement = value;
    }
  }
}
