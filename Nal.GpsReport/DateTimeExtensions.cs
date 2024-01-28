// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.DateTimeExtensions
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Globalization;

#nullable disable
namespace Nal.GpsReport
{
  public static class DateTimeExtensions
  {
    public static string ToXElementText(this DateTime dateTime, string secondFractionFormat)
    {
      return dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'ddTHH':'mm':'ss" + secondFractionFormat + "Z", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToXElementText(this DateTime dateTime)
    {
      return dateTime.ToXElementText(string.Empty);
    }
  }
}
