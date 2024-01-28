// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.GnssSatellite
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

#nullable disable
namespace Nal.GpsReport
{
  public class GnssSatellite
  {
    public GnssNetwork Network { get; set; }

    public byte Id { get; set; }

    public byte Signal { get; set; }
  }
}
