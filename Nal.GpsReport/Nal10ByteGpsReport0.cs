// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.Nal10ByteGpsReport0
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Globalization;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class Nal10ByteGpsReport0 : NalGpsReport
  {
    public DateTime Time { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public PositionFix Fix { get; set; }

    public double Pdop { get; set; }

    public bool Motion { get; set; }

    public bool Emergency { get; set; }

    public bool EmergencyAcknowledged { get; set; }

    public override bool IsEmergency() => this.Emergency;

    public override XElement ToXElement()
    {
      return new XElement((XName) "nal10ByteGpsReport0", new object[8]
      {
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText()),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "fix", (object) this.Fix.ToXElementText()),
        (object) new XElement((XName) "pdop", (object) this.Pdop.ToString("0.##")),
        (object) new XElement((XName) "motion", this.Motion ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emer", this.Emergency ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emerAcked", this.EmergencyAcknowledged ? (object) "1" : (object) "0")
      });
    }

    public static bool Parse(byte[] data, out Nal10ByteGpsReport0 report)
    {
      report = (Nal10ByteGpsReport0) null;
      if (data.Length != 10 && data.Length != 16)
        return false;
      uint num1;
      NalGpsReport.ExtractBits(data, 4, 25, out num1);
      uint num2;
      NalGpsReport.ExtractBits(data, 29, 26, out num2);
      uint num3;
      NalGpsReport.ExtractBits(data, 55, 17, out num3);
      uint index;
      NalGpsReport.ExtractBits(data, 72, 4, out index);
      uint num4;
      NalGpsReport.ExtractBits(data, 76, 4, out num4);
      if (num1 <= 18000000U && num2 <= 36000000U && num3 <= 86399U)
      {
        if (index <= 7U)
        {
          try
          {
            report = new Nal10ByteGpsReport0();
            Nal10ByteGpsReport0 nal10ByteGpsReport0 = report;
            DateTime dateTime1 = DateTime.UtcNow;
            dateTime1 = dateTime1.Date;
            DateTime dateTime2 = dateTime1.AddSeconds((double) num3);
            nal10ByteGpsReport0.Time = dateTime2;
            report.Latitude = (double) ((int) num1 - 9000000) / 100000.0;
            report.Longitude = (double) ((int) num2 - 18000000) / 100000.0;
            report.Pdop = new double[8]
            {
              1.0,
              2.0,
              5.0,
              10.0,
              20.0,
              40.0,
              70.0,
              99.99
            }[(int) index];
            report.Fix = ((int) num4 & 1) != 0 ? PositionFix.Valid3D : PositionFix.Other;
            report.Emergency = (num4 & 2U) > 0U;
            report.EmergencyAcknowledged = (num4 & 4U) > 0U;
            report.Motion = (num4 & 8U) > 0U;
          }
          catch (ArgumentException ex)
          {
            report = (Nal10ByteGpsReport0) null;
            return false;
          }
          return true;
        }
      }
      return false;
    }
  }
}
