// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest3ManDownFlags
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;

#nullable disable
namespace Nal.RemoteUpdate
{
  [Flags]
  public enum UpdateRequest3ManDownFlags
  {
    None = 0,
    LocatorAlert = 1,
    StartEmergencyMode = 2,
    SendMandownMessage = 4,
  }
}
