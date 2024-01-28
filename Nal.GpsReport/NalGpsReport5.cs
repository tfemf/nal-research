// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsReport5
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class NalGpsReport5 : NalGpsReport
  {
    private const int GpsDataSize = 30;
    private const int MaxIdentifierSize = 50;
    private const int EncryptionBlockSize = 16;

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

    public string Id { get; set; }

    public byte InputPins { get; set; }

    public byte OutputPins { get; set; }

    public override bool IsEmergency() => this.Emergency;

    public override XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "nalGpsReport5", new object[16]
      {
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".f")),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "alt", (object) this.Altitude.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gndVel", (object) this.GroundVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "course", (object) this.Course.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "verVel", (object) this.VerticalVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "fix", (object) this.Fix.ToXElementText()),
        (object) new XElement((XName) "sats", (object) this.Satellites),
        (object) new XElement((XName) "hdop", (object) this.Hdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "vdop", (object) this.Vdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "motion", this.Motion ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emer", this.Emergency ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emerAcked", this.EmergencyAcknowledged ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "inputPins", (object) this.InputPins),
        (object) new XElement((XName) "outputPins", (object) this.OutputPins)
      });
      if (this.Id != null)
        xelement.Add((object) new XElement((XName) "id", (object) this.Id));
      return xelement;
    }

    public static bool Parse(byte[] data, out NalGpsReport5 report)
    {
      report = (NalGpsReport5) null;
      if (data.Length < 30 || data.Length > 99 || data[0] != (byte) 5)
        return false;
      report = new NalGpsReport5();
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(data));
      int num1 = (int) binaryReader.ReadByte();
      report.InputPins = binaryReader.ReadByte();
      ulong extractFrom1 = binaryReader.ReadUInt64();
      ulong extractFrom2 = binaryReader.ReadUInt64();
      ulong extractFrom3 = binaryReader.ReadUInt64();
      int num2 = (int) binaryReader.ReadUInt16();
      byte num3 = binaryReader.ReadByte();
      byte num4 = binaryReader.ReadByte();
      bool flag1 = NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000000000000UL) == 1UL;
      int valueAfter1 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000000000UL);
      int valueAfter2 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100000000000000UL);
      int valueAfter3 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100000000000UL);
      int valueAfter4 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 1000000000UL);
      int valueAfter5 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000UL);
      int valueAfter6 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000UL);
      int num5 = (int) extractFrom1;
      bool flag2 = NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000000000000000UL) != 1UL;
      int valueAfter7 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000000000000000UL);
      int valueAfter8 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 1000000000000000UL);
      int valueAfter9 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000000000UL);
      int valueAfter10 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 1000000000UL);
      int valueAfter11 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000UL);
      int valueAfter12 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000UL);
      int valueAfter13 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000UL);
      int num6 = (int) extractFrom2;
      bool flag3 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000000000000000UL) != 1UL;
      ulong valueAfter14 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100000000000000UL);
      byte valueAfter15 = (byte) NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000000000UL);
      ulong valueAfter16 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100000000UL);
      byte valueAfter17 = (byte) NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000UL);
      ulong valueAfter18 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100UL);
      report.OutputPins = (byte) (extractFrom3 % 64UL);
      byte num7 = (byte) ((ulong) (uint) num2 / 10000UL);
      int num8 = (int) ((ulong) (uint) num2 % 10000UL);
      report.Satellites = (byte) ((uint) num3 / 10U);
      int num9 = (int) num3 % 10;
      bool flag4 = ((int) num4 & 1) == 1;
      bool flag5 = ((int) num4 & 2) == 2;
      bool flag6 = ((int) num4 & 4) == 4;
      bool flag7 = ((int) num4 & 8) == 8;
      report.Emergency = ((int) num4 & 16) == 16;
      report.Motion = ((int) num4 & 32) == 32;
      report.EmergencyAcknowledged = ((int) num4 & 64) == 64;
      bool flag8;
      int num10;
      if (valueAfter1 < 200)
      {
        flag8 = false;
        num10 = valueAfter1;
      }
      else
      {
        flag8 = true;
        num10 = valueAfter1 - 200;
      }
      try
      {
        report.Time = new DateTime(valueAfter9, valueAfter7, valueAfter8, valueAfter10, valueAfter11, valueAfter12, valueAfter13 * 100, DateTimeKind.Utc);
        report.Latitude = ((double) valueAfter4 + ((double) valueAfter5 + (double) valueAfter6 / 1000.0) / 60.0) * (flag1 ? -1.0 : 1.0);
        report.Longitude = ((double) num10 + ((double) valueAfter2 + (double) valueAfter3 / 1000.0) / 60.0) * (flag8 ? -1.0 : 1.0);
        report.Altitude = (double) valueAfter16 / Math.Pow(10.0, (double) (5 - (int) valueAfter15)) * (flag3 ? -1.0 : 1.0);
        report.GroundVelocity = (double) valueAfter18 / Math.Pow(10.0, (double) (5 - (int) valueAfter17));
        report.Course = (double) valueAfter14 / 100.0;
        report.VerticalVelocity = (double) (num9 * 10000 + num6) / Math.Pow(10.0, (double) (5 - (int) num7)) * (flag2 ? -1.0 : 1.0);
        report.Hdop = (double) num5 / 100.0;
        report.Vdop = (double) num8 / 100.0;
        report.Fix = flag6 ? PositionFix.Valid3D : (flag4 ? PositionFix.TwoD : (flag5 ? PositionFix.DeadReckoning : PositionFix.Other));
        if (report.Latitude >= -90.0 && report.Latitude <= 90.0 && report.Longitude >= -180.0)
        {
          if (report.Longitude <= 180.0)
            goto label_10;
        }
        report = (NalGpsReport5) null;
        return false;
      }
      catch (ArgumentException ex)
      {
        report = (NalGpsReport5) null;
        return false;
      }
label_10:
      if (flag7)
      {
        report.Id = Encoding.ASCII.GetString(data, 30, data.Length - 30);
        int startIndex = report.Id.IndexOf('\r');
        if (startIndex != -1)
          report.Id = report.Id.Remove(startIndex);
      }
      return true;
    }
  }
}
