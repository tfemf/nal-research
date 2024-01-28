// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateResponse2
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
  public class UpdateResponse2 : UpdateResponse
  {
    public UpdateResponseStatus Status { get; set; }

    public UpdateRequest2 Request { get; set; }

    public byte InputPinStates { get; set; }

    public override XElement ToXElement()
    {
      XElement xelement = this.Request.ToXElement();
      xelement.Name = (XName) "request";
      xelement.Element((XName) "password").Remove();
      return new XElement((XName) "updateResponse2", new object[3]
      {
        (object) new XElement((XName) "status", (object) this.Status.GetDescription()),
        (object) new XElement((XName) "inputPinStates", (object) this.InputPinStates),
        (object) xelement
      });
    }

    public static bool Parse(IList<byte> data, out UpdateResponse2 response)
    {
      response = (UpdateResponse2) null;
      if ((data.Count == 76 || data.Count == 80) && data[0] == (byte) 118 && data[1] == (byte) 2)
      {
        byte num1 = 0;
        for (int index = 0; index < 75; ++index)
          num1 ^= data[index];
        if ((int) num1 == (int) data[75])
        {
          try
          {
            List<byte> byteList = new List<byte>();
            byteList.Add((byte) 117);
            byteList.AddRange(data.Skip<byte>(1).Take<byte>(9));
            byteList.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes("12345678"));
            byteList.AddRange(data.Skip<byte>(11).Take<byte>(13));
            byteList.AddRange(data.Skip<byte>(25).Take<byte>(50));
            byte num2 = 0;
            for (int index = 0; index < byteList.Count; ++index)
              num2 ^= byteList[index];
            byteList.Add(num2);
            UpdateRequest2 update;
            if (UpdateRequest2.Parse(byteList.ToArray(), out update))
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
              byte num3 = data[24];
              response = new UpdateResponse2();
              response.Status = updateResponseStatus;
              response.Request = update;
              response.InputPinStates = num3;
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
