// Decompiled with JetBrains decompiler
// Type: Nal.Network.Pop3ResponseReceivedEventArgs
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

#nullable disable
namespace Nal.Network
{
  public class Pop3ResponseReceivedEventArgs
  {
    private Pop3Command command;
    private Pop3ResponseType responseType;
    private string statusIndicator;

    public Pop3ResponseReceivedEventArgs(
      Pop3Command command,
      Pop3ResponseType responseType,
      string statusIndicator)
    {
      this.command = command;
      this.responseType = responseType;
      this.statusIndicator = statusIndicator;
    }

    public Pop3Command Command => this.command;

    public Pop3ResponseType ResponseType => this.responseType;

    public string StatusIndicator => this.statusIndicator;
  }
}
