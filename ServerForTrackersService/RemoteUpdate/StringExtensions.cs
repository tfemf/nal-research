// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.StringExtensions
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Text.RegularExpressions;

#nullable disable
namespace Nal.RemoteUpdate
{
  public static class StringExtensions
  {
    public static string Wordify(this string value)
    {
      return new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])").Replace(value.ToString(), " ${x}").Replace("Gps", "GPS").Replace("Gsm", "GSM").Replace("Sbd", "SBD").Replace("Sms", "SMS").Replace("Tcp", "TCP");
    }
  }
}
