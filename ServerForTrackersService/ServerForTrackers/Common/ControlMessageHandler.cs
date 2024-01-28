// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.ControlMessageHandler
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using Nal.Network;
using System;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class ControlMessageHandler
  {
    private SocketWrapper socket;
    private string dataBuffer = string.Empty;

    public ControlMessageHandler(SocketWrapper socket) => this.socket = socket;

    public event AddMessageReceivedEventHandler AddMessageReceived;

    public event InfMessageReceivedEventHandler InfMessageReceived;

    public event SimpleMessageReceivedEventHandler StaMessageReceived;

    public event SimpleMessageReceivedEventHandler NamMessageReceived;

    public event ActMessageReceivedEventHandler ActMessageReceived;

    public event RemMessageReceivedEventHandler RemMessageReceived;

    public event SndMessageReceivedEventHandler SndMessageReceived;

    public event EventHandler QryMessageReceived;

    public event EventHandler AboMessageReceived;

    public event SimpleMessageReceivedEventHandler CmdMessageReceived;

    public event ConMessageReceivedEventHandler ConMessageReceived;

    public event StringEventHandler PduMessageReceived;

    public void ProcessIncommingData(byte[] data)
    {
      this.dataBuffer += Encoding.GetEncoding(1252).GetString(data);
      while (true)
      {
        int startIndex = this.dataBuffer.IndexOf("<m>");
        if (startIndex != -1)
        {
          int num = this.dataBuffer.IndexOf("</m>");
          if (num != -1)
          {
            int count = num + 4;
            string message = this.dataBuffer.Substring(startIndex, count - startIndex);
            this.dataBuffer = this.dataBuffer.Remove(0, count);
            try
            {
              this.ProcessMessage(message);
            }
            catch (XmlException ex)
            {
            }
          }
          else
            goto label_7;
        }
        else
          break;
      }
      return;
label_7:;
    }

    public void SendAddMessage(
      string id,
      string type,
      string name,
      string status,
      string activity)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("add");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("t");
      xmlWriter.WriteString(type);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("n");
      xmlWriter.WriteString(name);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("s");
      xmlWriter.WriteString(status);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("a");
      xmlWriter.WriteString(activity);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendInfMessage(string info)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("inf");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("v");
      xmlWriter.WriteString(info);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendStaMessage(string id, string status)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("sta");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("v");
      xmlWriter.WriteString(status);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendActMessage(string id, bool append, string activity)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("act");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("a");
      xmlWriter.WriteString(append ? "1" : "0");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("v");
      xmlWriter.WriteString(activity);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendNamMessage(string id, string name)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("nam");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("v");
      xmlWriter.WriteString(name);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendRemMessage(string id)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("rem");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendSndMessage(
      string id,
      string protocol,
      string remoteModemInfo,
      byte[] data,
      bool sendBackUpdate)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("snd");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("p");
      xmlWriter.WriteString(protocol);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("r");
      xmlWriter.WriteString(remoteModemInfo);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("d");
      xmlWriter.WriteBase64(data, 0, data.Length);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("s");
      xmlWriter.WriteString(sendBackUpdate ? "1" : "0");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendQryMessage()
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("qry");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendAboMessage()
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("abo");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendCmdMessage(string id, string command)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("cmd");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("v");
      xmlWriter.WriteString(command);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendConMessage(string id, string protocol, string remoteModemInfo)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString("con");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("i");
      xmlWriter.WriteString(id);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("p");
      xmlWriter.WriteString(protocol);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("r");
      xmlWriter.WriteString(remoteModemInfo);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    public void SendPduMessage(string pdu)
    {
      StringBuilder stringBuilder = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings()
      {
        CheckCharacters = false,
        OmitXmlDeclaration = true
      });
      xmlWriter.WriteStartElement("m");
      xmlWriter.WriteStartElement("x");
      xmlWriter.WriteString(nameof (pdu));
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("p");
      xmlWriter.WriteString(pdu);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      xmlWriter.Close();
      this.SendMessage(stringBuilder);
    }

    private void ProcessMessage(string message)
    {
      XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(message), new XmlReaderSettings()
      {
        CheckCharacters = false
      });
      xmlReader.ReadStartElement("m");
      switch (xmlReader.ReadElementString("x"))
      {
        case "add":
          AddMessageReceivedEventArgs e1 = new AddMessageReceivedEventArgs();
          e1.id = xmlReader.ReadElementString("i");
          e1.type = xmlReader.ReadElementString("t");
          e1.name = xmlReader.ReadElementString("n");
          e1.status = xmlReader.ReadElementString("s");
          e1.activity = xmlReader.ReadElementString("a");
          if (this.AddMessageReceived == null)
            break;
          this.AddMessageReceived((object) this, e1);
          break;
        case "inf":
          InfMessageReceivedEventArgs e2 = new InfMessageReceivedEventArgs();
          e2.info = xmlReader.ReadElementString("v");
          if (this.InfMessageReceived == null)
            break;
          this.InfMessageReceived((object) this, e2);
          break;
        case "act":
          ActMessageReceivedEventArgs e3 = new ActMessageReceivedEventArgs();
          e3.id = xmlReader.ReadElementString("i");
          e3.append = xmlReader.ReadElementString("a") == "1";
          e3.activity = xmlReader.ReadElementString("v");
          if (this.ActMessageReceived == null)
            break;
          this.ActMessageReceived((object) this, e3);
          break;
        case "sta":
          SimpleMessageReceivedEventArgs e4 = new SimpleMessageReceivedEventArgs();
          e4.id = xmlReader.ReadElementString("i");
          e4.value = xmlReader.ReadElementString("v");
          if (this.StaMessageReceived == null)
            break;
          this.StaMessageReceived((object) this, e4);
          break;
        case "nam":
          SimpleMessageReceivedEventArgs e5 = new SimpleMessageReceivedEventArgs();
          e5.id = xmlReader.ReadElementString("i");
          e5.value = xmlReader.ReadElementString("v");
          if (this.NamMessageReceived == null)
            break;
          this.NamMessageReceived((object) this, e5);
          break;
        case "rem":
          RemMessageReceivedEventArgs e6 = new RemMessageReceivedEventArgs();
          e6.id = xmlReader.ReadElementString("i");
          if (this.RemMessageReceived == null)
            break;
          this.RemMessageReceived((object) this, e6);
          break;
        case "snd":
          SndMessageReceivedEventArgs e7 = new SndMessageReceivedEventArgs();
          e7.id = xmlReader.ReadElementString("i");
          e7.protocol = xmlReader.ReadElementString("p");
          e7.remoteModemInfo = xmlReader.ReadElementString("r");
          e7.data = Convert.FromBase64String(xmlReader.ReadElementString("d"));
          e7.sendBackUpdate = xmlReader.ReadElementString("s") == "1";
          if (this.SndMessageReceived == null)
            break;
          this.SndMessageReceived((object) this, e7);
          break;
        case "con":
          ConMessageReceivedEventArgs e8 = new ConMessageReceivedEventArgs();
          e8.id = xmlReader.ReadElementString("i");
          e8.protocol = xmlReader.ReadElementString("p");
          e8.remoteModemInfo = xmlReader.ReadElementString("r");
          if (this.ConMessageReceived == null)
            break;
          this.ConMessageReceived((object) this, e8);
          break;
        case "qry":
          if (this.QryMessageReceived == null)
            break;
          this.QryMessageReceived((object) this, EventArgs.Empty);
          break;
        case "abo":
          if (this.AboMessageReceived == null)
            break;
          this.AboMessageReceived((object) this, EventArgs.Empty);
          break;
        case "cmd":
          SimpleMessageReceivedEventArgs e9 = new SimpleMessageReceivedEventArgs();
          e9.id = xmlReader.ReadElementString("i");
          e9.value = xmlReader.ReadElementString("v");
          if (this.CmdMessageReceived == null)
            break;
          this.CmdMessageReceived((object) this, e9);
          break;
        case "pdu":
          StringEventArgs e10 = new StringEventArgs();
          e10.str = xmlReader.ReadElementString("p");
          if (this.PduMessageReceived == null)
            break;
          this.PduMessageReceived((object) this, e10);
          break;
      }
    }

    private void SendMessage(StringBuilder message)
    {
      if (!this.socket.Connected)
        return;
      byte[] bytes = Encoding.GetEncoding(1252).GetBytes(message.ToString());
      this.socket.Send(bytes, 0, bytes.Length);
    }
  }
}
