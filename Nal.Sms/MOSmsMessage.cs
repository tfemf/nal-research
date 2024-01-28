// Decompiled with JetBrains decompiler
// Type: Nal.Sms.MOSmsMessage
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.Sms
{
  public class MOSmsMessage
  {
    public const int MaxDataLen = 160;
    private byte serviceCenterAddressType;
    private string serviceCenterAddress;
    private bool replyPath;
    private bool userDataHeaderIndicator;
    private bool statusReportIndication;
    private bool moreMessagesToSend;
    private byte destinationAddressType;
    private string destinationAddress;
    private byte messageReference;
    private byte protocolId;
    private byte dataCodingScheme;
    private ValidityPeriodFormat validityPeriodFormat;
    private byte relativeValidityPeriod;
    private DateTimeOffset absoluteValidityPeriod;
    private List<byte> userData;

    public MOSmsMessage()
    {
      this.serviceCenterAddressType = (byte) 145;
      this.serviceCenterAddress = "881662900005";
      this.destinationAddressType = (byte) 145;
      this.destinationAddress = string.Empty;
      this.validityPeriodFormat = ValidityPeriodFormat.Relative;
      this.relativeValidityPeriod = byte.MaxValue;
      this.absoluteValidityPeriod = (DateTimeOffset) DateTime.UtcNow.AddDays(1.0);
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

    public bool ReplyPath
    {
      get => this.replyPath;
      set => this.replyPath = value;
    }

    public bool UserDataHeaderIndicator
    {
      get => this.userDataHeaderIndicator;
      set => this.userDataHeaderIndicator = value;
    }

    public bool StatusReportIndication
    {
      get => this.statusReportIndication;
      set => this.statusReportIndication = value;
    }

    public bool MoreMessagesToSend
    {
      get => this.moreMessagesToSend;
      set => this.moreMessagesToSend = value;
    }

    public byte DestinationAddressType
    {
      get => this.destinationAddressType;
      set => this.destinationAddressType = value;
    }

    public string DestinationAddress
    {
      get => this.destinationAddress;
      set => this.destinationAddress = value;
    }

    public byte MessageReference
    {
      get => this.messageReference;
      set => this.messageReference = value;
    }

    public byte ProtocolId
    {
      get => this.protocolId;
      set => this.protocolId = value;
    }

    public byte DataCodingScheme
    {
      get => this.dataCodingScheme;
      set => this.dataCodingScheme = value;
    }

    public ValidityPeriodFormat ValidityPeriodFormat
    {
      get => this.validityPeriodFormat;
      set => this.validityPeriodFormat = value;
    }

    public byte RelativeValidityPeriod
    {
      get => this.relativeValidityPeriod;
      set => this.relativeValidityPeriod = value;
    }

    public DateTimeOffset AbsoluteValidityPeriod
    {
      get => this.absoluteValidityPeriod;
      set => this.absoluteValidityPeriod = value;
    }

    public List<byte> UserData => this.userData;

    public static MOSmsMessage Parse(string pdu)
    {
      MOSmsMessage moSmsMessage1 = new MOSmsMessage();
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
        MOSmsMessage moSmsMessage2 = moSmsMessage1;
        List<byte> byteList2 = bytes;
        int index2 = num2;
        int index3 = index2 + 1;
        int num4 = (int) byteList2[index2];
        moSmsMessage2.serviceCenterAddressType = (byte) num4;
        moSmsMessage1.serviceCenterAddress = SmsUtilities.ConvertFromBcd(bytes.GetRange(index3, count));
        num2 = index3 + count;
      }
      else
      {
        moSmsMessage1.serviceCenterAddressType = (byte) 145;
        moSmsMessage1.serviceCenterAddress = string.Empty;
      }
      if (num2 + 4 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList3 = bytes;
      int index4 = num2;
      int num5 = index4 + 1;
      byte num6 = byteList3[index4];
      if (((int) num6 & 3) != 1)
        throw new ArgumentException();
      switch ((int) num6 & 24)
      {
        case 0:
          moSmsMessage1.validityPeriodFormat = ValidityPeriodFormat.None;
          break;
        case 8:
          moSmsMessage1.validityPeriodFormat = ValidityPeriodFormat.Enhanced;
          break;
        case 16:
          moSmsMessage1.validityPeriodFormat = ValidityPeriodFormat.Relative;
          break;
        case 24:
          moSmsMessage1.validityPeriodFormat = ValidityPeriodFormat.Absolute;
          break;
      }
      moSmsMessage1.moreMessagesToSend = ((int) num6 & 4) == 4;
      moSmsMessage1.statusReportIndication = ((int) num6 & 32) == 32;
      moSmsMessage1.userDataHeaderIndicator = ((int) num6 & 64) == 64;
      moSmsMessage1.replyPath = ((int) num6 & 128) == 128;
      MOSmsMessage moSmsMessage3 = moSmsMessage1;
      List<byte> byteList4 = bytes;
      int index5 = num5;
      int num7 = index5 + 1;
      int num8 = (int) byteList4[index5];
      moSmsMessage3.messageReference = (byte) num8;
      List<byte> byteList5 = bytes;
      int index6 = num7;
      int num9 = index6 + 1;
      byte num10 = byteList5[index6];
      MOSmsMessage moSmsMessage4 = moSmsMessage1;
      List<byte> byteList6 = bytes;
      int index7 = num9;
      int index8 = index7 + 1;
      int num11 = (int) byteList6[index7];
      moSmsMessage4.destinationAddressType = (byte) num11;
      int count1 = (int) num10 / 2 + (int) num10 % 2;
      if (count1 > 10 || index8 + count1 > bytes.Count)
        throw new ArgumentException();
      moSmsMessage1.destinationAddress = SmsUtilities.ConvertFromBcd(bytes.GetRange(index8, count1));
      int num12 = index8 + count1;
      if (num12 + 2 > bytes.Count)
        throw new ArgumentException();
      MOSmsMessage moSmsMessage5 = moSmsMessage1;
      List<byte> byteList7 = bytes;
      int index9 = num12;
      int num13 = index9 + 1;
      int num14 = (int) byteList7[index9];
      moSmsMessage5.protocolId = (byte) num14;
      MOSmsMessage moSmsMessage6 = moSmsMessage1;
      List<byte> byteList8 = bytes;
      int index10 = num13;
      int index11 = index10 + 1;
      int num15 = (int) byteList8[index10];
      moSmsMessage6.dataCodingScheme = (byte) num15;
      switch (moSmsMessage1.validityPeriodFormat)
      {
        case ValidityPeriodFormat.Relative:
          if (index11 + 1 > bytes.Count)
            throw new ArgumentException();
          moSmsMessage1.relativeValidityPeriod = bytes[index11++];
          break;
        case ValidityPeriodFormat.Absolute:
          if (index11 + 7 > bytes.Count)
            throw new ArgumentException();
          List<byte> byteList9 = SmsUtilities.UnpackValues((IEnumerable<byte>) bytes.GetRange(index11, 7), 4, false);
          int year = 2000 + (int) byteList9[0] * 10 + (int) byteList9[1];
          int month = (int) byteList9[2] * 10 + (int) byteList9[3];
          int day = (int) byteList9[4] * 10 + (int) byteList9[5];
          int hour = (int) byteList9[6] * 10 + (int) byteList9[7];
          int minute = (int) byteList9[8] * 10 + (int) byteList9[9];
          int second = (int) byteList9[10] * 10 + (int) byteList9[11];
          bool flag = ((int) byteList9[12] & 8) == 8;
          int num16 = ((((int) byteList9[12] & 7) << 4) + (int) byteList9[13]) * (flag ? -1 : 1);
          moSmsMessage1.absoluteValidityPeriod = new DateTimeOffset(year, month, day, hour, minute, second, new TimeSpan(num16 / 4, num16 % 4 * 15, 0));
          index11 += 7;
          break;
      }
      if (index11 + 1 > bytes.Count)
        throw new ArgumentException();
      List<byte> byteList10 = bytes;
      int index12 = index11;
      int index13 = index12 + 1;
      int valuesLength = (int) byteList10[index12];
      int packedLength = SmsUtilities.GetPackedLength(valuesLength, 7, false);
      if (valuesLength > 160 || index13 + packedLength != bytes.Count)
        throw new ArgumentException();
      moSmsMessage1.userData = SmsUtilities.UnpackValues((IEnumerable<byte>) bytes.GetRange(index13, packedLength), 7, true);
      return moSmsMessage1;
    }

    public string GetPdu(out int lengthForCommand)
    {
      string empty = string.Empty;
      string str1;
      int num1;
      if (string.IsNullOrEmpty(this.serviceCenterAddress))
      {
        str1 = empty + "00";
      }
      else
      {
        List<byte> bcd = SmsUtilities.ConvertToBcd(this.serviceCenterAddress);
        string str2 = empty;
        num1 = 1 + bcd.Count;
        string str3 = num1.ToString("X2");
        str1 = str2 + str3 + this.serviceCenterAddressType.ToString("X2") + SmsUtilities.ConvertBytesToHexStr((IList<byte>) bcd);
      }
      lengthForCommand = -str1.Length / 2;
      byte num2 = 1;
      switch (this.validityPeriodFormat)
      {
        case ValidityPeriodFormat.None:
          num2 |= (byte) 0;
          break;
        case ValidityPeriodFormat.Enhanced:
          num2 |= (byte) 8;
          break;
        case ValidityPeriodFormat.Relative:
          num2 |= (byte) 16;
          break;
        case ValidityPeriodFormat.Absolute:
          num2 |= (byte) 24;
          break;
      }
      byte num3 = (byte) ((int) (byte) ((int) (byte) ((int) (byte) ((int) num2 | (this.moreMessagesToSend ? 4 : 0)) | (this.statusReportIndication ? 32 : 0)) | (this.userDataHeaderIndicator ? 64 : 0)) | (this.replyPath ? 128 : 0));
      string str4 = str1 + num3.ToString("X2") + this.messageReference.ToString("X2");
      num1 = this.destinationAddress.Length;
      string str5 = num1.ToString("X2");
      string str6 = str4 + str5 + this.destinationAddressType.ToString("X2") + SmsUtilities.ConvertBytesToHexStr((IList<byte>) SmsUtilities.ConvertToBcd(this.destinationAddress)) + this.protocolId.ToString("X2") + this.dataCodingScheme.ToString("X2");
      switch (this.validityPeriodFormat)
      {
        case ValidityPeriodFormat.Relative:
          str6 += this.relativeValidityPeriod.ToString("X2");
          break;
        case ValidityPeriodFormat.Absolute:
          List<byte> values = new List<byte>();
          values.Add((byte) (this.absoluteValidityPeriod.Year % 100 / 10));
          values.Add((byte) (this.absoluteValidityPeriod.Year % 100 % 10));
          values.Add((byte) (this.absoluteValidityPeriod.Month / 10));
          values.Add((byte) (this.absoluteValidityPeriod.Month % 10));
          values.Add((byte) (this.absoluteValidityPeriod.Day / 10));
          values.Add((byte) (this.absoluteValidityPeriod.Day % 10));
          values.Add((byte) (this.absoluteValidityPeriod.Hour / 10));
          values.Add((byte) (this.absoluteValidityPeriod.Hour % 10));
          values.Add((byte) (this.absoluteValidityPeriod.Minute / 10));
          values.Add((byte) (this.absoluteValidityPeriod.Minute % 10));
          values.Add((byte) (this.absoluteValidityPeriod.Second / 10));
          values.Add((byte) (this.absoluteValidityPeriod.Second % 10));
          int num4 = (int) this.absoluteValidityPeriod.Offset.TotalMinutes / 15;
          values.Add((byte) (Math.Abs(num4) / 16 | (num4 < 0 ? 8 : 0)));
          values.Add((byte) (Math.Abs(num4) % 16));
          str6 += SmsUtilities.ConvertBytesToHexStr((IList<byte>) SmsUtilities.PackValues(values, 4, false));
          break;
      }
      List<byte> bytes = SmsUtilities.PackValues(this.userData, 7, false);
      string str7 = str6;
      num1 = this.userData.Count;
      string str8 = num1.ToString("X2");
      string pdu = str7 + str8 + SmsUtilities.ConvertBytesToHexStr((IList<byte>) bytes);
      lengthForCommand += pdu.Length / 2;
      return pdu;
    }
  }
}
