// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.RURequestInfo
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class RURequestInfo
  {
    public DateTime TimeSent { get; set; }

    public ModemInfoTypes RecipientType { get; set; }

    public string Recipient { get; set; }

    public IList<byte> Data { get; set; }
  }
}
