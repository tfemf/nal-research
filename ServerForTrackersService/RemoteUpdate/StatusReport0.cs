// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.StatusReport0
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class StatusReport0
  {
    private List<StatusReport0Item> items;

    public StatusReport0() => this.items = new List<StatusReport0Item>();

    public List<StatusReport0Item> Items => this.items;

    public static StatusReport0Tag LookUpTag(int table, byte tag)
    {
      if (table != 0)
        return StatusReport0Tag.Unknown;
      switch (tag)
      {
        case 0:
          return StatusReport0Tag.OverallStatisticsTimestamps;
        case 1:
          return StatusReport0Tag.OverallIridiumSignalStatistics;
        case 2:
          return StatusReport0Tag.OverallIridiumGpsStatistics;
        case 3:
          return StatusReport0Tag.OverallGsmSignalStatistics;
        case 4:
          return StatusReport0Tag.OverallGsmGpsStatistics;
        case 5:
          return StatusReport0Tag.TrackingStatisticsTimestamps;
        case 6:
          return StatusReport0Tag.TrackingIridiumSignalStatistics;
        case 7:
          return StatusReport0Tag.TrackingIridiumGpsStatistics;
        case 8:
          return StatusReport0Tag.TrackingGsmSignalStatistics;
        case 9:
          return StatusReport0Tag.TrackingGsmGpsStatistics;
        case 10:
          return StatusReport0Tag.EmergencyActivationSource;
        case 11:
          return StatusReport0Tag.IridiumLastReportTimes;
        case 12:
          return StatusReport0Tag.GsmLastReportTimes;
        case 13:
          return StatusReport0Tag.AntennaStatus;
        default:
          return StatusReport0Tag.Unknown;
      }
    }

    public static int LookUpTagTable(StatusReport0Tag tag)
    {
      return tag == StatusReport0Tag.Unknown ? int.MaxValue : 0;
    }

    public static byte LookUpTagValue(StatusReport0Tag tag)
    {
      switch (tag)
      {
        case StatusReport0Tag.Unknown:
          return byte.MaxValue;
        case StatusReport0Tag.AntennaStatus:
          return 13;
        case StatusReport0Tag.EmergencyActivationSource:
          return 10;
        case StatusReport0Tag.GsmLastReportTimes:
          return 12;
        case StatusReport0Tag.IridiumLastReportTimes:
          return 11;
        case StatusReport0Tag.OverallGsmGpsStatistics:
          return 4;
        case StatusReport0Tag.OverallGsmSignalStatistics:
          return 3;
        case StatusReport0Tag.OverallIridiumGpsStatistics:
          return 2;
        case StatusReport0Tag.OverallIridiumSignalStatistics:
          return 1;
        case StatusReport0Tag.OverallStatisticsTimestamps:
          return 0;
        case StatusReport0Tag.TrackingGsmGpsStatistics:
          return 9;
        case StatusReport0Tag.TrackingGsmSignalStatistics:
          return 8;
        case StatusReport0Tag.TrackingIridiumGpsStatistics:
          return 7;
        case StatusReport0Tag.TrackingIridiumSignalStatistics:
          return 6;
        case StatusReport0Tag.TrackingStatisticsTimestamps:
          return 5;
        default:
          return 0;
      }
    }

    public XElement ToXElement()
    {
      XElement element = new XElement((XName) "statusReport0");
      foreach (StatusReport0Item statusReport0Item in this.items)
      {
        try
        {
          switch (statusReport0Item.Tag)
          {
            case StatusReport0Tag.AntennaStatus:
              byte status;
              StatusReport0.ParseAntennaStatusData((IList<byte>) statusReport0Item.Data, out status);
              element.Add((object) new XElement((XName) "antennaStatus", (object) status));
              continue;
            case StatusReport0Tag.EmergencyActivationSource:
              StatusReport0EmergencyActivationSource source;
              StatusReport0.ParseEmergencyActivationSourceData((IList<byte>) statusReport0Item.Data, out source);
              XElement xelement = element;
              XName name = (XName) "emergencyActivationSource";
              string content1;
              switch (source)
              {
                case StatusReport0EmergencyActivationSource.LatchingPin:
                  content1 = "Latching Pin";
                  break;
                case StatusReport0EmergencyActivationSource.MomentaryPin:
                  content1 = "Momentary Pin";
                  break;
                case StatusReport0EmergencyActivationSource.RemoteUpdate:
                  content1 = "Remote Update";
                  break;
                case StatusReport0EmergencyActivationSource.BaseStation:
                  content1 = "Base Station";
                  break;
                default:
                  content1 = "None";
                  break;
              }
              XElement content2 = new XElement(name, (object) content1);
              xelement.Add((object) content2);
              continue;
            case StatusReport0Tag.GsmLastReportTimes:
              StatusReport0.AddTimestampsItemToXml(element, (IList<byte>) statusReport0Item.Data, "gsmLastReportTimes", "attempted", "successful");
              continue;
            case StatusReport0Tag.IridiumLastReportTimes:
              StatusReport0.AddTimestampsItemToXml(element, (IList<byte>) statusReport0Item.Data, "iridiumLastReportTimes", "attempted", "successful");
              continue;
            case StatusReport0Tag.OverallGsmGpsStatistics:
              StatusReport0.AddGpsStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "overallGsmGpsStatistics");
              continue;
            case StatusReport0Tag.OverallGsmSignalStatistics:
              StatusReport0.AddSignalStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "overallGsmSignalStatistics");
              continue;
            case StatusReport0Tag.OverallIridiumGpsStatistics:
              StatusReport0.AddGpsStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "overallIridiumGpsStatistics");
              continue;
            case StatusReport0Tag.OverallIridiumSignalStatistics:
              StatusReport0.AddSignalStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "overallIridiumSignalStatistics");
              continue;
            case StatusReport0Tag.OverallStatisticsTimestamps:
              StatusReport0.AddTimestampsItemToXml(element, (IList<byte>) statusReport0Item.Data, "overallStatisticsTimestamps", "firstValid", "lastValid");
              continue;
            case StatusReport0Tag.TrackingGsmGpsStatistics:
              StatusReport0.AddGpsStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "trackingGsmGpsStatistics");
              continue;
            case StatusReport0Tag.TrackingGsmSignalStatistics:
              StatusReport0.AddSignalStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "trackingGsmSignalStatistics");
              continue;
            case StatusReport0Tag.TrackingIridiumGpsStatistics:
              StatusReport0.AddGpsStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "trackingIridiumGpsStatistics");
              continue;
            case StatusReport0Tag.TrackingIridiumSignalStatistics:
              StatusReport0.AddSignalStatisticsItemToXml(element, (IList<byte>) statusReport0Item.Data, "trackingIridiumSignalStatistics");
              continue;
            case StatusReport0Tag.TrackingStatisticsTimestamps:
              StatusReport0.AddTimestampsItemToXml(element, (IList<byte>) statusReport0Item.Data, "trackingStatisticsTimestamps", "firstValid", "lastValid");
              continue;
            default:
              element.Add((object) new XElement((XName) "unknown", new object[3]
              {
                (object) new XAttribute((XName) "tagTable", (object) statusReport0Item.TagTable),
                (object) new XAttribute((XName) "tagValue", (object) statusReport0Item.TagValue),
                (object) string.Concat(statusReport0Item.Data.Select<byte, string>((Func<byte, string>) (x => x.ToString("X2"))).ToArray<string>())
              }));
              continue;
          }
        }
        catch (ArgumentException ex)
        {
        }
      }
      return element;
    }

    public byte[] GetData()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 115);
      byteList.Add((byte) 0);
      foreach (StatusReport0Item statusReport0Item in this.items)
      {
        for (int index = 0; index < statusReport0Item.TagTable; ++index)
          byteList.Add(byte.MaxValue);
        byteList.Add(statusReport0Item.TagValue);
        byteList.Add((byte) statusReport0Item.Data.Count);
        byteList.AddRange((IEnumerable<byte>) statusReport0Item.Data);
      }
      byte num1 = 0;
      byte num2 = 0;
      foreach (byte num3 in byteList)
      {
        num1 ^= num3;
        num2 += num3;
      }
      byteList.Add(num1);
      byteList.Add(num2);
      return byteList.ToArray();
    }

    public static bool Parse(IList<byte> data, out StatusReport0 update)
    {
      update = (StatusReport0) null;
      if (data.Count >= 4 && data[0] == (byte) 115 && data[1] == (byte) 0)
      {
        byte num1 = 0;
        byte num2 = 0;
        bool flag = false;
        int num3 = 0;
        int index1 = 0;
        while (index1 < data.Count - 2)
        {
          num1 ^= data[index1];
          num2 += data[index1];
          ++index1;
          if (index1 >= 2 && (int) num1 == (int) data[index1] && (int) num2 == (int) data[index1 + 1])
          {
            num3 = index1;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          update = new StatusReport0();
          int count1;
          byte count2;
          for (int index2 = 2; index2 < num3; index2 = count1 + (int) count2)
          {
            int tagTable = 0;
            for (; index2 < num3 && data[index2] == byte.MaxValue; ++index2)
              ++tagTable;
            if (index2 + 2 <= num3)
            {
              IList<byte> byteList1 = data;
              int index3 = index2;
              int num4 = index3 + 1;
              byte tagValue = byteList1[index3];
              IList<byte> byteList2 = data;
              int index4 = num4;
              count1 = index4 + 1;
              count2 = byteList2[index4];
              if (count1 + (int) count2 <= num3)
                update.Items.Add(new StatusReport0Item(tagTable, tagValue, (IList<byte>) data.Skip<byte>(count1).Take<byte>((int) count2).ToList<byte>()));
              else
                break;
            }
            else
              break;
          }
        }
      }
      return update != null;
    }

    public static void ParseAntennaStatusData(IList<byte> data, out byte status)
    {
      status = data.Count == 1 ? data[0] : throw new ArgumentException();
    }

    public static void ParseEmergencyActivationSourceData(
      IList<byte> data,
      out StatusReport0EmergencyActivationSource source)
    {
      source = data.Count == 1 ? (StatusReport0EmergencyActivationSource) data[0] : throw new ArgumentException();
    }

    public static void ParseSignalStatistics(
      IList<byte> data,
      out uint attempted0Or1,
      out uint attempted2Or3,
      out uint attempted4Or5,
      out uint successful0Or1,
      out uint successful2Or3,
      out uint successful4Or5)
    {
      if (data.Count != 24)
        throw new ArgumentException();
      attempted0Or1 = (uint) ((int) data[0] * 16777216 + (int) data[1] * 65536 + (int) data[2] * 256) + (uint) data[3];
      attempted2Or3 = (uint) ((int) data[4] * 16777216 + (int) data[5] * 65536 + (int) data[6] * 256) + (uint) data[7];
      attempted4Or5 = (uint) ((int) data[8] * 16777216 + (int) data[9] * 65536 + (int) data[10] * 256) + (uint) data[11];
      successful0Or1 = (uint) ((int) data[12] * 16777216 + (int) data[13] * 65536 + (int) data[14] * 256) + (uint) data[15];
      successful2Or3 = (uint) ((int) data[16] * 16777216 + (int) data[17] * 65536 + (int) data[18] * 256) + (uint) data[19];
      successful4Or5 = (uint) ((int) data[20] * 16777216 + (int) data[21] * 65536 + (int) data[22] * 256) + (uint) data[23];
    }

    public static void ParseGpsStatistics(
      IList<byte> data,
      out uint attemptedXX,
      out uint attempted2D,
      out uint attempted3D,
      out uint successfulXX,
      out uint successful2D,
      out uint successful3D)
    {
      if (data.Count != 24)
        throw new ArgumentException();
      attemptedXX = (uint) ((int) data[0] * 16777216 + (int) data[1] * 65536 + (int) data[2] * 256) + (uint) data[3];
      attempted2D = (uint) ((int) data[4] * 16777216 + (int) data[5] * 65536 + (int) data[6] * 256) + (uint) data[7];
      attempted3D = (uint) ((int) data[8] * 16777216 + (int) data[9] * 65536 + (int) data[10] * 256) + (uint) data[11];
      successfulXX = (uint) ((int) data[12] * 16777216 + (int) data[13] * 65536 + (int) data[14] * 256) + (uint) data[15];
      successful2D = (uint) ((int) data[16] * 16777216 + (int) data[17] * 65536 + (int) data[18] * 256) + (uint) data[19];
      successful3D = (uint) ((int) data[20] * 16777216 + (int) data[21] * 65536 + (int) data[22] * 256) + (uint) data[23];
    }

    public static void ParseTwoDateTimes(
      IList<byte> data,
      out DateTime dateTime1,
      out DateTime dateTime2)
    {
      if (data.Count != 8)
        throw new ArgumentException();
      uint num1 = (uint) ((int) data[0] * 16777216 + (int) data[1] * 65536 + (int) data[2] * 256) + (uint) data[3];
      uint num2 = (uint) ((int) data[4] * 16777216 + (int) data[5] * 65536 + (int) data[6] * 256) + (uint) data[7];
      DateTime dateTime3;
      if (num1 == 0U)
      {
        dateTime1 = DateTime.MinValue;
      }
      else
      {
        ref DateTime local = ref dateTime1;
        dateTime3 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime dateTime4 = dateTime3.AddSeconds((double) (num1 - 1U));
        local = dateTime4;
      }
      if (num2 == 0U)
      {
        dateTime2 = DateTime.MinValue;
      }
      else
      {
        ref DateTime local = ref dateTime2;
        dateTime3 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime dateTime5 = dateTime3.AddSeconds((double) (num2 - 1U));
        local = dateTime5;
      }
    }

    private static void AddGpsStatisticsItemToXml(
      XElement element,
      IList<byte> data,
      string tagName)
    {
      uint attemptedXX;
      uint attempted2D;
      uint attempted3D;
      uint successfulXX;
      uint successful2D;
      uint successful3D;
      StatusReport0.ParseGpsStatistics(data, out attemptedXX, out attempted2D, out attempted3D, out successfulXX, out successful2D, out successful3D);
      element.Add((object) new XElement((XName) tagName, new object[2]
      {
        (object) new XElement((XName) "attempts", new object[3]
        {
          (object) new XElement((XName) "noOrDROrTimeOnly", (object) attemptedXX),
          (object) new XElement((XName) "twoD", (object) attempted2D),
          (object) new XElement((XName) "threeD", (object) attempted3D)
        }),
        (object) new XElement((XName) "successes", new object[3]
        {
          (object) new XElement((XName) "noOrDROrTimeOnly", (object) successfulXX),
          (object) new XElement((XName) "twoD", (object) successful2D),
          (object) new XElement((XName) "threeD", (object) successful3D)
        })
      }));
    }

    private static void AddSignalStatisticsItemToXml(
      XElement element,
      IList<byte> data,
      string tagName)
    {
      uint attempted0Or1;
      uint attempted2Or3;
      uint attempted4Or5;
      uint successful0Or1;
      uint successful2Or3;
      uint successful4Or5;
      StatusReport0.ParseSignalStatistics(data, out attempted0Or1, out attempted2Or3, out attempted4Or5, out successful0Or1, out successful2Or3, out successful4Or5);
      element.Add((object) new XElement((XName) tagName, new object[2]
      {
        (object) new XElement((XName) "attempts", new object[3]
        {
          (object) new XElement((XName) "zeroOrOne", (object) attempted0Or1),
          (object) new XElement((XName) "twoOrThree", (object) attempted2Or3),
          (object) new XElement((XName) "fourOrFive", (object) attempted4Or5)
        }),
        (object) new XElement((XName) "successes", new object[3]
        {
          (object) new XElement((XName) "zeroOrOne", (object) successful0Or1),
          (object) new XElement((XName) "twoOrThree", (object) successful2Or3),
          (object) new XElement((XName) "fourOrFive", (object) successful4Or5)
        })
      }));
    }

    private static void AddTimestampsItemToXml(
      XElement element,
      IList<byte> data,
      string tagName,
      string timeAName,
      string timeBName)
    {
      DateTime dateTime1;
      DateTime dateTime2;
      StatusReport0.ParseTwoDateTimes(data, out dateTime1, out dateTime2);
      element.Add((object) new XElement((XName) tagName, new object[2]
      {
        (object) new XElement((XName) timeAName, (object) dateTime1.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss.fZ", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) timeBName, (object) dateTime2.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss.fZ", (IFormatProvider) CultureInfo.InvariantCulture))
      }));
    }
  }
}
