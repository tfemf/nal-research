// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.Utils
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Nal.EncryptionModule
{
  internal static class Utils
  {
    internal const int UserNameMaxLength = 30;
    internal const int PasswordMinLength = 8;
    internal const int PasswordMaxLength = 16;
    internal const int NumBytesInKey = 32;
    internal const int BlockSize = 16;
    internal const int DesKeySize = 24;
    internal const int PartialKeyLength = 8;
    internal static string userFilePartialAesKey = ",\u008F2d¤E>£,\u008F2d¤E>£,\u008F2d¤E>£";
    internal static Aes userFileAesForPasswordKey = new Aes();
    internal static Aes userFileAesForStaticKey = new Aes("u\u0012\u0081_/\u008Fn,=KQß\u00BEBJÔN\u008FÇa|¢\u008DY^À\u008DY,<U\u0005".Substring(0, 32));
    internal static Tdes userFileTdesForStaticKey = new Tdes("5\u000FS3~3:U", "_\u001E  r\u007Fe*", "._s.CZ-\u0018");
    private static string encryptionModuleDirectory = Path.GetDirectoryName(typeof (CryptoOfficerControl).Assembly.Location);
    private static string cryptoOfficerFileName = "crypto_officer.dat";
    private static string edcFileName = "Encryption_Module_EDC_DLL.dat";
    private static string encryptionModuleFileName = Path.GetFileName(typeof (CryptoOfficerControl).Assembly.Location);
    private static Edc edc = new Edc("\u0011õU\u001Emñ&À¸\u0099?ð\u0006tð¥ÅL4Ê\u008Fç\u0006iµ\u0010#\u008BW\u0095Wþ", "\u001EM\u0005\u001AJDB{", "\u0005\u0004\"!R\u001FC9", "\u0018U=6o{nS");

    internal static string EncryptionModuleDirectory => Utils.encryptionModuleDirectory;

    internal static string CryptoOfficerFileName => Utils.cryptoOfficerFileName;

    internal static string EdcFileName => Utils.edcFileName;

    internal static string EncryptionModuleFileName => Utils.encryptionModuleFileName;

    internal static string RunEncryptionSelfTests(Aes aesForPasswordKey, Tdes tdesForStaticKey)
    {
      string empty = string.Empty;
      if (!aesForPasswordKey.Test())
        empty += "AES Self Test Failed\n";
      if (!tdesForStaticKey.Test())
        empty += "Triple DES Self Test Failed\n";
      return empty;
    }

    internal static string RunSoftwareIntegritySelfTest()
    {
      if (!Utils.edc.loadEdcFile(Utils.EncryptionModuleDirectory + "\\" + Utils.EdcFileName))
        return "Could not find EDC file.";
      if (!Utils.edc.setEdcCal(Utils.EncryptionModuleDirectory + "\\" + Utils.EncryptionModuleFileName))
        return "Could not calculate EDC for module.";
      return !Utils.edc.checkAndCalMatch() ? "Calculated and saved EDC do not match." : string.Empty;
    }

    internal static byte[] HexStringToBytes(string hexString)
    {
      int length = hexString.Length / 2;
      byte[] bytes = new byte[length];
      for (int index = 0; index < length; ++index)
      {
        string empty = string.Empty;
        char ch = hexString[index * 2];
        string str1 = ch.ToString();
        string str2 = empty + str1;
        ch = hexString[index * 2 + 1];
        string str3 = ch.ToString();
        byte num = byte.Parse(str2 + str3, NumberStyles.HexNumber);
        bytes[index] = num;
      }
      return bytes;
    }

    internal static string BytesToHexString(byte[] bytes)
    {
      string empty = string.Empty;
      for (int index = 0; index < bytes.Length; ++index)
        empty += bytes[index].ToString("X2");
      return empty;
    }

    internal static Array ResizeArray(Array oldArray, int newSize)
    {
      if (oldArray.Length == newSize)
        return oldArray;
      Array instance = Array.CreateInstance(oldArray.GetType().GetElementType(), newSize);
      int length = Math.Min(oldArray.Length, newSize);
      Array.Copy(oldArray, instance, length);
      return instance;
    }

    internal static string CalculateChecksum(string s)
    {
      int length = s.Length;
      int index = 0;
      ushort num = 0;
      for (; index < length; ++index)
      {
        char ch = s[index];
        num += (ushort) ch;
      }
      return num.ToString("X");
    }

    internal static bool ValidUserName(string userName)
    {
      int index = 0;
      int length;
      for (length = userName.Length; index < length; ++index)
      {
        switch (userName[index])
        {
          case '"':
          case '*':
          case '/':
          case ':':
          case '<':
          case '>':
          case '?':
          case '\\':
          case '|':
            return false;
          default:
            continue;
        }
      }
      return length != 0 && length <= 30;
    }

    internal static bool ValidPassword(string password)
    {
      return password.Length >= 8 && password.Length <= 16;
    }

    internal static bool ValidModemInfo(string modemInfo)
    {
      foreach (char c in modemInfo)
      {
        if (!char.IsDigit(c))
          return false;
      }
      return true;
    }

    internal static bool ValidKey(string key)
    {
      int num = 0;
      foreach (char c in key)
      {
        num += (int) c;
        if (!char.IsDigit(c) && (c < 'A' || c > 'F'))
          return false;
      }
      return num != 0 && key.Length == 64;
    }

    internal static void AppendTo16Block(ref string data)
    {
      Random random = new Random();
      while (data.Length % 16 != 0)
      {
        random.Next();
        random.Next();
        char ch = (char) (random.Next() % 256);
        data += ch.ToString();
      }
    }

    internal static Erc EncryptFileContents(
      string password,
      string partialAesKey,
      string fileText,
      out byte[] fileBytes,
      Aes aesForPasswordKey,
      Aes aesForStaticKey,
      Tdes tdesForStaticKey)
    {
      if (!Utils.ValidPassword(password))
      {
        fileBytes = (byte[]) null;
        return Erc.InvalidPassword;
      }
      Utils.AppendTo16Block(ref fileText);
      fileBytes = Encoding.GetEncoding(1252).GetBytes(fileText);
      aesForPasswordKey.SetKey(password + partialAesKey.Substring(password.Length - 8));
      fileBytes = tdesForStaticKey.Encrypt(fileBytes);
      fileBytes = aesForStaticKey.Encrypt(fileBytes);
      fileBytes = aesForPasswordKey.Encrypt(fileBytes);
      return Erc.Success;
    }

    internal static Erc DecryptFileContents(
      string password,
      string partialAesKey,
      byte[] fileBytes,
      out string fileText,
      Aes aesForPasswordKey,
      Aes aesForStaticKey,
      Tdes tdesForStaticKey)
    {
      if (!Utils.ValidPassword(password))
      {
        fileText = string.Empty;
        return Erc.InvalidPassword;
      }
      aesForPasswordKey.SetKey(password + partialAesKey.Substring(password.Length - 8));
      fileBytes = aesForPasswordKey.Decrypt(fileBytes);
      fileBytes = aesForStaticKey.Decrypt(fileBytes);
      fileBytes = tdesForStaticKey.Decrypt(fileBytes);
      if (fileBytes == null)
      {
        fileText = string.Empty;
        return Erc.ErrorInDecryption;
      }
      fileText = Encoding.GetEncoding(1252).GetString(fileBytes);
      return Erc.Success;
    }
  }
}
