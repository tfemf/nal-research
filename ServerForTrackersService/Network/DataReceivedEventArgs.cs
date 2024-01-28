// Decompiled with JetBrains decompiler
// Type: Nal.Network.DataReceivedEventArgs
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;

#nullable disable
namespace Nal.Network
{
  public class DataReceivedEventArgs : EventArgs
  {
    private byte[] data;

    public DataReceivedEventArgs(byte[] data, int start, int amount)
    {
      this.data = new byte[amount];
      Array.Copy((Array) data, 0, (Array) this.data, 0, amount);
    }

    public byte[] Data => this.data;
  }
}
