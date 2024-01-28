// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsReport6
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class NalGpsReport6 : NalGpsReport
  {
    private List<string> routingEmailAddresses;
    private List<string> routingImeis;
    private List<string> routingPhoneNumbers;

    public NalGpsReport6()
    {
      this.routingEmailAddresses = new List<string>();
      this.routingImeis = new List<string>();
      this.routingPhoneNumbers = new List<string>();
    }

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

    public byte AddressBookCode { get; set; }

    public byte CannedMessageCode { get; set; }

    public string FreeText { get; set; }

    public List<string> RoutingEmailAddresses => this.routingEmailAddresses;

    public List<string> RoutingImeis => this.routingImeis;

    public List<string> RoutingPhoneNumbers => this.routingPhoneNumbers;

    public override bool IsEmergency() => this.Emergency;

    public override XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "nalGpsReport6", new object[16]
      {
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".f")),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "alt", (object) this.Altitude.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gndVel", (object) this.GroundVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "course", (object) this.Course.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "verVel", (object) this.VerticalVelocity.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "fix", (object) this.Fix.ToXElementText()),
        (object) new XElement((XName) "sats", (object) this.Satellites),
        (object) new XElement((XName) "hdop", (object) this.Hdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "vdop", (object) this.Vdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "motion", this.Motion ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emer", this.Emergency ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "emerAcked", this.EmergencyAcknowledged ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "abCode", (object) this.AddressBookCode),
        (object) new XElement((XName) "cmCode", (object) this.CannedMessageCode)
      });
      if (this.FreeText != null)
        xelement.Add((object) new XElement((XName) "freeText", (object) this.FreeText));
      if (this.RoutingEmailAddresses.Any<string>() || this.RoutingPhoneNumbers.Any<string>() || this.RoutingImeis.Any<string>())
        xelement.Add((object) new XElement((XName) "routing", new object[3]
        {
          (object) this.RoutingEmailAddresses.Select<string, XElement>((Func<string, XElement>) (x => new XElement((XName) "email", (object) x))),
          (object) this.RoutingPhoneNumbers.Select<string, XElement>((Func<string, XElement>) (x => new XElement((XName) "phone", (object) x))),
          (object) this.RoutingImeis.Select<string, XElement>((Func<string, XElement>) (x => new XElement((XName) "imei", (object) x)))
        }));
      return xelement;
    }

    public static bool Parse(byte[] data, out NalGpsReport6 report)
    {
      report = (NalGpsReport6) null;
      if (data.Length < 30 || data[0] != (byte) 6)
        return false;
      report = new NalGpsReport6();
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(data));
      int num1 = (int) binaryReader.ReadByte();
      report.AddressBookCode = binaryReader.ReadByte();
      ulong extractFrom1 = binaryReader.ReadUInt64();
      ulong extractFrom2 = binaryReader.ReadUInt64();
      ulong extractFrom3 = binaryReader.ReadUInt64();
      int num2 = (int) binaryReader.ReadUInt16();
      byte num3 = binaryReader.ReadByte();
      byte num4 = binaryReader.ReadByte();
      bool flag1 = NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000000000000UL) == 1UL;
      int valueAfter1 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000000000UL);
      int valueAfter2 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100000000000000UL);
      int valueAfter3 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 10000000000UL);
      int valueAfter4 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100000000UL);
      int valueAfter5 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 1000000UL);
      int valueAfter6 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom1, 100UL);
      int num5 = (int) extractFrom1;
      bool flag2 = NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000000000000000UL) != 1UL;
      int valueAfter7 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000000000000000UL);
      int valueAfter8 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000000000UL);
      int valueAfter9 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 1000000000UL);
      int valueAfter10 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000000UL);
      int valueAfter11 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100000UL);
      int valueAfter12 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 10000UL);
      int valueAfter13 = (int) NalGpsReport.ExtractValueAfter(ref extractFrom2, 100UL);
      int num6 = (int) extractFrom2;
      bool flag3 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000000000000000UL) != 1UL;
      ulong valueAfter14 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100000000000000UL);
      byte valueAfter15 = (byte) NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000000000UL);
      ulong valueAfter16 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100000000UL);
      byte valueAfter17 = (byte) NalGpsReport.ExtractValueAfter(ref extractFrom3, 10000000UL);
      ulong valueAfter18 = NalGpsReport.ExtractValueAfter(ref extractFrom3, 100UL);
      report.CannedMessageCode = (byte) extractFrom3;
      int month = num2 / 1000;
      int day = num2 % 1000 / 10;
      int num7 = num2 % 10;
      report.Satellites = (byte) ((uint) num3 / 10U);
      int num8 = (int) num3 % 10;
      bool flag4 = ((uint) num4 & 1U) > 0U;
      bool flag5 = ((uint) num4 & 2U) > 0U;
      bool flag6 = ((uint) num4 & 4U) > 0U;
      bool flag7 = ((uint) num4 & 8U) > 0U;
      report.Emergency = ((uint) num4 & 16U) > 0U;
      report.Motion = ((uint) num4 & 32U) > 0U;
      report.EmergencyAcknowledged = ((uint) num4 & 64U) > 0U;
      bool flag8;
      int num9;
      if (valueAfter1 < 200)
      {
        flag8 = false;
        num9 = valueAfter1;
      }
      else
      {
        flag8 = true;
        num9 = valueAfter1 - 200;
      }
      try
      {
        report.Time = valueAfter9 != 0 || month != 0 || day != 0 || valueAfter7 != 0 || valueAfter10 != 0 || valueAfter11 != 0 || valueAfter12 != 0 ? new DateTime(valueAfter9, month, day, valueAfter7, valueAfter10, valueAfter11, valueAfter12 * 100, DateTimeKind.Utc) : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        report.Latitude = ((double) valueAfter4 + ((double) valueAfter5 + (double) valueAfter6 / 10000.0) / 60.0) * (flag1 ? -1.0 : 1.0);
        report.Longitude = ((double) num9 + ((double) valueAfter2 + (double) valueAfter3 / 10000.0) / 60.0) * (flag8 ? -1.0 : 1.0);
        report.Altitude = (double) valueAfter16 / Math.Pow(10.0, (double) (5 - (int) valueAfter15)) * (flag3 ? -1.0 : 1.0);
        report.GroundVelocity = (double) valueAfter18 / Math.Pow(10.0, (double) (5 - (int) valueAfter17));
        report.Course = (double) valueAfter14 / 100.0;
        report.VerticalVelocity = ((double) (num8 * 100 + num7 * 10) + (double) valueAfter13 / 10.0) * (flag2 ? -1.0 : 1.0);
        report.Hdop = (double) num5 + (double) num6 / 100.0;
        report.Vdop = (double) valueAfter8 / 100.0;
        report.Fix = flag6 ? PositionFix.Valid3D : (flag4 ? PositionFix.TwoD : PositionFix.Other);
        if (report.Latitude >= -90.0 && report.Latitude <= 90.0 && report.Longitude >= -180.0)
        {
          if (report.Longitude <= 180.0)
            goto label_10;
        }
        report = (NalGpsReport6) null;
        return false;
      }
      catch (ArgumentException ex)
      {
        report = (NalGpsReport6) null;
        return false;
      }
label_10:
      int index = 30;
      if (flag5 && index + 1 <= data.Length)
      {
        byte num10 = binaryReader.ReadByte();
        ++index;
        if (((int) num10 & 1) != 0 && index + 1 <= data.Length)
        {
          byte count = binaryReader.ReadByte();
          ++index;
          if (index + (int) count <= data.Length)
          {
            string str = Encoding.GetEncoding(1252).GetString(binaryReader.ReadBytes((int) count));
            report.RoutingEmailAddresses.AddRange((IEnumerable<string>) str.Split(','));
            index += (int) count;
          }
        }
        int num11 = (int) num10 & 2;
        int num12 = (int) num10 & 4;
      }
      if (flag7)
      {
        report.FreeText = Encoding.GetEncoding(1252).GetString(data, index, data.Length - index);
        int startIndex = report.FreeText.IndexOf('\r');
        if (startIndex != -1)
          report.FreeText = report.FreeText.Remove(startIndex);
      }
      return true;
    }
  }
}
