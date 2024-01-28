// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsReport4
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class NalGpsReport4 : NalGpsReport
  {
    private const int HeaderSize = 2;
    private const int GpsPointSize = 28;
    private const int MaxIdentifierSize = 50;
    private const int EncryptionBlockSize = 16;
    private List<NalGpsPoint4> points;

    public NalGpsReport4() => this.points = new List<NalGpsPoint4>();

    public string Id { get; set; }

    public List<NalGpsPoint4> Points => this.points;

    public override bool IsEmergency()
    {
      foreach (NalGpsPoint4 point in this.points)
      {
        if (point.Emergency)
          return true;
      }
      return false;
    }

    public override XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "nalGpsReport4", (object) this.points.Select<NalGpsPoint4, XElement>((Func<NalGpsPoint4, XElement>) (x => x.ToXElement())));
      if (this.Id != null)
        xelement.AddFirst((object) new XElement((XName) "id", (object) this.Id));
      return xelement;
    }

    public static bool Parse(byte[] data, out NalGpsReport4 report)
    {
      report = (NalGpsReport4) null;
      if (data.Length < 30 || data[0] != (byte) 4)
        return false;
      int num = (int) data[1];
      int index1 = 2 + 28 * num;
      if (data.Length < index1 || data.Length > index1 + 50 + 16 + 3)
        return false;
      report = new NalGpsReport4();
      byte[] numArray = new byte[28];
      bool flag = false;
      for (int index2 = 0; index2 < num; ++index2)
      {
        Array.Copy((Array) data, 2 + 28 * index2, (Array) numArray, 0, numArray.Length);
        NalGpsPoint4 point;
        bool idIncluded;
        if (!NalGpsPoint4.Parse(numArray, out point, out idIncluded))
        {
          report = (NalGpsReport4) null;
          return false;
        }
        if (idIncluded)
          flag = true;
        report.points.Add(point);
      }
      if (flag)
      {
        report.Id = Encoding.ASCII.GetString(data, index1, data.Length - index1);
        int startIndex = report.Id.IndexOf('\r');
        if (startIndex != -1)
          report.Id = report.Id.Remove(startIndex);
      }
      return true;
    }
  }
}
