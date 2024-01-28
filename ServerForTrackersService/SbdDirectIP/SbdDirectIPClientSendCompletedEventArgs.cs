// Decompiled with JetBrains decompiler
// Type: Nal.SbdDirectIP.SbdDirectIPClientSendCompletedEventArgs
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;

#nullable disable
namespace Nal.SbdDirectIP
{
  public class SbdDirectIPClientSendCompletedEventArgs : EventArgs
  {
    private Exception error;
    private int? confirmationStatus;

    public SbdDirectIPClientSendCompletedEventArgs(Exception error, int? confirmationStatus)
    {
      this.error = error;
      this.confirmationStatus = confirmationStatus;
    }

    public Exception Error => this.error;

    public int? ConfirmationStatus => this.confirmationStatus;
  }
}
