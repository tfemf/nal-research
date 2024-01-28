// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.LoggingDirectory
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.ComponentModel;
using System.Linq.Expressions;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class LoggingDirectory : GuiItem
  {
    private string directory;
    private string filePrefix;
    private bool includeSbd;
    private bool includeSms;
    private bool includeCalls;
    private bool includeTcp;
    private BindingList<DataTypeSettings> dataTypeSettingsList;

    public LoggingDirectory()
    {
      this.directory = string.Empty;
      this.filePrefix = string.Empty;
      this.dataTypeSettingsList = new BindingList<DataTypeSettings>();
    }

    public string Directory
    {
      get => this.directory;
      set
      {
        this.SetProperty<string>(ref this.directory, value, (Expression<Func<string>>) (() => this.Directory));
      }
    }

    public string FilePrefix
    {
      get => this.filePrefix;
      set
      {
        this.SetProperty<string>(ref this.filePrefix, value, (Expression<Func<string>>) (() => this.FilePrefix));
      }
    }

    public bool IncludeSbd
    {
      get => this.includeSbd;
      set
      {
        this.SetProperty<bool>(ref this.includeSbd, value, (Expression<Func<bool>>) (() => this.IncludeSbd));
      }
    }

    public bool IncludeSms
    {
      get => this.includeSms;
      set
      {
        this.SetProperty<bool>(ref this.includeSms, value, (Expression<Func<bool>>) (() => this.IncludeSms));
      }
    }

    public bool IncludeCalls
    {
      get => this.includeCalls;
      set
      {
        this.SetProperty<bool>(ref this.includeCalls, value, (Expression<Func<bool>>) (() => this.IncludeCalls));
      }
    }

    public bool IncludeTcp
    {
      get => this.includeTcp;
      set => this.includeTcp = value;
    }

    public BindingList<DataTypeSettings> DataTypeSettingsList => this.dataTypeSettingsList;

    public override string ToString() => this.Directory;
  }
}
