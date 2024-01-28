// Decompiled with JetBrains decompiler
// Type: Nal.Network.ExceptionEventArgs
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;

#nullable disable
namespace Nal.Network
{
  public class ExceptionEventArgs : EventArgs
  {
    private Exception exception;

    public ExceptionEventArgs(Exception exception) => this.exception = exception;

    public Exception Exception => this.exception;
  }
}
