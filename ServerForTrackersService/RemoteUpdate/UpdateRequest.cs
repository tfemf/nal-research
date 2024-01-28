// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

#nullable disable
namespace Nal.RemoteUpdate
{
  public abstract class UpdateRequest
  {
    public abstract string Password { get; set; }

    public abstract byte[] GetUpdate();

    public abstract void ResetValues();

    public static bool Parse(byte[] data, out UpdateRequest request)
    {
      request = (UpdateRequest) null;
      if (data.Length >= 2 && data[0] == (byte) 117)
      {
        switch (data[1])
        {
          case 0:
            UpdateRequest0 update1;
            if (UpdateRequest0.Parse(data, out update1))
            {
              request = (UpdateRequest) update1;
              break;
            }
            break;
          case 1:
            UpdateRequest1 update2;
            if (UpdateRequest1.Parse(data, out update2))
            {
              request = (UpdateRequest) update2;
              break;
            }
            break;
          case 2:
            UpdateRequest2 update3;
            if (UpdateRequest2.Parse(data, out update3))
            {
              request = (UpdateRequest) update3;
              break;
            }
            break;
          case 3:
            UpdateRequest3 update4;
            if (UpdateRequest3.Parse(data, out update4))
            {
              request = (UpdateRequest) update4;
              break;
            }
            break;
        }
      }
      return request != null;
    }
  }
}
