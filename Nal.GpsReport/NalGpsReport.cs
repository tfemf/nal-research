// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.NalGpsReport
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

#nullable disable
namespace Nal.GpsReport
{
  public abstract class NalGpsReport : Nal.GpsReport.GpsReport
  {
    public const ulong My1E19 = 10000000000000000000;
    public const ulong My1E18 = 1000000000000000000;
    public const ulong My1E17 = 100000000000000000;
    public const ulong My1E16 = 10000000000000000;
    public const ulong My1E15 = 1000000000000000;
    public const ulong My1E14 = 100000000000000;
    public const ulong My1E13 = 10000000000000;
    public const ulong My1E12 = 1000000000000;
    public const ulong My1E11 = 100000000000;
    public const ulong My1E10 = 10000000000;
    public const ulong My1E9 = 1000000000;
    public const ulong My1E8 = 100000000;
    public const ulong My1E7 = 10000000;
    public const ulong My1E6 = 1000000;
    public const ulong My1E5 = 100000;
    public const ulong My1E4 = 10000;
    public const ulong My1E3 = 1000;
    public const ulong My1E2 = 100;
    public const ulong My1E1 = 10;

    public static bool Parse(byte[] data, out NalGpsReport report)
    {
      return NalGpsReport.Parse(data, false, 20, out report);
    }

    public static bool Parse(
      byte[] data,
      bool autoSetMilCent,
      int assumedMilCent,
      out NalGpsReport report)
    {
      report = (NalGpsReport) null;
      if (data.Length == 10 || data.Length == 16)
      {
        uint num;
        NalGpsReport.ExtractBits(data, 0, 4, out num);
        Nal10ByteGpsReport0 report1;
        if (num == 0U && Nal10ByteGpsReport0.Parse(data, out report1))
          report = (NalGpsReport) report1;
      }
      else if (data.Length >= 30)
      {
        switch (data[0])
        {
          case 3:
            NalGpsReport3 report2;
            if (NalGpsReport3.Parse(data, autoSetMilCent, assumedMilCent, out report2))
            {
              report = (NalGpsReport) report2;
              break;
            }
            break;
          case 4:
            NalGpsReport4 report3;
            if (NalGpsReport4.Parse(data, out report3))
            {
              report = (NalGpsReport) report3;
              break;
            }
            break;
          case 5:
            NalGpsReport5 report4;
            if (NalGpsReport5.Parse(data, out report4))
            {
              report = (NalGpsReport) report4;
              break;
            }
            break;
          case 6:
            NalGpsReport6 report5;
            if (NalGpsReport6.Parse(data, out report5))
            {
              report = (NalGpsReport) report5;
              break;
            }
            break;
          case 7:
            NalGpsReport7 report6;
            if (NalGpsReport7.Parse(data, out report6))
            {
              report = (NalGpsReport) report6;
              break;
            }
            break;
        }
      }
      return report != null;
    }

    public static ulong ExtractValueAfter(ref ulong extractFrom, ulong startingAt)
    {
      long valueAfter = (long) (extractFrom / startingAt);
      extractFrom %= startingAt;
      return (ulong) valueAfter;
    }

    public static void ExtractBits(byte[] data, int start, int amount, out uint value)
    {
      value = 0U;
      int num = start + amount;
      for (int index = start; index < num; ++index)
      {
        if (((int) data[index / 8] & 1 << index % 8) != 0)
          value += (uint) (1 << index - start);
      }
    }
  }
}
