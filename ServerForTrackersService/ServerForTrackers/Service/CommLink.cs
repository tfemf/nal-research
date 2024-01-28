// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.CommLink
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.EncryptionModule;
using Nal.ServerForTrackers.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class CommLink
  {
    private static bool useEncryption;
    private static EncryptionUser encryptionUser;
    private static string outputFormat;
    private static string reportFormat;
    private string id;
    private string type;
    private string name;
    private string status;
    private string activity;
    private bool appendActivity;
    private string finalWords;

    public CommLink()
    {
      this.appendActivity = true;
      this.type = this.GetType().ToString().Substring(this.GetType().ToString().LastIndexOf('.') + 1);
    }

    public event EventHandler NameChanged;

    public event EventHandler StatusChanged;

    public event EventHandler ActivityOccured;

    public event CommLink.DataReceivedEventHandler DataReceived;

    public event EventHandler RemoveMe;

    public static bool UseEncryption
    {
      get => CommLink.useEncryption;
      set => CommLink.useEncryption = value;
    }

    public static EncryptionUser EncryptionUser
    {
      get => CommLink.encryptionUser;
      set => CommLink.encryptionUser = value;
    }

    public static string OutputFormat
    {
      get => CommLink.outputFormat;
      set => CommLink.outputFormat = value;
    }

    public static string ReportFormat
    {
      get => CommLink.reportFormat;
      set => CommLink.reportFormat = value;
    }

    public string ID
    {
      get => this.id;
      set => this.id = value;
    }

    public string Type => this.type;

    public string Name
    {
      get => this.name;
      protected set
      {
        this.name = value;
        if (this.NameChanged == null)
          return;
        this.NameChanged((object) this, EventArgs.Empty);
      }
    }

    public string Status
    {
      get => this.status;
      protected set
      {
        this.status = value;
        if (this.StatusChanged == null)
          return;
        this.StatusChanged((object) this, EventArgs.Empty);
      }
    }

    public string Activity
    {
      get => this.activity;
      protected set
      {
        this.activity = value;
        if (this.ActivityOccured == null)
          return;
        this.ActivityOccured((object) this, EventArgs.Empty);
      }
    }

    public bool AppendActivity
    {
      get => this.appendActivity;
      protected set => this.appendActivity = value;
    }

    public string FinalWords => this.finalWords;

    protected static byte[] DecryptIfNeeded(
      string remoteModemInfo,
      string remoteModemInfoType,
      byte[] data)
    {
      if (CommLink.UseEncryption && CommLink.EncryptionUser.Decrypt(remoteModemInfo, remoteModemInfoType, data, ref data) != Erc.Success)
        data = (byte[]) null;
      return data;
    }

    protected void TriggerDataReceived(CommLink.DataReceivedEventArgs args)
    {
      if (this.DataReceived == null)
        return;
      this.DataReceived((object) this, args);
    }

    protected void TriggerRemoveMe(string finalWords)
    {
      this.finalWords = finalWords;
      if (this.RemoveMe == null)
        return;
      this.RemoveMe((object) this, EventArgs.Empty);
    }

    public delegate void DataReceivedEventHandler(object sender, CommLink.DataReceivedEventArgs e);

    public class DataReceivedEventArgs : EventArgs
    {
      private DataType? type;
      private ModemInfoTypes senderType;
      private string sender;
      private Protocol protocol;
      private DateTime sessionStart;
      private List<byte> data;

      public DataReceivedEventArgs(
        DataType? type,
        ModemInfoTypes senderType,
        string sender,
        Protocol protocol,
        DateTime sessionStart,
        IList<byte> data)
      {
        this.type = type;
        this.senderType = senderType;
        this.sender = sender;
        this.protocol = protocol;
        this.sessionStart = sessionStart;
        this.data = new List<byte>((IEnumerable<byte>) data);
      }

      public DataType? Type => this.type;

      public ModemInfoTypes SenderType => this.senderType;

      public string Sender => this.sender;

      public Protocol Protocol => this.protocol;

      public DateTime SessionStart => this.sessionStart;

      public List<byte> Data => new List<byte>((IEnumerable<byte>) this.data);
    }
  }
}
