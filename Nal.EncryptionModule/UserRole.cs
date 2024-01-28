// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.UserRole
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Nal.EncryptionModule
{
  internal class UserRole
  {
    private string keysDirectory;
    private bool loggedIn;
    private string userName;
    private string password;
    private List<UserRole.Record> records;
    private List<UserRole.Record>.Enumerator recordEnumerator;

    internal UserRole()
    {
      this.loggedIn = false;
      this.records = new List<UserRole.Record>();
      this.recordEnumerator = this.records.GetEnumerator();
    }

    internal Erc RunSelfTests()
    {
      string str1 = Utils.RunSoftwareIntegritySelfTest();
      string str2 = Utils.RunEncryptionSelfTests(Utils.userFileAesForPasswordKey, Utils.userFileTdesForStaticKey);
      if (str1 == string.Empty && str2 == string.Empty)
        return Erc.Success;
      if (str1 != string.Empty && str2 == string.Empty)
        return Erc.SoftwareIntegrityTestError;
      return str1 == string.Empty && str2 != string.Empty ? Erc.EncryptionTestError : Erc.SoftwareIntegrityEncryptionTestsError;
    }

    internal Erc SetKeysDirectory(string value)
    {
      if (this.loggedIn)
        return Erc.AlreadyLoggedIn;
      this.keysDirectory = value;
      return Erc.Success;
    }

    internal bool IsLoggedIn() => this.loggedIn;

    internal Erc Login(string userName, string password)
    {
      if (this.loggedIn)
        return Erc.AlreadyLoggedIn;
      Erc erc = this.LoadFile(userName, password);
      if (erc != Erc.Success)
        return erc;
      this.loggedIn = true;
      this.userName = userName;
      this.password = password;
      return Erc.Success;
    }

    internal Erc Logout()
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      this.loggedIn = false;
      this.records.Clear();
      return Erc.Success;
    }

    private Erc LoadFile(string userName, string password)
    {
      string path1 = this.keysDirectory + "\\" + userName + ".dat";
      if (!File.Exists(path1))
        return Erc.FileDoesNotExist;
      string path2 = this.keysDirectory + "\\Temp\\" + userName + ".dat";
      string path3;
      if ((File.GetAttributes(path1) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
      {
        path3 = path1;
      }
      else
      {
        if (!File.Exists(path2))
          return Erc.UserFileLockedAndNoTemporary;
        path3 = path2;
      }
      byte[] numArray;
      try
      {
        numArray = File.ReadAllBytes(path3);
      }
      catch
      {
        return Erc.CouldNotReadFile;
      }
      if (numArray.Length % 16 == 1)
        numArray = (byte[]) Utils.ResizeArray((Array) numArray, numArray.Length - 1);
      string fileText;
      Erc erc = Utils.DecryptFileContents(password, Utils.userFilePartialAesKey, numArray, out fileText, Utils.userFileAesForPasswordKey, Utils.userFileAesForStaticKey, Utils.userFileTdesForStaticKey);
      if (erc != Erc.Success)
        return erc;
      string[] strArray1 = fileText.Split('\n');
      if (strArray1.Length < 1)
        return Erc.InvalidPassword;
      if (!strArray1[0].StartsWith("uSeRF:"))
        return Erc.InvalidPassword;
      int int32;
      try
      {
        int32 = Convert.ToInt32(strArray1[0].Substring(6));
      }
      catch (Exception ex)
      {
        return Erc.InvalidPassword;
      }
      for (int index1 = 1; index1 < strArray1.Length; ++index1)
      {
        string[] strArray2 = strArray1[index1].Split('\t');
        if (strArray2.Length == 6)
        {
          string str1 = strArray2[0];
          string str2 = strArray2[1];
          string str3 = strArray2[2];
          string str4 = strArray2[3];
          string str5 = strArray2[4];
          string str6 = strArray2[5];
          if (str2.Length == 1 && str3.Length == 64 && str4.Length == 64 && str5.Length == 1 && str6.Length == 4)
          {
            string str7 = str1;
            int index2 = 0;
            while (index2 < str7.Length && char.IsDigit(str7[index2]))
              ++index2;
            if (!(str2 != "I") || !(str2 != "P"))
            {
              string str8 = str3;
              int index3 = 0;
              while (index3 < str8.Length && char.IsLetterOrDigit(str8[index3]))
                ++index3;
              string str9 = str4;
              int index4 = 0;
              while (index4 < str9.Length && char.IsLetterOrDigit(str9[index4]))
                ++index4;
              if ((!(str5 != "E") || !(str5 != "D") || !(str5 != "B")) && !(Utils.CalculateChecksum(strArray1[index1].Substring(0, strArray1[index1].Length - 4)) != str6))
                this.records.Add(new UserRole.Record()
                {
                  ModemInfo = str1,
                  ModemInfoType = str2,
                  EncryptionKey = str3,
                  DecryptionKey = str4,
                  KeyType = str5
                });
              else
                break;
            }
            else
              break;
          }
          else
            break;
        }
        else
          break;
      }
      if (this.records.Count == int32)
        return Erc.Success;
      this.records.Clear();
      return Erc.InvalidPassword;
    }

    public bool GotoFirstRecord()
    {
      this.recordEnumerator = this.records.GetEnumerator();
      return this.recordEnumerator.MoveNext();
    }

    public bool GotoNextRecord() => this.recordEnumerator.MoveNext();

    public bool GotoRecord(string modemInfo, string modemInfoType)
    {
      this.recordEnumerator = this.records.GetEnumerator();
      while (this.recordEnumerator.MoveNext())
      {
        if (this.recordEnumerator.Current.ModemInfo == modemInfo && this.recordEnumerator.Current.ModemInfoType == modemInfoType)
          return true;
      }
      this.recordEnumerator = this.records.GetEnumerator();
      while (this.recordEnumerator.MoveNext())
      {
        if (this.recordEnumerator.Current.ModemInfo == string.Empty && this.recordEnumerator.Current.ModemInfoType == modemInfoType)
          return true;
      }
      return false;
    }

    public string GetCurrentRecordModemInfo()
    {
      return this.recordEnumerator.Current == null ? string.Empty : this.recordEnumerator.Current.ModemInfo;
    }

    public string GetCurrentRecordModemInfoType()
    {
      return this.recordEnumerator.Current == null ? string.Empty : this.recordEnumerator.Current.ModemInfoType;
    }

    public Erc DecryptWithCurrentRecord(byte[] bytes, ref byte[] decryptedBytes)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      return this.recordEnumerator.Current == null ? Erc.RecordDoesNotExist : this.recordEnumerator.Current.Decrypt(bytes, ref decryptedBytes);
    }

    public Erc EncryptWithCurrentRecord(byte[] bytes, ref byte[] encryptedBytes)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      return this.recordEnumerator.Current == null ? Erc.RecordDoesNotExist : this.recordEnumerator.Current.Encrypt(bytes, ref encryptedBytes);
    }

    public Erc Decrypt(
      string modemInfo,
      string modemInfoType,
      byte[] bytes,
      ref byte[] decryptedBytes)
    {
      return !this.GotoRecord(modemInfo, modemInfoType) ? Erc.RecordDoesNotExist : this.DecryptWithCurrentRecord(bytes, ref decryptedBytes);
    }

    public Erc Encrypt(
      string modemInfo,
      string modemInfoType,
      byte[] bytes,
      ref byte[] encryptedBytes)
    {
      return !this.GotoRecord(modemInfo, modemInfoType) ? Erc.RecordDoesNotExist : this.EncryptWithCurrentRecord(bytes, ref encryptedBytes);
    }

    private class Record
    {
      private string modemInfo;
      private string modemInfoType;
      private Aes encryptCTX;
      private Aes decryptCTX;
      private string keyType;

      public Record()
      {
        this.encryptCTX = new Aes();
        this.decryptCTX = new Aes();
      }

      public string ModemInfo
      {
        get => this.modemInfo;
        set => this.modemInfo = value;
      }

      public string ModemInfoType
      {
        get => this.modemInfoType;
        set => this.modemInfoType = value;
      }

      public string EncryptionKey
      {
        set => this.encryptCTX.SetKey(Utils.HexStringToBytes(value));
      }

      public string DecryptionKey
      {
        set => this.decryptCTX.SetKey(Utils.HexStringToBytes(value));
      }

      public string KeyType
      {
        get => this.keyType;
        set => this.keyType = value;
      }

      public Erc Encrypt(byte[] bytes, ref byte[] encryptedBytes)
      {
        if (this.keyType != "B" && this.keyType != "E")
          return Erc.EncryptionKeyNotSetForModem;
        encryptedBytes = this.encryptCTX.Encrypt(bytes);
        return Erc.Success;
      }

      public Erc Decrypt(byte[] bytes, ref byte[] decryptedBytes)
      {
        decryptedBytes = (byte[]) null;
        if (this.keyType != "B" && this.keyType != "D")
          return Erc.DecryptionKeyNotSetForModem;
        decryptedBytes = this.decryptCTX.Decrypt(bytes);
        return decryptedBytes == null ? Erc.ErrorInDecryption : Erc.Success;
      }
    }
  }
}
