// Decompiled with JetBrains decompiler
// Type: Nal.DataCallPackets.TimerWrapper
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.ComponentModel;
using System.Timers;

#nullable disable
namespace Nal.DataCallPackets
{
  public class TimerWrapper
  {
    private Timer timer;
    private DateTime startedAt;
    private bool started;

    public TimerWrapper(ISynchronizeInvoke synchronizationObject)
    {
      this.started = false;
      this.timer = new Timer();
      this.timer.AutoReset = false;
      this.timer.SynchronizingObject = synchronizationObject;
      this.timer.Elapsed += new ElapsedEventHandler(this.OnTimerElapsed);
    }

    public event ElapsedEventHandler Elapsed;

    public double Interval
    {
      get => this.timer.Interval;
      set => this.timer.Interval = value;
    }

    public bool Enabled => this.started;

    public void Stop()
    {
      this.started = false;
      this.timer.Stop();
    }

    public void Start()
    {
      this.started = true;
      this.startedAt = DateTime.Now;
      this.timer.Start();
    }

    public void Restart()
    {
      if (!this.Enabled)
        return;
      this.Stop();
      this.Start();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
      if (!this.started || !(e.SignalTime > this.startedAt))
        return;
      if (this.Elapsed != null)
        this.Elapsed((object) this, e);
      if (!this.Enabled)
        return;
      this.timer.Start();
    }
  }
}
