// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosP4Message
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class PecosP4Message : PecosMessage
  {
    public override short Id => 904;

    public int BrevityCode { get; set; }

    public float Pdop { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public float Altitude { get; set; }

    public float Course { get; set; }

    public float GroundVelocity { get; set; }

    public byte Quality { get; set; }

    public byte FixMode { get; set; }

    public byte FixType { get; set; }

    public float Variation { get; set; }

    public float Hdop { get; set; }

    public float Vdop { get; set; }

    public PositionFix Fix
    {
      get
      {
        if (this.FixType == (byte) 51)
          return PositionFix.ThreeD;
        return this.FixType != (byte) 50 ? PositionFix.Other : PositionFix.TwoD;
      }
    }

    public override List<byte> GetPayload()
    {
      List<byte> data = new List<byte>();
      PecosMessage.AppendInt32(data, this.BrevityCode);
      PecosMessage.AppendSingle(data, this.Pdop);
      PecosMessage.AppendDouble(data, this.Latitude);
      PecosMessage.AppendDouble(data, this.Longitude);
      PecosMessage.AppendSingle(data, this.Altitude);
      PecosMessage.AppendSingle(data, this.Course);
      PecosMessage.AppendSingle(data, this.GroundVelocity);
      data.Add(this.Quality);
      data.Add(this.FixMode);
      data.Add(this.FixType);
      PecosMessage.AppendSingle(data, this.Variation);
      PecosMessage.AppendSingle(data, this.Hdop);
      PecosMessage.AppendSingle(data, this.Vdop);
      return data;
    }

    public override XElement ToXElement()
    {
      return new XElement((XName) "pecosP4Message", new object[12]
      {
        (object) new XElement((XName) "imei", (object) this.Imei),
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".fff")),
        (object) new XElement((XName) "brevity", (object) this.BrevityCode),
        (object) new XElement((XName) "pdop", (object) this.Pdop.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "alt", (object) this.Altitude.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "course", (object) this.Course.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gndVel", (object) this.GroundVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "fix", (object) this.Fix.ToXElementText()),
        (object) new XElement((XName) "hdop", (object) this.Hdop.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "vdop", (object) this.Vdop.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture))
      });
    }

    public static bool ParsePayload(IList<byte> data, int pos, int len, out PecosP4Message report)
    {
      report = (PecosP4Message) null;
      if (len >= pos + 51)
      {
        report = new PecosP4Message();
        report.BrevityCode = PecosMessage.ExtractInt32((IEnumerable<byte>) data, ref pos);
        report.Pdop = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Latitude = PecosMessage.ExtractDouble((IEnumerable<byte>) data, ref pos);
        report.Longitude = PecosMessage.ExtractDouble((IEnumerable<byte>) data, ref pos);
        report.Altitude = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Course = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.GroundVelocity = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Quality = data[pos++];
        report.FixMode = data[pos++];
        report.FixType = data[pos++];
        report.Variation = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Hdop = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Vdop = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
      }
      return report != null;
    }
  }
}
