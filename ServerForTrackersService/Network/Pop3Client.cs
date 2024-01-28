// Decompiled with JetBrains decompiler
// Type: Nal.Network.Pop3Client
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Nal.Network
{
  public class Pop3Client
  {
    private bool secureLoginSupported;
    private int count;
    private int size;
    private string uidl;
    private string from;
    private string subject;
    private List<MimeBodyPart> bodyParts = new List<MimeBodyPart>();
    private string userName;
    private string password;
    private string timestamp;
    private Pop3Command currCommand;
    private Pop3ResponseType responseType;
    private bool multiLineResponseExpected;
    private StringBuilder receiveBuffer = new StringBuilder();
    private SocketWrapper socket;

    public Pop3Client()
    {
      this.socket = new SocketWrapper();
      this.socket.ConnectionMade += new EventHandler(this.SocketConnectionMade);
      this.socket.ConnectFailed += new EventHandler<ExceptionEventArgs>(this.SocketConnectFailed);
      this.socket.DataReceived += new EventHandler<DataReceivedEventArgs>(this.SocketDataReceived);
      this.socket.ConnectionClosed += new EventHandler<ExceptionEventArgs>(this.SocketConnectionClosed);
    }

    public event EventHandler ConnectionMade;

    public event EventHandler<ExceptionEventArgs> ConnectFailed;

    public event Pop3ResponseReceivedEventHandler ResponseReceived;

    public event EventHandler ConnectionDropped;

    public bool SecureLoginSupported => this.secureLoginSupported;

    public int Count => this.count;

    public int Size => this.size;

    public string Uidl => this.uidl;

    public string From => this.from;

    public string Subject => this.subject;

    public List<MimeBodyPart> BodyParts => this.bodyParts;

    public void Connect(string host, int port, string userName, string password)
    {
      if (this.Connecting() || this.Connected())
        return;
      this.count = 0;
      this.uidl = string.Empty;
      this.from = string.Empty;
      this.subject = string.Empty;
      this.userName = userName;
      this.password = password;
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Connect;
      this.socket.Connect(host, port);
    }

    public void Disconnect() => this.socket.Disconnect();

    public bool Connecting() => this.socket.Connecting;

    public bool Connected() => this.socket.Connected;

    public void SendApopCommand()
    {
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Apop;
      this.socket.Send(Encoding.ASCII.GetBytes("APOP " + this.userName + " " + this.GetEncryptedPassword() + "\r\n"));
    }

    public void SendUserCommand()
    {
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.User;
      this.socket.Send(Encoding.ASCII.GetBytes("USER " + this.userName + "\r\n"));
    }

    public void SendPassCommand()
    {
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Pass;
      this.socket.Send(Encoding.ASCII.GetBytes("PASS " + this.password + "\r\n"));
    }

    public void SendStatCommand()
    {
      this.count = 0;
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Stat;
      this.socket.Send(Encoding.ASCII.GetBytes("STAT\r\n"));
    }

    public void SendListCommand(int msgNumber)
    {
      this.size = 0;
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.List;
      this.socket.Send(Encoding.ASCII.GetBytes("LIST " + (object) msgNumber + "\r\n"));
    }

    public void SendUidlCommand(int msgNumber)
    {
      this.uidl = string.Empty;
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Uidl;
      this.socket.Send(Encoding.ASCII.GetBytes("UIDL " + (object) msgNumber + "\r\n"));
    }

    public void SendTopCommand(int msgNumber, int lines)
    {
      this.from = string.Empty;
      this.subject = string.Empty;
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = true;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Top;
      this.socket.Send(Encoding.ASCII.GetBytes("TOP " + (object) msgNumber + " " + (object) lines + "\r\n"));
    }

    public void SendRetrCommand(int msgNumber)
    {
      this.bodyParts.Clear();
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = true;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Retr;
      this.socket.Send(Encoding.ASCII.GetBytes("RETR " + (object) msgNumber + "\r\n"));
    }

    public void SendDeleCommand(int msgNumber)
    {
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Dele;
      this.socket.Send(Encoding.ASCII.GetBytes("DELE " + (object) msgNumber + "\r\n"));
    }

    public void SendQuitCommand()
    {
      this.receiveBuffer.Length = 0;
      this.multiLineResponseExpected = false;
      this.responseType = Pop3ResponseType.Invalid;
      this.currCommand = Pop3Command.Quit;
      this.socket.Send(Encoding.ASCII.GetBytes("QUIT\r\n"));
    }

    private void SocketConnectionMade(object sender, EventArgs e)
    {
      if (this.ConnectionMade == null)
        return;
      this.ConnectionMade((object) this, e);
    }

    private void SocketConnectFailed(object sender, ExceptionEventArgs e)
    {
      if (this.ConnectFailed == null)
        return;
      this.ConnectFailed((object) this, e);
    }

    private void SocketConnectionClosed(object sender, EventArgs e)
    {
      if (this.ConnectionDropped == null)
        return;
      this.ConnectionDropped((object) this, e);
    }

    private void SocketDataReceived(object sender, DataReceivedEventArgs e)
    {
      this.receiveBuffer.Append(Encoding.GetEncoding(1252).GetString(e.Data));
      if (this.receiveBuffer.Length < 5)
        return;
      if (this.responseType == Pop3ResponseType.Invalid)
      {
        string str = this.receiveBuffer.ToString(0, 4);
        if (str.StartsWith("+OK"))
          this.responseType = Pop3ResponseType.Ok;
        else if (str.StartsWith("-ERR"))
        {
          this.responseType = Pop3ResponseType.Err;
        }
        else
        {
          if (this.ResponseReceived == null)
            return;
          this.ResponseReceived((object) this, new Pop3ResponseReceivedEventArgs(this.currCommand, this.responseType, string.Empty));
          return;
        }
      }
      string str1 = this.receiveBuffer.ToString(this.receiveBuffer.Length - 5, 5);
      List<string> lines = new List<string>();
      List<string> stringList = new List<string>();
      string empty = string.Empty;
      if (this.multiLineResponseExpected)
      {
        if (str1.EndsWith("\r\n.\r\n"))
        {
          lines.AddRange((IEnumerable<string>) this.receiveBuffer.ToString().Split(new string[1]
          {
            "\r\n"
          }, StringSplitOptions.None));
          empty = lines[0];
          lines.RemoveAt(0);
          lines.RemoveRange(lines.Count - 2, 2);
        }
      }
      else if (str1.EndsWith("\r\n"))
      {
        this.receiveBuffer.Remove(this.receiveBuffer.Length - 2, 2);
        empty = this.receiveBuffer.ToString();
        stringList.AddRange((IEnumerable<string>) empty.Split(new char[1]
        {
          ' '
        }, StringSplitOptions.None));
      }
      if (empty == string.Empty)
        return;
      if (this.responseType == Pop3ResponseType.Ok)
      {
        switch (this.currCommand)
        {
          case Pop3Command.Connect:
            this.timestamp = Regex.Match(this.receiveBuffer.ToString(), "(<)[\\S\\s]*?(>)").Value;
            this.secureLoginSupported = this.timestamp != string.Empty;
            break;
          case Pop3Command.Stat:
            if (stringList.Count < 2)
            {
              this.responseType = Pop3ResponseType.Invalid;
              break;
            }
            try
            {
              this.count = int.Parse(stringList[1]);
              break;
            }
            catch (Exception ex)
            {
              this.responseType = Pop3ResponseType.Invalid;
              break;
            }
          case Pop3Command.List:
            if (stringList.Count < 3)
            {
              this.responseType = Pop3ResponseType.Invalid;
              break;
            }
            try
            {
              this.size = int.Parse(stringList[2]);
              break;
            }
            catch (Exception ex)
            {
              this.responseType = Pop3ResponseType.Invalid;
              break;
            }
          case Pop3Command.Uidl:
            if (stringList.Count < 3)
            {
              this.responseType = Pop3ResponseType.Invalid;
              break;
            }
            this.uidl = stringList[2];
            break;
          case Pop3Command.Top:
            this.ParseHeaders(lines, out this.from, out this.subject);
            break;
          case Pop3Command.Retr:
            this.ParsePart(lines);
            break;
        }
      }
      if (this.ResponseReceived == null)
        return;
      this.ResponseReceived((object) this, new Pop3ResponseReceivedEventArgs(this.currCommand, this.responseType, empty));
    }

    private string GetEncryptedPassword()
    {
      return Encoding.ASCII.GetString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(this.timestamp + this.password)));
    }

    private void ParsePart(List<string> lines)
    {
      string boundary;
      bool isAttachment;
      TransferEncoding transferEncoding;
      int headers = this.ParseHeaders(lines, out string _, out string _, out boundary, out isAttachment, out transferEncoding);
      if (boundary == string.Empty)
      {
        string content = string.Join("\r\n", lines.Skip<string>(headers).ToArray<string>());
        this.bodyParts.Add(new MimeBodyPart(isAttachment, transferEncoding, content));
      }
      else
      {
        List<string> lines1 = new List<string>();
        bool flag = false;
        for (int index = headers; index < lines.Count; ++index)
        {
          if (lines[index].StartsWith("--" + boundary))
          {
            if (!flag)
              flag = true;
            else
              this.ParsePart(lines1);
            lines1.Clear();
          }
          else
          {
            if (lines[index].StartsWith("--" + boundary + "--"))
            {
              this.ParsePart(lines1);
              break;
            }
            lines1.Add(lines[index]);
          }
        }
      }
    }

    private int ParseHeaders(List<string> lines, out string from, out string subject)
    {
      return this.ParseHeaders(lines, out from, out subject, out string _, out bool _, out TransferEncoding _);
    }

    private int ParseHeaders(
      List<string> lines,
      out string from,
      out string subject,
      out string boundary,
      out bool isAttachment,
      out TransferEncoding transferEncoding)
    {
      from = string.Empty;
      subject = string.Empty;
      boundary = string.Empty;
      isAttachment = false;
      transferEncoding = TransferEncoding.Unknown;
      int index;
      for (index = 0; index < lines.Count; ++index)
      {
        if (lines[index] == string.Empty)
        {
          ++index;
          break;
        }
        Match match1 = Regex.Match(lines[index], "^[^ ]+(?=:)");
        if (match1.Success)
        {
          switch (match1.Value)
          {
            case "From":
              from = lines[index].Length < 7 ? string.Empty : lines[index].Substring(6);
              continue;
            case "Subject":
              subject = lines[index].Length < 10 ? string.Empty : lines[index].Substring(9);
              continue;
            case "Content-Type":
              Match match2 = Regex.Match(lines[index], "(?<=boundary=[\\x22]?)[^\\x22]+(?=[\\x22]?)");
              if (match2.Success)
              {
                boundary = match2.Value;
                continue;
              }
              continue;
            case "Content-Transfer-Encoding":
              if (Regex.Match(lines[index], "base64").Success)
              {
                transferEncoding = TransferEncoding.Base64;
                continue;
              }
              if (Regex.Match(lines[index], "quoted-printable").Success)
              {
                transferEncoding = TransferEncoding.QuotedPrintable;
                continue;
              }
              continue;
            case "Content-Disposition":
              if (Regex.Match(lines[index], "attachment").Success)
              {
                isAttachment = true;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      return index;
    }
  }
}
