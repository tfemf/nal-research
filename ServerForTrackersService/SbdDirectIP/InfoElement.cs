// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.InfoElement
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;

#nullable disable
namespace Nal.SbdDirectIP
{
  public abstract class InfoElement
  {
    public abstract byte Id { get; }

    public virtual bool Validate() => true;

    public abstract List<byte> GetBytes();

    public static class Ids
    {
      public const byte MOHeader = 1;
      public const byte MOPayload = 2;
      public const byte MOLocation = 3;
      public const byte MOConfirmation = 5;
      public const byte MTHeader = 65;
      public const byte MTPayload = 66;
      public const byte MTConfirmation = 68;
    }
  }
}
