﻿// Decompiled with JetBrains decompiler
// Type: Nal.GpsReport.PecosP4GpsReport
// Assembly: Nal.GpsReport, Version=2.3.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9970C11E-5893-4F90-9AF9-4BC7D4E4B32C
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.GpsReport.DLL

using System.Xml.Linq;

#nullable disable
namespace Nal.GpsReport
{
  public class PecosP4GpsReport : Nal.GpsReport.GpsReport
  {
    private PecosP4Message pecosMessage;

    public PecosP4GpsReport() => this.pecosMessage = new PecosP4Message();

    public PecosP4GpsReport(PecosP4Message pecosMessage) => this.pecosMessage = pecosMessage;

    public PecosP4Message PecosMessage => this.pecosMessage;

    public override bool IsEmergency() => this.PecosMessage.BrevityCode == 911;

    public override XElement ToXElement()
    {
      XElement xelement = this.pecosMessage.ToXElement();
      xelement.Name = (XName) "pecosP4GpsReport";
      return xelement;
    }
  }
}
