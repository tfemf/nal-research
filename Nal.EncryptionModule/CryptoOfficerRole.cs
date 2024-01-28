// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.CryptoOfficerRole
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Nal.EncryptionModule
{
  internal class CryptoOfficerRole
  {
    private string keysDirectory;
    private List<CryptoOfficerRole.Record> records;
    private List<CryptoOfficerRole.Record> previousRecords;
    private bool loggedIn;
    private string password;
    private string coFilePartialAesKey;
    private Aes coFileAesForPasswordKey;
    private Aes coFileAesForStaticKey;
    private Tdes coFileTdesForStaticKey;

    internal CryptoOfficerRole()
    {
      this.records = new List<CryptoOfficerRole.Record>();
      this.loggedIn = false;
      this.coFilePartialAesKey = "\u008FÊ\u001E\u0099\u009DÃèt\u008FÊ\u001E\u0099\u009DÃèt\u008FÊ\u001E\u0099\u009DÃèt";
      this.coFileAesForPasswordKey = new Aes();
      this.coFileAesForStaticKey = new Aes("þ*ÕRK\u0090£îP\u0018ï\u001EVë]x\u008B,¨'O_Ï\u0083è¢íQ_è\vÚ".Substring(0, 32));
      this.coFileTdesForStaticKey = new Tdes("vR26sn\u0005\u0014", "*+\u007Fb~\u0013ce", "\u001D\u001AO7Nc8\f");
    }

    internal string KeysDirectory
    {
      get => this.keysDirectory;
      set
      {
        if (this.loggedIn)
          throw new Exception("Cannot set the keys directory while logged in.");
        this.keysDirectory = value;
      }
    }

    internal Erc RunSelfTests()
    {
      string str1 = Utils.RunSoftwareIntegritySelfTest();
      string str2 = Utils.RunEncryptionSelfTests(this.coFileAesForPasswordKey, this.coFileTdesForStaticKey);
      if (str1 == string.Empty && str2 == string.Empty)
        return Erc.Success;
      if (str1 != string.Empty && str2 == string.Empty)
        return Erc.SoftwareIntegrityTestError;
      return str1 == string.Empty && str2 != string.Empty ? Erc.EncryptionTestError : Erc.SoftwareIntegrityEncryptionTestsError;
    }

    internal bool IsLoggedIn() => this.loggedIn;

    internal Erc Login(string password)
    {
      if (this.loggedIn)
        return Erc.AlreadyLoggedIn;
      this.password = password;
      Erc erc = this.LoadCoFile();
      if (erc != Erc.Success)
        return erc;
      this.previousRecords = new List<CryptoOfficerRole.Record>((IEnumerable<CryptoOfficerRole.Record>) this.records);
      this.loggedIn = true;
      return Erc.Success;
    }

    internal Erc Logout()
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      this.loggedIn = false;
      this.password = string.Empty;
      this.records.Clear();
      try
      {
        File.SetAttributes(this.KeysDirectory + "\\" + Utils.CryptoOfficerFileName, File.GetAttributes(this.KeysDirectory) | FileAttributes.ReadOnly);
      }
      catch (Exception ex)
      {
        return Erc.CouldNotUnlockFile;
      }
      return Erc.Success;
    }

    internal Erc UnlockCoFile()
    {
      string path = this.KeysDirectory + "\\" + Utils.CryptoOfficerFileName;
      try
      {
        FileAttributes fileAttributes = File.GetAttributes(path) | FileAttributes.ReadOnly;
        File.SetAttributes(path, fileAttributes);
      }
      catch
      {
        return Erc.CouldNotUnlockFile;
      }
      return Erc.Success;
    }

    internal Erc UpdatePassword(string password)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      if (!Utils.ValidPassword(password))
        return Erc.InvalidPassword;
      this.password = password;
      return this.SaveCoFile(this.previousRecords);
    }

    internal List<string> GetUsers()
    {
      if (!this.loggedIn)
        return (List<string>) null;
      List<string> users = new List<string>();
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (!users.Contains(record.user))
          users.Add(record.user);
      }
      return users;
    }

    internal string GetPasswordForUser(string user)
    {
      if (!this.loggedIn)
        return string.Empty;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (user == record.user)
          return record.password;
      }
      return string.Empty;
    }

    internal List<string> GetModemsForUser(string user, string modemInfoType)
    {
      if (!this.loggedIn)
        return (List<string>) null;
      List<string> modemsForUser = new List<string>();
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == user && record.modemInfoType == modemInfoType)
          modemsForUser.Add(record.modemInfo);
      }
      return modemsForUser;
    }

    internal List<string> GetUsersForModem(string modemInfo, string modemInfoType)
    {
      if (!this.loggedIn)
        return (List<string>) null;
      List<string> usersForModem = new List<string>();
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
          usersForModem.Add(record.user);
      }
      return usersForModem;
    }

    internal Erc GetKeysForModem(
      string modemInfo,
      string modemInfoType,
      out string encryptionKey,
      out string decryptionKey,
      out string type)
    {
      encryptionKey = (string) null;
      decryptionKey = (string) null;
      type = string.Empty;
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
        {
          encryptionKey = record.encryptionKey;
          decryptionKey = record.decryptionKey;
          type = record.keyType;
          return Erc.Success;
        }
      }
      return Erc.ModemNotFound;
    }

    internal CryptoOfficerRole.Record GetRecord(
      string user,
      string modemInfo,
      string modemInfoType)
    {
      if (!this.loggedIn)
        return (CryptoOfficerRole.Record) null;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == user && record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
          return new CryptoOfficerRole.Record()
          {
            user = record.user,
            password = record.password,
            modemInfo = record.modemInfo,
            modemInfoType = record.modemInfoType,
            encryptionKey = record.encryptionKey,
            decryptionKey = record.decryptionKey,
            keyType = record.keyType
          };
      }
      return (CryptoOfficerRole.Record) null;
    }

    internal CryptoOfficerRole.Record GetRecordByEncryptionKey(
      string modemInfoType,
      string encryptionKey)
    {
      if (!this.loggedIn)
        return (CryptoOfficerRole.Record) null;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.modemInfoType == modemInfoType && (record.keyType == "E" || record.keyType == "B") && record.encryptionKey == encryptionKey)
          return new CryptoOfficerRole.Record()
          {
            user = record.user,
            password = record.password,
            modemInfo = record.modemInfo,
            modemInfoType = record.modemInfoType,
            encryptionKey = record.encryptionKey,
            decryptionKey = record.decryptionKey,
            keyType = record.keyType
          };
      }
      return (CryptoOfficerRole.Record) null;
    }

    internal CryptoOfficerRole.Record GetRecordByDecryptionKey(
      string modemInfoType,
      string decryptionKey)
    {
      if (!this.loggedIn)
        return (CryptoOfficerRole.Record) null;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.modemInfoType == modemInfoType && (record.keyType == "D" || record.keyType == "B") && record.decryptionKey == decryptionKey)
          return new CryptoOfficerRole.Record()
          {
            user = record.user,
            password = record.password,
            modemInfo = record.modemInfo,
            modemInfoType = record.modemInfoType,
            encryptionKey = record.encryptionKey,
            decryptionKey = record.decryptionKey,
            keyType = record.keyType
          };
      }
      return (CryptoOfficerRole.Record) null;
    }

    internal bool PasswordIsUnique(string password)
    {
      if (!this.loggedIn)
        return false;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.password == password)
          return false;
      }
      return true;
    }

    internal bool ModemExistsForOtherUsers(string user, string modemInfo, string modemInfoType)
    {
      if (!this.loggedIn)
        return false;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user != user && record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
          return true;
      }
      return false;
    }

    internal bool RecordExists(string user, string modemInfo, string modemInfoType)
    {
      if (!this.loggedIn)
        return false;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == user && record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
          return true;
      }
      return false;
    }

    internal Erc AddRecord(
      string user,
      string password,
      string modemInfo,
      string modemInfoType,
      string encryptionKey,
      string decryptionKey,
      string keyType)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      if (!Utils.ValidUserName(user))
        return Erc.InvalidUser;
      if (!Utils.ValidPassword(password))
        return Erc.InvalidPassword;
      if (!Utils.ValidModemInfo(modemInfo))
        return Erc.InvalidModem;
      if ((keyType == "E" || keyType == "B") && !Utils.ValidKey(encryptionKey))
        return Erc.InvalidEncryptionKey;
      if ((keyType == "D" || keyType == "B") && !Utils.ValidKey(decryptionKey))
        return Erc.InvalidDecryptionKey;
      if (this.RecordExists(user, modemInfo, modemInfoType))
        return Erc.RecordAlreadyExists;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == user)
          record.password = password;
        if (record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
        {
          record.encryptionKey = encryptionKey;
          record.decryptionKey = decryptionKey;
          record.keyType = keyType;
        }
      }
      this.records.Add(new CryptoOfficerRole.Record()
      {
        user = user,
        password = password,
        modemInfo = modemInfo,
        modemInfoType = modemInfoType,
        encryptionKey = encryptionKey,
        decryptionKey = decryptionKey,
        keyType = keyType
      });
      return Erc.Success;
    }

    internal Erc DeleteRecord(string user, string modemInfo, string modemInfoType)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      bool flag = false;
      if (!Utils.ValidUserName(user))
        return Erc.InvalidUser;
      if (!Utils.ValidModemInfo(modemInfo))
        return Erc.InvalidModem;
      for (int index = 0; index < this.records.Count; ++index)
      {
        if (this.records[index].user == user && this.records[index].modemInfo == modemInfo && this.records[index].modemInfoType == modemInfoType)
        {
          this.records.RemoveAt(index);
          flag = true;
          break;
        }
      }
      return !flag ? Erc.RecordDoesNotExist : Erc.Success;
    }

    internal Erc UpdatePasswordForUser(string user, string password)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      if (!Utils.ValidPassword(password))
        return Erc.InvalidPassword;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == user)
          record.password = password;
      }
      return Erc.Success;
    }

    internal Erc UpdateRecord(
      string userToUpdate,
      string modemInfoToUpdate,
      string modemInfoTypeToUpdate,
      string user,
      string password,
      string modemInfo,
      string modemInfoType,
      string encryptionKey,
      string decryptionKey,
      string keyType)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      if ((user != userToUpdate || modemInfo != modemInfoToUpdate || modemInfoType != modemInfoTypeToUpdate) && this.RecordExists(user, modemInfo, modemInfoType))
        return Erc.RecordAlreadyExists;
      if (!Utils.ValidUserName(user))
        return Erc.InvalidUser;
      if (!Utils.ValidPassword(password))
        return Erc.InvalidPassword;
      if (!Utils.ValidModemInfo(modemInfo))
        return Erc.InvalidModem;
      if ((keyType == "E" || keyType == "B") && !Utils.ValidKey(encryptionKey))
        return Erc.InvalidEncryptionKey;
      if ((keyType == "D" || keyType == "B") && !Utils.ValidKey(decryptionKey))
        return Erc.InvalidDecryptionKey;
      if (!this.RecordExists(userToUpdate, modemInfoToUpdate, modemInfoTypeToUpdate))
        return Erc.RecordDoesNotExist;
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == userToUpdate && record.modemInfo == modemInfoToUpdate && record.modemInfoType == modemInfoTypeToUpdate)
        {
          record.user = user;
          record.password = password;
          record.modemInfo = modemInfo;
          record.modemInfoType = modemInfoType;
          record.encryptionKey = encryptionKey;
          record.decryptionKey = decryptionKey;
          record.keyType = keyType;
        }
        else
        {
          if (record.user == user)
            record.password = password;
          if (record.modemInfo == modemInfo && record.modemInfoType == modemInfoType)
          {
            record.encryptionKey = encryptionKey;
            record.decryptionKey = decryptionKey;
            record.keyType = keyType;
          }
        }
      }
      return Erc.Success;
    }

    internal void DeleteAllRecords()
    {
      if (!this.loggedIn)
        return;
      this.records.Clear();
    }

    internal Erc ApplyChanges(out string errors)
    {
      errors = string.Empty;
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      Erc erc = this.SaveCoFile(this.records);
      if (erc != Erc.Success)
        return erc;
      this.previousRecords = new List<CryptoOfficerRole.Record>((IEnumerable<CryptoOfficerRole.Record>) this.records);
      List<string> users = this.GetUsers();
      foreach (string str in users)
      {
        string passwordForUser = this.GetPasswordForUser(str);
        Erc code = this.SaveUserFile(str, passwordForUser);
        if (code != Erc.Success)
          errors = errors + "Could not save user file for " + str + ": " + ErcStr.GetStringForCode(code) + "\n";
      }
      if (errors != string.Empty)
        return Erc.CouldNotSaveAllUserFiles;
      string[] files;
      try
      {
        files = Directory.GetFiles(this.KeysDirectory, "*.dat", SearchOption.TopDirectoryOnly);
      }
      catch
      {
        return Erc.Success;
      }
      string[] strArray = new string[2]
      {
        this.KeysDirectory,
        this.KeysDirectory + "\\Temp"
      };
      foreach (string path1 in files)
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(path1);
        if (!Utils.CryptoOfficerFileName.StartsWith(withoutExtension) && !users.Contains(withoutExtension))
        {
          foreach (string str in strArray)
          {
            string path2 = str + "\\" + withoutExtension + ".dat";
            try
            {
              FileAttributes attributes = File.GetAttributes(path2);
              if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                File.SetAttributes(path2, attributes & ~FileAttributes.ReadOnly);
              File.Delete(path2);
            }
            catch
            {
            }
          }
        }
      }
      return Erc.Success;
    }

    internal void CancelChanges()
    {
      if (!this.loggedIn)
        return;
      this.records = new List<CryptoOfficerRole.Record>((IEnumerable<CryptoOfficerRole.Record>) this.previousRecords);
    }

    private Erc LoadCoFile()
    {
      if (!Directory.Exists(this.KeysDirectory))
      {
        try
        {
          Directory.CreateDirectory(this.KeysDirectory);
        }
        catch
        {
          return Erc.CouldNotCreateKeysDirectory;
        }
      }
      string path = this.KeysDirectory + "\\" + Utils.CryptoOfficerFileName;
      if (!File.Exists(path))
        return this.SaveCoFile(this.records);
      FileAttributes attributes = File.GetAttributes(path);
      if ((attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
        return Erc.CryptoOfficerFileLocked;
      File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
      byte[] numArray;
      try
      {
        numArray = File.ReadAllBytes(path);
      }
      catch
      {
        return Erc.CouldNotReadFile;
      }
      if (numArray.Length % 16 == 1)
        numArray = (byte[]) Utils.ResizeArray((Array) numArray, numArray.Length - 1);
      string fileText;
      Erc erc = Utils.DecryptFileContents(this.password, this.coFilePartialAesKey, numArray, out fileText, this.coFileAesForPasswordKey, this.coFileAesForStaticKey, this.coFileTdesForStaticKey);
      if (erc != Erc.Success)
        return erc;
      string[] strArray1 = fileText.Split('\n');
      if (strArray1.Length < 1)
        return Erc.InvalidPassword;
      if (!strArray1[0].StartsWith("cOmF:"))
        return Erc.InvalidPassword;
      int int32;
      try
      {
        int32 = Convert.ToInt32(strArray1[0].Substring(5));
      }
      catch (Exception ex)
      {
        return Erc.InvalidPassword;
      }
      for (int index1 = 1; index1 < strArray1.Length; ++index1)
      {
        string[] strArray2 = strArray1[index1].Split('\t');
        if (strArray2.Length == 8)
        {
          CryptoOfficerRole.Record record = new CryptoOfficerRole.Record();
          record.user = strArray2[0];
          record.password = strArray2[1];
          record.modemInfo = strArray2[2];
          record.modemInfoType = strArray2[3];
          record.encryptionKey = strArray2[4];
          record.decryptionKey = strArray2[5];
          record.keyType = strArray2[6];
          string str = strArray2[7];
          if (record.user.Length <= 30 && record.password.Length <= 16 && record.modemInfoType.Length == 1 && record.encryptionKey.Length == 64 && record.encryptionKey.Length == 64 && record.keyType.Length == 1 && str.Length == 4)
          {
            string modemInfo = record.modemInfo;
            int index2 = 0;
            while (index2 < modemInfo.Length && char.IsDigit(modemInfo[index2]))
              ++index2;
            if (!(record.modemInfoType != "I") || !(record.modemInfoType != "P"))
            {
              string encryptionKey = record.encryptionKey;
              int index3 = 0;
              while (index3 < encryptionKey.Length && char.IsLetterOrDigit(encryptionKey[index3]))
                ++index3;
              string decryptionKey = record.decryptionKey;
              int index4 = 0;
              while (index4 < decryptionKey.Length && char.IsLetterOrDigit(decryptionKey[index4]))
                ++index4;
              if ((!(record.keyType != "E") || !(record.keyType != "D") || !(record.keyType != "B")) && !(Utils.CalculateChecksum(strArray1[index1].Substring(0, strArray1[index1].Length - 4)) != str))
                this.records.Add(record);
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

    private Erc SaveCoFile(List<CryptoOfficerRole.Record> records)
    {
      string fileText = "cOmF:" + records.Count<CryptoOfficerRole.Record>().ToString() + "\n";
      foreach (CryptoOfficerRole.Record record in records)
      {
        string s = record.user + "\t" + record.password + "\t" + record.modemInfo + "\t" + record.modemInfoType + "\t" + record.encryptionKey + "\t" + record.decryptionKey + "\t" + record.keyType + "\t";
        string str = s + Utils.CalculateChecksum(s);
        fileText = fileText + str + "\n";
      }
      byte[] fileBytes;
      if (Utils.EncryptFileContents(this.password, this.coFilePartialAesKey, fileText, out fileBytes, this.coFileAesForPasswordKey, this.coFileAesForStaticKey, this.coFileTdesForStaticKey) != Erc.Success)
        return Erc.InvalidPassword;
      if (!Directory.Exists(this.KeysDirectory))
      {
        try
        {
          Directory.CreateDirectory(this.KeysDirectory);
        }
        catch
        {
          return Erc.CouldNotCreateKeysDirectory;
        }
      }
      string path = this.KeysDirectory + "\\" + Utils.CryptoOfficerFileName;
      if (!File.Exists(path))
      {
        try
        {
          File.Create(path).Close();
        }
        catch
        {
          return Erc.CouldNotCreateFile;
        }
      }
      try
      {
        FileAttributes attributes = File.GetAttributes(path);
        File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
      }
      catch
      {
        return Erc.CouldNotLockFile;
      }
      try
      {
        File.WriteAllBytes(path, fileBytes);
      }
      catch
      {
        return Erc.CouldNotWriteFile;
      }
      try
      {
        FileAttributes attributes = File.GetAttributes(path);
        File.SetAttributes(path, attributes | FileAttributes.ReadOnly);
      }
      catch
      {
        return Erc.CouldNotUnlockFile;
      }
      return Erc.Success;
    }

    private Erc SaveUserFile(string userName, string userFilePassword)
    {
      if (!this.loggedIn)
        return Erc.NotLoggedIn;
      List<CryptoOfficerRole.Record> recordList = new List<CryptoOfficerRole.Record>();
      foreach (CryptoOfficerRole.Record record in this.records)
      {
        if (record.user == userName)
          recordList.Add(record);
      }
      string fileText = "uSeRF:" + recordList.Count.ToString() + "\n";
      foreach (CryptoOfficerRole.Record record in recordList)
      {
        string s = record.modemInfo + "\t" + record.modemInfoType + "\t" + record.encryptionKey + "\t" + record.decryptionKey + "\t" + record.keyType + "\t";
        string str = s + Utils.CalculateChecksum(s);
        fileText = fileText + str + "\n";
      }
      byte[] fileBytes;
      Erc erc = Utils.EncryptFileContents(userFilePassword, Utils.userFilePartialAesKey, fileText, out fileBytes, Utils.userFileAesForPasswordKey, Utils.userFileAesForStaticKey, Utils.userFileTdesForStaticKey);
      if (erc != Erc.Success)
        return erc;
      if (!Directory.Exists(this.KeysDirectory))
      {
        try
        {
          Directory.CreateDirectory(this.KeysDirectory);
        }
        catch
        {
          return Erc.CouldNotCreateKeysDirectory;
        }
      }
      string str1 = this.KeysDirectory + "\\" + userName + ".dat";
      if (File.Exists(str1))
      {
        string path = this.KeysDirectory + "\\Temp";
        if (!Directory.Exists(path))
        {
          try
          {
            Directory.CreateDirectory(path);
          }
          catch
          {
            return Erc.CouldNotCopyUserFile;
          }
        }
        string str2 = path + "\\" + userName + ".dat";
        if (File.Exists(str2))
        {
          FileAttributes attributes = File.GetAttributes(str2);
          if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
          {
            try
            {
              File.SetAttributes(str2, attributes & ~FileAttributes.ReadOnly);
            }
            catch
            {
              return Erc.CouldNotCopyUserFile;
            }
          }
        }
        try
        {
          File.Copy(str1, str2, true);
        }
        catch
        {
          return Erc.CouldNotCopyUserFile;
        }
        try
        {
          FileAttributes attributes = File.GetAttributes(str1);
          File.SetAttributes(str1, attributes & ~FileAttributes.ReadOnly);
        }
        catch
        {
          return Erc.CouldNotLockFile;
        }
      }
      try
      {
        File.WriteAllBytes(str1, fileBytes);
      }
      catch
      {
        return Erc.CouldNotWriteFile;
      }
      try
      {
        FileAttributes attributes = File.GetAttributes(str1);
        File.SetAttributes(str1, attributes | FileAttributes.ReadOnly);
      }
      catch
      {
        return Erc.CouldNotUnlockFile;
      }
      return Erc.Success;
    }

    internal class Record
    {
      public string user;
      public string password;
      public string modemInfo;
      public string modemInfoType;
      public string encryptionKey;
      public string decryptionKey;
      public string keyType;
    }
  }
}
