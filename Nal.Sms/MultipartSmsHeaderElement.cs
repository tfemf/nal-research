// Decompiled with JetBrains decompiler
// Type: Nal.Sms.MultipartSmsHeaderElement
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System.Collections.Generic;

#nullable disable
namespace Nal.Sms
{
  public class MultipartSmsHeaderElement : SmsHeaderElement
  {
    public MultipartSmsHeaderElement(byte referenceNumber, byte total, byte sequenceNumber)
    {
      this.ReferenceNumber = referenceNumber;
      this.Total = total;
      this.SequenceNumber = sequenceNumber;
    }

    public override byte Id => 0;

    public byte ReferenceNumber { get; set; }

    public byte Total { get; set; }

    public byte SequenceNumber { get; set; }

    public static bool Parse(IList<byte> data, out MultipartSmsHeaderElement element)
    {
      if (data.Count == 5 && data[0] == (byte) 0 && data[1] == (byte) 3)
      {
        byte referenceNumber = data[2];
        byte total = data[3];
        byte sequenceNumber = data[4];
        if (sequenceNumber >= (byte) 1 && total >= (byte) 1 && (int) sequenceNumber <= (int) total)
        {
          element = new MultipartSmsHeaderElement(referenceNumber, total, sequenceNumber);
          return true;
        }
      }
      element = (MultipartSmsHeaderElement) null;
      return false;
    }
  }
}
