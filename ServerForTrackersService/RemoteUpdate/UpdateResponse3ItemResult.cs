// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateResponse3ItemResult
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.RemoteUpdate
{
  public enum UpdateResponse3ItemResult
  {
    Success,
    [Description("Out of Range")] OutOfRange,
    [Description("Length Error")] LengthError,
    [Description("Invalid Profile Header")] InvalidProfileHeader,
    [Description("Not Supported")] NotSupported,
    [Description("No Change")] NoChange,
    Failed,
    Skipped,
    [Description("Time Check Failed")] TimeCheckFailed,
  }
}
