// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateResponse
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Nal.RemoteUpdate
{
  public abstract class UpdateResponse
  {
    public abstract XElement ToXElement();

    public static bool Parse(IList<byte> data, out UpdateResponse response)
    {
      response = (UpdateResponse) null;
      if (data.Count >= 2 && data[0] == (byte) 118)
      {
        switch (data[1])
        {
          case 0:
            UpdateResponse0 response1;
            if (UpdateResponse0.Parse(data, out response1))
            {
              response = (UpdateResponse) response1;
              break;
            }
            break;
          case 1:
            UpdateResponse1 response2;
            if (UpdateResponse1.Parse(data, out response2))
            {
              response = (UpdateResponse) response2;
              break;
            }
            break;
          case 2:
            UpdateResponse2 response3;
            if (UpdateResponse2.Parse(data, out response3))
            {
              response = (UpdateResponse) response3;
              break;
            }
            break;
          case 3:
            UpdateResponse3 response4;
            if (UpdateResponse3.Parse(data, out response4))
            {
              response = (UpdateResponse) response4;
              break;
            }
            break;
        }
      }
      return response != null;
    }
  }
}
