// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.CommLinkInfo
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class CommLinkInfo : INotifyPropertyChanged
  {
    private string id;
    private string name;

    public event PropertyChangedEventHandler PropertyChanged;

    public string ID
    {
      get => this.id;
      set => this.id = value;
    }

    public string Name
    {
      get => this.name;
      set
      {
        if (!(this.name != value))
          return;
        this.name = value;
        if (this.PropertyChanged == null)
          return;
        this.PropertyChanged((object) this, new PropertyChangedEventArgs(nameof (Name)));
      }
    }

    public override string ToString() => this.name;
  }
}
