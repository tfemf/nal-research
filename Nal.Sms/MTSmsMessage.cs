// Decompiled with JetBrains decompiler
// Type: Nal.Sms.MTSmsMessage
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.Sms
{
  public class MTSmsMessage
  {
    public const int MaxDataLen = 160;
    private byte serviceCenterAddressType;
    private string serviceCenterAddress;
    private byte originatorAddressType;
    private string originatorAddress;
    private DateTimeOffset serviceCenterTimeStamp;
    private List<byte> userData;

    public MTSmsMessage()
    {
      this.serviceCenterAddressType = (byte) 145;
      this.serviceCenterAddress = "881662900005";
      this.originatorAddressType = (byte) 145;
      this.originatorAddress = string.Empty;
      this.serviceCenterTimeStamp = new DateTimeOffset();
      this.userData = new List<byte>();
    }

    public byte ServiceCenterAddressType
    {
      get => this.serviceCenterAddressType;
      set => this.serviceCenterAddressType = value;
    }

    public string ServiceCenterAddress
    {
      get => this.serviceCenterAddress;
      set
      {
        foreach (char c in value)
        {
          if (!char.IsDigit(c))
            throw new ArgumentException();
        }
        this.serviceCenterAddress = value;
      }
    }

    public byte OriginatorAddressType
    {
      get => this.originatorAddressType;
      set => this.originatorAddressType = value;
    }

    public string OriginatorAddress
    {
      get => this.originatorAddress;
      set => this.originatorAddress = value;
    }

    public DateTimeOffset ServiceCenterTimeStamp
    {
      get => this.serviceCenterTimeStamp;
      set => this.serviceCenterTimeStamp = value;
    }

    public IList<byte> UserData => (IList<byte>) this.userData;

    public static MTSmsMessage Parse(string pdu)
    {
      MTSmsMessage mtSmsMessage1 = new MTSmsMessage();
      int num1 = 0;
      List<byte> bytes = SmsUtilities.ConvertHexStrToBytes(pdu);
      if (num1 + 1 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList1 = bytes;
      int index1 = num1;
      int num2 = index1 + 1;
      byte num3 = byteList1[index1];
      if (num3 > (byte) 0)
      {
        if (num3 > (byte) 11 || num2 + (int) num3 > bytes.Count)
          throw new ArgumentException();
        int count = (int) num3 - 1;
        MTSmsMessage mtSmsMessage2 = mtSmsMessage1;
        List<byte> byteList2 = bytes;
        int index2 = num2;
        int index3 = index2 + 1;
        int num4 = (int) byteList2[index2];
        mtSmsMessage2.serviceCenterAddressType = (byte) num4;
        mtSmsMessage1.serviceCenterAddress = SmsUtilities.ConvertFromBcd(bytes.GetRange(index3, count));
        num2 = index3 + count;
      }
      else
      {
        mtSmsMessage1.serviceCenterAddressType = (byte) 145;
        mtSmsMessage1.serviceCenterAddress = string.Empty;
      }
      if (num2 + 4 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList3 = bytes;
      int index4 = num2;
      int num5 = index4 + 1;
      if (((int) byteList3[index4] & 3) != 0)
        throw new ArgumentException();
      List<byte> byteList4 = bytes;
      int index5 = num5;
      int num6 = index5 + 1;
      byte num7 = byteList4[index5];
      MTSmsMessage mtSmsMessage3 = mtSmsMessage1;
      List<byte> byteList5 = bytes;
      int index6 = num6;
      int index7 = index6 + 1;
      int num8 = (int) byteList5[index6];
      mtSmsMessage3.originatorAddressType = (byte) num8;
      int count1 = (int) num7 / 2 + (int) num7 % 2;
      if (count1 > 10 || index7 + count1 > bytes.Count)
        throw new ArgumentException();
      mtSmsMessage1.originatorAddress = SmsUtilities.ConvertFromBcd(bytes.GetRange(index7, count1));
      int num9 = index7 + count1;
      if (num9 + 2 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList6 = bytes;
      int index8 = num9;
      int num10 = index8 + 1;
      int num11 = (int) byteList6[index8];
      List<byte> byteList7 = bytes;
      int index9 = num10;
      int index10 = index9 + 1;
      int num12 = (int) byteList7[index9];
      if (index10 + 7 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList8 = SmsUtilities.UnpackValues((IEnumerable<byte>) bytes.GetRange(index10, 7), 4, false);
      int year = 2000 + (int) byteList8[0] * 10 + (int) byteList8[1];
      int month = (int) byteList8[2] * 10 + (int) byteList8[3];
      int day = (int) byteList8[4] * 10 + (int) byteList8[5];
      int hour = (int) byteList8[6] * 10 + (int) byteList8[7];
      int minute = (int) byteList8[8] * 10 + (int) byteList8[9];
      int second = (int) byteList8[10] * 10 + (int) byteList8[11];
      bool flag = ((int) byteList8[12] & 8) == 8;
      int num13 = ((((int) byteList8[12] & 7) << 4) + (int) byteList8[13]) * (flag ? -1 : 1);
      mtSmsMessage1.serviceCenterTimeStamp = new DateTimeOffset(year, month, day, hour, minute, second, new TimeSpan(num13 / 4, num13 % 4 * 15, 0));
      int num14 = index10 + 7;
      if (num14 + 1 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList9 = bytes;
      int index11 = num14;
      int index12 = index11 + 1;
      int valuesLength = (int) byteList9[index11];
      int packedLength = SmsUtilities.GetPackedLength(valuesLength, 7, false);
      if (valuesLength > 160 || index12 + packedLength != bytes.Count)
        throw new ArgumentException();
      mtSmsMessage1.userData = SmsUtilities.UnpackValues((IEnumerable<byte>) bytes.GetRange(index12, packedLength), 7, true);
      return mtSmsMessage1;
    }
  }
}
