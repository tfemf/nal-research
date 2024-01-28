// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.EnumExtensions
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace Nal.RemoteUpdate
{
  public static class EnumExtensions
  {
    public static string Wordify(this Enum value) => value.ToString().Wordify();

    public static string GetDescription(this Enum value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      List<string> stringList = new List<string>();
      Type type = value.GetType();
      string str = value.ToString();
      string[] separator = new string[1]{ ", " };
      foreach (string name in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        FieldInfo field = type.GetField(name);
        if (field == (FieldInfo) null)
        {
          stringList.Add(name);
        }
        else
        {
          DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) field.GetCustomAttributes(typeof (DescriptionAttribute), false);
          if (customAttributes != null && customAttributes.Length != 0)
            stringList.Add(customAttributes[0].Description);
          else
            stringList.Add(name.Wordify());
        }
      }
      return string.Join(", ", stringList.ToArray());
    }
  }
}
