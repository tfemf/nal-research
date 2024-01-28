// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.ProjectInstaller
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  [RunInstaller(true)]
  public class ProjectInstaller : Installer
  {
    private IContainer components;
    private ServiceProcessInstaller serviceProcessInstaller;
    private ServiceInstaller serviceInstaller;

    public ProjectInstaller() => this.InitializeComponent();

    public override void Commit(IDictionary savedState)
    {
      base.Commit(savedState);
      FileSystemAccessRule rule = new FileSystemAccessRule((IdentityReference) new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, (SecurityIdentifier) null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
      string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\NAL\\Server for Trackers Service";
      DirectorySecurity accessControl = Directory.GetAccessControl(path);
      accessControl.AddAccessRule(rule);
      Directory.SetAccessControl(path, accessControl);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.serviceProcessInstaller = new ServiceProcessInstaller();
      this.serviceInstaller = new ServiceInstaller();
      this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
      this.serviceProcessInstaller.Password = (string) null;
      this.serviceProcessInstaller.Username = (string) null;
      this.serviceInstaller.DisplayName = "Server for Trackers Service";
      this.serviceInstaller.ServiceName = "ServerForTrackersService";
      this.serviceInstaller.StartType = ServiceStartMode.Automatic;
      this.Installers.AddRange(new Installer[2]
      {
        (Installer) this.serviceProcessInstaller,
        (Installer) this.serviceInstaller
      });
    }
  }
}
