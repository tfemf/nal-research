// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosP3Message
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
  public class PecosP3Message : PecosMessage
  {
    public override short Id => 903;

    public int BrevityCode { get; set; }

    public float Pdop { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public float Altitude { get; set; }

    public float Course { get; set; }

    public float GroundVelocity { get; set; }

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
      return data;
    }

    public override XElement ToXElement()
    {
      return new XElement((XName) "pecosP3Message", new object[9]
      {
        (object) new XElement((XName) "imei", (object) this.Imei),
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".fff")),
        (object) new XElement((XName) "brevity", (object) this.BrevityCode),
        (object) new XElement((XName) "pdop", (object) this.Pdop.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lat", (object) this.Latitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "lng", (object) this.Longitude.ToString("0.0000000", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "alt", (object) this.Altitude.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "course", (object) this.Course.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gndVel", (object) this.GroundVelocity.ToString("0.#####", (IFormatProvider) CultureInfo.InvariantCulture))
      });
    }

    public static bool ParsePayload(IList<byte> data, int pos, int len, out PecosP3Message report)
    {
      report = (PecosP3Message) null;
      if (len >= pos + 36)
      {
        report = new PecosP3Message();
        report.BrevityCode = PecosMessage.ExtractInt32((IEnumerable<byte>) data, ref pos);
        report.Pdop = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Latitude = PecosMessage.ExtractDouble((IEnumerable<byte>) data, ref pos);
        report.Longitude = PecosMessage.ExtractDouble((IEnumerable<byte>) data, ref pos);
        report.Altitude = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.Course = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
        report.GroundVelocity = PecosMessage.ExtractSingle((IEnumerable<byte>) data, ref pos);
      }
      return report != null;
    }
  }
}
