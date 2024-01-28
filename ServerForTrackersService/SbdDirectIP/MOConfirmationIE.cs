// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.MOConfirmationIE
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class MOConfirmationIE : InfoElement
  {
    public override byte Id => 5;

    public bool Status { get; set; }

    public static bool Parse(IList<byte> data, out MOConfirmationIE ie)
    {
      if (data.Count == 4 && data[0] == (byte) 5 && data[1] == (byte) 0 && data[2] == (byte) 1)
      {
        ie = new MOConfirmationIE();
        ie.Status = data[3] > (byte) 0;
        return true;
      }
      ie = (MOConfirmationIE) null;
      return false;
    }

    public override List<byte> GetBytes()
    {
      return new List<byte>()
      {
        this.Id,
        (byte) 0,
        (byte) 1,
        this.Status ? (byte) 1 : (byte) 0
      };
    }
  }
}
