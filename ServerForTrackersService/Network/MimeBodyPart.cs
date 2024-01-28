// Decompiled with JetBrains decompiler
// Type: Nal.Network.MimeBodyPart
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Linq;
using System.Net.Mime;
using System.Text;

#nullable disable
namespace Nal.Network
{
  public class MimeBodyPart
  {
    private bool isAttachment;
    private TransferEncoding transferEncoding;
    private string content;

    public MimeBodyPart(bool isAttachment, TransferEncoding transferEncoding, string content)
    {
      this.isAttachment = isAttachment;
      this.transferEncoding = transferEncoding;
      this.content = content;
    }

    public bool IsAttachment => this.isAttachment;

    public TransferEncoding TransferEncoding => this.transferEncoding;

    public string Content => this.content;

    public string GetContentAsString()
    {
      try
      {
        switch (this.TransferEncoding)
        {
          case TransferEncoding.QuotedPrintable:
            return this.DecodeQuotedPrintable(this.content);
          case TransferEncoding.Base64:
            return Encoding.Unicode.GetString(Convert.FromBase64String(this.content));
          default:
            return this.content;
        }
      }
      catch
      {
        return string.Empty;
      }
    }

    public byte[] GetContentAsBytes()
    {
      try
      {
        switch (this.TransferEncoding)
        {
          case TransferEncoding.QuotedPrintable:
            return Encoding.Unicode.GetBytes(this.DecodeQuotedPrintable(this.content));
          case TransferEncoding.Base64:
            return Convert.FromBase64String(this.content);
          default:
            return Encoding.Unicode.GetBytes(this.content);
        }
      }
      catch
      {
        return new byte[0];
      }
    }

    private string DecodeQuotedPrintable(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < input.Length; ++index)
      {
        char ch = input[index];
        if (ch == '=')
        {
          if (index < input.Length - 2)
          {
            string upper = input.Substring(index + 1, 2).ToUpper();
            if (upper.All<char>((Func<char, bool>) (x =>
            {
              if (char.IsDigit(x))
                return true;
              return x >= 'A' && x <= 'F';
            })))
              stringBuilder.Append((char) Convert.ToInt32(upper, 16));
          }
          index += 2;
        }
        else
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }
  }
}
