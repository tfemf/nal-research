// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.Tdes
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Nal.EncryptionModule
{
  internal class Tdes
  {
    private static int DefaultKeySize = 24;
    private static int DefaultIVSize = 8;
    private byte[] key = new byte[Tdes.DefaultKeySize];

    public Tdes()
    {
    }

    public Tdes(string key1, string key2, string key3)
    {
      if (this.SetKey(Encoding.GetEncoding(1252).GetBytes(key1 + key2 + key3)) == -1)
        throw new Exception("Invalid key length.");
    }

    public int SetKeys(string key1, string key2, string key3)
    {
      string s = key1 + key2 + key3;
      return this.SetKey(Encoding.GetEncoding(1252).GetBytes(s));
    }

    public byte[] GetKey() => this.key;

    public bool Test()
    {
      byte[] numArray1 = new byte[48]
      {
        (byte) 84,
        (byte) 104,
        (byte) 101,
        (byte) 32,
        (byte) 113,
        (byte) 117,
        (byte) 105,
        (byte) 99,
        (byte) 107,
        (byte) 32,
        (byte) 98,
        (byte) 114,
        (byte) 111,
        (byte) 119,
        (byte) 110,
        (byte) 32,
        (byte) 102,
        (byte) 111,
        (byte) 120,
        (byte) 32,
        (byte) 106,
        (byte) 117,
        (byte) 109,
        (byte) 112,
        (byte) 101,
        (byte) 100,
        (byte) 32,
        (byte) 111,
        (byte) 118,
        (byte) 101,
        (byte) 114,
        (byte) 32,
        (byte) 116,
        (byte) 104,
        (byte) 101,
        (byte) 32,
        (byte) 108,
        (byte) 97,
        (byte) 122,
        (byte) 121,
        (byte) 32,
        (byte) 100,
        (byte) 111,
        (byte) 103,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      byte[] keyByte = new byte[24]
      {
        (byte) 49,
        (byte) 50,
        (byte) 51,
        (byte) 52,
        (byte) 53,
        (byte) 54,
        (byte) 55,
        (byte) 56,
        (byte) 57,
        (byte) 48,
        (byte) 49,
        (byte) 50,
        (byte) 51,
        (byte) 52,
        (byte) 53,
        (byte) 54,
        (byte) 65,
        (byte) 66,
        (byte) 67,
        (byte) 68,
        (byte) 69,
        (byte) 70,
        (byte) 71,
        (byte) 72
      };
      byte[] arrayTwo = new byte[48]
      {
        (byte) 19,
        (byte) 212,
        (byte) 211,
        (byte) 84,
        (byte) 148,
        (byte) 147,
        (byte) 210,
        (byte) 135,
        (byte) 15,
        (byte) 147,
        (byte) 195,
        (byte) 224,
        (byte) 129,
        (byte) 42,
        (byte) 6,
        (byte) 222,
        (byte) 70,
        (byte) 126,
        (byte) 31,
        (byte) 156,
        (byte) 11,
        (byte) 251,
        (byte) 22,
        (byte) 192,
        (byte) 112,
        (byte) 237,
        (byte) 229,
        (byte) 202,
        (byte) 187,
        (byte) 211,
        (byte) 202,
        (byte) 98,
        (byte) 242,
        (byte) 23,
        (byte) 167,
        (byte) 174,
        (byte) 141,
        (byte) 71,
        (byte) 242,
        (byte) 199,
        (byte) 191,
        (byte) 98,
        (byte) 235,
        (byte) 48,
        (byte) 147,
        (byte) 35,
        (byte) 181,
        (byte) 139
      };
      bool flag = false;
      byte[] key = this.key;
      try
      {
        this.SetKey(keyByte);
        byte[] numArray2 = this.Encrypt(numArray1);
        if (this.CompareByteArrays(numArray2, arrayTwo))
        {
          byte[] arrayOne = this.Decrypt(numArray2);
          if (arrayOne != null)
          {
            if (this.CompareByteArrays(arrayOne, numArray1))
              flag = true;
          }
        }
      }
      finally
      {
        this.key = key;
      }
      return flag;
    }

    public byte[] Encrypt(byte[] bytes)
    {
      int num = (8 - bytes.Length % 8) % 8;
      byte[] buffer;
      if (num > 0)
      {
        buffer = new byte[bytes.Length + num];
        bytes.CopyTo((Array) buffer, 0);
        for (int index = 0; index < num; ++index)
          buffer[bytes.Length + index] = (byte) 32;
      }
      else
        buffer = bytes;
      TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
      cryptoServiceProvider.Mode = CipherMode.ECB;
      cryptoServiceProvider.Padding = PaddingMode.None;
      ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(this.key, new byte[Tdes.DefaultIVSize]);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(buffer, 0, buffer.Length);
      cryptoStream.FlushFinalBlock();
      byte[] array = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      return array;
    }

    public byte[] Decrypt(byte[] bytes)
    {
      if (bytes == null || bytes.Length % 8 != 0)
        return (byte[]) null;
      TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
      cryptoServiceProvider.Mode = CipherMode.ECB;
      cryptoServiceProvider.Padding = PaddingMode.None;
      ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor(this.key, new byte[Tdes.DefaultIVSize]);
      MemoryStream memoryStream = new MemoryStream(bytes);
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] buffer = new byte[bytes.Length];
      cryptoStream.Read(buffer, 0, buffer.Length);
      memoryStream.Close();
      cryptoStream.Close();
      return buffer;
    }

    private int SetKey(byte[] keyByte)
    {
      if (keyByte.Length != Tdes.DefaultKeySize)
        return -1;
      this.key = keyByte;
      return 0;
    }

    private bool CompareByteArrays(byte[] arrayOne, byte[] arrayTwo)
    {
      bool flag = false;
      if (arrayOne.Length == arrayTwo.Length)
      {
        int index = 0;
        while (index < arrayOne.Length && (int) arrayOne[index] == (int) arrayTwo[index])
          ++index;
        if (index == arrayOne.Length)
          flag = true;
      }
      return flag;
    }
  }
}
