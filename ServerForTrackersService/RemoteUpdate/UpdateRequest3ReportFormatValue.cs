// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest3ReportFormatValue
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.RemoteUpdate
{
  public enum UpdateRequest3ReportFormatValue
  {
    [Description("NAL Version 3")] NalVersion3 = 1,
    [Description("NAL Version 4")] NalVersion4 = 2,
    [Description("NAL Version 5")] NalVersion5 = 3,
    [Description("Pecos P3")] PecosP3 = 4,
    [Description("Pecos P4")] PecosP4 = 5,
    [Description("NAL Version 6")] NalVersion6 = 6,
    [Description("NAL 10 Byte Version 0")] NalTenByteVersion0 = 7,
  }
}
