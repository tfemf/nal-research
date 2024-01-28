// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.Utils
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.ComponentModel;
using System.IO.Ports;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public static class Utils
  {
    public static bool ParseFromDescription(string description, out StopBits value)
    {
      switch (description)
      {
        case "1":
          value = StopBits.One;
          return true;
        case "1.5":
          value = StopBits.OnePointFive;
          return true;
        case "2":
          value = StopBits.Two;
          return true;
        default:
          value = StopBits.None;
          return false;
      }
    }

    public static bool ParseFromDescription<T>(string description, out T value)
    {
      Type enumType = typeof (T);
      foreach (T obj in Enum.GetValues(enumType))
      {
        string name = obj.ToString();
        DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) enumType.GetField(name).GetCustomAttributes(typeof (DescriptionAttribute), false);
        if (customAttributes != null && customAttributes.Length != 0)
        {
          if (customAttributes[0].Description == description)
          {
            value = obj;
            return true;
          }
        }
        else if (name == description)
        {
          value = obj;
          return true;
        }
      }
      value = default (T);
      return false;
    }

    public static string FormatForDisplay(string buffer)
    {
      string str = string.Empty;
      for (int index = 0; index < buffer.Length; ++index)
      {
        char c = buffer[index];
        if (!char.IsWhiteSpace(c) && (c < ' ' || c > '~'))
        {
          int num = (int) c;
          str = str + "[0x" + num.ToString("X").PadLeft(2, '0') + "]";
        }
        else if (c != '\r')
          str += c.ToString();
      }
      return str;
    }

    public static string GetServiceDataDirectory(string dataDirectory)
    {
      return string.IsNullOrEmpty(dataDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\NAL\\Server for Trackers Service" : dataDirectory;
    }

    public static string GetRUPairsFileName(string dataDirectory)
    {
      return Utils.GetServiceDataDirectory(dataDirectory) + "\\RemoteUpdatePairs.xml";
    }
  }
}
