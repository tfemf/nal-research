// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PositionFixExtensions
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

#nullable disable
namespace Nal.GpsReport
{
  public static class PositionFixExtensions
  {
    public static string ToXElementText(this PositionFix fix)
    {
      switch (fix)
      {
        case PositionFix.Other:
          return "Other";
        case PositionFix.No:
          return "No Fix";
        case PositionFix.TimeOnly:
          return "Time Only";
        case PositionFix.DeadReckoning:
          return "DR";
        case PositionFix.GpsAndDeadReckoning:
          return "GPS+DR";
        case PositionFix.TwoD:
          return "2D";
        case PositionFix.ThreeD:
          return "3D";
        case PositionFix.Valid3D:
          return "Valid 3D";
        default:
          return "Unknown";
      }
    }
  }
}
