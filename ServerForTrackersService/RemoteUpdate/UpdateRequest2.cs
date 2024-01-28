// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest2
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace Nal.RemoteUpdate
{
  public class UpdateRequest2 : UpdateRequest
  {
    private string password;
    private DateTime time;
    private bool updateNormalTimings;
    private bool updateTestTimings;
    private bool updateEmergencyTimings;
    private bool updateNormalAwakeTimings;
    private bool updateTestAwakeTimings;
    private bool updateEmergencyAwakeTimings;
    private bool updateNormalCallable;
    private bool updateTestCallable;
    private bool updateEmergencyCallable;
    private bool updateMotionSensorAwakes;
    private bool updateMotionSensorBegin;
    private bool updateMotionSensorEnd;
    private bool updateNormalMotionSensorWait;
    private bool updateTestMotionSensorWait;
    private bool updateEmergencyMotionSensorWait;
    private bool updateNormalSamePlaceSkipReports;
    private bool updateTestSamePlaceSkipReports;
    private bool updateEmergencySamePlaceSkipReports;
    private bool updateDeliveryShortCode;
    private bool updateOutputPinsSetup;
    private bool updateSignalingRepetitions;
    private bool updateBlockInvalidGpsReports;
    private bool updateEmergencyReportFlood;
    private bool updateOutputPinStates;
    private double normalTimeBetweenReports;
    private ushort normalTimeToKeepTrying;
    private double testTimeBetweenReports;
    private ushort testTimeToKeepTrying;
    private double emergencyTimeBetweenReports;
    private ushort emergencyTimeToKeepTrying;
    private double normalAwakeTimeBetweenReports;
    private ushort normalAwakeTimeToKeepTrying;
    private double testAwakeTimeBetweenReports;
    private ushort testAwakeTimeToKeepTrying;
    private double emergencyAwakeTimeBetweenReports;
    private ushort emergencyAwakeTimeToKeepTrying;
    private UpdateRequest2CallableValue normalCallable;
    private UpdateRequest2CallableValue testCallable;
    private UpdateRequest2CallableValue emergencyCallable;
    private bool normalMotionSensorAwakes;
    private bool testMotionSensorAwakes;
    private bool emergencyMotionSensorAwakes;
    private byte motionSensorBeginWindowCount;
    private byte motionSensorBeginSensitivity;
    private byte motionSensorEnd;
    private ushort normalMotionSensorWait;
    private ushort testMotionSensorWait;
    private ushort emergencyMotionSensorWait;
    private UpdateRequest2SamePlaceSkipReportsMode normalSpsrMode;
    private ushort normalSpsrRadius;
    private ushort normalSpsrCyclesBeforeSkipping;
    private ushort normalSpsrCyclesToSkip;
    private UpdateRequest2SamePlaceSkipReportsMode testSpsrMode;
    private ushort testSpsrRadius;
    private ushort testSpsrCyclesBeforeSkipping;
    private ushort testSpsrCyclesToSkip;
    private UpdateRequest2SamePlaceSkipReportsMode emerSpsrMode;
    private ushort emerSpsrRadius;
    private ushort emerSpsrCyclesBeforeSkipping;
    private ushort emerSpsrCyclesToSkip;
    private byte deliveryShortCode;
    private byte signalingPins;
    private bool ignoreTestAndEmergencyPins;
    private byte signalingRepetitions;
    private UpdateRequest2BlockInvalidGpsReportsValue blockInvalidGpsReports;
    private byte emergencyReportFlood;
    private byte outputPinStates;

    public UpdateRequest2() => this.ResetValues();

    public override string Password
    {
      get => this.password;
      set => this.password = value.Length == 8 ? value : throw new ArgumentException();
    }

    public DateTime Time
    {
      get => this.time;
      set => this.time = value.ToUniversalTime();
    }

    public bool UpdateNormalTimings => this.updateNormalTimings;

    public bool UpdateTestTimings => this.updateTestTimings;

    public bool UpdateEmergencyTimings => this.updateEmergencyTimings;

    public bool UpdateNormalAwakeTimings => this.updateNormalAwakeTimings;

    public bool UpdateTestAwakeTimings => this.updateTestAwakeTimings;

    public bool UpdateEmergencyAwakeTimings => this.updateEmergencyAwakeTimings;

    public bool UpdateNormalCallable => this.updateNormalCallable;

    public bool UpdateTestCallable => this.updateTestCallable;

    public bool UpdateEmergencyCallable => this.updateEmergencyCallable;

    public bool UpdateMotionSensorAwakes => this.updateMotionSensorAwakes;

    public bool UpdateMotionSensorBegin => this.updateMotionSensorBegin;

    public bool UpdateMotionSensorEnd => this.updateMotionSensorEnd;

    public bool UpdateNormalMotionSensorWait => this.updateNormalMotionSensorWait;

    public bool UpdateTestMotionSensorWait => this.updateTestMotionSensorWait;

    public bool UpdateEmergencyMotionSensorWait => this.updateEmergencyMotionSensorWait;

    public bool UpdateNormalSamePlaceSkipReports => this.updateNormalSamePlaceSkipReports;

    public bool UpdateTestSamePlaceSkipReports => this.updateTestSamePlaceSkipReports;

    public bool UpdateEmergencySamePlaceSkipReports => this.updateEmergencySamePlaceSkipReports;

    public bool UpdateDeliveryShortCode => this.updateDeliveryShortCode;

    public bool UpdateOutputPinsSetup => this.updateOutputPinsSetup;

    public bool UpdateSignalingRepetitions => this.updateSignalingRepetitions;

    public bool UpdateBlockInvalidGpsReports => this.updateBlockInvalidGpsReports;

    public bool UpdateEmergencyReportFlood => this.updateEmergencyReportFlood;

    public bool UpdateOutputPinStates => this.updateOutputPinStates;

    public double NormalTimeBetweenReports => this.normalTimeBetweenReports;

    public ushort NormalTimeToKeepTrying => this.normalTimeToKeepTrying;

    public double TestTimeBetweenReports => this.testTimeBetweenReports;

    public ushort TestTimeToKeepTrying => this.testTimeToKeepTrying;

    public double EmergencyTimeBetweenReports => this.emergencyTimeBetweenReports;

    public ushort EmergencyTimeToKeepTrying => this.emergencyTimeToKeepTrying;

    public double NormalAwakeTimeBetweenReports => this.normalAwakeTimeBetweenReports;

    public ushort NormalAwakeTimeToKeepTrying => this.normalAwakeTimeToKeepTrying;

    public double TestAwakeTimeBetweenReports => this.testAwakeTimeBetweenReports;

    public ushort TestAwakeTimeToKeepTrying => this.testAwakeTimeToKeepTrying;

    public double EmergencyAwakeTimeBetweenReports => this.emergencyAwakeTimeBetweenReports;

    public ushort EmergencyAwakeTimeToKeepTrying => this.emergencyAwakeTimeToKeepTrying;

    public UpdateRequest2CallableValue NormalCallable => this.normalCallable;

    public UpdateRequest2CallableValue TestCallable => this.testCallable;

    public UpdateRequest2CallableValue EmergencyCallable => this.emergencyCallable;

    public bool NormalMotionSensorAwakes => this.normalMotionSensorAwakes;

    public bool TestMotionSensorAwakes => this.testMotionSensorAwakes;

    public bool EmergencyMotionSensorAwakes => this.emergencyMotionSensorAwakes;

    public byte MotionSensorBeginWindowCount => this.motionSensorBeginWindowCount;

    public byte MotionSensorBeginSensitivity => this.motionSensorBeginSensitivity;

    public byte MotionSensorEnd => this.motionSensorEnd;

    public ushort NormalMotionSensorWait => this.normalMotionSensorWait;

    public ushort TestMotionSensorWait => this.testMotionSensorWait;

    public ushort EmergencyMotionSensorWait => this.emergencyMotionSensorWait;

    public UpdateRequest2SamePlaceSkipReportsMode NormalSamePlaceSkipReportsMode
    {
      get => this.normalSpsrMode;
    }

    public ushort NormalSamePlaceSkipReportsRadius => this.normalSpsrRadius;

    public ushort NormalSamePlaceSkipReportsCyclesBeforeSkipping
    {
      get => this.normalSpsrCyclesBeforeSkipping;
    }

    public ushort NormalSamePlaceSkipReportsCyclesToSkip => this.normalSpsrCyclesToSkip;

    public UpdateRequest2SamePlaceSkipReportsMode TestSamePlaceSkipReportsMode => this.testSpsrMode;

    public ushort TestSamePlaceSkipReportsRadius => this.testSpsrRadius;

    public ushort TestSamePlaceSkipReportsCyclesBeforeSkipping => this.testSpsrCyclesBeforeSkipping;

    public ushort TestSamePlaceSkipReportsCyclesToSkip => this.testSpsrCyclesToSkip;

    public UpdateRequest2SamePlaceSkipReportsMode EmergencySamePlaceSkipReportsMode
    {
      get => this.emerSpsrMode;
    }

    public ushort EmergencySamePlaceSkipReportsRadius => this.emerSpsrRadius;

    public ushort EmergencySamePlaceSkipReportsCyclesBeforeSkipping
    {
      get => this.emerSpsrCyclesBeforeSkipping;
    }

    public ushort EmergencySamePlaceSkipReportsCyclesToSkip => this.emerSpsrCyclesToSkip;

    public byte DeliveryShortCode => this.deliveryShortCode;

    public byte SignalingPins => this.signalingPins;

    public bool IgnoreTestAndEmergencyPins => this.ignoreTestAndEmergencyPins;

    public byte SignalingRepetitions => this.signalingRepetitions;

    public UpdateRequest2BlockInvalidGpsReportsValue BlockInvalidGpsReports
    {
      get => this.blockInvalidGpsReports;
    }

    public byte EmergencyReportFlood => this.emergencyReportFlood;

    public byte OutputPinStates => this.outputPinStates;

    public bool ResponseMatches(UpdateResponse2 response)
    {
      if (!(this.time == response.Request.Time) || this.updateNormalTimings != response.Request.UpdateNormalTimings || this.updateTestTimings != response.Request.UpdateTestTimings || this.updateEmergencyTimings != response.Request.UpdateEmergencyTimings || this.updateNormalAwakeTimings != response.Request.UpdateNormalAwakeTimings || this.updateTestAwakeTimings != response.Request.UpdateTestAwakeTimings || this.updateEmergencyAwakeTimings != response.Request.UpdateEmergencyAwakeTimings || this.updateNormalCallable != response.Request.UpdateNormalCallable || this.updateTestCallable != response.Request.UpdateTestCallable || this.updateEmergencyCallable != response.Request.UpdateEmergencyCallable || this.updateMotionSensorAwakes != response.Request.UpdateMotionSensorAwakes || this.updateMotionSensorBegin != response.Request.UpdateMotionSensorBegin || this.updateMotionSensorEnd != response.Request.UpdateMotionSensorEnd || this.updateNormalMotionSensorWait != response.Request.UpdateNormalMotionSensorWait || this.updateTestMotionSensorWait != response.Request.UpdateTestMotionSensorWait || this.updateEmergencyMotionSensorWait != response.Request.UpdateEmergencyMotionSensorWait || this.updateNormalSamePlaceSkipReports != response.Request.UpdateNormalSamePlaceSkipReports || this.updateTestSamePlaceSkipReports != response.Request.UpdateTestSamePlaceSkipReports || this.updateEmergencySamePlaceSkipReports != response.Request.UpdateEmergencySamePlaceSkipReports || this.updateDeliveryShortCode != response.Request.UpdateDeliveryShortCode || this.updateOutputPinsSetup != response.Request.UpdateOutputPinsSetup || this.updateSignalingRepetitions != response.Request.UpdateSignalingRepetitions || this.updateBlockInvalidGpsReports != response.Request.UpdateBlockInvalidGpsReports || this.updateEmergencyReportFlood != response.Request.UpdateEmergencyReportFlood || this.updateOutputPinStates != response.Request.UpdateOutputPinStates || this.updateNormalTimings && (this.normalTimeBetweenReports != response.Request.NormalTimeBetweenReports || (int) this.normalTimeToKeepTrying != (int) response.Request.NormalTimeToKeepTrying) || this.updateTestTimings && (this.testTimeBetweenReports != response.Request.TestTimeBetweenReports || (int) this.testTimeToKeepTrying != (int) response.Request.TestTimeToKeepTrying) || this.updateEmergencyTimings && (this.emergencyTimeBetweenReports != response.Request.EmergencyTimeBetweenReports || (int) this.emergencyTimeToKeepTrying != (int) response.Request.EmergencyTimeToKeepTrying) || this.updateNormalAwakeTimings && (this.normalAwakeTimeBetweenReports != response.Request.NormalAwakeTimeBetweenReports || (int) this.normalAwakeTimeToKeepTrying != (int) response.Request.NormalAwakeTimeToKeepTrying) || this.updateTestAwakeTimings && (this.testAwakeTimeBetweenReports != response.Request.TestAwakeTimeBetweenReports || (int) this.testAwakeTimeToKeepTrying != (int) response.Request.TestAwakeTimeToKeepTrying) || this.updateEmergencyAwakeTimings && (this.emergencyAwakeTimeBetweenReports != response.Request.EmergencyAwakeTimeBetweenReports || (int) this.emergencyAwakeTimeToKeepTrying != (int) response.Request.EmergencyAwakeTimeToKeepTrying) || this.updateNormalCallable && this.normalCallable != response.Request.NormalCallable || this.updateTestCallable && this.testCallable != response.Request.TestCallable || this.updateEmergencyCallable && this.emergencyCallable != response.Request.EmergencyCallable || this.updateMotionSensorAwakes && (this.normalMotionSensorAwakes != response.Request.NormalMotionSensorAwakes || this.testMotionSensorAwakes != response.Request.TestMotionSensorAwakes || this.emergencyMotionSensorAwakes != response.Request.EmergencyMotionSensorAwakes) || this.updateMotionSensorBegin && ((int) this.motionSensorBeginWindowCount != (int) response.Request.MotionSensorBeginWindowCount || (int) this.motionSensorBeginSensitivity != (int) response.Request.MotionSensorBeginSensitivity) || this.updateMotionSensorEnd && (int) this.motionSensorEnd != (int) response.Request.MotionSensorEnd || this.updateNormalMotionSensorWait && (int) this.normalMotionSensorWait != (int) response.Request.NormalMotionSensorWait || this.updateTestMotionSensorWait && (int) this.testMotionSensorWait != (int) response.Request.TestMotionSensorWait || this.updateEmergencyMotionSensorWait && (int) this.emergencyMotionSensorWait != (int) response.Request.EmergencyMotionSensorWait || this.updateNormalSamePlaceSkipReports && (this.normalSpsrMode != response.Request.NormalSamePlaceSkipReportsMode || (int) this.normalSpsrRadius != (int) response.Request.NormalSamePlaceSkipReportsRadius || (int) this.normalSpsrCyclesBeforeSkipping != (int) response.Request.NormalSamePlaceSkipReportsCyclesBeforeSkipping || (int) this.normalSpsrCyclesToSkip != (int) response.Request.NormalSamePlaceSkipReportsCyclesToSkip) || this.updateTestSamePlaceSkipReports && (this.testSpsrMode != response.Request.TestSamePlaceSkipReportsMode || (int) this.testSpsrRadius != (int) response.Request.TestSamePlaceSkipReportsRadius || (int) this.testSpsrCyclesBeforeSkipping != (int) response.Request.TestSamePlaceSkipReportsCyclesBeforeSkipping || (int) this.testSpsrCyclesToSkip != (int) response.Request.TestSamePlaceSkipReportsCyclesToSkip) || this.updateEmergencySamePlaceSkipReports && (this.emerSpsrMode != response.Request.EmergencySamePlaceSkipReportsMode || (int) this.emerSpsrRadius != (int) response.Request.EmergencySamePlaceSkipReportsRadius || (int) this.emerSpsrCyclesBeforeSkipping != (int) response.Request.EmergencySamePlaceSkipReportsCyclesBeforeSkipping || (int) this.emerSpsrCyclesToSkip != (int) response.Request.EmergencySamePlaceSkipReportsCyclesToSkip) || this.updateDeliveryShortCode && (int) this.deliveryShortCode != (int) response.Request.DeliveryShortCode || this.updateOutputPinsSetup && ((int) this.signalingPins != (int) response.Request.SignalingPins || this.ignoreTestAndEmergencyPins != response.Request.IgnoreTestAndEmergencyPins) || this.updateSignalingRepetitions && (int) this.signalingRepetitions != (int) response.Request.SignalingRepetitions || this.updateBlockInvalidGpsReports && this.blockInvalidGpsReports != response.Request.BlockInvalidGpsReports || this.updateEmergencyReportFlood && (int) this.emergencyReportFlood != (int) response.Request.EmergencyReportFlood)
        return false;
      return !this.updateOutputPinStates || (int) this.outputPinStates == (int) response.Request.OutputPinStates;
    }

    public override byte[] GetUpdate()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 117);
      byteList.Add((byte) 2);
      byteList.Add((byte) ((this.time.Year / 1000 % 10 << 4) + this.time.Year / 100 % 10));
      byteList.Add((byte) ((this.time.Year / 10 % 10 << 4) + this.time.Year % 10));
      byteList.Add((byte) ((this.time.Month / 10 % 10 << 4) + this.time.Month % 10));
      byteList.Add((byte) ((this.time.Day / 10 % 10 << 4) + this.time.Day % 10));
      byteList.Add((byte) ((this.time.Hour / 10 % 10 << 4) + this.time.Hour % 10));
      byteList.Add((byte) ((this.time.Minute / 10 % 10 << 4) + this.time.Minute % 10));
      byteList.Add((byte) ((this.time.Second / 10 % 10 << 4) + this.time.Second % 10));
      byteList.Add((byte) ((this.time.Millisecond / 100 % 10 << 4) + this.time.Millisecond / 10 % 10));
      byteList.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(this.password));
      byteList.Add((byte) ((uint) (ushort) this.normalTimeBetweenReports / 256U));
      byteList.Add((byte) ((uint) (ushort) this.normalTimeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.normalTimeToKeepTrying / 5U));
      byteList.Add((byte) ((uint) (ushort) this.testTimeBetweenReports / 256U));
      byteList.Add((byte) ((uint) (ushort) this.testTimeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.testTimeToKeepTrying / 5U));
      byteList.Add((byte) ((uint) (ushort) this.emergencyTimeBetweenReports / 256U));
      byteList.Add((byte) ((uint) (ushort) this.emergencyTimeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.emergencyTimeToKeepTrying / 5U));
      byteList.Add(this.emergencyReportFlood);
      byteList.Add((byte) ((int) (byte) this.blockInvalidGpsReports & 7 | (this.normalTimeBetweenReports % 1.0 != 0.0 ? 8 : 0) | (this.testTimeBetweenReports % 1.0 != 0.0 ? 16 : 0) | (this.emergencyTimeBetweenReports % 1.0 != 0.0 ? 32 : 0)));
      byteList.Add((byte) ((this.normalMotionSensorAwakes ? 1 : 0) | (this.testMotionSensorAwakes ? 2 : 0) | (this.emergencyMotionSensorAwakes ? 4 : 0)));
      byteList.Add(this.deliveryShortCode);
      byteList.Add(this.outputPinStates);
      byteList.Add(this.motionSensorBeginWindowCount);
      byteList.Add(this.motionSensorBeginSensitivity);
      byteList.Add(this.motionSensorEnd);
      byteList.Add((byte) ((uint) this.normalMotionSensorWait / 256U));
      byteList.Add((byte) ((uint) this.normalMotionSensorWait % 256U));
      byteList.Add((byte) ((uint) this.testMotionSensorWait / 256U));
      byteList.Add((byte) ((uint) this.testMotionSensorWait % 256U));
      byteList.Add((byte) ((uint) this.emergencyMotionSensorWait / 256U));
      byteList.Add((byte) ((uint) this.emergencyMotionSensorWait % 256U));
      byteList.Add((byte) ((uint) (ushort) this.normalAwakeTimeBetweenReports / 256U));
      byteList.Add((byte) ((uint) (ushort) this.normalAwakeTimeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.normalAwakeTimeToKeepTrying / 5U));
      byteList.Add((byte) ((uint) (ushort) this.testAwakeTimeBetweenReports / 256U));
      byteList.Add((byte) ((uint) (ushort) this.testAwakeTimeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.testAwakeTimeToKeepTrying / 5U));
      byteList.Add((byte) ((uint) (ushort) this.emergencyAwakeTimeBetweenReports / 256U));
      byteList.Add((byte) ((uint) (ushort) this.emergencyAwakeTimeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.emergencyAwakeTimeToKeepTrying / 5U));
      byteList.Add((byte) ((this.normalAwakeTimeBetweenReports % 1.0 != 0.0 ? 1 : 0) | (this.testAwakeTimeBetweenReports % 1.0 != 0.0 ? 2 : 0) | (this.emergencyAwakeTimeBetweenReports % 1.0 != 0.0 ? 4 : 0)));
      byteList.Add((byte) ((int) this.signalingPins & (int) sbyte.MaxValue | (this.ignoreTestAndEmergencyPins ? 128 : 0)));
      byteList.Add((byte) this.normalCallable);
      byteList.Add((byte) this.testCallable);
      byteList.Add((byte) this.emergencyCallable);
      byteList.Add(this.signalingRepetitions);
      byteList.Add((byte) this.normalSpsrMode);
      byteList.Add((byte) ((uint) this.normalSpsrRadius / 256U));
      byteList.Add((byte) ((uint) this.normalSpsrRadius % 256U));
      byteList.Add((byte) ((uint) this.normalSpsrCyclesBeforeSkipping / 256U));
      byteList.Add((byte) ((uint) this.normalSpsrCyclesBeforeSkipping % 256U));
      byteList.Add((byte) ((uint) this.normalSpsrCyclesToSkip / 256U));
      byteList.Add((byte) ((uint) this.normalSpsrCyclesToSkip % 256U));
      byteList.Add((byte) this.testSpsrMode);
      byteList.Add((byte) ((uint) this.testSpsrRadius / 256U));
      byteList.Add((byte) ((uint) this.testSpsrRadius % 256U));
      byteList.Add((byte) ((uint) this.testSpsrCyclesBeforeSkipping / 256U));
      byteList.Add((byte) ((uint) this.testSpsrCyclesBeforeSkipping % 256U));
      byteList.Add((byte) ((uint) this.testSpsrCyclesToSkip / 256U));
      byteList.Add((byte) ((uint) this.testSpsrCyclesToSkip % 256U));
      byteList.Add((byte) this.emerSpsrMode);
      byteList.Add((byte) ((uint) this.emerSpsrRadius / 256U));
      byteList.Add((byte) ((uint) this.emerSpsrRadius % 256U));
      byteList.Add((byte) ((uint) this.emerSpsrCyclesBeforeSkipping / 256U));
      byteList.Add((byte) ((uint) this.emerSpsrCyclesBeforeSkipping % 256U));
      byteList.Add((byte) ((uint) this.emerSpsrCyclesToSkip / 256U));
      byteList.Add((byte) ((uint) this.emerSpsrCyclesToSkip % 256U));
      byteList.Add((byte) ((this.updateNormalCallable ? 2 : 0) | (this.updateNormalTimings ? 4 : 0) | (this.updateDeliveryShortCode ? 8 : 0) | (this.updateBlockInvalidGpsReports ? 16 : 0) | (this.updateTestTimings ? 32 : 0) | (this.updateEmergencyTimings ? 64 : 0) | (this.updateEmergencyReportFlood ? 128 : 0)));
      byteList.Add((byte) ((this.updateMotionSensorAwakes ? 1 : 0) | (this.updateTestCallable ? 2 : 0) | (this.updateEmergencyCallable ? 4 : 0) | (this.updateOutputPinStates ? 8 : 0) | (this.updateMotionSensorBegin ? 16 : 0) | (this.updateMotionSensorEnd ? 32 : 0) | (this.updateNormalMotionSensorWait ? 64 : 0) | (this.updateTestMotionSensorWait ? 128 : 0)));
      byteList.Add((byte) ((this.updateEmergencyMotionSensorWait ? 1 : 0) | (this.updateNormalAwakeTimings ? 2 : 0) | (this.updateTestAwakeTimings ? 4 : 0) | (this.updateEmergencyAwakeTimings ? 8 : 0) | (this.updateOutputPinsSetup ? 16 : 0) | (this.updateSignalingRepetitions ? 32 : 0) | (this.updateNormalSamePlaceSkipReports ? 64 : 0) | (this.updateTestSamePlaceSkipReports ? 128 : 0)));
      byteList.Add(this.updateEmergencySamePlaceSkipReports ? (byte) 1 : (byte) 0);
      byte num1 = 0;
      foreach (byte num2 in byteList)
        num1 ^= num2;
      byteList.Add(num1);
      return byteList.ToArray();
    }

    public override void ResetValues()
    {
      this.Password = "12345678";
      this.Time = DateTime.UtcNow;
      this.ExcludeNormalTimings();
      this.ExcludeTestTimings();
      this.ExcludeEmergencyTimings();
      this.ExcludeNormalAwakeTimings();
      this.ExcludeTestAwakeTimings();
      this.ExcludeEmergencyAwakeTimings();
      this.ExcludeNormalCallable();
      this.ExcludeTestCallable();
      this.ExcludeEmergencyCallable();
      this.ExcludeMotionSensorAwakes();
      this.ExcludeMotionSensorBegin();
      this.ExcludeMotionSensorEnd();
      this.ExcludeNormalMotionSensorWait();
      this.ExcludeTestMotionSensorWait();
      this.ExcludeEmergencyMotionSensorWait();
      this.ExcludeNormalSamePlaceSkipReports();
      this.ExcludeTestSamePlaceSkipReports();
      this.ExcludeEmergencySamePlaceSkipReports();
      this.ExcludeDeliveryShortCode();
      this.ExcludeOutputPinsSetup();
      this.ExcludeSignalingRepetitions();
      this.ExcludeBlockInvalidGpsReports();
      this.ExcludeEmergencyReportFlood();
      this.ExcludeOutputPinStates();
    }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "updateRequest2", new object[2]
      {
        (object) new XElement((XName) "time", (object) this.time.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss.fZ", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "password", (object) this.password)
      });
      if (this.updateNormalTimings)
        xelement.Add((object) new XElement((XName) "normalTimings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.normalTimeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.normalTimeToKeepTrying)
        }));
      if (this.updateTestTimings)
        xelement.Add((object) new XElement((XName) "testTimings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.testTimeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.testTimeToKeepTrying)
        }));
      if (this.updateEmergencyTimings)
        xelement.Add((object) new XElement((XName) "emergencyTimings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.emergencyTimeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.emergencyTimeToKeepTrying)
        }));
      if (this.updateNormalAwakeTimings)
        xelement.Add((object) new XElement((XName) "normalAwakeTimings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.normalAwakeTimeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.normalAwakeTimeToKeepTrying)
        }));
      if (this.updateTestAwakeTimings)
        xelement.Add((object) new XElement((XName) "testAwakeTimings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.testAwakeTimeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.testAwakeTimeToKeepTrying)
        }));
      if (this.updateEmergencyAwakeTimings)
        xelement.Add((object) new XElement((XName) "emergencyAwakeTimings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.emergencyAwakeTimeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.emergencyAwakeTimeToKeepTrying)
        }));
      if (this.updateNormalCallable)
        xelement.Add((object) new XElement((XName) "normalCallable", (object) this.normalCallable.GetDescription()));
      if (this.updateTestCallable)
        xelement.Add((object) new XElement((XName) "testCallable", (object) this.testCallable.GetDescription()));
      if (this.updateEmergencyCallable)
        xelement.Add((object) new XElement((XName) "emergencyCallable", (object) this.emergencyCallable.GetDescription()));
      if (this.updateMotionSensorAwakes)
        xelement.Add((object) new XElement((XName) "motionSensorAwakes", new object[3]
        {
          (object) new XElement((XName) "inNormalMode", this.normalMotionSensorAwakes ? (object) "1" : (object) "0"),
          (object) new XElement((XName) "inTestMode", this.testMotionSensorAwakes ? (object) "1" : (object) "0"),
          (object) new XElement((XName) "inEmergencyMode", this.emergencyMotionSensorAwakes ? (object) "1" : (object) "0")
        }));
      if (this.updateMotionSensorBegin)
        xelement.Add((object) new XElement((XName) "motionSensorBegin", new object[2]
        {
          (object) new XElement((XName) "windowCount", (object) this.motionSensorBeginWindowCount),
          (object) new XElement((XName) "sensitivity", (object) this.motionSensorBeginSensitivity)
        }));
      if (this.updateMotionSensorEnd)
        xelement.Add((object) new XElement((XName) "motionSensorEnd", (object) this.motionSensorEnd));
      if (this.updateNormalMotionSensorWait)
        xelement.Add((object) new XElement((XName) "normalMotionSensorWait", (object) this.normalMotionSensorWait));
      if (this.updateTestMotionSensorWait)
        xelement.Add((object) new XElement((XName) "testMotionSensorWait", (object) this.testMotionSensorWait));
      if (this.updateEmergencyMotionSensorWait)
        xelement.Add((object) new XElement((XName) "emergencyMotionSensorWait", (object) this.emergencyMotionSensorWait));
      if (this.updateNormalSamePlaceSkipReports)
        xelement.Add((object) new XElement((XName) "normalSamePlaceSkipReports", new object[4]
        {
          (object) new XElement((XName) "mode", (object) this.normalSpsrMode.GetDescription()),
          (object) new XElement((XName) "radius", (object) this.normalSpsrRadius),
          (object) new XElement((XName) "cyclesBeforeSkipping", (object) this.normalSpsrCyclesBeforeSkipping),
          (object) new XElement((XName) "cyclesToSkip", (object) this.normalSpsrCyclesToSkip)
        }));
      if (this.updateTestSamePlaceSkipReports)
        xelement.Add((object) new XElement((XName) "testSamePlaceSkipReports", new object[4]
        {
          (object) new XElement((XName) "mode", (object) this.testSpsrMode.GetDescription()),
          (object) new XElement((XName) "radius", (object) this.testSpsrRadius),
          (object) new XElement((XName) "cyclesBeforeSkipping", (object) this.testSpsrCyclesBeforeSkipping),
          (object) new XElement((XName) "cyclesToSkip", (object) this.testSpsrCyclesToSkip)
        }));
      if (this.updateEmergencySamePlaceSkipReports)
        xelement.Add((object) new XElement((XName) "emergencySamePlaceSkipReports", new object[4]
        {
          (object) new XElement((XName) "mode", (object) this.emerSpsrMode.GetDescription()),
          (object) new XElement((XName) "radius", (object) this.emerSpsrRadius),
          (object) new XElement((XName) "cyclesBeforeSkipping", (object) this.emerSpsrCyclesBeforeSkipping),
          (object) new XElement((XName) "cyclesToSkip", (object) this.emerSpsrCyclesToSkip)
        }));
      if (this.updateDeliveryShortCode)
        xelement.Add((object) new XElement((XName) "deliveryShortCode", (object) this.deliveryShortCode));
      if (this.updateOutputPinsSetup)
        xelement.Add((object) new XElement((XName) "outputPinsSetup", new object[2]
        {
          (object) new XElement((XName) "signalingPins", (object) this.signalingPins),
          (object) new XElement((XName) "ignoreTestAndEmergencyPins", this.ignoreTestAndEmergencyPins ? (object) "1" : (object) "0")
        }));
      if (this.updateSignalingRepetitions)
        xelement.Add((object) new XElement((XName) "signalingRepetitions", (object) this.signalingRepetitions));
      if (this.updateBlockInvalidGpsReports)
        xelement.Add((object) new XElement((XName) "blockInvalidGpsReports", (object) this.blockInvalidGpsReports.GetDescription()));
      if (this.updateEmergencyReportFlood)
        xelement.Add((object) new XElement((XName) "emergencyReportFlood", (object) this.emergencyReportFlood));
      if (this.updateOutputPinStates)
        xelement.Add((object) new XElement((XName) "outputPinStates", (object) this.outputPinStates));
      return xelement;
    }

    public static bool Parse(byte[] data, out UpdateRequest2 update)
    {
      update = (UpdateRequest2) null;
      if (data.Length == 82 && data[0] == (byte) 117 && data[1] == (byte) 2)
      {
        byte num1 = 0;
        for (int index = 0; index < data.Length - 1; ++index)
          num1 ^= data[index];
        if ((int) num1 == (int) data[data.Length - 1])
        {
          UpdateRequest2 updateRequest2 = new UpdateRequest2();
          try
          {
            updateRequest2.Password = Encoding.GetEncoding(1252).GetString(data, 10, 8);
            byte[] numArray1 = new byte[16];
            int num2 = 0;
            for (int index1 = 2; index1 < 10; ++index1)
            {
              byte[] numArray2 = numArray1;
              int index2 = num2;
              int num3 = index2 + 1;
              int num4 = (int) (byte) ((uint) data[index1] / 16U);
              numArray2[index2] = (byte) num4;
              byte[] numArray3 = numArray1;
              int index3 = num3;
              num2 = index3 + 1;
              int num5 = (int) (byte) ((uint) data[index1] % 16U);
              numArray3[index3] = (byte) num5;
              if (numArray1[num2 - 2] > (byte) 9 || numArray1[num2 - 1] > (byte) 9)
                throw new ArgumentOutOfRangeException();
            }
            updateRequest2.Time = new DateTime((int) numArray1[0] * 1000 + (int) numArray1[1] * 100 + (int) numArray1[2] * 10 + (int) numArray1[3], (int) numArray1[4] * 10 + (int) numArray1[5], (int) numArray1[6] * 10 + (int) numArray1[7], (int) numArray1[8] * 10 + (int) numArray1[9], (int) numArray1[10] * 10 + (int) numArray1[11], (int) numArray1[12] * 10 + (int) numArray1[13], (int) numArray1[14] * 100 + (int) numArray1[15] * 10, DateTimeKind.Utc);
            byte num6 = data[77];
            byte num7 = data[78];
            byte num8 = data[79];
            byte num9 = data[80];
            if (((int) num6 & 4) != 0)
            {
              double timeBetweenReports = (double) ((int) data[18] * 256 + (int) data[19]) + (((int) data[28] & 8) != 0 ? 0.5 : 0.0);
              ushort timeToKeepTrying = (ushort) ((uint) data[20] * 5U);
              updateRequest2.IncludeNormalTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 32) != 0)
            {
              double timeBetweenReports = (double) ((int) data[21] * 256 + (int) data[22]) + (((int) data[28] & 16) != 0 ? 0.5 : 0.0);
              ushort timeToKeepTrying = (ushort) ((uint) data[23] * 5U);
              updateRequest2.IncludeTestTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 64) != 0)
            {
              double timeBetweenReports = (double) ((int) data[24] * 256 + (int) data[25]) + (((int) data[28] & 32) != 0 ? 0.5 : 0.0);
              ushort timeToKeepTrying = (ushort) ((uint) data[26] * 5U);
              updateRequest2.IncludeEmergencyTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num8 & 2) != 0)
            {
              double timeBetweenReports = (double) ((int) data[41] * 256 + (int) data[42]);
              ushort timeToKeepTrying = (ushort) ((uint) data[43] * 5U);
              updateRequest2.IncludeNormalAwakeTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num8 & 4) != 0)
            {
              double timeBetweenReports = (double) ((int) data[44] * 256 + (int) data[45]);
              ushort timeToKeepTrying = (ushort) ((uint) data[46] * 5U);
              updateRequest2.IncludeTestAwakeTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num8 & 8) != 0)
            {
              double timeBetweenReports = (double) ((int) data[47] * 256 + (int) data[48]);
              ushort timeToKeepTrying = (ushort) ((uint) data[49] * 5U);
              updateRequest2.IncludeEmergencyAwakeTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 2) != 0)
              updateRequest2.IncludeNormalCallable((UpdateRequest2CallableValue) data[52]);
            if (((int) num7 & 2) != 0)
              updateRequest2.IncludeTestCallable((UpdateRequest2CallableValue) data[53]);
            if (((int) num7 & 4) != 0)
              updateRequest2.IncludeEmergencyCallable((UpdateRequest2CallableValue) data[54]);
            if (((int) num7 & 1) != 0)
            {
              byte num10 = data[29];
              updateRequest2.IncludeMotionSensorAwakes(((uint) num10 & 1U) > 0U, ((uint) num10 & 2U) > 0U, ((uint) num10 & 4U) > 0U);
            }
            if (((int) num7 & 16) != 0)
              updateRequest2.IncludeMotionSensorBegin(data[32], data[33]);
            if (((int) num7 & 32) != 0)
              updateRequest2.IncludeMotionSensorEnd(data[34]);
            if (((int) num7 & 64) != 0)
              updateRequest2.IncludeNormalMotionSensorWait((ushort) ((uint) data[35] * 256U + (uint) data[36]));
            if (((int) num7 & 128) != 0)
              updateRequest2.IncludeTestMotionSensorWait((ushort) ((uint) data[37] * 256U + (uint) data[38]));
            if (((int) num8 & 1) != 0)
              updateRequest2.IncludeEmergencyMotionSensorWait((ushort) ((uint) data[39] * 256U + (uint) data[40]));
            if (((int) num8 & 64) != 0)
              updateRequest2.IncludeNormalSamePlaceSkipReports((UpdateRequest2SamePlaceSkipReportsMode) data[56], (ushort) ((uint) data[57] * 256U + (uint) data[58]), (ushort) ((uint) data[59] * 256U + (uint) data[60]), (ushort) ((uint) data[61] * 256U + (uint) data[62]));
            if (((int) num8 & 128) != 0)
              updateRequest2.IncludeTestSamePlaceSkipReports((UpdateRequest2SamePlaceSkipReportsMode) data[63], (ushort) ((uint) data[64] * 256U + (uint) data[65]), (ushort) ((uint) data[66] * 256U + (uint) data[67]), (ushort) ((uint) data[68] * 256U + (uint) data[69]));
            if (((int) num9 & 1) != 0)
              updateRequest2.IncludeEmergencySamePlaceSkipReports((UpdateRequest2SamePlaceSkipReportsMode) data[70], (ushort) ((uint) data[71] * 256U + (uint) data[72]), (ushort) ((uint) data[73] * 256U + (uint) data[74]), (ushort) ((uint) data[75] * 256U + (uint) data[76]));
            if (((int) num6 & 8) != 0)
              updateRequest2.IncludeDeliveryShortCode(data[30]);
            if (((int) num8 & 16) != 0)
            {
              byte num11 = data[51];
              updateRequest2.IncludeOutputPinsSetup((byte) ((uint) num11 & (uint) sbyte.MaxValue), ((uint) num11 & 128U) > 0U);
            }
            if (((int) num8 & 32) != 0)
              updateRequest2.IncludeSignalingRepetitions(data[55]);
            if (((int) num6 & 16) != 0)
              updateRequest2.IncludeBlockInvalidGpsReports((UpdateRequest2BlockInvalidGpsReportsValue) ((int) data[28] & 7));
            if (((int) num6 & 128) != 0)
              updateRequest2.IncludeEmergencyReportFlood(data[27]);
            if (((int) num7 & 8) != 0)
              updateRequest2.IncludeOutputPinStates(data[31]);
            update = updateRequest2;
          }
          catch (ArgumentOutOfRangeException ex)
          {
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      return update != null;
    }

    public void IncludeNormalTimings(double timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < 0.0 || timeBetweenReports > 10080.0 && timeBetweenReports != 64801.0 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateNormalTimings = true;
      this.normalTimeBetweenReports = Math.Round(timeBetweenReports * 2.0, MidpointRounding.AwayFromZero) / 2.0;
      this.normalTimeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeTestTimings(double timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < 0.0 || timeBetweenReports > 10080.0 && timeBetweenReports != 64801.0 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateTestTimings = true;
      this.testTimeBetweenReports = Math.Round(timeBetweenReports * 2.0, MidpointRounding.AwayFromZero) / 2.0;
      this.testTimeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeEmergencyTimings(double timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < 0.0 || timeBetweenReports > 10080.0 && timeBetweenReports != 64801.0 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateEmergencyTimings = true;
      this.emergencyTimeBetweenReports = Math.Round(timeBetweenReports * 2.0, MidpointRounding.AwayFromZero) / 2.0;
      this.emergencyTimeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeNormalAwakeTimings(double timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < 0.0 || timeBetweenReports > 10080.0 && timeBetweenReports != 64801.0 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateNormalAwakeTimings = true;
      this.normalAwakeTimeBetweenReports = Math.Round(timeBetweenReports * 2.0, MidpointRounding.AwayFromZero) / 2.0;
      this.normalAwakeTimeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeTestAwakeTimings(double timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < 0.0 || timeBetweenReports > 10080.0 && timeBetweenReports != 64801.0 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateTestAwakeTimings = true;
      this.testAwakeTimeBetweenReports = Math.Round(timeBetweenReports * 2.0, MidpointRounding.AwayFromZero) / 2.0;
      this.testAwakeTimeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeEmergencyAwakeTimings(double timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < 0.0 || timeBetweenReports > 10080.0 && timeBetweenReports != 64801.0 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateEmergencyAwakeTimings = true;
      this.emergencyAwakeTimeBetweenReports = Math.Round(timeBetweenReports * 2.0, MidpointRounding.AwayFromZero) / 2.0;
      this.emergencyAwakeTimeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeNormalCallable(UpdateRequest2CallableValue value)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2CallableValue), (object) value))
        throw new ArgumentException();
      this.updateNormalCallable = true;
      this.normalCallable = value;
    }

    public void IncludeTestCallable(UpdateRequest2CallableValue value)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2CallableValue), (object) value))
        throw new ArgumentException();
      this.updateTestCallable = true;
      this.testCallable = value;
    }

    public void IncludeEmergencyCallable(UpdateRequest2CallableValue value)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2CallableValue), (object) value))
        throw new ArgumentException();
      this.updateEmergencyCallable = true;
      this.emergencyCallable = value;
    }

    public void IncludeMotionSensorAwakes(
      bool inNormalState,
      bool inTestState,
      bool inEmergencyState)
    {
      this.updateMotionSensorAwakes = true;
      this.normalMotionSensorAwakes = inNormalState;
      this.testMotionSensorAwakes = inTestState;
      this.emergencyMotionSensorAwakes = inEmergencyState;
    }

    public void IncludeMotionSensorBegin(byte windowCount, byte sensitivity)
    {
      if (windowCount > (byte) 60 || sensitivity < (byte) 1 || sensitivity > (byte) 26)
        throw new ArgumentException();
      this.updateMotionSensorBegin = true;
      this.motionSensorBeginWindowCount = windowCount;
      this.motionSensorBeginSensitivity = sensitivity;
    }

    public void IncludeMotionSensorEnd(byte duration)
    {
      if (duration < (byte) 1 || duration > (byte) 60)
        throw new ArgumentException();
      this.updateMotionSensorEnd = true;
      this.motionSensorEnd = duration;
    }

    public void IncludeNormalMotionSensorWait(ushort duration)
    {
      this.updateNormalMotionSensorWait = true;
      this.normalMotionSensorWait = duration;
    }

    public void IncludeTestMotionSensorWait(ushort duration)
    {
      this.updateTestMotionSensorWait = true;
      this.testMotionSensorWait = duration;
    }

    public void IncludeEmergencyMotionSensorWait(ushort duration)
    {
      this.updateEmergencyMotionSensorWait = true;
      this.emergencyMotionSensorWait = duration;
    }

    public void IncludeNormalSamePlaceSkipReports(
      UpdateRequest2SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort cyclesBeforeSkipping,
      ushort cyclesToSkip)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2SamePlaceSkipReportsMode), (object) mode) || radius < (ushort) 10 || cyclesToSkip < (ushort) 1)
        throw new ArgumentException();
      this.updateNormalSamePlaceSkipReports = true;
      this.normalSpsrMode = mode;
      this.normalSpsrRadius = radius;
      this.normalSpsrCyclesBeforeSkipping = cyclesBeforeSkipping;
      this.normalSpsrCyclesToSkip = cyclesToSkip;
    }

    public void IncludeTestSamePlaceSkipReports(
      UpdateRequest2SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort cyclesBeforeSkipping,
      ushort cyclesToSkip)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2SamePlaceSkipReportsMode), (object) mode) || radius < (ushort) 10 || cyclesToSkip < (ushort) 1)
        throw new ArgumentException();
      this.updateTestSamePlaceSkipReports = true;
      this.testSpsrMode = mode;
      this.testSpsrRadius = radius;
      this.testSpsrCyclesBeforeSkipping = cyclesBeforeSkipping;
      this.testSpsrCyclesToSkip = cyclesToSkip;
    }

    public void IncludeEmergencySamePlaceSkipReports(
      UpdateRequest2SamePlaceSkipReportsMode mode,
      ushort radius,
      ushort cyclesBeforeSkipping,
      ushort cyclesToSkip)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2SamePlaceSkipReportsMode), (object) mode) || radius < (ushort) 10 || cyclesToSkip < (ushort) 1)
        throw new ArgumentException();
      this.updateEmergencySamePlaceSkipReports = true;
      this.emerSpsrMode = mode;
      this.emerSpsrRadius = radius;
      this.emerSpsrCyclesBeforeSkipping = cyclesBeforeSkipping;
      this.emerSpsrCyclesToSkip = cyclesToSkip;
    }

    public void IncludeDeliveryShortCode(byte value)
    {
      this.updateDeliveryShortCode = true;
      this.deliveryShortCode = value;
    }

    public void IncludeOutputPinsSetup(byte signalingPins, bool ignoreTestAndEmergencyPins)
    {
      this.updateOutputPinsSetup = true;
      this.signalingPins = (byte) ((uint) signalingPins & (uint) sbyte.MaxValue);
      this.ignoreTestAndEmergencyPins = ignoreTestAndEmergencyPins;
    }

    public void IncludeSignalingRepetitions(byte value)
    {
      if (value < (byte) 1 || value > (byte) 50)
        throw new ArgumentException();
      this.updateSignalingRepetitions = true;
      this.signalingRepetitions = value;
    }

    public void IncludeBlockInvalidGpsReports(UpdateRequest2BlockInvalidGpsReportsValue value)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest2BlockInvalidGpsReportsValue), (object) value))
        throw new ArgumentException();
      this.updateBlockInvalidGpsReports = true;
      this.blockInvalidGpsReports = value;
    }

    public void IncludeEmergencyReportFlood(byte value)
    {
      this.updateEmergencyReportFlood = true;
      this.emergencyReportFlood = value;
    }

    public void IncludeOutputPinStates(byte states)
    {
      this.updateOutputPinStates = true;
      this.outputPinStates = states;
    }

    public void ExcludeNormalTimings() => this.updateNormalTimings = false;

    public void ExcludeTestTimings() => this.updateTestTimings = false;

    public void ExcludeEmergencyTimings() => this.updateEmergencyTimings = false;

    public void ExcludeNormalAwakeTimings() => this.updateNormalAwakeTimings = false;

    public void ExcludeTestAwakeTimings() => this.updateTestAwakeTimings = false;

    public void ExcludeEmergencyAwakeTimings() => this.updateEmergencyAwakeTimings = false;

    public void ExcludeNormalCallable() => this.updateNormalCallable = false;

    public void ExcludeTestCallable() => this.updateTestCallable = false;

    public void ExcludeEmergencyCallable() => this.updateEmergencyCallable = false;

    public void ExcludeMotionSensorAwakes() => this.updateMotionSensorAwakes = false;

    public void ExcludeMotionSensorBegin() => this.updateMotionSensorBegin = false;

    public void ExcludeMotionSensorEnd() => this.updateMotionSensorEnd = false;

    public void ExcludeNormalMotionSensorWait() => this.updateNormalMotionSensorWait = false;

    public void ExcludeTestMotionSensorWait() => this.updateTestMotionSensorWait = false;

    public void ExcludeEmergencyMotionSensorWait() => this.updateEmergencyMotionSensorWait = false;

    public void ExcludeNormalSamePlaceSkipReports()
    {
      this.updateNormalSamePlaceSkipReports = false;
    }

    public void ExcludeTestSamePlaceSkipReports() => this.updateTestSamePlaceSkipReports = false;

    public void ExcludeEmergencySamePlaceSkipReports()
    {
      this.updateEmergencySamePlaceSkipReports = false;
    }

    public void ExcludeDeliveryShortCode() => this.updateDeliveryShortCode = false;

    public void ExcludeOutputPinsSetup() => this.updateOutputPinsSetup = false;

    public void ExcludeSignalingRepetitions() => this.updateSignalingRepetitions = false;

    public void ExcludeBlockInvalidGpsReports() => this.updateBlockInvalidGpsReports = false;

    public void ExcludeEmergencyReportFlood() => this.updateEmergencyReportFlood = false;

    public void ExcludeOutputPinStates() => this.updateOutputPinStates = false;
  }
}
