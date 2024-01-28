// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.Aes
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
  public class Aes
  {
    private static int DefaultKeySize = 32;
    private static int DefaultIVSize = 16;
    private static int DefaultBlockSize = 16;
    private byte[] key = new byte[Aes.DefaultKeySize];

    public Aes()
    {
    }

    public Aes(string key)
      : this(Encoding.GetEncoding(1252).GetBytes(key))
    {
    }

    public Aes(byte[] key)
    {
      if (this.SetKey(key) == -1)
        throw new Exception("Invalid key length.");
    }

    public int SetKey(string keyString)
    {
      return this.SetKey(Encoding.GetEncoding(1252).GetBytes(keyString));
    }

    public int SetKey(byte[] keyByte)
    {
      if (keyByte.Length != Aes.DefaultKeySize)
        return -1;
      this.key = keyByte;
      return 0;
    }

    public byte[] GetKey() => this.key;

    public byte[] Encrypt(byte[] bytes)
    {
      int num = (Aes.DefaultBlockSize - bytes.Length % Aes.DefaultBlockSize) % Aes.DefaultBlockSize;
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
      AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
      cryptoServiceProvider.Mode = CipherMode.ECB;
      cryptoServiceProvider.Padding = PaddingMode.None;
      ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(this.key, new byte[Aes.DefaultIVSize]);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(buffer, 0, buffer.Length);
      cryptoStream.FlushFinalBlock();
      byte[] array = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      cryptoServiceProvider.Clear();
      return array;
    }

    public byte[] Decrypt(byte[] bytes)
    {
      if (bytes == null || bytes.Length % Aes.DefaultBlockSize != 0)
        return (byte[]) null;
      AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
      cryptoServiceProvider.Mode = CipherMode.ECB;
      cryptoServiceProvider.Padding = PaddingMode.None;
      ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor(this.key, new byte[Aes.DefaultIVSize]);
      MemoryStream memoryStream = new MemoryStream(bytes);
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] buffer = new byte[bytes.Length];
      cryptoStream.Read(buffer, 0, buffer.Length);
      memoryStream.Close();
      cryptoStream.Close();
      cryptoServiceProvider.Clear();
      return buffer;
    }

    internal bool Test()
    {
      byte[] numArray1 = new byte[16]
      {
        (byte) 6,
        (byte) 154,
        (byte) 0,
        (byte) 127,
        (byte) 199,
        (byte) 106,
        (byte) 69,
        (byte) 159,
        (byte) 152,
        (byte) 186,
        (byte) 249,
        (byte) 23,
        (byte) 254,
        (byte) 223,
        (byte) 149,
        (byte) 33
      };
      byte[] keyByte = new byte[32]
      {
        (byte) 8,
        (byte) 9,
        (byte) 10,
        (byte) 11,
        (byte) 13,
        (byte) 14,
        (byte) 15,
        (byte) 16,
        (byte) 18,
        (byte) 19,
        (byte) 20,
        (byte) 21,
        (byte) 23,
        (byte) 24,
        (byte) 25,
        (byte) 26,
        (byte) 28,
        (byte) 29,
        (byte) 30,
        (byte) 31,
        (byte) 33,
        (byte) 34,
        (byte) 35,
        (byte) 36,
        (byte) 38,
        (byte) 39,
        (byte) 40,
        (byte) 41,
        (byte) 43,
        (byte) 44,
        (byte) 45,
        (byte) 46
      };
      byte[] arrayTwo = new byte[16]
      {
        (byte) 8,
        (byte) 14,
        (byte) 149,
        (byte) 23,
        (byte) 235,
        (byte) 22,
        (byte) 119,
        (byte) 113,
        (byte) 154,
        (byte) 207,
        (byte) 114,
        (byte) 128,
        (byte) 134,
        (byte) 4,
        (byte) 10,
        (byte) 227
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

    private bool CompareByteArrays(byte[] arrayOne, byte[] arrayTwo)
    {
      if (arrayOne.Length != arrayTwo.Length)
        return false;
      for (int index = 0; index < arrayOne.Length; ++index)
      {
        if ((int) arrayOne[index] != (int) arrayTwo[index])
          return false;
      }
      return true;
    }
  }
}
