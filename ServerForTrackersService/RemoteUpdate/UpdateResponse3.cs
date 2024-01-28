// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateResponse3
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class UpdateResponse3 : UpdateResponse
  {
    private List<UpdateResponse3Item> items;

    public UpdateResponse3() => this.items = new List<UpdateResponse3Item>();

    public UpdateResponse3Validation Validation { get; set; }

    public List<UpdateResponse3Item> Items => this.items;

    public override XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "results");
      foreach (UpdateResponse3Item updateResponse3Item in this.items)
      {
        XElement content = new XElement((XName) this.GetElementName(updateResponse3Item.Tag), (object) updateResponse3Item.Result.GetDescription());
        xelement.Add((object) content);
        if (updateResponse3Item.Tag == UpdateRequest3Tag.Unknown)
          content.Add((object) new XAttribute((XName) "tagTable", (object) updateResponse3Item.TagTable), (object) new XAttribute((XName) "tagValue", (object) updateResponse3Item.TagValue));
      }
      return new XElement((XName) "updateResponse3", new object[2]
      {
        (object) new XElement((XName) "validation", (object) this.Validation.GetDescription()),
        (object) xelement
      });
    }

    private string GetElementName(UpdateRequest3Tag tag)
    {
      switch (tag)
      {
        case UpdateRequest3Tag.AddCallOutSchedule:
          return "addCallOutSchedule";
        case UpdateRequest3Tag.AddGeofence:
          return "addGeofence";
        case UpdateRequest3Tag.AwakeTimeBetweenReports:
          return "awakeTimeBetweenReports";
        case UpdateRequest3Tag.AwakeTimeToKeepTrying:
          return "awakeTimeToKeepTrying";
        case UpdateRequest3Tag.BlockInvalidGpsReports:
          return "blockInvalidGpsReports";
        case UpdateRequest3Tag.BlockInvalidGpsReportsPerMode:
          return "blockInvalidGpsReportsPerMode";
        case UpdateRequest3Tag.BufferAndSendMissedReports:
          return "bufferAndSendMissedReports";
        case UpdateRequest3Tag.Callable:
          return "callable";
        case UpdateRequest3Tag.ClearLastRemoteUpdateTime:
          return "clearLastDateTime";
        case UpdateRequest3Tag.DataCallDestination:
          return "dataCallDestination";
        case UpdateRequest3Tag.DataLoggingEnabled:
          return "dataLoggingEnabled";
        case UpdateRequest3Tag.EmergencyAcknowledgement:
          return "emergencyAcknowledgement";
        case UpdateRequest3Tag.EmergencyAwakeTimeBetweenReports:
          return "emergencyAwakeTimeBetweenReports";
        case UpdateRequest3Tag.EmergencyAwakeTimeToKeepTrying:
          return "emergencyAwakeTimeToKeepTrying";
        case UpdateRequest3Tag.EmergencyCallable:
          return "emergencyCallable";
        case UpdateRequest3Tag.EmergencyEnabled:
          return "emergencyEnabled";
        case UpdateRequest3Tag.EmergencyMailboxChecksBetweenReportCycles:
          return "emergencyMailboxChecksBetweenReportCycles";
        case UpdateRequest3Tag.EmergencyMotionSensorWait:
          return "emergencyMotionSensorWait";
        case UpdateRequest3Tag.EmergencyReportFlood:
          return "emergencyReportFlood";
        case UpdateRequest3Tag.EmergencySamePlaceSkipReports:
          return "emergencySamePlaceSkipReports";
        case UpdateRequest3Tag.EmergencyTimeBetweenReports:
          return "emergencyTimeBetweenReports";
        case UpdateRequest3Tag.EmergencyTimeToKeepTrying:
          return "emergencyTimeToKeepTrying";
        case UpdateRequest3Tag.GeneralProfileHeader:
          return "generalProfileHeader";
        case UpdateRequest3Tag.GeofenceCheckFrequency:
          return "geofenceCheckFrequency";
        case UpdateRequest3Tag.GsmSendMethod:
          return "gsmSendMethod";
        case UpdateRequest3Tag.GsmTcpConnectionType:
          return "gsmTcpConnectionType";
        case UpdateRequest3Tag.GsmTcpDestination:
          return "gsmTcpDestination";
        case UpdateRequest3Tag.GsmTcpTimeOuts:
          return "gsmTcpTimeOuts";
        case UpdateRequest3Tag.IgnoreTestAndEmergencySwitches:
          return "ignoreTestAndEmergencySwitches";
        case UpdateRequest3Tag.IncludeGpsInMessages:
          return "includeGpsInMessages";
        case UpdateRequest3Tag.InputPin:
          return "inputPin";
        case UpdateRequest3Tag.Links:
          return "links";
        case UpdateRequest3Tag.LinkSwitchTime:
          return "linkSwitchTime";
        case UpdateRequest3Tag.LockPin:
          return "lockPin";
        case UpdateRequest3Tag.MailboxCheckRate:
          return "mailboxCheckRate";
        case UpdateRequest3Tag.MailboxChecksBetweenReportCycles:
          return "mailboxChecksBetweenReportCycles";
        case UpdateRequest3Tag.ModifyGeofence:
          return "modifyGeofence";
        case UpdateRequest3Tag.MotionSensorAwakes:
          return "motionSensorAwakes";
        case UpdateRequest3Tag.MotionSensorAwakesPerMode:
          return "motionSensorAwakesPerMode";
        case UpdateRequest3Tag.MotionSensorBegin:
          return "motionSensorBegin";
        case UpdateRequest3Tag.MotionSensorBeginWithWindowSize:
          return "motionSensorBeginWithWindowSize";
        case UpdateRequest3Tag.MotionSensorEnd:
          return "motionSensorEnd";
        case UpdateRequest3Tag.MotionSensorWait:
          return "motionSensorWait";
        case UpdateRequest3Tag.OutputPinStates:
          return "outputPinStates";
        case UpdateRequest3Tag.PollAntennaStatus:
          return "pollAntennaStatus";
        case UpdateRequest3Tag.PollEmergencyActivationSource:
          return "pollEmergencyActivationSource";
        case UpdateRequest3Tag.PollGsmLastReportTimes:
          return "pollGsmLastReportTimes";
        case UpdateRequest3Tag.PollIridiumLastReportTimes:
          return "pollIridiumLastReportTimes";
        case UpdateRequest3Tag.PollReport:
          return "pollReport";
        case UpdateRequest3Tag.PollStatistics:
          return "pollStatistics";
        case UpdateRequest3Tag.PowerUpDelay:
          return "powerUpDelay";
        case UpdateRequest3Tag.PowerUpTimeout:
          return "powerUpTimeout";
        case UpdateRequest3Tag.QueuedReports:
          return "queuedReports";
        case UpdateRequest3Tag.RemoteUpdateTimeCheckEnabled:
          return "remoteUpdateTimeCheckEnabled";
        case UpdateRequest3Tag.RemoveCallOutSchedule:
          return "removeCallOutSchedule";
        case UpdateRequest3Tag.RemoveGeofence:
          return "removeGeofence";
        case UpdateRequest3Tag.ReportFlood:
          return "reportFlood";
        case UpdateRequest3Tag.ReportFormat:
          return "reportFormat";
        case UpdateRequest3Tag.ResponseMethod:
          return "responseMethod";
        case UpdateRequest3Tag.SamePlaceSkipReports:
          return "samePlaceSkipReports";
        case UpdateRequest3Tag.SignalingPins:
          return "signalingPins";
        case UpdateRequest3Tag.SignalingRepetitions:
          return "signalingRepetitions";
        case UpdateRequest3Tag.SmsDestination:
          return "smsDestination";
        case UpdateRequest3Tag.SmsEmailDestination:
          return "smsEmailDestination";
        case UpdateRequest3Tag.SmsPhoneNumberDestination:
          return "smsPhoneNumberDestination";
        case UpdateRequest3Tag.SmsValidityPeriod:
          return "smsValidityPeriod";
        case UpdateRequest3Tag.StartupProfile:
          return "startupProfile";
        case UpdateRequest3Tag.SuccessfulSendRequired:
          return "successfulSendRequired";
        case UpdateRequest3Tag.TestAwakeTimeBetweenReports:
          return "testAwakeTimeBetweenReports";
        case UpdateRequest3Tag.TestAwakeTimeToKeepTrying:
          return "testAwakeTimeToKeepTrying";
        case UpdateRequest3Tag.TestCallable:
          return "testCallable";
        case UpdateRequest3Tag.TestMotionSensorWait:
          return "testMotionSensorWait";
        case UpdateRequest3Tag.TestReportFlood:
          return "testReportFlood";
        case UpdateRequest3Tag.TestSamePlaceSkipReports:
          return "testSamePlaceSkipReports";
        case UpdateRequest3Tag.TestTimeBetweenReports:
          return "testTimeBetweenReports";
        case UpdateRequest3Tag.TestTimeToKeepTrying:
          return "testTimeToKeepTrying";
        case UpdateRequest3Tag.Time:
          return "time";
        case UpdateRequest3Tag.TimeBetweenReports:
          return "timeBetweenReports";
        case UpdateRequest3Tag.TimeToKeepTrying:
          return "timeToKeepTrying";
        case UpdateRequest3Tag.TrackingEnabled:
          return "trackingEnabled";
        case UpdateRequest3Tag.TrackingMethod:
          return "trackingMethod";
        case UpdateRequest3Tag.TrackingProfileHeader:
          return "trackingProfileHeader";
        default:
          return "unknown";
      }
    }

    public static bool Parse(IList<byte> data, out UpdateResponse3 response)
    {
      response = (UpdateResponse3) null;
      if (data.Count >= 5 && data[0] == (byte) 118 && data[1] == (byte) 3)
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
          if (index1 >= 3 && (int) num1 == (int) data[index1] && (int) num2 == (int) data[index1 + 1])
          {
            num3 = index1;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          UpdateResponse3Validation? nullable1 = new UpdateResponse3Validation?();
          switch (data[2])
          {
            case 0:
              nullable1 = new UpdateResponse3Validation?(UpdateResponse3Validation.Valid);
              break;
            case 1:
              nullable1 = new UpdateResponse3Validation?(UpdateResponse3Validation.InvalidPassword);
              break;
            case 2:
              nullable1 = new UpdateResponse3Validation?(UpdateResponse3Validation.InvalidChecksum);
              break;
            case 4:
              nullable1 = new UpdateResponse3Validation?(UpdateResponse3Validation.LengthError);
              break;
          }
          if (nullable1.HasValue)
          {
            response = new UpdateResponse3();
            response.Validation = nullable1.Value;
            int index2;
            for (int index3 = 3; response != null && index3 < num3; index3 = index2 + 1)
            {
              int tagTable = 0;
              for (; index3 < num3 && data[index3] == byte.MaxValue; ++index3)
                ++tagTable;
              if (index3 + 2 <= num3)
              {
                IList<byte> byteList = data;
                int index4 = index3;
                index2 = index4 + 1;
                byte tagValue = byteList[index4];
                UpdateResponse3ItemResult? nullable2 = new UpdateResponse3ItemResult?();
                switch (data[index2])
                {
                  case 0:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.Success);
                    break;
                  case 1:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.OutOfRange);
                    break;
                  case 2:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.LengthError);
                    break;
                  case 3:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.InvalidProfileHeader);
                    break;
                  case 4:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.NotSupported);
                    break;
                  case 5:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.NoChange);
                    break;
                  case 6:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.Failed);
                    break;
                  case 7:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.Skipped);
                    break;
                  case 8:
                    nullable2 = new UpdateResponse3ItemResult?(UpdateResponse3ItemResult.TimeCheckFailed);
                    break;
                }
                if (!nullable2.HasValue)
                {
                  response = (UpdateResponse3) null;
                  break;
                }
                response.items.Add(new UpdateResponse3Item(tagTable, tagValue, nullable2.Value));
              }
              else
                break;
            }
          }
        }
      }
      return response != null;
    }
  }
}
