﻿// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest2SamePlaceSkipReportsMode
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;

#nullable disable
namespace Nal.RemoteUpdate
{
  public enum UpdateRequest2SamePlaceSkipReportsMode
  {
    [Description("No Skipping")] NoSkipping = 48, // 0x00000030
    [Description("Skip Specified Cycles")] SkipSpecifiedCycles = 49, // 0x00000031
    [Description("Skip Until Motion")] SkipUntilMotion = 50, // 0x00000032
  }
}
