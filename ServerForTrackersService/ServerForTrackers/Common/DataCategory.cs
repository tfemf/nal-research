// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.DataCategory
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public enum DataCategory
  {
    [Description("All Known")] AllKnown,
    [Description("GPS Reports")] GpsReports,
    [Description("Update Responses")] UpdateResponses,
  }
}
