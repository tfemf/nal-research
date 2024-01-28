// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosB5Message
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
  public class PecosB5Message : PecosMessage
  {
    private ulong destinationImei;

    public PecosB5Message() => this.destinationImei = 0UL;

    public override short Id => 401;

    public ulong DestinationImei
    {
      get => this.destinationImei;
      set
      {
        if (value > 999999999999999UL)
          return;
        this.destinationImei = value;
      }
    }

    public List<byte> PayloadData { get; set; }

    public override List<byte> GetPayload()
    {
      List<byte> data = new List<byte>();
      PecosMessage.AppendUInt64(data, this.DestinationImei);
      data.AddRange((IEnumerable<byte>) this.PayloadData);
      return data;
    }

    public override XElement ToXElement()
    {
      object content = (object) null;
      bool isEncrypted;
      ulong imei;
      byte[] innerData;
      PecosMessage message;
      if (PecosMessage.ParseOuter((IList<byte>) this.PayloadData, out isEncrypted, out imei, out innerData) && !isEncrypted && PecosMessage.ParseInner(isEncrypted, imei, (IList<byte>) innerData, out message))
        content = (object) message.ToXElement();
      if (content == null)
      {
        StringBuilder sb = new StringBuilder();
        this.PayloadData.ForEach((Action<byte>) (x => sb.Append(string.Format("{0:X2}", (object) x))));
        content = (object) sb.ToString();
      }
      return new XElement((XName) "pecosB5Message", new object[4]
      {
        (object) new XElement((XName) "imei", (object) this.Imei),
        (object) new XElement((XName) "time", (object) this.Time.ToXElementText(".fff")),
        (object) new XElement((XName) "destinationImei", (object) this.DestinationImei),
        (object) new XElement((XName) "payload", content)
      });
    }

    public static bool ParsePayload(
      IList<byte> data,
      int pos,
      int len,
      out PecosB5Message message)
    {
      message = (PecosB5Message) null;
      if (len >= pos + 8)
      {
        ulong uint64 = PecosMessage.ExtractUInt64((IEnumerable<byte>) data, ref pos);
        if (uint64 <= 999999999999999UL)
        {
          message = new PecosB5Message();
          message.DestinationImei = uint64;
          message.PayloadData = data.Skip<byte>(pos).Take<byte>(len - pos).ToList<byte>();
        }
      }
      return message != null;
    }
  }
}
