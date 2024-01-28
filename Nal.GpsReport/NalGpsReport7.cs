// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsReport7
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class NalGpsReport7 : NalGpsReport
  {
    public const int MaxStlSatellies = 4;
    public const int MaxGnssSatellites = 16;
    public const int AnalogSignalsCount = 4;
    private List<StlSatellite> stlSatellites;
    private List<GnssSatellite> gnssSatellites;
    private short[] analogSignals;

    public NalGpsReport7()
    {
      this.stlSatellites = new List<StlSatellite>();
      this.gnssSatellites = new List<GnssSatellite>();
      this.analogSignals = new short[4];
    }

    public DateTime StlTime { get; set; }

    public double StlLatitude { get; set; }

    public double StlLongitude { get; set; }

    public float StlAltitude { get; set; }

    public StlStatus StlStatus { get; set; }

    public ushort StlCovariance { get; set; }

    public List<StlSatellite> StlSatellites => this.stlSatellites;

    public DateTime GnssTime { get; set; }

    public double GnssLatitude { get; set; }

    public double GnssLongitude { get; set; }

    public float GnssAltitude { get; set; }

    public PositionFix GnssFix { get; set; }

    public bool GnssFixGood { get; set; }

    public bool GnssDiffSolution { get; set; }

    public double GnssHdop { get; set; }

    public List<GnssSatellite> GnssSatellites => this.gnssSatellites;

    public DateTime SnapshotTime { get; set; }

    public EmergencyState EmergencyState { get; set; }

    public bool Motion { get; set; }

    public short AccelerationX { get; set; }

    public short AccelerationY { get; set; }

    public short AccelerationZ { get; set; }

    public short[] AnalogSignals => this.analogSignals;

    public byte BatteryPercentage { get; set; }

    public byte IridiumSignal { get; set; }

    public byte InputPins { get; set; }

    public override bool IsEmergency() => this.EmergencyState != 0;

    public override XElement ToXElement()
    {
      return new XElement((XName) "nalGpsReport7", new object[26]
      {
        (object) new XElement((XName) "stlTime", (object) this.StlTime.ToXElementText()),
        (object) new XElement((XName) "stlLat", (object) this.StlLatitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "stlLng", (object) this.StlLongitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "stlAlt", (object) this.StlAltitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "stlStatus", (object) this.GetXElementTextForStlStatus(this.StlStatus)),
        (object) new XElement((XName) "stlCovariance", (object) this.StlCovariance),
        (object) new XElement((XName) "stlSats", (object) this.StlSatellites.Select<StlSatellite, XElement>((Func<StlSatellite, XElement>) (x => new XElement((XName) "sat", new object[3]
        {
          (object) new XElement((XName) "id", (object) x.Id),
          (object) new XElement((XName) "signal", (object) x.Signal),
          (object) new XElement((XName) "random", (object) x.Random)
        })))),
        (object) new XElement((XName) "gnssTime", (object) this.GnssTime.ToXElementText()),
        (object) new XElement((XName) "gnssLat", (object) this.GnssLatitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gnssLng", (object) this.GnssLongitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gnssAlt", (object) this.GnssAltitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gnssFix", (object) this.GnssFix.ToXElementText()),
        (object) new XElement((XName) "gnssFixGood", this.GnssFixGood ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "gnssDiffSol", this.GnssDiffSolution ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "gnssHdop", (object) this.GnssHdop.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XElement((XName) "gnssSats", (object) this.GnssSatellites.Select<GnssSatellite, XElement>((Func<GnssSatellite, XElement>) (x => new XElement((XName) "sat", new object[3]
        {
          (object) new XElement((XName) "net", (object) this.GetXElementTextForGnssNetwork(x.Network)),
          (object) new XElement((XName) "id", (object) x.Id),
          (object) new XElement((XName) "signal", (object) x.Signal)
        })))),
        (object) new XElement((XName) "snapshotTime", (object) this.SnapshotTime.ToXElementText()),
        (object) new XElement((XName) "emerState", (object) this.GetXElementTextForEmergencyState(this.EmergencyState)),
        (object) new XElement((XName) "motion", this.Motion ? (object) "1" : (object) "0"),
        (object) new XElement((XName) "accelX", (object) this.AccelerationX),
        (object) new XElement((XName) "accelY", (object) this.AccelerationY),
        (object) new XElement((XName) "accelZ", (object) this.AccelerationZ),
        (object) new XElement((XName) "analogSignals", (object) ((IEnumerable<short>) this.AnalogSignals).Select<short, XElement>((Func<short, XElement>) (x => new XElement((XName) "signal", (object) x)))),
        (object) new XElement((XName) "battPercentage", (object) this.BatteryPercentage),
        (object) new XElement((XName) "irdSignal", (object) this.IridiumSignal),
        (object) new XElement((XName) "inputPins", (object) this.InputPins)
      });
    }

    public static bool Parse(byte[] data, out NalGpsReport7 report)
    {
      report = (NalGpsReport7) null;
      if (data.Length < 163)
        return false;
      if (data[0] != (byte) 7)
        return false;
      try
      {
        int pos = 8;
        report = new NalGpsReport7();
        NalGpsReport7 reportForLambda = report;
        report.StlTime = NalGpsReport7.ParseTime(data, ref pos);
        report.StlLatitude = NalGpsReport7.ExtractDouble(data, ref pos);
        report.StlLongitude = NalGpsReport7.ExtractDouble(data, ref pos);
        report.StlAltitude = NalGpsReport7.ExtractSingle(data, ref pos);
        report.StlStatus = NalGpsReport7.ParseStlStatus(NalGpsReport7.ExtractByte(data, ref pos));
        report.StlCovariance = NalGpsReport7.ExtractUInt16(data, ref pos);
        report.StlSatellites.AddRange((IEnumerable<StlSatellite>) NalGpsReport7.ExtractStlSatellites(data, ref pos));
        report.GnssTime = NalGpsReport7.ParseTime(data, ref pos);
        report.GnssLatitude = NalGpsReport7.ExtractDouble(data, ref pos);
        report.GnssLongitude = NalGpsReport7.ExtractDouble(data, ref pos);
        report.GnssAltitude = NalGpsReport7.ExtractSingle(data, ref pos);
        report.GnssFix = NalGpsReport7.ParseGnssFix(NalGpsReport7.ExtractBits(data, ref pos, 4));
        report.GnssFixGood = NalGpsReport7.ExtractBoolean(data, ref pos);
        report.GnssDiffSolution = NalGpsReport7.ExtractBoolean(data, ref pos);
        NalGpsReport7.ExtractBits(data, ref pos, 2);
        report.GnssHdop = (double) NalGpsReport7.ExtractUInt16(data, ref pos) / 100.0;
        report.GnssSatellites.AddRange((IEnumerable<GnssSatellite>) NalGpsReport7.ExtractGnssSatellites(data, ref pos));
        report.SnapshotTime = NalGpsReport7.ParseTime(data, ref pos);
        report.EmergencyState = NalGpsReport7.ParseEmergencyState(NalGpsReport7.ExtractBits(data, ref pos, 2));
        report.Motion = NalGpsReport7.ExtractBoolean(data, ref pos);
        NalGpsReport7.ExtractBits(data, ref pos, 5);
        report.AccelerationX = NalGpsReport7.ExtractInt16(data, ref pos);
        report.AccelerationY = NalGpsReport7.ExtractInt16(data, ref pos);
        report.AccelerationZ = NalGpsReport7.ExtractInt16(data, ref pos);
        4.Repeat((Action<int>) (i => reportForLambda.AnalogSignals[i] = NalGpsReport7.ExtractInt16(data, ref pos)));
        report.BatteryPercentage = NalGpsReport7.ExtractByte(data, ref pos);
        report.IridiumSignal = NalGpsReport7.ExtractByte(data, ref pos);
        report.InputPins = NalGpsReport7.ExtractByte(data, ref pos);
        return true;
      }
      catch (NalGpsReport7.ParseException ex)
      {
        report = (NalGpsReport7) null;
      }
      return false;
    }

    private string GetXElementTextForStlStatus(StlStatus status)
    {
      switch (status)
      {
        case StlStatus.NoBurstReceived:
          return "No Burst Received";
        case StlStatus.AwaitingCorrections:
          return "Awaiting Corrections";
        case StlStatus.NotConverged:
          return "Not Converged";
        case StlStatus.Positioning:
          return "Positioning";
        default:
          return "Unknown";
      }
    }

    private string GetXElementTextForGnssNetwork(GnssNetwork id)
    {
      switch (id)
      {
        case GnssNetwork.Gps:
          return "GPS";
        case GnssNetwork.Sbas:
          return "SBAS";
        case GnssNetwork.Galileo:
          return "Galileo";
        case GnssNetwork.BeiDou:
          return "BeiDou";
        case GnssNetwork.Imes:
          return "IMES";
        case GnssNetwork.Qzss:
          return "QZSS";
        case GnssNetwork.Glonass:
          return "Glonass";
        default:
          return "Unknown";
      }
    }

    private string GetXElementTextForEmergencyState(EmergencyState state)
    {
      switch (state)
      {
        case EmergencyState.Not:
          return "Not In";
        case EmergencyState.Unacked:
          return "Unacked";
        case EmergencyState.UseEmerTimings:
          return "Acked (Emergency Timings)";
        case EmergencyState.UseNormalTimings:
          return "Acked (Normal Timings)";
        default:
          return "Unknown";
      }
    }

    private static DateTime ParseTime(byte[] data, ref int pos)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) NalGpsReport7.ExtractUInt64(data, ref pos));
    }

    private static List<StlSatellite> ExtractStlSatellites(byte[] data, ref int pos)
    {
      byte num = NalGpsReport7.ExtractByte(data, ref pos);
      List<StlSatellite> stlSatellites = new List<StlSatellite>();
      for (int index = 0; index < 4; ++index)
      {
        if (index < (int) num)
          stlSatellites.Add(new StlSatellite()
          {
            Id = NalGpsReport7.ExtractByte(data, ref pos),
            Signal = NalGpsReport7.ExtractByte(data, ref pos),
            Random = NalGpsReport7.ExtractUInt32(data, ref pos)
          });
        else
          pos += 48;
      }
      return stlSatellites;
    }

    private static List<GnssSatellite> ExtractGnssSatellites(byte[] data, ref int pos)
    {
      byte num = NalGpsReport7.ExtractByte(data, ref pos);
      List<GnssSatellite> gnssSatellites = new List<GnssSatellite>();
      for (int index = 0; index < 16; ++index)
      {
        if (index < (int) num)
          gnssSatellites.Add(new GnssSatellite()
          {
            Network = NalGpsReport7.ParseGnssNetwork(NalGpsReport7.ExtractByte(data, ref pos)),
            Id = NalGpsReport7.ExtractByte(data, ref pos),
            Signal = NalGpsReport7.ExtractByte(data, ref pos)
          });
        else
          pos += 24;
      }
      return gnssSatellites;
    }

    private static StlStatus ParseStlStatus(byte value)
    {
      switch (value)
      {
        case 0:
          return StlStatus.NoBurstReceived;
        case 1:
          return StlStatus.AwaitingCorrections;
        case 2:
          return StlStatus.NotConverged;
        case 3:
          return StlStatus.Positioning;
        default:
          throw new NalGpsReport7.ParseException("Unknown STL Status");
      }
    }

    private static PositionFix ParseGnssFix(int value)
    {
      switch (value)
      {
        case 0:
          return PositionFix.No;
        case 1:
          return PositionFix.DeadReckoning;
        case 2:
          return PositionFix.TwoD;
        case 3:
          return PositionFix.ThreeD;
        case 4:
          return PositionFix.GpsAndDeadReckoning;
        case 5:
          return PositionFix.TimeOnly;
        default:
          throw new NalGpsReport7.ParseException("Unknown GNSS Fix");
      }
    }

    private static GnssNetwork ParseGnssNetwork(byte value)
    {
      switch (value)
      {
        case 0:
          return GnssNetwork.Gps;
        case 1:
          return GnssNetwork.Sbas;
        case 2:
          return GnssNetwork.Galileo;
        case 3:
          return GnssNetwork.BeiDou;
        case 4:
          return GnssNetwork.Imes;
        case 5:
          return GnssNetwork.Qzss;
        case 6:
          return GnssNetwork.Glonass;
        default:
          throw new NalGpsReport7.ParseException("Unknown GNSS Network");
      }
    }

    private static EmergencyState ParseEmergencyState(int value)
    {
      switch (value)
      {
        case 0:
          return EmergencyState.Not;
        case 1:
          return EmergencyState.Unacked;
        case 2:
          return EmergencyState.UseEmerTimings;
        case 3:
          return EmergencyState.UseNormalTimings;
        default:
          throw new NalGpsReport7.ParseException("Unknown Emergency State");
      }
    }

    private static byte ExtractByte(byte[] data, ref int bitPos)
    {
      NalGpsReport7.AssertByteBoundary(bitPos);
      int index = bitPos / 8;
      bitPos += 8;
      return data[index];
    }

    private static bool ExtractBoolean(byte[] data, ref int bitPos)
    {
      return NalGpsReport7.ExtractBits(data, ref bitPos, 1) == 1;
    }

    private static short ExtractInt16(byte[] data, ref int bitPos)
    {
      return BitConverter.ToInt16(NalGpsReport7.ExtractBytes(data, ref bitPos, 2).ToArray<byte>(), 0);
    }

    private static ushort ExtractUInt16(byte[] data, ref int bitPos)
    {
      return BitConverter.ToUInt16(NalGpsReport7.ExtractBytes(data, ref bitPos, 2).ToArray<byte>(), 0);
    }

    private static uint ExtractUInt32(byte[] data, ref int bitPos)
    {
      return BitConverter.ToUInt32(NalGpsReport7.ExtractBytes(data, ref bitPos, 4).ToArray<byte>(), 0);
    }

    private static ulong ExtractUInt64(byte[] data, ref int bitPos)
    {
      return BitConverter.ToUInt64(NalGpsReport7.ExtractBytes(data, ref bitPos, 8).ToArray<byte>(), 0);
    }

    private static double ExtractDouble(byte[] data, ref int bitPos)
    {
      return BitConverter.ToDouble(NalGpsReport7.ExtractBytes(data, ref bitPos, 8).ToArray<byte>(), 0);
    }

    private static float ExtractSingle(byte[] data, ref int bitPos)
    {
      return BitConverter.ToSingle(NalGpsReport7.ExtractBytes(data, ref bitPos, 4).ToArray<byte>(), 0);
    }

    private static IEnumerable<byte> ExtractBytes(byte[] data, ref int bitPos, int count)
    {
      NalGpsReport7.AssertByteBoundary(bitPos);
      IEnumerable<byte> source = ((IEnumerable<byte>) data).Skip<byte>(bitPos / 8).Take<byte>(count);
      bitPos += count * 8;
      return !BitConverter.IsLittleEndian ? source : source.Reverse<byte>();
    }

    private static int ExtractBits(byte[] data, ref int bitPos, int bitCount)
    {
      int index = bitPos / 8;
      int num = bitPos % 8;
      if (num + bitCount > 8)
        throw new NalGpsReport7.ParseException("Cannot extract bits outside of byte.");
      bitPos += bitCount;
      return (int) data[index] >> num & (1 << bitCount) - 1;
    }

    private static void AssertByteBoundary(int bitPos)
    {
      if (bitPos % 8 != 0)
        throw new NalGpsReport7.ParseException("Double values must start at a byte boundary.");
    }

    private class ParseException : Exception
    {
      public ParseException()
      {
      }

      public ParseException(string message)
        : base(message)
      {
      }
    }
  }
}
