// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosB3Message
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
  public class PecosB3Message : PecosMessage
  {
    public override short Id => 399;

    public List<byte> PayloadData { get; set; }

    public override List<byte> GetPayload() => new List<byte>((IEnumerable<byte>) this.PayloadData);

    public override XElement ToXElement() => this.ToXElement((Func<IList<byte>, XElement>) null);

    public XElement ToXElement(Func<IList<byte>, XElement> parsePayload)
    {
      object content = (object) null;
      if (parsePayload != null)
        content = (object) parsePayload((IList<byte>) this.PayloadData);
      if (content == null)
      {
        StringBuilder sb = new StringBuilder();
        this.PayloadData.ForEach((Action<byte>) (x => sb.Append(string.Format("{0:X2}", (object) x))));
        content = (object) sb.ToString();
      }
      return new XElement((XName) "pecosB3Message", new object[3]
      {
        (object) new XElement((XName) "imei", (object) this.Imei),
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".fff")),
        (object) new XElement((XName) "payload", content)
      });
    }

    public static bool ParsePayload(
      IList<byte> data,
      int pos,
      int len,
      out PecosB3Message message)
    {
      message = new PecosB3Message();
      message.PayloadData = data.Skip<byte>(pos).Take<byte>(len - pos).ToList<byte>();
      return true;
    }
  }
}
