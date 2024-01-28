// Decompiled with JetBrains decompiler
// Type: Nal.Sms.GenericSmsHeaderElement
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Nal.Sms
{
  public class GenericSmsHeaderElement : SmsHeaderElement
  {
    private byte id;
    private List<byte> content;

    public GenericSmsHeaderElement(byte id, IEnumerable<byte> data)
    {
      this.id = id;
      this.content = new List<byte>(data);
    }

    public override byte Id => this.id;

    public List<byte> Content => this.content;

    public static bool Parse(IList<byte> data, out GenericSmsHeaderElement element)
    {
      if (data.Count >= 2)
      {
        byte id = data[0];
        byte count = data[1];
        if (data.Count == 2 + (int) count)
        {
          element = new GenericSmsHeaderElement(id, data.Skip<byte>(2).Take<byte>((int) count));
          return true;
        }
      }
      element = (GenericSmsHeaderElement) null;
      return false;
    }
  }
}
