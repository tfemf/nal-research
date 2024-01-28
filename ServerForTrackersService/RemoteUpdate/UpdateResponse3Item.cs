// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateResponse3Item
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

#nullable disable
namespace Nal.RemoteUpdate
{
  public class UpdateResponse3Item
  {
    private UpdateRequest3Tag tag;
    private int tagTable;
    private byte tagValue;
    private UpdateResponse3ItemResult result;

    public UpdateResponse3Item(int tagTable, byte tagValue, UpdateResponse3ItemResult result)
    {
      this.tag = UpdateRequest3.LookUpTag(tagTable, tagValue);
      this.tagTable = tagTable;
      this.tagValue = tagValue;
      this.result = result;
    }

    public UpdateRequest3Tag Tag => this.tag;

    public int TagTable => this.tagTable;

    public byte TagValue => this.tagValue;

    public UpdateResponse3ItemResult Result => this.result;
  }
}
