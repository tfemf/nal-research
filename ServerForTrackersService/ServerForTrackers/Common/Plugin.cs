// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.Plugin
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class Plugin
  {
    private string file;
    private bool includeSbd;
    private bool includeSms;
    private bool includeCalls;
    private bool includeTcp;
    private BindingList<DataTypeSettings> dataTypeSettingsList;

    public Plugin() => this.dataTypeSettingsList = new BindingList<DataTypeSettings>();

    public string File
    {
      get => this.file;
      set => this.file = value;
    }

    public bool IncludeSbd
    {
      get => this.includeSbd;
      set => this.includeSbd = value;
    }

    public bool IncludeSms
    {
      get => this.includeSms;
      set => this.includeSms = value;
    }

    public bool IncludeCalls
    {
      get => this.includeCalls;
      set => this.includeCalls = value;
    }

    public bool IncludeTcp
    {
      get => this.includeTcp;
      set => this.includeTcp = value;
    }

    public BindingList<DataTypeSettings> DataTypeSettingsList => this.dataTypeSettingsList;

    public override string ToString() => this.file;
  }
}
