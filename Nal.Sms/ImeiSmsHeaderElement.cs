// Decompiled with JetBrains decompiler
// Type: Nal.Sms.ImeiSmsHeaderElement
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System.Collections.Generic;

#nullable disable
namespace Nal.Sms
{
  public class ImeiSmsHeaderElement : SmsHeaderElement
  {
    public ImeiSmsHeaderElement(ulong imei) => this.Imei = imei;

    public override byte Id => 1;

    public ulong Imei { get; set; }

    public static bool Parse(IList<byte> data, out ImeiSmsHeaderElement element)
    {
      if (data.Count == 10 && data[0] == (byte) 1 && data[1] == (byte) 8)
      {
        ulong imei = 0;
        for (int index = 2; index < 10; ++index)
          imei = imei * 256UL + (ulong) data[index];
        if (imei <= 999999999999999UL)
        {
          element = new ImeiSmsHeaderElement(imei);
          return true;
        }
      }
      element = (ImeiSmsHeaderElement) null;
      return false;
    }
  }
}
