// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateResponse0
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class UpdateResponse0 : UpdateResponse
  {
    public UpdateResponseStatus Status { get; set; }

    public UpdateRequest0 Request { get; set; }

    public override XElement ToXElement()
    {
      XElement xelement = this.Request.ToXElement();
      xelement.Name = (XName) "request";
      xelement.Element((XName) "password").Remove();
      return new XElement((XName) "updateResponse0", new object[2]
      {
        (object) new XElement((XName) "status", (object) this.Status.GetDescription()),
        (object) xelement
      });
    }

    public static bool Parse(IList<byte> data, out UpdateResponse0 response)
    {
      response = (UpdateResponse0) null;
      if ((data.Count == 17 || data.Count == 32) && data[0] == (byte) 118 && data[1] == (byte) 0)
      {
        byte num1 = 0;
        for (int index = 0; index < 16; ++index)
          num1 ^= data[index];
        if ((int) num1 == (int) data[16])
        {
          try
          {
            List<byte> byteList = new List<byte>();
            byteList.Add((byte) 117);
            byteList.AddRange(data.Skip<byte>(1).Take<byte>(9));
            byteList.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes("12345678"));
            byteList.AddRange(data.Skip<byte>(11).Take<byte>(5));
            byte num2 = 0;
            for (int index = 0; index < byteList.Count; ++index)
              num2 ^= byteList[index];
            byteList.Add(num2);
            UpdateRequest0 update;
            if (UpdateRequest0.Parse(byteList.ToArray(), out update))
            {
              UpdateResponseStatus updateResponseStatus;
              switch (data[10])
              {
                case 0:
                  updateResponseStatus = UpdateResponseStatus.Success;
                  break;
                case 1:
                  updateResponseStatus = UpdateResponseStatus.InvalidPassword;
                  break;
                case 2:
                  updateResponseStatus = UpdateResponseStatus.OldDateTimeStamp;
                  break;
                default:
                  throw new ArgumentException();
              }
              response = new UpdateResponse0();
              response.Status = updateResponseStatus;
              response.Request = update;
            }
          }
          catch (ArgumentOutOfRangeException ex)
          {
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      return response != null;
    }
  }
}
