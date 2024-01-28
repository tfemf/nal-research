// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.XmlNodeExtentions
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.IO.Ports;
using System.Xml;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public static class XmlNodeExtentions
  {
    public static float GetElementAsSingle(
      this XmlNode parentNode,
      string path,
      float defaultValue)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(path);
      if (xmlNode != null)
      {
        try
        {
          return float.Parse(xmlNode.InnerText);
        }
        catch
        {
        }
      }
      return defaultValue;
    }

    public static int GetElementAsInt32(this XmlNode parentNode, string path, int defaultValue)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(path);
      if (xmlNode != null)
      {
        try
        {
          return int.Parse(xmlNode.InnerText);
        }
        catch
        {
        }
      }
      return defaultValue;
    }

    public static bool GetElementAsBoolean(this XmlNode parentNode, string path, bool defaultValue)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(path);
      return xmlNode != null ? xmlNode.InnerText == "1" : defaultValue;
    }

    public static string GetElementAsString(
      this XmlNode parentNode,
      string path,
      string defaultValue)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(path);
      return xmlNode != null ? xmlNode.InnerText : defaultValue;
    }

    public static int GetElementAsInt32(this XmlNode parentNode, string path)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(path);
      if (xmlNode == null)
        throw new GetElementAsException();
      int result;
      if (!int.TryParse(xmlNode.InnerText, out result))
        throw new GetElementAsException();
      return result;
    }

    public static string GetElementAsString(this XmlNode parentNode, string path)
    {
      return (parentNode.SelectSingleNode(path) ?? throw new GetElementAsException()).InnerText;
    }

    public static bool GetElementAsBoolean(this XmlNode parentNode, string path)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(path);
      if (xmlNode == null)
        throw new GetElementAsException();
      return xmlNode.InnerText == "1";
    }

    public static StopBits GetElementAsEnum(this XmlNode parentNode, string path)
    {
      return (parentNode.SelectSingleNode(path) ?? throw new GetElementAsException()).GetAsEnum();
    }

    public static T GetElementAsEnum<T>(this XmlNode parentNode, string path)
    {
      return (parentNode.SelectSingleNode(path) ?? throw new GetElementAsException()).GetAsEnum<T>();
    }

    public static StopBits GetAsEnum(this XmlNode node)
    {
      StopBits asEnum;
      if (Utils.ParseFromDescription(node.InnerText, out asEnum))
        return asEnum;
      throw new GetElementAsException();
    }

    public static T GetAsEnum<T>(this XmlNode node)
    {
      T asEnum;
      if (Utils.ParseFromDescription<T>(node.InnerText, out asEnum))
        return asEnum;
      throw new GetElementAsException();
    }
  }
}
