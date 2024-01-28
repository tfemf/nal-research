// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.SbdDirectIPMessage
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class SbdDirectIPMessage
  {
    private List<InfoElement> infoElements;

    public SbdDirectIPMessage() => this.infoElements = new List<InfoElement>();

    public List<InfoElement> InfoElements => this.infoElements;

    public bool Validate()
    {
      return this.infoElements.All<InfoElement>((Func<InfoElement, bool>) (x => x.Validate()));
    }

    public List<byte> GetBytes()
    {
      if (!this.Validate())
        return (List<byte>) null;
      List<byte> bytes = new List<byte>();
      bytes.Add((byte) 1);
      bytes.Add((byte) 0);
      bytes.Add((byte) 0);
      foreach (InfoElement infoElement in this.InfoElements)
        bytes.AddRange((IEnumerable<byte>) infoElement.GetBytes());
      int num = bytes.Count - 3;
      bytes[1] = (byte) (num / 256);
      bytes[2] = (byte) num;
      return bytes;
    }

    public static bool ReceivedAll(IList<byte> data)
    {
      return data.Count >= 3 && data.Count >= 3 + ((int) data[1] * 256 + (int) data[2]);
    }

    public static bool Parse(IList<byte> data, out SbdDirectIPMessage message)
    {
      message = (SbdDirectIPMessage) null;
      if (SbdDirectIPMessage.ReceivedAll(data) && data[0] == (byte) 1 && data.Count >= 3)
      {
        int num1 = (int) data[1] * 256 + (int) data[2];
        if (data.Count == 3 + num1)
        {
          message = new SbdDirectIPMessage();
          bool flag = true;
          int count;
          for (int index = 3; index < data.Count; index += count)
          {
            flag = false;
            if (index + 3 <= data.Count)
            {
              int num2 = (int) data[index];
              count = 3 + ((int) data[index + 1] * 256 + (int) data[index + 2]);
              InfoElement ie;
              if (index + count <= data.Count && InfoElementParser.Parse((IList<byte>) data.Skip<byte>(index).Take<byte>(count).ToArray<byte>(), out ie))
              {
                message.infoElements.Add(ie);
                flag = true;
              }
              else
                break;
            }
            else
              break;
          }
          if (!flag)
            message = (SbdDirectIPMessage) null;
        }
      }
      return message != null;
    }
  }
}
