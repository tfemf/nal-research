// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.EncryptionUser
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;

#nullable disable
namespace Nal.EncryptionModule
{
  public class EncryptionUser
  {
    private Exception lastException;
    private UserRole user;

    public EncryptionUser() => this.user = new UserRole();

    public Exception LastException => this.lastException;

    public Erc RunSelfTests()
    {
      try
      {
        return this.user.RunSelfTests();
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public Erc SetKeysDirectory(string value) => this.user.SetKeysDirectory(value);

    public bool IsLoggedIn() => this.user.IsLoggedIn();

    public Erc Login(string userName, string password)
    {
      Erc erc = this.user.RunSelfTests();
      if (erc != Erc.Success)
        return erc;
      try
      {
        return this.user.Login(userName, password);
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public Erc Logout()
    {
      try
      {
        return this.user.Logout();
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public bool GotoFirstRecord() => this.user.GotoFirstRecord();

    public bool GotoNextRecord() => this.user.GotoNextRecord();

    public bool GotoRecord(string modemInfo, string modemInfoType)
    {
      return this.user.GotoRecord(modemInfo, modemInfoType);
    }

    public string GetCurrentRecordModemInfo() => this.user.GetCurrentRecordModemInfo();

    public string GetCurrentRecordModemInfoType() => this.user.GetCurrentRecordModemInfoType();

    public Erc EncryptWithCurrentRecord(byte[] bytes, ref byte[] encryptedBytes)
    {
      try
      {
        return this.user.EncryptWithCurrentRecord(bytes, ref encryptedBytes);
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public Erc DecryptWithCurrentRecord(byte[] bytes, ref byte[] decryptedBytes)
    {
      try
      {
        return this.user.DecryptWithCurrentRecord(bytes, ref decryptedBytes);
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public Erc Encrypt(
      string modemInfo,
      string modemInfoType,
      byte[] bytes,
      ref byte[] encryptedBytes)
    {
      try
      {
        return this.user.Encrypt(modemInfo, modemInfoType, bytes, ref encryptedBytes);
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public Erc Decrypt(
      string modemInfo,
      string modemInfoType,
      byte[] bytes,
      ref byte[] decryptedBytes)
    {
      try
      {
        return this.user.Decrypt(modemInfo, modemInfoType, bytes, ref decryptedBytes);
      }
      catch (Exception ex)
      {
        this.lastException = ex;
        return Erc.Exception;
      }
    }

    public long EncryptedLength(long length)
    {
      long num = length % 16L;
      return num == 0L ? length : length - num + 16L;
    }
  }
}
