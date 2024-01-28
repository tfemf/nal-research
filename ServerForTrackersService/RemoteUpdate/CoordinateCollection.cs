// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.CoordinateCollection
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class CoordinateCollection : List<Coordinate>, IEquatable<IEnumerable<Coordinate>>
  {
    public CoordinateCollection(IEnumerable<Coordinate> collection) => this.AddRange(collection);

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is IEnumerable<Coordinate> other && this.Equals(other);
    }

    public bool Equals(IEnumerable<Coordinate> other) => this.SequenceEqual<Coordinate>(other);
  }
}
