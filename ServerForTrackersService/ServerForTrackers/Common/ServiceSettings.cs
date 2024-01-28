// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.ServiceSettings
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.RemoteUpdate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class ServiceSettings
  {
    private List<EmailAccountInfo> emailAccounts;
    private List<ServerModemInfo> serverModems;
    private List<SbdDirectIPAccountInfo> sbdDirectIpAccounts;
    private List<TcpProtocolServerInfo> tcpProtocolServers;
    private List<RudicsAccountInfo> rudicsAccounts;
    private List<LogFile> logFiles;
    private List<LoggingDirectory> loggingDirectories;
    private List<BatchFile> batchFiles;
    private List<Plugin> plugins;
    private List<DataServerInfo> dataServers;

    public ServiceSettings()
    {
      this.emailAccounts = new List<EmailAccountInfo>();
      this.serverModems = new List<ServerModemInfo>();
      this.sbdDirectIpAccounts = new List<SbdDirectIPAccountInfo>();
      this.tcpProtocolServers = new List<TcpProtocolServerInfo>();
      this.rudicsAccounts = new List<RudicsAccountInfo>();
      this.logFiles = new List<LogFile>();
      this.loggingDirectories = new List<LoggingDirectory>();
      this.batchFiles = new List<BatchFile>();
      this.plugins = new List<Plugin>();
      this.dataServers = new List<DataServerInfo>();
    }

    public int ControlServerPort { get; set; }

    public bool ControlServerListen { get; set; }

    public bool UseEncryption { get; set; }

    public bool LogInForEncryption { get; set; }

    public string EncryptionUserName { get; set; }

    public string EncryptionUserPassword { get; set; }

    public List<EmailAccountInfo> EmailAccounts => this.emailAccounts;

    public List<ServerModemInfo> ServerModems => this.serverModems;

    public List<SbdDirectIPAccountInfo> SbdDirectIpAccounts => this.sbdDirectIpAccounts;

    public List<TcpProtocolServerInfo> TcpProtocolServers => this.tcpProtocolServers;

    public List<RudicsAccountInfo> RudicsAccounts => this.rudicsAccounts;

    public List<LogFile> LogFiles => this.logFiles;

    public List<LoggingDirectory> LoggingDirectories => this.loggingDirectories;

    public List<BatchFile> BatchFiles => this.batchFiles;

    public List<Plugin> Plugins => this.plugins;

    public List<DataServerInfo> DataServers => this.dataServers;

    public void Load(string fileName)
    {
      this.emailAccounts.Clear();
      this.serverModems.Clear();
      this.sbdDirectIpAccounts.Clear();
      this.tcpProtocolServers.Clear();
      this.rudicsAccounts.Clear();
      this.logFiles.Clear();
      this.loggingDirectories.Clear();
      this.batchFiles.Clear();
      this.plugins.Clear();
      this.dataServers.Clear();
      string xml = File.ReadAllText(fileName);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      switch (xmlDocument.GetElementAsInt32("/settings/settingsFileVersion", 3))
      {
        case 1:
        case 2:
          for (int index1 = 0; index1 < this.emailAccounts.Count - 1; ++index1)
          {
            int index2 = index1 + 1;
            while (index2 < this.emailAccounts.Count)
            {
              if (this.emailAccounts[index2].ID == this.emailAccounts[index1].ID)
                this.emailAccounts.RemoveAt(index2);
              else
                ++index2;
            }
          }
          for (int index3 = 0; index3 < this.serverModems.Count - 1; ++index3)
          {
            int index4 = index3 + 1;
            while (index4 < this.serverModems.Count)
            {
              if (this.serverModems[index4].ID == this.serverModems[index3].ID)
                this.serverModems.RemoveAt(index4);
              else
                ++index4;
            }
          }
          for (int index5 = 0; index5 < this.sbdDirectIpAccounts.Count - 1; ++index5)
          {
            int index6 = index5 + 1;
            while (index6 < this.sbdDirectIpAccounts.Count)
            {
              if (this.sbdDirectIpAccounts[index6].ID == this.sbdDirectIpAccounts[index5].ID)
                this.sbdDirectIpAccounts.RemoveAt(index6);
              else
                ++index6;
            }
          }
          for (int index7 = 0; index7 < this.tcpProtocolServers.Count - 1; ++index7)
          {
            int index8 = index7 + 1;
            while (index8 < this.tcpProtocolServers.Count)
            {
              if (this.tcpProtocolServers[index8].ID == this.tcpProtocolServers[index7].ID)
                this.tcpProtocolServers.RemoveAt(index8);
              else
                ++index8;
            }
          }
          for (int index9 = 0; index9 < this.rudicsAccounts.Count - 1; ++index9)
          {
            int index10 = index9 + 1;
            while (index10 < this.rudicsAccounts.Count)
            {
              if (this.rudicsAccounts[index10].ID == this.rudicsAccounts[index9].ID)
                this.rudicsAccounts.RemoveAt(index10);
              else
                ++index10;
            }
          }
          for (int index11 = 0; index11 < this.logFiles.Count; ++index11)
          {
            int index12 = index11 + 1;
            while (index12 < this.logFiles.Count)
            {
              if (this.logFiles[index12].File == this.logFiles[index11].File)
                this.logFiles.RemoveAt(index12);
              else
                ++index12;
            }
            this.RemoveDuplicateDataTypeSettings((IList<DataTypeSettings>) this.logFiles[index11].DataTypeSettingsList);
          }
          for (int index13 = 0; index13 < this.loggingDirectories.Count; ++index13)
          {
            int index14 = index13 + 1;
            while (index14 < this.loggingDirectories.Count)
            {
              if (this.loggingDirectories[index14].Directory == this.loggingDirectories[index13].Directory)
                this.loggingDirectories.RemoveAt(index14);
              else
                ++index14;
            }
            this.RemoveDuplicateDataTypeSettings((IList<DataTypeSettings>) this.loggingDirectories[index13].DataTypeSettingsList);
          }
          for (int index15 = 0; index15 < this.batchFiles.Count; ++index15)
          {
            int index16 = index15 + 1;
            while (index16 < this.batchFiles.Count)
            {
              if (this.batchFiles[index16].File == this.batchFiles[index15].File)
                this.batchFiles.RemoveAt(index16);
              else
                ++index16;
            }
            this.RemoveDuplicateDataTypeSettings((IList<DataTypeSettings>) this.batchFiles[index15].DataTypeSettingsList);
          }
          for (int index17 = 0; index17 < this.plugins.Count; ++index17)
          {
            int index18 = index17 + 1;
            while (index18 < this.plugins.Count)
            {
              if (this.plugins[index18].File == this.plugins[index17].File)
                this.plugins.RemoveAt(index18);
              else
                ++index18;
            }
            this.RemoveDuplicateDataTypeSettings((IList<DataTypeSettings>) this.plugins[index17].DataTypeSettingsList);
          }
          for (int index19 = 0; index19 < this.dataServers.Count; ++index19)
          {
            int index20 = index19 + 1;
            while (index20 < this.dataServers.Count)
            {
              if (this.dataServers[index20].Port == this.dataServers[index19].Port)
                this.dataServers.RemoveAt(index20);
              else
                ++index20;
            }
            this.RemoveDuplicateDataTypeSettings((IList<DataTypeSettings>) this.dataServers[index19].DataTypeSettingsList);
          }
          break;
        default:
          this.Version3Load(xmlDocument);
          goto case 1;
      }
    }

    public void Save(string fileName)
    {
      using (XmlWriter writer = XmlWriter.Create(fileName, new XmlWriterSettings()
      {
        Indent = true
      }))
        new XDocument(new object[1]
        {
          (object) new XElement((XName) "settings", new object[17]
          {
            (object) new XAttribute((XName) "settingsFileVersion", (object) "3"),
            (object) this.emailAccounts.Select<EmailAccountInfo, XElement>((Func<EmailAccountInfo, XElement>) (x => new XElement((XName) "emailAccount", new object[19]
            {
              (object) new XElement((XName) "id", (object) x.ID),
              (object) new XElement((XName) "displayName", (object) x.DisplayName),
              (object) new XElement((XName) "autoRtrv", x.AutoRetrieve ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "autoRtrvFreq", (object) x.AutoRetrieveFrequency.ToString()),
              (object) new XElement((XName) "pop3Srv", (object) x.Pop3Server),
              (object) new XElement((XName) "pop3Port", (object) x.Pop3Port.ToString()),
              (object) new XElement((XName) "pop3UserName", (object) x.Pop3UserName),
              (object) new XElement((XName) "pop3Password", (object) x.Pop3Password),
              (object) new XElement((XName) "pop3UseSsl", x.Pop3UseSsl ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "pop3SizeFilter", (object) x.Pop3SizeFilter.ToString()),
              (object) new XElement((XName) "delMail", x.DeleteMailOnServer ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "delAll", x.DeleteAll ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "smtpSrv", (object) x.SmtpServer),
              (object) new XElement((XName) "smtpPort", (object) x.SmtpPort.ToString()),
              (object) new XElement((XName) "fromAddr", (object) x.FromAddress),
              (object) new XElement((XName) "smtpReqAuth", x.SmtpRequiresAuthentication ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "smtpUsePop3Cred", x.SmtpUsePop3Credentials ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "smtpUserName", (object) x.SmtpUserName),
              (object) new XElement((XName) "smtpPassword", (object) x.SmtpPassword)
            }))),
            (object) this.serverModems.Select<ServerModemInfo, XElement>((Func<ServerModemInfo, XElement>) (x => new XElement((XName) "serverModem", new object[11]
            {
              (object) new XElement((XName) "id", (object) x.ID),
              (object) new XElement((XName) "port", (object) x.Port),
              (object) new XElement((XName) "openPort", x.OpenPort ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "portBitsPerSec", (object) x.PortBitsPerSec.ToString()),
              (object) new XElement((XName) "portStopBits", (object) this.FormatStopBits(x.PortStopBits)),
              (object) new XElement((XName) "portParity", (object) x.PortParity.GetDescription()),
              (object) new XElement((XName) "portDataBits", (object) x.PortDataBits.ToString()),
              (object) new XElement((XName) "recvSms", x.RecvSms ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "manageCnx", x.ManageConn ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "autoAnswer", x.AutoAnswer ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "callProtocol", (object) x.CallProtocol.GetDescription())
            }))),
            (object) this.sbdDirectIpAccounts.Select<SbdDirectIPAccountInfo, XElement>((Func<SbdDirectIPAccountInfo, XElement>) (x => new XElement((XName) "sbdDirectIpAccount", new object[7]
            {
              (object) new XElement((XName) "id", (object) x.ID),
              (object) new XElement((XName) "displayName", (object) x.DisplayName),
              (object) new XElement((XName) "serverPort", (object) x.ServerPort.ToString()),
              (object) new XElement((XName) "serverListen", x.ServerListen ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "clientHost", (object) x.ClientHost),
              (object) new XElement((XName) "clientPort", (object) x.ClientPort.ToString()),
              (object) new XElement((XName) "sendMODirectIPAcknowledgement", x.SendMODirectIPAcknowledgement ? (object) "1" : (object) "0")
            }))),
            (object) this.tcpProtocolServers.Select<TcpProtocolServerInfo, XElement>((Func<TcpProtocolServerInfo, XElement>) (x => new XElement((XName) "tcpProtocolServer", new object[4]
            {
              (object) new XElement((XName) "id", (object) x.ID),
              (object) new XElement((XName) "displayName", (object) x.DisplayName),
              (object) new XElement((XName) "serverPort", (object) x.ServerPort.ToString()),
              (object) new XElement((XName) "serverListen", x.ServerListen ? (object) "1" : (object) "0")
            }))),
            (object) this.rudicsAccounts.Select<RudicsAccountInfo, XElement>((Func<RudicsAccountInfo, XElement>) (x => new XElement((XName) "rudicsAccount", new object[12]
            {
              (object) new XElement((XName) "id", (object) x.ID),
              (object) new XElement((XName) "displayName", (object) x.DisplayName),
              (object) new XElement((XName) "enablePipe", x.EnablePipe ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "pipeHost", (object) x.PipeHost),
              (object) new XElement((XName) "pipePort", (object) x.PipePort.ToString()),
              (object) new XElement((XName) "serverPort", (object) x.ServerPort.ToString()),
              (object) new XElement((XName) "serverListen", x.ServerListen ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "clientHandlerCallProtocol", (object) x.ClientHandlerCallProtocol.GetDescription()),
              (object) new XElement((XName) "clientHost", (object) x.ClientHost),
              (object) new XElement((XName) "clientPortsBegin", (object) x.ClientPortsBegin.ToString()),
              (object) new XElement((XName) "clientPortsEnd", (object) x.ClientPortsEnd.ToString()),
              (object) new XElement((XName) "clientCallProtocol", (object) x.ClientCallProtocol.GetDescription())
            }))),
            (object) this.logFiles.Select<LogFile, XElement>((Func<LogFile, XElement>) (x => new XElement((XName) "logFile", new object[6]
            {
              (object) new XElement((XName) "file", (object) x.File),
              (object) new XElement((XName) "includeSbd", x.IncludeSbd ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeSms", x.IncludeSms ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeCalls", x.IncludeCalls ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeTcp", x.IncludeTcp ? (object) "1" : (object) "0"),
              (object) x.DataTypeSettingsList.Select<DataTypeSettings, XElement>((Func<DataTypeSettings, XElement>) (t =>
              {
                XName name = (XName) "dataTypeSettings";
                object[] objArray = new object[3]
                {
                  t.IsCategory ? (object) new XElement((XName) "category", (object) t.DataCategory.GetDescription()) : (object) new XElement((XName) "type", (object) t.DataType.GetDescription()),
                  (object) new XElement((XName) "format", (object) t.Format.GetDescription()),
                  null
                };
                XElement[] xelementArray;
                if (string.IsNullOrEmpty(t.Stylesheet))
                  xelementArray = new XElement[0];
                else
                  xelementArray = new XElement[1]
                  {
                    new XElement((XName) "stylesheet", (object) t.Stylesheet)
                  };
                objArray[2] = (object) xelementArray;
                return new XElement(name, objArray);
              }))
            }))),
            (object) this.loggingDirectories.Select<LoggingDirectory, XElement>((Func<LoggingDirectory, XElement>) (x => new XElement((XName) "loggingDirectory", new object[7]
            {
              (object) new XElement((XName) "directory", (object) x.Directory),
              (object) new XElement((XName) "filePrefix", (object) x.FilePrefix),
              (object) new XElement((XName) "includeSbd", x.IncludeSbd ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeSms", x.IncludeSms ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeCalls", x.IncludeCalls ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeTcp", x.IncludeTcp ? (object) "1" : (object) "0"),
              (object) x.DataTypeSettingsList.Select<DataTypeSettings, XElement>((Func<DataTypeSettings, XElement>) (t =>
              {
                XName name = (XName) "dataTypeSettings";
                object[] objArray = new object[3]
                {
                  t.IsCategory ? (object) new XElement((XName) "category", (object) t.DataCategory.GetDescription()) : (object) new XElement((XName) "type", (object) t.DataType.GetDescription()),
                  (object) new XElement((XName) "format", (object) t.Format.GetDescription()),
                  null
                };
                XElement[] xelementArray;
                if (string.IsNullOrEmpty(t.Stylesheet))
                  xelementArray = new XElement[0];
                else
                  xelementArray = new XElement[1]
                  {
                    new XElement((XName) "stylesheet", (object) t.Stylesheet)
                  };
                objArray[2] = (object) xelementArray;
                return new XElement(name, objArray);
              }))
            }))),
            (object) this.batchFiles.Select<BatchFile, XElement>((Func<BatchFile, XElement>) (x => new XElement((XName) "batchFile", new object[6]
            {
              (object) new XElement((XName) "file", (object) x.File),
              (object) new XElement((XName) "includeSbd", x.IncludeSbd ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeSms", x.IncludeSms ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeCalls", x.IncludeCalls ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeTcp", x.IncludeTcp ? (object) "1" : (object) "0"),
              (object) x.DataTypeSettingsList.Select<DataTypeSettings, XElement>((Func<DataTypeSettings, XElement>) (t =>
              {
                XName name = (XName) "dataTypeSettings";
                object[] objArray = new object[3]
                {
                  t.IsCategory ? (object) new XElement((XName) "category", (object) t.DataCategory.GetDescription()) : (object) new XElement((XName) "type", (object) t.DataType.GetDescription()),
                  (object) new XElement((XName) "format", (object) t.Format.GetDescription()),
                  null
                };
                XElement[] xelementArray;
                if (string.IsNullOrEmpty(t.Stylesheet))
                  xelementArray = new XElement[0];
                else
                  xelementArray = new XElement[1]
                  {
                    new XElement((XName) "stylesheet", (object) t.Stylesheet)
                  };
                objArray[2] = (object) xelementArray;
                return new XElement(name, objArray);
              }))
            }))),
            (object) this.plugins.Select<Plugin, XElement>((Func<Plugin, XElement>) (x => new XElement((XName) "plugin", new object[6]
            {
              (object) new XElement((XName) "file", (object) x.File),
              (object) new XElement((XName) "includeSbd", x.IncludeSbd ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeSms", x.IncludeSms ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeCalls", x.IncludeCalls ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeTcp", x.IncludeTcp ? (object) "1" : (object) "0"),
              (object) x.DataTypeSettingsList.Select<DataTypeSettings, XElement>((Func<DataTypeSettings, XElement>) (t =>
              {
                XName name = (XName) "dataTypeSettings";
                object[] objArray = new object[3]
                {
                  t.IsCategory ? (object) new XElement((XName) "category", (object) t.DataCategory.GetDescription()) : (object) new XElement((XName) "type", (object) t.DataType.GetDescription()),
                  (object) new XElement((XName) "format", (object) t.Format.GetDescription()),
                  null
                };
                XElement[] xelementArray;
                if (string.IsNullOrEmpty(t.Stylesheet))
                  xelementArray = new XElement[0];
                else
                  xelementArray = new XElement[1]
                  {
                    new XElement((XName) "stylesheet", (object) t.Stylesheet)
                  };
                objArray[2] = (object) xelementArray;
                return new XElement(name, objArray);
              }))
            }))),
            (object) this.dataServers.Select<DataServerInfo, XElement>((Func<DataServerInfo, XElement>) (x => new XElement((XName) "dataServer", new object[7]
            {
              (object) new XElement((XName) "port", (object) x.Port.ToString()),
              (object) new XElement((XName) "listen", x.Listen ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeSbd", x.IncludeSbd ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeSms", x.IncludeSms ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeCalls", x.IncludeCalls ? (object) "1" : (object) "0"),
              (object) new XElement((XName) "includeTcp", x.IncludeTcp ? (object) "1" : (object) "0"),
              (object) x.DataTypeSettingsList.Select<DataTypeSettings, XElement>((Func<DataTypeSettings, XElement>) (t =>
              {
                XName name = (XName) "dataTypeSettings";
                object[] objArray = new object[3]
                {
                  t.IsCategory ? (object) new XElement((XName) "category", (object) t.DataCategory.GetDescription()) : (object) new XElement((XName) "type", (object) t.DataType.GetDescription()),
                  (object) new XElement((XName) "format", (object) t.Format.GetDescription()),
                  null
                };
                XElement[] xelementArray;
                if (string.IsNullOrEmpty(t.Stylesheet))
                  xelementArray = new XElement[0];
                else
                  xelementArray = new XElement[1]
                  {
                    new XElement((XName) "stylesheet", (object) t.Stylesheet)
                  };
                objArray[2] = (object) xelementArray;
                return new XElement(name, objArray);
              }))
            }))),
            (object) new XElement((XName) "controlServerListen", this.ControlServerListen ? (object) "1" : (object) "0"),
            (object) new XElement((XName) "controlServerPort", (object) this.ControlServerPort.ToString()),
            (object) new XElement((XName) "useEncryption", this.UseEncryption ? (object) "1" : (object) "0"),
            (object) new XElement((XName) "logInForEncryption", this.LogInForEncryption ? (object) "1" : (object) "0"),
            (object) new XElement((XName) "encryptionUserName", (object) this.EncryptToHexString(this.EncryptionUserName)),
            (object) new XElement((XName) "encryptionPassword", (object) this.EncryptToHexString(this.EncryptionUserPassword))
          })
        }).WriteTo(writer);
    }

    private void Version3Load(XmlDocument xmlDoc)
    {
      this.ControlServerPort = xmlDoc.GetElementAsInt32("/settings/controlServerPort", 0);
      this.ControlServerListen = xmlDoc.GetElementAsBoolean("/settings/controlServerListen", false);
      this.UseEncryption = xmlDoc.GetElementAsBoolean("/settings/useEncryption", false);
      this.LogInForEncryption = xmlDoc.GetElementAsBoolean("/settings/logInForEncryption", false);
      this.EncryptionUserName = this.DecryptFromHexString(xmlDoc.GetElementAsString("/settings/encryptionUserName", string.Empty));
      this.EncryptionUserPassword = this.DecryptFromHexString(xmlDoc.GetElementAsString("/settings/encryptionPassword", string.Empty));
      foreach (XmlNode selectNode in xmlDoc.SelectNodes("/settings/emailAccount"))
      {
        EmailAccountInfo emailAccountInfo = new EmailAccountInfo();
        emailAccountInfo.ID = selectNode.GetElementAsString("id", string.Empty);
        emailAccountInfo.DisplayName = selectNode.GetElementAsString("displayName", string.Empty);
        emailAccountInfo.AutoRetrieve = selectNode.GetElementAsBoolean("autoRtrv", false);
        emailAccountInfo.AutoRetrieveFrequency = selectNode.GetElementAsInt32("autoRtrvFreq", 0);
        emailAccountInfo.Pop3Server = selectNode.GetElementAsString("pop3Srv", string.Empty);
        emailAccountInfo.Pop3Port = selectNode.GetElementAsInt32("pop3Port", 0);
        emailAccountInfo.Pop3UserName = selectNode.GetElementAsString("pop3UserName", string.Empty);
        emailAccountInfo.Pop3Password = selectNode.GetElementAsString("pop3Password", string.Empty);
        emailAccountInfo.Pop3UseSsl = selectNode.GetElementAsBoolean("pop3UseSsl", false);
        emailAccountInfo.Pop3SizeFilter = selectNode.GetElementAsInt32("pop3SizeFilter", 6000);
        emailAccountInfo.DeleteMailOnServer = selectNode.GetElementAsBoolean("delMail", false);
        emailAccountInfo.DeleteAll = selectNode.GetElementAsBoolean("delAll", false);
        emailAccountInfo.SmtpServer = selectNode.GetElementAsString("smtpSrv", string.Empty);
        emailAccountInfo.SmtpPort = selectNode.GetElementAsInt32("smtpPort", 0);
        emailAccountInfo.FromAddress = selectNode.GetElementAsString("fromAddr", string.Empty);
        emailAccountInfo.SmtpRequiresAuthentication = selectNode.GetElementAsBoolean("smtpReqAuth", false);
        emailAccountInfo.SmtpUsePop3Credentials = selectNode.GetElementAsBoolean("smtpUsePop3Cred", false);
        emailAccountInfo.SmtpUserName = selectNode.GetElementAsString("smtpUserName", string.Empty);
        emailAccountInfo.SmtpPassword = selectNode.GetElementAsString("smtpPassword", string.Empty);
        this.emailAccounts.Add(emailAccountInfo);
      }
      foreach (XmlNode selectNode in xmlDoc.SelectNodes("/settings/serverModem"))
      {
        try
        {
          ServerModemInfo serverModemInfo = new ServerModemInfo();
          serverModemInfo.ID = selectNode.GetElementAsString("id", string.Empty);
          serverModemInfo.Port = selectNode.GetElementAsString("port", string.Empty);
          serverModemInfo.OpenPort = selectNode.GetElementAsBoolean("openPort", false);
          serverModemInfo.PortBitsPerSec = selectNode.GetElementAsInt32("portBitsPerSec", 19200);
          serverModemInfo.PortStopBits = selectNode.GetElementAsEnum("portStopBits");
          serverModemInfo.PortParity = selectNode.GetElementAsEnum<Parity>("portParity");
          serverModemInfo.PortDataBits = selectNode.GetElementAsInt32("portDataBits", 8);
          serverModemInfo.RecvSms = selectNode.GetElementAsBoolean("recvSms", false);
          serverModemInfo.ManageConn = selectNode.GetElementAsBoolean("manageCnx", false);
          serverModemInfo.AutoAnswer = selectNode.GetElementAsBoolean("autoAnswer", false);
          serverModemInfo.CallProtocol = selectNode.GetElementAsEnum<CallProtocol>("callProtocol");
          this.serverModems.Add(serverModemInfo);
        }
        catch (GetElementAsException ex)
        {
        }
      }
      foreach (XmlNode selectNode in xmlDoc.SelectNodes("/settings/sbdDirectIpAccount"))
      {
        SbdDirectIPAccountInfo directIpAccountInfo = new SbdDirectIPAccountInfo();
        directIpAccountInfo.ID = selectNode.GetElementAsString("id", string.Empty);
        directIpAccountInfo.DisplayName = selectNode.GetElementAsString("displayName", string.Empty);
        directIpAccountInfo.ServerPort = selectNode.GetElementAsInt32("serverPort", 0);
        directIpAccountInfo.ServerListen = selectNode.GetElementAsBoolean("serverListen", false);
        directIpAccountInfo.ClientHost = selectNode.GetElementAsString("clientHost", string.Empty);
        directIpAccountInfo.ClientPort = selectNode.GetElementAsInt32("clientPort", 0);
        directIpAccountInfo.SendMODirectIPAcknowledgement = selectNode.GetElementAsBoolean("sendMODirectIPAcknowledgement", false);
        this.sbdDirectIpAccounts.Add(directIpAccountInfo);
      }
      foreach (XmlNode selectNode in xmlDoc.SelectNodes("/settings/tcpProtocolServer"))
      {
        TcpProtocolServerInfo protocolServerInfo = new TcpProtocolServerInfo();
        protocolServerInfo.ID = selectNode.GetElementAsString("id", string.Empty);
        protocolServerInfo.DisplayName = selectNode.GetElementAsString("displayName", string.Empty);
        protocolServerInfo.ServerPort = selectNode.GetElementAsInt32("serverPort", 0);
        protocolServerInfo.ServerListen = selectNode.GetElementAsBoolean("serverListen", false);
        this.tcpProtocolServers.Add(protocolServerInfo);
      }
      foreach (XmlNode selectNode in xmlDoc.SelectNodes("/settings/rudicsAccount"))
      {
        try
        {
          RudicsAccountInfo rudicsAccountInfo = new RudicsAccountInfo();
          rudicsAccountInfo.ID = selectNode.GetElementAsString("id", string.Empty);
          rudicsAccountInfo.DisplayName = selectNode.GetElementAsString("displayName", string.Empty);
          rudicsAccountInfo.EnablePipe = selectNode.GetElementAsBoolean("enablePipe", false);
          rudicsAccountInfo.PipeHost = selectNode.GetElementAsString("pipeHost", string.Empty);
          rudicsAccountInfo.PipePort = selectNode.GetElementAsInt32("pipePort", 0);
          rudicsAccountInfo.ServerPort = selectNode.GetElementAsInt32("serverPort", 0);
          rudicsAccountInfo.ServerListen = selectNode.GetElementAsBoolean("serverListen", false);
          rudicsAccountInfo.ClientHandlerCallProtocol = selectNode.GetElementAsEnum<CallProtocol>("clientHandlerCallProtocol");
          rudicsAccountInfo.ClientHost = selectNode.GetElementAsString("clientHost", string.Empty);
          rudicsAccountInfo.ClientPortsBegin = selectNode.GetElementAsInt32("clientPortsBegin", 0);
          rudicsAccountInfo.ClientPortsEnd = selectNode.GetElementAsInt32("clientPortsEnd", 0);
          rudicsAccountInfo.ClientCallProtocol = selectNode.GetElementAsEnum<CallProtocol>("clientCallProtocol");
          this.rudicsAccounts.Add(rudicsAccountInfo);
        }
        catch (GetElementAsException ex)
        {
        }
      }
      foreach (XmlNode selectNode1 in xmlDoc.SelectNodes("/settings/logFile"))
      {
        LogFile logFile = new LogFile();
        logFile.File = selectNode1.GetElementAsString("file", string.Empty);
        logFile.IncludeSbd = selectNode1.GetElementAsBoolean("includeSbd", false);
        logFile.IncludeSms = selectNode1.GetElementAsBoolean("includeSms", false);
        logFile.IncludeCalls = selectNode1.GetElementAsBoolean("includeCalls", false);
        logFile.IncludeTcp = selectNode1.GetElementAsBoolean("includeTcp", false);
        foreach (XmlNode selectNode2 in selectNode1.SelectNodes("dataTypeSettings"))
        {
          try
          {
            DataTypeSettings dataTypeSettings = new DataTypeSettings();
            XmlNode node = selectNode2.SelectSingleNode("type");
            if (node != null)
            {
              dataTypeSettings.IsCategory = false;
              dataTypeSettings.DataType = node.GetAsEnum<DataType>();
            }
            else
            {
              dataTypeSettings.IsCategory = true;
              dataTypeSettings.DataCategory = selectNode2.GetElementAsEnum<DataCategory>("category");
            }
            dataTypeSettings.Format = selectNode2.GetElementAsEnum<LoggingFormat>("format");
            dataTypeSettings.Stylesheet = selectNode2.GetElementAsString("stylesheet", string.Empty);
            logFile.DataTypeSettingsList.Add(dataTypeSettings);
          }
          catch (GetElementAsException ex)
          {
          }
        }
        this.logFiles.Add(logFile);
      }
      foreach (XmlNode selectNode3 in xmlDoc.SelectNodes("/settings/loggingDirectory"))
      {
        LoggingDirectory loggingDirectory = new LoggingDirectory();
        loggingDirectory.Directory = selectNode3.GetElementAsString("directory", string.Empty);
        loggingDirectory.FilePrefix = selectNode3.GetElementAsString("filePrefix", string.Empty);
        loggingDirectory.IncludeSbd = selectNode3.GetElementAsBoolean("includeSbd", false);
        loggingDirectory.IncludeSms = selectNode3.GetElementAsBoolean("includeSms", false);
        loggingDirectory.IncludeCalls = selectNode3.GetElementAsBoolean("includeCalls", false);
        loggingDirectory.IncludeTcp = selectNode3.GetElementAsBoolean("includeTcp", false);
        foreach (XmlNode selectNode4 in selectNode3.SelectNodes("dataTypeSettings"))
        {
          try
          {
            DataTypeSettings dataTypeSettings = new DataTypeSettings();
            XmlNode node = selectNode4.SelectSingleNode("type");
            if (node != null)
            {
              dataTypeSettings.IsCategory = false;
              dataTypeSettings.DataType = node.GetAsEnum<DataType>();
            }
            else
            {
              dataTypeSettings.IsCategory = true;
              dataTypeSettings.DataCategory = selectNode4.GetElementAsEnum<DataCategory>("category");
            }
            dataTypeSettings.Format = selectNode4.GetElementAsEnum<LoggingFormat>("format");
            dataTypeSettings.Stylesheet = selectNode4.GetElementAsString("stylesheet", string.Empty);
            loggingDirectory.DataTypeSettingsList.Add(dataTypeSettings);
          }
          catch (GetElementAsException ex)
          {
          }
        }
        this.loggingDirectories.Add(loggingDirectory);
      }
      foreach (XmlNode selectNode5 in xmlDoc.SelectNodes("/settings/batchFile"))
      {
        BatchFile batchFile = new BatchFile();
        batchFile.File = selectNode5.GetElementAsString("file", string.Empty);
        batchFile.IncludeSbd = selectNode5.GetElementAsBoolean("includeSbd", false);
        batchFile.IncludeSms = selectNode5.GetElementAsBoolean("includeSms", false);
        batchFile.IncludeCalls = selectNode5.GetElementAsBoolean("includeCalls", false);
        batchFile.IncludeTcp = selectNode5.GetElementAsBoolean("includeTcp", false);
        foreach (XmlNode selectNode6 in selectNode5.SelectNodes("dataTypeSettings"))
        {
          try
          {
            DataTypeSettings dataTypeSettings = new DataTypeSettings();
            XmlNode node = selectNode6.SelectSingleNode("type");
            if (node != null)
            {
              dataTypeSettings.IsCategory = false;
              dataTypeSettings.DataType = node.GetAsEnum<DataType>();
            }
            else
            {
              dataTypeSettings.IsCategory = true;
              dataTypeSettings.DataCategory = selectNode6.GetElementAsEnum<DataCategory>("category");
            }
            dataTypeSettings.Format = selectNode6.GetElementAsEnum<LoggingFormat>("format");
            dataTypeSettings.Stylesheet = selectNode6.GetElementAsString("stylesheet", string.Empty);
            batchFile.DataTypeSettingsList.Add(dataTypeSettings);
          }
          catch (GetElementAsException ex)
          {
          }
        }
        this.batchFiles.Add(batchFile);
      }
      foreach (XmlNode selectNode7 in xmlDoc.SelectNodes("/settings/plugin"))
      {
        Plugin plugin = new Plugin();
        plugin.File = selectNode7.GetElementAsString("file", string.Empty);
        plugin.IncludeSbd = selectNode7.GetElementAsBoolean("includeSbd", false);
        plugin.IncludeSms = selectNode7.GetElementAsBoolean("includeSms", false);
        plugin.IncludeCalls = selectNode7.GetElementAsBoolean("includeCalls", false);
        plugin.IncludeTcp = selectNode7.GetElementAsBoolean("includeTcp", false);
        foreach (XmlNode selectNode8 in selectNode7.SelectNodes("dataTypeSettings"))
        {
          try
          {
            DataTypeSettings dataTypeSettings = new DataTypeSettings();
            XmlNode node = selectNode8.SelectSingleNode("type");
            if (node != null)
            {
              dataTypeSettings.IsCategory = false;
              dataTypeSettings.DataType = node.GetAsEnum<DataType>();
            }
            else
            {
              dataTypeSettings.IsCategory = true;
              dataTypeSettings.DataCategory = selectNode8.GetElementAsEnum<DataCategory>("category");
            }
            dataTypeSettings.Format = selectNode8.GetElementAsEnum<LoggingFormat>("format");
            dataTypeSettings.Stylesheet = selectNode8.GetElementAsString("stylesheet", string.Empty);
            plugin.DataTypeSettingsList.Add(dataTypeSettings);
          }
          catch (GetElementAsException ex)
          {
          }
        }
        this.plugins.Add(plugin);
      }
      foreach (XmlNode selectNode9 in xmlDoc.SelectNodes("/settings/dataServer"))
      {
        DataServerInfo dataServerInfo = new DataServerInfo();
        dataServerInfo.Port = selectNode9.GetElementAsInt32("port", 0);
        dataServerInfo.Listen = selectNode9.GetElementAsBoolean("listen", false);
        dataServerInfo.IncludeSbd = selectNode9.GetElementAsBoolean("includeSbd", false);
        dataServerInfo.IncludeSms = selectNode9.GetElementAsBoolean("includeSms", false);
        dataServerInfo.IncludeCalls = selectNode9.GetElementAsBoolean("includeCalls", false);
        dataServerInfo.IncludeTcp = selectNode9.GetElementAsBoolean("includeTcp", false);
        foreach (XmlNode selectNode10 in selectNode9.SelectNodes("dataTypeSettings"))
        {
          try
          {
            DataTypeSettings dataTypeSettings = new DataTypeSettings();
            XmlNode node = selectNode10.SelectSingleNode("type");
            if (node != null)
            {
              dataTypeSettings.IsCategory = false;
              dataTypeSettings.DataType = node.GetAsEnum<DataType>();
            }
            else
            {
              dataTypeSettings.IsCategory = true;
              dataTypeSettings.DataCategory = selectNode10.GetElementAsEnum<DataCategory>("category");
            }
            dataTypeSettings.Format = selectNode10.GetElementAsEnum<LoggingFormat>("format");
            dataTypeSettings.Stylesheet = selectNode10.GetElementAsString("stylesheet", string.Empty);
            dataServerInfo.DataTypeSettingsList.Add(dataTypeSettings);
          }
          catch (GetElementAsException ex)
          {
          }
        }
        this.dataServers.Add(dataServerInfo);
      }
    }

    private void RemoveDuplicateDataTypeSettings(IList<DataTypeSettings> dataTypeSettingsList)
    {
      for (int index1 = 0; index1 < dataTypeSettingsList.Count - 1; ++index1)
      {
        DataTypeSettings dataTypeSettings1 = dataTypeSettingsList[index1];
        int index2 = index1 + 1;
        while (index2 < dataTypeSettingsList.Count)
        {
          DataTypeSettings dataTypeSettings2 = dataTypeSettingsList[index2];
          if (dataTypeSettings1.IsCategory == dataTypeSettings2.IsCategory && (dataTypeSettings1.IsCategory ? (dataTypeSettings1.DataCategory == dataTypeSettings2.DataCategory ? 1 : 0) : (dataTypeSettings1.DataType == dataTypeSettings2.DataType ? 1 : 0)) != 0)
            dataTypeSettingsList.RemoveAt(index2);
          else
            ++index2;
        }
      }
    }

    private string EncryptToHexString(string plainString)
    {
      byte[] bytes1 = Encoding.GetEncoding(1252).GetBytes("u\u0012\u0081_/\u008Fn,=KQß\u00BEBJÔN\u008FÇa|¢\u008DY^À\u008DY,<U\u0005");
      while (plainString.Length % 16 != 0)
        plainString += "\0";
      byte[] bytes2 = Encoding.GetEncoding(1252).GetBytes(plainString);
      AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
      cryptoServiceProvider.Mode = CipherMode.ECB;
      cryptoServiceProvider.Padding = PaddingMode.None;
      ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(bytes1, new byte[16]);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(bytes2, 0, bytes2.Length);
      cryptoStream.FlushFinalBlock();
      byte[] array = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      cryptoServiceProvider.Clear();
      string empty = string.Empty;
      foreach (byte num in array)
        empty += num.ToString("X").PadLeft(2, '0');
      return empty;
    }

    private string DecryptFromHexString(string hexString)
    {
      byte[] bytes = Encoding.GetEncoding(1252).GetBytes("u\u0012\u0081_/\u008Fn,=KQß\u00BEBJÔN\u008FÇa|¢\u008DY^À\u008DY,<U\u0005");
      if (hexString.Length == 0 || hexString.Length % 2 == 1 || hexString.Length % 32 != 0)
        return string.Empty;
      int length = hexString.Length / 2;
      byte[] buffer = new byte[length];
      try
      {
        for (int index = 0; index < length; ++index)
          buffer[index] = byte.Parse(hexString.Substring(index * 2, 2), NumberStyles.HexNumber);
      }
      catch (FormatException ex)
      {
        return string.Empty;
      }
      AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
      cryptoServiceProvider.Mode = CipherMode.ECB;
      cryptoServiceProvider.Padding = PaddingMode.None;
      ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor(bytes, new byte[16]);
      MemoryStream memoryStream = new MemoryStream(buffer);
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] numArray = new byte[buffer.Length];
      cryptoStream.Read(numArray, 0, numArray.Length);
      memoryStream.Close();
      cryptoStream.Close();
      cryptoServiceProvider.Clear();
      string str = Encoding.GetEncoding(1252).GetString(numArray);
      if (str.EndsWith("\0"))
        str = str.Substring(0, str.IndexOf(char.MinValue));
      return str;
    }

    private string FormatStopBits(StopBits value)
    {
      switch (value)
      {
        case StopBits.One:
          return "1";
        case StopBits.Two:
          return "2";
        case StopBits.OnePointFive:
          return "1.5";
        default:
          return string.Empty;
      }
    }
  }
}
