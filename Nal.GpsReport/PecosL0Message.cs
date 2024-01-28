// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosL0Message
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class PecosL0Message : PecosMessage
  {
    public override short Id => 756;

    public string RawText { get; set; }

    public string Text
    {
      get
      {
        int startIndex = this.RawText.IndexOf(char.MinValue);
        return startIndex != -1 ? this.RawText.Remove(startIndex) : this.RawText;
      }
    }

    public override List<byte> GetPayload()
    {
      List<byte> payload = new List<byte>();
      payload.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.RawText));
      return payload;
    }

    public override XElement ToXElement()
    {
      return new XElement((XName) "pecosL0Message", new object[3]
      {
        (object) new XElement((XName) "imei", (object) this.Imei),
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".fff")),
        (object) new XElement((XName) "text", (object) this.Text)
      });
    }

    public static bool ParsePayload(
      IList<byte> data,
      int pos,
      int len,
      out PecosL0Message message)
    {
      message = new PecosL0Message();
      message.RawText = Encoding.ASCII.GetString(data.Skip<byte>(pos).Take<byte>(len - pos).ToArray<byte>());
      return true;
    }
  }
}
