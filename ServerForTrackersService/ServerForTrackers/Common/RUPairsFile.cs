// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.RUPairsFile
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public static class RUPairsFile
  {
    public static List<RUPair> Load(string fileName)
    {
      string xml = File.ReadAllText(fileName);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      xmlDocument.GetElementAsInt32("/RemoteUpdates/Version", 1);
      return RUPairsFile.Version1Load(xmlDocument);
    }

    public static void Save(string fileName, List<RUPair> ruPairs)
    {
      XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings()
      {
        Indent = true
      });
      xmlWriter.WriteStartElement("RemoteUpdatePairs");
      xmlWriter.WriteStartElement("Version");
      xmlWriter.WriteString("1");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("Pairs");
      foreach (RUPair ruPair in ruPairs)
      {
        xmlWriter.WriteStartElement("Pair");
        if (ruPair.RequestInfo != null)
        {
          xmlWriter.WriteStartElement("Request");
          xmlWriter.WriteAttributeString("To", (ruPair.RequestInfo.RecipientType == ModemInfoTypes.Identifier ? "D" : (ruPair.RequestInfo.RecipientType == ModemInfoTypes.Imei ? "I" : (ruPair.RequestInfo.RecipientType == ModemInfoTypes.PhoneNumber ? "P" : "U"))) + ":" + ruPair.RequestInfo.Recipient);
          xmlWriter.WriteAttributeString("Sent", ruPair.RequestInfo.TimeSent.ToString("yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture));
          xmlWriter.WriteString(Convert.ToBase64String(ruPair.RequestInfo.Data.ToArray<byte>()));
          xmlWriter.WriteEndElement();
        }
        if (ruPair.ResponseInfo != null)
        {
          xmlWriter.WriteStartElement("Response");
          xmlWriter.WriteAttributeString("From", (ruPair.ResponseInfo.SenderType == ModemInfoTypes.Identifier ? "D" : (ruPair.ResponseInfo.SenderType == ModemInfoTypes.Imei ? "I" : (ruPair.ResponseInfo.SenderType == ModemInfoTypes.PhoneNumber ? "P" : "U"))) + ":" + ruPair.ResponseInfo.Sender);
          xmlWriter.WriteAttributeString("Received", ruPair.ResponseInfo.TimeReceived.ToString("yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture));
          xmlWriter.WriteString(Convert.ToBase64String(ruPair.ResponseInfo.Data.ToArray<byte>()));
          xmlWriter.WriteEndElement();
        }
        xmlWriter.WriteEndElement();
      }
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
    }

    private static List<RUPair> Version1Load(XmlDocument xmlDoc)
    {
      List<RUPair> ruPairList = new List<RUPair>();
      foreach (XmlNode selectNode in xmlDoc.SelectNodes("/RemoteUpdatePairs/Pairs/Pair"))
      {
        RUPair ruPair = new RUPair();
        XmlNode parentNode1 = selectNode.SelectSingleNode("Request");
        if (parentNode1 != null)
        {
          RURequestInfo ruRequestInfo = new RURequestInfo();
          string elementAsString = parentNode1.GetElementAsString("@Sent", string.Empty);
          ruRequestInfo.TimeSent = DateTime.ParseExact(elementAsString, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture);
          ruRequestInfo.TimeSent = DateTime.SpecifyKind(ruRequestInfo.TimeSent, DateTimeKind.Utc);
          string[] strArray = parentNode1.GetElementAsString("@To", string.Empty).Split(':');
          switch (strArray[0])
          {
            case "D":
              ruRequestInfo.RecipientType = ModemInfoTypes.Identifier;
              break;
            case "I":
              ruRequestInfo.RecipientType = ModemInfoTypes.Imei;
              break;
            case "P":
              ruRequestInfo.RecipientType = ModemInfoTypes.PhoneNumber;
              break;
            default:
              ruRequestInfo.RecipientType = ModemInfoTypes.Unknown;
              break;
          }
          ruRequestInfo.Recipient = strArray[1];
          ruRequestInfo.Data = (IList<byte>) new List<byte>((IEnumerable<byte>) Convert.FromBase64String(parentNode1.InnerText));
          ruPair.RequestInfo = ruRequestInfo;
        }
        XmlNode parentNode2 = selectNode.SelectSingleNode("Response");
        if (parentNode2 != null)
        {
          RUResponseInfo ruResponseInfo = new RUResponseInfo();
          string elementAsString = parentNode2.GetElementAsString("@Received", string.Empty);
          ruResponseInfo.TimeReceived = DateTime.ParseExact(elementAsString, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture);
          ruResponseInfo.TimeReceived = DateTime.SpecifyKind(ruResponseInfo.TimeReceived, DateTimeKind.Utc);
          string[] strArray = parentNode2.GetElementAsString("@From", string.Empty).Split(':');
          switch (strArray[0])
          {
            case "D":
              ruResponseInfo.SenderType = ModemInfoTypes.Identifier;
              break;
            case "I":
              ruResponseInfo.SenderType = ModemInfoTypes.Imei;
              break;
            case "P":
              ruResponseInfo.SenderType = ModemInfoTypes.PhoneNumber;
              break;
            default:
              ruResponseInfo.SenderType = ModemInfoTypes.Unknown;
              break;
          }
          ruResponseInfo.Sender = strArray[1];
          ruResponseInfo.Data = (IList<byte>) new List<byte>((IEnumerable<byte>) Convert.FromBase64String(parentNode2.InnerText));
          ruPair.ResponseInfo = ruResponseInfo;
        }
        ruPairList.Add(ruPair);
      }
      return ruPairList;
    }
  }
}
