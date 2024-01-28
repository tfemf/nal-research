// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.MTHeaderIE
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class MTHeaderIE : InfoElement
  {
    public MTHeaderIE() => this.ClientMessageId = new byte[4];

    public override byte Id => 65;

    public byte[] ClientMessageId { get; set; }

    public string Imei { get; set; }

    public ushort DispositionFlags { get; set; }

    public static bool Parse(IList<byte> data, out MTHeaderIE ie)
    {
      if (data.Count == 24 && data[0] == (byte) 65 && data[1] == (byte) 0 && data[2] == (byte) 21)
      {
        ie = new MTHeaderIE();
        ie.ClientMessageId = data.Skip<byte>(3).Take<byte>(4).ToArray<byte>();
        ie.Imei = Encoding.ASCII.GetString(data.Skip<byte>(7).Take<byte>(15).ToArray<byte>());
        ie.DispositionFlags = (ushort) ((uint) data[22] * 256U + (uint) data[23]);
        return true;
      }
      ie = (MTHeaderIE) null;
      return false;
    }

    public override bool Validate()
    {
      return this.ClientMessageId.Length == 4 && this.Imei != null && this.Imei.Length == 15 && this.Imei.All<char>((Func<char, bool>) (x => char.IsDigit(x)));
    }

    public override List<byte> GetBytes()
    {
      if (!this.Validate())
        return (List<byte>) null;
      List<byte> bytes = new List<byte>();
      bytes.Add(this.Id);
      bytes.Add((byte) 0);
      bytes.Add((byte) 21);
      bytes.Add(this.ClientMessageId[0]);
      bytes.Add(this.ClientMessageId[1]);
      bytes.Add(this.ClientMessageId[2]);
      bytes.Add(this.ClientMessageId[3]);
      bytes.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.Imei));
      bytes.Add((byte) ((uint) this.DispositionFlags / 256U));
      bytes.Add((byte) this.DispositionFlags);
      return bytes;
    }
  }
}
