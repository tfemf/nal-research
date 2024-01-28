// Decompiled with JetBrains decompiler
// Type: Nal.Sms.SmsUserData
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Nal.Sms
{
  public class SmsUserData
  {
    private List<SmsHeaderElement> headerElements;
    private List<byte> payload;

    public SmsUserData()
    {
      this.headerElements = new List<SmsHeaderElement>();
      this.payload = new List<byte>();
    }

    public List<SmsHeaderElement> HeaderElements => this.headerElements;

    public List<byte> Payload => this.payload;

    public static bool Parse(IEnumerable<byte> gsmBytes, out SmsUserData userData)
    {
      return SmsUserData.Parse(SmsUtilities.ConvertGsmBytesToStr(gsmBytes), out userData);
    }

    public static bool Parse(string text, out SmsUserData userData)
    {
      userData = new SmsUserData();
      bool flag1 = text.Length > 0 && text[0] == '!';
      if (flag1)
        text = text.Remove(0, 1);
      bool flag2 = text.All<char>((Func<char, bool>) (x => char.IsLetterOrDigit(x) || x == ';' || x == '#'));
      if (flag2)
      {
        List<byte> bytes = SmsUtilities.ConvertSmsBase64StrToBytes(text);
        if (flag1)
        {
          flag2 = false;
          if (bytes.Count >= 1)
          {
            byte count1 = bytes[0];
            bytes.RemoveAt(0);
            if ((int) count1 <= bytes.Count)
            {
              flag2 = true;
              int count2;
              for (int count3 = 0; count3 < (int) count1; count3 += count2)
              {
                if (count3 + 2 > (int) count1)
                {
                  flag2 = false;
                  break;
                }
                count2 = 2 + (int) bytes[count3 + 1];
                if (count3 + count2 > (int) count1)
                {
                  flag2 = false;
                  break;
                }
                SmsHeaderElement element;
                if (!SmsHeaderElement.Parse((IList<byte>) bytes.Skip<byte>(count3).Take<byte>(count2).ToArray<byte>(), out element))
                {
                  flag2 = false;
                  break;
                }
                userData.HeaderElements.Add(element);
              }
              if (flag2)
                userData.Payload.AddRange(bytes.Skip<byte>((int) count1));
            }
          }
        }
        else if (bytes.Count >= 10)
        {
          byte num1 = 0;
          byte num2 = 0;
          for (int index = 0; index < bytes.Count - 2; ++index)
          {
            if (index % 2 == 0)
              num1 ^= bytes[index];
            else
              num2 += bytes[index];
          }
          if ((int) num1 == (int) bytes[bytes.Count - 2] && (int) num2 == (int) bytes[bytes.Count - 1])
          {
            ulong imei = 0;
            for (int index = 0; index < 8; ++index)
              imei = imei * 256UL + (ulong) bytes[index];
            if (imei <= 999999999999999UL)
            {
              userData.HeaderElements.Add((SmsHeaderElement) new ImeiSmsHeaderElement(imei));
              userData.Payload.AddRange(bytes.Skip<byte>(8).Take<byte>(bytes.Count - 10));
            }
          }
          else
            userData.payload.AddRange((IEnumerable<byte>) bytes);
        }
      }
      if (!flag2)
        userData = (SmsUserData) null;
      return flag2;
    }
  }
}
