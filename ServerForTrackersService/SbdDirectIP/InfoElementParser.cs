// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.InfoElementParser
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;

#nullable disable
namespace Nal.SbdDirectIP
{
  public static class InfoElementParser
  {
    public static bool Parse(IList<byte> data, out InfoElement ie)
    {
      ie = (InfoElement) null;
      if (data.Count < 1)
        return false;
      switch (data[0])
      {
        case 1:
          MOHeaderIE ie1;
          if (MOHeaderIE.Parse(data, out ie1))
          {
            ie = (InfoElement) ie1;
            break;
          }
          break;
        case 3:
          MOLocationIE ie2;
          if (MOLocationIE.Parse(data, out ie2))
          {
            ie = (InfoElement) ie2;
            break;
          }
          break;
        case 5:
          MOConfirmationIE ie3;
          if (MOConfirmationIE.Parse(data, out ie3))
          {
            ie = (InfoElement) ie3;
            break;
          }
          break;
        case 65:
          MTHeaderIE ie4;
          if (MTHeaderIE.Parse(data, out ie4))
          {
            ie = (InfoElement) ie4;
            break;
          }
          break;
        case 68:
          MTConfirmationIE ie5;
          if (MTConfirmationIE.Parse(data, out ie5))
          {
            ie = (InfoElement) ie5;
            break;
          }
          break;
        default:
          GenericIE ie6;
          if (GenericIE.Parse(data, out ie6))
          {
            ie = (InfoElement) ie6;
            break;
          }
          break;
      }
      return ie != null;
    }
  }
}
