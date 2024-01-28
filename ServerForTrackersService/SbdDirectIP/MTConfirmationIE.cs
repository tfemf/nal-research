// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.MTConfirmationIE
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class MTConfirmationIE : InfoElement
  {
    public override byte Id => 68;

    public byte[] ClientMessageId { get; set; }

    public string Imei { get; set; }

    public uint AutoIdRef { get; set; }

    public short Status { get; set; }

    public static bool Parse(IList<byte> data, out MTConfirmationIE ie)
    {
      if (data.Count == 28 && data[0] == (byte) 68 && data[1] == (byte) 0 && data[2] == (byte) 25)
      {
        ie = new MTConfirmationIE();
        ie.ClientMessageId = data.Skip<byte>(3).Take<byte>(4).ToArray<byte>();
        ie.Imei = Encoding.ASCII.GetString(data.Skip<byte>(7).Take<byte>(15).ToArray<byte>());
        ie.AutoIdRef = (uint) ((int) data[22] * 16777216 + (int) data[23] * 65536 + (int) data[24] * 256) + (uint) data[25];
        ie.Status = (short) ((int) data[26] * 256 + (int) data[27]);
        return true;
      }
      ie = (MTConfirmationIE) null;
      return false;
    }

    public override bool Validate()
    {
      return this.ClientMessageId != null && this.ClientMessageId.Length == 4;
    }

    public override List<byte> GetBytes()
    {
      if (!this.Validate())
        return (List<byte>) null;
      List<byte> bytes = new List<byte>();
      bytes.Add(this.Id);
      bytes.Add((byte) 0);
      bytes.Add((byte) 25);
      bytes.Add(this.ClientMessageId[0]);
      bytes.Add(this.ClientMessageId[1]);
      bytes.Add(this.ClientMessageId[2]);
      bytes.Add(this.ClientMessageId[3]);
      bytes.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.Imei));
      bytes.Add((byte) (this.AutoIdRef / 16777216U));
      bytes.Add((byte) (this.AutoIdRef / 65536U));
      bytes.Add((byte) (this.AutoIdRef / 256U));
      bytes.Add((byte) this.AutoIdRef);
      bytes.Add((byte) ((uint) this.Status / 256U));
      bytes.Add((byte) this.Status);
      return bytes;
    }
  }
}
