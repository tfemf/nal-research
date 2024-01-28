// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.ServerForTrackersService
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class ServerForTrackersService : ServiceBase
  {
    private static HiddenForm hiddenForm;
    private IContainer components;

    public ServerForTrackersService() => this.InitializeComponent();

    public static HiddenForm HiddenForm => ServerForTrackersService.hiddenForm;

    protected override void OnStart(string[] args)
    {
      new Thread(new ThreadStart(this.RunMessageLoop))
      {
        Name = "Message Loop"
      }.Start();
    }

    protected override void OnStop() => Application.Exit();

    private void RunMessageLoop()
    {
      ServerForTrackersService.hiddenForm = new HiddenForm();
      Application.Run((Form) ServerForTrackersService.hiddenForm);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.AutoLog = false;
      this.ServiceName = nameof (ServerForTrackersService);
    }
  }
}
