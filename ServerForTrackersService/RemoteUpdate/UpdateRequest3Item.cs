// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest3Item
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class UpdateRequest3Item
  {
    private UpdateRequest3Tag tag;
    private int tagTable;
    private byte tagValue;
    private List<byte> data;

    public UpdateRequest3Item(int tagTable, byte tagValue, IList<byte> data)
    {
      if (data.Count > (int) byte.MaxValue)
        throw new ArgumentException("Tag data cannot exceed 255 bytes.");
      this.tag = UpdateRequest3.LookUpTag(tagTable, tagValue);
      this.tagTable = tagTable;
      this.tagValue = tagValue;
      this.data = new List<byte>((IEnumerable<byte>) data);
    }

    public UpdateRequest3Item(UpdateRequest3Tag tag, IList<byte> data)
    {
      if (data.Count > (int) byte.MaxValue)
        throw new ArgumentException("Tag data cannot exceed 255 bytes.");
      this.tag = tag;
      this.tagTable = UpdateRequest3.LookUpTagTable(this.tag);
      this.tagValue = UpdateRequest3.LookUpTagValue(this.tag);
      this.data = new List<byte>((IEnumerable<byte>) data);
    }

    public UpdateRequest3Tag Tag => this.tag;

    public int TagTable => this.tagTable;

    public byte TagValue => this.tagValue;

    public ReadOnlyCollection<byte> Data => new ReadOnlyCollection<byte>((IList<byte>) this.data);
  }
}
