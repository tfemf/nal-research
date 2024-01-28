// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest1
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
  public class UpdateRequest1 : UpdateRequest
  {
    private string password;
    private DateTime time;
    private bool updateNormalTimings;
    private bool updateTestTimings;
    private bool updateEmergencyTimings;
    private bool updateNormalCallable;
    private bool updateTestCallable;
    private bool updateEmergencyCallable;
    private bool updateMotionSensorAwakes;
    private bool updateDeliveryShortCode;
    private bool updateBlockInvalidGpsReports;
    private bool updateEmergencyReportFlood;
    private bool updateOutputPinStates;
    private double normalTimeBetweenReports;
    private ushort normalTimeToKeepTrying;
    private double testTimeBetweenReports;
    private ushort testTimeToKeepTrying;
    private double emergencyTimeBetweenReports;
    private ushort emergencyTimeToKeepTrying;
    private bool normalCallable;
    private bool testCallable;
    private bool emergencyCallable;
    private bool normalMotionSensorAwakes;
    private bool testMotionSensorAwakes;
    private bool emergencyMotionSensorAwakes;
    private byte deliveryShortCode;
    private UpdateRequest1BlockInvalidGpsReportsValue blockInvalidGpsReports;
    private byte emergencyReportFlood;
    private byte outputPinStates;

    public UpdateRequest1() => this.ResetValues();

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

    public bool UpdateNormalCallable => this.updateNormalCallable;

    public bool UpdateTestCallable => this.updateTestCallable;

    public bool UpdateEmergencyCallable => this.updateEmergencyCallable;

    public bool UpdateMotionSensorAwakes => this.updateMotionSensorAwakes;

    public bool UpdateDeliveryShortCode => this.updateDeliveryShortCode;

    public bool UpdateBlockInvalidGpsReports => this.updateBlockInvalidGpsReports;

    public bool UpdateEmergencyReportFlood => this.updateEmergencyReportFlood;

    public bool UpdateOutputPinStates => this.updateOutputPinStates;

    public double NormalTimeBetweenReports => this.normalTimeBetweenReports;

    public ushort NormalTimeToKeepTrying => this.normalTimeToKeepTrying;

    public double TestTimeBetweenReports => this.testTimeBetweenReports;

    public ushort TestTimeToKeepTrying => this.testTimeToKeepTrying;

    public double EmergencyTimeBetweenReports => this.emergencyTimeBetweenReports;

    public ushort EmergencyTimeToKeepTrying => this.emergencyTimeToKeepTrying;

    public bool NormalCallable => this.normalCallable;

    public bool TestCallable => this.testCallable;

    public bool EmergencyCallable => this.emergencyCallable;

    public bool NormalMotionSensorAwakes => this.normalMotionSensorAwakes;

    public bool TestMotionSensorAwakes => this.testMotionSensorAwakes;

    public bool EmergencyMotionSensorAwakes => this.emergencyMotionSensorAwakes;

    public byte DeliveryShortCode => this.deliveryShortCode;

    public UpdateRequest1BlockInvalidGpsReportsValue BlockInvalidGpsReports
    {
      get => this.blockInvalidGpsReports;
    }

    public byte EmergencyReportFlood => this.emergencyReportFlood;

    public byte OutputPinStates => this.outputPinStates;

    public bool ResponseMatches(UpdateResponse1 response)
    {
      if (!(this.time == response.Request.Time) || this.updateNormalTimings != response.Request.UpdateNormalTimings || this.updateEmergencyTimings != response.Request.UpdateEmergencyTimings || this.updateTestTimings != response.Request.UpdateTestTimings || this.updateNormalCallable != response.Request.UpdateNormalCallable || this.updateEmergencyCallable != response.Request.UpdateEmergencyCallable || this.updateTestCallable != response.Request.UpdateTestCallable || this.updateMotionSensorAwakes != response.Request.UpdateMotionSensorAwakes || this.updateDeliveryShortCode != response.Request.UpdateDeliveryShortCode || this.updateBlockInvalidGpsReports != response.Request.UpdateBlockInvalidGpsReports || this.updateEmergencyReportFlood != response.Request.UpdateEmergencyReportFlood || this.updateOutputPinStates != response.Request.UpdateOutputPinStates || this.updateNormalTimings && (this.normalTimeBetweenReports != response.Request.NormalTimeBetweenReports || (int) this.normalTimeToKeepTrying != (int) response.Request.NormalTimeToKeepTrying) || this.updateEmergencyTimings && (this.emergencyTimeBetweenReports != response.Request.EmergencyTimeBetweenReports || (int) this.emergencyTimeToKeepTrying != (int) response.Request.EmergencyTimeToKeepTrying) || this.updateTestTimings && (this.testTimeBetweenReports != response.Request.TestTimeBetweenReports || (int) this.testTimeToKeepTrying != (int) response.Request.TestTimeToKeepTrying) || this.updateNormalCallable && this.normalCallable != response.Request.NormalCallable || this.updateEmergencyCallable && this.emergencyCallable != response.Request.EmergencyCallable || this.updateTestCallable && this.testCallable != response.Request.TestCallable || this.updateMotionSensorAwakes && (this.normalMotionSensorAwakes != response.Request.NormalMotionSensorAwakes || this.emergencyMotionSensorAwakes != response.Request.EmergencyMotionSensorAwakes || this.testMotionSensorAwakes != response.Request.TestMotionSensorAwakes) || this.updateDeliveryShortCode && (int) this.deliveryShortCode != (int) response.Request.DeliveryShortCode || this.updateBlockInvalidGpsReports && this.blockInvalidGpsReports != response.Request.BlockInvalidGpsReports || this.updateEmergencyReportFlood && (int) this.emergencyReportFlood != (int) response.Request.EmergencyReportFlood)
        return false;
      return !this.updateOutputPinStates || (int) this.outputPinStates == (int) response.Request.OutputPinStates;
    }

    public override byte[] GetUpdate()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 117);
      byteList.Add((byte) 1);
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
      byteList.Add((byte) ((int) (byte) this.blockInvalidGpsReports & 7 | (this.normalTimeBetweenReports % 1.0 != 0.0 ? 8 : 0) | (this.testTimeBetweenReports % 1.0 != 0.0 ? 16 : 0) | (this.emergencyTimeBetweenReports % 1.0 != 0.0 ? 32 : 0) | (this.testCallable ? 64 : 0) | (this.emergencyCallable ? 128 : 0)));
      byteList.Add((byte) ((this.normalMotionSensorAwakes ? 1 : 0) | (this.testMotionSensorAwakes ? 2 : 0) | (this.emergencyMotionSensorAwakes ? 4 : 0)));
      byteList.Add(this.deliveryShortCode);
      byteList.Add(this.outputPinStates);
      byteList.Add((byte) ((this.normalCallable ? 1 : 0) | (this.updateNormalCallable ? 2 : 0) | (this.updateNormalTimings ? 4 : 0) | (this.updateDeliveryShortCode ? 8 : 0) | (this.updateBlockInvalidGpsReports ? 16 : 0) | (this.updateTestTimings ? 32 : 0) | (this.updateEmergencyTimings ? 64 : 0) | (this.updateEmergencyReportFlood ? 128 : 0)));
      byteList.Add((byte) ((this.updateMotionSensorAwakes ? 1 : 0) | (this.updateTestCallable ? 2 : 0) | (this.updateEmergencyCallable ? 4 : 0) | (this.updateOutputPinStates ? 8 : 0)));
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
      this.ExcludeNormalCallable();
      this.ExcludeTestCallable();
      this.ExcludeEmergencyCallable();
      this.ExcludeMotionSensorAwakes();
      this.ExcludeDeliveryShortCode();
      this.ExcludeBlockInvalidGpsReports();
      this.ExcludeEmergencyReportFlood();
      this.ExcludeOutputPinStates();
    }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "updateRequest1", new object[2]
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
      if (this.updateNormalCallable)
        xelement.Add((object) new XElement((XName) "normalCallable", this.normalCallable ? (object) "1" : (object) "0"));
      if (this.updateTestCallable)
        xelement.Add((object) new XElement((XName) "testCallable", this.testCallable ? (object) "1" : (object) "0"));
      if (this.updateEmergencyCallable)
        xelement.Add((object) new XElement((XName) "emergencyCallable", this.emergencyCallable ? (object) "1" : (object) "0"));
      if (this.updateMotionSensorAwakes)
        xelement.Add((object) new XElement((XName) "motionSensorAwakes", new object[3]
        {
          (object) new XElement((XName) "inNormalMode", this.normalMotionSensorAwakes ? (object) "1" : (object) "0"),
          (object) new XElement((XName) "inTestMode", this.testMotionSensorAwakes ? (object) "1" : (object) "0"),
          (object) new XElement((XName) "inEmergencyMode", this.emergencyMotionSensorAwakes ? (object) "1" : (object) "0")
        }));
      if (this.updateDeliveryShortCode)
        xelement.Add((object) new XElement((XName) "deliveryShortCode", (object) this.deliveryShortCode));
      if (this.updateBlockInvalidGpsReports)
        xelement.Add((object) new XElement((XName) "blockInvalidGpsReports", (object) this.blockInvalidGpsReports.GetDescription()));
      if (this.updateEmergencyReportFlood)
        xelement.Add((object) new XElement((XName) "emergencyReportFlood", (object) this.emergencyReportFlood));
      if (this.updateOutputPinStates)
        xelement.Add((object) new XElement((XName) "outputPinStates", (object) this.outputPinStates));
      return xelement;
    }

    public static bool Parse(byte[] data, out UpdateRequest1 update)
    {
      update = (UpdateRequest1) null;
      if (data.Length == 35 && data[0] == (byte) 117 && data[1] == (byte) 1)
      {
        byte num1 = 0;
        for (int index = 0; index < data.Length - 1; ++index)
          num1 ^= data[index];
        if ((int) num1 == (int) data[data.Length - 1])
        {
          UpdateRequest1 updateRequest1 = new UpdateRequest1();
          try
          {
            updateRequest1.Password = Encoding.GetEncoding(1252).GetString(data, 10, 8);
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
            updateRequest1.Time = new DateTime((int) numArray1[0] * 1000 + (int) numArray1[1] * 100 + (int) numArray1[2] * 10 + (int) numArray1[3], (int) numArray1[4] * 10 + (int) numArray1[5], (int) numArray1[6] * 10 + (int) numArray1[7], (int) numArray1[8] * 10 + (int) numArray1[9], (int) numArray1[10] * 10 + (int) numArray1[11], (int) numArray1[12] * 10 + (int) numArray1[13], (int) numArray1[14] * 100 + (int) numArray1[15] * 10, DateTimeKind.Utc);
            byte num6 = data[32];
            int num7 = (int) data[33];
            if (((int) num6 & 4) != 0)
            {
              double timeBetweenReports = (double) ((int) data[18] * 256 + (int) data[19]) + (((int) data[28] & 8) != 0 ? 0.5 : 0.0);
              ushort timeToKeepTrying = (ushort) ((uint) data[20] * 5U);
              updateRequest1.IncludeNormalTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 32) != 0)
            {
              double timeBetweenReports = (double) ((int) data[21] * 256 + (int) data[22]) + (((int) data[28] & 16) != 0 ? 0.5 : 0.0);
              ushort timeToKeepTrying = (ushort) ((uint) data[23] * 5U);
              updateRequest1.IncludeTestTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 64) != 0)
            {
              double timeBetweenReports = (double) ((int) data[24] * 256 + (int) data[25]) + (((int) data[28] & 32) != 0 ? 0.5 : 0.0);
              ushort timeToKeepTrying = (ushort) ((uint) data[26] * 5U);
              updateRequest1.IncludeEmergencyTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 2) != 0)
              updateRequest1.IncludeNormalCallable(((uint) num6 & 1U) > 0U);
            if ((num7 & 2) != 0)
              updateRequest1.IncludeTestCallable(((uint) data[28] & 64U) > 0U);
            if ((num7 & 4) != 0)
              updateRequest1.IncludeEmergencyCallable(((uint) data[28] & 128U) > 0U);
            if ((num7 & 1) != 0)
            {
              byte num8 = data[29];
              updateRequest1.IncludeMotionSensorAwakes(((uint) num8 & 1U) > 0U, ((uint) num8 & 2U) > 0U, ((uint) num8 & 4U) > 0U);
            }
            if (((int) num6 & 8) != 0)
              updateRequest1.IncludeDeliveryShortCode(data[30]);
            if (((int) num6 & 16) != 0)
              updateRequest1.IncludeBlockInvalidGpsReports((UpdateRequest1BlockInvalidGpsReportsValue) ((int) data[28] & 7));
            if (((int) num6 & 128) != 0)
              updateRequest1.IncludeEmergencyReportFlood(data[27]);
            if ((num7 & 8) != 0)
              updateRequest1.IncludeOutputPinStates(data[31]);
            update = updateRequest1;
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

    public void IncludeNormalCallable(bool value)
    {
      this.updateNormalCallable = true;
      this.normalCallable = value;
    }

    public void IncludeTestCallable(bool value)
    {
      this.updateTestCallable = true;
      this.testCallable = value;
    }

    public void IncludeEmergencyCallable(bool value)
    {
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

    public void IncludeDeliveryShortCode(byte value)
    {
      this.updateDeliveryShortCode = true;
      this.deliveryShortCode = value;
    }

    public void IncludeBlockInvalidGpsReports(UpdateRequest1BlockInvalidGpsReportsValue value)
    {
      if (!Enum.IsDefined(typeof (UpdateRequest1BlockInvalidGpsReportsValue), (object) value))
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

    public void ExcludeNormalCallable() => this.updateNormalCallable = false;

    public void ExcludeTestCallable() => this.updateTestCallable = false;

    public void ExcludeEmergencyCallable() => this.updateEmergencyCallable = false;

    public void ExcludeMotionSensorAwakes() => this.updateMotionSensorAwakes = false;

    public void ExcludeDeliveryShortCode() => this.updateDeliveryShortCode = false;

    public void ExcludeBlockInvalidGpsReports() => this.updateBlockInvalidGpsReports = false;

    public void ExcludeEmergencyReportFlood() => this.updateEmergencyReportFlood = false;

    public void ExcludeOutputPinStates() => this.updateOutputPinStates = false;
  }
}
