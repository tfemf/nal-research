// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.ErcStr
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

#nullable disable
namespace Nal.EncryptionModule
{
  public static class ErcStr
  {
    public static string GetStringForCode(Erc code)
    {
      switch (code)
      {
        case Erc.Success:
          return string.Empty;
        case Erc.Exception:
          return "Exception";
        case Erc.SoftwareIntegrityTestError:
          return "Failed Software Integrity Test";
        case Erc.EncryptionTestError:
          return "Failed Encryption Test";
        case Erc.SoftwareIntegrityEncryptionTestsError:
          return "Failed Software Integrity and Encryption Tests";
        case Erc.UserFileLockedAndNoTemporary:
          return "User File Locked and No Temporary File";
        case Erc.CryptoOfficerFileLocked:
          return "The Crypto Officer File is Locked";
        case Erc.AlreadyLoggedIn:
          return "Already Logged In";
        case Erc.NotLoggedIn:
          return "Not Logged In";
        case Erc.EncryptionKeyNotSetForModem:
          return "Encrytpion Key Not Set for IMEI/Phone Number";
        case Erc.DecryptionKeyNotFound:
          return "Decryption Key Not Found";
        case Erc.DecryptionKeyNotSetForModem:
          return "Decryption Key Not Set for IMEI/Phone Number";
        case Erc.ErrorInDecryption:
          return "Could Not Decryption";
        case Erc.ModemNotFound:
          return "IMEI/Phone Number Not Found";
        case Erc.RecordDoesNotExist:
          return "The Account Does Not Exist";
        case Erc.RecordAlreadyExists:
          return "The Account Already Exists";
        case Erc.FileDoesNotExist:
          return "User File Does Not Exist";
        case Erc.InvalidEncryptionBlock:
          return "Invalid Encryption Block";
        case Erc.InvalidModem:
          return "Invalid IMEI/Phone Number";
        case Erc.InvalidUser:
          return "Invalid User Name";
        case Erc.InvalidPassword:
          return "Invalid Password";
        case Erc.InvalidEncryptionKey:
          return "Invalid Encryption Key";
        case Erc.InvalidDecryptionKey:
          return "Invalid Decryption Key";
        case Erc.CouldNotSaveAllUserFiles:
        case Erc.CouldNotCreateKeysDirectory:
          return "Could Not Create Keys Directory";
        case Erc.CouldNotCopyUserFile:
          return "Could Not Copy the User File";
        case Erc.CouldNotCreateFile:
          return "Error Cannot Open Crypto Officer File";
        case Erc.CouldNotReadFile:
          return "Could Not Read the File";
        case Erc.CouldNotWriteFile:
          return "Could Not Write the File";
        case Erc.CouldNotLockFile:
          return "Could Not Lock the File";
        case Erc.CouldNotUnlockFile:
          return "Could Not Unlock the File";
        default:
          return string.Empty;
      }
    }
  }
}
