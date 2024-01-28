// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.MOLocationIE
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class MOLocationIE : InfoElement
  {
    public override byte Id => 3;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public uint CepRad { get; set; }

    public static bool Parse(IList<byte> data, out MOLocationIE ie)
    {
      if (data.Count == 14 && data[0] == (byte) 3 && data[1] == (byte) 0 && data[2] == (byte) 11)
      {
        ie = new MOLocationIE();
        int num1 = (int) data[3];
        int num2 = (int) data[4];
        double num3 = (double) ((int) data[5] * 256 + (int) data[6]) / 1000.0;
        ie.Latitude = (double) num2 + num3 / 60.0;
        if ((num1 & 2) != 0)
          ie.Latitude *= -1.0;
        int num4 = (int) data[7];
        double num5 = (double) ((int) data[8] * 256 + (int) data[9]) / 1000.0;
        ie.Longitude = (double) num4 + num5 / 60.0;
        if ((num1 & 1) != 0)
          ie.Longitude *= -1.0;
        ie.CepRad = (uint) ((int) data[10] * 16777216 + (int) data[11] * 65536 + (int) data[12] * 256) + (uint) data[13];
        return true;
      }
      ie = (MOLocationIE) null;
      return false;
    }

    public override List<byte> GetBytes() => throw new NotImplementedException();
  }
}
