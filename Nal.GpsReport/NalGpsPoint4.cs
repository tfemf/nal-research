// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsPoint4
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class NalGpsPoint4
  {
    public DateTime Time { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Altitude { get; set; }

    public double GroundVelocity { get; set; }

    public double Course { get; set; }

    public double VerticalVelocity { get; set; }

    public PositionFix Fix { get; set; }

    public byte Satellites { get; set; }

    public double Hdop { get; set; }

    public double Vdop { get; set; }

    public bool Motion { get; set; }

    public bool Emergency { get; set; }

    public bool EmergencyAcknowledged { get; set; }

    public XElement ToXElement()
    {
      return new XElement((XName) "point", new object[14]
      {
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".f")),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "alt", (object) this.Altitude.ToString("0.#######", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gndVel", (object) this.GroundVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "course", (object) this.Course.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "verVel", (object) this.VerticalVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "fix", (object) this.Fix.ToXElementText()),
        (object) new XElement((XName) "sats", (object) this.Satellites),
        (object) new XElement((XName) "hdop", (object) this.Hdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "vdop", (object) this.Vdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "motion", this.Motion ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emer", this.Emergency ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emerAcked", this.EmergencyAcknowledged ? (object) "1" : (object) "0")
      });
    }

    public static bool Parse(byte[] data, out NalGpsPoint4 point, out bool idIncluded)
    {
      point = (NalGpsPoint4) null;
      idIncluded = false;
      if (data.Length != 28)
        return false;
      point = new NalGpsPoint4();
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(data));
      ulong extractFrom1 = binaryReader.ReadUInt64();
      ulong extractFrom2 = binaryReader.ReadUInt64();
      ulong extractFrom3 = binaryReader.ReadUInt64();
      ushort num1 = binaryReader.ReadUInt16();
      byte num2 = binaryReader.ReadByte();
      byte num3 = binaryReader.ReadByte();
      bool flag1 = NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000000000000UL) == 1UL;
      int valueAfter1 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000000000UL);
      int valueAfter2 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100000000000000UL);
      int valueAfter3 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100000000000UL);
      int valueAfter4 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 1000000000UL);
      int valueAfter5 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000UL);
      int valueAfter6 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000UL);
      int num4 = (int) extractFrom1;
      bool flag2 = NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000000000000000UL) != 1UL;
      int valueAfter7 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000000000000000UL);
      int valueAfter8 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 1000000000000000UL);
      int valueAfter9 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000000000UL);
      int valueAfter10 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 1000000000UL);
      int valueAfter11 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000UL);
      int valueAfter12 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000UL);
      int valueAfter13 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000UL);
      int num5 = (int) extractFrom2;
      bool flag3 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000000000000000UL) != 1UL;
      ulong valueAfter14 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100000000000000UL);
      byte valueAfter15 = (byte) NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000000000UL);
      ulong valueAfter16 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 1000000UL);
      byte valueAfter17 = (byte) NalGpsReport.ExtractValueAfter(ref extractFrom3, 100000UL);
      ulong num6 = extractFrom3;
      byte num7 = (byte) ((ulong) num1 / 10000UL);
      int num8 = (int) ((ulong) num1 % 10000UL);
      point.Satellites = (byte) ((uint) num2 / 10U);
      int num9 = (int) num2 % 10;
      bool flag4 = ((int) num3 & 1) == 1;
      bool flag5 = ((int) num3 & 2) == 2;
      bool flag6 = ((int) num3 & 4) == 4;
      idIncluded = ((int) num3 & 8) == 8;
      point.Emergency = ((int) num3 & 16) == 16;
      point.Motion = ((int) num3 & 32) == 32;
      point.EmergencyAcknowledged = ((int) num3 & 64) == 64;
      bool flag7;
      int num10;
      if (valueAfter1 < 200)
      {
        flag7 = false;
        num10 = valueAfter1;
      }
      else
      {
        flag7 = true;
        num10 = valueAfter1 - 200;
      }
      try
      {
        point.Time = new DateTime(valueAfter9, valueAfter7, valueAfter8, valueAfter10, valueAfter11, valueAfter12, valueAfter13 * 100, DateTimeKind.Utc);
        point.Latitude = ((double) valueAfter4 + ((double) valueAfter5 + (double) valueAfter6 / 1000.0) / 60.0) * (flag1 ? -1.0 : 1.0);
        point.Longitude = ((double) num10 + ((double) valueAfter2 + (double) valueAfter3 / 1000.0) / 60.0) * (flag7 ? -1.0 : 1.0);
        point.Altitude = (double) valueAfter16 / Math.Pow(10.0, (double) (7 - (int) valueAfter15)) * (flag3 ? -1.0 : 1.0);
        point.GroundVelocity = (double) num6 / Math.Pow(10.0, (double) (5 - (int) valueAfter17));
        point.Course = (double) valueAfter14 / 100.0;
        point.VerticalVelocity = (double) (num9 * 10000 + num5) / Math.Pow(10.0, (double) (5 - (int) num7)) * (flag2 ? -1.0 : 1.0);
        point.Hdop = (double) num4 / 100.0;
        point.Vdop = (double) num8 / 100.0;
        point.Fix = flag6 ? PositionFix.Valid3D : (flag4 ? PositionFix.TwoD : (flag5 ? PositionFix.DeadReckoning : PositionFix.Other));
        if (point.Latitude >= -90.0 && point.Latitude <= 90.0 && point.Longitude >= -180.0)
        {
          if (point.Longitude <= 180.0)
            goto label_10;
        }
        point = (NalGpsPoint4) null;
        return false;
      }
      catch (ArgumentException ex)
      {
        point = (NalGpsPoint4) null;
        return false;
      }
label_10:
      return true;
    }
  }
}
