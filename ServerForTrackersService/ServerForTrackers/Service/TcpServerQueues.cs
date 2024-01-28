// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.TcpServerQueues
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.ServerForTrackers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class TcpServerQueues
  {
    private static string fileName;
    private Dictionary<string, List<byte[]>> queues;

    public TcpServerQueues()
    {
      TcpServerQueues.fileName = Utils.GetServiceDataDirectory((string) null) + "\\TcpServerQueues.xml";
      this.queues = new Dictionary<string, List<byte[]>>();
    }

    public void Load()
    {
      this.queues.Clear();
      try
      {
        string xml = File.ReadAllText(TcpServerQueues.fileName);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        foreach (XmlNode selectNode1 in xmlDocument.SelectSingleNode("/Queues").SelectNodes("Queue"))
        {
          XmlNode xmlNode = selectNode1.SelectSingleNode("Imei");
          if (xmlNode != null)
          {
            string innerText = xmlNode.InnerText;
            if (innerText != null && innerText.Length == 15 && innerText.All<char>((Func<char, bool>) (x => char.IsDigit(x))))
            {
              foreach (XmlNode selectNode2 in selectNode1.SelectNodes("Message"))
              {
                byte[] message = this.FromHexStr(selectNode2.InnerText);
                if (message != null)
                  this.Queue(innerText, message);
              }
            }
          }
        }
      }
      catch (FileNotFoundException ex)
      {
      }
      catch (DirectoryNotFoundException ex)
      {
      }
    }

    public void Save()
    {
      XDocument xdocument = new XDocument(new object[1]
      {
        (object) new XElement((XName) "Queues", (object) this.queues.Select<KeyValuePair<string, List<byte[]>>, XElement>((Func<KeyValuePair<string, List<byte[]>>, XElement>) (x => new XElement((XName) "Queue", new object[2]
        {
          (object) new XElement((XName) "Imei", (object) x.Key),
          (object) x.Value.Select<byte[], XElement>((Func<byte[], XElement>) (m => new XElement((XName) "Message", (object) this.ToHexStr(m))))
        }))))
      });
      StringBuilder output = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings()
      {
        Indent = true
      }))
        xdocument.WriteTo(writer);
      try
      {
        File.WriteAllText(TcpServerQueues.fileName, output.ToString());
      }
      catch
      {
      }
    }

    public int Count(string imei)
    {
      List<byte[]> numArrayList;
      return this.queues.TryGetValue(imei, out numArrayList) ? numArrayList.Count : 0;
    }

    public byte[] Peek(string imei)
    {
      List<byte[]> numArrayList;
      return this.queues.TryGetValue(imei, out numArrayList) && numArrayList.Count > 0 ? numArrayList[0] : (byte[]) null;
    }

    public void Queue(string imei, byte[] message)
    {
      List<byte[]> numArrayList;
      if (!this.queues.TryGetValue(imei, out numArrayList))
      {
        numArrayList = new List<byte[]>();
        this.queues[imei] = numArrayList;
      }
      numArrayList.Add(message);
    }

    public void Dequeue(string imei)
    {
      List<byte[]> numArrayList;
      if (!this.queues.TryGetValue(imei, out numArrayList))
        return;
      if (numArrayList.Count > 0)
        numArrayList.RemoveAt(0);
      if (numArrayList.Count != 0)
        return;
      this.queues.Remove(imei);
    }

    private byte[] FromHexStr(string hexStr)
    {
      if (!hexStr.All<char>((Func<char, bool>) (x =>
      {
        if (char.IsDigit(x))
          return true;
        return char.ToUpper(x) >= 'A' && char.ToUpper(x) <= 'F';
      })))
        return (byte[]) null;
      byte[] numArray = new byte[hexStr.Length / 2];
      for (int startIndex = 0; startIndex + 1 < hexStr.Length; startIndex += 2)
        numArray[startIndex / 2] = Convert.ToByte(hexStr.Substring(startIndex, 2), 16);
      return numArray;
    }

    private string ToHexStr(byte[] bytes)
    {
      if (bytes == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
      foreach (byte num in bytes)
        stringBuilder.Append(num.ToString("X2"));
      return stringBuilder.ToString();
    }
  }
}
