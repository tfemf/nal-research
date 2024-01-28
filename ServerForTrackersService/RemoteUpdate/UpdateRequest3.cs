// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest3
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class UpdateRequest3 : UpdateRequest
  {
    private string password;
    private List<UpdateRequest3Item> items;

    public UpdateRequest3()
    {
      this.items = new List<UpdateRequest3Item>();
      this.ResetValues();
    }

    public bool IsForShoutGsm110To111 { get; set; }

    public override string Password
    {
      get => this.password;
      set => this.password = value.Length == 8 ? value : throw new ArgumentException();
    }

    public List<UpdateRequest3Item> Items => this.items;

    public static UpdateRequest3Tag LookUpTag(int table, byte tag)
    {
      if (table != 0)
        return UpdateRequest3Tag.Unknown;
      switch (tag)
      {
        case 0:
          return UpdateRequest3Tag.TimeBetweenReports;
        case 1:
          return UpdateRequest3Tag.TimeToKeepTrying;
        case 2:
          return UpdateRequest3Tag.TestTimeBetweenReports;
        case 3:
          return UpdateRequest3Tag.TestTimeToKeepTrying;
        case 4:
          return UpdateRequest3Tag.EmergencyTimeBetweenReports;
        case 5:
          return UpdateRequest3Tag.EmergencyTimeToKeepTrying;
        case 6:
          return UpdateRequest3Tag.AwakeTimeBetweenReports;
        case 7:
          return UpdateRequest3Tag.AwakeTimeToKeepTrying;
        case 8:
          return UpdateRequest3Tag.TestAwakeTimeBetweenReports;
        case 9:
          return UpdateRequest3Tag.TestAwakeTimeToKeepTrying;
        case 10:
          return UpdateRequest3Tag.EmergencyAwakeTimeBetweenReports;
        case 11:
          return UpdateRequest3Tag.EmergencyAwakeTimeToKeepTrying;
        case 12:
          return UpdateRequest3Tag.BlockInvalidGpsReportsPerMode;
        case 13:
          return UpdateRequest3Tag.MotionSensorWait;
        case 14:
          return UpdateRequest3Tag.TestMotionSensorWait;
        case 15:
          return UpdateRequest3Tag.EmergencyMotionSensorWait;
        case 16:
          return UpdateRequest3Tag.EmergencyReportFlood;
        case 17:
          return UpdateRequest3Tag.Callable;
        case 18:
          return UpdateRequest3Tag.TestCallable;
        case 19:
          return UpdateRequest3Tag.EmergencyCallable;
        case 20:
          return UpdateRequest3Tag.MotionSensorAwakesPerMode;
        case 21:
          return UpdateRequest3Tag.IgnoreTestAndEmergencySwitches;
        case 22:
          return UpdateRequest3Tag.SignalingPins;
        case 23:
          return UpdateRequest3Tag.SignalingRepetitions;
        case 24:
          return UpdateRequest3Tag.OutputPinStates;
        case 25:
          return UpdateRequest3Tag.ReportFormat;
        case 26:
          return UpdateRequest3Tag.SamePlaceSkipReports;
        case 27:
          return UpdateRequest3Tag.TestSamePlaceSkipReports;
        case 28:
          return UpdateRequest3Tag.EmergencySamePlaceSkipReports;
        case 29:
          return UpdateRequest3Tag.MotionSensorBegin;
        case 30:
          return UpdateRequest3Tag.MotionSensorEnd;
        case 31:
          return UpdateRequest3Tag.BufferAndSendMissedReports;
        case 32:
          return UpdateRequest3Tag.DataLoggingEnabled;
        case 33:
          return UpdateRequest3Tag.EmergencyEnabled;
        case 34:
          return UpdateRequest3Tag.TrackingEnabled;
        case 35:
          return UpdateRequest3Tag.IncludeGpsInMessages;
        case 36:
          return UpdateRequest3Tag.TrackingMethod;
        case 37:
          return UpdateRequest3Tag.ResponseMethod;
        case 38:
          return UpdateRequest3Tag.SmsDestination;
        case 39:
          return UpdateRequest3Tag.SmsValidityPeriod;
        case 40:
          return UpdateRequest3Tag.DataCallDestination;
        case 41:
          return UpdateRequest3Tag.PollReport;
        case 42:
          return UpdateRequest3Tag.MailboxChecksBetweenReportCycles;
        case 43:
          return UpdateRequest3Tag.EmergencyMailboxChecksBetweenReportCycles;
        case 44:
          return UpdateRequest3Tag.TestReportFlood;
        case 45:
          return UpdateRequest3Tag.EmergencyAcknowledgement;
        case 46:
          return UpdateRequest3Tag.StartupProfile;
        case 47:
          return UpdateRequest3Tag.AddCallOutSchedule;
        case 48:
          return UpdateRequest3Tag.RemoveCallOutSchedule;
        case 49:
          return UpdateRequest3Tag.AddGeofence;
        case 50:
          return UpdateRequest3Tag.RemoveGeofence;
        case 51:
          return UpdateRequest3Tag.ModifyGeofence;
        case 52:
          return UpdateRequest3Tag.PowerUpDelay;
        case 53:
          return UpdateRequest3Tag.GeneralProfileHeader;
        case 54:
          return UpdateRequest3Tag.TrackingProfileHeader;
        case 55:
          return UpdateRequest3Tag.ReportFlood;
        case 56:
          return UpdateRequest3Tag.BlockInvalidGpsReports;
        case 57:
          return UpdateRequest3Tag.MotionSensorAwakes;
        case 58:
          return UpdateRequest3Tag.QueuedReports;
        case 59:
          return UpdateRequest3Tag.GeofenceCheckFrequency;
        case 60:
          return UpdateRequest3Tag.PollStatistics;
        case 61:
          return UpdateRequest3Tag.Links;
        case 62:
          return UpdateRequest3Tag.LinkSwitchTime;
        case 63:
          return UpdateRequest3Tag.SuccessfulSendRequired;
        case 64:
          return UpdateRequest3Tag.MotionSensorBeginWithWindowSize;
        case 65:
          return UpdateRequest3Tag.InputPin;
        case 66:
          return UpdateRequest3Tag.SmsPhoneNumberDestination;
        case 67:
          return UpdateRequest3Tag.SmsEmailDestination;
        case 68:
          return UpdateRequest3Tag.PowerUpTimeout;
        case 69:
          return UpdateRequest3Tag.MailboxCheckRate;
        case 70:
          return UpdateRequest3Tag.GsmSendMethod;
        case 71:
          return UpdateRequest3Tag.GsmTcpConnectionType;
        case 72:
          return UpdateRequest3Tag.GsmTcpDestination;
        case 73:
          return UpdateRequest3Tag.GsmTcpTimeOuts;
        case 74:
          return UpdateRequest3Tag.PollEmergencyActivationSource;
        case 75:
          return UpdateRequest3Tag.PollIridiumLastReportTimes;
        case 76:
          return UpdateRequest3Tag.PollGsmLastReportTimes;
        case 77:
          return UpdateRequest3Tag.PollAntennaStatus;
        case 78:
          return UpdateRequest3Tag.Time;
        case 79:
          return UpdateRequest3Tag.ClearLastRemoteUpdateTime;
        case 80:
          return UpdateRequest3Tag.RemoteUpdateTimeCheckEnabled;
        case 82:
          return UpdateRequest3Tag.ManDown;
        case 83:
          return UpdateRequest3Tag.DelayFirstReport;
        case 84:
          return UpdateRequest3Tag.DelayFirstReportWhileInMotion;
        case 85:
          return UpdateRequest3Tag.UseAlternateMotionSettings;
        case 86:
          return UpdateRequest3Tag.ManDownTimeDown;
        case 87:
          return UpdateRequest3Tag.LockPin;
        case 89:
          return UpdateRequest3Tag.EncryptionKey;
        case 90:
          return UpdateRequest3Tag.DecryptionKey;
        case 91:
          return UpdateRequest3Tag.ZeroizeKeys;
        case 92:
          return UpdateRequest3Tag.AddModifyContact;
        case 93:
          return UpdateRequest3Tag.DeleteContact;
        case 94:
          return UpdateRequest3Tag.ClearContacts;
        case 96:
          return UpdateRequest3Tag.AddModifyCannedMessage;
        case 97:
          return UpdateRequest3Tag.DeleteCannedMessage;
        case 98:
          return UpdateRequest3Tag.ClearCannedMessages;
        case 99:
          return UpdateRequest3Tag.MapPin;
        case 100:
          return UpdateRequest3Tag.ZeroizeFirmware;
        case 101:
          return UpdateRequest3Tag.PollDeviceState;
        default:
          return UpdateRequest3Tag.Unknown;
      }
    }

    public static int LookUpTagTable(UpdateRequest3Tag tag)
    {
      return tag == UpdateRequest3Tag.Unknown ? int.MaxValue : 0;
    }

    public static byte LookUpTagValue(UpdateRequest3Tag tag)
    {
      switch (tag)
      {
        case UpdateRequest3Tag.Unknown:
          return byte.MaxValue;
        case UpdateRequest3Tag.AddCallOutSchedule:
          return 47;
        case UpdateRequest3Tag.AddGeofence:
          return 49;
        case UpdateRequest3Tag.AddModifyCannedMessage:
          return 96;
        case UpdateRequest3Tag.AddModifyContact:
          return 92;
        case UpdateRequest3Tag.AwakeTimeBetweenReports:
          return 6;
        case UpdateRequest3Tag.AwakeTimeToKeepTrying:
          return 7;
        case UpdateRequest3Tag.BlockInvalidGpsReports:
          return 56;
        case UpdateRequest3Tag.BlockInvalidGpsReportsPerMode:
          return 12;
        case UpdateRequest3Tag.BufferAndSendMissedReports:
          return 31;
        case UpdateRequest3Tag.Callable:
          return 17;
        case UpdateRequest3Tag.ClearCannedMessages:
          return 98;
        case UpdateRequest3Tag.ClearContacts:
          return 94;
        case UpdateRequest3Tag.ClearLastRemoteUpdateTime:
          return 79;
        case UpdateRequest3Tag.DataCallDestination:
          return 40;
        case UpdateRequest3Tag.DataLoggingEnabled:
          return 32;
        case UpdateRequest3Tag.DecryptionKey:
          return 90;
        case UpdateRequest3Tag.DelayFirstReport:
          return 83;
        case UpdateRequest3Tag.DelayFirstReportWhileInMotion:
          return 84;
        case UpdateRequest3Tag.DeleteCannedMessage:
          return 97;
        case UpdateRequest3Tag.DeleteContact:
          return 93;
        case UpdateRequest3Tag.EmergencyAcknowledgement:
          return 45;
        case UpdateRequest3Tag.EmergencyAwakeTimeBetweenReports:
          return 10;
        case UpdateRequest3Tag.EmergencyAwakeTimeToKeepTrying:
          return 11;
        case UpdateRequest3Tag.EmergencyCallable:
          return 19;
        case UpdateRequest3Tag.EmergencyEnabled:
          return 33;
        case UpdateRequest3Tag.EmergencyMailboxChecksBetweenReportCycles:
          return 43;
        case UpdateRequest3Tag.EmergencyMotionSensorWait:
          return 15;
        case UpdateRequest3Tag.EmergencyReportFlood:
          return 16;
        case UpdateRequest3Tag.EmergencySamePlaceSkipReports:
          return 28;
        case UpdateRequest3Tag.EmergencyTimeBetweenReports:
          return 4;
        case UpdateRequest3Tag.EmergencyTimeToKeepTrying:
          return 5;
        case UpdateRequest3Tag.EncryptionKey:
          return 89;
        case UpdateRequest3Tag.GeneralProfileHeader:
          return 53;
        case UpdateRequest3Tag.GeofenceCheckFrequency:
          return 59;
        case UpdateRequest3Tag.GsmSendMethod:
          return 70;
        case UpdateRequest3Tag.GsmTcpConnectionType:
          return 71;
        case UpdateRequest3Tag.GsmTcpDestination:
          return 72;
        case UpdateRequest3Tag.GsmTcpTimeOuts:
          return 73;
        case UpdateRequest3Tag.IgnoreTestAndEmergencySwitches:
          return 21;
        case UpdateRequest3Tag.IncludeGpsInMessages:
          return 35;
        case UpdateRequest3Tag.InputPin:
          return 65;
        case UpdateRequest3Tag.Links:
          return 61;
        case UpdateRequest3Tag.LinkSwitchTime:
          return 62;
        case UpdateRequest3Tag.LockPin:
          return 87;
        case UpdateRequest3Tag.MailboxCheckRate:
          return 69;
        case UpdateRequest3Tag.MailboxChecksBetweenReportCycles:
          return 42;
        case UpdateRequest3Tag.ManDown:
          return 82;
        case UpdateRequest3Tag.ManDownTimeDown:
          return 86;
        case UpdateRequest3Tag.MapPin:
          return 99;
        case UpdateRequest3Tag.ModifyGeofence:
          return 51;
        case UpdateRequest3Tag.MotionSensorAwakes:
          return 57;
        case UpdateRequest3Tag.MotionSensorAwakesPerMode:
          return 20;
        case UpdateRequest3Tag.MotionSensorBegin:
          return 29;
        case UpdateRequest3Tag.MotionSensorBeginWithWindowSize:
          return 64;
        case UpdateRequest3Tag.MotionSensorEnd:
          return 30;
        case UpdateRequest3Tag.MotionSensorWait:
          return 13;
        case UpdateRequest3Tag.OutputPinStates:
          return 24;
        case UpdateRequest3Tag.PollAntennaStatus:
          return 77;
        case UpdateRequest3Tag.PollDeviceState:
          return 101;
        case UpdateRequest3Tag.PollEmergencyActivationSource:
          return 74;
        case UpdateRequest3Tag.PollGsmLastReportTimes:
          return 76;
        case UpdateRequest3Tag.PollIridiumLastReportTimes:
          return 75;
        case UpdateRequest3Tag.PollReport:
          return 41;
        case UpdateRequest3Tag.PollStatistics:
          return 60;
        case UpdateRequest3Tag.PowerUpDelay:
          return 52;
        case UpdateRequest3Tag.PowerUpTimeout:
          return 68;
        case UpdateRequest3Tag.QueuedReports:
          return 58;
        case UpdateRequest3Tag.RemoteUpdateTimeCheckEnabled:
          return 80;
        case UpdateRequest3Tag.RemoveCallOutSchedule:
          return 48;
        case UpdateRequest3Tag.RemoveGeofence:
          return 50;
        case UpdateRequest3Tag.ReportFlood:
          return 55;
        case UpdateRequest3Tag.ReportFormat:
          return 25;
        case UpdateRequest3Tag.ResponseMethod:
          return 37;
        case UpdateRequest3Tag.SamePlaceSkipReports:
          return 26;
        case UpdateRequest3Tag.SignalingPins:
          return 22;
        case UpdateRequest3Tag.SignalingRepetitions:
          return 23;
        case UpdateRequest3Tag.SmsDestination:
          return 38;
        case UpdateRequest3Tag.SmsEmailDestination:
          return 67;
        case UpdateRequest3Tag.SmsPhoneNumberDestination:
          return 66;
        case UpdateRequest3Tag.SmsValidityPeriod:
          return 39;
        case UpdateRequest3Tag.StartupProfile:
          return 46;
        case UpdateRequest3Tag.SuccessfulSendRequired:
          return 63;
        case UpdateRequest3Tag.TestAwakeTimeBetweenReports:
          return 8;
        case UpdateRequest3Tag.TestAwakeTimeToKeepTrying:
          return 9;
        case UpdateRequest3Tag.TestCallable:
          return 18;
        case UpdateRequest3Tag.TestMotionSensorWait:
          return 14;
        case UpdateRequest3Tag.TestReportFlood:
          return 44;
        case UpdateRequest3Tag.TestSamePlaceSkipReports:
          return 27;
        case UpdateRequest3Tag.TestTimeBetweenReports:
          return 2;
        case UpdateRequest3Tag.TestTimeToKeepTrying:
          return 3;
        case UpdateRequest3Tag.Time:
          return 78;
        case UpdateRequest3Tag.TimeBetweenReports:
          return 0;
        case UpdateRequest3Tag.TimeToKeepTrying:
          return 1;
        case UpdateRequest3Tag.TrackingEnabled:
          return 34;
        case UpdateRequest3Tag.TrackingMethod:
          return 36;
        case UpdateRequest3Tag.TrackingProfileHeader:
          return 54;
        case UpdateRequest3Tag.UseAlternateMotionSettings:
          return 85;
        case UpdateRequest3Tag.ZeroizeFirmware:
          return 100;
        case UpdateRequest3Tag.ZeroizeKeys:
          return 91;
        default:
          return 0;
      }
    }

    public override byte[] GetUpdate()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 117);
      byteList.Add((byte) 3);
      byteList.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(this.password));
      foreach (UpdateRequest3Item updateRequest3Item in this.items)
      {
        for (int index = 0; index < updateRequest3Item.TagTable; ++index)
          byteList.Add(byte.MaxValue);
        if (updateRequest3Item.Tag == UpdateRequest3Tag.ManDownTimeDown && this.IsForShoutGsm110To111)
          byteList.Add((byte) 83);
        else
          byteList.Add(updateRequest3Item.TagValue);
        byteList.Add((byte) updateRequest3Item.Data.Count);
        byteList.AddRange((IEnumerable<byte>) updateRequest3Item.Data);
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

    public override void ResetValues()
    {
      this.Password = "12345678";
      this.items.Clear();
    }

    public static bool Parse(byte[] data, out UpdateRequest3 update)
    {
      update = (UpdateRequest3) null;
      if (data.Length >= 12 && data[0] == (byte) 117 && data[1] == (byte) 3)
      {
        byte num1 = 0;
        byte num2 = 0;
        bool flag = false;
        int num3 = 0;
        int index1 = 0;
        while (index1 < data.Length - 2)
        {
          num1 ^= data[index1];
          num2 += data[index1];
          ++index1;
          if (index1 >= 10 && (int) num1 == (int) data[index1] && (int) num2 == (int) data[index1 + 1])
          {
            num3 = index1;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          update = new UpdateRequest3();
          update.Password = Encoding.GetEncoding(1252).GetString(data, 2, 8);
          int count1;
          byte count2;
          for (int index2 = 10; index2 < num3; index2 = count1 + (int) count2)
          {
            int tagTable = 0;
            for (; index2 < num3 && data[index2] == byte.MaxValue; ++index2)
              ++tagTable;
            if (index2 + 2 <= num3)
            {
              byte[] numArray1 = data;
              int index3 = index2;
              int num4 = index3 + 1;
              byte tagValue = numArray1[index3];
              byte[] numArray2 = data;
              int index4 = num4;
              count1 = index4 + 1;
              count2 = numArray2[index4];
              if (count1 + (int) count2 <= num3)
                update.Items.Add(new UpdateRequest3Item(tagTable, tagValue, (IList<byte>) ((IEnumerable<byte>) data).Skip<byte>(count1).Take<byte>((int) count2).ToList<byte>()));
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

    public static void ParseAddCallOutScheduleData(
      IList<byte> data,
      out DateTime time,
      out UpdateRequest3AddCallOutScheduleLink link)
    {
      if (data.Count != 4)
        throw new ArgumentException();
      try
      {
        time = new DateTime(1, 1, 1, (int) data[0], (int) data[1], (int) data[2], DateTimeKind.Utc);
      }
      catch (ArgumentOutOfRangeException ex)
      {
        throw new ArgumentException();
      }
      link = (UpdateRequest3AddCallOutScheduleLink) data[3];
    }

    public static void ParseAddGeofenceData(
      IList<byte> data,
      out string id,
      out UpdateRequest3GeofenceOptions options,
      out byte profileNumber,
      out List<Coordinate> points)
    {
      int num1 = 0;
      while (num1 < data.Count && data[num1] != (byte) 0)
        ++num1;
      if (data.Count < num1 + 3)
        throw new ArgumentException();
      id = Encoding.GetEncoding(1252).GetString(data.Take<byte>(num1).ToArray<byte>());
      int num2 = num1 + 1;
      ref UpdateRequest3GeofenceOptions local1 = ref options;
      IList<byte> byteList1 = data;
      int index1 = num2;
      int num3 = index1 + 1;
      int num4 = (int) byteList1[index1];
      local1 = (UpdateRequest3GeofenceOptions) num4;
      ref byte local2 = ref profileNumber;
      IList<byte> byteList2 = data;
      int index2 = num3;
      int index3 = index2 + 1;
      int num5 = (int) byteList2[index2];
      local2 = (byte) num5;
      points = new List<Coordinate>();
      for (; index3 <= data.Count - 8; index3 += 8)
      {
        uint num6 = (uint) ((int) data[index3] * 16777216 + (int) data[index3 + 1] * 65536 + (int) data[index3 + 2] * 256) + (uint) data[index3 + 3];
        int num7 = (int) data[index3 + 4] * 16777216 + (int) data[index3 + 5] * 65536 + (int) data[index3 + 6] * 256 + (int) data[index3 + 7];
        int num8 = (int) ((long) num6 - 900000000L);
        int num9 = (int) ((long) (uint) num7 - 1800000000L);
        points.Add(new Coordinate((double) num8 / 10000000.0, (double) num9 / 10000000.0));
      }
    }

    public static void ParseAddModifyCannedMessageData(
      IList<byte> data,
      out byte code,
      out ushort index,
      out string label,
      out string text)
    {
      code = data.Count >= 4 ? data[0] : throw new ArgumentException();
      index = (ushort) ((uint) data[1] * 256U + (uint) data[2]);
      int index1 = 3;
      int count1 = index1;
      while (index1 < data.Count && data[index1] != (byte) 0)
        ++index1;
      if (index1 == data.Count)
        throw new ArgumentException();
      label = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(count1).Take<byte>(index1 - count1).ToArray<byte>());
      int count2 = index1 + 1;
      text = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(count2).Take<byte>(data.Count - count2).ToArray<byte>());
    }

    public static void ParseAddModifyContactData(
      IList<byte> data,
      out ushort code,
      out Dictionary<int, string> fields)
    {
      if (data.Count < 2)
        throw new ArgumentException();
      code = (ushort) ((uint) data[0] * 256U + (uint) data[1]);
      fields = new Dictionary<int, string>();
      int index1 = 2;
      int key = 0;
      while (index1 < data.Count)
      {
        byte num = data[index1++];
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((int) num & 1 << index2) != 0)
          {
            int count = index1;
            while (index1 < data.Count && data[index1] != (byte) 0)
              ++index1;
            if (index1 == data.Count)
              throw new ArgumentException();
            fields[key] = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(count).Take<byte>(index1 - count).ToArray<byte>());
            ++index1;
          }
          ++key;
        }
      }
    }

    public static void ParseAwakeTimeBetweenReportsData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseAwakeTimeToKeepTryingData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseTimeToKeepTryingHelper(data, out duration);
    }

    public static void ParseBlockInvalidGpsReportsData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseBlockInvalidGpsReportsPerModeData(
      IList<byte> data,
      out bool normal,
      out bool test,
      out bool emergency)
    {
      bool flag;
      UpdateRequest3.ParseMaskData(data, out normal, out test, out emergency, out flag, out flag, out flag, out flag, out flag);
    }

    public static void ParseBufferAndSendMissedReportsData(
      IList<byte> data,
      out bool enabled,
      out byte maxToSkip,
      out ushort maxBufferCount)
    {
      if (data.Count != 4)
        throw new ArgumentException();
      enabled = data[0] > (byte) 0;
      maxToSkip = data[1];
      maxBufferCount = (ushort) ((uint) data[2] * 256U + (uint) data[3]);
    }

    public static void ParseCallableData(IList<byte> data, out UpdateRequest3CallableValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3CallableValue) data[0] : throw new ArgumentException();
    }

    public static void ParseClearCannedMessagesData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParseClearContactsData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParseClearLastRemoteUpdateTimeData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParseDataCallDestinationData(
      IList<byte> data,
      out UpdateRequest3DataCallDestinationType type,
      out string value)
    {
      type = data.Count >= 1 ? (UpdateRequest3DataCallDestinationType) data[0] : throw new ArgumentException();
      value = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(1).ToArray<byte>());
    }

    public static void ParseDataLoggingEnabledData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseDelayFirstReportData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseDelayFirstReportWhileInMotionData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseDecryptionKeyData(
      IList<byte> data,
      out string coPassword,
      out List<byte> key)
    {
      UpdateRequest3.ParseKeyHelper(data, out coPassword, out key);
    }

    public static void ParseDeleteCannedMessageData(IList<byte> data, out byte code)
    {
      code = data.Count == 1 ? data[0] : throw new ArgumentException();
    }

    public static void ParseDeleteContactData(IList<byte> data, out ushort code)
    {
      if (data.Count != 2)
        throw new ArgumentException();
      code = (ushort) ((uint) data[0] * 256U + (uint) data[1]);
    }

    public static void ParseEmergencyAcknowledgementData(
      IList<byte> data,
      out UpdateRequest3EmergencyAcknowledgementValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3EmergencyAcknowledgementValue) data[0] : throw new ArgumentException();
    }

    public static void ParseEmergencyAwakeTimeBetweenReportsData(
      IList<byte> data,
      out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseEmergencyAwakeTimeToKeepTryingData(
      IList<byte> data,
      out TimeSpan duration)
    {
      UpdateRequest3.ParseTimeToKeepTryingHelper(data, out duration);
    }

    public static void ParseEmergencyCallableData(
      IList<byte> data,
      out UpdateRequest3CallableValue value)
    {
      UpdateRequest3.ParseCallableData(data, out value);
    }

    public static void ParseEmergencyEnabledData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseEmergencyMailboxChecksBetweenReportCyclesData(
      IList<byte> data,
      out byte value)
    {
      UpdateRequest3.ParseByteData(data, out value);
    }

    public static void ParseEmergencyMotionSensorWaitData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutes(data, out duration);
    }

    public static void ParseEmergencyReportFloodData(IList<byte> data, out byte value)
    {
      UpdateRequest3.ParseByteData(data, out value);
    }

    public static void ParseEmergencySamePlaceSkipReportsData(
      IList<byte> data,
      out UpdateRequest3SamePlaceSkipReportsMode mode,
      out ushort radius,
      out ushort beforeSkip,
      out ushort toSkip)
    {
      UpdateRequest3.ParseSamePlaceSkipReportsHelper(data, out mode, out radius, out beforeSkip, out toSkip);
    }

    public static void ParseEmergencyTimeBetweenReportsData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseEmergencyTimeToKeepTryingData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseTimeToKeepTryingHelper(data, out duration);
    }

    public static void ParseEncryptionKeyData(
      IList<byte> data,
      out string coPassword,
      out List<byte> key)
    {
      UpdateRequest3.ParseKeyHelper(data, out coPassword, out key);
    }

    public static void ParseGeneralProfileHeaderData(IList<byte> data, out byte profileNumber)
    {
      UpdateRequest3.ParseByteData(data, out profileNumber);
    }

    public static void ParseGeofenceCheckFrequencyData(
      IList<byte> data,
      out TimeSpan gpsSearchTime,
      out TimeSpan frequency)
    {
      UpdateRequest3.ParseAcquisitionTimeout(data, out gpsSearchTime, out frequency);
    }

    public static void ParseGsmSendMethodData(
      IList<byte> data,
      out UpdateRequest3GsmSendMethodValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3GsmSendMethodValue) data[0] : throw new ArgumentException();
    }

    public static void ParseGsmTcpConnectionTypeData(
      IList<byte> data,
      out UpdateRequest3GsmTcpConnectionTypeValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3GsmTcpConnectionTypeValue) data[0] : throw new ArgumentException();
    }

    public static void ParseGsmTcpDestinationData(
      IList<byte> data,
      out string ipAddress,
      out ushort port)
    {
      if (data.Count < 2)
        throw new ArgumentException();
      port = (ushort) ((uint) data[0] * 256U + (uint) data[1]);
      ipAddress = Encoding.ASCII.GetString(data.Skip<byte>(2).ToArray<byte>());
    }

    public static void ParseGsmTcpTimeOutsData(
      IList<byte> data,
      out TimeSpan inactivityTimeOut,
      out TimeSpan closingTimeOut)
    {
      inactivityTimeOut = data.Count == 2 ? new TimeSpan(0, 0, (int) data[0]) : throw new ArgumentException();
      closingTimeOut = new TimeSpan(0, 0, (int) data[1]);
    }

    public static void ParseIgnoreTestAndEmergencySwitchesData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseIncludeGpsInMessagesData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseInputPinData(
      IList<byte> data,
      out byte number,
      out UpdateRequest3InputPinType type,
      out UpdateRequest3InputPinOptions options)
    {
      number = data.Count == 3 ? data[0] : throw new ArgumentException();
      type = (UpdateRequest3InputPinType) data[1];
      options = (UpdateRequest3InputPinOptions) data[2];
    }

    public static void ParseLinksData(
      IList<byte> data,
      out UpdateRequest3LinksAllowedLinks allowedLinks,
      out UpdateRequest3LinksPrimaryLink primaryLink)
    {
      allowedLinks = data.Count == 2 ? (UpdateRequest3LinksAllowedLinks) data[0] : throw new ArgumentException();
      primaryLink = (UpdateRequest3LinksPrimaryLink) data[1];
    }

    public static void ParseLinkSwitchTimeData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseLockPinData(IList<byte> data, out ushort? pin)
    {
      if (data.Count == 0)
        pin = new ushort?();
      else
        pin = data.Count == 2 ? new ushort?((ushort) ((uint) data[0] * 256U + (uint) data[1])) : throw new ArgumentOutOfRangeException();
    }

    public static void ParseMailboxCheckRateData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseMailboxChecksBetweenReportCyclesData(IList<byte> data, out byte value)
    {
      UpdateRequest3.ParseByteData(data, out value);
    }

    public static void ParseManDownData(
      IList<byte> data,
      out bool enable,
      out TimeSpan countdownDuration,
      out UpdateRequest3ManDownFlags flags)
    {
      if (data.Count != 4)
        throw new ArgumentOutOfRangeException();
      enable = data[0] > (byte) 0;
      countdownDuration = new TimeSpan(0, 0, (int) data[1] * 256 + (int) data[2]);
      flags = (UpdateRequest3ManDownFlags) data[3];
    }

    public static void ParseManDownTimeDownData(IList<byte> data, out TimeSpan duration)
    {
      duration = data.Count == 2 ? new TimeSpan(0, 0, (int) data[0] * 256 + (int) data[1]) : throw new ArgumentOutOfRangeException();
    }

    public static void ParseMapPinData(IList<byte> data, out ushort pin)
    {
      if (data.Count != 2)
        throw new ArgumentOutOfRangeException();
      pin = (ushort) ((uint) data[0] * 256U + (uint) data[1]);
    }

    public static void ParseModifyGeofenceData(
      IList<byte> data,
      out string id,
      out string newId,
      out UpdateRequest3GeofenceOptions? newOptions,
      out byte? newProfileNumber)
    {
      int num1 = 0;
      while (num1 < data.Count && data[num1] != (byte) 0)
        ++num1;
      if (data.Count < num1 + 2)
        throw new ArgumentException();
      id = Encoding.GetEncoding(1252).GetString(data.Take<byte>(num1).ToArray<byte>());
      int num2 = num1 + 1;
      IList<byte> byteList = data;
      int index = num2;
      int count = index + 1;
      int num3 = (int) byteList[index];
      if ((num3 & 2) != 0)
      {
        if (data.Count < count + 1)
          throw new ArgumentException();
        newOptions = new UpdateRequest3GeofenceOptions?((UpdateRequest3GeofenceOptions) data[count++]);
      }
      else
        newOptions = new UpdateRequest3GeofenceOptions?();
      if ((num3 & 4) != 0)
      {
        if (data.Count < count + 1)
          throw new ArgumentException();
        newProfileNumber = new byte?(data[count++]);
      }
      else
        newProfileNumber = new byte?();
      if ((num3 & 1) != 0)
        newId = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(count).ToArray<byte>());
      else
        newId = (string) null;
    }

    public static void ParseMotionSensorAwakesData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseMotionSensorAwakesPerModeData(
      IList<byte> data,
      out bool normal,
      out bool test,
      out bool emergency)
    {
      bool flag;
      UpdateRequest3.ParseMaskData(data, out normal, out test, out emergency, out flag, out flag, out flag, out flag, out flag);
    }

    public static void ParseMotionSensorBeginData(
      IList<byte> data,
      out byte windowCount,
      out byte sensitivity)
    {
      windowCount = data.Count == 2 ? data[0] : throw new ArgumentException();
      sensitivity = data[1];
    }

    public static void ParseMotionSensorBeginWithWindowSizeData(
      IList<byte> data,
      out TimeSpan windowSize,
      out byte windowCount,
      out byte sensitivity)
    {
      windowSize = data.Count == 3 ? new TimeSpan(0, 0, (int) data[0]) : throw new ArgumentException();
      windowCount = data[1];
      sensitivity = data[2];
    }

    public static void ParseMotionSensorEndData(IList<byte> data, out TimeSpan duration)
    {
      duration = data.Count == 1 ? new TimeSpan(0, (int) data[0], 0) : throw new ArgumentException();
    }

    public static void ParseMotionSensorWaitData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutes(data, out duration);
    }

    public static void ParseOutputPinStatesData(
      IList<byte> data,
      out bool pin0,
      out bool pin1,
      out bool pin2,
      out bool pin3,
      out bool pin4,
      out bool pin5,
      out bool pin6,
      out bool pin7)
    {
      UpdateRequest3.ParseMaskData(data, out pin0, out pin1, out pin2, out pin3, out pin4, out pin5, out pin6, out pin7);
    }

    public static void ParsePollAntennaStatusData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParsePollDeviceStateData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParsePollEmergencyActivationSourceData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParsePollGsmLastReportTimesData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParsePollIridiumLastReportTimesData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParsePollReportData(IList<byte> data) => UpdateRequest3.ParseEmptyData(data);

    public static void ParsePollStatisticsData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParsePowerUpTimeoutData(
      IList<byte> data,
      out TimeSpan gpsSearchTime,
      out TimeSpan retryTime)
    {
      UpdateRequest3.ParseAcquisitionTimeout(data, out gpsSearchTime, out retryTime);
    }

    public static void ParsePowerUpDelayData(IList<byte> data, out DateTime? dateTime)
    {
      if (data.Count < 1)
        throw new ArgumentException();
      if (data[0] != (byte) 0)
      {
        if (data.Count != 8)
          throw new ArgumentException();
        try
        {
          dateTime = new DateTime?(new DateTime((int) data[1] * 256 + (int) data[2], (int) data[3], (int) data[4], (int) data[5], (int) data[6], (int) data[7], DateTimeKind.Utc));
        }
        catch (ArgumentOutOfRangeException ex)
        {
          throw new ArgumentException();
        }
      }
      else
        dateTime = new DateTime?();
    }

    public static void ParseQueuedReportsData(
      IList<byte> data,
      out bool restricted,
      out bool failed)
    {
      bool flag;
      UpdateRequest3.ParseMaskData(data, out restricted, out failed, out flag, out flag, out flag, out flag, out flag, out flag);
    }

    public static void ParseRemoteUpdateTimeCheckEnabledData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseRemoveCallOutScheduleData(IList<byte> data, out DateTime time)
    {
      if (data.Count != 3)
        throw new ArgumentException();
      try
      {
        time = new DateTime(1, 1, 1, (int) data[0], (int) data[1], (int) data[2], DateTimeKind.Utc);
      }
      catch (ArgumentOutOfRangeException ex)
      {
        throw new ArgumentException();
      }
    }

    public static void ParseRemoveGeofenceData(IList<byte> data, out string id)
    {
      id = Encoding.GetEncoding(1252).GetString(data.ToArray<byte>());
    }

    public static void ParseReportFloodData(IList<byte> data, out byte value)
    {
      UpdateRequest3.ParseByteData(data, out value);
    }

    public static void ParseReportFormatData(
      IList<byte> data,
      out UpdateRequest3ReportFormatValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3ReportFormatValue) data[0] : throw new ArgumentException();
    }

    public static void ParseResponseMethodData(
      IList<byte> data,
      out UpdateRequest3ResponseMethodValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3ResponseMethodValue) data[0] : throw new ArgumentException();
    }

    public static void ParseSamePlaceSkipReportsData(
      IList<byte> data,
      out UpdateRequest3SamePlaceSkipReportsMode mode,
      out ushort radius,
      out ushort beforeSkip,
      out ushort toSkip)
    {
      UpdateRequest3.ParseSamePlaceSkipReportsHelper(data, out mode, out radius, out beforeSkip, out toSkip);
    }

    public static void ParseSignalingPinsData(IList<byte> data, out byte signalingPins)
    {
      UpdateRequest3.ParseByteData(data, out signalingPins);
    }

    public static void ParseSignalingRepetitionsData(IList<byte> data, out byte value)
    {
      UpdateRequest3.ParseByteData(data, out value);
    }

    public static void ParseSmsDestinationData(
      IList<byte> data,
      out UpdateRequest3SmsDestinationType type,
      out string value)
    {
      type = data.Count >= 1 ? (UpdateRequest3SmsDestinationType) data[0] : throw new ArgumentException();
      value = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(1).ToArray<byte>());
    }

    public static void ParseSmsEmailDestinationData(IList<byte> data, out string value)
    {
      value = Encoding.GetEncoding(1252).GetString(data.ToArray<byte>());
    }

    public static void ParseSmsPhoneNumberDestinationData(
      IList<byte> data,
      out byte type,
      out string value)
    {
      type = data.Count >= 1 ? data[0] : throw new ArgumentException();
      value = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(1).ToArray<byte>());
    }

    public static void ParseSmsValidityPeriodData(IList<byte> data, out byte relVp)
    {
      UpdateRequest3.ParseByteData(data, out relVp);
    }

    public static void ParseStartupProfileData(IList<byte> data, out byte profileNumber)
    {
      UpdateRequest3.ParseByteData(data, out profileNumber);
    }

    public static void ParseSuccessfulSendRequiredData(
      IList<byte> data,
      out UpdateRequest3SuccessfulSendRequiredValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3SuccessfulSendRequiredValue) data[0] : throw new ArgumentException();
    }

    public static void ParseTestAwakeTimeBetweenReportsData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseTestAwakeTimeToKeepTryingData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseTimeToKeepTryingHelper(data, out duration);
    }

    public static void ParseTestCallableData(
      IList<byte> data,
      out UpdateRequest3CallableValue value)
    {
      UpdateRequest3.ParseCallableData(data, out value);
    }

    public static void ParseTestMotionSensorWaitData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutes(data, out duration);
    }

    public static void ParseTestReportFloodData(IList<byte> data, out byte value)
    {
      UpdateRequest3.ParseEmergencyReportFloodData(data, out value);
    }

    public static void ParseTestSamePlaceSkipReportsData(
      IList<byte> data,
      out UpdateRequest3SamePlaceSkipReportsMode mode,
      out ushort radius,
      out ushort beforeSkip,
      out ushort toSkip)
    {
      UpdateRequest3.ParseSamePlaceSkipReportsHelper(data, out mode, out radius, out beforeSkip, out toSkip);
    }

    public static void ParseTestTimeBetweenReportsData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseTestTimeToKeepTryingData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseTimeToKeepTryingHelper(data, out duration);
    }

    public static void ParseTimeData(IList<byte> data, out DateTime value)
    {
      if (data.Count != 4)
        throw new ArgumentException();
      uint num = (uint) ((int) data[0] * 16777216 + (int) data[1] * 65536 + (int) data[2] * 256) + (uint) data[3];
      if (num == 0U)
        value = DateTime.MinValue;
      else
        value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) (num - 1U));
    }

    public static void ParseTimeBetweenReportsData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseMinutesAndHalfMinute(data, out duration);
    }

    public static void ParseTimeToKeepTryingData(IList<byte> data, out TimeSpan duration)
    {
      UpdateRequest3.ParseTimeToKeepTryingHelper(data, out duration);
    }

    public static void ParseTrackingEnabledData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseTrackingMethodData(
      IList<byte> data,
      out UpdateRequest3TrackingMethodValue value)
    {
      value = data.Count == 1 ? (UpdateRequest3TrackingMethodValue) data[0] : throw new ArgumentException();
    }

    public static void ParseTrackingProfileHeaderData(IList<byte> data, out byte profileNumber)
    {
      UpdateRequest3.ParseByteData(data, out profileNumber);
    }

    public static void ParseUseAlternateMotionSettingsData(IList<byte> data, out bool value)
    {
      UpdateRequest3.ParseBooleanData(data, out value);
    }

    public static void ParseZeroizeFirmwareData(IList<byte> data)
    {
      UpdateRequest3.ParseEmptyData(data);
    }

    public static void ParseZeroizeKeysData(IList<byte> data, out string coPassword)
    {
      coPassword = Encoding.GetEncoding(1252).GetString(data.ToArray<byte>());
      if (coPassword.IndexOf(char.MinValue) != -1)
        throw new ArgumentException();
    }

    public static UpdateRequest3Item CreateAddCallOutSchedule(
      DateTime time,
      UpdateRequest3AddCallOutScheduleLink link)
    {
      time = time.ToUniversalTime();
      return new UpdateRequest3Item(UpdateRequest3Tag.AddCallOutSchedule, (IList<byte>) new byte[4]
      {
        (byte) time.Hour,
        (byte) time.Minute,
        (byte) time.Second,
        (byte) link
      });
    }

    public static UpdateRequest3Item CreateAddGeofence(
      string identifier,
      UpdateRequest3GeofenceOptions options,
      byte profileNumber,
      IEnumerable<Coordinate> coordinates)
    {
      List<byte> data = new List<byte>();
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(identifier));
      data.Add((byte) 0);
      data.Add((byte) options);
      data.Add(profileNumber);
      foreach (Coordinate coordinate in coordinates)
      {
        if (coordinate.Latitude < -90.0 || coordinate.Latitude > 90.0 || coordinate.Longitude < -180.0 || coordinate.Longitude > 180.0)
          throw new ArgumentOutOfRangeException();
        double num1 = coordinate.Latitude;
        int num2 = int.Parse(num1.ToString("0.0000000").Replace(".", string.Empty));
        num1 = coordinate.Longitude;
        int num3 = int.Parse(num1.ToString("0.0000000").Replace(".", string.Empty));
        uint num4 = (uint) ((ulong) num2 + 900000000UL);
        uint num5 = (uint) ((ulong) num3 + 1800000000UL);
        data.Add((byte) (num4 / 16777216U));
        data.Add((byte) (num4 / 65536U % 256U));
        data.Add((byte) (num4 / 256U % 256U));
        data.Add((byte) (num4 % 256U));
        data.Add((byte) (num5 / 16777216U));
        data.Add((byte) (num5 / 65536U % 256U));
        data.Add((byte) (num5 / 256U % 256U));
        data.Add((byte) (num5 % 256U));
      }
      return new UpdateRequest3Item(UpdateRequest3Tag.AddGeofence, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateAddModifyCannedMessage(
      byte code,
      ushort index,
      string label,
      string text)
    {
      List<byte> data = new List<byte>();
      data.Add(code);
      data.Add((byte) ((uint) index / 256U));
      data.Add((byte) ((uint) index % 256U));
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(label));
      data.Add((byte) 0);
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(text));
      return new UpdateRequest3Item(UpdateRequest3Tag.AddModifyCannedMessage, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateAddModifyContact(
      ushort code,
      IDictionary<int, string> fields)
    {
      List<byte> data = new List<byte>();
      data.Add((byte) ((uint) code / 256U));
      data.Add((byte) ((uint) code % 256U));
      int index = 0;
      int num = 0;
      foreach (KeyValuePair<int, string> keyValuePair in (IEnumerable<KeyValuePair<int, string>>) fields.OrderBy<KeyValuePair<int, string>, int>((Func<KeyValuePair<int, string>, int>) (x => x.Key)))
      {
        for (; num * 8 < keyValuePair.Key + 1; ++num)
        {
          index = data.Count;
          data.Add((byte) 0);
        }
        if (keyValuePair.Value != null)
        {
          data[index] |= (byte) (1 << keyValuePair.Key % 8);
          data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(keyValuePair.Value));
          data.Add((byte) 0);
        }
      }
      return new UpdateRequest3Item(UpdateRequest3Tag.AddModifyContact, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateAwakeTimeBetweenReports(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.AwakeTimeBetweenReports, duration);
    }

    public static UpdateRequest3Item CreateAwakeTimeToKeepTrying(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeToKeepTryingHelper(UpdateRequest3Tag.AwakeTimeToKeepTrying, duration);
    }

    public static UpdateRequest3Item CreateBlockInvalidGpsReports(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.BlockInvalidGpsReports, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateBlockInvalidGpsReportsPerMode(
      bool normal,
      bool test,
      bool emergency)
    {
      byte num = 0;
      if (normal)
        num |= (byte) 1;
      if (test)
        num |= (byte) 2;
      if (emergency)
        num |= (byte) 4;
      return new UpdateRequest3Item(UpdateRequest3Tag.BlockInvalidGpsReportsPerMode, (IList<byte>) new byte[1]
      {
        num
      });
    }

    public static UpdateRequest3Item CreateBufferAndSendMissedReports(
      bool enabled,
      byte maxToSkip,
      ushort maxBufferCount)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.BufferAndSendMissedReports, (IList<byte>) new byte[4]
      {
        enabled ? (byte) 1 : (byte) 0,
        maxToSkip,
        (byte) ((uint) maxBufferCount / 256U),
        (byte) ((uint) maxBufferCount % 256U)
      });
    }

    public static UpdateRequest3Item CreateDataCallDestination(
      UpdateRequest3DataCallDestinationType type,
      string value)
    {
      List<byte> data = new List<byte>();
      data.Add((byte) type);
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(value));
      return new UpdateRequest3Item(UpdateRequest3Tag.DataCallDestination, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateCallable(UpdateRequest3CallableValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.Callable, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateClearCannedMessages()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ClearCannedMessages, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreateClearContacts()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ClearContacts, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreateClearLastRemoteUpdateTime()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ClearLastRemoteUpdateTime, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreateDataLoggingEnabled(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.DataLoggingEnabled, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateDelayFirstReport(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.DelayFirstReport, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateDelayFirstReportWhileInMotion(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.DelayFirstReportWhileInMotion, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateDecryptionKey(string coPassword, IEnumerable<byte> key)
    {
      return UpdateRequest3.CreateKeyHelper(UpdateRequest3Tag.DecryptionKey, coPassword, key);
    }

    public static UpdateRequest3Item CreateDeleteCannedMessage(byte code)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.DeleteCannedMessage, (IList<byte>) new byte[1]
      {
        code
      });
    }

    public static UpdateRequest3Item CreateDeleteContact(ushort code)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.DeleteContact, (IList<byte>) new byte[2]
      {
        (byte) ((uint) code / 256U),
        (byte) ((uint) code % 256U)
      });
    }

    public static UpdateRequest3Item CreateEmergencyAcknowledgement(
      UpdateRequest3EmergencyAcknowledgementValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.EmergencyAcknowledgement, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateEmergencyAwakeTimeBetweenReports(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.EmergencyAwakeTimeBetweenReports, duration);
    }

    public static UpdateRequest3Item CreateEmergencyAwakeTimeToKeepTrying(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeToKeepTryingHelper(UpdateRequest3Tag.EmergencyAwakeTimeToKeepTrying, duration);
    }

    public static UpdateRequest3Item CreateEmergencyCallable(UpdateRequest3CallableValue callable)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.EmergencyCallable, (IList<byte>) new byte[1]
      {
        (byte) callable
      });
    }

    public static UpdateRequest3Item CreateEmergencyEnabled(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.EmergencyEnabled, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateEmergencyMailboxChecksBetweenReportCycles(byte value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.EmergencyMailboxChecksBetweenReportCycles, (IList<byte>) new byte[1]
      {
        value
      });
    }

    public static UpdateRequest3Item CreateEmergencyMotionSensorWait(TimeSpan duration)
    {
      return UpdateRequest3.CreateMinutesHelper(UpdateRequest3Tag.EmergencyMotionSensorWait, duration);
    }

    public static UpdateRequest3Item CreateEmergencyReportFlood(byte value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.EmergencyReportFlood, (IList<byte>) new byte[1]
      {
        value
      });
    }

    public static UpdateRequest3Item CreateEmergencySamePlaceSkipReports(
      UpdateRequest3SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort beforeSkip,
      ushort toSkip)
    {
      return UpdateRequest3.CreateSamePlaceSkipReportsHelper(UpdateRequest3Tag.EmergencySamePlaceSkipReports, mode, radius, beforeSkip, toSkip);
    }

    public static UpdateRequest3Item CreateEmergencyTimeBetweenReports(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.EmergencyTimeBetweenReports, duration);
    }

    public static UpdateRequest3Item CreateEmergencyTimeToKeepTrying(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeToKeepTryingHelper(UpdateRequest3Tag.EmergencyTimeToKeepTrying, duration);
    }

    public static UpdateRequest3Item CreateEncryptionKey(string coPassword, IEnumerable<byte> key)
    {
      return UpdateRequest3.CreateKeyHelper(UpdateRequest3Tag.EncryptionKey, coPassword, key);
    }

    public static UpdateRequest3Item CreateGeneralProfileHeader(byte profileNumber)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.GeneralProfileHeader, (IList<byte>) new byte[1]
      {
        profileNumber
      });
    }

    public static UpdateRequest3Item CreateGeofenceCheckFrequency(
      TimeSpan gpsSearchTime,
      TimeSpan frequency)
    {
      return UpdateRequest3.CreateAcquisitionTimeoutHelper(UpdateRequest3Tag.GeofenceCheckFrequency, gpsSearchTime, frequency);
    }

    public static UpdateRequest3Item CreateGsmSendMethod(UpdateRequest3GsmSendMethodValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.GsmSendMethod, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateGsmTcpConnectionType(
      UpdateRequest3GsmTcpConnectionTypeValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.GsmTcpConnectionType, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateGsmTcpDestination(string ipAddress, ushort port)
    {
      List<byte> data = new List<byte>();
      data.Add((byte) ((uint) port / 256U));
      data.Add((byte) ((uint) port % 256U));
      data.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(ipAddress));
      return new UpdateRequest3Item(UpdateRequest3Tag.GsmTcpDestination, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateGsmTcpTimeOuts(
      TimeSpan inactivityTimeOut,
      TimeSpan closingTimeOut)
    {
      if (inactivityTimeOut.Milliseconds != 0 || inactivityTimeOut.TotalSeconds < 0.0 || inactivityTimeOut.TotalSeconds > (double) byte.MaxValue || closingTimeOut.Milliseconds != 0 || closingTimeOut.TotalSeconds < 0.0 || closingTimeOut.TotalSeconds > (double) byte.MaxValue)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(UpdateRequest3Tag.GsmTcpTimeOuts, (IList<byte>) new byte[2]
      {
        (byte) inactivityTimeOut.TotalSeconds,
        (byte) closingTimeOut.TotalSeconds
      });
    }

    public static UpdateRequest3Item CreateIgnoreTestAndEmergencySwitches(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.IgnoreTestAndEmergencySwitches, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateIncludeGpsInMessages(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.IncludeGpsInMessages, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateInputPin(
      byte number,
      UpdateRequest3InputPinType type,
      UpdateRequest3InputPinOptions options)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.InputPin, (IList<byte>) new byte[3]
      {
        number,
        (byte) type,
        (byte) options
      });
    }

    public static UpdateRequest3Item CreateLinks(
      UpdateRequest3LinksAllowedLinks allowed,
      UpdateRequest3LinksPrimaryLink primary)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.Links, (IList<byte>) new byte[2]
      {
        (byte) allowed,
        (byte) primary
      });
    }

    public static UpdateRequest3Item CreateLinkSwitchTime(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.LinkSwitchTime, duration);
    }

    public static UpdateRequest3Item CreateLockPin(ushort? pin)
    {
      byte[] data;
      if (pin.HasValue)
      {
        byte[] numArray = new byte[2];
        ushort? nullable = pin;
        numArray[0] = (byte) (nullable.HasValue ? new int?((int) nullable.GetValueOrDefault() / 256) : new int?()).Value;
        nullable = pin;
        numArray[1] = (byte) (nullable.HasValue ? new int?((int) nullable.GetValueOrDefault() % 256) : new int?()).Value;
        data = numArray;
      }
      else
        data = new byte[0];
      return new UpdateRequest3Item(UpdateRequest3Tag.LockPin, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateMailboxCheckRate(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.MailboxCheckRate, duration);
    }

    public static UpdateRequest3Item CreateMailboxChecksBetweenReportCycles(byte value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.MailboxChecksBetweenReportCycles, (IList<byte>) new byte[1]
      {
        value
      });
    }

    public static UpdateRequest3Item CreateManDown(
      bool enable,
      TimeSpan countdownDuration,
      UpdateRequest3ManDownFlags flags)
    {
      if (countdownDuration.Milliseconds != 0 || countdownDuration.TotalSeconds < 0.0 || countdownDuration.TotalSeconds > (double) ushort.MaxValue)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(UpdateRequest3Tag.ManDown, (IList<byte>) new byte[4]
      {
        enable ? (byte) 1 : (byte) 0,
        (byte) ((uint) (int) countdownDuration.TotalSeconds / 256U),
        (byte) ((uint) (int) countdownDuration.TotalSeconds % 256U),
        (byte) flags
      });
    }

    public static UpdateRequest3Item CreateManDownTimeDown(TimeSpan duration)
    {
      if (duration.Milliseconds != 0 || duration.TotalSeconds < 0.0 || duration.TotalSeconds > (double) ushort.MaxValue)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(UpdateRequest3Tag.ManDownTimeDown, (IList<byte>) new byte[2]
      {
        (byte) ((uint) (int) duration.TotalSeconds / 256U),
        (byte) ((uint) (int) duration.TotalSeconds % 256U)
      });
    }

    public static UpdateRequest3Item CreateMapPin(ushort pin)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.MapPin, (IList<byte>) new byte[2]
      {
        (byte) ((uint) pin / 256U),
        (byte) ((uint) pin % 256U)
      });
    }

    public static UpdateRequest3Item CreateModifyGeofence(
      string id,
      string newId,
      UpdateRequest3GeofenceOptions? newOptions,
      byte? newProfileNumber)
    {
      byte num = 0;
      if (newId != null)
        num |= (byte) 1;
      if (newOptions.HasValue)
        num |= (byte) 2;
      if (newProfileNumber.HasValue)
        num |= (byte) 4;
      List<byte> data = new List<byte>();
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(id));
      data.Add((byte) 0);
      data.Add(num);
      if (newOptions.HasValue)
        data.Add((byte) newOptions.Value);
      if (newProfileNumber.HasValue)
        data.Add(newProfileNumber.Value);
      if (newId != null)
        data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(newId));
      return new UpdateRequest3Item(UpdateRequest3Tag.ModifyGeofence, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateMotionSensorAwakes(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.MotionSensorAwakes, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateMotionSensorAwakesPerMode(
      bool normal,
      bool test,
      bool emergency)
    {
      byte num = 0;
      if (normal)
        num |= (byte) 1;
      if (test)
        num |= (byte) 2;
      if (emergency)
        num |= (byte) 4;
      return new UpdateRequest3Item(UpdateRequest3Tag.MotionSensorAwakesPerMode, (IList<byte>) new byte[1]
      {
        num
      });
    }

    public static UpdateRequest3Item CreateMotionSensorBegin(byte windowCount, byte sensitivity)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.MotionSensorBegin, (IList<byte>) new byte[2]
      {
        windowCount,
        sensitivity
      });
    }

    public static UpdateRequest3Item CreateMotionSensorBeginWithWindowSize(
      TimeSpan windowSize,
      byte windowCount,
      byte sensitivity)
    {
      if (windowSize.Milliseconds != 0 || windowSize.TotalSeconds < 0.0 || windowSize.TotalSeconds > (double) byte.MaxValue)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(UpdateRequest3Tag.MotionSensorBeginWithWindowSize, (IList<byte>) new byte[3]
      {
        (byte) windowSize.TotalSeconds,
        windowCount,
        sensitivity
      });
    }

    public static UpdateRequest3Item CreateMotionSensorEnd(TimeSpan duration)
    {
      if (duration.Milliseconds != 0 || duration.Seconds != 0 || duration.TotalMinutes < 0.0 || duration.TotalMinutes > (double) byte.MaxValue)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(UpdateRequest3Tag.MotionSensorEnd, (IList<byte>) new byte[1]
      {
        (byte) duration.TotalMinutes
      });
    }

    public static UpdateRequest3Item CreateMotionSensorWait(TimeSpan duration)
    {
      return UpdateRequest3.CreateMinutesHelper(UpdateRequest3Tag.MotionSensorWait, duration);
    }

    public static UpdateRequest3Item CreateOutputPinStates(
      bool pin0,
      bool pin1,
      bool pin2,
      bool pin3,
      bool pin4,
      bool pin5,
      bool pin6,
      bool pin7)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.OutputPinStates, (IList<byte>) new byte[1]
      {
        (byte) ((pin0 ? 1 : 0) | (pin1 ? 2 : 0) | (pin2 ? 4 : 0) | (pin3 ? 8 : 0) | (pin4 ? 16 : 0) | (pin5 ? 32 : 0) | (pin6 ? 64 : 0) | (pin7 ? 128 : 0))
      });
    }

    public static UpdateRequest3Item CreatePollAntennaStatus()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollAntennaStatus, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePollDeviceState()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollDeviceState, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePollEmergencyActivationSource()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollEmergencyActivationSource, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePollGsmLastReportTimes()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollGsmLastReportTimes, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePollIridiumLastReportTimes()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollIridiumLastReportTimes, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePollReport()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollReport, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePollStatistics()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.PollStatistics, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreatePowerUpDelay(DateTime? dateTime)
    {
      List<byte> data = new List<byte>();
      data.Add(dateTime.HasValue ? (byte) 1 : (byte) 0);
      if (dateTime.HasValue)
      {
        DateTime universalTime = dateTime.Value.ToUniversalTime();
        data.Add((byte) (universalTime.Year / 256));
        data.Add((byte) (universalTime.Year % 256));
        data.Add((byte) universalTime.Month);
        data.Add((byte) universalTime.Day);
        data.Add((byte) universalTime.Hour);
        data.Add((byte) universalTime.Minute);
        data.Add((byte) universalTime.Second);
      }
      return new UpdateRequest3Item(UpdateRequest3Tag.PowerUpDelay, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreatePowerUpTimeout(
      TimeSpan gpsSearchTime,
      TimeSpan retryTime)
    {
      return UpdateRequest3.CreateAcquisitionTimeoutHelper(UpdateRequest3Tag.PowerUpTimeout, gpsSearchTime, retryTime);
    }

    public static UpdateRequest3Item CreateQueuedReports(bool restricted, bool failed)
    {
      byte num = 0;
      if (restricted)
        num |= (byte) 1;
      if (failed)
        num |= (byte) 2;
      return new UpdateRequest3Item(UpdateRequest3Tag.QueuedReports, (IList<byte>) new byte[1]
      {
        num
      });
    }

    public static UpdateRequest3Item CreateRemoteUpdateTimeCheckEnabled(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.RemoteUpdateTimeCheckEnabled, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateRemoveCallOutSchedule(DateTime time)
    {
      time = time.ToUniversalTime();
      return new UpdateRequest3Item(UpdateRequest3Tag.RemoveCallOutSchedule, (IList<byte>) new byte[3]
      {
        (byte) time.Hour,
        (byte) time.Minute,
        (byte) time.Second
      });
    }

    public static UpdateRequest3Item CreateRemoveGeofence(string id)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.RemoveGeofence, (IList<byte>) Encoding.GetEncoding(1252).GetBytes(id));
    }

    public static UpdateRequest3Item CreateReportFlood(byte value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ReportFlood, (IList<byte>) new byte[1]
      {
        value
      });
    }

    public static UpdateRequest3Item CreateReportFormat(UpdateRequest3ReportFormatValue format)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ReportFormat, (IList<byte>) new byte[1]
      {
        (byte) format
      });
    }

    public static UpdateRequest3Item CreateResponseMethod(UpdateRequest3ResponseMethodValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ResponseMethod, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateSamePlaceSkipReports(
      UpdateRequest3SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort beforeSkip,
      ushort toSkip)
    {
      return UpdateRequest3.CreateSamePlaceSkipReportsHelper(UpdateRequest3Tag.SamePlaceSkipReports, mode, radius, beforeSkip, toSkip);
    }

    public static UpdateRequest3Item CreateSignalingPins(byte signalingPins)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.SignalingPins, (IList<byte>) new byte[1]
      {
        signalingPins
      });
    }

    public static UpdateRequest3Item CreateSignalingRepetitions(byte value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.SignalingRepetitions, (IList<byte>) new byte[1]
      {
        value
      });
    }

    public static UpdateRequest3Item CreateSmsDestination(
      UpdateRequest3SmsDestinationType type,
      string value)
    {
      List<byte> data = new List<byte>();
      data.Add((byte) type);
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(value));
      return new UpdateRequest3Item(UpdateRequest3Tag.SmsDestination, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateSmsEmailDestination(string value)
    {
      List<byte> data = new List<byte>();
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(value));
      return new UpdateRequest3Item(UpdateRequest3Tag.SmsEmailDestination, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateSmsPhoneNumberDestination(byte type, string value)
    {
      List<byte> data = new List<byte>();
      data.Add(type);
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(value));
      return new UpdateRequest3Item(UpdateRequest3Tag.SmsPhoneNumberDestination, (IList<byte>) data);
    }

    public static UpdateRequest3Item CreateSmsValidityPeriod(byte relVp)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.SmsValidityPeriod, (IList<byte>) new byte[1]
      {
        relVp
      });
    }

    public static UpdateRequest3Item CreateStartupProfile(byte profileNumber)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.StartupProfile, (IList<byte>) new byte[1]
      {
        profileNumber
      });
    }

    public static UpdateRequest3Item CreateSuccessfulSendRequired(
      UpdateRequest3SuccessfulSendRequiredValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.SuccessfulSendRequired, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateTestAwakeTimeBetweenReports(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.TestAwakeTimeBetweenReports, duration);
    }

    public static UpdateRequest3Item CreateTestAwakeTimeToKeepTrying(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeToKeepTryingHelper(UpdateRequest3Tag.TestAwakeTimeToKeepTrying, duration);
    }

    public static UpdateRequest3Item CreateTestCallable(UpdateRequest3CallableValue callable)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.TestCallable, (IList<byte>) new byte[1]
      {
        (byte) callable
      });
    }

    public static UpdateRequest3Item CreateTestMotionSensorWait(TimeSpan duration)
    {
      return UpdateRequest3.CreateMinutesHelper(UpdateRequest3Tag.TestMotionSensorWait, duration);
    }

    public static UpdateRequest3Item CreateTestReportFlood(byte value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.TestReportFlood, (IList<byte>) new byte[1]
      {
        value
      });
    }

    public static UpdateRequest3Item CreateTestSamePlaceSkipReports(
      UpdateRequest3SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort beforeSkip,
      ushort toSkip)
    {
      return UpdateRequest3.CreateSamePlaceSkipReportsHelper(UpdateRequest3Tag.TestSamePlaceSkipReports, mode, radius, beforeSkip, toSkip);
    }

    public static UpdateRequest3Item CreateTestTimeBetweenReports(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.TestTimeBetweenReports, duration);
    }

    public static UpdateRequest3Item CreateTestTimeToKeepTrying(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeToKeepTryingHelper(UpdateRequest3Tag.TestTimeToKeepTrying, duration);
    }

    public static UpdateRequest3Item CreateTime(DateTime value)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      double totalSeconds = (value.ToUniversalTime() - dateTime).TotalSeconds;
      byte[] numArray = totalSeconds >= 0.0 && totalSeconds < (double) uint.MaxValue ? BitConverter.GetBytes((uint) (totalSeconds + 1.0)) : throw new ArgumentException();
      if (BitConverter.IsLittleEndian)
        numArray = ((IEnumerable<byte>) numArray).Reverse<byte>().ToArray<byte>();
      return new UpdateRequest3Item(UpdateRequest3Tag.Time, (IList<byte>) numArray);
    }

    public static UpdateRequest3Item CreateTimeBetweenReports(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeBetweenReportsHelper(UpdateRequest3Tag.TimeBetweenReports, duration);
    }

    public static UpdateRequest3Item CreateTimeToKeepTrying(TimeSpan duration)
    {
      return UpdateRequest3.CreateTimeToKeepTryingHelper(UpdateRequest3Tag.TimeToKeepTrying, duration);
    }

    public static UpdateRequest3Item CreateTrackingEnabled(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.TrackingEnabled, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateTrackingMethod(UpdateRequest3TrackingMethodValue value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.TrackingMethod, (IList<byte>) new byte[1]
      {
        (byte) value
      });
    }

    public static UpdateRequest3Item CreateTrackingProfileHeader(byte profileNumber)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.TrackingProfileHeader, (IList<byte>) new byte[1]
      {
        profileNumber
      });
    }

    public static UpdateRequest3Item CreateUseAlternateMotionSettings(bool value)
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.UseAlternateMotionSettings, (IList<byte>) new byte[1]
      {
        value ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateZeroizeFirmware()
    {
      return new UpdateRequest3Item(UpdateRequest3Tag.ZeroizeFirmware, (IList<byte>) new byte[0]);
    }

    public static UpdateRequest3Item CreateZeroizeKeys(string coPassword)
    {
      List<byte> data = new List<byte>();
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(coPassword));
      return new UpdateRequest3Item(UpdateRequest3Tag.ZeroizeKeys, (IList<byte>) data);
    }

    private static void ParseAcquisitionTimeout(
      IList<byte> data,
      out TimeSpan acquisitionTime,
      out TimeSpan retryTime)
    {
      acquisitionTime = data.Count == 4 ? new TimeSpan(0, 0, (int) data[0]) : throw new ArgumentException();
      retryTime = new TimeSpan(0, (int) data[1] * 256 + (int) data[2], data[3] != (byte) 0 ? 30 : 0);
    }

    private static void ParseBooleanData(IList<byte> data, out bool value)
    {
      if (data.Count != 1)
        throw new ArgumentException();
      value = data[0] > (byte) 0;
    }

    private static void ParseByteData(IList<byte> data, out byte value)
    {
      value = data.Count == 1 ? data[0] : throw new ArgumentException();
    }

    private static void ParseEmptyData(IList<byte> data)
    {
      if (data.Count != 0)
        throw new ArgumentException();
    }

    private static void ParseKeyHelper(IList<byte> data, out string coPassword, out List<byte> key)
    {
      key = data.Count >= 32 ? data.Take<byte>(32).ToList<byte>() : throw new ArgumentException();
      coPassword = Encoding.GetEncoding(1252).GetString(data.Skip<byte>(32).ToArray<byte>());
      if (coPassword.IndexOf(char.MinValue) != -1)
        throw new ArgumentException();
    }

    private static void ParseMaskData(
      IList<byte> data,
      out bool a,
      out bool b,
      out bool c,
      out bool d,
      out bool e,
      out bool f,
      out bool g,
      out bool h)
    {
      if (data.Count != 1)
        throw new ArgumentException();
      a = ((uint) data[0] & 1U) > 0U;
      b = ((uint) data[0] & 2U) > 0U;
      c = ((uint) data[0] & 4U) > 0U;
      d = ((uint) data[0] & 8U) > 0U;
      e = ((uint) data[0] & 16U) > 0U;
      f = ((uint) data[0] & 32U) > 0U;
      g = ((uint) data[0] & 64U) > 0U;
      h = ((uint) data[0] & 128U) > 0U;
    }

    private static void ParseMinutes(IList<byte> data, out TimeSpan value)
    {
      value = data.Count == 2 ? new TimeSpan(0, (int) data[0] * 256 + (int) data[1], 0) : throw new ArgumentException();
    }

    private static void ParseMinutesAndHalfMinute(IList<byte> data, out TimeSpan value)
    {
      value = data.Count == 3 ? new TimeSpan(0, (int) data[0] * 256 + (int) data[1], data[2] != (byte) 0 ? 30 : 0) : throw new ArgumentException();
    }

    private static void ParseSamePlaceSkipReportsHelper(
      IList<byte> data,
      out UpdateRequest3SamePlaceSkipReportsMode mode,
      out ushort radius,
      out ushort beforeSkip,
      out ushort toSkip)
    {
      mode = data.Count == 7 ? (UpdateRequest3SamePlaceSkipReportsMode) data[0] : throw new ArgumentException();
      radius = (ushort) ((uint) data[1] * 256U + (uint) data[2]);
      beforeSkip = (ushort) ((uint) data[3] * 256U + (uint) data[4]);
      toSkip = (ushort) ((uint) data[5] * 256U + (uint) data[6]);
    }

    private static void ParseTimeToKeepTryingHelper(IList<byte> data, out TimeSpan duration)
    {
      byte num;
      UpdateRequest3.ParseByteData(data, out num);
      duration = new TimeSpan(0, 0, (int) num * 5);
    }

    private static UpdateRequest3Item CreateAcquisitionTimeoutHelper(
      UpdateRequest3Tag tag,
      TimeSpan acquisitionTimeout,
      TimeSpan retryTime)
    {
      if (acquisitionTimeout.Milliseconds != 0 || acquisitionTimeout.TotalSeconds < 0.0 || acquisitionTimeout.TotalSeconds > (double) byte.MaxValue || retryTime.Milliseconds != 0 || retryTime.Seconds != 0 && retryTime.Seconds != 30 || retryTime.TotalMinutes < 0.0 || retryTime > new TimeSpan(0, (int) ushort.MaxValue, 30))
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(tag, (IList<byte>) new byte[4]
      {
        (byte) acquisitionTimeout.TotalSeconds,
        (byte) ((uint) (int) retryTime.TotalMinutes / 256U),
        (byte) ((uint) (int) retryTime.TotalMinutes % 256U),
        retryTime.Seconds == 30 ? (byte) 1 : (byte) 0
      });
    }

    public static UpdateRequest3Item CreateKeyHelper(
      UpdateRequest3Tag tag,
      string coPassword,
      IEnumerable<byte> key)
    {
      if (key.Count<byte>() != 32)
        throw new ArgumentOutOfRangeException();
      List<byte> data = new List<byte>();
      data.AddRange(key);
      data.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(coPassword));
      return new UpdateRequest3Item(tag, (IList<byte>) data);
    }

    private static UpdateRequest3Item CreateSamePlaceSkipReportsHelper(
      UpdateRequest3Tag tag,
      UpdateRequest3SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort beforeSkip,
      ushort toSkip)
    {
      return new UpdateRequest3Item(tag, (IList<byte>) new byte[7]
      {
        (byte) mode,
        (byte) ((uint) radius / 256U),
        (byte) ((uint) radius % 256U),
        (byte) ((uint) beforeSkip / 256U),
        (byte) ((uint) beforeSkip % 256U),
        (byte) ((uint) toSkip / 256U),
        (byte) ((uint) toSkip % 256U)
      });
    }

    private static UpdateRequest3Item CreateTimeBetweenReportsHelper(
      UpdateRequest3Tag tag,
      TimeSpan duration)
    {
      if (duration.Milliseconds != 0 || duration.Seconds != 0 && duration.Seconds != 30 || duration.TotalMinutes < 0.0 || duration > new TimeSpan(0, (int) ushort.MaxValue, 30))
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(tag, (IList<byte>) new byte[3]
      {
        (byte) ((uint) (int) duration.TotalMinutes / 256U),
        (byte) ((uint) (int) duration.TotalMinutes % 256U),
        duration.Seconds == 30 ? (byte) 1 : (byte) 0
      });
    }

    private static UpdateRequest3Item CreateTimeToKeepTryingHelper(
      UpdateRequest3Tag tag,
      TimeSpan duration)
    {
      if (duration.Milliseconds != 0 || duration.TotalSeconds < 0.0 || duration.TotalSeconds > 1275.0)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(tag, (IList<byte>) new byte[1]
      {
        (byte) (duration.TotalSeconds / 5.0)
      });
    }

    private static UpdateRequest3Item CreateMinutesHelper(UpdateRequest3Tag tag, TimeSpan duration)
    {
      if (duration.Milliseconds != 0 || duration.Seconds != 0 || duration.TotalMinutes < 0.0 || duration.TotalMinutes > (double) ushort.MaxValue)
        throw new ArgumentOutOfRangeException();
      return new UpdateRequest3Item(tag, (IList<byte>) new byte[2]
      {
        (byte) (duration.TotalMinutes / 256.0),
        (byte) (duration.TotalMinutes % 256.0)
      });
    }
  }
}
