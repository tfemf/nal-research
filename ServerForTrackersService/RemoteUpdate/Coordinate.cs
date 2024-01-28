// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.Coordinate
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class Coordinate : IEquatable<Coordinate>
  {
    public Coordinate(double latitude, double longitude)
    {
      this.Latitude = latitude;
      this.Longitude = longitude;
    }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public bool Equals(Coordinate other)
    {
      return this.Latitude == other.Latitude && this.Longitude == other.Longitude;
    }
  }
}
