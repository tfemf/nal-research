// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosMessage
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public abstract class PecosMessage
  {
    public const short P3MessageId = 903;
    public const short P4MessageId = 904;
    public const short B3MessageId = 399;
    public const short B5MessageId = 401;
    public const short T3MessageId = 1047;
    public const short L0MessageId = 756;
    private ulong imei;

    public PecosMessage()
    {
      this.imei = 0UL;
      this.RequestAck = false;
    }

    public ulong Imei
    {
      get => this.imei;
      set
      {
        if (value > 999999999999999UL)
          return;
        this.imei = value;
      }
    }

    public abstract short Id { get; }

    public DateTime Time { get; set; }

    public bool RequestAck { get; set; }

    public abstract List<byte> GetPayload();

    public List<byte> GetBytes(byte[] encryptKey)
    {
      if (encryptKey != null && encryptKey.Length != 32)
        throw new ArgumentException("The encryption key is not 32 bytes.");
      long imei = (long) this.Imei;
      double totalMilliseconds = (this.Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
      List<byte> data = new List<byte>();
      PecosMessage.AppendInt16(data, this.RequestAck ? this.Id : (short) ((int) this.Id * -1));
      PecosMessage.AppendInt64(data, (long) totalMilliseconds);
      data.AddRange((IEnumerable<byte>) this.GetPayload());
      if (encryptKey != null)
      {
        imei *= -1L;
        data.Add(PecosMessage.GetXorChecksum((IList<byte>) data, data.Count));
        AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
        cryptoServiceProvider.Mode = CipherMode.ECB;
        cryptoServiceProvider.Padding = PaddingMode.PKCS7;
        ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(encryptKey, new byte[16]);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(data.ToArray(), 0, data.Count);
        cryptoStream.FlushFinalBlock();
        data = ((IEnumerable<byte>) memoryStream.ToArray()).ToList<byte>();
        memoryStream.Close();
        cryptoStream.Close();
        cryptoServiceProvider.Clear();
      }
      List<byte> byteList = new List<byte>();
      PecosMessage.AppendInt64(byteList, imei);
      data.InsertRange(0, (IEnumerable<byte>) byteList);
      data.Add(PecosMessage.GetXorChecksum((IList<byte>) data, data.Count));
      return data;
    }

    public abstract XElement ToXElement();

    public static bool ParseOuter(
      IList<byte> data,
      out bool isEncrypted,
      out ulong imei,
      out byte[] innerData)
    {
      isEncrypted = false;
      imei = 0UL;
      innerData = (byte[]) null;
      if (data.Count < 19 || (int) PecosMessage.GetXorChecksum(data, data.Count - 1) != (int) data[data.Count - 1])
        return false;
      int pos = 0;
      long num = PecosMessage.ExtractInt64((IEnumerable<byte>) data, ref pos);
      if (num < 0L)
      {
        isEncrypted = true;
        num = Math.Abs(num);
      }
      if (num > 999999999999999L)
        return false;
      imei = (ulong) num;
      innerData = data.Skip<byte>(8).Take<byte>(data.Count - 9).ToArray<byte>();
      return true;
    }

    public static bool ParseInner(
      bool isDecrypted,
      ulong imei,
      IList<byte> data,
      out PecosMessage message)
    {
      message = (PecosMessage) null;
      int count = data.Count;
      if (isDecrypted)
      {
        bool flag = false;
        if (count >= 2)
        {
          byte num = data[count - 1];
          if (num <= (byte) 16 && (int) num < count)
          {
            count -= (int) num;
            if ((int) PecosMessage.GetXorChecksum(data, count - 1) == (int) data[count - 1])
            {
              --count;
              flag = true;
            }
          }
        }
        if (!flag)
          return false;
      }
      if (count < 10)
        return false;
      int pos = 0;
      short int16 = PecosMessage.ExtractInt16((IEnumerable<byte>) data, ref pos);
      long int64 = PecosMessage.ExtractInt64((IEnumerable<byte>) data, ref pos);
      if (int64 < 0L || int64 >= 253402300800000L)
        return false;
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double) int64);
      switch (Math.Abs(int16))
      {
        case 399:
          PecosB3Message message1;
          if (PecosB3Message.ParsePayload(data, pos, count, out message1))
          {
            message = (PecosMessage) message1;
            break;
          }
          break;
        case 401:
          PecosB5Message message2;
          if (PecosB5Message.ParsePayload(data, pos, count, out message2))
          {
            message = (PecosMessage) message2;
            break;
          }
          break;
        case 756:
          PecosL0Message message3;
          if (PecosL0Message.ParsePayload(data, pos, count, out message3))
          {
            message = (PecosMessage) message3;
            break;
          }
          break;
        case 903:
          PecosP3Message report1;
          if (PecosP3Message.ParsePayload(data, pos, count, out report1))
          {
            message = (PecosMessage) report1;
            break;
          }
          break;
        case 904:
          PecosP4Message report2;
          if (PecosP4Message.ParsePayload(data, pos, count, out report2))
          {
            message = (PecosMessage) report2;
            break;
          }
          break;
        case 1047:
          PecosT3Message message4;
          if (PecosT3Message.ParsePayload(data, pos, count, out message4))
          {
            message = (PecosMessage) message4;
            break;
          }
          break;
      }
      if (message == null)
        return false;
      message.Imei = imei;
      message.Time = dateTime;
      return true;
    }

    protected static short ExtractInt16(IEnumerable<byte> data, ref int pos)
    {
      IEnumerable<byte> source = data.Skip<byte>(pos).Take<byte>(2);
      pos += 2;
      if (BitConverter.IsLittleEndian)
        source = source.Reverse<byte>();
      return BitConverter.ToInt16(source.ToArray<byte>(), 0);
    }

    protected static int ExtractInt32(IEnumerable<byte> data, ref int pos)
    {
      IEnumerable<byte> source = data.Skip<byte>(pos).Take<byte>(4);
      pos += 4;
      if (BitConverter.IsLittleEndian)
        source = source.Reverse<byte>();
      return BitConverter.ToInt32(source.ToArray<byte>(), 0);
    }

    protected static long ExtractInt64(IEnumerable<byte> data, ref int pos)
    {
      IEnumerable<byte> source = data.Skip<byte>(pos).Take<byte>(8);
      pos += 8;
      if (BitConverter.IsLittleEndian)
        source = source.Reverse<byte>();
      return BitConverter.ToInt64(source.ToArray<byte>(), 0);
    }

    protected static ulong ExtractUInt64(IEnumerable<byte> data, ref int pos)
    {
      IEnumerable<byte> source = data.Skip<byte>(pos).Take<byte>(8);
      pos += 8;
      if (BitConverter.IsLittleEndian)
        source = source.Reverse<byte>();
      return BitConverter.ToUInt64(source.ToArray<byte>(), 0);
    }

    protected static float ExtractSingle(IEnumerable<byte> data, ref int pos)
    {
      IEnumerable<byte> source = data.Skip<byte>(pos).Take<byte>(4);
      pos += 4;
      if (BitConverter.IsLittleEndian)
        source = source.Reverse<byte>();
      return BitConverter.ToSingle(source.ToArray<byte>(), 0);
    }

    protected static double ExtractDouble(IEnumerable<byte> data, ref int pos)
    {
      IEnumerable<byte> source = data.Skip<byte>(pos).Take<byte>(8);
      pos += 8;
      if (BitConverter.IsLittleEndian)
        source = source.Reverse<byte>();
      return BitConverter.ToDouble(source.ToArray<byte>(), 0);
    }

    protected static void AppendInt16(List<byte> data, short value)
    {
      IEnumerable<byte> bytes = (IEnumerable<byte>) BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        bytes = bytes.Reverse<byte>();
      data.AddRange(bytes);
    }

    protected static void AppendInt32(List<byte> data, int value)
    {
      IEnumerable<byte> bytes = (IEnumerable<byte>) BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        bytes = bytes.Reverse<byte>();
      data.AddRange(bytes);
    }

    protected static void AppendInt64(List<byte> data, long value)
    {
      IEnumerable<byte> bytes = (IEnumerable<byte>) BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        bytes = bytes.Reverse<byte>();
      data.AddRange(bytes);
    }

    protected static void AppendUInt64(List<byte> data, ulong value)
    {
      IEnumerable<byte> bytes = (IEnumerable<byte>) BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        bytes = bytes.Reverse<byte>();
      data.AddRange(bytes);
    }

    protected static void AppendSingle(List<byte> data, float value)
    {
      IEnumerable<byte> bytes = (IEnumerable<byte>) BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        bytes = bytes.Reverse<byte>();
      data.AddRange(bytes);
    }

    protected static void AppendDouble(List<byte> data, double value)
    {
      IEnumerable<byte> bytes = (IEnumerable<byte>) BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        bytes = bytes.Reverse<byte>();
      data.AddRange(bytes);
    }

    private static byte GetXorChecksum(IList<byte> data, int length)
    {
      byte xorChecksum = 0;
      for (int index = 0; index < length; ++index)
        xorChecksum ^= data[index];
      return xorChecksum;
    }
  }
}
