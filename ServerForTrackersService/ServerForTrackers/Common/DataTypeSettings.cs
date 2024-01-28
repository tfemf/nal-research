// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.DataTypeSettings
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.RemoteUpdate;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class DataTypeSettings : GuiItem
  {
    private LoggingFormat format;
    private string stylesheet;

    public DataTypeSettings()
    {
      this.format = LoggingFormat.Xml;
      this.stylesheet = string.Empty;
    }

    public string DataTypeDisplay
    {
      get
      {
        return !this.IsCategory ? this.DataType.GetDescription() : "Category: " + this.DataCategory.GetDescription();
      }
    }

    public bool IsCategory { get; set; }

    public DataCategory DataCategory { get; set; }

    public DataType DataType { get; set; }

    public LoggingFormat Format
    {
      get => this.format;
      set
      {
        if (this.format == value)
          return;
        this.format = value;
        this.RaisePropertyChanged<LoggingFormat>((Expression<Func<LoggingFormat>>) (() => this.Format));
        this.RaisePropertyChanged<string>((Expression<Func<string>>) (() => this.FormatDisplay));
        if (this.format != LoggingFormat.Binary)
          return;
        this.Stylesheet = string.Empty;
      }
    }

    public string FormatDisplay => this.format.GetDescription();

    public string Stylesheet
    {
      get => this.stylesheet;
      set
      {
        this.SetProperty<string>(ref this.stylesheet, value, (Expression<Func<string>>) (() => this.Stylesheet));
      }
    }
  }
}
