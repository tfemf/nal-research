// Decompiled with JetBrains decompiler
// Type: Nal.RemoteUpdate.UpdateRequest0
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
  public class UpdateRequest0 : UpdateRequest
  {
    private string password;
    private DateTime time;
    private bool updateTimings;
    private bool updateCallable;
    private bool updateDeliveryShortCode;
    private ushort timeBetweenReports;
    private ushort timeToKeepTrying;
    private bool callable;
    private byte deliveryShortCode;

    public UpdateRequest0() => this.ResetValues();

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

    public bool UpdateTimings => this.updateTimings;

    public bool UpdateCallable => this.updateCallable;

    public bool UpdateDeliveryShortCode => this.updateDeliveryShortCode;

    public ushort TimeBetweenReports => this.timeBetweenReports;

    public ushort TimeToKeepTrying => this.timeToKeepTrying;

    public bool Callable => this.callable;

    public byte DeliveryShortCode => this.deliveryShortCode;

    public bool ResponseMatches(UpdateResponse0 response)
    {
      if (!(this.time == response.Request.Time) || this.updateTimings != response.Request.UpdateTimings || this.updateCallable != response.Request.UpdateCallable || this.updateDeliveryShortCode != response.Request.UpdateDeliveryShortCode || this.updateTimings && ((int) this.timeBetweenReports != (int) response.Request.TimeBetweenReports || (int) this.timeToKeepTrying != (int) response.Request.TimeToKeepTrying) || this.updateCallable && this.callable != response.Request.Callable)
        return false;
      return !this.updateDeliveryShortCode || (int) this.deliveryShortCode == (int) response.Request.DeliveryShortCode;
    }

    public override byte[] GetUpdate()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 117);
      byteList.Add((byte) 0);
      byteList.Add((byte) ((this.time.Year / 1000 % 10 << 4) + this.time.Year / 100 % 10));
      byteList.Add((byte) ((this.time.Year / 10 % 10 << 4) + this.time.Year % 10));
      byteList.Add((byte) ((this.time.Month / 10 % 10 << 4) + this.time.Month % 10));
      byteList.Add((byte) ((this.time.Day / 10 % 10 << 4) + this.time.Day % 10));
      byteList.Add((byte) ((this.time.Hour / 10 % 10 << 4) + this.time.Hour % 10));
      byteList.Add((byte) ((this.time.Minute / 10 % 10 << 4) + this.time.Minute % 10));
      byteList.Add((byte) ((this.time.Second / 10 % 10 << 4) + this.time.Second % 10));
      byteList.Add((byte) ((this.time.Millisecond / 100 % 10 << 4) + this.time.Millisecond / 10 % 10));
      byteList.AddRange((IEnumerable<byte>) Encoding.GetEncoding(1252).GetBytes(this.password));
      byteList.Add((byte) ((uint) this.timeBetweenReports / 256U));
      byteList.Add((byte) ((uint) this.timeBetweenReports % 256U));
      byteList.Add((byte) ((uint) this.timeToKeepTrying / 5U));
      byteList.Add(this.deliveryShortCode);
      byteList.Add((byte) ((this.callable ? 1 : 0) | (this.updateCallable ? 2 : 0) | (this.updateTimings ? 4 : 0) | (this.updateDeliveryShortCode ? 8 : 0)));
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
      this.ExcludeTimings();
      this.ExcludeCallable();
      this.ExcludeDeliveryShortCode();
    }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "updateRequest0", new object[2]
      {
        (object) new XElement((XName) "time", (object) this.time.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss.fZ", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "password", (object) this.password)
      });
      if (this.updateTimings)
        xelement.Add((object) new XElement((XName) "timings", new object[2]
        {
          (object) new XElement((XName) "timeBetweenReports", (object) this.timeBetweenReports),
          (object) new XElement((XName) "timeToKeepTrying", (object) this.timeToKeepTrying)
        }));
      if (this.updateCallable)
        xelement.Add((object) new XElement((XName) "callable", this.callable ? (object) "1" : (object) "0"));
      if (this.updateDeliveryShortCode)
        xelement.Add((object) new XElement((XName) "deliveryShortCode", (object) this.deliveryShortCode));
      return xelement;
    }

    public static bool Parse(byte[] data, out UpdateRequest0 update)
    {
      update = (UpdateRequest0) null;
      if (data.Length == 24 && data[0] == (byte) 117 && data[1] == (byte) 0)
      {
        byte num1 = 0;
        for (int index = 0; index < data.Length - 1; ++index)
          num1 ^= data[index];
        if ((int) num1 == (int) data[data.Length - 1])
        {
          UpdateRequest0 updateRequest0 = new UpdateRequest0();
          try
          {
            updateRequest0.Password = Encoding.GetEncoding(1252).GetString(data, 10, 8);
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
            updateRequest0.Time = new DateTime((int) numArray1[0] * 1000 + (int) numArray1[1] * 100 + (int) numArray1[2] * 10 + (int) numArray1[3], (int) numArray1[4] * 10 + (int) numArray1[5], (int) numArray1[6] * 10 + (int) numArray1[7], (int) numArray1[8] * 10 + (int) numArray1[9], (int) numArray1[10] * 10 + (int) numArray1[11], (int) numArray1[12] * 10 + (int) numArray1[13], (int) numArray1[14] * 100 + (int) numArray1[15] * 10, DateTimeKind.Utc);
            byte num6 = data[22];
            if (((int) num6 & 4) != 0)
            {
              ushort timeBetweenReports = (ushort) ((uint) data[18] * 256U + (uint) data[19]);
              ushort timeToKeepTrying = (ushort) ((uint) data[20] * 5U);
              updateRequest0.IncludeTimings(timeBetweenReports, timeToKeepTrying);
            }
            if (((int) num6 & 2) != 0)
              updateRequest0.IncludeCallable(((uint) num6 & 1U) > 0U);
            if (((int) num6 & 8) != 0)
              updateRequest0.IncludeDeliveryShortCode(data[21]);
            update = updateRequest0;
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

    public void IncludeTimings(ushort timeBetweenReports, ushort timeToKeepTrying)
    {
      if (timeBetweenReports < (ushort) 0 || timeBetweenReports > (ushort) 10080 && timeBetweenReports != (ushort) 64801 || timeToKeepTrying < (ushort) 60 && timeToKeepTrying != (ushort) 0 || timeToKeepTrying > (ushort) 1275)
        throw new ArgumentException();
      this.updateTimings = true;
      this.timeBetweenReports = timeBetweenReports;
      this.timeToKeepTrying = timeToKeepTrying;
    }

    public void IncludeCallable(bool value)
    {
      this.updateCallable = true;
      this.callable = value;
    }

    public void IncludeDeliveryShortCode(byte value)
    {
      this.updateDeliveryShortCode = true;
      this.deliveryShortCode = value;
    }

    public void ExcludeTimings() => this.updateTimings = false;

    public void ExcludeCallable() => this.updateCallable = false;

    public void ExcludeDeliveryShortCode() => this.updateDeliveryShortCode = false;
  }
}
