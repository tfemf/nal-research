// Decompiled with JetBrains decompiler
// Type: Nal.Sms.SmsHeaderElement
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System.Collections.Generic;

#nullable disable
namespace Nal.Sms
{
  public abstract class SmsHeaderElement
  {
    public abstract byte Id { get; }

    public static bool Parse(IList<byte> data, out SmsHeaderElement element)
    {
      element = (SmsHeaderElement) null;
      if (data.Count < 1)
        return false;
      switch (data[0])
      {
        case 0:
          MultipartSmsHeaderElement element1;
          if (MultipartSmsHeaderElement.Parse(data, out element1))
          {
            element = (SmsHeaderElement) element1;
            break;
          }
          break;
        case 1:
          ImeiSmsHeaderElement element2;
          if (ImeiSmsHeaderElement.Parse(data, out element2))
          {
            element = (SmsHeaderElement) element2;
            break;
          }
          break;
        default:
          GenericSmsHeaderElement element3;
          if (GenericSmsHeaderElement.Parse(data, out element3))
          {
            element = (SmsHeaderElement) element3;
            break;
          }
          break;
      }
      return element != null;
    }

    public static class Ids
    {
      public const byte Multipart = 0;
      public const byte Imei = 1;
    }
  }
}
