// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.MOHeaderIE
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class MOHeaderIE : InfoElement
  {
    public MOHeaderIE() => this.TimeOfSession = DateTime.UtcNow;

    public override byte Id => 1;

    public uint CdrReference { get; set; }

    public string Imei { get; set; }

    public byte SessionStatus { get; set; }

    public ushort Momsn { get; set; }

    public ushort Mtmsn { get; set; }

    public DateTime TimeOfSession { get; set; }

    public static bool Parse(IList<byte> data, out MOHeaderIE ie)
    {
      if (data.Count == 31 && data[0] == (byte) 1 && data[1] == (byte) 0 && data[2] == (byte) 28)
      {
        ie = new MOHeaderIE();
        ie.CdrReference = (uint) ((int) data[3] * 16777216 + (int) data[4] * 65536 + (int) data[5] * 256) + (uint) data[6];
        ie.Imei = Encoding.ASCII.GetString(data.Skip<byte>(7).Take<byte>(15).ToArray<byte>());
        ie.SessionStatus = data[22];
        ie.Momsn = (ushort) ((uint) data[23] * 256U + (uint) data[24]);
        ie.Mtmsn = (ushort) ((uint) data[25] * 256U + (uint) data[26]);
        ie.TimeOfSession = MOHeaderIE.EpochTimeToDateTime((uint) ((int) data[27] * 16777216 + (int) data[28] * 65536 + (int) data[29] * 256) + (uint) data[30]);
        return true;
      }
      ie = (MOHeaderIE) null;
      return false;
    }

    public override bool Validate()
    {
      return this.Imei != null && this.Imei.Length == 15 && this.Imei.All<char>((Func<char, bool>) (x => char.IsDigit(x)));
    }

    public override List<byte> GetBytes()
    {
      if (!this.Validate())
        return (List<byte>) null;
      uint epochTime = MOHeaderIE.DateTimeToEpochTime(this.TimeOfSession);
      List<byte> bytes = new List<byte>();
      bytes.Add(this.Id);
      bytes.Add((byte) 0);
      bytes.Add((byte) 28);
      bytes.Add((byte) (this.CdrReference / 16777216U));
      bytes.Add((byte) (this.CdrReference / 65536U));
      bytes.Add((byte) (this.CdrReference / 256U));
      bytes.Add((byte) this.CdrReference);
      bytes.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.Imei));
      bytes.Add(this.SessionStatus);
      bytes.Add((byte) ((uint) this.Momsn / 256U));
      bytes.Add((byte) this.Momsn);
      bytes.Add((byte) ((uint) this.Mtmsn / 256U));
      bytes.Add((byte) this.Mtmsn);
      bytes.Add((byte) (epochTime / 16777216U));
      bytes.Add((byte) (epochTime / 65536U));
      bytes.Add((byte) (epochTime / 256U));
      bytes.Add((byte) epochTime);
      return bytes;
    }

    private static DateTime EpochTimeToDateTime(uint epochTime)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) epochTime);
    }

    private static uint DateTimeToEpochTime(DateTime dateTime)
    {
      DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      dateTime = dateTime.ToUniversalTime();
      return dateTime < dateTime1 ? 0U : (uint) (dateTime - dateTime1).TotalSeconds;
    }
  }
}
