// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.DataType
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public enum DataType
  {
    [Description("Other")] Other,
    [Description("NAL GPS Report 3")] NalGpsReport3,
    [Description("NAL GPS Report 4")] NalGpsReport4,
    [Description("NAL GPS Report 5")] NalGpsReport5,
    [Description("NAL GPS Report 6")] NalGpsReport6,
    [Description("NAL GPS Report 7")] NalGpsReport7,
    [Description("NAL 10 Byte GPS Report 0")] Nal10ByteGpsReport0,
    [Description("PECOS P3 GPS Report")] PecosP3GpsReport,
    [Description("PECOS P4 GPS Report")] PecosP4GpsReport,
    [Description("Update Response 0")] UpdateResponse0,
    [Description("Update Response 1")] UpdateResponse1,
    [Description("Update Response 2")] UpdateResponse2,
    [Description("Update Response 3")] UpdateResponse3,
    [Description("Status Report 0")] StatusReport0,
  }
}
