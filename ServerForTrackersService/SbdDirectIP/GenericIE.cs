// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.GenericIE
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class GenericIE : InfoElement
  {
    private byte id;
    private List<byte> content;

    public GenericIE(byte id)
    {
      this.id = id;
      this.content = new List<byte>();
    }

    public GenericIE(byte id, IEnumerable<byte> data)
    {
      this.id = id;
      this.content = new List<byte>(data);
    }

    public override byte Id => this.id;

    public List<byte> Content => this.content;

    public static bool Parse(IList<byte> data, out GenericIE ie)
    {
      ie = (GenericIE) null;
      if (data.Count < 3)
        return false;
      byte id = data[0];
      int count = (int) data[1] * 256 + (int) data[2];
      if (3 + count > data.Count)
        return false;
      ie = new GenericIE(id);
      ie.Content.AddRange(data.Skip<byte>(3).Take<byte>(count));
      return true;
    }

    public override List<byte> GetBytes()
    {
      List<byte> bytes = new List<byte>();
      bytes.Add(this.Id);
      bytes.Add((byte) (this.content.Count / 256));
      bytes.Add((byte) this.content.Count);
      bytes.AddRange((IEnumerable<byte>) this.content);
      return bytes;
    }
  }
}
