// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.StatusReport0Item
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class StatusReport0Item
  {
    private StatusReport0Tag tag;
    private int tagTable;
    private byte tagValue;
    private List<byte> data;

    public StatusReport0Item(int tagTable, byte tagValue, IList<byte> data)
    {
      if (data.Count > (int) byte.MaxValue)
        throw new ArgumentException();
      this.tag = StatusReport0.LookUpTag(tagTable, tagValue);
      this.tagTable = tagTable;
      this.tagValue = tagValue;
      this.data = new List<byte>((IEnumerable<byte>) data);
    }

    public StatusReport0Item(StatusReport0Tag tag, IList<byte> data)
    {
      if (data.Count > (int) byte.MaxValue)
        throw new ArgumentException();
      this.tag = tag;
      this.tagTable = StatusReport0.LookUpTagTable(this.tag);
      this.tagValue = StatusReport0.LookUpTagValue(this.tag);
      this.data = new List<byte>((IEnumerable<byte>) data);
    }

    public StatusReport0Tag Tag => this.tag;

    public int TagTable => this.tagTable;

    public byte TagValue => this.tagValue;

    public ReadOnlyCollection<byte> Data => new ReadOnlyCollection<byte>((IList<byte>) this.data);
  }
}
