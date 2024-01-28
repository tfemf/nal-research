// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsPoint3
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Globalization;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class NalGpsPoint3
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

    public bool Emergency { get; set; }

    public XElement ToXElement()
    {
      return new XElement((XName) "point", new object[10]
      {
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".ff")),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "alt", (object) this.Altitude.ToString("0.#######", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gndVel", (object) this.GroundVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "course", (object) this.Course.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "verVel", (object) this.VerticalVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "fix", (object) this.Fix.ToXElementText()),
        (object) new XElement((XName) "sats", (object) this.Satellites),
        (object) new XElement((XName) "emer", this.Emergency ? (object) "1" : (object) "0")
      });
    }

    public static bool Parse(
      byte[] data,
      bool autoSetMilCent,
      int assumedMilCent,
      out NalGpsPoint3 point,
      out bool idIncluded)
    {
      point = (NalGpsPoint3) null;
      idIncluded = false;
      if (data.Length != 28)
        return false;
      point = new NalGpsPoint3();
      byte num1 = data[0];
      byte minute = data[1];
      byte num2 = data[2];
      byte num3 = data[3];
      byte num4 = data[4];
      byte day = data[5];
      byte num5 = data[6];
      byte num6 = data[7];
      byte num7 = data[8];
      byte num8 = data[9];
      byte num9 = data[10];
      byte num10 = data[11];
      byte num11 = data[12];
      byte num12 = data[13];
      byte num13 = data[14];
      byte num14 = data[15];
      byte num15 = data[16];
      byte num16 = data[17];
      byte num17 = data[18];
      byte num18 = data[19];
      int num19 = (int) data[20];
      byte num20 = data[21];
      byte num21 = data[22];
      byte num22 = data[23];
      byte num23 = data[24];
      byte num24 = data[25];
      byte num25 = data[26];
      byte num26 = data[27];
      byte hour = (byte) ((uint) num1 / 10U);
      byte num27 = (byte) ((uint) num1 % 10U);
      byte num28 = (byte) ((uint) num2 / 64U);
      byte second = (byte) ((uint) num2 % 64U);
      point.Emergency = ((uint) num3 & 128U) > 0U;
      byte num29 = (byte) ((uint) num3 & (uint) sbyte.MaxValue);
      byte month = (byte) ((uint) num4 / 10U);
      byte num30 = (byte) ((uint) num4 % 10U);
      byte num31 = (byte) ((uint) num11 / 10U);
      byte num32 = (byte) ((uint) num11 % 10U);
      point.Satellites = (byte) ((uint) num14 / 10U);
      byte num33 = (byte) ((uint) num14 % 10U);
      byte num34 = (byte) ((uint) num15 / 10U);
      byte num35 = (byte) ((uint) num15 % 10U);
      byte num36 = (byte) (num19 / 10);
      byte num37 = (byte) (num19 % 10);
      bool flag1 = ((uint) num26 & 1U) > 0U;
      bool flag2 = ((uint) num26 & 2U) > 0U;
      idIncluded = ((uint) num26 & 4U) > 0U;
      bool flag3 = ((uint) num26 & 8U) > 0U;
      bool flag4 = ((uint) num26 & 16U) > 0U;
      bool flag5 = ((uint) num26 & 32U) > 0U;
      bool flag6 = ((uint) num26 & 64U) > 0U;
      bool flag7 = ((uint) num26 & 128U) > 0U;
      int num38 = (int) num27 * 10 + (int) num30;
      int year;
      if (autoSetMilCent)
      {
        DateTime dateTime = DateTime.UtcNow;
        dateTime = dateTime.Date;
        int num39 = dateTime.Year / 100;
        dateTime = DateTime.UtcNow;
        dateTime = dateTime.Date;
        int num40 = dateTime.Year % 100;
        if (num38 == 99 && num40 == 0)
          --num39;
        else if (num38 == 0 && num40 == 99)
          ++num39;
        else if (num38 != num40)
          num39 = assumedMilCent;
        year = num39 * 100 + num38;
      }
      else
        year = assumedMilCent * 100 + num38;
      try
      {
        point.Time = new DateTime(year, (int) month, (int) day, (int) hour, (int) minute, (int) second, (int) num29 * 10, DateTimeKind.Utc);
        point.Latitude = ((double) num5 + ((double) num6 + (double) ((int) num9 * 1000 + (int) num10 * 10 + (int) num31) / 100000.0) / 60.0) * (flag5 ? -1.0 : 1.0);
        point.Longitude = ((double) num7 + ((double) num8 + (double) ((int) num32 * 10000 + (int) num12 * 100 + (int) num13) / 100000.0) / 60.0) * (flag6 ? -1.0 : 1.0);
        point.Altitude = (double) ((int) num35 * 1000000 + (int) num16 * 10000 + (int) num17 * 100 + (int) num18) / Math.Pow(10.0, (double) (7 - (int) num34)) * (flag4 ? -1.0 : 1.0);
        point.GroundVelocity = (double) ((int) num37 * 10000 + (int) num20 * 100 + (int) num21) / Math.Pow(10.0, (double) (5 - (int) num36));
        point.Course = (double) ((int) num25 * 256 + (int) num24) / 100.0;
        point.VerticalVelocity = (double) ((int) num33 * 10000 + (int) num22 * 100 + (int) num23) / Math.Pow(10.0, (double) (5 - (int) num28)) * (flag3 ? -1.0 : 1.0);
        point.Fix = flag7 ? PositionFix.Valid3D : (flag1 ? PositionFix.TwoD : (flag2 ? PositionFix.DeadReckoning : PositionFix.Other));
        if (point.Latitude >= -90.0 && point.Latitude <= 90.0 && point.Longitude >= -180.0)
        {
          if (point.Longitude <= 180.0)
            goto label_16;
        }
        point = (NalGpsPoint3) null;
        return false;
      }
      catch (ArgumentException ex)
      {
        point = (NalGpsPoint3) null;
        return false;
      }
label_16:
      return true;
    }
  }
}
