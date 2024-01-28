// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.CharProcessor
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class CharProcessor
  {
    private List<string> strs;
    private Timer timeoutTimer;
    private int maxBufferSize;
    private string buffer;
    private string log;
    private bool logging;
    private string identifier;

    public CharProcessor()
    {
      this.logging = false;
      this.strs = new List<string>();
      this.timeoutTimer = new Timer();
      this.timeoutTimer.Tick += new EventHandler(this.OnTimeoutTimerTick);
    }

    public event CharProcessor.LookupFinishedEventHandler LookupFinished;

    public bool Looking => this.strs.Count > 0;

    public string LookupID => this.identifier;

    public void LookUp(string strsToLookUp, int timeout, bool enableLogging, string id)
    {
      if (this.Looking)
        throw new Exception("Already Looking");
      this.strs.AddRange((IEnumerable<string>) strsToLookUp.Split('|'));
      if (this.strs.Count == 0)
        return;
      this.maxBufferSize = 0;
      foreach (string str in this.strs)
      {
        if (str.Length > this.maxBufferSize)
          this.maxBufferSize = str.Length;
      }
      this.identifier = id;
      this.log = string.Empty;
      this.logging = enableLogging;
      if (timeout == 0)
        return;
      this.timeoutTimer.Interval = timeout;
      this.timeoutTimer.Start();
    }

    public void ClearBuffer() => this.buffer = string.Empty;

    public void CancelLookUp()
    {
      this.timeoutTimer.Stop();
      this.strs.Clear();
      this.identifier = string.Empty;
    }

    public void HandleReceivedData(string data)
    {
      if (!this.Looking)
        return;
      this.buffer += data;
      int num1;
      do
      {
        num1 = -1;
        int length = this.buffer.Length;
        for (int index = 0; index < this.strs.Count; ++index)
        {
          int num2 = this.buffer.IndexOf(this.strs[index]);
          if (num2 != -1 && num2 < length)
          {
            num1 = index;
            length = num2;
          }
        }
        if (num1 != -1)
        {
          if (this.logging)
            this.log += this.buffer.Substring(0, length);
          this.buffer = this.buffer.Substring(length + this.strs[num1].Length);
          this.TriggerLookupFinished(num1);
        }
        else if (this.buffer.Length > this.maxBufferSize)
        {
          int num3 = this.buffer.Length - this.maxBufferSize;
          if (this.logging)
            this.log += this.buffer.Substring(0, num3);
          this.buffer = this.buffer.Substring(num3);
        }
      }
      while (num1 != -1 && this.Looking && this.buffer.Length > 0);
    }

    private void OnTimeoutTimerTick(object sender, EventArgs e)
    {
      if (this.logging)
        this.log += this.buffer;
      this.TriggerLookupFinished(-1);
    }

    private void TriggerLookupFinished(int result)
    {
      this.timeoutTimer.Stop();
      this.strs.Clear();
      if (this.LookupFinished == null)
        return;
      this.LookupFinished((object) this, new CharProcessor.LookupFinishedEventArgs(result, this.log));
    }

    public delegate void LookupFinishedEventHandler(
      object sender,
      CharProcessor.LookupFinishedEventArgs e);

    public class LookupFinishedEventArgs
    {
      private int result;
      private string logStr;

      public LookupFinishedEventArgs(int result, string logStr)
      {
        this.result = result;
        this.logStr = logStr;
      }

      public int Result => this.result;

      public string LogStr => this.logStr;
    }
  }
}
