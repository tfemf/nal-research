// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.EmailAccountInfo
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class EmailAccountInfo : CommLinkInfo
  {
    public EmailAccountInfo() => this.Pop3SizeFilter = 6000;

    public string DisplayName
    {
      get => this.Name;
      set => this.Name = value;
    }

    public string Pop3Server { get; set; }

    public int Pop3Port { get; set; }

    public string Pop3UserName { get; set; }

    public string Pop3Password { get; set; }

    public bool Pop3UseSsl { get; set; }

    public int Pop3SizeFilter { get; set; }

    public bool DeleteMailOnServer { get; set; }

    public bool DeleteAll { get; set; }

    public bool AutoRetrieve { get; set; }

    public int AutoRetrieveFrequency { get; set; }

    public string SmtpServer { get; set; }

    public int SmtpPort { get; set; }

    public string SmtpUserName { get; set; }

    public string SmtpPassword { get; set; }

    public string FromAddress { get; set; }

    public bool SmtpRequiresAuthentication { get; set; }

    public bool SmtpUsePop3Credentials { get; set; }
  }
}
