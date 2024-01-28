// Decompiled with JetBrains decompiler
// Type: Nal.Sms.SmsUtilities
// Assembly: Nal.Sms, Version=1.2.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 575A539B-1F46-4610-96E4-FD89E5BCD099
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.Sms.DLL

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Nal.Sms
{
  public static class SmsUtilities
  {
    public static List<byte> ConvertStrToGsmBytes(string str)
    {
      List<byte> gsmBytes = new List<byte>();
      foreach (int num in str)
      {
        switch (num)
        {
          case 10:
            gsmBytes.Add((byte) 10);
            break;
          case 12:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 10);
            break;
          case 13:
            gsmBytes.Add((byte) 13);
            break;
          case 32:
            gsmBytes.Add((byte) 32);
            break;
          case 33:
            gsmBytes.Add((byte) 33);
            break;
          case 34:
            gsmBytes.Add((byte) 34);
            break;
          case 35:
            gsmBytes.Add((byte) 35);
            break;
          case 36:
            gsmBytes.Add((byte) 2);
            break;
          case 37:
            gsmBytes.Add((byte) 37);
            break;
          case 38:
            gsmBytes.Add((byte) 38);
            break;
          case 39:
            gsmBytes.Add((byte) 39);
            break;
          case 40:
            gsmBytes.Add((byte) 40);
            break;
          case 41:
            gsmBytes.Add((byte) 41);
            break;
          case 42:
            gsmBytes.Add((byte) 42);
            break;
          case 43:
            gsmBytes.Add((byte) 43);
            break;
          case 44:
            gsmBytes.Add((byte) 44);
            break;
          case 45:
            gsmBytes.Add((byte) 45);
            break;
          case 46:
            gsmBytes.Add((byte) 46);
            break;
          case 47:
            gsmBytes.Add((byte) 47);
            break;
          case 48:
            gsmBytes.Add((byte) 48);
            break;
          case 49:
            gsmBytes.Add((byte) 49);
            break;
          case 50:
            gsmBytes.Add((byte) 50);
            break;
          case 51:
            gsmBytes.Add((byte) 51);
            break;
          case 52:
            gsmBytes.Add((byte) 52);
            break;
          case 53:
            gsmBytes.Add((byte) 53);
            break;
          case 54:
            gsmBytes.Add((byte) 54);
            break;
          case 55:
            gsmBytes.Add((byte) 55);
            break;
          case 56:
            gsmBytes.Add((byte) 56);
            break;
          case 57:
            gsmBytes.Add((byte) 57);
            break;
          case 58:
            gsmBytes.Add((byte) 58);
            break;
          case 59:
            gsmBytes.Add((byte) 59);
            break;
          case 60:
            gsmBytes.Add((byte) 60);
            break;
          case 61:
            gsmBytes.Add((byte) 61);
            break;
          case 62:
            gsmBytes.Add((byte) 62);
            break;
          case 63:
            gsmBytes.Add((byte) 63);
            break;
          case 64:
            gsmBytes.Add((byte) 0);
            break;
          case 65:
            gsmBytes.Add((byte) 65);
            break;
          case 66:
            gsmBytes.Add((byte) 66);
            break;
          case 67:
            gsmBytes.Add((byte) 67);
            break;
          case 68:
            gsmBytes.Add((byte) 68);
            break;
          case 69:
            gsmBytes.Add((byte) 69);
            break;
          case 70:
            gsmBytes.Add((byte) 70);
            break;
          case 71:
            gsmBytes.Add((byte) 71);
            break;
          case 72:
            gsmBytes.Add((byte) 72);
            break;
          case 73:
            gsmBytes.Add((byte) 73);
            break;
          case 74:
            gsmBytes.Add((byte) 74);
            break;
          case 75:
            gsmBytes.Add((byte) 75);
            break;
          case 76:
            gsmBytes.Add((byte) 76);
            break;
          case 77:
            gsmBytes.Add((byte) 77);
            break;
          case 78:
            gsmBytes.Add((byte) 78);
            break;
          case 79:
            gsmBytes.Add((byte) 79);
            break;
          case 80:
            gsmBytes.Add((byte) 80);
            break;
          case 81:
            gsmBytes.Add((byte) 81);
            break;
          case 82:
            gsmBytes.Add((byte) 82);
            break;
          case 83:
            gsmBytes.Add((byte) 83);
            break;
          case 84:
            gsmBytes.Add((byte) 84);
            break;
          case 85:
            gsmBytes.Add((byte) 85);
            break;
          case 86:
            gsmBytes.Add((byte) 86);
            break;
          case 87:
            gsmBytes.Add((byte) 87);
            break;
          case 88:
            gsmBytes.Add((byte) 88);
            break;
          case 89:
            gsmBytes.Add((byte) 89);
            break;
          case 90:
            gsmBytes.Add((byte) 90);
            break;
          case 91:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 60);
            break;
          case 92:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 47);
            break;
          case 93:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 62);
            break;
          case 94:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 20);
            break;
          case 95:
            gsmBytes.Add((byte) 17);
            break;
          case 97:
            gsmBytes.Add((byte) 97);
            break;
          case 98:
            gsmBytes.Add((byte) 98);
            break;
          case 99:
            gsmBytes.Add((byte) 99);
            break;
          case 100:
            gsmBytes.Add((byte) 100);
            break;
          case 101:
            gsmBytes.Add((byte) 101);
            break;
          case 102:
            gsmBytes.Add((byte) 102);
            break;
          case 103:
            gsmBytes.Add((byte) 103);
            break;
          case 104:
            gsmBytes.Add((byte) 104);
            break;
          case 105:
            gsmBytes.Add((byte) 105);
            break;
          case 106:
            gsmBytes.Add((byte) 106);
            break;
          case 107:
            gsmBytes.Add((byte) 107);
            break;
          case 108:
            gsmBytes.Add((byte) 108);
            break;
          case 109:
            gsmBytes.Add((byte) 109);
            break;
          case 110:
            gsmBytes.Add((byte) 110);
            break;
          case 111:
            gsmBytes.Add((byte) 111);
            break;
          case 112:
            gsmBytes.Add((byte) 112);
            break;
          case 113:
            gsmBytes.Add((byte) 113);
            break;
          case 114:
            gsmBytes.Add((byte) 114);
            break;
          case 115:
            gsmBytes.Add((byte) 115);
            break;
          case 116:
            gsmBytes.Add((byte) 116);
            break;
          case 117:
            gsmBytes.Add((byte) 117);
            break;
          case 118:
            gsmBytes.Add((byte) 118);
            break;
          case 119:
            gsmBytes.Add((byte) 119);
            break;
          case 120:
            gsmBytes.Add((byte) 120);
            break;
          case 121:
            gsmBytes.Add((byte) 121);
            break;
          case 122:
            gsmBytes.Add((byte) 122);
            break;
          case 123:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 40);
            break;
          case 124:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 64);
            break;
          case 125:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 41);
            break;
          case 126:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 61);
            break;
          case 160:
            gsmBytes.Add((byte) 27);
            break;
          case 161:
            gsmBytes.Add((byte) 64);
            break;
          case 163:
            gsmBytes.Add((byte) 1);
            break;
          case 164:
            gsmBytes.Add((byte) 36);
            break;
          case 165:
            gsmBytes.Add((byte) 3);
            break;
          case 167:
            gsmBytes.Add((byte) 95);
            break;
          case 191:
            gsmBytes.Add((byte) 96);
            break;
          case 196:
            gsmBytes.Add((byte) 91);
            break;
          case 197:
            gsmBytes.Add((byte) 14);
            break;
          case 198:
            gsmBytes.Add((byte) 28);
            break;
          case 199:
            gsmBytes.Add((byte) 9);
            break;
          case 201:
            gsmBytes.Add((byte) 31);
            break;
          case 209:
            gsmBytes.Add((byte) 93);
            break;
          case 214:
            gsmBytes.Add((byte) 92);
            break;
          case 216:
            gsmBytes.Add((byte) 11);
            break;
          case 220:
            gsmBytes.Add((byte) 94);
            break;
          case 223:
            gsmBytes.Add((byte) 30);
            break;
          case 224:
            gsmBytes.Add((byte) 127);
            break;
          case 228:
            gsmBytes.Add((byte) 123);
            break;
          case 229:
            gsmBytes.Add((byte) 15);
            break;
          case 230:
            gsmBytes.Add((byte) 29);
            break;
          case 231:
            gsmBytes.Add((byte) 9);
            break;
          case 232:
            gsmBytes.Add((byte) 4);
            break;
          case 233:
            gsmBytes.Add((byte) 5);
            break;
          case 236:
            gsmBytes.Add((byte) 7);
            break;
          case 241:
            gsmBytes.Add((byte) 125);
            break;
          case 242:
            gsmBytes.Add((byte) 8);
            break;
          case 246:
            gsmBytes.Add((byte) 124);
            break;
          case 248:
            gsmBytes.Add((byte) 12);
            break;
          case 249:
            gsmBytes.Add((byte) 6);
            break;
          case 252:
            gsmBytes.Add((byte) 126);
            break;
          case 913:
            gsmBytes.Add((byte) 65);
            break;
          case 914:
            gsmBytes.Add((byte) 66);
            break;
          case 915:
            gsmBytes.Add((byte) 19);
            break;
          case 916:
            gsmBytes.Add((byte) 16);
            break;
          case 917:
            gsmBytes.Add((byte) 69);
            break;
          case 918:
            gsmBytes.Add((byte) 90);
            break;
          case 919:
            gsmBytes.Add((byte) 72);
            break;
          case 920:
            gsmBytes.Add((byte) 25);
            break;
          case 921:
            gsmBytes.Add((byte) 73);
            break;
          case 922:
            gsmBytes.Add((byte) 75);
            break;
          case 923:
            gsmBytes.Add((byte) 20);
            break;
          case 924:
            gsmBytes.Add((byte) 77);
            break;
          case 925:
            gsmBytes.Add((byte) 78);
            break;
          case 926:
            gsmBytes.Add((byte) 26);
            break;
          case 927:
            gsmBytes.Add((byte) 79);
            break;
          case 928:
            gsmBytes.Add((byte) 22);
            break;
          case 929:
            gsmBytes.Add((byte) 80);
            break;
          case 931:
            gsmBytes.Add((byte) 24);
            break;
          case 932:
            gsmBytes.Add((byte) 84);
            break;
          case 933:
            gsmBytes.Add((byte) 89);
            break;
          case 934:
            gsmBytes.Add((byte) 18);
            break;
          case 935:
            gsmBytes.Add((byte) 88);
            break;
          case 936:
            gsmBytes.Add((byte) 23);
            break;
          case 937:
            gsmBytes.Add((byte) 21);
            break;
          case 8364:
            gsmBytes.Add((byte) 27);
            gsmBytes.Add((byte) 101);
            break;
        }
      }
      return gsmBytes;
    }

    public static string ConvertGsmBytesToStr(IEnumerable<byte> bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (byte num in bytes)
      {
        if (flag)
        {
          switch (num)
          {
            case 10:
              stringBuilder.Append('\f');
              continue;
            case 20:
              stringBuilder.Append('^');
              continue;
            case 40:
              stringBuilder.Append('{');
              continue;
            case 41:
              stringBuilder.Append('}');
              continue;
            case 47:
              stringBuilder.Append('\\');
              continue;
            case 60:
              stringBuilder.Append('[');
              continue;
            case 61:
              stringBuilder.Append('~');
              continue;
            case 62:
              stringBuilder.Append(']');
              continue;
            case 64:
              stringBuilder.Append('|');
              continue;
            case 101:
              stringBuilder.Append('€');
              continue;
            default:
              continue;
          }
        }
        else if (num == (byte) 27)
        {
          flag = true;
        }
        else
        {
          switch (num)
          {
            case 0:
              stringBuilder.Append('@');
              continue;
            case 1:
              stringBuilder.Append('£');
              continue;
            case 2:
              stringBuilder.Append('$');
              continue;
            case 3:
              stringBuilder.Append('¥');
              continue;
            case 4:
              stringBuilder.Append('è');
              continue;
            case 5:
              stringBuilder.Append('é');
              continue;
            case 6:
              stringBuilder.Append('ù');
              continue;
            case 7:
              stringBuilder.Append('ì');
              continue;
            case 8:
              stringBuilder.Append('ò');
              continue;
            case 9:
              stringBuilder.Append('Ç');
              continue;
            case 10:
              stringBuilder.Append('\n');
              continue;
            case 11:
              stringBuilder.Append('Ø');
              continue;
            case 12:
              stringBuilder.Append('ø');
              continue;
            case 13:
              stringBuilder.Append('\r');
              continue;
            case 14:
              stringBuilder.Append('Å');
              continue;
            case 15:
              stringBuilder.Append('å');
              continue;
            case 16:
              stringBuilder.Append('Δ');
              continue;
            case 17:
              stringBuilder.Append('_');
              continue;
            case 18:
              stringBuilder.Append('Φ');
              continue;
            case 19:
              stringBuilder.Append('Γ');
              continue;
            case 20:
              stringBuilder.Append('Λ');
              continue;
            case 21:
              stringBuilder.Append('Ω');
              continue;
            case 22:
              stringBuilder.Append('Π');
              continue;
            case 23:
              stringBuilder.Append('Ψ');
              continue;
            case 24:
              stringBuilder.Append('Σ');
              continue;
            case 25:
              stringBuilder.Append('Θ');
              continue;
            case 26:
              stringBuilder.Append('Ξ');
              continue;
            case 27:
              stringBuilder.Append(' ');
              continue;
            case 28:
              stringBuilder.Append('Æ');
              continue;
            case 29:
              stringBuilder.Append('æ');
              continue;
            case 30:
              stringBuilder.Append('ß');
              continue;
            case 31:
              stringBuilder.Append('É');
              continue;
            case 32:
              stringBuilder.Append(' ');
              continue;
            case 33:
              stringBuilder.Append('!');
              continue;
            case 34:
              stringBuilder.Append('"');
              continue;
            case 35:
              stringBuilder.Append('#');
              continue;
            case 36:
              stringBuilder.Append('¤');
              continue;
            case 37:
              stringBuilder.Append('%');
              continue;
            case 38:
              stringBuilder.Append('&');
              continue;
            case 39:
              stringBuilder.Append('\'');
              continue;
            case 40:
              stringBuilder.Append('(');
              continue;
            case 41:
              stringBuilder.Append(')');
              continue;
            case 42:
              stringBuilder.Append('*');
              continue;
            case 43:
              stringBuilder.Append('+');
              continue;
            case 44:
              stringBuilder.Append(',');
              continue;
            case 45:
              stringBuilder.Append('-');
              continue;
            case 46:
              stringBuilder.Append('.');
              continue;
            case 47:
              stringBuilder.Append('/');
              continue;
            case 48:
              stringBuilder.Append('0');
              continue;
            case 49:
              stringBuilder.Append('1');
              continue;
            case 50:
              stringBuilder.Append('2');
              continue;
            case 51:
              stringBuilder.Append('3');
              continue;
            case 52:
              stringBuilder.Append('4');
              continue;
            case 53:
              stringBuilder.Append('5');
              continue;
            case 54:
              stringBuilder.Append('6');
              continue;
            case 55:
              stringBuilder.Append('7');
              continue;
            case 56:
              stringBuilder.Append('8');
              continue;
            case 57:
              stringBuilder.Append('9');
              continue;
            case 58:
              stringBuilder.Append(':');
              continue;
            case 59:
              stringBuilder.Append(';');
              continue;
            case 60:
              stringBuilder.Append('<');
              continue;
            case 61:
              stringBuilder.Append('=');
              continue;
            case 62:
              stringBuilder.Append('>');
              continue;
            case 63:
              stringBuilder.Append('?');
              continue;
            case 64:
              stringBuilder.Append('¡');
              continue;
            case 65:
              stringBuilder.Append('A');
              continue;
            case 66:
              stringBuilder.Append('B');
              continue;
            case 67:
              stringBuilder.Append('C');
              continue;
            case 68:
              stringBuilder.Append('D');
              continue;
            case 69:
              stringBuilder.Append('E');
              continue;
            case 70:
              stringBuilder.Append('F');
              continue;
            case 71:
              stringBuilder.Append('G');
              continue;
            case 72:
              stringBuilder.Append('H');
              continue;
            case 73:
              stringBuilder.Append('I');
              continue;
            case 74:
              stringBuilder.Append('J');
              continue;
            case 75:
              stringBuilder.Append('K');
              continue;
            case 76:
              stringBuilder.Append('L');
              continue;
            case 77:
              stringBuilder.Append('M');
              continue;
            case 78:
              stringBuilder.Append('N');
              continue;
            case 79:
              stringBuilder.Append('O');
              continue;
            case 80:
              stringBuilder.Append('P');
              continue;
            case 81:
              stringBuilder.Append('Q');
              continue;
            case 82:
              stringBuilder.Append('R');
              continue;
            case 83:
              stringBuilder.Append('S');
              continue;
            case 84:
              stringBuilder.Append('T');
              continue;
            case 85:
              stringBuilder.Append('U');
              continue;
            case 86:
              stringBuilder.Append('V');
              continue;
            case 87:
              stringBuilder.Append('W');
              continue;
            case 88:
              stringBuilder.Append('X');
              continue;
            case 89:
              stringBuilder.Append('Y');
              continue;
            case 90:
              stringBuilder.Append('Z');
              continue;
            case 91:
              stringBuilder.Append('Ä');
              continue;
            case 92:
              stringBuilder.Append('Ö');
              continue;
            case 93:
              stringBuilder.Append('Ñ');
              continue;
            case 94:
              stringBuilder.Append('Ü');
              continue;
            case 95:
              stringBuilder.Append('§');
              continue;
            case 96:
              stringBuilder.Append('¿');
              continue;
            case 97:
              stringBuilder.Append('a');
              continue;
            case 98:
              stringBuilder.Append('b');
              continue;
            case 99:
              stringBuilder.Append('c');
              continue;
            case 100:
              stringBuilder.Append('d');
              continue;
            case 101:
              stringBuilder.Append('e');
              continue;
            case 102:
              stringBuilder.Append('f');
              continue;
            case 103:
              stringBuilder.Append('g');
              continue;
            case 104:
              stringBuilder.Append('h');
              continue;
            case 105:
              stringBuilder.Append('i');
              continue;
            case 106:
              stringBuilder.Append('j');
              continue;
            case 107:
              stringBuilder.Append('k');
              continue;
            case 108:
              stringBuilder.Append('l');
              continue;
            case 109:
              stringBuilder.Append('m');
              continue;
            case 110:
              stringBuilder.Append('n');
              continue;
            case 111:
              stringBuilder.Append('o');
              continue;
            case 112:
              stringBuilder.Append('p');
              continue;
            case 113:
              stringBuilder.Append('q');
              continue;
            case 114:
              stringBuilder.Append('r');
              continue;
            case 115:
              stringBuilder.Append('s');
              continue;
            case 116:
              stringBuilder.Append('t');
              continue;
            case 117:
              stringBuilder.Append('u');
              continue;
            case 118:
              stringBuilder.Append('v');
              continue;
            case 119:
              stringBuilder.Append('w');
              continue;
            case 120:
              stringBuilder.Append('x');
              continue;
            case 121:
              stringBuilder.Append('y');
              continue;
            case 122:
              stringBuilder.Append('z');
              continue;
            case 123:
              stringBuilder.Append('ä');
              continue;
            case 124:
              stringBuilder.Append('ö');
              continue;
            case 125:
              stringBuilder.Append('ñ');
              continue;
            case 126:
              stringBuilder.Append('ü');
              continue;
            case 127:
              stringBuilder.Append('à');
              continue;
            default:
              continue;
          }
        }
      }
      return stringBuilder.ToString();
    }

    public static string ConvertBytesToSmsBase64Str(IEnumerable<byte> bytes)
    {
      string smsBase64Str = string.Empty;
      char ch;
      foreach (byte unpackValue in SmsUtilities.UnpackValues(bytes, 6, false))
      {
        if (unpackValue <= (byte) 25)
        {
          string str1 = smsBase64Str;
          ch = (char) ((uint) unpackValue + 65U);
          string str2 = ch.ToString();
          smsBase64Str = str1 + str2;
        }
        else if (unpackValue >= (byte) 26 && unpackValue <= (byte) 51)
        {
          string str3 = smsBase64Str;
          ch = (char) ((int) unpackValue - 26 + 97);
          string str4 = ch.ToString();
          smsBase64Str = str3 + str4;
        }
        else if (unpackValue >= (byte) 52 && unpackValue <= (byte) 61)
        {
          string str5 = smsBase64Str;
          ch = (char) ((int) unpackValue - 52 + 48);
          string str6 = ch.ToString();
          smsBase64Str = str5 + str6;
        }
        else
        {
          switch (unpackValue)
          {
            case 62:
              smsBase64Str += ";";
              continue;
            case 63:
              smsBase64Str += "#";
              continue;
            default:
              continue;
          }
        }
      }
      return smsBase64Str;
    }

    public static List<byte> ConvertSmsBase64StrToBytes(string str)
    {
      List<byte> bytes = new List<byte>();
      int num = 0;
      for (int index = 0; index < str.Length - 1; ++index)
      {
        byte sixBitValue1 = SmsUtilities.GetSixBitValue(str[index]);
        byte sixBitValue2 = SmsUtilities.GetSixBitValue(str[index + 1]);
        bytes.Add((byte) (((int) sixBitValue1 >> num) + ((int) sixBitValue2 << 6 - num)));
        if (num == 4)
        {
          ++index;
          num = 0;
        }
        else
          num += 2;
      }
      return bytes;
    }

    private static byte GetSixBitValue(char c)
    {
      int sixBitValue = 0;
      if (c >= 'A' && c <= 'Z')
        sixBitValue = (int) c - 65;
      else if (c >= 'a' && c <= 'z')
        sixBitValue = 26 + (int) c - 97;
      else if (c >= '0' && c <= '9')
      {
        sixBitValue = 52 + (int) c - 48;
      }
      else
      {
        switch (c)
        {
          case '#':
            sixBitValue = 63;
            break;
          case ';':
            sixBitValue = 62;
            break;
        }
      }
      return (byte) sixBitValue;
    }

    public static List<byte> ConvertToBcd(string s)
    {
      List<byte> values = new List<byte>();
      foreach (char ch in s)
      {
        if (ch >= '0' && ch <= '9')
          values.Add((byte) ((uint) ch - 48U));
        else if (ch == '*')
          values.Add((byte) 10);
        else
          values.Add((byte) 0);
      }
      if (values.Count % 2 == 1)
        values.Add((byte) 15);
      return SmsUtilities.PackValues(values, 4, false);
    }

    public static string ConvertFromBcd(List<byte> bytes)
    {
      string str = string.Empty;
      if (bytes.Count > 0)
      {
        List<byte> byteList = SmsUtilities.UnpackValues((IEnumerable<byte>) bytes, 4, false);
        if (byteList[byteList.Count - 1] == (byte) 15)
          byteList.RemoveAt(byteList.Count - 1);
        foreach (byte num in byteList)
          str = num > (byte) 9 ? (num != (byte) 10 ? str + "0" : str + "*") : str + ((char) ((uint) num + 48U)).ToString();
      }
      return str;
    }

    public static List<byte> ConvertHexStrToBytes(string hexStr)
    {
      List<byte> bytes = new List<byte>();
      for (int startIndex = 0; startIndex + 1 < hexStr.Length; startIndex += 2)
        bytes.Add(Convert.ToByte(hexStr.Substring(startIndex, 2), 16));
      return bytes;
    }

    public static string ConvertBytesToHexStr(IList<byte> bytes)
    {
      StringBuilder stringBuilder = new StringBuilder(bytes.Count * 2);
      foreach (byte num in (IEnumerable<byte>) bytes)
        stringBuilder.Append(num.ToString("X2"));
      return stringBuilder.ToString();
    }

    public static int GetPackedLength(int valuesLength, int bitsPerValue, bool truncate)
    {
      int num = valuesLength * bitsPerValue;
      return truncate ? num / 8 : num / 8 + (num % 8 > 0 ? 1 : 0);
    }

    public static int GetUnpackedLength(int bytesLength, int bitsPerValue, bool truncate)
    {
      int num = bytesLength * 8;
      return truncate ? num / bitsPerValue : num / bitsPerValue + (num % bitsPerValue > 0 ? 1 : 0);
    }

    public static List<byte> PackValues(List<byte> values, int bitsPerValue, bool truncate)
    {
      List<byte> byteList = new List<byte>();
      int num1 = 0;
      byte num2 = 0;
      int num3;
      switch (bitsPerValue)
      {
        case 1:
          num3 = 1;
          break;
        case 2:
          num3 = 2;
          break;
        case 3:
          num3 = 4;
          break;
        case 4:
          num3 = 8;
          break;
        case 5:
          num3 = 16;
          break;
        case 6:
          num3 = 32;
          break;
        case 7:
          num3 = 64;
          break;
        default:
          num3 = 128;
          break;
      }
      foreach (byte num4 in values)
      {
        for (int index = 1; index <= num3; index *= 2)
        {
          if (((int) num4 & index) == index)
            num2 += (byte) (1 << num1);
          if (++num1 == 8)
          {
            byteList.Add(num2);
            num2 = (byte) 0;
            num1 = 0;
          }
        }
      }
      if (!truncate && num1 != 0)
        byteList.Add(num2);
      return byteList;
    }

    public static List<byte> UnpackValues(IEnumerable<byte> bytes, int bitsPerValue, bool truncate)
    {
      List<byte> byteList = new List<byte>();
      int num1 = 0;
      byte num2 = 0;
      foreach (byte num3 in bytes)
      {
        for (int index = 1; index <= 128; index *= 2)
        {
          if (((int) num3 & index) == index)
            num2 += (byte) (1 << num1);
          if (++num1 == bitsPerValue)
          {
            byteList.Add(num2);
            num2 = (byte) 0;
            num1 = 0;
          }
        }
      }
      if (!truncate && num1 != 0)
        byteList.Add(num2);
      return byteList;
    }

    public static bool IsMTPdu(string pdu)
    {
      if (pdu.Length >= 2)
      {
        pdu = pdu.ToUpper();
        try
        {
          int startIndex = (int) Convert.ToByte(pdu.Substring(0, 2), 16) * 2 + 2;
          if (startIndex + 2 <= pdu.Length)
            return ((int) Convert.ToByte(pdu.Substring(startIndex, 2), 16) & 3) == 0;
        }
        catch (ArgumentException ex)
        {
        }
        catch (FormatException ex)
        {
        }
      }
      return false;
    }

    public static string RemoveUnsendableChars(string latinStr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in latinStr)
      {
        if (SmsUtilities.ConvertStrToGsmBytes(ch.ToString()).Count > 0)
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    public static string RemoveUnsafeChars(string latinStr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char c in latinStr)
      {
        if (!SmsUtilities.IsUnsafe(c))
          stringBuilder.Append(c);
      }
      return stringBuilder.ToString();
    }

    public static string DetectUnsendableChars(string latinStr)
    {
      string empty = string.Empty;
      foreach (char ch in latinStr)
      {
        if (SmsUtilities.ConvertStrToGsmBytes(ch.ToString()).Count == 0 && empty.IndexOf(ch) == -1)
          empty += ch.ToString();
      }
      return empty;
    }

    public static string DetectUnsafeChars(string latinStr)
    {
      string empty = string.Empty;
      foreach (char c in latinStr)
      {
        if (SmsUtilities.IsUnsafe(c) && empty.IndexOf(c) == -1)
          empty += c.ToString();
      }
      return empty;
    }

    public static bool IsUnsafe(char c)
    {
      switch (c)
      {
        case '¡':
        case '£':
        case '¤':
        case '¥':
        case '§':
        case '¿':
        case 'Ä':
        case 'Å':
        case 'Æ':
        case 'Ç':
        case 'É':
        case 'Ñ':
        case 'Ö':
        case 'Ø':
        case 'Ü':
        case 'ß':
        case 'à':
        case 'ä':
        case 'å':
        case 'æ':
        case 'è':
        case 'é':
        case 'ì':
        case 'ñ':
        case 'ò':
        case 'ö':
        case 'ø':
        case 'ù':
        case 'ü':
          return true;
        default:
          return false;
      }
    }
  }
}
